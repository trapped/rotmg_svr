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
        static _ SnakePit = Behav()
            .Init(0x0917, Behaves("Stheno the Snake Queen",
              new QueuedBehavior(
                    Cooldown.Instance(2000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                    Cooldown.Instance(4000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))
              ),
              new RunBehaviors(
                HpGreaterEqual.Instance(5625,
                  new RunBehaviors(
                    //Cooldown.Instance(2000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                    //Cooldown.Instance(4000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                    Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 90 * (float)Math.PI / 180, projectileIndex: 0)),
                    Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 180 * (float)Math.PI / 180, projectileIndex: 0)),
                    Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 360 * (float)Math.PI / 180, projectileIndex: 0)),
                    Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 270 * (float)Math.PI / 180, projectileIndex: 0))
                    )
                ),
                If.Instance(
                  And.Instance(HpLesser.Instance(5625, NullBehavior.Instance), HpGreaterEqual.Instance(3750, NullBehavior.Instance)),
                    new RunBehaviors(
                      //Cooldown.Instance(5000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                      //Cooldown.Instance(10000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                      Once.Instance(SpawnMinionImmediate.Instance(0x0919, 3, 8, 8)),
                      SmoothWandering.Instance(2f, 3f)
                    )
                ),
                If.Instance(
                  And.Instance(HpLesser.Instance(3750, NullBehavior.Instance), HpGreaterEqual.Instance(1875, NullBehavior.Instance)),
                    new RunBehaviors(
                      Cooldown.Instance(500, RingAttack.Instance(8, 10, 0, projectileIndex: 1)),
                      Cooldown.Instance(500, ThrowAttack.Instance(5, 10, 100))
                      )
                ),
                If.Instance(
                  And.Instance(HpLesser.Instance(1875, NullBehavior.Instance), HpGreaterEqual.Instance(0, NullBehavior.Instance)),
                    new RunBehaviors(
                      //Cooldown.Instance(2000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                      //Cooldown.Instance(4000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                      Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 90 * (float)Math.PI / 180, projectileIndex: 0)),
                      Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 180 * (float)Math.PI / 180, projectileIndex: 0)),
                      Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 360 * (float)Math.PI / 180, projectileIndex: 0)),
                      Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 270 * (float)Math.PI / 180, projectileIndex: 0))
                      )
                   )
                ),
                loot: new LootBehavior(LootDef.Empty,
                  Tuple.Create(100, new LootDef(0, 2, 2, 8,
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Wand of the Bulwark")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Snake Skin Armor")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Snake Skin Shield")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Snake Eye Ring")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                    Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Spd)),
                    Tuple.Create(0.1, (ILoot)new StatPotionLoot(StatPotion.Def)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(9, ItemType.Weapon)),
                    Tuple.Create(0.1, (ILoot)new TierLoot(10, ItemType.Weapon)),
                    Tuple.Create(0.3, (ILoot)new TierLoot(8, ItemType.Armor)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(9, ItemType.Armor)),
                    Tuple.Create(0.1, (ILoot)new TierLoot(10, ItemType.Armor))
                    )))
            ))
            .Init(0x0e26, Behaves("Snakepit Guard",
                SetSize.Instance(100),
                SmoothWandering.Instance(2f, 2f),
                  new RunBehaviors(
                    Cooldown.Instance(2000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 1, 120 * (float)Math.PI / 180, projectileIndex: 2)),
                    Cooldown.Instance(2000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 1, 240 * (float)Math.PI / 180, projectileIndex: 2)),
                    Cooldown.Instance(2000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 1, 360 * (float)Math.PI / 180, projectileIndex: 2)),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 30 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                    Cooldown.Instance(1000, RingAttack.Instance(6, 10, 0, projectileIndex: 1))
                ),
                loot: new LootBehavior(LootDef.Empty,
                  Tuple.Create(100, new LootDef(0, 2, 2, 8,
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Wand of the Bulwark")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Snake Skin Armor")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Snake Skin Shield")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Snake Eye Ring")),
                    Tuple.Create(0.05, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                    Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Spd)),
                    Tuple.Create(0.1, (ILoot)new StatPotionLoot(StatPotion.Def)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(9, ItemType.Weapon)),
                    Tuple.Create(0.1, (ILoot)new TierLoot(10, ItemType.Weapon)),
                    Tuple.Create(0.3, (ILoot)new TierLoot(8, ItemType.Armor)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(9, ItemType.Armor)),
                    Tuple.Create(0.1, (ILoot)new TierLoot(10, ItemType.Armor))
                    )))
            ))
            .Init(0x0918, Behaves("Stheno Pet",
                IfNot.Instance(
                  Circling.Instance(10, 20, 25, 0x0917),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(1000, SimpleAttack.Instance(20, projectileIndex: 0))
            ))
            .Init(0x0919, Behaves("Stheno Swarm",
                IfNot.Instance(
                  Chasing.Instance(4, 100, 2, 0x0917),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(1000, SimpleAttack.Instance(20, projectileIndex: 0))
            ))
            .Init(0x0223, Behaves("Pit Snake",
                SimpleWandering.Instance(8),
                Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0))
            ))
            .Init(0x0224, Behaves("Pit Viper",
                SimpleWandering.Instance(9),
                Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0))
            ))
            .Init(0x0227, Behaves("Yellow Python",
                IfNot.Instance(
                    Chasing.Instance(10, 10, 1, null),
                    SimpleWandering.Instance(8)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Snake Oil")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Ring of Speed")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Ring of Vitality"))
                            ))

            ))
            .Init(0x0226, Behaves("Brown Python",
                SimpleWandering.Instance(5),
                Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                ),
                loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Snake Oil")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Leather Armor")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Ring of Wisdom"))
                            ))
            ))
            .Init(0x0225, Behaves("Fire Python",
                IfNot.Instance(
                    Cooldown.Instance(2000, Charge.Instance(10, 10/*radius*/, null)),
                    SimpleWandering.Instance(8)
                ),
                Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                ),
                loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Snake Oil")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Fire Bow")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Fire Nova Spell"))
                            ))
            ))
            .Init(0x0228, Behaves("Greater Pit Snake",
                IfNot.Instance(
                Chasing.Instance(10, 10, 5, null),
                SimpleWandering.Instance(8)
                ),
                Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Snake Oil")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Glass Sword")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Avenger Staff")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Wand of Dark Magic"))
                            )))
            ))
            .Init(0x0229, Behaves("Greater Pit Viper",
                IfNot.Instance(
                Chasing.Instance(10, 10, 5, null),
                SimpleWandering.Instance(8)
                ),
                Cooldown.Instance(300, SimpleAttack.Instance(10, projectileIndex: 0)
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Snake Oil")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Ring of Greater Attack")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Ring of Greater Health"))
                            )))
            ));
    }
}
