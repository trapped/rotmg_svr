using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class MapInfoPacket : ServerPacket
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public uint Seed { get; set; }
        public int Background { get; set; }
        public bool AllowTeleport { get; set; }
        public bool ShowDisplays { get; set; }
        public string[] ClientXML { get; set; }
        public string[] ExtraXML { get; set; }

        public override PacketID ID { get { return PacketID.MapInfo; } }
        public override Packet CreateInstance() { return new MapInfoPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Width = rdr.ReadInt32();
            Height = rdr.ReadInt32();
            Name = rdr.ReadUTF();
            Seed = rdr.ReadUInt32();
            Background = rdr.ReadInt32();
            AllowTeleport = rdr.ReadBoolean();
            ShowDisplays = rdr.ReadBoolean();

            ClientXML = new string[rdr.ReadInt16()];
            for (int i = 0; i < ClientXML.Length; i++)
                ClientXML[i] = rdr.Read32UTF();

            ExtraXML = new string[rdr.ReadInt16()];
            for (int i = 0; i < ExtraXML.Length; i++)
                ExtraXML[i] = rdr.Read32UTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Width);
            wtr.Write(Height);
            wtr.WriteUTF(Name);
            wtr.Write(Seed);
            wtr.Write(Background);
            wtr.Write(AllowTeleport);
            wtr.Write(ShowDisplays);

            wtr.Write((short)ClientXML.Length);
            foreach (var i in ClientXML)
                wtr.Write32UTF(i);

            wtr.Write((short)ExtraXML.Length);
            foreach (var i in ExtraXML)
                wtr.Write32UTF(i);
        }
    }
}
