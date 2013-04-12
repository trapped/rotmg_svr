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
        static _ Sphinx = Behav()
            .Init(0x0d54, Behaves("Grand Sphinx",
                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Timed.Instance(2500, False.Instance(Flashing.Instance(200, 0xff00ff00))),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(300, RingAttack.Instance(9, projectileIndex: 2)),

                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(500, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 8, projectileIndex: 2)),
                        Cooldown.Instance(500, RingAttack.Instance(9, projectileIndex: 2)),

                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(500, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 8, projectileIndex: 2)),
                        Cooldown.Instance(500, RingAttack.Instance(9, projectileIndex: 2)),

                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(600, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 2, projectileIndex: 2)),
                        Cooldown.Instance(500, MultiAttack.Instance(15, 5 * (float)Math.PI / 180, 8, projectileIndex: 2)),
                        Cooldown.Instance(500, RingAttack.Instance(9, projectileIndex: 2)),

                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Timed.Instance(2500, False.Instance(Flashing.Instance(200, 0xff00ff00))),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                        Timed.Instance(7000, False.Instance(new QueuedBehavior(
                            Cooldown.Instance(300, MultiAttack.Instance(15, 30 * (float)Math.PI / 180, 3, projectileIndex: 0)),
                            Cooldown.Instance(300, RingAttack.Instance(3, projectileIndex: 0))
                        ))),

                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        new SimpleTaunt("You hide like cowards... but you can't hide from this!"),
                        Timed.Instance(2500, False.Instance(Flashing.Instance(200, 0xff00ff00))),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                        AngleMultiAttack.Instance((0 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((0 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((1 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((1 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((2 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((2 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((3 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((3 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((4 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((4 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((0 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((0 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((1 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((1 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((2 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((2 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((3 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((3 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((4 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((4 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000),

                        AngleMultiAttack.Instance((0 * 36) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        AngleMultiAttack.Instance((0 * 36 + 180) * (float)Math.PI / 180, 5 * (float)Math.PI / 180, 11, projectileIndex: 1),
                        CooldownExact.Instance(1000)
                    ),
                    new RunBehaviors(
                        Once.Instance(SpawnMinionImmediate.Instance(0x0d55, 4, 4, 4)),
                        HpLesserPercent.Instance(0.15f,
                            OrderAllEntity.Instance(30, 0x0d55,
                                new SetKey(-1, 1)
                            )
                        ),
                        SmoothWandering.Instance(3, 2)
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.001, (ILoot)new ItemLoot("Helm of the Juggernaut")),
                            Tuple.Create(0.05, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.05, (ILoot)new StatPotionLoot(StatPotion.Wis))
                        ))
                    )
                ))
            .Init(0x0d55, Behaves("Horrid Reaper",
                    new QueuedBehavior(
                        True.Instance(Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))),
                        Timed.Instance(3000, False.Instance(new RunBehaviors(
                            Cooldown.Instance(500,
                                SimpleAttack.Instance(10, projectileIndex: 1)
                            ),
                            Chasing.Instance(4, 15, 0, null)
                        ))),
                        Timed.Instance(3000, False.Instance(new RunBehaviors(
                            Cooldown.Instance(200,
                                SimpleAttack.Instance(10, projectileIndex: 0)
                            ),
                            Chasing.Instance(4, 15, 0, null)
                        ))),
                        Timed.Instance(3000, False.Instance(
                            IfNot.Instance(
                                Chasing.Instance(20, 40, 30, 0x0d54),
                                SimpleWandering.Instance(20)
                            )
                        )),
                        Timed.Instance(8000, False.Instance(
                            Cooldown.Instance(600,
                                RingAttack.Instance(6, offset: 30 * (float)Math.PI / 180, projectileIndex: 0)
                            )
                        ))
                    ),
                    IfExist.Instance(-1, new QueuedBehavior(
                        new SimpleTaunt("OOaoaoAaAoaAAOOAoaaoooaa!!!"),
                        Timed.Instance(500, False.Instance(Flashing.Instance(100, 0xff00ff00))),
                        Despawn.Instance,
                        False.Instance(NullBehavior.Instance)
                    ))
                ));
    }
}
