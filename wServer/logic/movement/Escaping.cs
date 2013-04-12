using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;
using Mono.Game;

namespace wServer.logic.movement
{
    class Escaping : Behavior
    {
        float speed;
        float radius;
        int threshold;
        short? objType;
        private Escaping(float speed, float radius, int threshold, short? objType)
        {
            this.speed = speed;
            this.radius = radius;
            this.threshold = threshold;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, int, short?>, Escaping> instances = new Dictionary<Tuple<float, float, int, short?>, Escaping>();
        public static Escaping Instance(float speed, float radius, int threshold, short? objType)
        {
            var key = new Tuple<float, float, int, short?>(speed, radius, threshold, objType);
            Escaping ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Escaping(speed, radius, threshold, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = GetNearestEntity(ref dist, objType);
            Character chr = Host as Character;
            if (entity != null && chr.HP < threshold)
            {
                var x = Host.Self.X;
                var y = Host.Self.Y;
                Vector2 vect = new Vector2(entity.X, entity.Y) - new Vector2(Host.Self.X, Host.Self.Y);
                vect.Normalize();
                vect *= -1 * (speed / 1.5f) * (time.thisTickTimes / 1000f);
                ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                Host.Self.UpdateCount++;

                if (!Host.StateStorage.ContainsKey(Key))
                {
                    chr.Owner.BroadcastPacket(new ShowEffectPacket()
                    {
                        EffectType = EffectType.Flashing,
                        PosA = new Position() { X = 1, Y = 1000000 },
                        TargetId = chr.Id,
                        Color = new ARGB(0xff303030)
                    }, null);
                    Host.StateStorage[Key] = true;
                }

                return true;
            }
            else return false;
        }
    }
}
