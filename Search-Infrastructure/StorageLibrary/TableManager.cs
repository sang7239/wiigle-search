using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back_End
{
    class TableManager
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("myConnectionString"));

        public static CloudTable GetUrlTable()
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable urlTable = tableClient.GetTableReference("urltable");
            urlTable.CreateIfNotExists();
            return urlTable;
        }

        public static CloudTable GetErrorTable()
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable errorTable = tableClient.GetTableReference("errortable");
            errorTable.CreateIfNotExists();
            return errorTable;
        }
    }
}
