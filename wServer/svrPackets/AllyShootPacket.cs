using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class AllyShootPacket : ServerPacket
    {
        public byte BulletId { get; set; }
        public int OwnerId { get; set; }
        public short ContainerType { get; set; }
        public float Angle { get; set; }

        public override PacketID ID { get { return PacketID.AllyShoot; } }
        public override Packet CreateInstance() { return new AllyShootPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            BulletId = rdr.ReadByte();
            OwnerId = rdr.ReadInt32();
            ContainerType = rdr.ReadInt16();
            Angle = rdr.ReadSingle();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(BulletId);
            wtr.Write(OwnerId);
            wtr.Write(ContainerType);
            wtr.Write(Angle);
        }
    }
}
