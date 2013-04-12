using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.attack;
using wServer.logic.movement;
using wServer.logic.loot;
using wServer.logic.taunt;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        static _ ForbiddenJungle = Behav()
            .Init(0x0dc2, Behaves("Great Coil Snake",
                    new QueuedBehavior(
                        Timed.Instance(5000, False.Instance(SimpleWandering.Instance(2))),
                        Timed.Instance(1500, Not.Instance(ReturnSpawn.Instance(2)))
                    ),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, PredictiveAttack.Instance(10, 1, projectileIndex: 0)),
                        Cooldown.Instance(1000, RingAttack.Instance(10, 10, projectileIndex: 1)),
                        Cooldown.Instance(1000, Rand.Instance(
                            TossEnemy.Instance(0 * (float)Math.PI / 180, 4, 0x0dc1),
                            TossEnemy.Instance(90 * (float)Math.PI / 180, 4, 0x0dc1),
                            TossEnemy.Instance(180 * (float)Math.PI / 180, 4, 0x0dc1),
                            TossEnemy.Instance(270 * (float)Math.PI / 180, 4, 0x0dc1)
                        )),
                        Cooldown.Instance(1000, Rand.Instance(
                            TossEnemy.Instance(0 * (float)Math.PI / 180, 4, 0x0dc1),
                            TossEnemy.Instance(90 * (float)Math.PI / 180, 4, 0x0dc1),
                            TossEnemy.Instance(180 * (float)Math.PI / 180, 4, 0x0dc1),
                            TossEnemy.Instance(270 * (float)Math.PI / 180, 4, 0x0dc1)
                        ))
                    )
                ))
            .Init(0x0dc1, Behaves("Great Snake Egg",
                    new QueuedBehavior(
                        Cooldown.Instance(5000),
                        new Transmute(0x0dc0, 1, 2)
                    )
                ))
            .Init(0x0dc0, Behaves("Great Temple Snake",
                    False.Instance(SimpleWandering.Instance(3)),
                    new QueuedBehavior(
                        Cooldown.Instance(1000, MultiAttack.Instance(4, 7 * (float)Math.PI / 180, 2, projectileIndex: 0)),
                        Cooldown.Instance(1000, RingAttack.Instance(6, 4, projectileIndex: 1))
                    )
                ));
    }
}
