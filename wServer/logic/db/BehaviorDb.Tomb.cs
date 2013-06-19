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
        static _ Tomb = Behav()
            .Init(0x0d28, Behaves("Tomb Defender",
                    HpGreaterEqual.Instance(99000,
                        new RunBehaviors(
                            Circling.Instance(10, 100, 5, 0x0d25)
                        )),
                        new RunBehaviors(
                         If.Instance(
                            And.Instance(HpLesser.Instance(100000, NullBehavior.Instance), HpGreaterEqual.Instance(99000, NullBehavior.Instance)),
                                new RunBehaviors(
                                    Circling.Instance(10, 100, 5, 0x0d25),
                                    Cooldown.Instance(5000, MultiAttack.Instance(18, 18 * (float)Math.PI / 180, 20, 0, projectileIndex: 3))

                                ))
                        ),
                    new RunBehaviors(
                         If.Instance(
                            And.Instance(HpLesser.Instance(99000, NullBehavior.Instance), HpGreaterEqual.Instance(50000, NullBehavior.Instance)),
                            new RunBehaviors(
                                Circling.Instance(10, 100, 5, 0x0d25),
                                Cooldown.Instance(2000, MultiAttack.Instance(45, 45 * (float)Math.PI / 180, 8, 0, projectileIndex: 2)),
                                Cooldown.Instance(5000, MultiAttack.Instance(20, 20 * (float)Math.PI / 180, 4, 0, projectileIndex: 1)),
                                Cooldown.Instance(5000, MultiAttack.Instance(18, 18 * (float)Math.PI / 180, 6, 0, projectileIndex: 1)),
                                Cooldown.Instance(2000, MultiAttack.Instance(75, 75 * (float)Math.PI / 180, 5, 0, projectileIndex: 0))

                                )
                        ),
                        If.Instance(
                            And.Instance(HpLesser.Instance(50000, NullBehavior.Instance), HpGreaterEqual.Instance(10000, NullBehavior.Instance)),
                            new RunBehaviors(
                                Circling.Instance(10, 100, 5, 0x0d25),
                                Cooldown.Instance(2000, MultiAttack.Instance(45, 45 * (float)Math.PI / 180, 8, 0, projectileIndex: 2)),
                                Cooldown.Instance(5000, MultiAttack.Instance(20, 20 * (float)Math.PI / 180, 4, 0, projectileIndex: 1)),
                                Cooldown.Instance(5000, MultiAttack.Instance(18, 18 * (float)Math.PI / 180, 6, 0, projectileIndex: 1)),
                                Cooldown.Instance(5000, MultiAttack.Instance(140, 140 * (float)Math.PI / 180, 2, 0, projectileIndex: 1)),
                                Cooldown.Instance(2000, MultiAttack.Instance(75, 75 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                                Cooldown.Instance(2000, MultiAttack.Instance(15, 15 * (float)Math.PI / 180, 2, 0, projectileIndex: 0)),
                                Once.Instance(new SimpleTaunt("My artifacts shall prove my wall of defense is impenetrable!")),
                                Once.Instance(
                                    new RunBehaviors(
                                        SpawnMinionImmediate.Instance(0x0d22, 3, 3, 3),
                                        SpawnMinionImmediate.Instance(0x0d23, 3, 3, 3),
                                        SpawnMinionImmediate.Instance(0x0d24, 3, 3, 3)
                                        )
                                )
                            )
                        ),
                        HpLesserPercent.Instance(0.1f,
                            new RunBehaviors(
                                Flashing.Instance(600, 0xff0000),
                                Charge.Instance(6, 10, null),
                                Once.Instance(
                                    new RunBehaviors(
                                        SpawnMinionImmediate.Instance(0x0d22, 3, 3, 5),
                                        SpawnMinionImmediate.Instance(0x0d23, 3, 3, 5),
                                        SpawnMinionImmediate.Instance(0x0d24, 3, 3, 5)
                                )),
                                Cooldown.Instance(2000, MultiAttack.Instance(45, 45 * (float)Math.PI / 180, 8, 0, projectileIndex: 2)),
                                Cooldown.Instance(5000, MultiAttack.Instance(20, 20 * (float)Math.PI / 180, 4, 0, projectileIndex: 1)),
                                Cooldown.Instance(5000, MultiAttack.Instance(18, 18 * (float)Math.PI / 180, 6, 0, projectileIndex: 1)),
                                Cooldown.Instance(5000, MultiAttack.Instance(140, 140 * (float)Math.PI / 180, 2, 0, projectileIndex: 1)),
                                Cooldown.Instance(2000, MultiAttack.Instance(75, 75 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                                Cooldown.Instance(2000, MultiAttack.Instance(15, 15 * (float)Math.PI / 180, 2, 0, projectileIndex: 0)),
                                Cooldown.Instance(500, MultiAttack.Instance(5, 5 * (float)Math.PI / 180, 9, 0, projectileIndex: 4)),
                                Once.Instance(new SimpleTaunt("The end of your path is here!")),
                                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                Cooldown.Instance(5000, Once.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)))

                            )
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                            Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Tome of Holy Protection")),
                            Tuple.Create(0.009, (ILoot)new ItemLoot("Ring of the Pyramid")),
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(1.00, (ILoot)new ItemLoot("Potion of Life"))

                    )))

            ))
            .Init(0x0d26, Behaves("Tomb Support",
                new RunBehaviors(
                        HpGreaterEqual.Instance(5000,
                            new RunBehaviors(
                                Circling.Instance(10, 100, 5, 0x0d25),
                                Cooldown.Instance(1000, Heal.Instance(6f, 100, 0x0d28)),
                                Cooldown.Instance(1000, Heal.Instance(6f, 100, 0x0d27))
                            )
                        ),
                        HpLesserPercent.Instance(0.99f,
                    new RunBehaviors(

                        Cooldown.Instance(2000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 5)),
                        Cooldown.Instance(5000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 6)),
                        Cooldown.Instance(3000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 1)),
                        Cooldown.Instance(3500, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 2)),
                        Cooldown.Instance(3500, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 3)),
                        Cooldown.Instance(3500, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 4)),
                        Cooldown.Instance(5000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 7, 0, projectileIndex: 0))
                    )),
                    HpLesserPercent.Instance(0.5f,
                        Once.Instance(
                            new RunBehaviors(
                                True.Instance(Once.Instance(new SimpleTaunt("My artifacts shall make your lethargic lives end much more switfly!"))),
                                  Once.Instance(
                                    new RunBehaviors(
                                        SpawnMinionImmediate.Instance(0x0d1c, 3, 3, 4),
                                        SpawnMinionImmediate.Instance(0x0d1d, 3, 3, 4),
                                        SpawnMinionImmediate.Instance(0x0d1e, 3, 3, 4)
                                        )
                                    )
                                )
                            )
                        ),
                        HpLesserPercent.Instance(0.0625f,
                            new RunBehaviors(
                                Flashing.Instance(500, 0xffff3333),
                                Chasing.Instance(8, 10, 0, null),
                                Cooldown.Instance(3000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 1)),
                                Cooldown.Instance(3500, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 2)),
                                Cooldown.Instance(3500, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 3)),
                                Cooldown.Instance(500, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 8)),
                                Cooldown.Instance(700, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 5)),
                                Cooldown.Instance(4000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 6)),
                                Once.Instance(new SimpleTaunt("This cannot be! You shall not succeed!"))
                            ))

                    ),
                    loot: new LootBehavior(LootDef.Empty,
                            Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(0.009, (ILoot)new ItemLoot("Ring of the Sphinx")),
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(1.00, (ILoot)new ItemLoot("Potion of Life"))

                    )))
            ))
            .Init(0x0d27, Behaves("Geb",
                new RunBehaviors(
                        HpGreaterEqual.Instance(9000, //If his HP is greater than 9000 (rage), 
                            new RunBehaviors(
                                Circling.Instance(10, 100, 5, 0x0d25)
                            )
                        ),
                        HpLesserPercent.Instance(0.99f,
                    new RunBehaviors(

                        Cooldown.Instance(700, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 2)),   // Multi attack, Multiple projectiles, (same projectile), 
                        Cooldown.Instance(3000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 1)), /* Green Slow Rings*/
                        Cooldown.Instance(3100, ThrowAttack.Instance(4, 8, 120)),/* Bomb 1*/
                        Cooldown.Instance(2000, ThrowAttack.Instance(3, 8, 70)),/* Bomb 2*/
                        Cooldown.Instance(2500, ThrowAttack.Instance(10, 12, 40))/* Anti Spectate Bomb*/
                    )),
                    HpLesserPercent.Instance(0.1f, /*If Hp is less than 10%, activate behaviors (rage)*/
                            new RunBehaviors(
                                Flashing.Instance(500, 0xffff3333), /* Flash red when rage*/
                                MaintainDist.Instance(50, 5, 15, null), /* This creates Geb's Skipping, he is maintaing distance from players*/
                                True.Instance(Once.Instance(SpawnMinionImmediate.Instance(0x0d1f, 1, 1, 4))),   //Artifact 1 (Circles Geb)
                                True.Instance(Once.Instance(SpawnMinionImmediate.Instance(0x0d20, 1, 1, 4))),   //Artifact 2 (Circles Bes and Nut)
                                True.Instance(Once.Instance(SpawnMinionImmediate.Instance(0x0d21, 1, 1, 4))),    //Artifact 3 (chases)
                                Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 2)),
                                Cooldown.Instance(4000, RingAttack.Instance(5, 0, 5, projectileIndex: 0)),/*Just some random Black shots during rage.*/
                                Cooldown.Instance(500, AngleAttack.Instance(225)),/*Just some random Black shots during rage.*/
                                Cooldown.Instance(500, AngleAttack.Instance(36)),/*Just some random Black shots during rage.*/
                                Cooldown.Instance(500, AngleAttack.Instance(0)),/*Just some random Black shots during rage.*/
                                Cooldown.Instance(500, AngleAttack.Instance(135)),/*Just some random Black shots during rage.*/
                                Cooldown.Instance(500, AngleAttack.Instance(90)),/*Just some random Black shots during rage.*/
                                Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 5)), /*Fire Magic Shots.*/
                                Cooldown.Instance(3000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 1)),/*Green Slow Boomerang*/
                                Once.Instance(new SimpleTaunt("Argh! You shall pay for your crimes!")) /*Taunt for Rage*/
                            )

                    ),


                    HpLesserPercent.Instance(0.5f, /*If HP is less than 50%, run new behaviors*/
                        Once.Instance(/*Run theese commands ONCE, to prevent infinite Spawning.*/
                            new RunBehaviors(
                                Circling.Instance(10, 100, 5, 0x0d25),
                                True.Instance(Once.Instance(new SimpleTaunt("My artifacts shall destroy you from your soul to your flesh!"))), /*Artifact Taunt.*/
                                Once.Instance(
                                    new RunBehaviors(
                                        SpawnMinionImmediate.Instance(0x0d1f, 3, 3, 4),   //Spawn Artifact 1
                                        SpawnMinionImmediate.Instance(0x0d20, 3, 3, 4),   //Spawn Artifact 2
                                        SpawnMinionImmediate.Instance(0x0d21, 3, 3, 4)    //Spawn Artifact 3
                                        ))
                                    )
                                )
                            )
                        ),
                        loot: new LootBehavior(LootDef.Empty, //Class for loot.
                          Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Life)),
                            Tuple.Create(0.015, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Ring of the Nile"))
                            ))
                        )
                ))
                .Init(0x0d1f, Behaves("Nile Artifact 1",
                    IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d26),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(3000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0))
                ))
                .Init(0x0d20, Behaves("Nile Artifact 2",
                    IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d27),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(3000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0))
                ))
                .Init(0x0d21, Behaves("Nile Artifact 3",
                    IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d28),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(3000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0))
                ))

                .Init(0x0d1c, Behaves("Sphinx Artifact 1",
                   IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d26),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(3000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                    Cooldown.Instance(5000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 1))

                ))
                .Init(0x0d1d, Behaves("Sphinx Artifact 2",
                   IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d27),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(3000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                    Cooldown.Instance(5000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 1))
                ))
                .Init(0x0d1e, Behaves("Sphinx Artifact 3",
                   IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d28),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(3000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                    Cooldown.Instance(5000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 1))
                ))
                .Init(0x0d22, Behaves("Pyramid Artifact 1",
                   IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d26),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(5000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0))
                ))
                .Init(0x0d23, Behaves("Pyramid Artifact 2",
                   IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d27),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(5000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0))
                ))
                .Init(0x0d24, Behaves("Pyramid Artifact 3",
                   IfNot.Instance(
                        Circling.Instance(3, 3, 10, 0x0d28),
                        Chasing.Instance(10, 20, 1, null)
                    ),
                    Cooldown.Instance(5000, MultiAttack.Instance(120, 120 * (float)Math.PI / 180, 3, 0, projectileIndex: 0))
                ))
                .Init(0x0d3c, Behaves("Lion Archer",
                    IfNot.Instance(
                        Chasing.Instance(11, 8, 6, 0x617),
                        IfNot.Instance(
                            Chasing.Instance(8, 8, 0, null),
                            SimpleWandering.Instance(4)
                            )
                        ),
                        new RunBehaviors(
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 0)),
                        Cooldown.Instance(1200, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 1)),
                        Cooldown.Instance(1300, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 2)),
                        Cooldown.Instance(1000, Once.Instance(RingAttack.Instance(5, 0, 5, projectileIndex: 3)))
                        ),
                        loot: new LootBehavior(
                            new LootDef(0, 2, 0, 8,
                                Tuple.Create(0.03, (ILoot)HpPotionLoot.Instance),
                                Tuple.Create(0.03, (ILoot)MpPotionLoot.Instance)
                                ))
                ))
                .Init(0x0d3b, Behaves("Jackal Priest",
                    IfNot.Instance(
                        Chasing.Instance(12, 9, 6, 0x0d3b),
                        Chasing.Instance(6.5f, 8, 1, null)
                        ),
                        Cooldown.Instance(600, SimpleAttack.Instance(10, projectileIndex: 0)),
                        Cooldown.Instance(1000, RingAttack.Instance(5, 10, 0, projectileIndex: 1)
                        ),
                        loot: new LootBehavior(
                          new LootDef(0, 2, 0, 8,
                            Tuple.Create(0.0001, (ILoot)HpPotionLoot.Instance)
                            ))
                ))
                .Init(0x0d1b, Behaves("Scarab",
                    IfNot.Instance(
                        Chasing.Instance(12, 9, 6, 0x0d1b),
                        Chasing.Instance(8.5f, 8, 1, null)
                        ),
                        Cooldown.Instance(200, SimpleAttack.Instance(3, projectileIndex: 0)),
                        Cooldown.Instance(400, RingAttack.Instance(5, 10, 0, projectileIndex: 1)
                        )
                ))
                .Init(0x0d3a, Behaves("Eagle Sentry",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(960, MultiAttack.Instance(900, 15 * (float)Math.PI / 360, 3, 0, projectileIndex: 0)),
                        Cooldown.Instance(1160, MultiAttack.Instance(900, 15 * (float)Math.PI / 360, 3, 0, projectileIndex: 1)),
                        Cooldown.Instance(1360, MultiAttack.Instance(900, 15 * (float)Math.PI / 360, 3, 0, projectileIndex: 2))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d2b, Behaves("Beam Priestess",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(860, MultiAttack.Instance(900, 15 * (float)Math.PI / 360, 3, 0, projectileIndex: 0)),
                        Cooldown.Instance(1100, MultiAttack.Instance(900, 20 * (float)Math.PI / 360, 1, 0, projectileIndex: 1))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d2a, Behaves("Beam Priest",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(860, MultiAttack.Instance(966, 15 * (float)Math.PI / 360, 1, 0, projectileIndex: 0)),
                        Cooldown.Instance(1100, MultiAttack.Instance(942, 20 * (float)Math.PI / 360, 3, 0, projectileIndex: 1))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d29, Behaves("Active Sarcophagus",
                    new RunBehaviors(
                        Cooldown.Instance(2800, MultiAttack.Instance(966, 30 * (float)Math.PI / 360, 26, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Health Potion")),
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Magic Potion")),
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Tincture of Mana")),
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Tincture of Life")),
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Tincture of Dexterity"))
                            )))
                ))
                .Init(0x0d1a, Behaves("Bloated Mummy",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1860, MultiAttack.Instance(500, 30 * (float)Math.PI / 360, 25, 0, projectileIndex: 0)),
                        Cooldown.Instance(1100, MultiAttack.Instance(600, 60 * (float)Math.PI / 360, 4, 0, projectileIndex: 1))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d19, Behaves("Jackal Lord",
                    new RunBehaviors(
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d18, 5, 1, 2)),
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d17, 5, 1, 2)),
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d16, 5, 1, 2)),
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(860, MultiAttack.Instance(900, 15 * (float)Math.PI / 360, 3, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d18, Behaves("Jackal Assassin",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(800, MultiAttack.Instance(500, 30 * (float)Math.PI / 360, 6, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d17, Behaves("Jackal Veteran",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1860, MultiAttack.Instance(600, 30 * (float)Math.PI / 360, 4, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d16, Behaves("Jackal Warrior",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1860, MultiAttack.Instance(600, 30 * (float)Math.PI / 360, 3, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d15, Behaves("Blue Swarm Masters",
                    new RunBehaviors(
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d14, 5, 2, 3)),
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1260, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 8, 0, projectileIndex: 0)),
                        Cooldown.Instance(1600, MultiAttack.Instance(600, 60 * (float)Math.PI / 360, 2, 0, projectileIndex: 1)),
                        Cooldown.Instance(1880, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 2, 0, projectileIndex: 2)),
                        Cooldown.Instance(1990, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 2, 0, projectileIndex: 3))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d14, Behaves("Blue Swarm Minions",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1860, MultiAttack.Instance(600, 30 * (float)Math.PI / 360, 8, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d13, Behaves("Yellow Swarm Masters",
                    new RunBehaviors(
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d14, 5, 2, 3)),
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1260, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 8, 0, projectileIndex: 0)),
                        Cooldown.Instance(1600, MultiAttack.Instance(600, 60 * (float)Math.PI / 360, 2, 0, projectileIndex: 1)),
                        Cooldown.Instance(1880, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 2, 0, projectileIndex: 2)),
                        Cooldown.Instance(1990, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 2, 0, projectileIndex: 3))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d12, Behaves("Yellow Swarm Minions",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1860, MultiAttack.Instance(600, 30 * (float)Math.PI / 360, 8, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d11, Behaves("Red Swarm Masters",
                    new RunBehaviors(
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d14, 5, 2, 3)),
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1260, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 8, 0, projectileIndex: 0)),
                        Cooldown.Instance(1600, MultiAttack.Instance(600, 60 * (float)Math.PI / 360, 2, 0, projectileIndex: 1)),
                        Cooldown.Instance(1880, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 2, 0, projectileIndex: 2)),
                        Cooldown.Instance(1990, MultiAttack.Instance(600, 20 * (float)Math.PI / 360, 2, 0, projectileIndex: 3))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ))
                .Init(0x0d10, Behaves("Red Swarm Minions",
                    new RunBehaviors(
                        SmoothWandering.Instance(2f, 2f),
                        Chasing.Instance(8, 11, 1, null),
                        Cooldown.Instance(1860, MultiAttack.Instance(600, 30 * (float)Math.PI / 360, 8, 0, projectileIndex: 0))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                          Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Health Potion"))
                            )))
                ));

    }
}