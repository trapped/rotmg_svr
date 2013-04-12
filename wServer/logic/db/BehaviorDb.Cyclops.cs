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
        static _ Cyclops = Behav()
            .Init(0x67d, Behaves("Cyclops God",
                    SmoothWandering.Instance(3f, 3f),
                    new RunBehaviors(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x67e, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x67f, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x680, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x681, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x682, 1, 1, 3)
                            )
                        ),
                        new QueuedBehavior(
                            Rand.Instance(
                                SpawnMinionImmediate.Instance(0x67e, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x67f, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x680, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x681, 1, 1, 3),
                                SpawnMinionImmediate.Instance(0x682, 1, 1, 3),
                                NullBehavior.Instance
                            ),
                            True.Instance(Rand.Instance(
                                new RandomTaunt(0.1, "I will floss with your tendons!"),
                                new RandomTaunt(0.1, "I smell the blood of an Englishman!"),
                                new RandomTaunt(0.1, "I will suck the marrow from your bones!"),
                                new RandomTaunt(0.1, "You will be my food, {PLAYER}!"),
                                new RandomTaunt(0.1, "Blargh!!"),
                                new RandomTaunt(0.1, "Leave my castle!"),
                                new RandomTaunt(0.1, "More wine!")
                            )),
                            Cooldown.Instance(2000)
                        ),
                        If.Instance(
                            IsEntityPresent.Instance(10, null),
                            new QueuedBehavior(
                                PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 4),
                                Cooldown.Instance(1000),
                                PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 4),
                                Cooldown.Instance(1000),
                                PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 4),
                                Cooldown.Instance(1000),
                                PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 4),
                                Cooldown.Instance(1000),
                                PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 4),
                                Cooldown.Instance(1000),

                                new RunBehaviors(
                                    SimpleAttack.Instance(10, projectileIndex: 0),
                                    SimpleAttack.Instance(10, projectileIndex: 1),
                                    SimpleAttack.Instance(10, projectileIndex: 2),
                                    SimpleAttack.Instance(10, projectileIndex: 3)
                                ),
                                Cooldown.Instance(1000),
                                new RunBehaviors(
                                    SimpleAttack.Instance(10, projectileIndex: 0),
                                    SimpleAttack.Instance(10, projectileIndex: 1),
                                    SimpleAttack.Instance(10, projectileIndex: 2),
                                    SimpleAttack.Instance(10, projectileIndex: 3)
                                ),
                                Cooldown.Instance(1000),
                                new RunBehaviors(
                                    SimpleAttack.Instance(10, projectileIndex: 0),
                                    SimpleAttack.Instance(10, projectileIndex: 1),
                                    SimpleAttack.Instance(10, projectileIndex: 2),
                                    SimpleAttack.Instance(10, projectileIndex: 3)
                                ),
                                Cooldown.Instance(1000),
                                new RunBehaviors(
                                    SimpleAttack.Instance(10, projectileIndex: 0),
                                    SimpleAttack.Instance(10, projectileIndex: 1),
                                    SimpleAttack.Instance(10, projectileIndex: 2),
                                    SimpleAttack.Instance(10, projectileIndex: 3)
                                ),
                                Cooldown.Instance(1000),
                                new RunBehaviors(
                                    SimpleAttack.Instance(10, projectileIndex: 0),
                                    SimpleAttack.Instance(10, projectileIndex: 1),
                                    SimpleAttack.Instance(10, projectileIndex: 2),
                                    SimpleAttack.Instance(10, projectileIndex: 3)
                                ),
                                Cooldown.Instance(1000)
                            ),
                            Cooldown.Instance(1000, RingAttack.Instance(10, projectileIndex: 4))
                        )
                    )
                ))
            .Init(0x67e, Behaves("Cyclops",
                    If.Instance(
                        IsEntityPresent.Instance(10, null),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(10f, 10, 2, null)),
                            Timed.Instance(1000, False.Instance(Retracting.Instance(8f, 8, null)))
                        ),
                        SimpleWandering.Instance(10)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x67f, Behaves("Cyclops Warrior",
                    If.Instance(
                        IsEntityPresent.Instance(10, null),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(10f, 10, 2, null)),
                            Timed.Instance(1000, False.Instance(Retracting.Instance(8f, 8, null)))
                        ),
                        SimpleWandering.Instance(10)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x680, Behaves("Cyclops Noble",
                    If.Instance(
                        IsEntityPresent.Instance(10, null),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(10f, 10, 2, null)),
                            Timed.Instance(1000, False.Instance(Retracting.Instance(8f, 8, null)))
                        ),
                        SimpleWandering.Instance(10)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x681, Behaves("Cyclops Prince",
                    If.Instance(
                        IsEntityPresent.Instance(10, null),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(10f, 10, 2, null)),
                            Timed.Instance(1000, False.Instance(Retracting.Instance(8f, 8, null)))
                        ),
                        SimpleWandering.Instance(10)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x682, Behaves("Cyclops King",
                    If.Instance(
                        IsEntityPresent.Instance(10, null),
                        new QueuedBehavior(
                            Not.Instance(Chasing.Instance(10f, 10, 2, null)),
                            Timed.Instance(1000, False.Instance(Retracting.Instance(8f, 8, null)))
                        ),
                        SimpleWandering.Instance(10)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ));
    }
}
