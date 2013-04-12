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
        static _ EntAncient = Behav()
            .Init(0x091f, Behaves("Ent Ancient",
                    IfExist.Instance(-1,
                        IfGreater.Instance(-1, 1,
                            IfEqual.Instance(-1, 3,
                                NullBehavior.Instance,
                                SmoothWandering.Instance(1.5f, 3f)
                            ),
                            SmoothWandering.Instance(1.5f, 2f)
                        ),
                        SmoothWandering.Instance(1.5f, 2f)
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
                                Flashing.Instance(500, 0xff0000ff),
                                Cooldown.Instance(500, MultiAttack.Instance(10, (float)Math.PI, 2, 0 * (float)Math.PI / 180)),
                                Cooldown.Instance(750, MultiAttack.Instance(10, (float)Math.PI, 2, 90 * (float)Math.PI / 180)),
                                new QueuedBehavior(
                                    CooldownExact.Instance(6000),
                                    new SetKey(-1, 3)
                                )
                            )
                        ),
                        IfEqual.Instance(-1, 3,
                            new RunBehaviors(
                                SetConditionEffect.Instance(ConditionEffectIndex.Invincible),
                                new QueuedBehavior(
                                    CooldownExact.Instance(3000),
                                    new Transmute(0x0920)
                                ),
                                Once.Instance(new RunBehaviors(
                                    DamageLesserEqual.Instance(4500,
                                        new RunBehaviors(
                                            new SimpleTaunt("Mmm? Did you say something, mortal?"),
                                            new SetKey(-2, 0)
                                        ),
                                        DamageLesserEqual.Instance(14400,
                                            new RunBehaviors(
                                                new SimpleTaunt("It will be trivial to dispose of you."),
                                                SpawnMinionImmediate.Instance(0x0923, 3, 1, 1),
                                                SpawnMinionImmediate.Instance(0x0921, 3, 2, 2),
                                                new SetKey(-2, 0)
                                            ),
                                            DamageLesserEqual.Instance(37500,
                                                new RunBehaviors(
                                                    new SimpleTaunt("Little flies, little flies... we will swat you."),
                                                    SpawnMinionImmediate.Instance(0x0923, 3, 3, 3),
                                                    SpawnMinionImmediate.Instance(0x0921, 3, 5, 5),
                                                    new SetKey(-2, 0)
                                                ),
                                                new RunBehaviors(
                                                    new SimpleTaunt("You are many, yet the sum of your years is nothing."),
                                                    SpawnMinionImmediate.Instance(0x0923, 3, 6, 6),
                                                    SpawnMinionImmediate.Instance(0x0921, 3, 11, 11),
                                                    new SetKey(-2, 0)
                                                )
                                            )
                                        )
                                    )
                                ))
                            )
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new OnHit(
                            Once.Instance(
                                new RunBehaviors(
                                    new SimpleTaunt("Uhh. So... sleepy..."),
                                    new SetKey(-1, 1),
                                    SetConditionEffect.Instance(ConditionEffectIndex.Invincible)
                                )
                            )
                        )
                    }
                ))
            .Init(0x0920, Behaves("Actual Ent Ancient",
                    SmoothWandering.Instance(0.5f, 2f),
                    new RunBehaviors(
                        IfExist.Instance(-1, NullBehavior.Instance,
                            new RunBehaviors(
                                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                new QueuedBehavior(
                                    CooldownExact.Instance(400),
                                    Once.Instance(new SimpleTaunt("With each attack, I only grow stronger."))
                                ),
                                SetSize.Instance(140),
                                new QueuedBehavior(CooldownExact.Instance(2000), new SetKey(-1, 1)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 0),
                                    RingAttack.Instance(3, projectileIndex: 0)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 1,
                            new RunBehaviors(
            //Once.Instance(new SimpleTaunt("I have withstood far worse.")),
                                SetSize.Instance(160),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 2)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 1),
                                    RingAttack.Instance(3, projectileIndex: 1)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 2,
                            new RunBehaviors(
                                Once.Instance(new RandomTaunt(0.35, "Little mortals, your years are my minutes.")),
                                SetSize.Instance(180),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 3)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 2),
                                    RingAttack.Instance(3, projectileIndex: 2)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 3,
                            new RunBehaviors(
            //Once.Instance(new SimpleTaunt("I am the land, and you are trespassing here.")),
                                SetSize.Instance(200),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 4)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 3),
                                    RingAttack.Instance(3, projectileIndex: 3)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 4,
                            new RunBehaviors(
                                Once.Instance(new RandomTaunt(0.35, "No axe can fell me!")),
                                SetSize.Instance(220),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 5)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 4),
                                    RingAttack.Instance(3, projectileIndex: 4)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 5,
                            new RunBehaviors(
            //Once.Instance(new SimpleTaunt("The forests are mine to command!")),
                                SetSize.Instance(240),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 6)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 5),
                                    RingAttack.Instance(3, projectileIndex: 5)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 6,
                            new RunBehaviors(
                                Once.Instance(new RandomTaunt(0.35, "Yes, YES...")),
                                SetSize.Instance(260),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 7)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 6),
                                    RingAttack.Instance(3, projectileIndex: 6)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 7,
                            new RunBehaviors(
            //Once.Instance(new SimpleTaunt("Cower before me, mortals!")),
                                SetSize.Instance(280),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 8)),
                            Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 7),
                                    RingAttack.Instance(3, projectileIndex: 7)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 8,
                            new RunBehaviors(
                                Once.Instance(new RandomTaunt(0.35, "I am the FOREST!!")),
                                SetSize.Instance(300),
                                new QueuedBehavior(CooldownExact.Instance(1600), new SetKey(-1, 9)),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 8),
                                    RingAttack.Instance(3, projectileIndex: 8)
                                ))
                            )
                        ),
                        IfEqual.Instance(-1, 9,
                            new RunBehaviors(
                                Once.Instance(new SimpleTaunt("YOU WILL DIE!!!")),
                                Once.Instance(OrderGroup.Instance(20, "Greater Nature Sprites",
                                    new Transmute(0x0924)
                                )),
                                SetSize.Instance(320),
                                new QueuedBehavior(
                                    CooldownExact.Instance(4000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                    CooldownExact.Instance(4000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))
                                ),
                                Cooldown.Instance(750, Rand.Instance(
                                    SimpleAttack.Instance(10, projectileIndex: 9),
                                    RingAttack.Instance(3, projectileIndex: 9)
                                ))
                            )
                        )
                    ),
                    If.Instance(
                        EntityLesserThan.Instance(10,5,0x0922),
                        Reproduce.Instance(0x0922, 3, 3000, 1)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 6, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(1, ItemType.Ability)),
                            Tuple.Create(0.1, (ILoot)new TierLoot(2, ItemType.Ability)),
                            Tuple.Create(0.5, (ILoot)PotionLoot.Instance)
                        ),
                        Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.20, (ILoot)new TierLoot(6, ItemType.Weapon)),
                            Tuple.Create(0.12, (ILoot)new TierLoot(7, ItemType.Weapon)),

                            Tuple.Create(0.20, (ILoot)new TierLoot(6, ItemType.Armor)),
                            Tuple.Create(0.12, (ILoot)new TierLoot(7, ItemType.Armor)),

                            Tuple.Create(0.80, (ILoot)new TierLoot(2, ItemType.Ring)),
                            Tuple.Create(0.20, (ILoot)new TierLoot(3, ItemType.Ring)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(3, ItemType.Ability))
                        ))
                    )
                ))
            .Init(0x0922, Behaves("Ent Sapling",
                    IfNot.Instance(
                        Chasing.Instance(5.5f, 10, 8, 0x0920),
                        SimpleWandering.Instance(5.5f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x0921, Behaves("Ent",
                    Chasing.Instance(1f, 10, 0, null),
                    Cooldown.Instance(1000,
                        IfNot.Instance(
                            SimpleAttack.Instance(10),
                            RingAttack.Instance(5)
                        )
                    ),
                    new RunBehaviors(
                        new QueuedBehavior(
                            CooldownExact.Instance(5000),
                            Timed.Instance(2000, False.Instance(
                                new RunBehaviors(
                                    SetSize.Instance(140),
                                    Flashing.Instance(500, 0xff00ff00)
                                )
                            )),
                            CooldownExact.Instance(5000),
                            Timed.Instance(2000, False.Instance(
                                new RunBehaviors(
                                    SetSize.Instance(160),
                                    Flashing.Instance(500, 0xff00ff00)
                                )
                            )),
                            CooldownExact.Instance(5000),
                            Timed.Instance(2000, False.Instance(
                                new RunBehaviors(
                                    SetSize.Instance(180),
                                    Flashing.Instance(500, 0xff00ff00)
                                )
                            )),
                            CooldownExact.Instance(5000),
                            Timed.Instance(2000, False.Instance(
                                new RunBehaviors(
                                    SetSize.Instance(200),
                                    Flashing.Instance(500, 0xff00ff00)
                                )
                            ))
                        ),
                        new QueuedBehavior(
                            CooldownExact.Instance(90000),
                            Despawn.Instance
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Tincture of Dexterity"))
                        ))
                    )
                ))
            .Init(0x0923, Behaves("Greater Nature Sprite",
                    If.Instance(
                        WithinSpawn.Instance(11),
                        Circling.Instance(4, 10, 15, null),
                        ReturnSpawn.Instance(15)
                    ),
                    new RunBehaviors(
                        Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4)),
                        new QueuedBehavior(
                            CooldownExact.Instance(90000),
                            Despawn.Instance
                        ),
                        Reproduce.Instance(0x0921, 5, 3000)
                    )
                ))
            .Init(0x0924, Behaves("Actual Greater Nature Sprite",
                    If.Instance(
                        WithinSpawn.Instance(11),
                        Circling.Instance(4, 10, 15, null),
                        ReturnSpawn.Instance(15)
                    ),
                    new RunBehaviors(
                        Flashing.Instance(1000, 0xff000000),
                        Cooldown.Instance(2000, HealGroup.Instance(20, 500, "Heros")),
                        Cooldown.Instance(5000,
                            OrderGroup.Instance(20, "Heros",
                                new SetConditionEffectTimed(ConditionEffectIndex.Armored, 5000)
                            )
                        ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4)),
                        new QueuedBehavior(
                            CooldownExact.Instance(60000),
                            Despawn.Instance
                        ),
                        Reproduce.Instance(0x0921, 5, 3000)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.25, (ILoot)MpPotionLoot.Instance)
                        ),
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.001, (ILoot)new ItemLoot("Quiver of Thunder")),
                            Tuple.Create(0.06, (ILoot)new ItemLoot("Tincture of Dexterity")),
                            Tuple.Create(0.06, (ILoot)new ItemLoot("Tincture of Life")),
                            Tuple.Create(0.08, (ILoot)new ItemLoot("Green Drake Egg")),
                            Tuple.Create(0.01, (ILoot)new StatPotionLoot(StatPotion.Att)),
                            Tuple.Create(0.03, (ILoot)new TierLoot(7, ItemType.Armor))
                        ))
                    )
                ));
    }
}
