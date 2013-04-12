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
        static _ Lowland = Behav()
            .Init(0x617, Behaves("Hobbit Mage",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x616, 2, 2, 4),
                                SpawnMinionImmediate.Instance(0x615, 2, 2, 3)
                            )
                        ),
                        IfNot.Instance(
                            Chasing.Instance(7.5f, 6, 4, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    new QueuedBehavior(
                        RingAttack.Instance(15, offset: 0, projectileIndex: 0),
                        Cooldown.Instance(400),
                        RingAttack.Instance(15, offset: 8 * (float)Math.PI / 180, projectileIndex: 1),
                        Cooldown.Instance(400),
                        RingAttack.Instance(15, offset: 16 * (float)Math.PI / 180, projectileIndex: 2),
                        Cooldown.Instance(400)
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x616, 4, 12000),
                        Reproduce.Instance(0x615, 3, 6000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Weapon)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Armor)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x616, Behaves("Hobbit Archer",
                    IfNot.Instance(
                        Chasing.Instance(11, 8, 6, 0x617),
                        IfNot.Instance(
                            Chasing.Instance(8, 8, 4, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x615, Behaves("Hobbit Rogue",
                    IfNot.Instance(
                        Chasing.Instance(12, 9, 6, 0x617),
                        IfNot.Instance(
                            Chasing.Instance(8.5f, 8, 1, null),
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
            .Init(0x61a, Behaves("Undead Hobbit Mage",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x619, 2, 2, 4),
                                SpawnMinionImmediate.Instance(0x618, 2, 2, 3)
                            )
                        ),
                        IfNot.Instance(
                            Chasing.Instance(7.5f, 6, 4, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    IfNot.Instance(
                        Cooldown.Instance(1000, SimpleAttack.Instance(10, 3)),
                        new QueuedBehavior(
                            RingAttack.Instance(15, offset: 0, projectileIndex: 0),
                            Cooldown.Instance(400),
                            RingAttack.Instance(15, offset: 8 * (float)Math.PI / 180, projectileIndex: 1),
                            Cooldown.Instance(400),
                            RingAttack.Instance(15, offset: 16 * (float)Math.PI / 180, projectileIndex: 2),
                            Cooldown.Instance(400)
                        )
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x616, 4, 12000),
                        Reproduce.Instance(0x615, 3, 6000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.20, (ILoot)new TierLoot(2, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(2, ItemType.Armor)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x619, Behaves("Undead Hobbit Archer",
                    IfNot.Instance(
                        Chasing.Instance(11, 8, 6, 0x61a),
                        IfNot.Instance(
                            Chasing.Instance(8, 8, 4, null),
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
            .Init(0x618, Behaves("Undead Hobbit Rogue",
                    IfNot.Instance(
                        Chasing.Instance(12, 9, 6, 0x61a),
                        IfNot.Instance(
                            Chasing.Instance(8.5f, 8, 1, null),
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
            .Init(0x7f00, Behaves("Sumo Master",
                    IfGreater.Instance(42, 250,
                        Chasing.Instance(4, 6, 1, null),
                        IfEqual.Instance(42, -1,
                            Chasing.Instance(6, 6, 1, null)
                        )
                    ),
                    IfEqual.Instance(42, -1,
                        new QueuedBehavior(
                            Cooldown.Instance(150, SimpleAttack.Instance(5, 1)),
                            Cooldown.Instance(150, SimpleAttack.Instance(5, 1)),
                            Cooldown.Instance(150, SimpleAttack.Instance(5, 1)),
                            Cooldown.Instance(150, SimpleAttack.Instance(5, 1)),
                            Cooldown.Instance(400)
                        ),
                        Cooldown.Instance(1000, SimpleAttack.Instance(10, 0))
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new SumoMaster()
                    },
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(2, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x7f01, Behaves("Lil Sumo",
                    IfNot.Instance(
                        Circling.Instance(3, 10, 4, 0x7f00),
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

            .Init(0x609, Behaves("Elf Wizard",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x605, 2, 0, 2),
                                SpawnMinionImmediate.Instance(0x606, 2, 1, 4),
                                SpawnMinionImmediate.Instance(0x607, 2, 1, 1)
                            )
                        ),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(6, 8, 2, null)),
                            Not.Instance(Retracting.Instance(6, 5, null))
                        )
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 14 * (float)Math.PI / 180, 3)),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x605, 2, 15000),
                        Reproduce.Instance(0x606, 4, 7000),
                        Reproduce.Instance(0x607, 1, 8000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.20, (ILoot)new TierLoot(2, ItemType.Weapon)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(2, ItemType.Armor)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x605, Behaves("Elf Archer",
                    IfNot.Instance(
                        Circling.Instance(6, 10, 5, 0x609),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x606, Behaves("Elf Swordsman",
                    IfNot.Instance(
                        Chasing.Instance(4, 3, 1, null),
                        IfNot.Instance(
                            Chasing.Instance(6, 10, 6, 0x609),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x607, Behaves("Elf Mage",
                    IfNot.Instance(
                        Chasing.Instance(6, 10, 6, 0x609),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(300, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x614, Behaves("Goblin Mage",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x612, 2, 3, 7),
                                SpawnMinionImmediate.Instance(0x613, 2, 3, 7)
                            )
                        ),
                        IfNot.Instance(
                            Escaping.Instance(5, 6, 182, null),
                            IfNot.Instance(
                                Chasing.Instance(5, 8, 6, null),
                                SimpleWandering.Instance(4)
                            )
                        )
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, SimpleAttack.Instance(10, 0)),
                        Cooldown.Instance(1000, SimpleAttack.Instance(10, 1))
                    ),
                    Rand.Instance(
                        Reproduce.Instance(0x612, 7, 12000),
                        Reproduce.Instance(0x613, 7, 12000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.20, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x612, Behaves("Goblin Rogue",
                    new QueuedBehavior(
                        Timed.Instance(2500, Not.Instance(Chasing.Instance(8, 8, 2, null))),
                        Timed.Instance(500, False.Instance(Circling.Instance(2, 10, 8, 0x614)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x613, Behaves("Goblin Warrior",
                    new QueuedBehavior(
                        Timed.Instance(2500, Not.Instance(Chasing.Instance(8, 8, 2, null))),
                        Timed.Instance(500, False.Instance(Circling.Instance(2, 10, 8, 0x614)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x6c8, Behaves("Easily Enraged Bunny",
                    Chasing.Instance(6, 10, 0, null),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x6c9)
                    }
                ))
            .Init(0x6c9, Behaves("Enraged Bunny",
                    new RunBehaviors(
                        Chasing.Instance(8, 10, 0, null),
                        new QueuedBehavior(
                            Flashing.Instance(1000, 0xffffff00),
                            Flashing.Instance(1000, 0xffff0000)
                        )
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(8)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x6d1, Behaves("Forest Nymph",
                    new QueuedBehavior(
                        Timed.Instance(2000, False.Instance(Circling.Instance(3, 10, 8, null))),
                        Timed.Instance(2000, False.Instance(SimpleWandering.Instance(6)))
                    ),
                    Cooldown.Instance(1500,
                        Rand.Instance(
                            SimpleAttack.Instance(10, projectileIndex: 0),
                            RingAttack.Instance(6, 10, projectileIndex: 1)
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x60d, Behaves("Sandsman King",
                    IfNot.Instance(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x60a, 2, 2, 4),
                                SpawnMinionImmediate.Instance(0x60c, 2, 2, 5)
                            )
                        ),
                        IfNot.Instance(
                            Chasing.Instance(6, 10, 4, null),
                            SimpleWandering.Instance(4)
                        )
                    ),
                    Cooldown.Instance(10000, SimpleAttack.Instance(10)),
                    Rand.Instance(
                        Reproduce.Instance(0x60a, 4, 10000),
                        Reproduce.Instance(0x60c, 5, 8000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x60a, Behaves("Sandsman Archer",
                    IfNot.Instance(
                        Chasing.Instance(8, 10, 6, 0x60d),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x60c, Behaves("Sandsman Sorcerer",
                    IfNot.Instance(
                        Chasing.Instance(12, 10, 6, 0x60d),
                        SimpleWandering.Instance(4)
                    ),
                    Rand.Instance(
                        Cooldown.Instance(5000, SimpleAttack.Instance(10, 0)),
                        Cooldown.Instance(400, SimpleAttack.Instance(5, 1))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x6c4, Behaves("Giant Crab",
                    new QueuedBehavior(
                        Timed.Instance(1000, False.Instance(IfNot.Instance(
                            Chasing.Instance(8, 10, 2, null),
                            SimpleWandering.Instance(4)
                        ))),
                        Cooldown.Instance(1000)
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, new RunBehaviors(
                            SimpleAttack.Instance(10, 0),
                            SimpleAttack.Instance(10, 1),
                            SimpleAttack.Instance(10, 2),
                            SimpleAttack.Instance(10, 3)
                        )),
                        Cooldown.Instance(1000, new RunBehaviors(
                            SimpleAttack.Instance(10, 0),
                            SimpleAttack.Instance(10, 1),
                            SimpleAttack.Instance(10, 2),
                            SimpleAttack.Instance(10, 3)
                        )),
                        Cooldown.Instance(1000, new RunBehaviors(
                            SimpleAttack.Instance(10, 0),
                            SimpleAttack.Instance(10, 1),
                            SimpleAttack.Instance(10, 2),
                            SimpleAttack.Instance(10, 3)
                        )),
                        Cooldown.Instance(1000, new RunBehaviors(
                            SimpleAttack.Instance(10, 0),
                            SimpleAttack.Instance(10, 1),
                            SimpleAttack.Instance(10, 2),
                            SimpleAttack.Instance(10, 3)
                        )),
                        Cooldown.Instance(1000, new RunBehaviors(
                            SimpleAttack.Instance(10, 0),
                            SimpleAttack.Instance(10, 1),
                            SimpleAttack.Instance(10, 2),
                            SimpleAttack.Instance(10, 3)
                        )),
                        Cooldown.Instance(500, SimpleAttack.Instance(10, 4)),
                        Cooldown.Instance(500, SimpleAttack.Instance(10, 4)),
                        Cooldown.Instance(500, SimpleAttack.Instance(10, 4))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(2, ItemType.Weapon)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(2, ItemType.Armor)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ring)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x6c7, Behaves("Sand Devil",
                    IfNot.Instance(
                        Timed.Instance(2500, Circling.Instance(2, 10, 8, null)),
                        Timed.Instance(1000, SimpleWandering.Instance(8))
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ));
    }
}
