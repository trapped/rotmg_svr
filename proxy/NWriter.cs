using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace wServer
{
    class NWriter : BinaryWriter
    {
        public NWriter(Stream s) : base(s, Encoding.UTF8) { }

        public override void Write(short value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }
        public override void Write(int value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }
        public override void Write(long value)
        {
            base.Write(IPAddress.HostToNetworkOrder(value));
        }
        public override void Write(ushort value)
        {
            base.Write((ushort)IPAddress.HostToNetworkOrder((short)value));
        }
        public override void Write(uint value)
        {
            base.Write((uint)IPAddress.HostToNetworkOrder((int)value));
        }
        public override void Write(ulong value)
        {
            base.Write((ulong)IPAddress.HostToNetworkOrder((long)value));
        }
        public override void Write(float value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }
        public override void Write(double value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }

        public void WriteNullTerminatedString(string str)
        {
            Write(Encoding.UTF8.GetBytes(str));
            Write((byte)0);
        }

        public void WriteUTF(string str)
        {
            Write((short)str.Length);
            Write(Encoding.UTF8.GetBytes(str));
        }

        public void Write32UTF(string str)
        {
            Write((int)str.Length);
            Write(Encoding.UTF8.GetBytes(str));
        }
    }
}
