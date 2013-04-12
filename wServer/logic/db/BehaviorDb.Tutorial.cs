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
        static _ Tutorial = Behav()
            .Init(0x0802, Behaves("South Tutorial Gun", attack: TutorialTower.Instance))
            .Init(0x0803, Behaves("North Tutorial Gun", attack: TutorialTower.Instance))
            .Init(0x0804, Behaves("East Tutorial Gun", attack: TutorialTower.Instance))
            .Init(0x0805, Behaves("West Tutorial Gun", attack: TutorialTower.Instance))

            .Init(0x06af, Behaves("Evil Chicken", SimpleWandering.Instance(3)))
            .Init(0x06b1, Behaves("Evil Chicken God",
                IfNot.Instance(
                    Chasing.Instance(4, 5, 3, null), SimpleWandering.Instance(3)),
                SpawnMinion.Instance(0x06b0, 5, 3, 2000, 5000)))
            .Init(0x06b0, Behaves("Evil Chicken Minion",
                IfNot.Instance(
                    Chasing.Instance(4, 5, 1, 0x06b1), SimpleWandering.Instance(3)),
                NullBehavior.Instance))
            .Init(0x06b2, Behaves("Evil Hen", SimpleWandering.Instance(3),
                loot: new LootBehavior(
                        new LootDef(1, 0, 1, 1,
                            Tuple.Create(1.0, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))

            .Init(0x06b3, Behaves("Kitchen Guard",
                IfNot.Instance(
                    Chasing.Instance(6, 8, 6, null), SimpleWandering.Instance(4)),
                Cooldown.Instance(1000, SimpleAttack.Instance(8))))
            .Init(0x06b4, Behaves("Butcher",
                IfNot.Instance(
                    Chasing.Instance(8, 8, 1, null), SimpleWandering.Instance(4)),
                Cooldown.Instance(1000, SimpleAttack.Instance(8)),
                loot: new LootBehavior(
                        new LootDef(2, 0, 0, 2,
                            Tuple.Create(0.2, (ILoot)HpPotionLoot.Instance)
                        )
                    )
                ))
            .Init(0x0926, Behaves("Bonegrind the Butcher",
                IfNot.Instance(
                    Escaping.Instance(4, 5, 100, null),
                    IfNot.Instance(
                        Chasing.Instance(8, 8, 1, null), SimpleWandering.Instance(4))),
                Cooldown.Instance(1000,
                    Rand.Instance(SimpleAttack.Instance(8), RingAttack.Instance(4, 8))),
                loot: new LootBehavior(
                        new LootDef(2, 0, 0, 2,
                            Tuple.Create(0.1, (ILoot)HpPotionLoot.Instance),
                            Tuple.Create(0.1, (ILoot)MpPotionLoot.Instance)
                        )
                    ),
                condBehaviors: new ConditionalBehavior[] { new ChefTaunt(100) }
                ));
    }
}
