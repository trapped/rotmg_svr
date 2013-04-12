using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class MultiShootPacket : ServerPacket
    {
        public byte BulletId { get; set; }
        public int OwnerId { get; set; }
        public byte BulletType { get; set; }
        public Position Position { get; set; }
        public float Angle { get; set; }
        public short Damage { get; set; }
        public byte NumShots { get; set; }
        public float AngleIncrement { get; set; }

        public override PacketID ID { get { return PacketID.MultiShoot; } }
        public override Packet CreateInstance() { return new MultiShootPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            OwnerId = rdr.ReadInt32();
            BulletType = rdr.ReadByte();
            Position = Position.Read(rdr);
            Angle = rdr.ReadSingle();
            Damage = rdr.ReadInt16();
            NumShots = rdr.ReadByte();
            AngleIncrement = rdr.ReadSingle();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(BulletType);
            Position.Write(wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
            wtr.Write(NumShots);
            wtr.Write(AngleIncrement);
        }
    }
}
