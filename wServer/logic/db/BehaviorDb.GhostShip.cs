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
        static _ gship = Behav()
            .Init(0x0e37, Behaves("Ghost Ship",
                    new RunBehaviors(
                        SimpleWandering.Instance(2, 2),
                        Cooldown.Instance(2000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                        Cooldown.Instance(4000, Once.Instance(RingAttack.Instance(6, 0, 5, projectileIndex: 1)))
                    ),
                    
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Wis)),
                            Tuple.Create(0.5, (ILoot)new ItemLoot("Ghost Pirate Rum"))
                    )))
            
            ));
    }
}