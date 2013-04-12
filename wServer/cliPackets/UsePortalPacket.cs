using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class UsePortalPacket : ClientPacket
    {
        public int ObjectId { get; set; }

        public override PacketID ID { get { return PacketID.UsePortal; } }
        public override Packet CreateInstance() { return new UsePortalPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
