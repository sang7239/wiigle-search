using HtmlAgilityPack;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Back_End
{
    class WebCrawler
    {
        private HashSet<string> visitedXml;
        private HashSet<string> visitedHtml;
        private HashSet<string> visitedError;
        public static CloudQueue xmlQueue;
        public static CloudQueue htmlQueue;
        public static CloudQueue errorQueue;
        public static CloudQueue signalQueue;
        public static CloudTable urlTable;
        public static CloudTable errorTable;
        public static WebClient client;
        private static HashSet<string> badExtensions;
        private static HashSet<string> filterWords;
        private static readonly List<string> domains = new List<string>() { "cnn.com", "bleacherreport.com" };
        public WebCrawler()
        {
            // queues
            xmlQueue = QueueManager.GetXmlQueue();
            htmlQueue = QueueManager.GetHtmlQueue();
            errorQueue = QueueManager.GetErrorQueue();
            signalQueue = QueueManager.GetSignalQueue();
            // tables
            urlTable = TableManager.GetUrlTable();
            errorTable = TableManager.GetErrorTable();
            //web client
            client = new WebClient();
            // cache
            visitedXml = new HashSet<string>();
            visitedHtml = new HashSet<string>();
            visitedError = new HashSet<string>();

            badExtensions = Dictionary.GetBadExtensions();
            filterWords = Dictionary.GetfilterWords();
        }

        public void ParseRobots(string[] robots)
        {
            foreach (string robot in robots)
            {
                ParseRobot(robot);
            }
        }

        private void ParseRobot(string robot)
        {
            List<string> res = new List<string>();
            WebClient client = new WebClient();
            string content = client.DownloadString(robot);
            string[] seperator = { "\n" };
            string[] lines = content.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (line.StartsWith("Sitemap:"))
                {
                    string site = line.Substring(9);
                    xmlQueue.AddMessage(GenerateMessage(site));
                    visitedXml.Add(site);
                }
            }
        }

        public void ParseXML()
        {
            CloudQueueMessage message = xmlQueue.GetMessage();
            try
            {
                string url = message.AsString;
                string content = client.DownloadString(url);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);
                XmlNodeList nodes = doc.GetElementsByTagName("loc");
                foreach (XmlNode node in nodes)
                {
                    string site = node.InnerText;
                    if (site.EndsWith(".xml") && !visitedXml.Contains(site))
                    {
                        xmlQueue.AddMessage(GenerateMessage(site));
                        visitedXml.Add(site);
                    }
                    else if (IsCrawlable(site) && !visitedHtml.Contains(site))
                    {
                        //add to html queue 
                        htmlQueue.AddMessage(GenerateMessage(site));
                        visitedHtml.Add(site);
                    } else // invalid urls
                    {
                        if (!visitedHtml.Contains(site) && !visitedError.Contains(site))
                        {
                            errorQueue.AddMessage(GenerateMessage("url: " + url + " error: invalid url"));
                            visitedError.Add(site);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }
        }

        private bool IsCrawlable(string site)
        {
            foreach (string domain in domains)
            {
                string[] url = domain.Split('.');
                string name = url[0];
                string extension = url[1];
                string pattern = @"(^|^[^:]+:\/\/|[^\.]+\.)" + name + "\\." + extension;
                Match m = Regex.Match(site, pattern);
                if (m.Success)
                {
                    return !badExtensions.Contains(site.Substring(site.Length - 4));
                }
            }
            return false;
        }


        public void ParseHtml()
        {
            CloudQueueMessage message = htmlQueue.GetMessage();
            string url = message.AsString;
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(url);
                InsertPage(htmlDoc, url);
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//a");
                foreach (HtmlNode node in nodes)
                {
                    string site = node.Attributes["href"].Value;
                    if (!site.StartsWith("https://") && !site.StartsWith("http://") && !site.StartsWith("//"))
                    {
                        site = "https://www.cnn.com" + site;
                    }
                    if (IsCrawlable(site) && !visitedHtml.Contains(site))
                    {
                        htmlQueue.AddMessage(GenerateMessage(site));
                        visitedHtml.Add(site);
                    } else // invalid urls
                    {
                        if (!visitedHtml.Contains(site) && !visitedError.Contains(site))
                        {
                            errorQueue.AddMessage(GenerateErrorMessage(site, "invalid url"));
                            visitedError.Add(site);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (!visitedError.Contains(url))
                {
                    errorQueue.AddMessage(GenerateErrorMessage(url, e.Message));
                    visitedError.Add(url);
                }
            }
        }

        private void InsertPage(HtmlDocument page, string url)
        {
            try
            {
                //if page is an article
                string title = page.DocumentNode.SelectSingleNode("//title").InnerText;
                string[] keywords = title.Split(' ');
                foreach (string word in keywords)
                {
                    string keyword = FilterWord(word);
                    if (!keyword.Equals("") && !filterWords.Contains(keyword))
                    {
                        try
                        {
                            Entity entity = new Entity(keyword, title, url, DateTime.Now);
                            TableOperation insert = TableOperation.Insert(entity);
                            TableResult result = urlTable.Execute(insert);
                        }
                        catch (StorageException e)
                        {
                            if (!visitedError.Contains(url))
                            {
                                errorQueue.AddMessage(GenerateErrorMessage(url, e.Message));
                                visitedError.Add(url);
                            }
                        }
                    }
                }
            }
            catch(StorageException e)
            {
                if (!visitedError.Contains(url))
                {
                    errorQueue.AddMessage(GenerateErrorMessage(url, e.Message));
                    visitedError.Add(url);
                }
            }
        }

        public void InsertError()
        {
            CloudQueueMessage message = errorQueue.GetMessage();
            try
            {
                string[] seperators = { "url: ", "error: " };
                string[] error = message.AsString.Split(seperators, 2,
                       StringSplitOptions.RemoveEmptyEntries);
                ErrorEntity entity = new ErrorEntity(Encoder.EncodeUrl(error[0]), error[1]);
                TableOperation insert = TableOperation.Insert(entity);
                TableResult result = errorTable.Execute(insert);
            } catch (StorageException e)
            {
                Debug.Print(e.Message);
            }
        }

        private static string FilterWord(string word)
        {
            string filtered = "";
            foreach (char c in word)
            {
                if (Char.IsLetter(c) || Char.IsDigit(c))
                {
                    filtered += c;
                }
            }
            return filtered.ToLower();
        }

        private CloudQueueMessage GenerateMessage(string message)
        {
            CloudQueueMessage queueMessage = new CloudQueueMessage(message);
            return queueMessage;
        }

        private CloudQueueMessage GenerateErrorMessage(string url, string message)
        {
            CloudQueueMessage queueMessage = new CloudQueueMessage("url: " + url + " error: " + message);
            return queueMessage;
        }
    }
}