using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class InvSwapPacket : ClientPacket
    {
        public int Time { get; set; }
        public Position Position { get; set; }
        public ObjectSlot Obj1 { get; set; }
        public ObjectSlot Obj2 { get; set; }

        public override PacketID ID { get { return PacketID.InvSwap; } }
        public override Packet CreateInstance() { return new InvSwapPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(rdr);
            Obj1 = ObjectSlot.Read(rdr);
            Obj2 = ObjectSlot.Read(rdr);
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(wtr);
            Obj1.Write(wtr);
            Obj2.Write(wtr);
        }
    }
}
