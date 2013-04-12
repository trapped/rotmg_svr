using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class CreateSuccessPacket : ServerPacket
    {
        public int ObjectID { get; set; }
        public int CharacterID { get; set; }

        public override PacketID ID { get { return PacketID.Create_Success; } }
        public override Packet CreateInstance() { return new CreateSuccessPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            ObjectID = rdr.ReadInt32();
            CharacterID = rdr.ReadInt32();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(ObjectID);
            wtr.Write(CharacterID);
        }
    }
}
