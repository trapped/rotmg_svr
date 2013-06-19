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
        static _ Pcave = Behav()
            .Init(0x0927, Behaves("Dreadstump the Pirate King",
                SmoothWandering.Instance(2f, 2f),
                  HpGreaterEqual.Instance(640, NullBehavior.Instance),
                    HpLesserPercent.Instance(0.8f,
                        new RunBehaviors(
                            Cooldown.Instance(5000, new SimpleTaunt("arrr...")),
                            Cooldown.Instance(10000, new SimpleTaunt("eat cannonballs!")),
                            Cooldown.Instance(15000, new SimpleTaunt(" I will drink my rum out of your skull!")),
                            Cooldown.Instance(500, SimpleAttack.Instance(10, projectileIndex: 0)),
                            Cooldown.Instance(500, SimpleAttack.Instance(10, projectileIndex: 1))
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                            Tuple.Create(100, new LootDef(0, 1, 0, 8,

                            Tuple.Create(0.01, (ILoot)new ItemLoot("Pirate Rum")),

                            Tuple.Create(0.15, (ILoot)new TierLoot(1, ItemType.Armor)),
                            Tuple.Create(0.5, (ILoot)new TierLoot(1, ItemType.Ring)),

                            Tuple.Create(0.20, (ILoot)new TierLoot(2, ItemType.Armor)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Weapon)),

                            Tuple.Create(0.25, (ILoot)new TierLoot(3, ItemType.Armor)),
                            Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Weapon)),

                            Tuple.Create(0.30, (ILoot)new TierLoot(4, ItemType.Armor)),
                            Tuple.Create(0.4, (ILoot)new TierLoot(4, ItemType.Weapon))
                            )))
            ))
            .Init(0x687, Behaves("Cave Pirate Brawler",
                IfNot.Instance(
                    Chasing.Instance(8, 11, 1, null),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)HpPotionLoot.Instance)
                            ))
            ))
            .Init(0x688, Behaves("Cave Pirate Sailor",
                IfNot.Instance(
                    Chasing.Instance(8, 11, 1, null),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)HpPotionLoot.Instance)
                            ))
            ))
            .Init(0x689, Behaves("Cave Pirate Veteran",
                IfNot.Instance(
                    Chasing.Instance(8, 11, 1, null),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)HpPotionLoot.Instance)
                            ))
            ))
            .Init(0x68f, Behaves("Cave Pirate Cabin Boy",
                new RunBehaviors(
                    SmoothWandering.Instance(2f, 2f)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Weapon))
                            ))
            ))
            .Init(0x68e, Behaves("Cave Pirate Hunchback",
                new RunBehaviors(
                    SmoothWandering.Instance(2f, 2f)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Ability))
                            ))
            ))
            .Init(0x68c, Behaves("Cave Pirate Macaw",
                new RunBehaviors(
                    SmoothWandering.Instance(2f, 2f)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Ability))
                            ))
            ))
            .Init(0x68a, Behaves("Cave Pirate Moll",
                new RunBehaviors(
                    SmoothWandering.Instance(2f, 2f)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Ability))
                            ))
            ))
            .Init(0x68d, Behaves("Cave Pirate Monkey",
                new RunBehaviors(
                    SmoothWandering.Instance(2f, 2f)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Ability))
                            ))
            ))
            .Init(0x68b, Behaves("Cave Pirate Parrot",
                new RunBehaviors(
                    SmoothWandering.Instance(2f, 2f)
                    ),
                    loot: new LootBehavior(
                            new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Ability))
                            ))
            ))
            .Init(0x686, Behaves("Pirate Admiral",
                IfNot.Instance(
                    Circling.Instance(3, 10, 6, 0x0927),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                            Tuple.Create(100, new LootDef(0, 1, 0, 8,
                                Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(3, ItemType.Weapon)),
                                Tuple.Create(0.01, (ILoot)new ItemLoot("Pirate Rum"))
                           )))
            ))
            .Init(0x685, Behaves("Pirate Captain",
                IfNot.Instance(
                    Circling.Instance(3, 10, 6, 0x0927),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 1, 0, 8,
                           Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Armor)),
                           Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Ring)),
                           Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Weapon)),
                           Tuple.Create(0.1, (ILoot)new TierLoot(3, ItemType.Weapon)),
                           Tuple.Create(0.01, (ILoot)new ItemLoot("Pirate Rum"))
                           )))
            ))
            .Init(0x684, Behaves("Pirate Commander",
                IfNot.Instance(
                    Circling.Instance(3, 10, 6, 0x0927),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 1, 0, 8,
                           Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Armor)),
                           Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Ring)),
                           Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Weapon)),
                           Tuple.Create(0.1, (ILoot)new TierLoot(3, ItemType.Weapon)),
                           Tuple.Create(0.01, (ILoot)new ItemLoot("Pirate Rum"))
                           )))
            ))
            .Init(0x683, Behaves("Pirate Lieutenant",
                IfNot.Instance(
                    Circling.Instance(3, 10, 6, 0x0927),
                    SimpleWandering.Instance(2)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 1, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 1, 0, 8,
                           Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Armor)),
                           Tuple.Create(0.3, (ILoot)new TierLoot(1, ItemType.Ring)),
                           Tuple.Create(0.2, (ILoot)new TierLoot(2, ItemType.Weapon)),
                           Tuple.Create(0.1, (ILoot)new TierLoot(3, ItemType.Weapon)),
                           Tuple.Create(0.01, (ILoot)new ItemLoot("Pirate Rum"))
                           )))
            ));
    }
}
