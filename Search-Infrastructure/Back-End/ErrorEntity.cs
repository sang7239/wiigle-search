using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back_End
{
    class ErrorEntity : TableEntity
    {
        public ErrorEntity() { }
        public ErrorEntity(string url, string message)
        {
            this.PartitionKey = Encoder.EncodeUrl(url);
            this.RowKey = message;
        }
    }
}
