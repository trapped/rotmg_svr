using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class EnemyHitPacket : ClientPacket
    {
        public int Time { get; set; }
        public byte BulletId { get; set; }
        public int TargetId { get; set; }
        public bool Killed { get; set; }

        public override PacketID ID { get { return PacketID.EnemyHit; } }
        public override Packet CreateInstance() { return new EnemyHitPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
            TargetId = rdr.ReadInt32();
            Killed = rdr.ReadBoolean();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Time);
            wtr.Write(BulletId);
            wtr.Write(TargetId);
            wtr.Write(Killed);
        }
    }
}
