using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class CancelTradePacket : ClientPacket
    {
        public override PacketID ID { get { return PacketID.CancelTrade; } }
        public override Packet CreateInstance() { return new CancelTradePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr) { }

        protected override void Write(ClientProcessor psr, NWriter wtr) { }
    }
}
