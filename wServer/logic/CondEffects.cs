using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic
{
    class SetConditionEffectTimed : Behavior
    {
        ConditionEffectIndex eff;
        int time;
        public SetConditionEffectTimed(ConditionEffectIndex eff, int time)
        {
            this.eff = eff;
            this.time = time;
        }

        protected override bool TickCore(RealmTime time)
        {
            Host.Self.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = eff,
                DurationMS = this.time
            });
            return true;
        }
    }
    class SetConditionEffect : Behavior
    {
        ConditionEffectIndex eff;
        private SetConditionEffect(ConditionEffectIndex eff)
        {
            this.eff = eff;
        }
        static readonly Dictionary<ConditionEffectIndex, SetConditionEffect> instances = new Dictionary<ConditionEffectIndex, SetConditionEffect>();
        public static SetConditionEffect Instance(ConditionEffectIndex eff)
        {
            SetConditionEffect ret;
            if (!instances.TryGetValue(eff, out ret))
                ret = instances[eff] = new SetConditionEffect(eff);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            Host.Self.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = eff,
                DurationMS = -1
            });
            return true;
        }
    }
    class UnsetConditionEffect : Behavior
    {
        ConditionEffectIndex eff;
        private UnsetConditionEffect(ConditionEffectIndex eff)
        {
            this.eff = eff;
        }
        static readonly Dictionary<ConditionEffectIndex, UnsetConditionEffect> instances = new Dictionary<ConditionEffectIndex, UnsetConditionEffect>();
        public static UnsetConditionEffect Instance(ConditionEffectIndex eff)
        {
            UnsetConditionEffect ret;
            if (!instances.TryGetValue(eff, out ret))
                ret = instances[eff] = new UnsetConditionEffect(eff);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            Host.Self.ApplyConditionEffect(new ConditionEffect()
            {
                Effect = eff,
                DurationMS = 0
            });
            return true;
        }
    }
}
