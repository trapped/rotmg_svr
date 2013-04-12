using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.attack;
using wServer.logic.movement;
using wServer.logic.loot;
using wServer.logic.taunt;

namespace wServer.logic
{
    using BehaviorDef = Tuple<Behavior, Behavior, Behavior, ConditionalBehavior[]>;

    partial class BehaviorDb
    {
        public static void ResolveBehavior(Entity entity)
        {
            BehaviorDef b;
            if (Behaviors.TryGetValue(entity.ObjectType, out b))
            {
                entity.MovementBehavior = b.Item1;
                entity.AttackBehavior = b.Item2;
                entity.ReproduceBehavior = b.Item3;
                entity.CondBehaviors = b.Item4;
            }
            else
            {
                entity.MovementBehavior =
                entity.AttackBehavior =
                entity.ReproduceBehavior = NullBehavior.Instance;
                entity.CondBehaviors = Empty<ConditionalBehavior>.Array;
            }
        }

        //Candies
        static BehaviorDef Behaves(
            string name,
            Behavior movement = null,
            Behavior attack = null,
            Behavior reproduce = null,
            LootBehavior loot = null,
            params ConditionalBehavior[] condBehaviors)
        {
            if (loot != null)
            {
                Array.Resize(ref condBehaviors, condBehaviors.Length + 1);
                condBehaviors[condBehaviors.Length - 1] = loot;
            }

            return new BehaviorDef(
                movement ?? NullBehavior.Instance,
                attack ?? NullBehavior.Instance,
                reproduce ?? NullBehavior.Instance,
                condBehaviors);
        }

        struct _
        {
            public _ Init(short objType, BehaviorDef b)
            {
                Behaviors.Add(objType, b);
                return this;
            }
        }

        static _ Behav()
        {
            if (Behaviors == null)
                Behaviors = new Dictionary<short, BehaviorDef>();
            return new _();
        }

        public static Dictionary<short, BehaviorDef> Behaviors;
    }
}
