using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back_End
{
    public static class QueueManager
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("myConnectionString"));

        public static CloudQueue GetXmlQueue()
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue xmlQueue = queueClient.GetQueueReference("xmlqueue");
            xmlQueue.CreateIfNotExists();
            return xmlQueue;
        }

        public static CloudQueue GetHtmlQueue()
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue htmlQueue = queueClient.GetQueueReference("htmlqueue");
            htmlQueue.CreateIfNotExists();
            return htmlQueue;
        }

        public static CloudQueue GetErrorQueue()
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue errorQueue = queueClient.GetQueueReference("errorqueue");
            errorQueue.CreateIfNotExists();
            return errorQueue;
        }

        public static CloudQueue GetSignalQueue()
        {
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue signalQueue = queueClient.GetQueueReference("signalqueue");
            signalQueue.CreateIfNotExists();
            return signalQueue;
        }
    }
}
