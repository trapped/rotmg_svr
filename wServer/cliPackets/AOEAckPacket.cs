using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class AOEAckPacket : ClientPacket
    {
        public int Time { get; set; }
        public Position Position { get; set; }

        public override PacketID ID { get { return PacketID.AOEAck; } }
        public override Packet CreateInstance() { return new AOEAckPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(rdr);
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(wtr);
        }
    }
}
