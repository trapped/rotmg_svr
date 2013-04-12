using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class ShootPacket : ServerPacket
    {
        public byte BulletId { get; set; }
        public int OwnerId { get; set; }
        public short ContainerType { get; set; }
        public Position Position { get; set; }
        public float Angle { get; set; }
        public short Damage { get; set; }

        public override PacketID ID { get { return PacketID.Shoot; } }
        public override Packet CreateInstance() { return new ShootPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            OwnerId = rdr.ReadInt32();
            ContainerType = rdr.ReadInt16();
            Position = Position.Read(rdr);
            Angle = rdr.ReadSingle();
            Damage = rdr.ReadInt16();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(ContainerType);
            Position.Write(wtr);
            wtr.Write(Angle);
            wtr.Write(Damage);
        }
    }
}
