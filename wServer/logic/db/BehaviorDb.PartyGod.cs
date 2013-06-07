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
        static _ PartyGod = Behav()
            .Init(0x0e56, Behaves("Masked Party God",

            new QueuedBehavior(
                        HpLesser.Instance(10000,
                        Cooldown.Instance(400, Heal.Instance(5, 25000, 0x0e56)),
            new QueuedBehavior(
                        Cooldown.Instance(500, new SimpleTaunt("Oh no, Mixcoatl is my brother, I prefer partying to fighting.")),
                        Cooldown.Instance(1500, new SimpleTaunt("Lets have a fun-time in the sun-shine!")),
                        Cooldown.Instance(2000, new SimpleTaunt("Nothing like relaxin' on the beach.")),
                        Cooldown.Instance(2500, new SimpleTaunt("Chillin' is the name of the game!")),
                        Cooldown.Instance(3000, new SimpleTaunt("I hope you're having a good time!")),
                        Cooldown.Instance(3500, new SimpleTaunt("How do you like my shades?")),
                        Cooldown.Instance(4000, new SimpleTaunt("EVERYBODY BOOGEY!")),
                        Cooldown.Instance(4500, new SimpleTaunt("What a beautiful day!")),
                        Cooldown.Instance(5000, new SimpleTaunt("Whoa there!")),
                        Cooldown.Instance(5500, new SimpleTaunt("Oh SNAP!")),
                        Cooldown.Instance(6000, new SimpleTaunt("Ho!"))
                        ))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 3, 0, 8,
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Blue Paradise")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Pink Passion Breeze")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Bahama Sunrise")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Lime Jungle Bay"))

                        ))
                    )
                ));
    }
}
