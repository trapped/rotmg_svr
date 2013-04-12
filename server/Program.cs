using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace server
{
    class Program
    {
        static HttpListener listener;
        static Thread listen;
        static readonly Thread[] workers = new Thread[5];
        static readonly Queue<HttpListenerContext> contextQueue = new Queue<HttpListenerContext>();
        static readonly object queueLock = new object();
        static readonly ManualResetEvent queueReady = new ManualResetEvent(false);

        const int port = 80;

        static void Main(string[] args)
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + port + "/");
            listener.Start();

            listen = new Thread(ListenerCallback);
            listen.Start();
            for (var i = 0; i < workers.Length; i++)
            {
                workers[i] = new Thread(Worker);
                workers[i].Start();
            }
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Terminating...");
                listener.Stop();
                while (contextQueue.Count > 0)
                    Thread.Sleep(100);
                Environment.Exit(0);
            };
            Console.WriteLine("Listening at port " + port + "...");

            Thread.CurrentThread.Join();
        }

        static void ListenerCallback()
        {
            try
            {
                do
                {
                    var context = listener.GetContext();
                    lock (queueLock)
                    {
                        contextQueue.Enqueue(context);
                        queueReady.Set();
                    }
                } while (true);
            }
            catch { }
        }

        static void Worker()
        {
            while (queueReady.WaitOne())
            {
                HttpListenerContext context;
                lock (queueLock)
                {
                    if (contextQueue.Count > 0)
                        context = contextQueue.Dequeue();
                    else
                    {
                        queueReady.Reset();
                        continue;
                    }
                }

                try
                {
                    ProcessRequest(context);
                }
                catch { }
            }
        }

        static void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                IRequestHandler handler;

                if (!RequestHandlers.Handlers.TryGetValue(context.Request.Url.LocalPath, out handler))
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad request";
                    using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                        wtr.Write("<h1>Bad request</h1>");
                }
                else
                    handler.HandleRequest(context);
            }
            catch (Exception e)
            {
                using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                    wtr.Write("<Error>Internal Server Error</Error>");
                Console.Error.WriteLine(e);
            }

            context.Response.Close();
        }
    }
}
