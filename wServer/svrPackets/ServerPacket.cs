using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public abstract class ServerPacket : Packet
    {
        public override byte[] Crypt(ClientProcessor psr, byte[] dat)
        {
            return psr.SendKey.Crypt(dat);
        }
    }
}
