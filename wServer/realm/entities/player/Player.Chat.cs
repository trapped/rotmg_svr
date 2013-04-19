using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;
using wServer.realm.setpieces;
using db;


namespace wServer.realm.entities
{
    partial class Player
    {
        string[] y;
        public void PlayerText(RealmTime time, PlayerTextPacket pkt)
        {
            if (pkt.Text[0] == '/')
            {
                string[] x = pkt.Text.Trim().Split(' ');
                string[] z = pkt.Text.Trim().Split('|');
                y = z.Skip(1).ToArray();
                ProcessCmd(x[0].Trim('/'), x.Skip(1).ToArray());
                
            }
            else
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

        bool CmdReqAdmin()
        {
            /*
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
             */
                return true;
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
            else if (cmd.Equals("spawn", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdmin() && args.Length > 0)
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
            else if (cmd.Equals("spawnx", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdmin() && args.Length > 1)
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
            else if (cmd.Equals("addEff", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdmin() && args.Length == 1)
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
                     CmdReqAdmin() && args.Length == 1)
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
                     CmdReqAdmin() && args.Length >= 1)
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
                     CmdReqAdmin() && args.Length >= 2)
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
            else if (cmd.Equals("setpiece", StringComparison.OrdinalIgnoreCase) &&
                     CmdReqAdmin() && args.Length == 1)
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
            else if (cmd.Equals("pause", StringComparison.OrdinalIgnoreCase))
            {
                try
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
            else if (cmd.Equals("level", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    if (args.Length == 0)
                    {
                        psr.Character.Level = psr.Character.Level + 1;
                        psr.Player.Level = psr.Player.Level + 1;
                        psr.Player.CheckLevelUp();
                        UpdateCount++;
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = "Success!"
                        });
                    }
                    else if (args.Length == 1)
                    {
                        psr.Character.Level = int.Parse(args[0]);
                        psr.Player.Level = int.Parse(args[0]);
                        psr.Player.CheckLevelUp();
                        UpdateCount++;
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Text = "Success!"
                        });
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
            else if (cmd.Equals("news", StringComparison.OrdinalIgnoreCase))
            {
                string[] args2 = y;
                Console.WriteLine(y.ToString());
                return;
                DateTime date1 = DateTime.Parse(DateTime.Now.ToString());
                String date2 = date1.Year.ToString() + "-" + date1.Month.ToString() + "-" + date1.Day.ToString() + " " + date1.Hour.ToString() + ":" + date1.Minute.ToString() + ":" + date1.Second.ToString();
                try
                {
                    using (var database1 = new Database())
                    {
                        if (args2.Length == 3)
                        {
                            var mysqlcommand = database1.CreateQuery();
                            mysqlcommand.CommandText = "INSERT INTO news(icon, title, text, link, date) VALUES (@icon, @title, @text, @link, @date);";
                            mysqlcommand.Parameters.AddWithValue("@icon", "info");
                            mysqlcommand.Parameters.AddWithValue("@title", args2[0]);
                            mysqlcommand.Parameters.AddWithValue("@text", args2[1]);
                            mysqlcommand.Parameters.AddWithValue("@link", args2[2]);
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
                        else if (args.Length == 2)
                        {
                            var mysqlcommand = database1.CreateQuery();
                            mysqlcommand.CommandText = "INSERT INTO news(icon, title, text, link, date) VALUES (@icon, @title, @text, @link, @date);";
                            mysqlcommand.Parameters.AddWithValue("@icon", "info");
                            mysqlcommand.Parameters.AddWithValue("@title", args2[0]);
                            mysqlcommand.Parameters.AddWithValue("@text", args2[1]);
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
                        else if (args.Length == 1)
                        {
                            var mysqlcommand = database1.CreateQuery();
                            mysqlcommand.Parameters.AddWithValue("@icon", "info");
                            mysqlcommand.Parameters.AddWithValue("@title", args2[0].ToString());
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
            else if (cmd.Equals("admin", StringComparison.OrdinalIgnoreCase) && args.Length == 0)
            {
                try
                {
                    Inventory[0] = XmlDatas.ItemDescs[3840];
                    Inventory[1] = XmlDatas.ItemDescs[3843];
                    Inventory[2] = XmlDatas.ItemDescs[3841];
                    Inventory[3] = XmlDatas.ItemDescs[3845];
                    UpdateCount++;
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
