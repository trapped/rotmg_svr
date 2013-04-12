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
        static _ Midland = Behav()
            .Init(0x624, Behaves("Fire Sprite",
                    SimpleWandering.Instance(15),
                    Cooldown.Instance(300, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 2)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x625, Behaves("Ice Sprite",
                    SimpleWandering.Instance(15),
                    Cooldown.Instance(1000, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(2, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x626, Behaves("Magic Sprite",
                    SimpleWandering.Instance(15),
                    Cooldown.Instance(1000, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 4)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x62c, Behaves("Orc King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x629, 2, 1, 3),
                                SpawnMinionImmediate.Instance(0x62a, 2, 1, 2),
                                SpawnMinionImmediate.Instance(0x62b, 2, 1, 2)
                            )
                        ),
                        Rand.Instance(
                            Chasing.Instance(12, 10, 1, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    Rand.Instance(
                        Reproduce.Instance(0x629, 3),
                        Reproduce.Instance(0x62a, 2),
                        Reproduce.Instance(0x62b, 2)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.20, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(5, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(2, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(2, ItemType.Ring))
                        ))
                    )
                ))
            .Init(0x629, Behaves("Orc Warrior",
                    IfNot.Instance(
                        Chasing.Instance(12, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x62a, Behaves("Orc Mage",
                    IfNot.Instance(
                        Circling.Instance(2, 10, 12, 0x62b),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x62b, Behaves("Orc Queen",
                    IfNot.Instance(
                        Chasing.Instance(12, 10, 7, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, Heal.Instance(10, 100, 0x62c)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x62f, Behaves("Earth Golem",
                    IfNot.Instance(
                        Chasing.Instance(8, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, MultiAttack.Instance(3, 7 * (float)Math.PI / 180, 2)),
                        Once.Instance(GolemSatelliteSpawn.Instance)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(2, ItemType.Ring))
                        ))
                    )
                ))
            .Init(0x631, Behaves("Paper Golem",
                    IfNot.Instance(
                        Chasing.Instance(8, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                        Once.Instance(GolemSatelliteSpawn.Instance)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Weapon))
                        )
                    )
                ))

            .Init(0x0205, Behaves("Pink Blob",
                    IfNot.Instance(
                        Chasing.Instance(8, 15, 5, null),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(6, 15 * (float)Math.PI / 180, 3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x020d, Behaves("Gray Blob",
                    new QueuedBehavior(
                        Charge.Instance(25, 15, null),
                        Timed.Instance(5000, SimpleWandering.Instance(2))
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                       new GrayBlob()
                    },
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Magic Mushroom"))
                        ))
                    )
                ))

            .Init(0x020e, Behaves("Big Green Slime",
                    SimpleWandering.Instance(4),
                    Cooldown.Instance(500, SimpleAttack.Instance(10)),
                    condBehaviors: new ConditionalBehavior[]
                    {
                       new DeathTransmute(0x020f, 3, 5)
                    }
                ))
            .Init(0x020f, Behaves("Little Green Slime",
                    SmoothWandering.Instance(2, 4),
                    Cooldown.Instance(500, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x0947, Behaves("Wasp Queen",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x0949, 2, 1, 2),
                                SpawnMinionImmediate.Instance(0x0948, 2, 2, 5)
                            )
                        ),
                        SmoothWandering.Instance(1, 4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    Rand.Instance(
                        Reproduce.Instance(0x0949, 2),
                        Reproduce.Instance(0x0948, 5)
                    )
                ))
            .Init(0x0948, Behaves("Worker Wasp",
                    IfNot.Instance(
                        Circling.Instance(2, 10, 8, 0x0947),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x0949, Behaves("Warrior Wasp",
                    IfNot.Instance(
                        Circling.Instance(2, 10, 8, 0x0947),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor))
                        ),
                        Tuple.Create(1, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(6, ItemType.Weapon)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(6, ItemType.Armor))
                        ))
                    )
                ))

            .Init(0x0950, Behaves("Shambling Sludge",
                    SmoothWandering.Instance(2, 4),
                    new RunBehaviors(
                        new ShamblingSpawn(),
                        Cooldown.Instance(2000, SimpleAttack.Instance(10))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor))
                        ),
                        Tuple.Create(1, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(6, ItemType.Weapon)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(6, ItemType.Armor))
                        ))
                    )
                ))
            .Init(0x0951, Behaves("Sludget",
                    new QueuedBehavior(
                        Timed.Instance(1000, SimpleWandering.Instance(4)),
                        Charge.Instance(10, 5, null)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x0955, Behaves("Swarm",
                    If.Instance(
                        IsEntityNotPresent.Instance(8, null),
                        Charge.Instance(15, 7, null),
                        Timed.Instance(3500, Circling.Instance(5, 10, 20, null))
                    ),
                    Rand.Instance(
                        Cooldown.Instance(500, RingAttack.Instance(5, 10)),
                        Cooldown.Instance(500, RingAttack.Instance(5, 10)),
                        Cooldown.Instance(500, RingAttack.Instance(5, 10)),
                        Cooldown.Instance(1000, SimpleAttack.Instance(10))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.15, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.15, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.15, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.15, (ILoot)new TierLoot(1, ItemType.Ring))
                        )
                    )
                ))

            .Init(0x0204, Behaves("Black Bat",
                    new QueuedBehavior(
                        Charge.Instance(20, 10, null),
                        Timed.Instance(5000, SimpleWandering.Instance(2))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(2, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x0202, Behaves("Red Spider",
                    SimpleWandering.Instance(8),
                    Cooldown.Instance(1000, SimpleAttack.Instance(9)),
                    Reproduce.Instance(0x0202, 3, 45000, 15),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x61f, Behaves("Dwarf King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x61e, 2, 1, 2),
                                SpawnMinionImmediate.Instance(0x61d, 2, 1, 2),
                                SpawnMinionImmediate.Instance(0x61c, 2, 1, 3)
                            )
                        ),
                        new QueuedBehavior(
                            Timed.Instance(2500, Chasing.Instance(10, 15, 1, null)),
                            Timed.Instance(2500, False.Instance(SimpleWandering.Instance(4))),
                            True.Instance(new RandomTaunt(0.01, "You'll taste my axe!"))
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    Rand.Instance(
                        Reproduce.Instance(0x61e, 2),
                        Reproduce.Instance(0x61d, 2),
                        Reproduce.Instance(0x61c, 3)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x61e, Behaves("Dwarf Veteran",
                    IfNot.Instance(
                        Chasing.Instance(10, 10, 1, null),
                        IfNot.Instance(
                            Chasing.Instance(10, 10, 3, 0x61f),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x61d, Behaves("Dwarf Mage",
                    IfNot.Instance(
                        Chasing.Instance(10, 10, 4, null),
                        IfNot.Instance(
                            Chasing.Instance(12, 10, 10, 0x61f),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x61c, Behaves("Dwarf Axebearer",
                    IfNot.Instance(
                        Chasing.Instance(10, 10, 1, null),
                        IfNot.Instance(
                            Chasing.Instance(10, 10, 3, 0x61f),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x637, Behaves("Werelion",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x635, 2, 1, 3),
                                SpawnMinionImmediate.Instance(0x636, 2, 1, 2),
                                SpawnMinionImmediate.Instance(0x639, 2, 1, 2)
                            )
                        ),
                        IfNot.Instance(
                            Chasing.Instance(10, 10, 3, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 3, projectileIndex: 0)),
                        Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 1))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x635, 3),
                        Reproduce.Instance(0x636, 2),
                        Reproduce.Instance(0x639, 2)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x635, Behaves("Weretiger",
                    IfNot.Instance(
                        Rand.Instance(
                            Chasing.Instance(10, 10, 2, null),
                            Circling.Instance(3, 10, 10, null)
                        ),
                        IfNot.Instance(
                            Chasing.Instance(10, 10, 3, 0x637),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x636, Behaves("Wereleopard",
                    IfNot.Instance(
                        Rand.Instance(
                            Chasing.Instance(10, 10, 2, null),
                            Circling.Instance(3, 10, 10, null)
                        ),
                        IfNot.Instance(
                            Chasing.Instance(10, 10, 3, 0x637),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x639, Behaves("Werepanther",
                    IfNot.Instance(
                        new QueuedBehavior(
                            Timed.Instance(7000, False.Instance(Chasing.Instance(10, 10, 2, null))),
                            Planewalk.Instance(5, null)
                        ),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x62d, Behaves("Metal Golem",
                    IfNot.Instance(
                        Chasing.Instance(8, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                        Once.Instance(GolemSatelliteSpawn.Instance)
                    ),
                    Reproduce.Instance(0x62d, 3),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x62e, Behaves("Clockwork Golem",
                    IfNot.Instance(
                        Chasing.Instance(8, 10, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(2000, SimpleAttack.Instance(10)),
                        Once.Instance(GolemSatelliteSpawn.Instance)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(5, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x0945, Behaves("Horned Drake",
                    IfNot.Instance(
                        Once.Instance(
                            SpawnMinionImmediate.Instance(0x0946, 1, 1, 1)
                        ),
                        Rand.Instance(
                            Chasing.Instance(4, 10, 1, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    new QueuedBehavior(
                        Timed.Instance(2000, Cooldown.Instance(500, MultiAttack.Instance(10, 40 * (float)Math.PI / 180, 3))),
                        Timed.Instance(900, Cooldown.Instance(800, SimpleAttack.Instance(10)))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(5, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance),
                            Tuple.Create(0.05, (ILoot)new TierLoot(2, ItemType.Ability))
                        ),
                        Tuple.Create(1, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.05, (ILoot)new TierLoot(6, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(6, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(2, ItemType.Ring)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(3, ItemType.Ring)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(3, ItemType.Ability))
                        ))
                    )
                ))
            .Init(0x0946, Behaves("Drake Baby",
                    IfNot.Instance(
                        Chasing.Instance(6, 10, 1, 0x0945),
                        If.Instance(
                            Escaping.Instance(6, 16, 400, null),
                            new RandomTaunt(0.0001, "Awwwk! Awwwk!"),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))

            .Init(0x6c6, Behaves("Nomadic Shaman",
                    IfNot.Instance(
                        Chasing.Instance(4, 10, 2, null),
                        SimpleWandering.Instance(4)
                    ),
                    new QueuedBehavior(
                        Timed.Instance(1000, Cooldown.Instance(400, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0))),
                        Timed.Instance(1000, Cooldown.Instance(400, SimpleAttack.Instance(10, 1)))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x6c5, Behaves("Sand Phantom",
                    IfNot.Instance(
                        Chasing.Instance(4, 10, 2, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(10)),
                        new QueuedBehavior(
                            Cooldown.Instance(5000),
                            new Transmute(0x7f22)
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x7f22, Behaves("Sand Phantom Wisp",
                    IfNot.Instance(
                        Chasing.Instance(4, 10, 2, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(10)),
                        new QueuedBehavior(
                            Cooldown.Instance(10000),
                            new Transmute(0x6c5)
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x632, Behaves("Fire Golem",
                    IfNot.Instance(
                        Chasing.Instance(10, 7, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    new RunBehaviors(
                        Rand.Instance(
                            Cooldown.Instance(1000, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0)),
                            Cooldown.Instance(1000, SimpleAttack.Instance(10, 1))
                        ),
                        Once.Instance(GolemSatelliteSpawn.Instance)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x633, Behaves("Darkness Golem",
                    IfNot.Instance(
                        Chasing.Instance(10, 7, 1, null),
                        SimpleWandering.Instance(4)
                    ),
                    new QueuedBehavior(
                        Once.Instance(GolemSatelliteSpawn.Instance),
                        Timed.Instance(5000,
                            new RunBehaviors(
                                Flashing.Instance(250, 0xffffffff),
                                Cooldown.Instance(250, SimpleAttack.Instance(10, 0))
                            )
                        ),
                        Timed.Instance(5000,
                            Cooldown.Instance(500, SimpleAttack.Instance(10, 1))
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor))
                        ))
                    )
                ))

            .Init(0x0952, Behaves("Great Lizard",
                    IfNot.Instance(
                        Rand.Instance(
                            Chasing.Instance(10, 10, 2, null),
                            SimpleWandering.Instance(4)
                        ),
                        SimpleWandering.Instance(4)
                    ),
                    new QueuedBehavior(
                        Timed.Instance(5000,
                            Cooldown.Instance(500,
                                Rand.Instance(
                                    SimpleAttack.Instance(10, 0),
                                    MultiAttack.Instance(10, 7 * (float)Math.PI / 180, 2, 1)
                                )
                            )
                        ),
                        Timed.Instance(2100, Cooldown.Instance(250, RingAttack.Instance(20, projectileIndex: 0))),
                        Planewalk.Instance(8, null),
                        Cooldown.Instance(1000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.05, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(5, ItemType.Armor))
                        ),
                        Tuple.Create(1, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.03, (ILoot)new TierLoot(2, ItemType.Ring))
                        ))
                    )
                ))

            .Init(0x622, Behaves("Demon Warg",
                    IfNot.Instance(
                        Chasing.Instance(4, 10, 2, 0x623),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10))
                ))
            .Init(0x620, Behaves("Tawny Warg",
                    IfNot.Instance(
                        Chasing.Instance(4, 10, 2, 0x623),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10))
                ))
            .Init(0x623, Behaves("Desert Werewolf",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x622, 2, 1, 3),
                                SpawnMinionImmediate.Instance(0x620, 2, 1, 3)
                            )
                        ),
                        IfNot.Instance(
                            Chasing.Instance(4, 10, 2, 0x623),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    HpLesser.Instance(200,
                        new RunBehaviors(
                            Once.Instance(SetSize.Instance(150)),
                            Once.Instance(new SimpleTaunt("GRRRRAAGH!")),
                            Flashing.Instance(500, 0xffff0000),
                            Cooldown.Instance(250, SimpleAttack.Instance(10, 1))
                        ),
                        Cooldown.Instance(500, SimpleAttack.Instance(10))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x622, 3),
                        Reproduce.Instance(0x620, 3)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(1, ItemType.Ring))
                        )
                    )
                ));
    }
}
