using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class ShowEffectPacket : ServerPacket
    {
        public EffectType EffectType { get; set; }
        public int TargetId { get; set; }
        public Position PosA { get; set; }
        public Position PosB { get; set; }
        public ARGB Color { get; set; }

        public override PacketID ID { get { return PacketID.Show_Effect; } }
        public override Packet CreateInstance() { return new ShowEffectPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            EffectType = (EffectType)rdr.ReadByte();
            TargetId = rdr.ReadInt32();
            PosA = Position.Read(rdr);
            PosB = Position.Read(rdr);
            Color = ARGB.Read(rdr);
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write((byte)EffectType);
            wtr.Write(TargetId);
            PosA.Write(wtr);
            PosB.Write(wtr);
            Color.Write(wtr);
        }
    }
}
