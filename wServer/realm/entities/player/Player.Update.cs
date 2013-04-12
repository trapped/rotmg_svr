using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.svrPackets;

namespace wServer.realm.entities
{
    public partial class Player
    {
        public const int RADIUS = 15;
        const int APPOX_AREA_OF_SIGHT = (int)(Math.PI * RADIUS * RADIUS + 1);

        double DistanceSquared(float x, float y, float oX, float oY)
        {
            return (x - 0.5 - (int)oX) * (x - 0.5 - (int)oX) + (y - 0.5 - (int)oY) * (y - 0.5 - (int)oY);
        }

        int mapWidth, mapHeight;

        HashSet<Entity> clientEntities = new HashSet<Entity>();
        HashSet<IntPoint> clientStatic = new HashSet<IntPoint>(new IntPointComparer());

        IEnumerable<Entity> GetNewEntities()
        {
            foreach (var i in Owner.Players)
            {
                if (clientEntities.Add(i.Value))
                    yield return i.Value;
            }
            foreach (var i in Owner.PlayersCollision.HitTest(X, Y, RADIUS))
            {
                if (i is Decoy)
                {
                    if (clientEntities.Add(i))
                        yield return i;
                }
            }
            foreach (var i in Owner.EnemiesCollision.HitTest(X, Y, RADIUS))
            {
                if (i is Container)
                {
                    int? owner = (i as Container).BagOwner;
                    if (owner != null && owner != AccountId) continue;
                }
                if (DistanceSquared(i.X, i.Y, X, Y) <= RADIUS * RADIUS)
                {
                    if (clientEntities.Add(i))
                        yield return i;
                }
            }
            if (questEntity != null && clientEntities.Add(questEntity))
                yield return questEntity;
        }
        IEnumerable<int> GetRemovedEntities()
        {
            foreach (var i in clientEntities)
            {
                if (i is Player && i.Owner != null) continue;
                if (DistanceSquared(i.X, i.Y, X, Y) > RADIUS * RADIUS &&
                    !(i is StaticObject && (i as StaticObject).Static) &&
                    i != questEntity)
                    yield return i.Id;
                else if (i.Owner == null)
                    yield return i.Id;
            }
        }
        IEnumerable<ObjectDef> GetNewStatics(int _x, int _y)
        {
            List<ObjectDef> ret = new List<ObjectDef>();
            foreach (var i in Sight.GetSightCircle(RADIUS))
            {
                int x = i.X + _x;
                int y = i.Y + _y;
                if (x < 0 || x >= mapWidth ||
                    y < 0 || y >= mapHeight) continue;
                var tile = Owner.Map[x, y];
                if (tile.ObjId != 0 && tile.ObjType != 0 && clientStatic.Add(new IntPoint(x, y)))
                    ret.Add(tile.ToDef(x, y));
            }
            return ret;
        }
        IEnumerable<IntPoint> GetRemovedStatics(int _x, int _y)
        {
            foreach (var i in clientStatic)
            {
                var dx = i.X - _x;
                var dy = i.Y - _y;
                var tile = Owner.Map[i.X, i.Y];
                if (dx * dx + dy * dy > RADIUS * RADIUS ||
                    tile.ObjType == 0)
                {
                    int objId = Owner.Map[i.X, i.Y].ObjId;
                    if (objId != 0)
                        yield return i;
                }
            }
        }

        Dictionary<Entity, int> lastUpdate = new Dictionary<Entity, int>();
        void SendUpdate(RealmTime time)
        {
            mapWidth = Owner.Map.Width;
            mapHeight = Owner.Map.Height;
            var map = Owner.Map;
            int _x = (int)X; int _y = (int)Y;

            var sendEntities = new HashSet<Entity>(GetNewEntities());

            var list = new List<UpdatePacket.TileData>(APPOX_AREA_OF_SIGHT);
            int sent = 0;
            foreach (var i in Sight.GetSightCircle(RADIUS))
            {
                int x = i.X + _x;
                int y = i.Y + _y;
                WmapTile tile;
                if (x < 0 || x >= mapWidth ||
                    y < 0 || y >= mapHeight ||
                    tiles[x, y] >= (tile = map[x, y]).UpdateCount) continue;
                list.Add(new UpdatePacket.TileData()
                {
                    X = (short)x,
                    Y = (short)y,
                    Tile = (Tile)tile.TileId
                });
                tiles[x, y] = tile.UpdateCount;
                sent++;
            }
            fames.TileSent(sent);

            var dropEntities = GetRemovedEntities().Distinct().ToArray();
            clientEntities.RemoveWhere(_ => Array.IndexOf(dropEntities, _.Id) != -1);

            foreach (var i in sendEntities)
                lastUpdate[i] = i.UpdateCount;

            var newStatics = GetNewStatics(_x, _y).ToArray();
            var removeStatics = GetRemovedStatics(_x, _y).ToArray();
            List<int> removedIds = new List<int>();
            foreach (var i in removeStatics)
            {
                removedIds.Add(Owner.Map[i.X, i.Y].ObjId);
                clientStatic.Remove(i);
            }

            if (sendEntities.Count > 0 || list.Count > 0 || dropEntities.Length > 0 ||
                newStatics.Length > 0 || removedIds.Count > 0)
            {
                UpdatePacket packet = new UpdatePacket();
                packet.Tiles = list.ToArray();
                packet.NewObjects = sendEntities.Select(_ => _.ToDefinition()).Concat(newStatics).ToArray();
                packet.RemovedObjectIds = dropEntities.Concat(removedIds).ToArray();
                psr.SendPacket(packet);
            }
            SendNewTick(time);
        }

        const int TICK_PER_SECOND = 30;
        int tickPassed = RealmManager.TPS / TICK_PER_SECOND;
        int timePassed = 0;
        int tickId = 0;
        long tickIdTime = 0;
        void SendNewTick(RealmTime time)
        {
            if (tickPassed > 1)
            {
                tickPassed--;
                timePassed += time.thisTickTimes;
                return;
            }
            else
            {
                tickPassed = RealmManager.TPS / TICK_PER_SECOND;
                tickIdTime = time.tickTimes;
            }

            var sendEntities = new List<Entity>();
            foreach (var i in clientEntities)
            {
                if (i.UpdateCount > lastUpdate[i])
                {
                    sendEntities.Add(i);
                    lastUpdate[i] = i.UpdateCount;
                }
            }
            if (questEntity != null && (!lastUpdate.ContainsKey(questEntity) || questEntity.UpdateCount > lastUpdate[questEntity]))
            {
                sendEntities.Add(questEntity);
                lastUpdate[questEntity] = questEntity.UpdateCount;
            }
            NewTickPacket p = new NewTickPacket();
            tickId++;
            p.TickId = tickId;
            p.TickTime = timePassed;
            timePassed = time.thisTickTimes;
            p.UpdateStatuses = sendEntities.Select(_ => _.ExportStats()).ToArray();
            psr.SendPacket(p);

            SaveToCharacter();
        }
    }
}
