using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.attack;
using wServer.logic.movement;
using wServer.logic.loot;
using wServer.logic.taunt;
using wServer.logic.cond;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        static _ Pentaract = Behav()
            .Init(0x0d5f, Behaves("Pentaract",
                    Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invincible)),
                    CooldownExact.Instance(250, new PentaractStar()),
                    CooldownExact.Instance(15000, OrderAllEntity.Instance(30, 0x0d60,
                        new Transmute(0x0d5e)
                    ))
                ))
            .Init(0x0d5e, Behaves("Pentaract Tower",
                    Cooldown.Instance(5000, ThrowAttack.Instance(4, 8, 100)),
                    Once.Instance(SpawnMinionImmediate.Instance(0x0d5d, 1, 5, 5)),
                    condBehaviors: new ConditionalBehavior[]{
                        new Corpse(0x0d60)
                    }
                ))
            .Init(0x0d60, Behaves("Pentaract Tower Corpse",
                    Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invincible)),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.001, (ILoot)new ItemLoot("Seal of Blasphemous Prayer")),

                            Tuple.Create(0.005, (ILoot)new TierLoot(11, ItemType.Weapon)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(11, ItemType.Armor)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(5, ItemType.Ring)),

                            Tuple.Create(0.01, (ILoot)new TierLoot(10, ItemType.Weapon)),
                            Tuple.Create(0.01, (ILoot)new TierLoot(10, ItemType.Armor)),

                            Tuple.Create(0.02, (ILoot)new TierLoot(9, ItemType.Weapon)),
                            Tuple.Create(0.02, (ILoot)new TierLoot(5, ItemType.Ability)),
                            Tuple.Create(0.02, (ILoot)new TierLoot(9, ItemType.Armor)),

                            Tuple.Create(0.03, (ILoot)new StatPotionsLoot(1, 2)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(4, ItemType.Ring)),

                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Ability)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(8, ItemType.Armor)),

                            Tuple.Create(0.1, (ILoot)new TierLoot(8, ItemType.Weapon)),
                            Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Armor)),
                            Tuple.Create(0.1, (ILoot)new TierLoot(3, ItemType.Ring))
                        ))
                    )
                ))
            .Init(0x0d5d, Behaves("Pentaract Eye",
                    Swirling.Instance(20, 20),
                    Cooldown.Instance(200, SimpleAttack.Instance(10))
                ));
    }
}
