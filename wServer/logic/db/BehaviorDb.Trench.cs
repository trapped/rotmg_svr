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
        static _ Trench = Behav()
            .Init(0x1706, Behaves("Thessal the Mermaid Goddess",
              NullBehavior.Instance,
                new RunBehaviors(
                  HpGreaterEqual.Instance(40000, SmoothWandering.Instance(5f, 1f)),
                  Cooldown.Instance(2500, RingAttack.Instance(16, 75, 75, projectileIndex: 0)),
                  Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 2)),
                  Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 90 * (float)Math.PI / 180, projectileIndex: 2)),
                  Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 180 * (float)Math.PI / 180, projectileIndex: 2)),
                  Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 360 * (float)Math.PI / 180, projectileIndex: 2)),
                  Cooldown.Instance(1000, MultiAttack.Instance(15, 10 * (float)Math.PI / 180, 2, 270 * (float)Math.PI / 180, projectileIndex: 2))
                  ),
                  If.Instance(
                    And.Instance(HpLesser.Instance(40000, NullBehavior.Instance), HpLesser.Instance(40000, NullBehavior.Instance)),
                      new RunBehaviors(
                      Once.Instance(Flashing.Instance(500, 0x01ADFF2F)),
                      Cooldown.Instance(550, Once.Instance(Flashing.Instance(500, 0x01ADFF2F))),
                      Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                      ReturnSpawn.Instance(40),
                      Cooldown.Instance(5000, Once.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))),
                      Cooldown.Instance(2500, RingAttack.Instance(16, 75, 75, projectileIndex: 0)),
                      Cooldown.Instance(3000, RingAttack.Instance(30, 75, 75, projectileIndex: 3)),
                      Cooldown.Instance(1000, PredictiveMultiAttack.Instance(20, 20, 2, 8, projectileIndex: 1))
                      )
                  ),
                  loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 5, 2, 5,
                      Tuple.Create(1.00, (ILoot)new ItemLoot("Potion of Mana")),
                      Tuple.Create(0.05, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                      Tuple.Create(0.02, (ILoot)new ItemLoot("Coral Bow")),
                      Tuple.Create(0.2, (ILoot)new ItemLoot("Coral Silk Armor")),
                      Tuple.Create(0.2, (ILoot)new ItemLoot("Coral Venom Trap")),
                      Tuple.Create(0.2, (ILoot)new ItemLoot("Coral Ring"))
                      )))
            ))
            .Init(0x1708, Behaves("Fishman",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 6, 0x1700),
                    IfNot.Instance(
                      Circling.Instance(3, 10, 6, null),
                      SimpleWandering.Instance(4)
                      )
                  ),
                  Cooldown.Instance(800, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)),
                  Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 1))
            ))
            .Init(0x1700, Behaves("Fishman Warrior",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 3, null),
                  IfNot.Instance(
                    Circling.Instance(3, 10, 6, null),
                    SimpleWandering.Instance(4)
                    )
                ),
                new RunBehaviors(
                  Cooldown.Instance(800, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)),
                  Cooldown.Instance(1000, SimpleAttack.Instance(3, projectileIndex: 1)),
                  Cooldown.Instance(2000, RingAttack.Instance(5, 10, 0, projectileIndex: 2))
                  )
             ))
            .Init(0x170a, Behaves("Sea Mare",
                IfNot.Instance(
                  Charge.Instance(10, 10, null),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(500, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                  Cooldown.Instance(1500, SimpleAttack.Instance(3, projectileIndex: 1))
            ))
            .Init(0x170c, Behaves("Giant Squid",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 6, null),
                  IfNot.Instance(
                    Cooldown.Instance(500, TossEnemy.Instance(0, 0, 0x170b)),
                    SimpleWandering.Instance(4)
                    
                    )
                  ),
                  Cooldown.Instance(100, SimpleAttack.Instance(10, projectileIndex: 0)),
                  Cooldown.Instance(500, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 1))
            ))
            .Init(0x170b, Behaves("Ink Bubble",
                Cooldown.Instance(100, RingAttack.Instance(1, 1, 0, projectileIndex: 0))
            ))
            .Init(0x1707, Behaves("Deep Sea Beast",
                SetSize .Instance(100),
                  new QueuedBehavior(
                    Cooldown.Instance(50, SimpleAttack.Instance(3, projectileIndex: 0)),
                    Cooldown.Instance(100, SimpleAttack.Instance(3, projectileIndex: 1)),
                    Cooldown.Instance(150, SimpleAttack.Instance(3, projectileIndex: 2)),
                    Cooldown.Instance(200, SimpleAttack.Instance(3, projectileIndex: 3)),
                    CooldownExact.Instance(300)
                    )
            ))
            .Init(0x1709, Behaves("Sea Horse",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 1, 0x170a),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(660, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0))
            ))
            .Init(0x170e, Behaves("Grey Sea Slurp",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 2, 0x170d),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(500, SimpleAttack.Instance(8, projectileIndex: 0)),
                  Cooldown.Instance(500, RingAttack.Instance(8, 4, 0, projectileIndex: 1))
            ))
            .Init(0x170d, Behaves("Sea Slurp Home",
                new QueuedBehavior(
                    Cooldown.Instance(500, RingAttack.Instance(8, 4, 0, projectileIndex: 0)),
                    Cooldown.Instance(500, RingAttack.Instance(8, 2, 0, projectileIndex: 1))
                    )
            ));
    }
}