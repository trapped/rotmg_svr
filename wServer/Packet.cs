using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.cliPackets;
using wServer.svrPackets;
using System.Net.Sockets;
using System.Net;

namespace wServer
{
    public abstract class Packet
    {
        public static Dictionary<PacketID, Packet> Packets = new Dictionary<PacketID, Packet>();
        static Packet()
        {
            foreach (var i in typeof(Packet).Assembly.GetTypes())
                if (typeof(Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet)Activator.CreateInstance(i);
                    if (!(pkt is ServerPacket))
                        Packets.Add(pkt.ID, pkt);
                }
        }
        public abstract PacketID ID { get; }
        public abstract Packet CreateInstance();

        public abstract byte[] Crypt(ClientProcessor psr, byte[] dat);


        public static void BeginReadPacket(Socket skt, ClientProcessor psr, Action<Packet> callback, Action failure)
        {
            byte[] n = new byte[5];
            skt.BeginReceive(n, 0, 5, 0, ar =>
            {
                try
                {
                    byte[] x = (byte[])ar.AsyncState;
                    int len = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(x, 0));
                    if (len == 0)
                    {
                        failure();
                        return;
                    }
                    len -= 5;
                    byte id = x[4];
                    Packet packet = Packets[(PacketID)id].CreateInstance();
                    byte[] content = new byte[len];
                    if (len > 0)
                        skt.Receive(content);
                    packet.Read(psr, new NReader(new MemoryStream(packet.Crypt(psr, content))));
                    callback(packet);
                }
                catch { failure(); }
            }, n);
        }
        public static Packet ReadPacket(NReader rdr, ClientProcessor psr)
        {
            int len = rdr.ReadInt32() - 5;
            byte id = rdr.ReadByte();
            Packet packet = Packets[(PacketID)id].CreateInstance();
            byte[] content = rdr.ReadBytes(len);
            packet.Read(psr, new NReader(new MemoryStream(packet.Crypt(psr, content))));
            return packet;
        }


        public static void WritePacket(NWriter wtr, Packet pkt, ClientProcessor psr)
        {
            MemoryStream s = new MemoryStream();
            pkt.Write(psr, new NWriter(s));

            byte[] content = s.ToArray();
            content = pkt.Crypt(psr, content);
            wtr.Write(content.Length + 5);
            wtr.Write((byte)pkt.ID);
            wtr.Write(content);
        }

        protected abstract void Read(ClientProcessor psr, NReader rdr);
        protected abstract void Write(ClientProcessor psr, NWriter wtr);

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder("{");
            var arr = GetType().GetProperties();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.AppendFormat("{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
            }
            ret.Append("}");
            return ret.ToString();
        }
    }

    public class NopPacket : Packet
    {
        public override PacketID ID { get { return PacketID.Packet; } }
        public override Packet CreateInstance() { return new NopPacket(); }
        public override byte[] Crypt(ClientProcessor psr, byte[] dat) { return dat; }
        protected override void Read(ClientProcessor psr, NReader rdr) { }
        protected override void Write(ClientProcessor psr, NWriter wtr) { }
    }
}
