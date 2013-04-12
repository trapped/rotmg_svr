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
        static _ GhostKing = Behav()
            .Init(0x0928, Behaves("Ghost King",
                    IfExist.Instance(-1,
                        IfGreater.Instance(-1, 2,
                            NullBehavior.Instance,
                            new QueuedBehavior(
                                Not.Instance(ReturnSpawn.Instance(4)),
                                False.Instance(SmoothWandering.Instance(1.5f, 3))
                            )
                        ),
                        new QueuedBehavior(
                            AngleMove.Instance(4, 180 * (float)Math.PI / 180, 6),
                            AngleMove.Instance(4, 0 * (float)Math.PI / 180, 6)
                        )
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
                                Once.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                Flashing.Instance(200, 0xff00ff00),
                                Cooldown.Instance(1000, MultiAttack.Instance(10, 30 * (float)Math.PI / 180, 4)),
                                new QueuedBehavior(
                                    CooldownExact.Instance(200),
                                    SetSize.Instance(120),
                                    CooldownExact.Instance(200),
                                    SetSize.Instance(140),
                                    CooldownExact.Instance(3000),
                                    new SetKey(-1, 3)
                                )
                            )
                        ),
                        IfEqual.Instance(-1, 3,
                            new RunBehaviors(
                                new SetKey(-1, 4),
                                Once.Instance(new RunBehaviors(
                                    Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                    new SimpleTaunt("Aye, let's be miserable together"),
                                    DamageLesserEqual.Instance(4500,
                                        new RunBehaviors(
                                            new SimpleTaunt("Just you? I guess I don't have any friends to play with."),
                                            new SetKey(-2, 0)
                                        ),
                                        DamageLesserEqual.Instance(14400,
                                            new RunBehaviors(
                                                new SimpleTaunt("Such a small party."),
                                                new SetKey(-2, 1)
                                            ),
                                            DamageLesserEqual.Instance(37500,
                                                new RunBehaviors(
                                                    new SimpleTaunt("There's a MOB of you."),
                                                    new SetKey(-2, 2)
                                                ),
                                                new RunBehaviors(
                                                    new SimpleTaunt("What a HUGE MOB!"),
                                                    new SetKey(-2, 3)
                                                )
                                            )
                                        )
                                    )
                                ))
                            )
                        ),
                        IfEqual.Instance(-2, 0,     //Being alone
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x0929),
                                TossEnemy.Instance(72 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(144 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(216 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(288 * (float)Math.PI / 180, 5, 0x092a),
                                CooldownExact.Instance(3000),
                                new Transmute(0x092d)
                            )
                        ),
                        IfEqual.Instance(-2, 1,     //Partying
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x092b),
                                TossEnemy.Instance(72 * (float)Math.PI / 180, 5, 0x092b),
                                TossEnemy.Instance(144 * (float)Math.PI / 180, 5, 0x092b),
                                TossEnemy.Instance(216 * (float)Math.PI / 180, 5, 0x092b),
                                TossEnemy.Instance(288 * (float)Math.PI / 180, 5, 0x092b),

                                CooldownExact.Instance(5000),
                                EntityGroupLesserThan.Instance(15, 2, "Ghosts"),
                                CooldownExact.Instance(3000),
                                new SimpleTaunt("Misery loves company!"),
                                new SetKey(-2, 4)
                            )
                        ),
                        IfEqual.Instance(-2, 2,     //A mob
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x092a),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),

                                CooldownExact.Instance(5000),
                                EntityGroupLesserThan.Instance(15, 2, "Ghosts"),
                                CooldownExact.Instance(3000),
                                new SimpleTaunt("Misery loves company!"),
                                new SetKey(-2, 5)
                            )
                        ),
                        IfEqual.Instance(-2, 3,     //Huge MOB
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x092a),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),

                                CooldownExact.Instance(5000),
                                EntityGroupLesserThan.Instance(15, 2, "Ghosts"),
                                CooldownExact.Instance(3000),
                                new SimpleTaunt("I feel almost manic!"),
                                new SetKey(-2, 6)
                            )
                        ),
                        IfEqual.Instance(-2, 4,     //Misery 1
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x0929),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),
                                CooldownExact.Instance(3000),

                                new Transmute(0x092d)
                            )
                        ),
                        IfEqual.Instance(-2, 5,     //Misery 2
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x0929),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),
                                CooldownExact.Instance(1000),

                                TossEnemy.Instance(0, 5, 0x092c),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),
                                CooldownExact.Instance(3000),

                                new Transmute(0x092d)
                            )
                        ),
                        IfEqual.Instance(-2, 6,     //Manic
                            new QueuedBehavior(
                                TossEnemy.Instance(0, 5, 0x092a),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092a),
                                CooldownExact.Instance(1000),

                                TossEnemy.Instance(0, 5, 0x0929),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092a),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),
                                CooldownExact.Instance(1000),

                                TossEnemy.Instance(0, 5, 0x092c),
                                TossEnemy.Instance(60 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(120 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(240 * (float)Math.PI / 180, 5, 0x092c),
                                TossEnemy.Instance(300 * (float)Math.PI / 180, 5, 0x092c),
                                CooldownExact.Instance(3000),

                                new Transmute(0x092d)
                            )
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new OnHit(
                            Once.Instance(
                                new RunBehaviors(
                                    new SimpleTaunt("No corporeal creature can kill my sorrow"),
                                    new SetKey(-1, 1),
                                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)
                                )
                            )
                        )
                    }
                ))
            .Init(0x092d, Behaves("Actual Ghost King",
                    IfExist.Instance(-1,
                        new RunBehaviors(
                            Once.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                            new QueuedBehavior(
                                AngleMove.Instance(4, 180 * (float)Math.PI / 180, 6),
                                AngleMove.Instance(4, 0 * (float)Math.PI / 180, 6)
                            ),
                            Flashing.Instance(1000, 0xff000000)
                        ),
                        new RunBehaviors(
                            Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                            SetSize.Instance(140),
                            Flashing.Instance(1000, 0xff00ff00),
                            new QueuedBehavior(
                                Timed.Instance(1000, False.Instance(
                                    If.Instance(
                                        WithinSpawn.Instance(15),
                                        Chasing.Instance(4f, 10, 0, null),
                                        ReturnSpawn.Instance(4f)
                                    )
                                )),
                                SimpleAttack.Instance(10)
                            ),
                            new RandomTaunt(0.0001, "I cannot be defeated while my loyal subjects sustain me!")
                        )
                    )
                ))

            .Init(0x092a, Behaves("Small Ghost",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Timed.Instance(1500, False.Instance(
                            IfNot.Instance(
                                ChasingGroup.Instance(20, 50, 15, "Heros"),
                                SimpleWandering.Instance(20, 3f)
                            )
                        )),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Timed.Instance(5000, False.Instance(
                            new RunBehaviors(
                                Cooldown.Instance(1000, Rand.Instance(
                                    new RandomTaunt(0.01, "Switch!"),
                                    new RandomTaunt(0.01, "Save the King's Soul!")
                                )),
                                Cooldown.Instance(250, RingAttack.Instance(4))
                            )
                        ))
                    ),
                    new QueuedBehavior(
                        Timed.Instance(3000, Flashing.Instance(500, 0xff00ff00)),
                        False.Instance(NullBehavior.Instance)
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x092b)
                    }
                ))
            .Init(0x092b, Behaves("Medium Ghost",
                    new RunBehaviors(
                        new QueuedBehavior(
                            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                            SetSize.Instance(120),
                            CooldownExact.Instance(200),
                            SetSize.Instance(140),
                            CooldownExact.Instance(200),
                            False.Instance(new QueuedBehavior(
                                SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(1500, False.Instance(
                                    IfNot.Instance(
                                        ChasingGroup.Instance(20, 50, 15, "Heros"),
                                        SimpleWandering.Instance(20, 3f)
                                    )
                                )),
                                UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(5000, False.Instance(
                                    new RunBehaviors(
                                        Cooldown.Instance(1000, new RandomTaunt(0.01, "I come back more powerful than you could ever imagine")),
                                        Cooldown.Instance(250, RingAttack.Instance(4, offset: 45 * (float)Math.PI / 180))
                                    )
                                ))
                            ))
                        ),
                        new QueuedBehavior(
                            Timed.Instance(3000, Flashing.Instance(500, 0xff00ff00)),
                            False.Instance(NullBehavior.Instance)
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x092c)
                    }
                ))
            .Init(0x092c, Behaves("Large Ghost",
                    new RunBehaviors(
                        new QueuedBehavior(
                            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                            SetSize.Instance(160),
                            CooldownExact.Instance(200),
                            SetSize.Instance(180),
                            CooldownExact.Instance(200),
                            False.Instance(new QueuedBehavior(
                                SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(1500, False.Instance(
                                    IfNot.Instance(
                                        ChasingGroup.Instance(20, 50, 15, "Heros"),
                                        SimpleWandering.Instance(20, 3f)
                                    )
                                )),
                                UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(5000, False.Instance(
                                    new RunBehaviors(
                                        Cooldown.Instance(1000, new RandomTaunt(0.01, "Only the Secret Ghost Master can kill the King")),
                                        Cooldown.Instance(250, RingAttack.Instance(8))
                                    )
                                ))
                            ))
                        ),
                        new QueuedBehavior(
                            Timed.Instance(3000, Flashing.Instance(500, 0xff00ff00)),
                            False.Instance(NullBehavior.Instance)
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new OnDeath(
                            If.Instance(
                                IsEntityNotPresent.Instance(15, 0x092d),
                                new Transmute(0x669, 2, 2)
                            )
                        )
                    }
                ))
            .Init(0x0929, Behaves("Ghost Master",
                    IfExist.Instance(-1,
                        IfEqual.Instance(-1, 0,
                            new RunBehaviors(     //Medium
                                HpLesser.Instance(100000 - 1000 - 4000,
                                    new SetKey(-1, 1)
                                ),
                                new QueuedBehavior(
                                    SetSize.Instance(120),
                                    CooldownExact.Instance(200),
                                    SetSize.Instance(140),
                                    CooldownExact.Instance(200),
                                    Timed.Instance(3000, Flashing.Instance(500, 0xff00ff00)),
                                    False.Instance(NullBehavior.Instance)
                                ),
                                new QueuedBehavior(
                                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                    Timed.Instance(1500, False.Instance(
                                        IfNot.Instance(
                                            ChasingGroup.Instance(20, 50, 15, "Heros"),
                                            SimpleWandering.Instance(20, 3f)
                                        )
                                    )),
                                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                    Timed.Instance(5000, False.Instance(Cooldown.Instance(250, RingAttack.Instance(4, offset: 45 * (float)Math.PI / 180))))
                                )
                            ),
                            IfEqual.Instance(-1, 1,
                                new RunBehaviors(     //Large
                                    HpLesser.Instance(100000 - 1000 - 4000 - 8000,
                                        new SetKey(-1, 2)
                                    ),
                                    new QueuedBehavior(
                                        SetSize.Instance(160),
                                        CooldownExact.Instance(200),
                                        SetSize.Instance(180),
                                        CooldownExact.Instance(200),
                                        Timed.Instance(3000, Flashing.Instance(500, 0xff00ff00)),
                                        False.Instance(NullBehavior.Instance)
                                    ),
                                    new QueuedBehavior(
                                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                        Timed.Instance(1500, False.Instance(
                                            IfNot.Instance(
                                                ChasingGroup.Instance(20, 50, 15, "Heros"),
                                                SimpleWandering.Instance(20, 3f)
                                            )
                                        )),
                                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                        Timed.Instance(5000, False.Instance(Cooldown.Instance(250, RingAttack.Instance(8))))
                                    )
                                ),
                                new QueuedBehavior(     //Death
                                    new SimpleTaunt("Your secret soul master is dying, Your Majesty"),
                                    OrderEntity.Instance(20, 0x092d,
                                        new RunBehaviors(
                                            Rand.Instance(
                                                new RunBehaviors(
                                                    new SimpleTaunt("I feel my flesh again! For the first time in a 1000 years I LIVE!"),
                                                    new SimpleTaunt("Will you release me?")
                                                ),
                                                new SimpleTaunt("You have sapped my energy. A curse on you!")
                                            ),
                                            new SimpleTaunt("I am still very alone"),
                                            SetSize.Instance(100),
                                            new SetKey(-1, 0)
                                        )
                                    ),
                                    CooldownExact.Instance(2000),
                                    new SimpleTaunt("I cannot live with my betrayal..."),
                                    RingAttack.Instance(8),
                                    Die.Instance
                                )
                            )
                        ),
                        new RunBehaviors(     //Small
                            HpLesser.Instance(100000 - 1000,
                                new SetKey(-1, 0)
                            ),
                            new QueuedBehavior(
                                Timed.Instance(3000, Flashing.Instance(500, 0xff00ff00)),
                                False.Instance(NullBehavior.Instance)
                            ),
                            new QueuedBehavior(
                                SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(1500, False.Instance(
                                    IfNot.Instance(
                                        ChasingGroup.Instance(20, 50, 15, "Heros"),
                                        SimpleWandering.Instance(20, 3f)
                                    )
                                )),
                                UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                                Timed.Instance(5000, False.Instance(Cooldown.Instance(250, RingAttack.Instance(4))))
                            )
                        )
                    )
                ));
    }
}
