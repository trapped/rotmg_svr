using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class PlayerTextPacket : ClientPacket
    {
        public string Text { get; set; }

        public override PacketID ID { get { return PacketID.PlayerText; } }
        public override Packet CreateInstance() { return new PlayerTextPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Text = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Text);
        }
    }
}
