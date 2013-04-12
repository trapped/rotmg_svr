using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.svrPackets
{
    public class UpdatePacket : ServerPacket
    {
        public struct TileData
        {
            public short X;
            public short Y;
            public Tile Tile;
        }

        public TileData[] Tiles { get; set; }
        public ObjectDef[] NewObjects { get; set; }
        public int[] RemovedObjectIds { get; set; }

        public override PacketID ID { get { return PacketID.Update; } }
        public override Packet CreateInstance() { return new UpdatePacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Tiles = new TileData[rdr.ReadInt16()];
            for (var i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = new TileData()
                {
                    X = rdr.ReadInt16(),
                    Y = rdr.ReadInt16(),
                    Tile = (Tile)rdr.ReadByte()
                };
            }

            NewObjects = new ObjectDef[rdr.ReadInt16()];
            for (var i = 0; i < NewObjects.Length; i++)
                NewObjects[i] = ObjectDef.Read(rdr);

            RemovedObjectIds = new int[rdr.ReadInt16()];
            for (var i = 0; i < RemovedObjectIds.Length; i++)
                RemovedObjectIds[i] = rdr.ReadInt32();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write((short)Tiles.Length);
            foreach (var i in Tiles)
            {
                wtr.Write(i.X);
                wtr.Write(i.Y);
                wtr.Write((byte)i.Tile);
            }
            wtr.Write((short)NewObjects.Length);
            foreach (var i in NewObjects)
            {
                i.Write(wtr);
            }
            wtr.Write((short)RemovedObjectIds.Length);
            foreach (var i in RemovedObjectIds)
            {
                wtr.Write(i);
            }
        }
    }
}
