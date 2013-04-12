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
        static _ Misc = Behav()
            .Init(0x01c7, Behaves("White Fountain",
                    MagicEye.Instance,
                    Cooldown.Instance(250,
                    new NexusHealHp()
                        /*IfNot.Instance(
                            new NexusHealHp(),
                            new NexusHealMp()
                        )*/
                    )
                ))
            .Init(0x6d2, Behaves("Red Satellite",
                    StrictCirclingGroup.Instance(3, 5, "Golems"),
                    GolemSatellites.Instance
                ))
            .Init(0x6d3, Behaves("Green Satellite",
                    StrictCirclingGroup.Instance(3, 5, "Golems"),
                    GolemSatellites.Instance
                ))
            .Init(0x6d4, Behaves("Blue Satellite",
                    StrictCirclingGroup.Instance(3, 5, "Golems"),
                    GolemSatellites.Instance
                ))

            .Init(0x6d5, Behaves("Gray Satellite 1",
                    StrictCirclingGroup.Instance(1f, 10, "Golem Satellites"),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(5)),
                        GolemSatellites.Instance
                    )
                ))
            .Init(0x6d6, Behaves("Gray Satellite 2",
                    StrictCirclingGroup.Instance(1f, 10, "Golem Satellites"),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(5)),
                        GolemSatellites.Instance
                    )
                ))
            .Init(0x6d7, Behaves("Gray Satellite 3",
                    StrictCirclingGroup.Instance(1f, 10, "Golem Satellites"),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(5)),
                        GolemSatellites.Instance
                    )
                ))
            .Init(0x01ff, Behaves("Sheep",
                    new QueuedBehavior(
                        RandomDelay.Instance(2500, 7500),
                        SimpleWandering.Instance(2, 2)
                    ),
                    Cooldown.Instance(1000,
                        Rand.Instance(
                            new RandomTaunt(0.001, "baa"),
                            new RandomTaunt(0.001, "baa baa")
                        )
                    )
                ));
    }
}
