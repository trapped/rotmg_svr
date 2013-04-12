using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.logic.cond
{
    class SumoMaster : ConditionalBehavior
    {
        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.OnHit | BehaviorCondition.Other; }
        }

        protected override bool ConditionMeetCore()
        {
            return true;
        }

        protected override void BehaveCore(BehaviorCondition cond, RealmTime? time, object state)
        {
            bool yes = Host.StateStorage.ContainsKey(42);
            int val = 0;
            if (yes)
                val = (int)Host.StateStorage[42];
            if (cond == BehaviorCondition.OnHit)
            {
                if (!yes)
                    Host.StateStorage[42] = val = 1;

                int hp = (Host as Enemy).HP;
                if (hp < 170 && val > 0)
                {
                    Taunt("Engaging Super-Mode!!!");
                    Host.Self.Owner.BroadcastPacket(new ShowEffectPacket()
                    {
                        EffectType = EffectType.Flashing,
                        PosA = new Position() { X = 3, Y = 1000000 },
                        TargetId = Host.Self.Id,
                        Color = new ARGB(0xffffff00)
                    }, null);
                    Host.StateStorage[42] = -1;
                }
            }
            else
            {
                var host = Host as Enemy;
                if (val > 0 && val <= 250)
                {
                    if (val == 1)
                    {
                        Random rand = new Random();
                        int count = rand.Next(4, 8);
                        System.Diagnostics.Debug.WriteLine(count);
                        for (int i = 0; i < count; i++)
                        {
                            Entity entity = Entity.Resolve(0x7f01);
                            entity.Move(Host.Self.X, Host.Self.Y);
                            Host.Self.Owner.EnterWorld(entity);
                        }
                    }
                    val += time.Value.thisTickTimes;
                    Host.StateStorage[42] = val;

                    if (host.AltTextureIndex != 2)
                    {
                        host.AltTextureIndex = 2;
                        host.UpdateCount++;
                    }
                }
                else if (val > 250)
                {
                    if (host.AltTextureIndex != 1)
                    {
                        host.AltTextureIndex = 1;
                        host.UpdateCount++;
                    }
                }
                else if (val < 0)
                {
                    if (host.AltTextureIndex != 4)
                    {
                        host.AltTextureIndex = 4;
                        host.UpdateCount++;
                    }
                }
                else
                {
                    int oldIndex = host.AltTextureIndex;
                    int newIndex = 0 + ((int)((time.Value.tickTimes / 1000) % 2) * 3);
                    if (newIndex != oldIndex)
                    {
                        host.AltTextureIndex = newIndex;
                        host.UpdateCount++;
                    }
                }
            }
        }
    }
}
