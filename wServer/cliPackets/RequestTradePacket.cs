using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class RequestTradePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID { get { return PacketID.RequestTrade; } }
        public override Packet CreateInstance() { return new RequestTradePacket(); }

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
