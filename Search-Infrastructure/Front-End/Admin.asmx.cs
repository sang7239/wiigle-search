using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Back_End;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

namespace Front_End
{
    /// <summary>
    /// Summary description for Admin
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Admin : System.Web.Services.WebService
    {
        private static CloudQueue signalQueue = QueueManager.GetSignalQueue();
        private static CloudQueue xmlQueue = QueueManager.GetXmlQueue();
        private static CloudQueue htmlQueue = QueueManager.GetHtmlQueue();
        private static CloudQueue errorQueue = QueueManager.GetErrorQueue();
        private static CloudTable urlTable = TableManager.GetUrlTable();
        private static CloudTable errorTable = TableManager.GetErrorTable();
        [WebMethod]
        public string StartCrawl()
        {
            signalQueue.Clear();
            signalQueue.AddMessage(new CloudQueueMessage("crawl"));
            return "crawling";
        }
        [WebMethod]
        public string PauseCrawl()
        {
            signalQueue.Clear();
            signalQueue.AddMessage(new CloudQueueMessage("idle"));
            return "paused";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RetrieveSearchResults(string searchWord)
        {
            searchWord = searchWord.Trim().ToLower();
            var keywords = searchWord.Split().Select(x => Encoder.EncodeUrl(x));
            List<Tuple<string, string>> results = new List<Tuple<string, string>>();
            foreach (string keyword in keywords)
            {
                TableQuery<Entity> rangeQuery = new TableQuery<Entity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, keyword));
                var result = urlTable.ExecuteQuery(rangeQuery).Select(x => new Tuple<string, string>( x.title, x.url ));
                results.AddRange(result);
            }
            return new JavaScriptSerializer().Serialize(results);
        } 
    }
}
