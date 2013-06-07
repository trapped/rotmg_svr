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

//namespace wServer.logic
//{
//    partial class BehaviorDb
//    {
//        static _ TombBosses = Behav()
            
//            .Init(0x0d28, Behaves("Tomb Defender",
//           new RunBehaviors(
//                new SetKey(-1, 1),
//                IfEqual.Instance(-1, 1,
//                  new QueuedBehavior(
//                    Circling.Instance(5, 10, 5, 0x0d25),
//                    Once.Instance(new SimpleTaunt("THIS WILL NOW BE YOUR TOMB!")),
//                    HpLesserPercent.Instance(0.99f, new SetKey(-1, 2))
//                    )
//                ),
//                IfEqual.Instance(-1, 2,
//                  new QueuedBehavior(
//                    Timed.Instance(5000, Cooldown.Instance(700, (RingAttack.Instance(25, 10, projectileIndex: 3)))),                   
//                    Once.Instance(new SimpleTaunt("Impudence! I am an immortal, I needn't take you seriously.")),
//                    HpLesserPercent.Instance(0.98f, new SetKey(-1, 3))
//                    )
//                ),
//                IfEqual.Instance(-1, 3,
//                  new QueuedBehavior(
//                       Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Armored)),
//                       Cooldown.Instance(5000, RingAttack.Instance(4, 10, projectileIndex: 1)),
//                       Cooldown.Instance(5000, RingAttack.Instance(5, 10, projectileIndex: 0)),
//                       Cooldown.Instance(2000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 10, 0, projectileIndex: 2))),
//                       HpLesserPercent.Instance(0.90f, new SetKey(-1, 4))
//                    )
//                ),
//                IfEqual.Instance(-1, 4,
//                  new QueuedBehavior(
//                        Cooldown.Instance(5000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 1)),
//                        Cooldown.Instance(5000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
//                        Cooldown.Instance(2000, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 8, 0, projectileIndex: 2)),
//                        HpLesserPercent.Instance(0.25f, new SetKey(-1, 5))
//                    )
//                ),
//                IfEqual.Instance(-1, 5,
//                  new QueuedBehavior(
//                      Once.Instance(new SimpleTaunt("My artifacts shall prove my wall of defense is impenetrable!")),
//                      SpawnMinionImmediate.Instance(0x0d22, 1, 1, 4),
//                      SpawnMinionImmediate.Instance(0x0d23, 1, 1, 4),
//                      SpawnMinionImmediate.Instance(0x0d24, 1, 1, 4),
//                      HpLesserPercent.Instance(0.1f, new SetKey(-1, 6))
//                      )
//                ),
//                IfEqual.Instance(-1, 6,
//                  new QueuedBehavior(
//                            Flashing.Instance(500, 0xffff3333),
//                            Chasing.Instance(6, 6, 0, null),
//                            Cooldown.Instance(5000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 1)),
//                            Cooldown.Instance(5000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
//                            Cooldown.Instance(500, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 4)),
//                            Once.Instance(new SimpleTaunt("The end of your path is here!"))
//                      )
//                ),
//                loot: new LootBehavior(LootDef.Empty,
//                        Tuple.Create(100, new LootDef(0, 2, 0, 2,
//                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Life)),
//                            Tuple.Create(0.05, (ILoot)new ItemLoot("Tome of Holy Protection")),
//                            Tuple.Create(0.25, (ILoot)new ItemLoot("Ring of the Pyramid"))
//                            )
//           )));
           
//            .Init(0x0d26, Behaves("Tomb Support",
//            new RunBehaviors(
//                new SetKey(-1, 1),
//                IfEqual.Instance(-1, 1,
//                  new QueuedBehavior(
//                    Circling.Instance(5, 10, 7, 0x0d25),
//                    Once.Instance(new SimpleTaunt("YOU HAVE AWAKENED US!")),
//                    HpLesserPercent.Instance(0.99f, new SetKey(-1, 2))
//                    )
//                ),
//             );

//            .Init(0x0d27, Behaves("Tomb Attacker",
//           );
        
//    }
//}
