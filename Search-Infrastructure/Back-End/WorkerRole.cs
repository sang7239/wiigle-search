using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Back_End
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private static CloudQueue xmlQueue = QueueManager.GetXmlQueue();
        private static CloudQueue htmlQueue = QueueManager.GetHtmlQueue();
        private static CloudQueue errorQueue = QueueManager.GetErrorQueue();
        private static CloudQueue signalQueue = QueueManager.GetSignalQueue();
        public override void Run()
        {
            Trace.TraceInformation("Back-End is running");
            string[] robots = { "https://www.cnn.com/robots.txt" };
            WebCrawler crawler = new WebCrawler();
            crawler.ParseRobots(robots);
            // status idle, crawl
            string status = "idle";
            while (status.Equals("idle") || status.Equals("crawl"))
            {
                if (signalQueue.PeekMessage() != null)
                {
                    status = signalQueue.GetMessage().AsString;
                }
                if (status.Equals("crawl"))
                {
                    if (xmlQueue.PeekMessage() != null)
                    {
                        crawler.ParseXML();
                    }
                    if (htmlQueue.PeekMessage() != null)
                    {
                        crawler.ParseHtml();
                    }
                    if (errorQueue.PeekMessage() != null)
                    {
                        crawler.InsertError();
                    }
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("Back-End has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Back-End is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("Back-End has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}