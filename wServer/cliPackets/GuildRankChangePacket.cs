using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class GuildRankChangePacket : ClientPacket
    {
        public string Name { get; set; }
        public int Rank { get; set; }
        public override PacketID ID { get { return PacketID.ChangeGuildRank; } }
        public override Packet CreateInstance() { return new GuildRankChangePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            Rank = rdr.ReadInt32();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.Write(Rank);
        }
    }
}
