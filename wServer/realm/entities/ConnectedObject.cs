using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using db;
using wServer.svrPackets;

namespace wServer.realm.entities
{
    public enum ConnectionType
    {
        Dot = 0,
        ShortLine = 1,
        L = 2,
        Line = 3,
        T = 4,
        Cross = 5
    }
    public class ConnectionInfo
    {
        public static readonly Dictionary<uint, ConnectionInfo> Infos = new Dictionary<uint, ConnectionInfo>();
        public static readonly Dictionary<Tuple<ConnectionType, int>, ConnectionInfo> Infos2 = new Dictionary<Tuple<ConnectionType, int>, ConnectionInfo>();
        static ConnectionInfo()
        {
            Build(0x02020202, ConnectionType.Dot);          //1111
            Build(0x01020202, ConnectionType.ShortLine);    //0111
            Build(0x01010202, ConnectionType.L);            //0011
            Build(0x01020102, ConnectionType.Line);         //0101
            Build(0x01010201, ConnectionType.T);            //0010
            Build(0x01010101, ConnectionType.Cross);        //0000
        }

        static void Build(uint bits, ConnectionType type)
        {
            for (int i = 0; i < 4; i++)
                if (!Infos.ContainsKey(bits))
                {
                    Infos[bits] = Infos2[Tuple.Create(type, i * 90)] = new ConnectionInfo(bits, type, i * 90);
                    bits = (bits >> 8) | (bits << 24);
                }
        }


        public ConnectionType Type { get; private set; }
        public int Rotation { get; private set; }
        public uint Bits { get; private set; }

        private ConnectionInfo(uint bits, ConnectionType type, int rotation)
        {
            Bits = bits;
            Type = type;
            Rotation = rotation;
        }
    }
    public class ConnectionComputer
    {
        public static ConnectionInfo Compute(Func<int, int, bool> offset)
        {
            bool[,] z = new bool[3, 3];
            for (int y = -1; y <= 1; y++)
                for (int x = -1; x <= 1; x++)
                    z[x + 1, y + 1] = offset(x, y);


            if (z[1, 0] && z[1, 2] && z[0, 1] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Cross, 0)];

            else if (z[0, 1] && z[1, 1] && z[2, 1] && z[1, 0])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 0)];
            else if (z[1, 0] && z[1, 1] && z[1, 2] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 90)];
            else if (z[0, 1] && z[1, 1] && z[2, 1] && z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 180)];
            else if (z[1, 0] && z[1, 1] && z[1, 2] && z[0, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.T, 270)];

            else if (z[0, 1] && z[1, 1] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Line, 0)];
            else if (z[1, 0] && z[1, 1] && z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Line, 90)];

            else if (z[1, 0] && z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 0)];
            else if (z[2, 1] && z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 90)];
            else if (z[1, 2] && z[0, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 180)];
            else if (z[0, 1] && z[1, 0])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.L, 270)];
                
            else if (z[1, 0])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ShortLine, 0)];
            else if (z[2, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ShortLine, 90)];
            else if (z[1, 2])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ShortLine, 180)];
            else if (z[0, 1])
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.ShortLine, 270)];

            else
                return ConnectionInfo.Infos2[Tuple.Create(ConnectionType.Dot, 0)];
        }
        public static string GetConnString(Func<int, int, bool> offset)
        {
            return "conn:" + Compute(offset).Bits;
        }
    }

    public class ConnectedObject : StaticObject
    {
        public ConnectionInfo Connection { get; set; }
        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.ObjectConnection)
                Connection = ConnectionInfo.Infos[(uint)(int)val];
            base.ImportStats(stats, val);
        }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.ObjectConnection] = (int)Connection.Bits;
            base.ExportStats(stats);
        }

        public ConnectedObject(short objType)
            : base(objType, null, true, false, true)
        {
        }


        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return true;
        }
    }
}
