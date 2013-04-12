using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.attack
{
    class TossEnemy : Behavior
    {
        float angle;
        float range;
        short objType;
        private TossEnemy(float angle, float range, short objType)
        {
            this.angle = angle;
            this.range = range;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, short>, TossEnemy> instances = new Dictionary<Tuple<float, float, short>, TossEnemy>();
        public static TossEnemy Instance(float angle, float range, short objType)
        {
            var key = new Tuple<float, float, short>(angle, range, objType);
            TossEnemy ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new TossEnemy(angle, range, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            var chr = Host as Character;
            Position target = new Position()
            {
                X = Host.Self.X,
                Y = Host.Self.Y
            };
            target.X += (float)Math.Cos(angle) * range;
            target.Y += (float)Math.Sin(angle) * range;
            chr.Owner.BroadcastPacket(new ShowEffectPacket()
            {
                EffectType = EffectType.Throw,
                Color = new ARGB(0xffffbf00),
                TargetId = Host.Self.Id,
                PosA = target
            }, null);
            chr.Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
            {
                Entity entity = Entity.Resolve(objType);
                entity.Move(target.X, target.Y);
                (entity as Enemy).Terrain = (chr as Enemy).Terrain;
                world.EnterWorld(entity);
            }));

            return true;
        }
    }
}
