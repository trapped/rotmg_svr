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
        static _ Crystal = Behav()
            .Init(0x0935, Behaves("Mysterious Crystal",
               new RunBehaviors(new SetKey(-1, 1),
                IfEqual.Instance(-1, 1,
                  new QueuedBehavior(
                    new SimpleTaunt("Break the crystal for great rewards..."),
                    new RandomTaunt(0.1, "Help me..."),
                    CooldownExact.Instance(10000, new SetKey(-1, 1)),
                    HpLesserPercent.Instance(0.9f, new SetKey(-1, 2))
                    )
                ),
                IfEqual.Instance(-1, 2,
                  new QueuedBehavior(
                    Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                    Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                    new SimpleTaunt("Fire upon this crystal with all your might for 5 seconds"),
                    new RandomTaunt(0.8, "If your attacks are weak, the crystal magically heals"),
                    new RandomTaunt(0.8, "Gather a large group to smash it open"),
                    HpLesserPercent.Instance(0.85f, new SetKey(-1, 3))
                    )
                ),
                IfEqual.Instance(-1, 3,
                  new QueuedBehavior(
                    new SimpleTaunt("Sweet treasure awaits for powerful adventurers!"),
                    new RandomTaunt(0.4, "Yes!  Smash my prison for great rewards!"),
                    CooldownExact.Instance(5000, new SetKey(-1, 4))
                    )
                ),
                IfEqual.Instance(-1, 4,
                  new QueuedBehavior(
                    new SimpleTaunt("If you are not very strong, this could kill you"),
                    new RandomTaunt(0.3, "If you are not yet powerful, stay away from the Crystal"),
                    new RandomTaunt(0.3, "New adventurers should stay away"),
                    new RandomTaunt(0.3, "That's the spirit. Lay your fire upon me."),
                    new RandomTaunt(0.3, "So close..."),
                    CooldownExact.Instance(5000, new SetKey(-1, 5))
                    )
                ),
                IfEqual.Instance(-1, 5,
                  new QueuedBehavior(
                      new SimpleTaunt("I think you need more people..."),
                      new RandomTaunt(0.4, "Call all your friends to help you break the crystal!"),
                      HpLesserPercent.Instance(0.8f, new SetKey(-1, 7)),
                      CooldownExact.Instance(10000, new SetKey(-1, 6))
                      )
                ),
                IfEqual.Instance(-1, 6,
                  new QueuedBehavior(
                      new SimpleTaunt("Perhaps you need a bigger group. Ask others to join you!"),
                      Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                      Heal.Instance(5000, 1000000, 0x0935),
                      RingAttack.Instance(16, 22, 16, projectileIndex: 0),
                      Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                      CooldownExact.Instance(5000, new SetKey(-1, 1))
                    )
                ),
               IfEqual.Instance(-1, 7,
                  new RunBehaviors(
                    new SimpleTaunt("You cracked the crystal! Soon we shall emerge!"),
                    Cooldown.Instance(1000, SetSize.Instance(80)),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                    Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                    CooldownExact.Instance(4000, new SetKey(-1, 8))
                    )
                ),
                IfEqual.Instance(-1, 8,
                  new QueuedBehavior(
                    new SimpleTaunt("This your reward! Imagine what evil even Oryx needs to keep locked up!"),
                    RingAttack.Instance(16, 22, 16, projectileIndex: 0),
                    SpawnMinionImmediate.Instance(0x0941, 0, 1, 1),
                    Despawn.Instance
                    )
                )
           )));
    }
}