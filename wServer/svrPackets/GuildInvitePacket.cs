using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class GuildInvitePacket : ServerPacket
    {
        public string Name { get; set; }
        public string Guild { get; set; }
        public override PacketID ID { get { return PacketID.InvitedToGuild; } }
        public override Packet CreateInstance() { return new GuildInvitePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            Guild = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.WriteUTF(Guild);
        }
    }
}
