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
        static _ Lich = Behav()
            .Init(0x091b, Behaves("Lich",
                    IfExist.Instance(-1,
                        IfGreater.Instance(-1, 2,
                            NullBehavior.Instance,
                            SmoothWandering.Instance(1.5f, 3f)
                        ),
                        SmoothWandering.Instance(1.5f, 3f)
                    ),
                    new RunBehaviors(
                        IfEqual.Instance(-1, 1,
                            new QueuedBehavior(
                                CooldownExact.Instance(2000),
                                new SetKey(-1, 2)
                            )
                        ),
                        IfEqual.Instance(-1, 2,
                            new RunBehaviors(
                                Once.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invincible)),
                                Flashing.Instance(200, 0xff00ff00),
                                new QueuedBehavior(
                                    MultiAttack.Instance(10, 120 * (float)Math.PI / 180, 3, projectileIndex: 1),
                                    CooldownExact.Instance(200),
                                    MultiAttack.Instance(10, 120 * (float)Math.PI / 180, 3, projectileIndex: 1),
                                    CooldownExact.Instance(1800)
                                ),
                                new QueuedBehavior(
                                    CooldownExact.Instance(6000),
                                    new SetKey(-1, 3)
                                )
                            )
                        ),
                        IfEqual.Instance(-1, 3,
                            new RunBehaviors(
                                new SetKey(-1, 4),
                                Once.Instance(new RunBehaviors(
                                    new SimpleTaunt("Time to meet your future brothers and sisters..."),
                                    DamageLesserEqual.Instance(4500,
                                        new RunBehaviors(
                                            new SimpleTaunt("...It's a small family, but you'll enjoy being part of it!"),
                                            new SetKey(-2, 0)
                                        ),
                                        DamageLesserEqual.Instance(14400,
                                            new RunBehaviors(
                                                new SimpleTaunt("...and there's more where they came from!"),
                                                new SetKey(-2, 1)
                                            ),
                                            DamageLesserEqual.Instance(37500,
                                                new RunBehaviors(
                                                    new SimpleTaunt("...there's a lot of them! Hahaha!!"),
                                                    new SetKey(-2, 2)
                                                ),
                                                new RunBehaviors(
                                                    new SimpleTaunt("... there's an ARMY of them! HahaHahaaaa!!!"),
                                                    new SetKey(-2, 3)
                                                )
                                            )
                                        )
                                    )
                                ))
                            )
                        ),
                        IfEqual.Instance(-2, 0,
                            new QueuedBehavior(
                                CooldownExact.Instance(3000),
                                new Transmute(0x091c)
                            )
                        ),
                        IfEqual.Instance(-2, 1,
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x091d),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x091d),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x091d),
                                TossEnemy.Instance(0, 5, 0x091e),
                                CooldownExact.Instance(6000),
                                new Transmute(0x091c)
                            )
                        ),
                        IfEqual.Instance(-2, 2,
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 4, 0x091d),
                                TossEnemy.Instance(72 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(144 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(216 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(288 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(0, 4, 0x091e),
                                TossEnemy.Instance(144 * (float)Math.PI / 180, 4, 0x091e),
                                TossEnemy.Instance(288 * (float)Math.PI / 180, 4, 0x091e),
                                CooldownExact.Instance(6000),
                                new Transmute(0x091c)
                            )
                        ),
                        IfEqual.Instance(-2, 3,
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 4, 0x091d),
                                TossEnemy.Instance(72 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(144 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(216 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(288 * (float)Math.PI / 180, 4, 0x091d),
                                TossEnemy.Instance(0, 4, 0x091e),
                                TossEnemy.Instance(144 * (float)Math.PI / 180, 4, 0x091e),
                                TossEnemy.Instance(288 * (float)Math.PI / 180, 4, 0x091e),
                                CooldownExact.Instance(5000),
                                EntityLesserThan.Instance(10, 4, 0x91d),
                                CooldownExact.Instance(3000),
                                new SimpleTaunt("My minions have stolen your life force and fed it to me!!"),
                                new SetKey(-2, 4)
                            )
                        ),
                        IfEqual.Instance(-2, 4,
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x091d),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x091d),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x091d),
                                CooldownExact.Instance(6000),
                                new Transmute(0x091c)
                            )
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new OnHit(
                            Once.Instance(
                                new RunBehaviors(
                                    new SimpleTaunt("New recruits for my undead army? How delightful!"),
                                    new SetKey(-1, 1),
                                    SetConditionEffect.Instance(ConditionEffectIndex.Invincible)
                                )
                            )
                        )
                    }
                ))
            .Init(0x091c, Behaves("Actual Lich",
                    SmoothWandering.Instance(1.5f, 3f),
                    Cooldown.Instance(500, Rand.Instance(
                         MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 2, projectileIndex: 0),
                         MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, projectileIndex: 1)
                    )),
                    new QueuedBehavior(
                        True.Instance(Once.Instance(new SimpleTaunt("Kneel before me! I am the master of life and death!"))),
                        SpawnMinionImmediate.Instance(0x660, 1, 1, 1),
                        SpawnMinionImmediate.Instance(0x661, 1, 1, 2),
                        SpawnMinionImmediate.Instance(0x65f, 1, 2, 3),
                        CooldownExact.Instance(5000),
                        Rand.Instance(
                            new RandomTaunt(0.1, "All that I touch turns to dust!"),
                            new RandomTaunt(0.1, "You will drown in a sea of undead!")
                        )
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 4, 0, 8,
                            Tuple.Create(0.01, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.01, (ILoot)new TierLoot(2, ItemType.Ability)),
                            Tuple.Create(0.01, (ILoot)new TierLoot(5, ItemType.Weapon)),
                            Tuple.Create(0.01, (ILoot)new TierLoot(5, ItemType.Armor)),
                            Tuple.Create(0.5, (ILoot)PotionLoot.Instance)
                        ),
                        Tuple.Create(100, new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.10, (ILoot)new TierLoot(6, ItemType.Weapon)),
                            Tuple.Create(0.06, (ILoot)new TierLoot(7, ItemType.Weapon)),

                            Tuple.Create(0.10, (ILoot)new TierLoot(6, ItemType.Armor)),
                            Tuple.Create(0.06, (ILoot)new TierLoot(7, ItemType.Armor)),

                            Tuple.Create(0.20, (ILoot)new TierLoot(2, ItemType.Ring)),
                            Tuple.Create(0.10, (ILoot)new TierLoot(3, ItemType.Ring)),
                            Tuple.Create(0.01, (ILoot)new TierLoot(3, ItemType.Ability))
                        ))
                    )
                ))
            .Init(0x091d, Behaves("Phylactery Bearer",
                    HpLesser.Instance(500,
                        SimpleWandering.Instance(10f),
                        IfNot.Instance(
                            ChasingGroup.Instance(2, 20, 8, "Heros"),
                            SimpleWandering.Instance(6f)
                        )
                    ),
                    new RunBehaviors(
                        HpLesser.Instance(500,
                            new RunBehaviors(
                                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                new QueuedBehavior(
                                    CooldownExact.Instance(3000),
                                    Die.Instance
                                )
                            )
                        ),
                        Cooldown.Instance(250, HealGroup.Instance(15, 500, "Heros")),
                        Cooldown.Instance(1000, new RandomTaunt(0.01, "We feed the master!")),
                        Cooldown.Instance(500,
                            Rand.Instance(
                                MultiAttack.Instance(10, 120 * (float)Math.PI / 180, 3, projectileIndex: 0),
                                MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3, projectileIndex: 0),
                                PredictiveAttack.Instance(10, 1, projectileIndex: 1)
                            )
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Orange Drake Egg"))
                        ))
                    )
                ))
            .Init(0x091e, Behaves("Haunted Spirit",
                    IfExist.Instance(-1,
                        SimpleWandering.Instance(10f),
                        NullBehavior.Instance
                    ),
                    new RunBehaviors(
                        new QueuedBehavior(
                            new SetKey(-1, 1),
                            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                            Cooldown.Instance(2500),
                            new RemoveKey(-1),
                            UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                            Cooldown.Instance(1500)
                        ),
                        Cooldown.Instance(1000,
                            Rand.Instance(
                                new RandomTaunt(0.01, "Hungry..."),
                                new RandomTaunt(0.01, "XxxXxxxXxXxXxxx...")
                            )
                        ),
                        new QueuedBehavior(
                            SimpleAttack.Instance(10),
                            CooldownExact.Instance(200),
                            MultiAttack.Instance(10, 40 * (float)Math.PI / 180, 3),
                            CooldownExact.Instance(200),
                            SimpleAttack.Instance(10),
                            CooldownExact.Instance(200),
                            MultiAttack.Instance(10, 40 * (float)Math.PI / 180, 3),
                            CooldownExact.Instance(200),
                            SimpleAttack.Instance(10),
                            CooldownExact.Instance(200),
                            MultiAttack.Instance(10, 40 * (float)Math.PI / 180, 3),
                            CooldownExact.Instance(200),
                            SimpleAttack.Instance(10),
                            CooldownExact.Instance(200),
                            MultiAttack.Instance(10, 40 * (float)Math.PI / 180, 3),
                            CooldownExact.Instance(2000)
                        ),
                        new QueuedBehavior(
                            new SetKey(-1, 1),
                            Cooldown.Instance(5000),
                            new RemoveKey(-1),
                            Cooldown.Instance(1000)
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Tincture of Defense")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Tincture of Life")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Tincture of Mana"))
                        ))
                    )
                ))
            .Init(0x65f, Behaves("Mummy",
                    SimpleWandering.Instance(6f),
                    Cooldown.Instance(500, SimpleAttack.Instance(10))
                ))
            .Init(0x660, Behaves("Mummy King",
                    SimpleWandering.Instance(6f),
                    Cooldown.Instance(500, SimpleAttack.Instance(10))
                ))
            .Init(0x661, Behaves("Mummy Pharaoh",
                    SimpleWandering.Instance(6f),
                    Cooldown.Instance(500, SimpleAttack.Instance(10))
                ));
    }
}
