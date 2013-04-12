using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class EscapePacket : ClientPacket
    {
        public override PacketID ID { get { return PacketID.Escape; } }
        public override Packet CreateInstance() { return new EscapePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr) { }

        protected override void Write(ClientProcessor psr, NWriter wtr) { }
    }
}
