using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace wServer.realm
{
    public class SpatialStorage
    {
        ConcurrentDictionary<int, ConcurrentDictionary<int, Entity>> store = new ConcurrentDictionary<int, ConcurrentDictionary<int, Entity>>();

        const int SCALE_FACTOR = 16;

        int HashPosition(double x, double y)
        {
            int ix = (int)x / SCALE_FACTOR;
            int iy = (int)y / SCALE_FACTOR;
            return (ix << 16) | iy;
        }

        public void Insert(Entity entity)
        {
            int hash = HashPosition(entity.X, entity.Y);
            var bucket = store.GetOrAdd(hash, _ => new ConcurrentDictionary<int, Entity>());
            bucket[entity.Id] = entity;
        }

        public void Remove(Entity entity)
        {
            int hash = HashPosition(entity.X, entity.Y);
            var bucket = store[hash];
            bucket.TryRemove(entity.Id, out entity);
        }

        public void Move(Entity entity, double x, double y)
        {
            int hash = HashPosition(entity.X, entity.Y);
            var bucket = store.GetOrAdd(hash, _ => new ConcurrentDictionary<int, Entity>());
            Entity dummy;
            bucket.TryRemove(entity.Id, out dummy);

            hash = HashPosition(x, y);
            bucket = store.GetOrAdd(hash, _ => new ConcurrentDictionary<int, Entity>());
            bucket[entity.Id] = entity;
        }

        public IEnumerable<Entity> HitTest(Position pos, float radius)
        {
            return HitTest(pos.X, pos.Y, radius);
        }

        public IEnumerable<Entity> HitTest(double _x, double _y, float radius)
        {
            int xl = (int)(_x - radius) / SCALE_FACTOR;
            int xh = (int)(_x + radius) / SCALE_FACTOR;
            int yl = (int)(_y - radius) / SCALE_FACTOR;
            int yh = (int)(_y + radius) / SCALE_FACTOR;
            for (var x = xl; x <= xh; x++)
                for (var y = yl; y <= yh; y++)
                {
                    ConcurrentDictionary<int, Entity> bucket;
                    if (store.TryGetValue((x << 16) | y, out bucket))
                        foreach (var i in bucket) yield return i.Value;
                }
        }

        public IEnumerable<Entity> HitTest(double _x, double _y)
        {
            int x = (int)_x / SCALE_FACTOR;
            int y = (int)_y / SCALE_FACTOR;
            ConcurrentDictionary<int, Entity> bucket;
            if (store.TryGetValue((x << 16) | y, out bucket))
                return bucket.Values;
            else
                return Enumerable.Empty<Entity>();
        }
    }
}
