using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class DamagePacket : ServerPacket
    {
        public int TargetId { get; set; }
        public ConditionEffects Effects { get; set; }
        public ushort Damage { get; set; }
        public bool Killed { get; set; }
        public byte BulletId { get; set; }
        public int ObjectId { get; set; }

        public override PacketID ID { get { return PacketID.Damage; } }
        public override Packet CreateInstance() { return new DamagePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            TargetId = rdr.ReadInt32();
            byte c = rdr.ReadByte();
            Effects = 0;
            for (int i = 0; i < c; i++)
                Effects |= (ConditionEffects)(1 << rdr.ReadByte());
            Damage = rdr.ReadUInt16();
            Killed = rdr.ReadBoolean();
            BulletId = rdr.ReadByte();
            ObjectId = rdr.ReadInt32();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(TargetId);
            List<byte> eff = new List<byte>();
            for (byte i = 1; i < 255; i++)
                if ((Effects & (ConditionEffects)(1 << i)) != 0)
                    eff.Add(i);
            wtr.Write((byte)eff.Count);
            foreach (var i in eff) wtr.Write(i);
            wtr.Write(Damage);
            wtr.Write(Killed);
            wtr.Write(BulletId);
            wtr.Write(ObjectId);
        }
    }
}
