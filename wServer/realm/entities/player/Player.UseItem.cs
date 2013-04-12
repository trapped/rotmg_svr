using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;
using wServer.logic;

namespace wServer.realm.entities
{
    partial class Player
    {
        static readonly ConditionEffect[] NegativeEffs = new ConditionEffect[]
        {
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Slowed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Paralyzed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Weak,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Stunned,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Confused,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Blind,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Quiet,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.ArmorBroken,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Bleeding,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Dazed,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Sick,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Drunk,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Hallucinating,
                DurationMS = 0
            },
            new ConditionEffect()
            {
                Effect = ConditionEffectIndex.Hexed,
                DurationMS = 0
            }
        };

        public void UseItem(RealmTime time, UseItemPacket pkt)
        {
            IContainer container = Owner.GetEntity(pkt.Slot.ObjectId) as IContainer;
            var item = container.Inventory[pkt.Slot.SlotId];
            Activate(time, item, pkt.Position);
            if (item.Consumable)
            {
                if (item.SuccessorId != null)
                    container.Inventory[pkt.Slot.SlotId] = XmlDatas.ItemDescs[XmlDatas.IdToType[item.SuccessorId]];
                else
                    container.Inventory[pkt.Slot.SlotId] = null;
                UpdateCount++;
            }
            if (container.SlotTypes[pkt.Slot.SlotId] != -1)
                fames.UseAbility();
        }

        static void ActivateHealHp(Player player, int amount, List<Packet> pkts)
        {
            int maxHp = player.Stats[0] + player.Boost[0];
            int newHp = Math.Min(maxHp, player.HP + amount);
            if (newHp != player.HP)
            {
                pkts.Add(new ShowEffectPacket()
                {
                    EffectType = EffectType.Potion,
                    TargetId = player.Id,
                    Color = new ARGB(0xffffffff)
                });
                pkts.Add(new NotificationPacket()
                {
                    Color = new ARGB(0xff00ff00),
                    ObjectId = player.Id,
                    Text = "+" + (newHp - player.HP)
                });
                player.HP = newHp;
                player.UpdateCount++;
            }
        }
        static void ActivateHealMp(Player player, int amount, List<Packet> pkts)
        {
            int maxMp = player.Stats[1] + player.Boost[1];
            int newMp = Math.Min(maxMp, player.MP + amount);
            if (newMp != player.MP)
            {
                pkts.Add(new ShowEffectPacket()
                {
                    EffectType = EffectType.Potion,
                    TargetId = player.Id,
                    Color = new ARGB(0xffffffff)
                });
                pkts.Add(new NotificationPacket()
                {
                    Color = new ARGB(0xff9000ff),
                    ObjectId = player.Id,
                    Text = "+" + (newMp - player.MP)
                });
                player.MP = newMp;
                player.UpdateCount++;
            }
        }
        void ActivateShoot(RealmTime time, Item item, Position target)
        {
            var arcGap = item.ArcGap * Math.PI / 180;
            var startAngle = Math.Atan2(target.Y - Y, target.X - X) - (item.NumProjectiles - 1) / 2 * arcGap;
            var prjDesc = item.Projectiles[0]; //Assume only one

            for (int i = 0; i < item.NumProjectiles; i++)
            {
                Projectile proj = CreateProjectile(prjDesc, item.ObjectType,
                    (int)statsMgr.GetAttackDamage(prjDesc.MinDamage, prjDesc.MaxDamage),
                    time.tickTimes, new Position() { X = X, Y = Y }, (float)(startAngle + arcGap * i));
                Owner.EnterWorld(proj);
                fames.Shoot(proj);
            }
        }
        void PoisonEnemy(Enemy enemy, ActivateEffect eff)
        {
            int remainingDmg = (int)StatsManager.GetDefenseDamage(enemy, eff.TotalDamage, enemy.ObjectDesc.Defense);
            int perDmg = (int)(remainingDmg * 1000 / eff.DurationMS);
            WorldTimer tmr = null;
            int x = 0;
            tmr = new WorldTimer(100, (w, t) =>
            {
                if (enemy.Owner == null) return;
                w.BroadcastPacket(new ShowEffectPacket()
                {
                    EffectType = EffectType.Dead,
                    TargetId = enemy.Id,
                    Color = new ARGB(0xffddff00)
                }, null);

                if (x % 10 == 0)
                {
                    int thisDmg;
                    if (remainingDmg < perDmg) thisDmg = remainingDmg;
                    else thisDmg = perDmg;

                    enemy.Damage(this, t, thisDmg, true);
                    remainingDmg -= thisDmg;
                    if (remainingDmg <= 0) return;
                }
                x++;

                tmr.Reset();

                RealmManager.AddPendingAction(_ => w.Timers.Add(tmr), PendingPriority.Creation);
            });
            Owner.Timers.Add(tmr);
        }

        void Activate(RealmTime time, Item item, Position target)
        {
            MP -= item.MpCost;
            foreach (var eff in item.ActivateEffects)
            {
                switch (eff.Effect)
                {
                    case ActivateEffects.BulletNova:
                        {
                            var prjDesc = item.Projectiles[0]; //Assume only one
                            Packet[] batch = new Packet[21];
                            uint s = Random.CurrentSeed;
                            Random.CurrentSeed = (uint)(s * time.tickTimes);
                            for (int i = 0; i < 20; i++)
                            {
                                Projectile proj = CreateProjectile(prjDesc, item.ObjectType,
                                    (int)statsMgr.GetAttackDamage(prjDesc.MinDamage, prjDesc.MaxDamage),
                                    time.tickTimes, target, (float)(i * (Math.PI * 2) / 20));
                                Owner.EnterWorld(proj);
                                fames.Shoot(proj);
                                batch[i] = new ShootPacket()
                                {
                                    BulletId = proj.ProjectileId,
                                    OwnerId = Id,
                                    ContainerType = item.ObjectType,
                                    Position = target,
                                    Angle = proj.Angle,
                                    Damage = (short)proj.Damage
                                };
                            }
                            Random.CurrentSeed = s;
                            batch[20] = new ShowEffectPacket()
                            {
                                EffectType = EffectType.Trail,
                                PosA = target,
                                TargetId = Id,
                                Color = new ARGB(0xFFFF00AA)
                            };
                            Owner.BroadcastPackets(batch, null);
                        } break;
                    case ActivateEffects.Shoot:
                        {
                            ActivateShoot(time, item, target);
                        } break;
                    case ActivateEffects.StatBoostSelf:
                        {
                            int idx = -1;
                            switch ((StatsType)eff.Stats)
                            {
                                case StatsType.MaximumHP: idx = 0; break;
                                case StatsType.MaximumMP: idx = 1; break;
                                case StatsType.Attack: idx = 2; break;
                                case StatsType.Defense: idx = 3; break;
                                case StatsType.Speed: idx = 4; break;
                                case StatsType.Vitality: idx = 5; break;
                                case StatsType.Wisdom: idx = 6; break;
                                case StatsType.Dexterity: idx = 7; break;
                            }
                            int s = eff.Amount;
                            Boost[idx] += s;
                            UpdateCount++;
                            Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                            {
                                Boost[idx] -= s;
                                UpdateCount++;
                            }));
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.Potion,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff)
                            }, null);
                        } break;
                    case ActivateEffects.StatBoostAura:
                        {
                            int idx = -1;
                            switch ((StatsType)eff.Stats)
                            {
                                case StatsType.MaximumHP: idx = 0; break;
                                case StatsType.MaximumMP: idx = 1; break;
                                case StatsType.Attack: idx = 2; break;
                                case StatsType.Defense: idx = 3; break;
                                case StatsType.Speed: idx = 4; break;
                                case StatsType.Vitality: idx = 5; break;
                                case StatsType.Wisdom: idx = 6; break;
                                case StatsType.Dexterity: idx = 7; break;
                            }
                            int s = eff.Amount;
                            Behavior.AOE(Owner, this, eff.Range / 2, true, player =>
                            {
                                (player as Player).Boost[idx] += s;
                                player.UpdateCount++;
                                Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                                {
                                    (player as Player).Boost[idx] -= s;
                                    player.UpdateCount++;
                                }));
                            });
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = eff.Range / 2 }
                            }, null);
                        } break;
                    case ActivateEffects.ConditionEffectSelf:
                        {
                            ApplyConditionEffect(new ConditionEffect()
                            {
                                Effect = eff.ConditionEffect.Value,
                                DurationMS = eff.DurationMS
                            });
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = 1 }
                            }, null);
                        } break;
                    case ActivateEffects.ConditionEffectAura:
                        {
                            Behavior.AOE(Owner, this, eff.Range / 2, true, player =>
                            {
                                ApplyConditionEffect(new ConditionEffect()
                                {
                                    Effect = eff.ConditionEffect.Value,
                                    DurationMS = eff.DurationMS
                                });
                            });
                            uint color = 0xffffffff;
                            if (eff.ConditionEffect.Value == ConditionEffectIndex.Damaging)
                                color = 0xffff0000;
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(color),
                                PosA = new Position() { X = eff.Range / 2 }
                            }, null);
                        } break;
                    case ActivateEffects.Heal:
                        {
                            List<Packet> pkts = new List<Packet>();
                            ActivateHealHp(this, eff.Amount, pkts);
                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.HealNova:
                        {
                            List<Packet> pkts = new List<Packet>();
                            Behavior.AOE(Owner, this, eff.Range / 2, true, player =>
                            {
                                ActivateHealHp(player as Player, eff.Amount, pkts);
                            });
                            pkts.Add(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = eff.Range / 2 }
                            });
                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.Magic:
                        {
                            List<Packet> pkts = new List<Packet>();
                            ActivateHealMp(this, eff.Amount, pkts);
                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.MagicNova:
                        {
                            List<Packet> pkts = new List<Packet>();
                            Behavior.AOE(Owner, this, eff.Range / 2, true, player =>
                            {
                                ActivateHealMp(player as Player, eff.Amount, pkts);
                            });
                            pkts.Add(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = eff.Range / 2 }
                            });
                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.Teleport:
                        {
                            Move(target.X, target.Y);
                            UpdateCount++;
                            Owner.BroadcastPackets(new Packet[]
                            {
                                new GotoPacket()
                                {
                                    ObjectId = Id,
                                    Position = new Position()
                                    {
                                        X = X,
                                        Y = Y
                                    }
                                },
                                new ShowEffectPacket()
                                {
                                    EffectType = EffectType.Teleport,
                                    TargetId = Id,
                                    PosA = new Position()
                                    {
                                        X = X,
                                        Y = Y
                                    },
                                    Color = new ARGB(0xFFFFFFFF)
                                }
                            }, null);
                        } break;
                    case ActivateEffects.VampireBlast:
                        {
                            List<Packet> pkts = new List<Packet>();
                            pkts.Add(new ShowEffectPacket()
                            {
                                EffectType = EffectType.Trail,
                                TargetId = Id,
                                PosA = target,
                                Color = new ARGB(0xFFFF0000)
                            });
                            pkts.Add(new AOEPacket()
                            {
                                Position = target,
                                Radius = eff.Radius,
                                Damage = (ushort)eff.TotalDamage,
                                EffectDuration = 0,
                                Effects = 0,
                                OriginType = item.ObjectType
                            });

                            int totalDmg = 0;
                            var enemies = new List<Enemy>();
                            Behavior.AOE(Owner, target, eff.Radius, false, enemy =>
                            {
                                enemies.Add(enemy as Enemy);
                                totalDmg += (enemy as Enemy).Damage(this, time, eff.TotalDamage, false);
                            });
                            var players = new List<Player>();
                            Behavior.AOE(Owner, this, eff.Radius, true, player =>
                            {
                                players.Add(player as Player);
                                ActivateHealHp(player as Player, totalDmg, pkts);
                            });

                            Random rand = new System.Random();
                            for (int i = 0; i < 5; i++)
                            {
                                Enemy a = enemies[rand.Next(0, enemies.Count)];
                                Player b = players[rand.Next(0, players.Count)];
                                pkts.Add(new ShowEffectPacket()
                                {
                                    EffectType = EffectType.Flow,
                                    TargetId = b.Id,
                                    PosA = new Position() { X = a.X, Y = a.Y },
                                    Color = new ARGB(0xffffffff)
                                });
                            }

                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.Trap:
                        {
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.Throw,
                                Color = new ARGB(0xff9000ff),
                                TargetId = Id,
                                PosA = target
                            }, null);
                            Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                            {
                                Trap trap = new Trap(
                                    this,
                                    eff.Radius,
                                    eff.TotalDamage,
                                    eff.ConditionEffect ?? ConditionEffectIndex.Slowed,
                                    eff.EffectDuration);
                                trap.Move(target.X, target.Y);
                                world.EnterWorld(trap);
                            }));
                        } break;
                    case ActivateEffects.StasisBlast:
                        {
                            List<Packet> pkts = new List<Packet>();

                            pkts.Add(new ShowEffectPacket()
                            {
                                EffectType = EffectType.Concentrate,
                                TargetId = Id,
                                PosA = target,
                                PosB = new Position() { X = target.X + 3, Y = target.Y },
                                Color = new ARGB(0xffffffff)
                            });
                            Behavior.AOE(Owner, target, 3, false, enemy =>
                            {
                                if (enemy.HasConditionEffect(ConditionEffects.StasisImmune))
                                {
                                    pkts.Add(new NotificationPacket()
                                    {
                                        ObjectId = enemy.Id,
                                        Color = new ARGB(0xff00ff00),
                                        Text = "Immune"
                                    });
                                }
                                else if (!enemy.HasConditionEffect(ConditionEffects.Stasis))
                                {
                                    enemy.ApplyConditionEffect(
                                        new ConditionEffect()
                                        {
                                            Effect = ConditionEffectIndex.Stasis,
                                            DurationMS = eff.DurationMS
                                        },
                                        new ConditionEffect()
                                        {
                                            Effect = ConditionEffectIndex.Confused,
                                            DurationMS = eff.DurationMS
                                        }
                                    );
                                    Owner.Timers.Add(new WorldTimer(eff.DurationMS, (world, t) =>
                                    {
                                        enemy.ApplyConditionEffect(new ConditionEffect()
                                            {
                                                Effect = ConditionEffectIndex.StasisImmune,
                                                DurationMS = 3000
                                            }
                                        );
                                    }
                                    ));
                                    pkts.Add(new NotificationPacket()
                                    {
                                        ObjectId = enemy.Id,
                                        Color = new ARGB(0xffff0000),
                                        Text = "Statis"
                                    });
                                }
                            });
                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.Decoy:
                        {
                            var decoy = new Decoy(this, eff.DurationMS, statsMgr.GetSpeed());
                            decoy.Move(X, Y);
                            Owner.EnterWorld(decoy);
                        } break;
                    case ActivateEffects.Lightning:
                        {
                            Enemy start = null;
                            double angle = Math.Atan2(target.Y - Y, target.X - X);
                            double diff = Math.PI / 3;
                            Behavior.AOE(Owner, target, 6, false, enemy =>
                            {
                                if (!(enemy is Enemy)) return;
                                var x = Math.Atan2(enemy.Y - Y, enemy.X - X);
                                if (Math.Abs(angle - x) < diff)
                                {
                                    start = enemy as Enemy;
                                    diff = Math.Abs(angle - x);
                                }
                            });
                            if (start == null)
                                break;

                            Enemy current = start;
                            Enemy[] targets = new Enemy[eff.MaxTargets];
                            for (int i = 0; i < targets.Length; i++)
                            {
                                targets[i] = current;
                                float dist = 8;
                                Enemy next = Behavior.GetNearestEntity(current, ref dist, false,
                                    enemy =>
                                        enemy is Enemy &&
                                        Array.IndexOf(targets, enemy) == -1 &&
                                        Behavior.Dist(this, enemy) <= 6) as Enemy;

                                if (next == null) break;
                                else current = next;
                            }

                            List<Packet> pkts = new List<Packet>();
                            for (int i = 0; i < targets.Length; i++)
                            {
                                if (targets[i] == null) break;
                                Entity prev = i == 0 ? (Entity)this : targets[i - 1];
                                targets[i].Damage(this, time, eff.TotalDamage, false);
                                pkts.Add(new ShowEffectPacket()
                                {
                                    EffectType = EffectType.Lightning,
                                    TargetId = prev.Id,
                                    Color = new ARGB(0xffff0088),
                                    PosA = new Position()
                                    {
                                        X = targets[i].X,
                                        Y = targets[i].Y
                                    },
                                    PosB = new Position() { X = 350 }
                                });
                            }
                            Owner.BroadcastPackets(pkts, null);
                        } break;
                    case ActivateEffects.PoisonGrenade:
                        {
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.Throw,
                                Color = new ARGB(0xffddff00),
                                TargetId = Id,
                                PosA = target
                            }, null);
                            Placeholder x = new Placeholder(1500);
                            x.Move(target.X, target.Y);
                            Owner.EnterWorld(x);
                            Owner.Timers.Add(new WorldTimer(1500, (world, t) =>
                            {
                                Owner.BroadcastPacket(new ShowEffectPacket()
                                {
                                    EffectType = EffectType.AreaBlast,
                                    Color = new ARGB(0xffddff00),
                                    TargetId = x.Id,
                                    PosA = new Position() { X = eff.Radius }
                                }, null);
                                List<Enemy> enemies = new List<Enemy>();
                                Behavior.AOE(world, target, eff.Radius, false,
                                    enemy => PoisonEnemy(enemy as Enemy, eff));
                            }));
                        } break;
                    case ActivateEffects.RemoveNegativeConditions:
                        {
                            Behavior.AOE(Owner, this, eff.Range / 2, true, player =>
                            {
                                ApplyConditionEffect(NegativeEffs);
                            });
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = eff.Range / 2 }
                            }, null);
                        } break;
                    case ActivateEffects.RemoveNegativeConditionsSelf:
                        {
                            ApplyConditionEffect(NegativeEffs);
                            Owner.BroadcastPacket(new ShowEffectPacket()
                            {
                                EffectType = EffectType.AreaBlast,
                                TargetId = Id,
                                Color = new ARGB(0xffffffff),
                                PosA = new Position() { X = 1 }
                            }, null);
                        } break;
                    case ActivateEffects.IncrementStat:
                        {
                            int idx = -1;
                            switch ((StatsType)eff.Stats)
                            {
                                case StatsType.MaximumHP: idx = 0; break;
                                case StatsType.MaximumMP: idx = 1; break;
                                case StatsType.Attack: idx = 2; break;
                                case StatsType.Defense: idx = 3; break;
                                case StatsType.Speed: idx = 4; break;
                                case StatsType.Vitality: idx = 5; break;
                                case StatsType.Wisdom: idx = 6; break;
                                case StatsType.Dexterity: idx = 7; break;
                            }
                            Stats[idx] += eff.Amount;
                            int limit = int.Parse(XmlDatas.TypeToElement[ObjectType].Element(StatsManager.StatsIndexToName(idx)).Attribute("max").Value);
                            if (Stats[idx] > limit)
                                Stats[idx] = limit;
                            UpdateCount++;
                        } break;
                    case ActivateEffects.Pet:
                    case ActivateEffects.Create:
                    case ActivateEffects.UnlockPortal:
                        break;
                }
            }
            UpdateCount++;
        }
    }
}
