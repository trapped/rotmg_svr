using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using wServer.realm;
using System.Net.NetworkInformation;

namespace wServer
{
    static class Program
    {
        static Socket svrSkt;

        static void HostPolicyServer()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 843);
                listener.Start();
                listener.BeginAcceptTcpClient(ServePolicyFile, listener);
            }
            catch { }
        }

        static void Main(string[] args)
        {
            svrSkt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            svrSkt.Bind(new IPEndPoint(IPAddress.Any, 2050));
            svrSkt.Listen(0xff);
            svrSkt.BeginAccept(Listen, null);
            Console.CancelKeyPress += (sender, e) =>
            {
                Console.WriteLine("Terminating...");
                svrSkt.Close();
                foreach (var i in RealmManager.Clients.Values.ToArray())
                    i.Disconnect();
                Environment.Exit(0);
            };
            Console.WriteLine("Listening at port 2050...");

            HostPolicyServer();

            RealmManager.CoreTickLoop();    //Never returns
        }

        static void ServePolicyFile(IAsyncResult ar)
        {
            TcpClient cli = (ar.AsyncState as TcpListener).EndAcceptTcpClient(ar);
            (ar.AsyncState as TcpListener).BeginAcceptTcpClient(ServePolicyFile, ar.AsyncState);
            try
            {
                var s = cli.GetStream();
                NReader rdr = new NReader(s);
                NWriter wtr = new NWriter(s);
                if (rdr.ReadNullTerminatedString() == "<policy-file-request/>")
                {
                    wtr.WriteNullTerminatedString(@"<cross-domain-policy>
     <allow-access-from domain=""*"" to-ports=""*"" />
</cross-domain-policy>");
                    wtr.Write((byte)'\r');
                    wtr.Write((byte)'\n');
                }
                cli.Close();
            }
            catch { }
        }

        static void Listen(IAsyncResult ar)
        {
            try
            {
                Socket skt = svrSkt.EndAccept(ar);
                svrSkt.BeginAccept(Listen, null);

                var psr = new ClientProcessor(skt);
                psr.BeginProcess();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
