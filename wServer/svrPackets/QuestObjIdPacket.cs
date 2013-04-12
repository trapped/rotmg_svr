using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class QuestObjIdPacket : ServerPacket
    {
        public int ObjectID { get; set; }

        public override PacketID ID { get { return PacketID.QuestObjId; } }
        public override Packet CreateInstance() { return new QuestObjIdPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            ObjectID = rdr.ReadInt32();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(ObjectID);
        }
    }
}
