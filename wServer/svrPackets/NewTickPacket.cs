using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.svrPackets
{
    public class NewTickPacket : ServerPacket
    {
        public int TickId { get; set; }
        public int TickTime { get; set; }
        public ObjectStats[] UpdateStatuses { get; set; }

        public override PacketID ID { get { return PacketID.New_Tick; } }
        public override Packet CreateInstance() { return new NewTickPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            TickId = rdr.ReadInt32();
            TickTime = rdr.ReadInt32();

            UpdateStatuses = new ObjectStats[rdr.ReadInt16()];
            for (var i = 0; i < UpdateStatuses.Length; i++)
                UpdateStatuses[i] = ObjectStats.Read(rdr);
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(TickId);
            wtr.Write(TickTime);

            wtr.Write((short)UpdateStatuses.Length);
            foreach (var i in UpdateStatuses)
                i.Write(wtr);
        }
    }
}
