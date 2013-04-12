using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.svrPackets
{
    public class ReconnectPacket : ServerPacket
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int GameId { get; set; }
        public int KeyTime { get; set; }
        public byte[] Key { get; set; }

        public override PacketID ID { get { return PacketID.Reconnect; } }
        public override Packet CreateInstance() { return new ReconnectPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            Host = rdr.ReadUTF();
            Port = rdr.ReadInt32();
            GameId = rdr.ReadInt32();
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.WriteUTF(Host);
            wtr.Write(Port);
            wtr.Write(GameId);
            wtr.Write(KeyTime);
            wtr.Write((short)Key.Length);
            wtr.Write(Key);
        }
    }
}
