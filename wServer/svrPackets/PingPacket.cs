using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class PingPacket : ServerPacket
    {
        public int Serial { get; set; }

        public override PacketID ID { get { return PacketID.Ping; } }
        public override Packet CreateInstance() { return new PingPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Serial = rdr.ReadInt32();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Serial);
        }
    }
}
