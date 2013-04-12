using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class PlayerHitPacket : ClientPacket
    {
        public byte BulletId { get; set; }
        public int ObjectId { get; set; }

        public override PacketID ID { get { return PacketID.PlayerHit; } }
        public override Packet CreateInstance() { return new PlayerHitPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            ObjectId = rdr.ReadInt32();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
        }
    }
}
