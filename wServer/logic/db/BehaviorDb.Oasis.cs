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
        static _ Oasis = Behav()
            .Init(0x678, Behaves("Oasis Giant",
                    SmoothWandering.Instance(1f, 3f),
                    new RunBehaviors(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x67c, 3, 1, 3),
                                SpawnMinionImmediate.Instance(0x67b, 3, 1, 3),
                                SpawnMinionImmediate.Instance(0x67a, 3, 1, 3),
                                SpawnMinionImmediate.Instance(0x679, 3, 1, 3)
                            )
                        ),
                        new QueuedBehavior(
                            Rand.Instance(
                                SpawnMinionImmediate.Instance(0x67c, 2, 1, 3),
                                SpawnMinionImmediate.Instance(0x67b, 2, 1, 3),
                                SpawnMinionImmediate.Instance(0x67a, 2, 1, 3),
                                SpawnMinionImmediate.Instance(0x679, 2, 1, 3),
                                NullBehavior.Instance
                            ),
                            True.Instance(Rand.Instance(
                                new RandomTaunt(0.1, "Come closer, {PLAYER}! Yes, closer!"),
                                new RandomTaunt(0.1, "I rule this place, {PLAYER}!"),
                                new RandomTaunt(0.1, "Surrender to my aquatic army, {PLAYER}!"),
                                new RandomTaunt(0.1, "You must be thirsty, {PLAYER}. Enter my waters!"),
                                new RandomTaunt(0.1, "Minions! We shall have {PLAYER} for dinner!")
                            )),
                            Cooldown.Instance(2000)
                        ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, 1))
                    )
                ))
            .Init(0x679, Behaves("Oasis Ruler",
                    IfNot.Instance(
                        Chasing.Instance(5f, 17, 12, 0x678),
                        Rand.Instance(
                            Chasing.Instance(5f, 10, 2, null),
                            SimpleWandering.Instance(5f)
                        )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10))
                ))
            .Init(0x67a, Behaves("Oasis Soldier",
                    IfNot.Instance(
                        Chasing.Instance(5f, 17, 12, 0x678),
                        Rand.Instance(
                            Chasing.Instance(5f, 10, 2, null),
                            SimpleWandering.Instance(5f)
                        )
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10))
                ))
            .Init(0x67b, Behaves("Oasis Creature",
                    IfNot.Instance(
                        Chasing.Instance(5f, 17, 12, 0x678),
                        Rand.Instance(
                            Chasing.Instance(5f, 10, 2, null),
                            SimpleWandering.Instance(5f)
                        )
                    ),
                    Cooldown.Instance(250, SimpleAttack.Instance(10))
                ))
            .Init(0x67c, Behaves("Oasis Monster",
                    IfNot.Instance(
                        Chasing.Instance(5f, 17, 12, 0x678),
                        Rand.Instance(
                            Chasing.Instance(5f, 10, 2, null),
                            SimpleWandering.Instance(5f)
                        )
                    ),
                    Cooldown.Instance(250, SimpleAttack.Instance(10))
                ));
    }
}
