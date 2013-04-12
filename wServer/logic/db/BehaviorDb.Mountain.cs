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
        static LootDef CommonGodSoulBag =
            new LootDef(0, 2, 0, 8,
                Tuple.Create(0.020, (ILoot)new TierLoot(6, ItemType.Weapon)),
                Tuple.Create(0.010, (ILoot)new TierLoot(7, ItemType.Weapon)),
                Tuple.Create(0.005, (ILoot)new TierLoot(8, ItemType.Weapon)),

                Tuple.Create(0.020, (ILoot)new TierLoot(6, ItemType.Armor)),
                Tuple.Create(0.010, (ILoot)new TierLoot(7, ItemType.Armor)),
                Tuple.Create(0.005, (ILoot)new TierLoot(8, ItemType.Armor)),
                Tuple.Create(0.003, (ILoot)new TierLoot(9, ItemType.Armor)),

                Tuple.Create(0.015, (ILoot)new TierLoot(3, ItemType.Ring)),
                Tuple.Create(0.005, (ILoot)new TierLoot(4, ItemType.Ring)),
                Tuple.Create(0.020, (ILoot)new TierLoot(4, ItemType.Ability))
            );
        const double PotProbability = 0.015;

        static _ Mountain = Behav()
            .Init(0x651, Behaves("White Demon",
                    IfNot.Instance(
                        Chasing.Instance(6, 9, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, PredictiveMultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 1)),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Att))
                        ))
                    )
                ))
            .Init(0x652, Behaves("Sprite God",
                    SimpleWandering.Instance(2),
                    new RunBehaviors(
                        Cooldown.Instance(1000, PredictiveMultiAttack.Instance(12, 10 * (float)Math.PI / 180, 4, 1, 0)),
                        Cooldown.Instance(1000, PredictiveAttack.Instance(10, 1, 1))
                    ),
                    IfNot.Instance(
                        Once.Instance(
                            SpawnMinionImmediate.Instance(0x653, 2, 3, 5)
                        ),
                        Reproduce.Instance(0x653, 5)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Att))
                        ))
                    )
                ))
            .Init(0x653, Behaves("Sprite Child",
                    IfNot.Instance(
                        Chasing.Instance(4, 5, 2, 0x652),
                        SimpleWandering.Instance(4)
                    )
                ))
            .Init(0x654, Behaves("Medusa",
                    IfNot.Instance(
                        Chasing.Instance(6, 7, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, MultiAttack.Instance(12, 10 * (float)Math.PI / 180, 5)),
                        Cooldown.Instance(3000, ThrowAttack.Instance(4, 8, 150))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Spd))
                        ))
                    )
                ))
            .Init(0x655, Behaves("Ent God",
                    IfNot.Instance(
                        Chasing.Instance(10, 7, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1250, PredictiveMultiAttack.Instance(12, 10 * (float)Math.PI / 180, 5, 1)),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Def))
                        ))
                    )
                ))
            .Init(0x656, Behaves("Beholder",
                    IfNot.Instance(
                        Chasing.Instance(4, 7, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(750, PredictiveRingAttack.Instance(5, .5f, 12, 0)),
                        Cooldown.Instance(1000, PredictiveAttack.Instance(10, 1, 1))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Def))
                        ))
                    )
                ))
            .Init(0x657, Behaves("Flying Brain",
                    IfNot.Instance(
                        Chasing.Instance(6, 7, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, RingAttack.Instance(5, 12)),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Att))
                        ))
                    )
                ))
            .Init(0x658, Behaves("Slime God",
                    IfNot.Instance(
                        Chasing.Instance(10, 7, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, MultiAttack.Instance(12, 10 * (float)Math.PI / 180, 5, 0)),
                        Cooldown.Instance(650, PredictiveAttack.Instance(10, 1, 1))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Def))
                        ))
                    )
                ))
            .Init(0x659, Behaves("Ghost God",
                    IfNot.Instance(
                        Chasing.Instance(4, 7, 4, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(900, MultiAttack.Instance(12, 25 * (float)Math.PI / 180, 7)),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Spd))
                        ))
                    )
                ))

            .Init(0x0905, Behaves("Rock Bot",
                    Swirling.Instance(2, 5),
                    new RunBehaviors(
                        Cooldown.Instance(1000, SimpleAttack.Instance(15)),
                        Cooldown.Instance(1000, Heal.Instance(5, 2500, 0x0907)),
                        new RandomTaunt(0.0001, "We are impervious to non-mystic attacks.")
                    ),
                    Once.Instance(
                        new RunBehaviors(
                            SpawnMinionImmediate.Instance(0x0906, 2, 1, 1),
                            SpawnMinionImmediate.Instance(0x0907, 2, 1, 1)
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.16, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.16, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.08, (ILoot)new TierLoot(5, ItemType.Armor))
                        ),
                        Tuple.Create(1, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.08, (ILoot)new TierLoot(6, ItemType.Weapon)),
                            Tuple.Create(0.04, (ILoot)new TierLoot(7, ItemType.Weapon)),
                            Tuple.Create(0.04, (ILoot)new TierLoot(6, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(3, ItemType.Ring))
                        )),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Purple Drake Egg")),
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Att))
                        ))
                    )
                ))
            .Init(0x0906, Behaves("Paper Bot",
                    IfNot.Instance(
                        Circling.Instance(4, 10, 5, 0x0905),
                        SimpleWandering.Instance(8)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(800, MultiAttack.Instance(15, 20 * (float)Math.PI / 180, 3)),
                        Cooldown.Instance(1000, Heal.Instance(5, 2500, 0x0905)),
                        HpLesser.Instance(400,
                            new RunBehaviors(
                                RingAttack.Instance(8),
                                Despawn.Instance
                            )
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(6, ItemType.Weapon))
                        )),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Tincture of Life")),
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Att))
                        ))
                    )
                ))
            .Init(0x0907, Behaves("Steel Bot",
                    IfNot.Instance(
                        Circling.Instance(4, 10, 5, 0x0905),
                        SimpleWandering.Instance(8)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(800, MultiAttack.Instance(15, 20 * (float)Math.PI / 180, 3)),
                        Cooldown.Instance(1000, Heal.Instance(5, 2500, 0x0906)),
                        new RandomTaunt(0.0001, "Silly squishy, we heal our brothers in a circle."),
                        HpLesser.Instance(400,
                            new RunBehaviors(
                                RingAttack.Instance(8),
                                Despawn.Instance
                            )
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(6, ItemType.Weapon))
                        )),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Att))
                        ))
                    )
                ))
            .Init(0x091a, Behaves("Djinn",
                    HpGreaterEqual.Instance(400,
                        new QueuedBehavior(
                            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                            Timed.Instance(2500, False.Instance(Chasing.Instance(6, 7, 0, null))),
                            UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                            If.Instance(IsEntityPresent.Instance(10, null),
                                new QueuedBehavior(
                                    Cooldown.Instance(200),
                                    new RunBehaviors(
                                        RingAttack.Instance(4, offset: 0 * (float)Math.PI / 180),
                                        RingAttack.Instance(4, offset: 90 * (float)Math.PI / 180)
                                    ),
                                    Cooldown.Instance(200),
                                    new RunBehaviors(
                                        RingAttack.Instance(4, offset: 10 * (float)Math.PI / 180),
                                        RingAttack.Instance(4, offset: 80 * (float)Math.PI / 180)
                                    ),
                                    Cooldown.Instance(200),
                                    new RunBehaviors(
                                        RingAttack.Instance(4, offset: 20 * (float)Math.PI / 180),
                                        RingAttack.Instance(4, offset: 70 * (float)Math.PI / 180)
                                    ),
                                    Cooldown.Instance(200),
                                    new RunBehaviors(
                                        RingAttack.Instance(4, offset: 30 * (float)Math.PI / 180),
                                        RingAttack.Instance(4, offset: 60 * (float)Math.PI / 180)
                                    ),
                                    Cooldown.Instance(200),
                                    new RunBehaviors(
                                        RingAttack.Instance(4, offset: 40 * (float)Math.PI / 180),
                                        RingAttack.Instance(4, offset: 50 * (float)Math.PI / 180)
                                    ),
                                    Cooldown.Instance(250)
                                )
                            )
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        HpLesserCond.Instance(400,
                            new QueuedBehavior(
                                SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(1000, Flashing.Instance(1500, 0xffff0000)),
                                RingAttack.Instance(12),
                                Die.Instance
                            )
                        )
                    },
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Spd))
                        ))
                    )
                ))
            .Init(0x6d8, Behaves("Leviathan",
                    new QueuedBehavior(
                        MagicEye.Instance,
                        Timed.Instance(2000,
                            Not.Instance(Chasing.Instance(8, 15, 5, null))
                        ),
                        new RemoveKey(Circling.Instance(5, 15, 8, null)),
                        Timed.Instance(2000,
                            False.Instance(Circling.Instance(5, 15, 8, null))
                        ),
                        Timed.Instance(2000,
                           Not.Instance(Tangential.Instance(8))
                        )
                    ),
                    new QueuedBehavior(
                        Timed.Instance(1500,
                            False.Instance(Cooldown.Instance(300,
                                new RunBehaviors(
                                    MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 3, 0 * (float)Math.PI / 180, projectileIndex: 0),
                                    MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 3, 120 * (float)Math.PI / 180, projectileIndex: 0),
                                    MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 3, 240 * (float)Math.PI / 180, projectileIndex: 0)
                                )
                            ))
                        ),
                        Timed.Instance(1500,
                            False.Instance(Cooldown.Instance(300,
                                new RunBehaviors(
                                    MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 4, 60 * (float)Math.PI / 180, projectileIndex: 0),
                                    MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 4, 180 * (float)Math.PI / 180, projectileIndex: 0),
                                    MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 4, 300 * (float)Math.PI / 180, projectileIndex: 0)
                                )
                            ))
                        ),
                        Timed.Instance(1500,
                            False.Instance(Cooldown.Instance(300,
                                PredictiveMultiAttack.Instance(15, 15 * (float)Math.PI / 180, 2, 1, projectileIndex: 1)
                            ))
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Def))
                        ))
                    )
                ))
            .Init(0x0d84, Behaves("Oryx Pet",   //Whoops!!
                    SimpleWandering.Instance(1),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, new LootDef(100, 1, 8, 16,
                            Tuple.Create(0.2, (ILoot)new TierLoot(14, ItemType.Weapon)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(15, ItemType.Armor)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Ability)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(5, ItemType.Ring)),
                            Tuple.Create(0.2, (ILoot)new StatPotionsLoot(1, 2, 3))
                        ))
                    )
                ));
    }
}
