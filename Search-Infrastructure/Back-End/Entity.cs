using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Back_End
{
    public class Entity : TableEntity
    {
        public Entity() { }
        public Entity(string keyword, string title, string url, DateTime time)
        {
            this.PartitionKey = Encoder.EncodeUrl(keyword);
            this.RowKey = Encoder.EncodeUrl(title);
            this.title = title;
            this.url = url;
            this.time = time;
        }

        public string url { get; set; }
        public DateTime time { get; set; }
        public string title { get; set; }
    }
}
