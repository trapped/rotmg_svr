using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using wServer.realm.entities;
using System.Collections.Concurrent;

namespace wServer.realm
{
    public abstract class World
    {
        public const int TUT_ID = -1;
        public const int NEXUS_ID = -2;
        public const int RAND_REALM = -3;
        public const int NEXUS_LIMBO = -4;
        public const int VAULT_ID = -5;
        public const int TEST_ID = -6;

        protected World()
        {
            Players = new ConcurrentDictionary<int, Player>();
            Enemies = new ConcurrentDictionary<int, Enemy>();
            Projectiles = new ConcurrentDictionary<Tuple<int, byte>, Projectile>();
            StaticObjects = new ConcurrentDictionary<int, StaticObject>();
            Timers = new List<WorldTimer>();
            EnemiesCollision = new SpatialStorage();
            PlayersCollision = new SpatialStorage();
            ClientXML = ExtraXML = Empty<string>.Array;
            Map = new Wmap();
            AllowTeleport = true;
            ShowDisplays = true;

            ExtraXML = new string[] { XmlDatas.AdditionXml };
        }

        public bool IsLimbo { get; protected set; }
        public virtual World GetInstance(ClientProcessor psr) { return null; }

        public int Id { get; internal set; }
        public string Name { get; protected set; }

        public ConcurrentDictionary<int, Player> Players { get; private set; }
        public ConcurrentDictionary<int, Enemy> Enemies { get; private set; }
        public ConcurrentDictionary<Tuple<int, byte>, Projectile> Projectiles { get; private set; }
        public ConcurrentDictionary<int, StaticObject> StaticObjects { get; private set; }
        public List<WorldTimer> Timers { get; private set; }
        public int Background { get; protected set; }

        public SpatialStorage EnemiesCollision { get; private set; }
        public SpatialStorage PlayersCollision { get; private set; }
        public byte[,] Obstacles { get; private set; }

        public bool AllowTeleport { get; protected set; }
        public bool ShowDisplays { get; protected set; }
        public string[] ClientXML { get; protected set; }
        public string[] ExtraXML { get; protected set; }

        public Wmap Map { get; private set; }

        private int entityInc = 0;
        public int GetNextEntityId()
        {
            return Interlocked.Increment(ref entityInc);
        }

        public bool Delete()
        {
            lock (this)
            {
                if (Players.Count > 0) return false;
                Id = 0;
            }
            Map = null;
            Players = null;
            Enemies = null;
            Projectiles = null;
            StaticObjects = null;
            return true;
        }

        protected void FromWorldMap(System.IO.Stream dat)
        {
            Wmap map = new Wmap();
            this.Map = map;
            entityInc = 0;
            entityInc += map.Load(dat, 0);

            int w = map.Width, h = map.Height;
            Obstacles = new byte[w, h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var tile = map[x, y];
                    ObjectDesc desc;
                    if (XmlDatas.TileDescs[tile.TileId].NoWalk)
                        Obstacles[x, y] = 3;
                    if (XmlDatas.ObjectDescs.TryGetValue(tile.ObjType, out desc))
                    {
                        if (desc.Class == "Wall" ||
                            desc.Class == "ConnectedWall" ||
                            desc.Class == "CaveWall")
                            Obstacles[x, y] = 2;
                        else if (desc.OccupySquare || desc.EnemyOccupySquare)
                            Obstacles[x, y] = 1;
                    }

                }

            Projectiles.Clear();
            StaticObjects.Clear();
            Enemies.Clear();
            Players.Clear();
            foreach (var i in map.InstantiateEntities())
            {
                if (i.ObjectDesc != null &&
                    (i.ObjectDesc.OccupySquare || i.ObjectDesc.EnemyOccupySquare))
                    Obstacles[(int)(i.X - 0.5), (int)(i.Y - 0.5)] = 2;
                EnterWorld(i);
            }
        }

        public virtual int EnterWorld(Entity entity)
        {
            if (entity is Player)
            {
                entity.Id = GetNextEntityId();
                entity.Init(this);
                Players.TryAdd(entity.Id, entity as Player);
                PlayersCollision.Insert(entity);
            }
            else if (entity is Enemy)
            {
                entity.Id = GetNextEntityId();
                entity.Init(this);
                Enemies.TryAdd(entity.Id, entity as Enemy);
            }
            else if (entity is Projectile)
            {
                entity.Init(this);
                Projectile prj = entity as Projectile;
                Projectiles[new Tuple<int, byte>(prj.ProjectileOwner.Self.Id, prj.ProjectileId)] = prj;
            }
            else if (entity is StaticObject)
            {
                entity.Id = GetNextEntityId();
                entity.Init(this);
                StaticObjects.TryAdd(entity.Id, entity as StaticObject);
                if (entity is Decoy)
                    PlayersCollision.Insert(entity);
                else
                    EnemiesCollision.Insert(entity);
            }
            return entity.Id;
        }
        public virtual void LeaveWorld(Entity entity)
        {
            if (entity is Player)
            {
                Player dummy;
                Players.TryRemove(entity.Id, out dummy);
                PlayersCollision.Remove(entity);
            }
            else if (entity is Enemy)
            {
                Enemy dummy;
                Enemies.TryRemove(entity.Id, out dummy);
                EnemiesCollision.Remove(entity);
            }
            else if (entity is Projectile)
            {
                Projectile p = entity as Projectile;
                Projectiles.TryRemove(new Tuple<int, byte>(p.ProjectileOwner.Self.Id, p.ProjectileId), out p);
            }
            else if (entity is StaticObject)
            {
                StaticObject dummy;
                StaticObjects.TryRemove(entity.Id, out dummy);
                if (entity is Decoy)
                    PlayersCollision.Remove(entity);
                else
                    EnemiesCollision.Remove(entity);
            }
            entity.Owner = null;
        }
        public Entity GetEntity(int id)
        {
            Player ret1;
            if (Players.TryGetValue(id, out ret1)) return ret1;
            Enemy ret2;
            if (Enemies.TryGetValue(id, out ret2)) return ret2;
            StaticObject ret3;
            if (StaticObjects.TryGetValue(id, out ret3)) return ret3;
            return null;
        }
        public Player GetUniqueNamedPlayer(string name)
        {
            foreach (var i in Players)
            {
                if (i.Value.NameChosen && i.Value.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return i.Value;
            }
            return null;
        }

        public void BroadcastPacket(Packet pkt, Player exclude)
        {
            foreach (var i in Players)
                if (i.Value != exclude)
                    i.Value.Client.SendPacket(pkt);
        }
        public void BroadcastPackets(IEnumerable<Packet> pkts, Player exclude)
        {
            foreach (var i in Players)
                if (i.Value != exclude)
                    i.Value.Client.SendPackets(pkts);
        }

        public virtual void Tick(RealmTime time)
        {
            if (IsLimbo) return;

            for (int i = 0; i < Timers.Count; i++)
                if (Timers[i].Tick(this, time))
                {
                    Timers.RemoveAt(i);
                    i--;
                }

            foreach (var i in Players)
                i.Value.Tick(time);
            foreach (var i in Enemies)
                i.Value.Tick(time);
            foreach (var i in Projectiles)
                i.Value.Tick(time);
            foreach (var i in StaticObjects)
                i.Value.Tick(time);
        }
    }
}
