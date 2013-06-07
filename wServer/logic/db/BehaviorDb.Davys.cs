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
        static _ Davy = Behav()
            .Init(0x0e32, Behaves("Davy Jones",
            new RunBehaviors(
                SimpleWandering.Instance(2, 5),
                        Cooldown.Instance(2000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                        Cooldown.Instance(4000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 1))
                        ))
        );
    }
}