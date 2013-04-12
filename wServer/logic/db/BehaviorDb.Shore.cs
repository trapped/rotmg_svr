using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.attack;
using wServer.logic.movement;
using wServer.logic.loot;
using wServer.logic.taunt;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        static _ Shore = Behav()
            .Init(0x600, Behaves("Pirate",
                    IfNot.Instance(
                        Chasing.Instance(8.5f, 6, 0, null), SimpleWandering.Instance(4)),
                    Cooldown.Instance(2500, SimpleAttack.Instance(3)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.2, (ILoot)new TierLoot(1, ItemType.Weapon)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x601, Behaves("Piratess",
                    IfNot.Instance(
                        Once.Instance(
                            SpawnMinionImmediate.Instance(0x600, 3, 2, 5)
                        ),
                        IfNot.Instance(
                            Chasing.Instance(11f, 6, 1, null),
                            SimpleWandering.Instance(6))
                    ),
                    Cooldown.Instance(2500, SimpleAttack.Instance(3)),
                    Rand.Instance(
                        Reproduce.Instance(0x600, 5),
                        Reproduce.Instance(0x601, 5)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.2, (ILoot)new TierLoot(1, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x602, Behaves("Snake",
                    SimpleWandering.Instance(6),
                    Cooldown.Instance(2000, SimpleAttack.Instance(10)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x604, Behaves("Scorpion Queen",
                    IfNot.Instance(
                        Once.Instance(
                            SpawnMinionImmediate.Instance(0x603, 3, 5, 10)
                        ),
                        SmoothWandering.Instance(1, 3)
                    ),
                    reproduce: Reproduce.Instance(0x603, 10),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x603, Behaves("Poison Scorpion",
                    IfNot.Instance(
                        Chasing.Instance(4, 8, 1, 0x604), SimpleWandering.Instance(4)),
                    Cooldown.Instance(2000, SimpleAttack.Instance(8))))
            .Init(0x953, Behaves("Bandit Leader",
                    IfNot.Instance(
                        Once.Instance(
                            SpawnMinionImmediate.Instance(0x0954, 2, 3, 5)
                        ),
                        IfNot.Instance(
                            If.Instance(
                                Escaping.Instance(4, 5, 80, null),
                                True.Instance(Once.Instance(new SimpleTaunt(
                                    "Forget this... run for it!")))
                            ),
                            IfNot.Instance(
                                Chasing.Instance(4, 6, 1, null),
                                SimpleWandering.Instance(4)
                            )
                        )
                    ),
                    Rand.Instance(
                        Cooldown.Instance(1000,
                            SimpleAttack.Instance(8)
                        ),
                        If.Instance(
                            Cooldown.Instance(2500, ThrowAttack.Instance(2, 5, 12)),
                            new RandomTaunt(0.1, "Catch!")
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.2, (ILoot)new TierLoot(1, ItemType.Weapon)),
                            Tuple.Create(0.1, (ILoot)new TierLoot(2, ItemType.Weapon)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(1, ItemType.Armor)),
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x954, Behaves("Bandit",
                    IfNot.Instance(
                        IfNot.Instance(
                            Chasing.Instance(8, 6, 3, 0x953),
                            Chasing.Instance(8, 6, 0, null)
                        ),
                        SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(2000,
                        Rand.Instance(
                            SimpleAttack.Instance(8, 0),
                            SimpleAttack.Instance(8, 1)
                        )
                    )
                ))
            .Init(0x60e, Behaves("Red Gelatinous Cube",
                    SimpleWandering.Instance(4),
                    Cooldown.Instance(1000, MultiAttack.Instance(5, 10 * (float)Math.PI / 180, 2)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x60f, Behaves("Purple Gelatinous Cube",
                    SimpleWandering.Instance(4),
                    Cooldown.Instance(600, SimpleAttack.Instance(5)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x610, Behaves("Green Gelatinous Cube",
                    SimpleWandering.Instance(4),
                    Cooldown.Instance(1800, RingAttack.Instance(5, 5)),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.02, (ILoot)MpPotionLoot.Instance)
                        )
                    )
                ));
    }
}
