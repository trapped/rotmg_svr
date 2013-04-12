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
        static _ Phoenix = Behav()
            .Init(0x675, Behaves("Phoenix Lord",
                    SmoothWandering.Instance(1f, 3f),
                    new RunBehaviors(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x677, 1, 3, 5),
                                SpawnMinionImmediate.Instance(0x676, 1, 1, 2)
                            )
                        ),
                        new QueuedBehavior(
                            If.Instance(
                                EntityGroupLesserThan.Instance(10, 25, "Pyre"),
                                Rand.Instance(
                                    SpawnMinionImmediate.Instance(0x677, 1, 2, 4),
                                    SpawnMinionImmediate.Instance(0x676, 1, 1, 2),
                                    NullBehavior.Instance
                                )
                            ),
                            True.Instance(Rand.Instance(
                                new RandomTaunt(0.1, "Alas, {PLAYER}, you will taste death but once!"),
                                new RandomTaunt(0.1, "I have met many like you, {PLAYER}, in my thrice thousand years!"),
                                new RandomTaunt(0.1, "Purge yourself, {PLAYER}, in the heat of my flames!"),
                                new RandomTaunt(0.1, "The ashes of past heroes cover my plains!"),
                                new RandomTaunt(0.1, "Some die and are ashes, but I am ever reborn!")
                            )),
                            Cooldown.Instance(2000)
                        ),
                        Cooldown.Instance(500, PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, 1))
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x6c2)
                    }
                ))
            .Init(0x6c2, Behaves("Phoenix Egg",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        CooldownExact.Instance(1500),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        False.Instance(new RunBehaviors(
                            Flashing.Instance(500, 0xffff0000),
                            new QueuedBehavior(
                                SetSize.Instance(140),
                                CooldownExact.Instance(500),
                                SetSize.Instance(130),
                                CooldownExact.Instance(500)
                            )
                        ))
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x6c1)
                    }
                ))
            .Init(0x6c1, Behaves("Phoenix Reborn",
                    SmoothWandering.Instance(1f, 3f),
                    new RunBehaviors(
                        Once.Instance(
                            new RunBehaviors(
                                SpawnMinionImmediate.Instance(0x677, 1, 3, 5),
                                SpawnMinionImmediate.Instance(0x676, 1, 1, 2)
                            )
                        ),
                        new QueuedBehavior(
                            If.Instance(
                                EntityGroupLesserThan.Instance(10, 25, "Pyre"),
                                Rand.Instance(
                                    SpawnMinionImmediate.Instance(0x677, 1, 2, 4),
                                    SpawnMinionImmediate.Instance(0x676, 1, 1, 2),
                                    NullBehavior.Instance
                                )
                            ),
                            Cooldown.Instance(2000)
                        ),
                        new QueuedBehavior(
                            Cooldown.Instance(500, RingAttack.Instance(12, projectileIndex: 0)),
                            Cooldown.Instance(500, RingAttack.Instance(12, projectileIndex: 0)),

                            Cooldown.Instance(200, SimpleAttack.Instance(10, projectileIndex: 1)),
                            Cooldown.Instance(200, SimpleAttack.Instance(10, projectileIndex: 1)),
                            Cooldown.Instance(200, SimpleAttack.Instance(10, projectileIndex: 1)),
                            Cooldown.Instance(200, SimpleAttack.Instance(10, projectileIndex: 1)),
                            Cooldown.Instance(200, SimpleAttack.Instance(10, projectileIndex: 1)),

                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, 1, projectileIndex: 1)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, 1, projectileIndex: 1)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, 1, projectileIndex: 1)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(10, 10 * (float)Math.PI / 180, 4, 1, projectileIndex: 1))
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(400, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.0001, (ILoot)IncLoot.Instance)
                        ))
                    )
                ))
            .Init(0x676, Behaves("Birdman Chief",
                    IfNot.Instance(
                        Chasing.Instance(5f, 17, 12, 0x678),
                        Rand.Instance(
                            Chasing.Instance(5f, 10, 2, null),
                            SimpleWandering.Instance(5f)
                        )
                    ),
                    Cooldown.Instance(1000, PredictiveAttack.Instance(10, 0.5f))
                ))
            .Init(0x677, Behaves("Birdman",
                    IfNot.Instance(
                        Chasing.Instance(5f, 17, 12, 0x678),
                        Rand.Instance(
                            Chasing.Instance(5f, 10, 2, null),
                            SimpleWandering.Instance(5f)
                        )
                    ),
                    Cooldown.Instance(500, PredictiveAttack.Instance(10, 0.5f))
                ));
    }
}
