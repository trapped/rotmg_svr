using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class GotoAckPacket : ClientPacket
    {
        public int Time { get; set; }

        public override PacketID ID { get { return PacketID.GotoAck; } }
        public override Packet CreateInstance() { return new GotoAckPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Time);
        }
    }
}
