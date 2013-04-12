using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class InvResultPacket : ServerPacket
    {
        public int Result { get; set; }

        public override PacketID ID { get { return PacketID.InvResult; } }
        public override Packet CreateInstance() { return new InvResultPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Result = rdr.ReadInt32();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Result);
        }
    }
}
