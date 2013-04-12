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
        static LootDef CommonLesserGodSoulBag =
            new LootDef(0, 2, 0, 8,
                Tuple.Create(0.020, (ILoot)new TierLoot(5, ItemType.Weapon)),
                Tuple.Create(0.010, (ILoot)new TierLoot(6, ItemType.Weapon)),
                Tuple.Create(0.005, (ILoot)new TierLoot(7, ItemType.Weapon)),
                Tuple.Create(0.020, (ILoot)new TierLoot(5, ItemType.Armor)),
                Tuple.Create(0.010, (ILoot)new TierLoot(6, ItemType.Armor)),
                Tuple.Create(0.005, (ILoot)new TierLoot(7, ItemType.Armor)),
                Tuple.Create(0.015, (ILoot)new TierLoot(3, ItemType.Ring)),
                Tuple.Create(0.015, (ILoot)new TierLoot(3, ItemType.Ability))
            );

        static _ Highland = Behav()
            .Init(0x646, Behaves("Minotaur",
                    IfNot.Instance(
                        Rand.Instance(
                            Chasing.Instance(10, 10, 1, null),
                            SimpleWandering.Instance(4)
                        ),
                        SimpleWandering.Instance(4)
                    ),
                    Rand.Instance(
                        Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                        Cooldown.Instance(500, RingAttack.Instance(20, projectileIndex: 0)),
                        Cooldown.Instance(500, Charge.Instance(20, 10, null))
                    ),
                    Reproduce.Instance(0x646, 2),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonLesserGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Purple Drake Egg"))
                        ))
                    )
                ))
            .Init(0x645, Behaves("Ogre King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x644, 3, 1, 2),
                                SpawnMinionImmediate.Instance(0x643, 3, 2, 4),
                                SpawnMinionImmediate.Instance(0x642, 3, 1, 2)
                            )
                        ),
                        IfNot.Instance(
                            Rand.Instance(
                                MaintainDist.Instance(4, 10, 8, null),
                                SimpleWandering.Instance(4)
                            ),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                        Cooldown.Instance(500, ThrowAttack.Instance(2, 5, 55))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x644, 2),
                        Reproduce.Instance(0x643, 4),
                        Reproduce.Instance(0x642, 2)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)new TierLoot(4, ItemType.Weapon))
                        )
                    )
                ))
            .Init(0x644, Behaves("Ogre Wizard",
                    IfNot.Instance(
                        MaintainDist.Instance(4, 10, 8, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(200, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x643, Behaves("Ogre Warrior",
                    new QueuedBehavior(
                        Timed.Instance(1500, Not.Instance(Chasing.Instance(12, 15, 2, null))),
                        Timed.Instance(1500, Not.Instance(Chasing.Instance(12, 15, 2, 0x645)))
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x642, Behaves("Ogre Mage",
                    IfNot.Instance(
                        MaintainDist.Instance(4, 10, 8, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x650, Behaves("Lizard God",
                    IfNot.Instance(
                        Once.Instance(
                            SpawnMinionImmediate.Instance(0x64f, 2, 1, 1)
                        ),
                        IfNot.Instance(
                            Rand.Instance(
                                Chasing.Instance(10, 10, 1, null),
                                SimpleWandering.Instance(4)
                            ),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 3)),
                    Rand.Instance(
                        Reproduce.Instance(0x64f, 1),
                        Reproduce.Instance(0x650, 2)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonLesserGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Purple Drake Egg"))
                        ))
                    )
                ))
            .Init(0x64f, Behaves("Night Elf King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x64d, 3, 1, 2),
                                SpawnMinionImmediate.Instance(0x64c, 3, 1, 2),
                                SpawnMinionImmediate.Instance(0x64b, 3, 1, 3),
                                SpawnMinionImmediate.Instance(0x64a, 3, 1, 4)
                            )
                        ),
                        IfNot.Instance(
                            Rand.Instance(
                                Chasing.Instance(15, 10, 7, null),
                                SimpleWandering.Instance(4)
                            ),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10)),
                    Rand.Instance(
                        Reproduce.Instance(0x64d, 2),
                        Reproduce.Instance(0x64c, 4),
                        Reproduce.Instance(0x64b, 2),
                        Reproduce.Instance(0x64a, 2)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x64d, Behaves("Night Elf Veteran",
                    IfNot.Instance(
                        Chasing.Instance(12, 10, 7, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x64c, Behaves("Night Elf Mage",
                    IfNot.Instance(
                        Chasing.Instance(12, 10, 7, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x64b, Behaves("Night Elf Warrior",
                    IfNot.Instance(
                        Chasing.Instance(12, 7, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(3, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x64a, Behaves("Night Elf Archer",
                    IfNot.Instance(
                        Chasing.Instance(12, 10, 7, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x640, Behaves("Undead Dwarf God",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x63e, 2, 1, 1),
                                SpawnMinionImmediate.Instance(0x63f, 2, 1, 1)
                            )
                        ),
                        IfNot.Instance(
                            Rand.Instance(
                                Chasing.Instance(10, 10, 1, null),
                                SimpleWandering.Instance(4)
                            ),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(500, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 3, projectileIndex: 0)),
                        Cooldown.Instance(500, PredictiveAttack.Instance(10, 1, projectileIndex: 1))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x63e, 1),
                        Reproduce.Instance(0x63f, 1),
                        Reproduce.Instance(0x640, 2)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonLesserGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Purple Drake Egg"))
                        ))
                    )
                ))
            .Init(0x63f, Behaves("Soulless Dwarf",
                    SimpleWandering.Instance(4),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x63e, Behaves("Undead Dwarf King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x63c, 3, 1, 3),
                                SpawnMinionImmediate.Instance(0x63b, 3, 1, 3),
                                SpawnMinionImmediate.Instance(0x63a, 3, 1, 3)
                            )
                        ),
                        IfNot.Instance(
                            Rand.Instance(
                                Chasing.Instance(10, 10, 1, null),
                                SimpleWandering.Instance(4)
                            ),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    Rand.Instance(
                        Reproduce.Instance(0x63c, 3),
                        Reproduce.Instance(0x63b, 3),
                        Reproduce.Instance(0x63a, 3)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x63c, Behaves("Undead Dwarf Mage",
                    IfNot.Instance(
                        Chasing.Instance(10, 10, 7, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x63b, Behaves("Undead Dwarf Axebearer",
                    IfNot.Instance(
                        Chasing.Instance(10, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x63a, Behaves("Undead Dwarf Warrior",
                    IfNot.Instance(
                        Chasing.Instance(10, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3, 0))
                ))

            .Init(0x649, Behaves("Flayer God",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x647, 2, 1, 4),
                                SpawnMinionImmediate.Instance(0x648, 2, 1, 3)
                            )
                        ),
                        IfNot.Instance(
                            Rand.Instance(
                                Chasing.Instance(10, 10, 1, null),
                                SimpleWandering.Instance(4)
                            ),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)),
                        Cooldown.Instance(1000, PredictiveAttack.Instance(10, 1, projectileIndex: 1))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x647, 4),
                        Reproduce.Instance(0x648, 3),
                        Reproduce.Instance(0x649, 2)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonLesserGodSoulBag),
                        Tuple.Create(360, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Purple Drake Egg"))
                        ))
                    )
                ))
            .Init(0x647, Behaves("Flayer",
                    Chasing.Instance(12, 10, 7, null),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x648, Behaves("Flayer Veteran",
                    Chasing.Instance(12, 10, 7, null),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, 0)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x0903, Behaves("Flamer King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x0904, 1, 5, 5)
                            )
                        ),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(7, 10, 2, null)),
                            Timed.Instance(4000, Not.Instance(new RunBehaviors(
                                Flashing.Instance(200, 0xffffff00),
                                GrowSize.Instance(20, 130),
                                Cooldown.Instance(200, SimpleAttack.Instance(10)),
                                HpLesser.Instance(200,
                                    new RunBehaviors(
                                        RingAttack.Instance(10),
                                        Despawn.Instance
                                    )
                                )
                            ))),
                            Not.Instance(ShrinkSize.Instance(20, 70)),
                            Not.Instance(HpLesser.Instance(200,
                                new RunBehaviors(
                                    RingAttack.Instance(10),
                                    Despawn.Instance
                                )
                            ))
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(2, ItemType.Ring))
                        ))
                    )
                ))
            .Init(0x0904, Behaves("Flamer",
                    new QueuedBehavior(
                        Not.Instance(Chasing.Instance(7, 10, 2, null)),
                        Timed.Instance(4000, Not.Instance(new RunBehaviors(
                            Flashing.Instance(200, 0xffffff00),
                            GrowSize.Instance(20, 130),
                            Cooldown.Instance(200, SimpleAttack.Instance(10)),
                            HpLesser.Instance(200,
                                new RunBehaviors(
                                    RingAttack.Instance(10),
                                    Despawn.Instance
                                )
                            )
                        ))),
                        Not.Instance(ShrinkSize.Instance(20, 70)),
                        Not.Instance(HpLesser.Instance(200,
                            new RunBehaviors(
                                RingAttack.Instance(10),
                                Despawn.Instance
                            )
                        ))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)new TierLoot(5, ItemType.Weapon))
                        )
                    )
                ))

            .Init(0x0206, Behaves("Dragon Egg",
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x0207)
                    }
                ))
            .Init(0x0207, Behaves("White Dragon Whelp",
                    SimpleWandering.Instance(20),
                    Cooldown.Instance(1000, SimpleAttack.Instance(8)),
                    new QueuedBehavior(
                        Cooldown.Instance(60 * 1000),
                        new Transmute(0x0208)
                    )
                ))
            .Init(0x0208, Behaves("Juvenile White Dragon",
                    SimpleWandering.Instance(20),
                    Cooldown.Instance(1000, SimpleAttack.Instance(8)),
                    new QueuedBehavior(
                        Cooldown.Instance(60 * 1000),
                        new Transmute(0x0209)
                    )
                ))
            .Init(0x0209, Behaves("Adult White Dragon",
                    SimpleWandering.Instance(20),
                    Cooldown.Instance(1000, MultiAttack.Instance(5, 7 * (float)Math.PI / 180, 3)),
                    Reproduce.Instance(0x0206, 2, 60 * 1000),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("White Drake Egg"))
                        ))
                    )
                ))

            .Init(0x0900, Behaves("Shield Orc Key",
                    IfNot.Instance(
                        StrictCircling.Instance(2, 6, 0x901),
                        SimpleWandering.Instance(2)
                    ),
                    new RunBehaviors(
                        OrderGroup.Instance(7, "Shield Orcs",
                            new RunBehaviors(
                                Flashing.Instance(10000, 0xff000000),
                                Cooldown.Instance(1000, SimpleAttack.Instance(8)),
                                HpLesserPercent.Instance(.5f,
                                    Cooldown.Instance(500, HealGroup.Instance(7, 250, "Shield Orcs"))
                                )
                            )
                        ),
                        HpLesser.Instance(500,
                            new RunBehaviors(
                                OrderGroup.Instance(7, "Shield Orcs",
                                    Flashing.Instance(1000, 0xffff0000)
                                ),
                                RingAttack.Instance(10),
                                Despawn.Instance
                            )
                        )
                    ),
                    Once.Instance(
                        new RunBehaviors(
                            SpawnMinionImmediate.Instance(0x0901, 1, 1, 1),
                            SpawnMinionImmediate.Instance(0x0902, 1, 2, 3)
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)new TierLoot(3, ItemType.Armor))
                        )
                    )
                ))
            .Init(0x0901, Behaves("Shield Orc Flooder",
                    SimpleWandering.Instance(.5f),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(4, ItemType.Ability))
                        ))
                    )
                ))
            .Init(0x0902, Behaves("Shield Orc Shield",
                    IfNot.Instance(
                        StrictCircling.Instance(2, 6, 0x901),
                        SimpleWandering.Instance(2)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.04, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.01, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(2, ItemType.Ring))
                        ))
                    )
                ))

            .Init(0x0958, Behaves("Left Horizontal Trap",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Cooldown.Instance(10000),
                        Despawn.Instance
                    ),
                    new QueuedBehavior(
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(0 * (float)Math.PI / 180, 0)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(0 * (float)Math.PI / 180, 1)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(0 * (float)Math.PI / 180, 2)
                            )
                        ))
                    )
                ))
            .Init(0x0959, Behaves("Top Vertical Trap",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Cooldown.Instance(10000),
                        Despawn.Instance
                    ),
                    new QueuedBehavior(
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(90 * (float)Math.PI / 180, 0)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(90 * (float)Math.PI / 180, 1)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(90 * (float)Math.PI / 180, 2)
                            )
                        ))
                    )
                ))
            .Init(0x0960, Behaves("45-225 Diagonal Trap",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Cooldown.Instance(10000),
                        Despawn.Instance
                    ),
                    new QueuedBehavior(
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(45 * (float)Math.PI / 180, 0)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(45 * (float)Math.PI / 180, 1)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(45 * (float)Math.PI / 180, 2)
                            )
                        ))
                    )
                ))
            .Init(0x0961, Behaves("135-315 Diagonal Trap",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Cooldown.Instance(10000),
                        Despawn.Instance
                    ),
                    new QueuedBehavior(
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(135 * (float)Math.PI / 180, 0)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(135 * (float)Math.PI / 180, 1)
                            )
                        )),
                        Timed.Instance(1000, False.Instance(
                            Cooldown.Instance(100,
                                AngleAttack.Instance(135 * (float)Math.PI / 180, 2)
                            )
                        ))
                    )
                ))
            .Init(0x0957, Behaves("Urgle",
                    SmoothWandering.Instance(1, 4),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    new QueuedBehavior(
                        IsEntityPresent.Instance(14, null),
                        new RunBehaviors(
                            TossEnemy.Instance(230 * (float)Math.PI / 180, 8, 0x0958),
                            TossEnemy.Instance(180 * (float)Math.PI / 180, 9, 0x0958),
                            TossEnemy.Instance(140 * (float)Math.PI / 180, 8, 0x0958)
                        ),
                        Cooldown.Instance(1000),
                        new RunBehaviors(
                            TossEnemy.Instance(200 * (float)Math.PI / 180, 7, 0x0959),
                            TossEnemy.Instance(240 * (float)Math.PI / 180, 9, 0x0959),
                            TossEnemy.Instance(280 * (float)Math.PI / 180, 9, 0x0959),
                            TossEnemy.Instance(320 * (float)Math.PI / 180, 7, 0x0959)
                        ),
                        Cooldown.Instance(1000),
                        new RunBehaviors(
                            TossEnemy.Instance(45 * (float)Math.PI / 180, 1, 0x0960),
                            TossEnemy.Instance(45 * (float)Math.PI / 180, 6, 0x0960),
                            TossEnemy.Instance(225 * (float)Math.PI / 180, 10, 0x0960),
                            TossEnemy.Instance(225 * (float)Math.PI / 180, 5, 0x0960),

                            TossEnemy.Instance(135 * (float)Math.PI / 180, 1, 0x0961),
                            TossEnemy.Instance(135 * (float)Math.PI / 180, 6, 0x0961),
                            TossEnemy.Instance(315 * (float)Math.PI / 180, 10, 0x0961),
                            TossEnemy.Instance(315 * (float)Math.PI / 180, 5, 0x0961)
                        ),
                        Cooldown.Instance(3400)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(1, CommonLesserGodSoulBag)
                    )
                ));
    }
}
