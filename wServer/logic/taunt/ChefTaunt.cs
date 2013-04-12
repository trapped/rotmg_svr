using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.svrPackets;
using wServer.realm.entities;

namespace wServer.logic.taunt
{
    class ChefTaunt : ConditionalBehavior
    {
        public ChefTaunt(int t) { HPThreshold = t; }
        public int HPThreshold { get; set; }

        public override BehaviorCondition Condition { get { return BehaviorCondition.Other; } }

        protected override bool ConditionMeetCore()
        {
            float dist = 8;
            return GetNearestEntity(ref dist, null) != null || (Host as Character).HP < HPThreshold;
        }


        protected override void BehaveCore(BehaviorCondition cond, RealmTime? time, object state)
        {
            float dist = 8;

            int n = 0;
            object o;
            if (Host.StateStorage.TryGetValue(Key, out o))
                n = (int)o;
            else
                n = 0;

            if (n == 0 && GetNearestEntity(ref dist, null) != null)
            {
                Taunt("Ah, fresh meat for the minions!");
                Host.StateStorage[Key] = 1;
            }
            else if (n < 2 && (Host as Character).HP < HPThreshold)
            {
                Taunt("The meat ain't supposed to bite back! Waaaaa!!");
                Host.StateStorage[Key] = 2;
            }
        }
    }
}
