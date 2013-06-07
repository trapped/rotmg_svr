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
        static _ LotLL = Behav()
            .Init(0x0d50, Behaves("Lord of the Lost Lands",
                    new RunBehaviors(
                        SimpleWandering.Instance(2, 2)
                    ),
                    
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Def)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Wis)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Dex)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Att)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Spd))
                    )))
            
            ));
    }
}