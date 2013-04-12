using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class InvDropPacket : ClientPacket
    {
        public ObjectSlot Slot { get; set; }

        public override PacketID ID { get { return PacketID.InvDrop; } }
        public override Packet CreateInstance() { return new InvDropPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Slot = ObjectSlot.Read(rdr);
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            Slot.Write(wtr);
        }
    }
}
