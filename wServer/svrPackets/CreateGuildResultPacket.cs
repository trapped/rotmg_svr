using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class CreateGuildResultPacket : ServerPacket
    {
        public string ResultMessage { get; set; }
        public bool Success { get; set; }

        public override PacketID ID { get { return PacketID.CreateGuildResult; } }
        public override Packet CreateInstance() { return new CreateGuildResultPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            ResultMessage = rdr.ReadUTF();
            Success = rdr.ReadBoolean();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(ResultMessage);
        }
    }
}
