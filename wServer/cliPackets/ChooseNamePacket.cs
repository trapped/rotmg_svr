using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class ChooseNamePacket : ClientPacket
    {
        public string Name { get; set; }

        public override PacketID ID { get { return PacketID.ChooseName; } }
        public override Packet CreateInstance() { return new ChooseNamePacket(); }

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
