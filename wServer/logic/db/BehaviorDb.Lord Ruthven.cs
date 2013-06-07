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
        static _ Manor = Behav()
            .Init(0x1720, Behaves("Lord Ruthven",
                    new RunBehaviors(
                        SimpleWandering.Instance(2, 2),
                        Timed.Instance(4500, Cooldown.Instance(350, RingAttack.Instance(20, 0, 5, projectileIndex: 1))),
                        Cooldown.Instance(1100, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 4, 0, projectileIndex: 0))
                    ),
                    
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(0.08, (ILoot)new ItemLoot("Tome of Purification")),
                            Tuple.Create(0.5, (ILoot)new ItemLoot("Holy Water")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("St. Abraham's Wand")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Chasuble of Holy Light")),
                            Tuple.Create(0.12, (ILoot)new ItemLoot("Ring of Divine Faith"))
                    )))
            
            ));
    }
}