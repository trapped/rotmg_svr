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
        static _ Crystal = Behav()
            .Init(0x0935, Behaves("Mysterious Crystal",
               new RunBehaviors(
                   Once.Instance(new SetKey(-1, 1)),
        #region HpLesserPercent

                   /*If.Instance(
                     And.Instance(HpLesser.Instance(999000, NullBehavior.Instance), HpGreaterEqual.Instance(1000000, NullBehavior.Instance)),
                       new SetKey(-1, 2)  //
                   ),
                   If.Instance(
                     And.Instance(HpLesser.Instance(998000, NullBehavior.Instance), HpGreaterEqual.Instance(999000, NullBehavior.Instance)),
                       new SetKey(-1, 3)  //
                   ),
                   If.Instance(
                     And.Instance(HpLesser.Instance(997000, NullBehavior.Instance), HpGreaterEqual.Instance(998000, NullBehavior.Instance)),
                       Once.Instance(new SetKey(-1, 7))   //break
                   ),*/
                   Cooldown.Instance(5000, HpLesserPercent.Instance(0.9f, new SetKey(-1, 2))),
                   Cooldown.Instance(5000, HpLesserPercent.Instance(0.85f, new SetKey(-1, 3))),
                   Cooldown.Instance(5000, HpLesserPercent.Instance(0.8f, new SetKey(-1, 7))),

        #endregion
                IfEqual.Instance(-1, 1,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "Break the crystal for great rewards..."),
                      new RandomTaunt(0.1, "Help me...")
                      )
                    ),
                    CooldownExact.Instance(10000, new SetKey(-1, 1))
                    )
                ),
                IfEqual.Instance(-1, 2,
                  new QueuedBehavior(
                    Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                    Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "Fire upon this crystal with all your might for 5 seconds"),
                      new RandomTaunt(0.1, "If your attacks are weak, the crystal magically heals"),
                      new RandomTaunt(0.1, "Gather a large group to smash it open")
                      )
                    ),
                    Cooldown.Instance(5000, new SetKey(-1, 1))
                    )
                ),
                IfEqual.Instance(-1, 3,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "Sweet treasure awaits for powerful adventurers!"),
                      new RandomTaunt(0.1, "Yes!  Smash my prison for great rewards!")
                      )
                    ),
                    Cooldown.Instance(5000, new SetKey(-1, 4))
                    )
                ),
                IfEqual.Instance(-1, 4,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "If you are not very strong, this could kill you"),
                      new RandomTaunt(0.1, "If you are not yet powerful, stay away from the Crystal"),
                      new RandomTaunt(0.1, "New adventurers should stay away"),
                      new RandomTaunt(0.1, "That's the spirit. Lay your fire upon me."),
                      new RandomTaunt(0.1, "So close...")
                      )
                    ),
                    Cooldown.Instance(5000, new SetKey(-1, 5))
                    )
                ),
                IfEqual.Instance(-1, 5,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "I think you need more people..."),
                      new RandomTaunt(0.1, "Call all your friends to help you break the crystal!")
                      )
                    ),
                    CooldownExact.Instance(10000, new SetKey(-1, 6))
                    )
                ),
                IfEqual.Instance(-1, 6,
                  new RunBehaviors(
                      new SimpleTaunt("Perhaps you need a bigger group. Ask others to join you!"),
                      Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                      Heal.Instance(5000, 1000000, 0x0935),
                      RingAttack.Instance(16, 22, 16, projectileIndex: 0),
                      Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                      CooldownExact.Instance(5000, new SetKey(-1, 1))
                    )
                ),
                IfEqual.Instance(-1, 7,
                  new QueuedBehavior(
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Once.Instance(new SimpleTaunt("You cracked the crystal! Soon we shall emerge!")),
                    Cooldown.Instance(1000, SetSize.Instance(95)),
                    Cooldown.Instance(1000, SetSize.Instance(90)),
                    Cooldown.Instance(1000, SetSize.Instance(85)),
                    Cooldown.Instance(1000, SetSize.Instance(80)),
                    Flashing.Instance(1000, 0xffffffff),
                    Flashing.Instance(1000, 0xffffffff),
                    CooldownExact.Instance(4000, new SetKey(-1, 8))
                    )
                ),
                IfEqual.Instance(-1, 8,
                  new QueuedBehavior(
                    Once.Instance(new SimpleTaunt("This your reward! Imagine what evil even Oryx needs to keep locked up!")),
                    Once.Instance(RingAttack.Instance(16, 22, 16, projectileIndex: 0)),
                    Once.Instance(SpawnMinionImmediate.Instance(0x0941, 0, 1, 1)),
                    Once.Instance(Despawn.Instance)
                    ))
                )
            ))
            .Init(0x0941, Behaves("Crystal Prisoner",
                  new RunBehaviors(
                    Once.Instance(new SimpleTaunt("I'm finally free! Yesss!!!")),
                    Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                    SmoothWandering.Instance(1f, 1f),
                    Once.Instance(SpawnMinionImmediate.Instance(0x0943, 3, 3, 5)
                    ),
                    If.Instance(
                        EntityLesserThan.Instance(100, 3, 0x0943),  //IsEntityNotPresent.Instance(100, 0x0943),
                        Rand.Instance(
                            Reproduce.Instance(0x0943, 3, 100, 3)
                            )
                    )
                ),
                new QueuedBehavior(//here Is shooting start

        #region Circle Attack 1
                    Cooldown.Instance(4000),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 360 * (float)Math.PI / 180)
                    ),
                    
        #endregion 

        #region Circle Attack 2

                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 360 * (float)Math.PI / 180)
                    ),

        #endregion

        #region Flashing + SetCondEff

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    /*Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),*/
                    Cooldown.Instance(3000),

        #endregion

        #region Spawn Clones

                    SpawnMinionImmediate.Instance(0x0942, 2, 4, 4),
                    TossEnemy.Instance(0, 5, 0x0942),
                    TossEnemy.Instance(60, 7, 0x0942),
                    TossEnemy.Instance(240, 5, 0x0942),
                    TossEnemy.Instance(300, 7, 0x0942),
                    Cooldown.Instance(2000),

        #endregion

        #region Invulnerable

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(5000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),

        #endregion

        #region Whoa nelly

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),

        #endregion 
                    
        #region Confuse Circle

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),

                    SetSize.Instance(125),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),

                    SetSize.Instance(150),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),

                    SetSize.Instance(175),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),

                    SetSize.Instance(200),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    SetSize.Instance(175),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),

                    SetSize.Instance(150),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),

                    SetSize.Instance(125),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),

                    SetSize.Instance(100),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    )

        #endregion

                ),
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 8, 0, 8,
                        Tuple.Create(0.02, (ILoot)new ItemLoot("Crystal Wand")),
                        Tuple.Create(0.02, (ILoot)new ItemLoot("Crystal Sword")),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Att)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Def)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Dex)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Spd)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Wis))
                        
                        )))
            ))
            .Init(0x0942, Behaves("Crystal Prisoner Clone",
                new RunBehaviors(
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)),
                    Cooldown.Instance(7500, Despawn.Instance)
                ),
                IfNot.Instance(
                    Chasing.Instance(2, 10, 3, 0x0941),
                    SimpleWandering.Instance(2f)
                    )
            ))
            .Init(0x0943, Behaves("Crystal Prisoner Steed",
                new RunBehaviors(
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0))
                ),
                IfNot.Instance(
                    Chasing.Instance(2, 10, 5, 0x0941),
                    SimpleWandering.Instance(2f)
                    )
            ));
    }
}