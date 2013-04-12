using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class NameResultPacket : ServerPacket
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public override PacketID ID { get { return PacketID.NameResult; } }
        public override Packet CreateInstance() { return new NameResultPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Success = rdr.ReadBoolean();
            Message = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Success);
            wtr.WriteUTF(Message);
        }
    }
}
