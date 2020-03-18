using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace Front_End
{
    /// <summary>
    /// Summary description for QuerySuggestionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class QuerySuggestionService : System.Web.Services.WebService
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("myConnectionString"));
        private static CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
        private static CloudBlobContainer container = serviceClient.GetContainerReference("blob");
        private static CloudBlockBlob blob = container.GetBlockBlobReference("filtered_words.txt");
        private static Trie trie;
        [WebMethod]
        public string BuildTrie()
        {
            if (blob.Exists())
            {
                trie = new Trie();
                using (StreamReader sr = new StreamReader(blob.OpenRead()))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        trie.AddTitle(line);
                    }
                }
                return "Trie Built!";
            }
            else
            {
                return "Trie can't be built because blob does not exist";
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetWordsWithPrefix(string prefix)
        {
            List<string> res = trie.GetWordsWithPrefix(prefix);
            if (res.Count > 10)
            {
                res = trie.GetWordsWithPrefix(prefix).GetRange(0, 10);
            }
            return new JavaScriptSerializer().Serialize(res);
        }
    }
}
