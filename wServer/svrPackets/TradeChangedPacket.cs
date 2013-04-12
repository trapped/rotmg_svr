using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class TradeChangedPacket : ServerPacket
    {
        public bool[] Offers { get; set; }

        public override PacketID ID { get { return PacketID.TradeChanged; } }
        public override Packet CreateInstance() { return new TradeChangedPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Offers = new bool[rdr.ReadInt16()];
            for (int i = 0; i < Offers.Length; i++)
                Offers[i] = rdr.ReadBoolean();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write((short)Offers.Length);
            foreach (var i in Offers)
                wtr.Write(i);
        }
    }
}
