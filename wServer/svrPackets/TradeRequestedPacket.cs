using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class TradeRequestedPacket : ServerPacket
    {
        public string Name { get; set; }

        public override PacketID ID { get { return PacketID.TradeRequested; } }
        public override Packet CreateInstance() { return new TradeRequestedPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
