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
        static _ Undead_Lair = Behav()
            .Init(0x0d90, Behaves("Septavius the Ghost God",
                new RunBehaviors(
                  SmoothWandering.Instance(0.5f, 0.5f),
                  Once.Instance(SpawnMinionImmediate.Instance(0x0db0, 0, 4, 6)),
                  Once.Instance(SpawnMinionImmediate.Instance(0x0db1, 0, 4, 6)),
                  Once.Instance(SpawnMinionImmediate.Instance(0x0db2, 0, 4, 6))
                ),
                Once.Instance(IsEntityPresent.Instance(1, null)),
                new QueuedBehavior(
        #region Circle Attack 1
                
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),
        #endregion 

        #region Circle Attack 2
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),
        #endregion 
                    //not used
        #region Circle Attack 3
                    /*Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),*/
        #endregion 
         
        #region Circle Attack 4
                    /*Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),*/
        #endregion
                    //end

        #region RingAttack + Flashing 1

                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Flashing.Instance(500, 0x0000FF0C),
                        Flashing.Instance(500, 0x0000FF0C),
                        Flashing.Instance(500, 0x0000FF0C),
                        Flashing.Instance(500, 0x0000FF0C),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),

        #endregion

        #region Flashing 2

                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

        #endregion

        #region Quite + Confuse
                        //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        //end confuse
                        //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite
                        //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        //end confuse
                        //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite
                        //confuse
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        //end confuse
                        //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite
                        //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        //end confuse
                        //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite
                        //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        //end confuse
                        //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite
                        //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        //end confuse
                        //quite
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite
                        //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        //end confuse
                        //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        //end quite

                        Cooldown.Instance(1500),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

        #endregion
                        

        #region Spawn Minions + Circleshoot

        #region Spawn Minions

                        //SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        //IsEntityNotPresent.Instance(100, 0x0db0),
                        //IsEntityNotPresent.Instance(100, 0x0db1),
                        //IsEntityNotPresent.Instance(100, 0x0db2),
                        //UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                        SpawnMinionImmediate.Instance(0x0db0, 0, 4, 6),
                        SpawnMinionImmediate.Instance(0x0db1, 0, 4, 6),
                        SpawnMinionImmediate.Instance(0x0db2, 0, 4, 6),

        #endregion

        #region Circleshoot

        #region Circle Attack 1

                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),
        #endregion 

        #region Circle Attack 2

                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(1100))
        #endregion 

        #endregion

        #endregion
            ),
            loot: new LootBehavior(LootDef.Empty,
                Tuple.Create(100, new LootDef(0, 5, 0, 8,
                    Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Wis)),
                    Tuple.Create(0.009, (ILoot)new ItemLoot("Doom Bow")),
                    Tuple.Create(0.005, (ILoot)new ItemLoot("Wine Cellar Incantation")),

                    Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ring)),
                    Tuple.Create(0.3, (ILoot)new TierLoot(7, ItemType.Weapon)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(8, ItemType.Weapon)),
                    Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ability)),
                    Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ability)),
                    Tuple.Create(0.1, (ILoot)new TierLoot(5, ItemType.Ability))
                    )))

                ))
                .Init(0x0db0, Behaves("Ghost Warrior of Septavius",
                    IfNot.Instance(
                      Chasing.Instance(12, 7, 1, null),
                        IfNot.Instance(
                          Chasing.Instance(10, 7, 1, 0x0d90),
                          SimpleWandering.Instance(4f)
                          )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.6, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0db1, Behaves("Ghost Mage of Septavius",
                    IfNot.Instance(
                      Chasing.Instance(12, 7, 1, null),
                        IfNot.Instance(
                          Chasing.Instance(10, 7, 1, 0x0d90),
                          SimpleWandering.Instance(4f)
                          )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.6, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0db2, Behaves("Ghost Rogue of Septavius",
                    IfNot.Instance(
                      Chasing.Instance(12, 7, 1, null),
                        IfNot.Instance(
                          Chasing.Instance(10, 7, 1, 0x0d90),
                          SimpleWandering.Instance(4f)
                          )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.6, (ILoot)PotionLoot.Instance)
                            ))
                ));
    }
}
