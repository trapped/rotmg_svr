using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class CreateGuildPacket : ClientPacket
    {
        public string Name;
        public bool Success;
        public override PacketID ID { get { return PacketID.CreateGuild; } }
        public override Packet CreateInstance() { return new CreateGuildPacket(); }

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
