using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;
using wServer.realm.setpieces;
using db;
using wServer.realm;


namespace wServer.realm.entities
{
    partial class Player
    {
        string[] y;
        String AnnounceText;
        String ChatMessage;
        string[] NewsText;
        public void PlayerText(RealmTime time, PlayerTextPacket pkt)
        {
            if (pkt.Text[0] == '/')
            {
                string[] x = pkt.Text.Trim().Split(' ');
                try
                {
                    AnnounceText = pkt.Text.Substring(10);
                    NewsText = pkt.Text.Substring(6).Split(';');
                }
                catch
                {
                    Console.WriteLine("Error at line 24 of Player.Chat.cs");
                }
                ChatMessage = pkt.Text;
                string[] z = pkt.Text.Trim().Split('|');
                y = z.Skip(1).ToArray();
                ProcessCmd(x[0].Trim('/'), x.Skip(1).ToArray());
            }
            else
            {
                if (psr.Account.Admin == true)
                {
                    Owner.BroadcastPacket(new TextPacket()
                        {
                            Name = "@" + psr.Account.Name,
                            Stars = psr.Player.Stars,
                            BubbleTime = 5,
                            Text = pkt.Text
                        }, null);
                }
                else
                {
                    int role;
                    using (Database db1 = new Database())
                    {
                        role = db1.getRole(psr.Account);
                    }
                    if (role >= (int)Database.Roles.Donator)
                    {
                        Owner.BroadcastPacket(new TextPacket()
                        {
                            Name = "#" + psr.Account.Name,
                            Stars = psr.Player.Stars,
                            BubbleTime = 5,
                            Text = pkt.Text
                        }, null);
                    }
                    else
                    {
                        Owner.BroadcastPacket(new TextPacket()
                        {
                            Name = Name,
                            ObjectId = Id,
                            Stars = Stars,
                            BubbleTime = 5,
                            Recipient = "",
                            Text = pkt.Text,
                            CleanText = pkt.Text
                        }, null);
                    }
                }
            }
        }
        //admin=1 in the db, may switch it
        bool CmdReqAdminActive()
        {
            if (!psr.Account.Admin)
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "You are not an admin!"
                });
                return false;
            }
            else
            return true;
        }
        //roles
        bool CmdReqOwner()
        {
            using (Database db1 = new Database())
            {
                if (db1.getRole(psr.Account) >= (int)db.Database.Roles.Owner)
                {
                    return true;
                }
                else
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "You are not an owner!"
                    });
                    return false;
                }
            }
        }
        bool CmdReqAdmin()
        {
            using (Database db1 = new Database())
            {
                if (db1.getRole(psr.Account) >= (int)db.Database.Roles.Admin)
                {
                    return true;
                }
                else
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "You are not an admin!"
                    });
                    return false;
                }
            }
        }
        bool CmdReqGM()
        {
            using (Database db1 = new Database())
            {
                if (db1.getRole(psr.Account) >= (int)db.Database.Roles.Game_Master)
                {
                    return true;
                }
                else
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "You are not a Game Master!"
                    });
                    return false;
                }
            }
        }
        bool CmdReqMod()
        {
            using (Database db1 = new Database())
            {
                if (db1.getRole(psr.Account) >= (int)db.Database.Roles.Moderator)
                {
                    return true;
                }
                else
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "You are not a moderator!"
                    });
                    return false;
                }
            }
        }
        bool CmdReqDonator()
        {
            using (Database db1 = new Database())
            {
                if (db1.getRole(psr.Account) >= (int)db.Database.Roles.Donator)
                {
                    return true;
                }
                else
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "You are not a donator!"
                    });
                    return false;
                }
            }
        }

        void ProcessCmd(string cmd, string[] args)
        {
            if (cmd.Equals("tutorial", StringComparison.OrdinalIgnoreCase))
                psr.Reconnect(new ReconnectPacket()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.TUT_ID,
                    Name = "Tutorial",
                    Key = Empty<byte>.Array,
                });
            //else if (cmd.Equals("a", StringComparison.OrdinalIgnoreCase) && args.Length == 0)
            //{
            //    if (psr.Account.Admin == true)
            //    {
            //        psr.Account.Admin = false;
            //        UpdateCount++;
            //        using(var db1 = new Database())
            //        {
            //            var sqlcmd = db1.CreateQuery();
            //            sqlcmd.CommandText = "UPDATE accounts SET admin = 0 WHERE id = " +psr.Account.AccountId.ToString()+ ";";
            //            sqlcmd.ExecuteNonQuery();
            //        }
            //    }
            //    else
            //    {
            //        psr.Account.Admin = true;
            //        UpdateCount++;
            //        using (var db1 = new Database())
            //        {
            //            var sqlcmd = db1.CreateQuery();
            //            sqlcmd.CommandText = "UPDATE accounts SET admin = 1 WHERE id = " + psr.Account.AccountId.ToString() + ";";
            //            sqlcmd.ExecuteNonQuery();
            //        }
            //    }
            //}
            else if (cmd.Equals("spawn", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdminActive() && CmdReqGM() && args.Length > 0)
            {
                string name = string.Join(" ", args);
                short objType;
                if (!XmlDatas.IdToType.TryGetValue(name, out objType) ||
                    !XmlDatas.ObjectDescs.ContainsKey(objType))
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Unknown entity!"
                    });
                else
                {
                    var entity = Entity.Resolve(objType);
                    entity.Move(X, Y);
                    Owner.EnterWorld(entity);
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Success!"
                    });
                }
            }
            else if (cmd.Equals("g", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var i in RealmManager.Clients.Values)
                {
                    if (i.Account.Guild.Name == psr.Account.Guild.Name)
                    {
                        i.SendPacket(new TextPacket()
                        {
                            BubbleTime = 5,
                            Name = ""+psr.Account.Name,
                            Stars = psr.Player.Stars,
                            Recipient = psr.Account.Guild.Name,
                            Text = ChatMessage.Substring(3)
                        });
                    }
                }
            }
            else if (cmd.Equals("spawnx", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdminActive() && CmdReqGM() && args.Length > 1)
            {
                string name = string.Join(" ", args.Skip(1).ToArray());
                short objType;
                if (!XmlDatas.IdToType.TryGetValue(name, out objType) ||
                    !XmlDatas.ObjectDescs.ContainsKey(objType))
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Unknown entity!"
                    });
                else
                {
                    int c = int.Parse(args[0]);
                    if (c > 50)
                    {
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = "Maximum spawn count is set to 50"
                        });
                        return;
                    }
                    for (int i = 0; i < c; i++)
                    {
                        var entity = Entity.Resolve(objType);
                        entity.Move(X, Y);
                        Owner.EnterWorld(entity);
                    }
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Success!"
                    });
                }
            }
            //else if (cmd.Equals("guild", StringComparison.OrdinalIgnoreCase)) //made this to test if the guild name thing was working, oh well...
            //{
            //    psr.Player.Guild = "gg";//GetGuildName(psr.Account.AccountId);
            //    psr.Player.GuildRank = 1;
            //    UpdateCount++;
            //}
            else if (cmd.Equals("addEff", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqGM() && args.Length == 1)
            {
                try
                {
                    ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim()),
                        DurationMS = -1
                    });
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Success!"
                    });
                }
                catch
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Invalid effect!"
                    });
                }
            }
            else if (cmd.Equals("removeEff", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqGM() && args.Length == 1)
            {
                try
                {
                    ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim()),
                        DurationMS = 0
                    });
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Success!"
                    });
                }
                catch
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Invalid effect!"
                    });
                }
            }
            else if (cmd.Equals("give", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqGM() && args.Length >= 1)
            {
                string name = string.Join(" ", args.ToArray()).Trim();
                short objType;
                if (!XmlDatas.IdToType.TryGetValue(name, out objType))
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Unknown type!"
                    });
                    return;
                }
                for (int i = 0; i < Inventory.Length; i++)
                    if (Inventory[i] == null)
                    {
                        Inventory[i] = XmlDatas.ItemDescs[objType];
                        UpdateCount++;
                        return;
                    }
            }
            else if (cmd.Equals("tp", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqGM() && args.Length >= 2)
            {
                int x, y;
                try
                {
                    x = int.Parse(args[0]);
                    y = int.Parse(args[1]);
                }
                catch
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Invalid coordinates!"
                    });
                    return;
                }
                Move(x + 0.5f, y + 0.5f);
                SetNewbiePeriod();
                UpdateCount++;
                Owner.BroadcastPacket(new GotoPacket()
                {
                    ObjectId = Id,
                    Position = new Position()
                    {
                        X = X,
                        Y = Y
                    }
                }, null);
            }
            //else if (cmd.Equals("teleport", StringComparison.OrdinalIgnoreCase) && args.Length == 1)
            //{
            //    if (psr.Player.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase))
            //    {
            //        psr.SendPacket(new TextPacket()
            //        {
            //            BubbleTime = 0,
            //            Stars = -1,
            //            Name = "",
            //            Text = "You already are at yourself, and always will be!"
            //        });
            //    }

            //    foreach (var i in psr.Player.Owner.Players)
            //    {
            //        if (i.Value.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase))
            //        {
            //            psr.Player.Teleport(timePassed, i.Value.Id);
            //            return true;
            //        }
            //    }
            //    psr.SendPacket(new TextPacket()
            //    {
            //        BubbleTime = 0,
            //        Stars = -1,
            //        Name = "",
            //        Text = string.Format("Unable to find player: {0}", args)
            //    });
            //}
            else if (cmd.Equals("setpiece", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdminActive() && CmdReqGM() && args.Length == 1)
            {
                try
                {
                    ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                        "wServer.realm.setpieces." + args[0]));
                    piece.RenderSetPiece(Owner, new IntPoint((int)X + 1, (int)Y + 1));
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Success!"
                    });
                }
                catch
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Cannot apply setpiece!"
                    });
                }
            }
            else if (cmd.Equals("pause", StringComparison.OrdinalIgnoreCase) && args.Length == 0)
            {
                if (psr.Player.HasConditionEffect(ConditionEffects.Paused) == false)
                {
                    if (psr.Player.Owner.EnemiesCollision.HitTest(psr.Player.X, psr.Player.Y, 8).OfType<Enemy>().Any())
                    {
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = "Not safe enough to pause!"
                        });
                    }
                    else
                    {
                        ApplyConditionEffect(new ConditionEffect()
                        {
                            Effect = ConditionEffectIndex.Paused,
                            DurationMS = -1
                        });
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = "Game paused."
                        });
                    }
                }
                else if (psr.Player.HasConditionEffect(ConditionEffects.Paused) == true)
                {
                    ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = 0
                    });
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Game resumed."
                    });
                }
            }
            //else if (cmd.Equals("level", StringComparison.OrdinalIgnoreCase) && CmdReqAdmin()) //DISABLED, NEEDS DEBUG - WILL PROVIDE AN EXP GIFT CHICKEN TEMPORARILY
            //{
            //    try
            //    {
            //        if (args.Length == 0)
            //        {
            //            psr.Character.Level = psr.Character.Level + 1;
            //            psr.Player.Level = psr.Player.Level + 1;
            //            psr.Player.CheckLevelUp();
            //            UpdateCount++;
            //            psr.SendPacket(new TextPacket()
            //            {
            //                BubbleTime = 0,
            //                Stars = -1,
            //                Name = "",
            //                Text = "Success!"
            //            });
            //        }
            //        else if (args.Length == 1)
            //        {
            //            psr.Character.Level = int.Parse(args[0]);
            //            psr.Player.Level = int.Parse(args[0]);
            //            psr.Player.Experience = GetExpGoal(int.Parse(args[0]));
            //            psr.Character.Exp = GetExpGoal(int.Parse(args[0]));
            //            psr.Player.CheckLevelUp();
            //            UpdateCount++;
            //            psr.SendPacket(new TextPacket()
            //            {
            //                BubbleTime = 0,
            //                Stars = -1,
            //                Name = "",
            //                Text = "Success!"
            //            });
            //        }
            //    }
            //    catch
            //    {
            //        psr.SendPacket(new TextPacket()
            //        {
            //            BubbleTime = 0,
            //            Stars = -1,
            //            Name = "",
            //            Text = "Error!"
            //        });
            //    }
            //}
            else if (cmd.Equals("news", StringComparison.OrdinalIgnoreCase) && CmdReqMod())
            {
                //Console.WriteLine(NewsText);
                String date2 = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                try
                {
                    using (var database1 = new Database())
                    {
                        if (NewsText.Length == 3)
                        {
                            var mysqlcommand = database1.CreateQuery();
                            mysqlcommand.CommandText = "INSERT INTO news(icon, title, text, link, date) VALUES (@icon, @title, @text, @link, @date);";
                            mysqlcommand.Parameters.AddWithValue("@icon", "info");
                            mysqlcommand.Parameters.AddWithValue("@title", NewsText[0]);
                            mysqlcommand.Parameters.AddWithValue("@text", NewsText[1]);
                            mysqlcommand.Parameters.AddWithValue("@link", NewsText[2]);
                            mysqlcommand.Parameters.AddWithValue("@date", date2);
                            if (mysqlcommand.ExecuteNonQuery() > 0)
                            {
                                psr.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Text = "Success!"
                                });
                            }
                            else
                            {
                                psr.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Text = "Error!"
                                });
                            }
                        }
                        else if (NewsText.Length == 2)
                        {
                            var mysqlcommand = database1.CreateQuery();
                            mysqlcommand.CommandText = "INSERT INTO news(icon, title, text, link, date) VALUES (@icon, @title, @text, @link, @date);";
                            mysqlcommand.Parameters.AddWithValue("@icon", "info");
                            mysqlcommand.Parameters.AddWithValue("@title", NewsText[0]);
                            mysqlcommand.Parameters.AddWithValue("@text", NewsText[1]);
                            mysqlcommand.Parameters.AddWithValue("@link", "http://forums.wildshadow.com/");
                            mysqlcommand.Parameters.AddWithValue("@date", date2);
                            if (mysqlcommand.ExecuteNonQuery() > 0)
                            {
                                psr.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Text = "Success!"
                                });
                            }
                            else
                            {
                                psr.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Text = "Error!"
                                });
                            }
                        }
                        else if (NewsText.Length == 1)
                        {
                            var mysqlcommand = database1.CreateQuery();
                            mysqlcommand.Parameters.AddWithValue("@icon", "info");
                            mysqlcommand.Parameters.AddWithValue("@title", NewsText[0].ToString());
                            mysqlcommand.Parameters.AddWithValue("@text", "Default news text");
                            mysqlcommand.Parameters.AddWithValue("@link", "http://forums.wildshadow.com/");
                            mysqlcommand.Parameters.AddWithValue("@date", date2);
                            mysqlcommand.CommandText = "INSERT INTO news(icon, title, text, link, date) VALUES (@icon, @title, @text, @link, @date);";
                            if (mysqlcommand.ExecuteNonQuery() > 0)
                            {
                                psr.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Text = "Success!"
                                });
                            }
                            else
                            {
                                psr.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Text = "Error!"
                                });
                            }
                        }
                        else
                        {
                            psr.SendPacket(new TextPacket()
                            {
                                BubbleTime = 0,
                                Stars = -1,
                                Name = "",
                                Text = "Database error!"
                            });
                        }
                    }
                }
                catch
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Error!"
                    });
                }
            }
            else if (cmd.Equals("admin", StringComparison.OrdinalIgnoreCase) && args.Length == 0 && CmdReqGM())
            {
                try
                {
                    //switches the admin variable and switches the Invincible (not-hittable) effect
                    if (psr.Account.Admin == true)
                    {
                        psr.Account.Admin = false;
                        ApplyConditionEffect(new ConditionEffect()
                            {
                                Effect = ConditionEffectIndex.Invincible,
                                DurationMS = 0
                            });
                        UpdateCount++;
                    }
                    else
                    {
                        psr.Account.Admin = true;
                        ApplyConditionEffect(new ConditionEffect()
                        {
                            Effect = ConditionEffectIndex.Invincible,
                            DurationMS = -1
                        });
                        UpdateCount++;
                    }


                    //below: obsolete
                    //Inventory[0] = XmlDatas.ItemDescs[3840];
                    //Inventory[1] = XmlDatas.ItemDescs[3843];
                    //Inventory[2] = XmlDatas.ItemDescs[3841];
                    //Inventory[3] = XmlDatas.ItemDescs[3845];
                    //UpdateCount++;
                    //psr.SendPacket(new TextPacket()
                    //{
                    //    BubbleTime = 0,
                    //    Stars = -1,
                    //    Name = "",
                    //    Text = "Success!"
                    //});
                }
                catch
                {
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Error!"
                    });
                }
            }
            //else if (cmd.Equals("banana", StringComparison.OrdinalIgnoreCase) && CmdReqAdmin())
            //{
            //    psr.Reconnect(new ReconnectPacket()
            //    {
            //        Host = "",
            //        Port = 2050,
            //        GameId = World.BANANA_ID,
            //        Name = "Banana",
            //        Key = Empty<byte>.Array,
            //    });
            //}
            //else if (cmd.Equals("dye1", StringComparison.OrdinalIgnoreCase))
            //{
            //    int converted = dyes_hextointmod.hextoint(args[0].ToString(), false);
            //    using (var db1 = new Database())
            //    {
            //        using (var mysqlcommand = db1.CreateQuery())
            //        {
            //            mysqlcommand.CommandText = "UPDATE characters SET tex1 = @tex1 WHERE accId=@accId AND charId=@charId AND charId=@charId;";
            //            mysqlcommand.Parameters.AddWithValue("@accId", psr.Account.AccountId);
            //            mysqlcommand.Parameters.AddWithValue("@tex1", converted);
            //            mysqlcommand.Parameters.AddWithValue("@charId", psr.Character.CharacterId);
            //            if (mysqlcommand.ExecuteNonQuery() > 0)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified dye!"
            //                });
            //            else if (mysqlcommand.ExecuteNonQuery() == -1)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified dye!"
            //                });
            //            else
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Error!"
            //                });
            //        }
            //    };
            //    psr.Player.Texture1 = converted;
            //    UpdateCount++;
            //}
            //else if (cmd.Equals("cloth1", StringComparison.OrdinalIgnoreCase))
            //{
            //    int converted = dyes_hextointmod.hextoint(args[0].ToString(), true);
            //    using (var db1 = new Database())
            //    {
            //        using (var mysqlcommand = db1.CreateQuery())
            //        {
            //            mysqlcommand.CommandText = "UPDATE characters SET tex1 = @tex1 WHERE accId=@accId AND charId=@charId AND charId=@charId;";
            //            mysqlcommand.Parameters.AddWithValue("@accId", psr.Account.AccountId);
            //            mysqlcommand.Parameters.AddWithValue("@tex1", converted);
            //            mysqlcommand.Parameters.AddWithValue("@charId", psr.Character.CharacterId);
            //            if (mysqlcommand.ExecuteNonQuery() > 0)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified cloth!"
            //                });
            //            else if (mysqlcommand.ExecuteNonQuery() == -1)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified cloth!"
            //                });
            //            else
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Error!"
            //                });
            //        }
            //    };
            //    psr.Player.Texture1 = converted;
            //    UpdateCount++;
            //}
            //else if (cmd.Equals("dye2", StringComparison.OrdinalIgnoreCase))
            //{
            //    int converted = dyes_hextointmod.hextoint(args[0].ToString(), false);
            //    using (var db1 = new Database())
            //    {
            //        using (var mysqlcommand = db1.CreateQuery())
            //        {
            //            mysqlcommand.CommandText = "UPDATE characters SET tex2 = @tex2 WHERE accId=@accId AND charId=@charId;";
            //            mysqlcommand.Parameters.AddWithValue("@accId", psr.Account.AccountId);
            //            mysqlcommand.Parameters.AddWithValue("@tex2", converted);
            //            mysqlcommand.Parameters.AddWithValue("@charId", psr.Character.CharacterId);
            //            if (mysqlcommand.ExecuteNonQuery() > 0)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified dye!"
            //                });
            //            else if (mysqlcommand.ExecuteNonQuery() == -1)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified dye!"
            //                });
            //            else
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Error!"
            //                });
            //        }
            //    };
            //    psr.Player.Texture2 = converted;
            //    UpdateCount++;
            //}
            //else if (cmd.Equals("cloth2", StringComparison.OrdinalIgnoreCase))
            //{
            //    int converted = dyes_hextointmod.hextoint(args[0].ToString(), true);
            //    using (var db1 = new Database())
            //    {
            //        using (var mysqlcommand = db1.CreateQuery())
            //        {
            //            mysqlcommand.CommandText = "UPDATE characters SET tex2 = @tex2 WHERE accId=@accId AND charId=@charId;";
            //            mysqlcommand.Parameters.AddWithValue("@accId", psr.Account.AccountId);
            //            mysqlcommand.Parameters.AddWithValue("@tex2", converted);
            //            mysqlcommand.Parameters.AddWithValue("@charId", psr.Character.CharacterId);
            //            if (mysqlcommand.ExecuteNonQuery() > 0)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified cloth!"
            //                });
            //            else if (mysqlcommand.ExecuteNonQuery() == -1)
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Modified cloth!"
            //                });
            //            else
            //                psr.SendPacket(new TextPacket()
            //                {
            //                    BubbleTime = 0,
            //                    Stars = -1,
            //                    Name = "",
            //                    Text = "Error!"
            //                });
            //        }
            //    };
            //    psr.Player.Texture2 = converted;
            //    UpdateCount++;
            //}
            else if (cmd.Equals("who", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder sb = new StringBuilder("Players online: ");
                var copy = psr.Player.Owner.Players.Values.ToArray();
                if (copy.Length == 0)
                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = "Nobody else is online"
                    });
                else
                {
                    for (int i = 0; i < copy.Length; i++)
                    {
                        if (i != 0) sb.Append(", ");
                        sb.Append(copy[i].Name);
                    }

                    psr.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "",
                        Text = sb.ToString()
                    });
                }
            }
            else if (cmd.Equals("server", StringComparison.OrdinalIgnoreCase))
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = psr.Player.Owner.Name
                });
            }
            else if (cmd.Equals("tell", StringComparison.OrdinalIgnoreCase))
            {
                if (!psr.Player.NameChosen)
                {
                    psr.SendPacket(new TextPacket() { BubbleTime = 0, Stars = -1, Name = "", Text = "Choose a name!" });
                    return;
                }
                if (args.Length < 1)
                {
                    psr.SendPacket(new TextPacket() { BubbleTime = 0, Stars = -1, Name = "", Text = "Usage: /tell <player name> <text>" });
                    return;
                }
                string playername = args[0];
                string msg = ChatMessage.Substring(cmd.Length + args[0].Length + 2);
                if (psr.Player.Name.ToLower() == playername.ToLower())
                {
                    psr.SendPacket(new TextPacket() { BubbleTime = 0, Stars = -1, Name = "", Text = "You cannot tell yourself!" });
                    return;
                }
                int clientcount = 0;
                foreach (var i in RealmManager.Clients.Values)
                {
                    if (i.Account.Name.ToLower() == playername.ToLower())
                    {
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 5,
                            Stars = psr.Player.Stars,
                            Recipient = playername,
                            Text = msg
                        });
                        i.SendPacket(new TextPacket()
                        {
                            BubbleTime = 5,
                            Stars = psr.Player.Stars,
                            Name = psr.Account.Name,
                            Recipient = i.Account.Name,
                            Text = msg
                        });
                        return;
                    }
                    else
                    {
                        clientcount = clientcount + 1;
                    }
                };
                if (clientcount == RealmManager.Clients.Values.ToArray().Length)
                {
                    psr.SendPacket(new TextPacket() { BubbleTime = 0, Stars = -1, Name = "", Text = String.Format("{0} not found", playername) });
                }
            }
            else if (cmd.Equals("announce", StringComparison.OrdinalIgnoreCase) && CmdReqMod())
            {

                foreach (var i in RealmManager.Clients.Values)
                {
                    i.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "@Announcement",
                        Text = AnnounceText.ToString()
                    });
                }
            }
            else if (cmd.Equals("killall", StringComparison.OrdinalIgnoreCase) && CmdReqGM())
            {
                foreach (var i in RealmManager.Worlds)
                {
                    World world = i.Value;
                    if (i.Key != 0)
                    {
                        foreach (var e in world.Enemies)
                        {
                            //TODO
                        }
                    }
                }
            }
            else
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "Unknown command!"
                });
            }
            //else if (cmd.Equals("setStat", StringComparison.OrdinalIgnoreCase))
            //{
            //    Char chr = psr.Character;
            //    try
            //    {
            //        if (args.Length == 0)
            //        {
            //            chr.Attack = chr.Attack + 100;//hp0 mp1 att2 def3 spd4 dex5 vit6 wis7
            //            chr.Defense = chr.Defense + 100;
            //            chr.Speed = chr.Speed + 100;
            //            chr.Dexterity = chr.Dexterity + 100;
            //            chr.HpRegen = chr.HpRegen + 100;
            //            chr.MpRegen = chr.MpRegen + 100;
            //            UpdateCount++;
            //        }
            //    }
            //    catch
            //    {
            //        psr.SendPacket(new TextPacket()
            //        {
            //            BubbleTime = 0,
            //            Stars = -1,
            //            Name = "",
            //            Text = "Error!"
            //        });
            //    }
            //}
        }
    }
}

