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
        static _ Deathmage = Behav()
            .Init(0x0925, Behaves("Deathmage",
                    SmoothWandering.Instance(3f, 3f),
                    new RunBehaviors(
                        new QueuedBehavior(
                            SpawnMinionImmediate.Instance(0x65a, 1, 3, 5),
                            SpawnMinionImmediate.Instance(0x65b, 1, 2, 4),
                            SpawnMinionImmediate.Instance(0x65c, 1, 1, 3),
                            SpawnMinionImmediate.Instance(0x65d, 1, 1, 2),
                            SpawnMinionImmediate.Instance(0x65e, 1, 1, 1),
                            Rand.Instance(
                                new RandomTaunt(0.1, "{PLAYER}, you will soon be my undead slave!"),
                                new RandomTaunt(0.1, "My skeletons will make short work of you."),
                                new RandomTaunt(0.1, "You will never leave this graveyard alive!")
                            ),
                            Cooldown.Instance(10000)
                        ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 6 * (float)Math.PI / 180, 3))
                    )
                ))
            .Init(0x65a, Behaves("Skeleton",
                    new QueuedBehavior(
                        Timed.Instance(2500, False.Instance(
                            Rand.Instance(
                                Circling.Instance(2, 10, 8f, null),
                                Chasing.Instance(8f, 10, 2, null)
                            )
                        )),
                        Timed.Instance(500, False.Instance(Retracting.Instance(8f, 10, null)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x65b, Behaves("Skeleton Swordsman",
                    new QueuedBehavior(
                        Timed.Instance(2500, False.Instance(
                            Rand.Instance(
                                Circling.Instance(2, 10, 8f, null),
                                Chasing.Instance(8f, 10, 2, null)
                            )
                        )),
                        Timed.Instance(500, False.Instance(Retracting.Instance(8f, 10, null)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x65c, Behaves("Skeleton Veteran",
                    new QueuedBehavior(
                        Timed.Instance(2500, False.Instance(
                            Rand.Instance(
                                Circling.Instance(2, 10, 8f, null),
                                Chasing.Instance(8f, 10, 2, null)
                            )
                        )),
                        Timed.Instance(500, False.Instance(Retracting.Instance(8f, 10, null)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x65d, Behaves("Skeleton Mage",
                    new QueuedBehavior(
                        Timed.Instance(2500, False.Instance(
                            Rand.Instance(
                                Circling.Instance(2, 10, 8f, null),
                                Chasing.Instance(8f, 10, 2, null)
                            )
                        )),
                        Timed.Instance(500, False.Instance(Retracting.Instance(8f, 10, null)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x65e, Behaves("Skeleton King",
                    new QueuedBehavior(
                        Timed.Instance(2500, False.Instance(
                            Rand.Instance(
                                Circling.Instance(2, 10, 8f, null),
                                Chasing.Instance(8f, 10, 2, null)
                            )
                        )),
                        Timed.Instance(500, False.Instance(Retracting.Instance(8f, 10, null)))
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ));
    }
}
