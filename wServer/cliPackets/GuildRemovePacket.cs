using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class GuildRemovePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID { get { return PacketID.GuildRemove; } }
        public override Packet CreateInstance() { return new GuildRemovePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
        }
    }
}
