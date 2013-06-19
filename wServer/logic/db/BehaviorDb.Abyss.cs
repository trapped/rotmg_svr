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
        static _ Abyss = Behav()
            /*.Init(0x090a, Behaves("Archdemon Malphas",
                    new RunBehaviors(
                        //Once.Instance(SpawnMinionImmediate.Instance(0x0908, 10, 4, 4)),//spawn "Malphas Protector"
                        HpLesserPercent.Instance(0.999f,
                            Once.Instance(SpawnMinionImmediate.Instance(0x0909, 0, 4, 4))//spawn "Malphas Missile"
                        ),
                        Chasing.Instance(2, 11, 3, null),
                        Cooldown.Instance(750, MultiAttack.Instance(11, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 2))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                         Tuple.Create(100, new LootDef(0, 5, 2, 8,

                            Tuple.Create(0.99, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.99, (ILoot)new StatPotionLoot(StatPotion.Def)),

                            Tuple.Create(0.05, (ILoot)new ItemLoot("Demon Blade")), //5% for Dblade loot

                            Tuple.Create(0.5, (ILoot)new TierLoot(4, ItemType.Ability)),
                            Tuple.Create(0.6, (ILoot)new TierLoot(3, ItemType.Ability)),

                            Tuple.Create(0.15, (ILoot)new TierLoot(10, ItemType.Armor)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(9, ItemType.Armor)),
                            Tuple.Create(0.25, (ILoot)new TierLoot(8, ItemType.Armor)),

                            Tuple.Create(0.2, (ILoot)new TierLoot(10, ItemType.Weapon)),
                            Tuple.Create(0.3, (ILoot)new TierLoot(9, ItemType.Weapon)),

                            Tuple.Create(0.5, (ILoot)new TierLoot(4, ItemType.Ring)),
                            Tuple.Create(0.6, (ILoot)new TierLoot(3, ItemType.Ring))
                    )))
            ))*/
            .Init(0x090a, Behaves("Archdemon Malphas",
                SimpleWandering.Instance(1, .5f),
                    new QueuedBehavior(
                        SimpleWandering.Instance(1, .5f),
                        HpLesser.Instance(50000,
                            new QueuedBehavior(
                                SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(1000, False.Instance(Flashing.Instance(1200, 0xff00ff00))),
                                UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(14000, 
                                False.Instance(
                                    new QueuedBehavior(
                                        SetSize.Instance(100),
                                        Cooldown.Instance(700, PredictiveAttack.Instance(15, 8, projectileIndex: 0)),
                                        Cooldown.Instance(800, RingAttack.Instance(8, 15, 25, projectileIndex: 1)),
                                        Cooldown.Instance(800, RingAttack.Instance(8, 15, 25, projectileIndex: 3)),
                                        Cooldown.Instance(800, RingAttack.Instance(8, 15, 15, projectileIndex: 0)),

                                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                        Timed.Instance(1000, False.Instance(Flashing.Instance(1200, 0xff00ff00))),
                                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                                        Timed.Instance(13000, 
                                        False.Instance(
                                            new QueuedBehavior(
                                                SetSize.Instance(170),
                                                Cooldown.Instance(800, RingAttack.Instance(8, 15, 25, projectileIndex: 3)),
                                                Cooldown.Instance(750, MultiAttack.Instance(15, 0 * (float)Math.PI / 2, 2, projectileIndex: 2)),
                                                Cooldown.Instance(850, PredictiveAttack.Instance(15, 8, projectileIndex: 0)),
                                                Cooldown.Instance(800, RingAttack.Instance(8, 15, 25, projectileIndex: 3)),

                                                SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                                Timed.Instance(1000, False.Instance(Flashing.Instance(1200, 0xff00ff00))),
                                                UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                                                Timed.Instance(12000, 
                                                False.Instance(
                                                    new QueuedBehavior(
                                                        SetSize.Instance(40),
                                                        Cooldown.Instance(870, RingAttack.Instance(9,15,25,projectileIndex: 3)),
                                                        Cooldown.Instance(870, RingAttack.Instance(9,15,25,projectileIndex: 0)),
                                                        Cooldown.Instance(870, RingAttack.Instance(9,15,25,projectileIndex: 1))
                                                        ))
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                ),
                loot: new LootBehavior(LootDef.Empty,
                         Tuple.Create(100, new LootDef(0, 5, 2, 8,

                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Def)),

                            Tuple.Create(0.05, (ILoot)new ItemLoot("Demon Blade")), //5% for Dblade loot

                            Tuple.Create(0.5, (ILoot)new TierLoot(4, ItemType.Ability)),
                            Tuple.Create(0.6, (ILoot)new TierLoot(3, ItemType.Ability)),

                            Tuple.Create(0.15, (ILoot)new TierLoot(10, ItemType.Armor)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(9, ItemType.Armor)),
                            Tuple.Create(0.25, (ILoot)new TierLoot(8, ItemType.Armor)),

                            Tuple.Create(0.2, (ILoot)new TierLoot(10, ItemType.Weapon)),
                            Tuple.Create(0.3, (ILoot)new TierLoot(9, ItemType.Weapon)),

                            Tuple.Create(0.5, (ILoot)new TierLoot(4, ItemType.Ring)),
                            Tuple.Create(0.6, (ILoot)new TierLoot(3, ItemType.Ring))
                    )))
            ))
            .Init(0x0908, Behaves("Malphas Protector",
                new RunBehaviors(
                    Circling.Instance(15, 50, 50, 0x0703),
                    Cooldown.Instance(2000, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0))
                    )
            ))
            .Init(0x0909, Behaves("Malphas Missile",
                new RunBehaviors(
                    Chasing.Instance(10, 11, 0, null),
                    CooldownExact.Instance(5000,
                        new RunBehaviors(
                            Cooldown.Instance(0, Flashing.Instance(500, 0x01FAEBD7)),
                            Cooldown.Instance(550, Flashing.Instance(500, 0x01FAEBD7)),
                            RingAttack.Instance(6, 360, 0, projectileIndex: 0),
                            Despawn.Instance
                            )))
            ))
            .Init(0x671, Behaves("Brute of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 1, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Obsidian Dagger")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Steel Helm")),
                            Tuple.Create(0.01, (ILoot)HpPotionLoot.Instance)
                            )))
            ))
            .Init(0x66d, Behaves("Imp of the Abyss",
                new RunBehaviors(
                    SimpleWandering.Instance(12f),
                    Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 20, 5, 0, projectileIndex: 0))
                ),
                loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Cloak of the Red Agent")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Felwasp Toxin")),
                            Tuple.Create(0.01, (ILoot)PotionLoot.Instance)
                            ))) 
            ))
            .Init(0x672, Behaves("Brute Warrior of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 1, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Glass Sword")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Ring of Greater Dexterity")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Magesteel Quiver")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Spirit Salve Tome"))
                            )))
            ))
            .Init(0x670, Behaves("Demon Mage of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 5, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 20, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Fire Nova Spell")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Wand of Dark Magic")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Avenger Staff")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Robe of the Invoker")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Essence Tap Skull")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Demonhunter Trap"))
                            )))
            ))
            .Init(0x66f, Behaves("Demon Warrior of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 5, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 20, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Fire Sword")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Steel Shield"))
                            )))
            ))
            .Init(0x66e, Behaves("Demon of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 5, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, PredictiveMultiAttack.Instance(100, 1 * (float)Math.PI / 25, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Fire Bow")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Mithril Armor"))
                            )))
            ))
            .Init(0x0e1d, Behaves("Abyss Idol",
                new RunBehaviors(
                    Cooldown.Instance(750, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 1, projectileIndex: 0))
                ),


                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.99, (ILoot)new ItemLoot("Potion of Defense")),
                            Tuple.Create(0.99, (ILoot)new ItemLoot("Potion of Vitality")),
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(0.5, (ILoot)new ItemLoot("Demon Blade"))
                            )))));
    }
}
