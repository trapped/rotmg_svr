using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic
{
    class OnDeath : ConditionalBehavior
    {
        Behavior behav;
        public OnDeath(Behavior behav)
        {
            this.behav = behav;
        }
        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.OnDeath; }
        }

        protected override void BehaveCore(BehaviorCondition cond, RealmTime? time, object state)
        {
            behav.Tick(Host, time.Value);
        }
    }

    class OnHit : ConditionalBehavior
    {
        Behavior behav;
        public OnHit(Behavior behav)
        {
            this.behav = behav;
        }
        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.OnHit; }
        }

        protected override void BehaveCore(BehaviorCondition cond, RealmTime? time, object state)
        {
            behav.Tick(Host, time.Value);
        }
    }
}
