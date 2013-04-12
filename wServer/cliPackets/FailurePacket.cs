using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class FailurePacket : ClientPacket
    {
        public string Message { get; set; }

        public override PacketID ID { get { return PacketID.Failure; } }
        public override Packet CreateInstance() { return new FailurePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Message = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Message);
        }
    }
}
