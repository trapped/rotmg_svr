using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using wServer.svrPackets;
using wServer.cliPackets;
using System.Xml;
using db;
using wServer.realm;
using wServer.realm.entities;

namespace wServer
{
    public enum ProtocalStage
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }
    public class ClientProcessor
    {
        Socket skt;
        Thread wkrThread;
        public RC4 ReceiveKey { get; private set; }
        public RC4 SendKey { get; private set; }

        public ClientProcessor(Socket skt)
        {
            this.skt = skt;
            ReceiveKey = new RC4(new byte[] { 0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1b, 0x09, 0xa1, 0x3a, 0x2a, 0x6e });
            SendKey = new RC4(new byte[] { 0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcb, 0xd7, 0x4b, 0x80 });
        }

        public void BeginProcess()
        {
            wkrThread = new Thread(Process);
            wkrThread.Start();
        }

        Queue<Packet> pending = new Queue<Packet>();        //TODO: thread safety
        AutoResetEvent sendLock = new AutoResetEvent(false);
        public void SendPacket(Packet pkt)
        {
            try
            {
                pending.Enqueue(pkt);
                sendLock.Set();
            }
            catch
            {
                Console.WriteLine("Error sending packet with ID "+pkt.ID.ToString());
            }
        }
        public void SendPackets(IEnumerable<Packet> pkts)
        {
            try
            {
                foreach (var i in pkts)
                    pending.Enqueue(i);
                sendLock.Set();
            }
            catch
            {
                Console.WriteLine("Error sending packets, check ClientProcessor.cs");
            }
        }

        void ReceivePacket(Packet pkt)
        {
            if (stage == ProtocalStage.Disconnected) return;
            if (stage == ProtocalStage.Ready && (entity == null || entity != null && entity.Owner == null)) return;
            Packet.BeginReadPacket(skt, this, ReceivePacket, Disconnect);
            try
            {
                if (pkt.ID == PacketID.Hello)
                    ProcessHelloPacket(pkt as HelloPacket);
                else if (pkt.ID == PacketID.Create)
                    ProcessCreatePacket(pkt as CreatePacket);
                else if (pkt.ID == PacketID.Load)
                    ProcessLoadPacket(pkt as LoadPacket);
                else if (pkt.ID == PacketID.Pong)
                    entity.Pong(pkt as PongPacket);
                else if (pkt.ID == PacketID.Move)
                    RealmManager.AddPendingAction(t => entity.Move(t, pkt as MovePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.PlayerShoot)
                    RealmManager.AddPendingAction(t => entity.PlayerShoot(t, pkt as PlayerShootPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.EnemyHit)
                    RealmManager.AddPendingAction(t => entity.EnemyHit(t, pkt as EnemyHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.OtherHit)
                    RealmManager.AddPendingAction(t => entity.OtherHit(t, pkt as OtherHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.SquareHit)
                    RealmManager.AddPendingAction(t => entity.SquareHit(t, pkt as SquareHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.PlayerHit)
                    RealmManager.AddPendingAction(t => entity.PlayerHit(t, pkt as PlayerHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ShootAck)
                    RealmManager.AddPendingAction(t => entity.ShootAck(t, pkt as ShootAckPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.InvSwap)
                    RealmManager.AddPendingAction(t => entity.InventorySwap(t, pkt as InvSwapPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.InvDrop)
                    RealmManager.AddPendingAction(t => entity.InventoryDrop(t, pkt as InvDropPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.UseItem)
                    RealmManager.AddPendingAction(t => entity.UseItem(t, pkt as UseItemPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.UsePortal)
                    RealmManager.AddPendingAction(t => entity.UsePortal(t, pkt as UsePortalPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.PlayerText)
                    RealmManager.AddPendingAction(t => entity.PlayerText(t, pkt as PlayerTextPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ChooseName)
                    RealmManager.AddPendingAction(t => ProcessChooseNamePacket(pkt as ChooseNamePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.Escape)
                    RealmManager.AddPendingAction(t => ProcessEscapePacket(pkt as EscapePacket), PendingPriority.Emergent);
                else if (pkt.ID == PacketID.Teleport)
                    RealmManager.AddPendingAction(t => entity.Teleport(t, pkt as TeleportPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.GotoAck)
                    RealmManager.AddPendingAction(t => entity.GotoAck(t, pkt as GotoAckPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.EditAccountList)
                    RealmManager.AddPendingAction(t => entity.EditAccountList(t, pkt as EditAccountListPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.Buy)
                    RealmManager.AddPendingAction(t => entity.Buy(t, pkt as BuyPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.RequestTrade)
                    RealmManager.AddPendingAction(t => entity.RequestTrade(t, pkt as RequestTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ChangeTrade)
                    RealmManager.AddPendingAction(t => entity.ChangeTrade(t, pkt as ChangeTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.AcceptTrade)
                    RealmManager.AddPendingAction(t => entity.AcceptTrade(t, pkt as AcceptTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.CancelTrade)
                    RealmManager.AddPendingAction(t => entity.CancelTrade(t, pkt as CancelTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.AOEAck)
                    RealmManager.AddPendingAction(t => entity.AOEAck(t, pkt as AOEAckPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.GroundDamage)
                    RealmManager.AddPendingAction(t => entity.GroundDamage(t, pkt as GroundDamagePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.CheckCredits)
                    RealmManager.AddPendingAction(t => entity.CheckCredits(t, pkt as CheckCreditsPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.CreateGuild)
                    RealmManager.AddPendingAction(t => entity.CreateGuild(pkt as CreateGuildPacket),PendingPriority.Networking);
                else if (pkt.ID != PacketID.Packet)
                {
                    Console.WriteLine("Unhandled packet: " + pkt.ToString());
                }
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (stage == ProtocalStage.Disconnected) return;
                var original = stage;
                stage = ProtocalStage.Disconnected;
                if (account != null)
                    DisconnectFromRealm();
                if (db0 != null && original != ProtocalStage.Ready)
                {
                    db0.Dispose();
                    db0 = null;
                }
                skt.Close();
                sendLock.Set();
            }
            catch
            {
                Console.WriteLine("Error disconnecting client, check ClientProcessor.cs");
            }
        }
        public void Save()
        {
            if (db0 != null)
            {
                if (character != null)
                {
                    entity.SaveToCharacter();
                    db0.SaveCharacter(account, character);
                }
                db0.Dispose();
                db0 = null;
            }
        }

        void Process()
        {
            try
            {
                stage = ProtocalStage.Connected;
                Packet.BeginReadPacket(skt, this, ReceivePacket, Disconnect);
                NWriter wtr = new NWriter(new NetworkStream(skt));
                while (sendLock.WaitOne())
                {
                    if (stage == ProtocalStage.Disconnected) break;
                    while (pending.Count > 0)
                    {
                        Packet pkt = pending.Dequeue();
                        //if (pkt.ID != PacketID.New_Tick &&
                        //    pkt.ID != PacketID.Ping)
                        //    Console.WriteLine(pkt.ID);
                        Packet.WritePacket(wtr, pkt, this);
                    }
                }
            }
            catch
            {
                Disconnect();
            }
        }

        Database db0;
        Account account;
        Char character;
        Player entity;
        bool isGuest = false;
        ProtocalStage stage;

        public Database Database { get { return db0; } }
        public Char Character { get { return character; } }
        public Account Account { get { return account; } }
        public ProtocalStage Stage { get { return stage; } }
        public Player Player { get { return entity; } }
        public wRandom Random { get; private set; }

        int targetWorld = -1;
        void ProcessHelloPacket(HelloPacket pkt)
        {
            Console.WriteLine("Accepting new client...");
            db0 = new Database();
            if ((account = db0.Verify(pkt.GUID, pkt.Password)) == null)
            {
                Console.WriteLine("Account not verified.");
                account = Database.CreateGuestAccount(pkt.GUID);

                //Database.SaveCharacter(account, Database.CreateCharacter(782, 1));
                 //Database.Register(pkt.GUID, pkt.Password, true);

                if (account == null)
                {
                    Console.WriteLine("Account is null");
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Invalid account."
                    });
                    
                    return;
                }
                
            }
            Console.WriteLine("Client trying to connect");
            if (!RealmManager.TryConnect(this))
            {
                if (CheckAccountInUse(account.AccountId) != false)
                {
                    Console.WriteLine("Account in use: " + account.AccountId);
                    account = null;
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Account in use! Retrying..."
                    });
                    Disconnect();
                    return;
                }
                account = null;
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Failed to connect."
                });
                Console.WriteLine("Failed to connect.");
            }
            else
            {
                Console.WriteLine("Client loading world");
                World world = RealmManager.GetWorld(pkt.GameId);
                if (world == null)
                {
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Invalid world."
                    });
                    Console.WriteLine("Invalid world");
                }
                else
                {
                    Console.WriteLine("Client joined world " + world.Id.ToString());
                    if (world.Id == -6) //Test World
                        (world as realm.worlds.Test).LoadJson(pkt.MapInfo);
                    else if (world.IsLimbo)
                        world = world.GetInstance(this);

                    var seed = (uint)((long)Environment.TickCount * pkt.GUID.GetHashCode()) % uint.MaxValue;
                    Random = new wRandom(seed);
                    targetWorld = world.Id;
                    SendPacket(new MapInfoPacket()
                    {
                        Width = world.Map.Width,
                        Height = world.Map.Height,
                        Name = world.Name,
                        Seed = seed,
                        Background = world.Background,
                        AllowTeleport = world.AllowTeleport,
                        ShowDisplays = world.ShowDisplays,
                        ClientXML = world.ClientXML,
                        ExtraXML = world.ExtraXML
                    });
                    stage = ProtocalStage.Handshaked;
                    Console.WriteLine("End of client world packet");
                }
            }
        }

        void ProcessCreatePacket(CreatePacket pkt)
        {
            Console.WriteLine("Client char create packet");
            int nextCharId = 1;
            nextCharId = db0.GetNextCharID(account);
            var cmd = db0.CreateQuery();
            cmd.CommandText = "SELECT maxCharSlot FROM accounts WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", account.AccountId);
            object maxChar1 = cmd.ExecuteScalar();
            int maxChar = int.Parse(maxChar1.ToString());
            cmd = db0.CreateQuery();
            cmd.CommandText = "SELECT COUNT(id) FROM characters WHERE accId=@accId AND dead = FALSE;";
            cmd.Parameters.AddWithValue("@accId", account.AccountId);
            object currChar1 = cmd.ExecuteScalar();
            int currChar = int.Parse(currChar1.ToString());

            if (currChar >= maxChar)
            {
                Disconnect();
            }
            if (CheckAccountInUse(account.AccountId) != false)
            {
                Console.WriteLine("Account in use: " + account.AccountId);
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Account in use! Retrying..."
                });
                return;
            }
            Console.WriteLine("Client second char create packet");

            character = Database.CreateCharacter(pkt.ObjectType, nextCharId);

            int[] stats = new int[]
            {
                character.MaxHitPoints,
                character.MaxMagicPoints,
                character.Attack,
                character.Defense,
                character.Speed,
                character.Dexterity,
                character.HpRegen,
                character.MpRegen,
            };

            bool ok = true;
            cmd = db0.CreateQuery();
            cmd.Parameters.AddWithValue("@accId", account.AccountId);
            cmd.Parameters.AddWithValue("@charId", nextCharId);
            cmd.Parameters.AddWithValue("@charType", pkt.ObjectType);
            cmd.Parameters.AddWithValue("@items", character._Equipment);
            cmd.Parameters.AddWithValue("@stats", Utils.GetCommaSepString(stats));
            cmd.Parameters.AddWithValue("@fameStats", character.FameStats.ToString());
            cmd.CommandText = "INSERT INTO characters (accId, charId, charType, level, exp, fame, items, hp, mp, stats, dead, pet, fameStats) VALUES (@accId, @charId, @charType, 1, 0, 0, @items, 100, 100, @stats, FALSE, -1, @fameStats);";
            int v = cmd.ExecuteNonQuery();
            //int v = 1;
            ok = v > 0;

            Console.WriteLine("Client char create packet result: "+ok);

            if (ok)
            {
                SendPacket(new CreateResultPacket()
                {
                    CharacterID = character.CharacterId,
                    ObjectID = RealmManager.Worlds[targetWorld].EnterWorld(entity = new Player(this))
                });
                stage = ProtocalStage.Ready;
            }
            else
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Failed to Load character."
                });
        }

        void ProcessLoadPacket(LoadPacket pkt)
        {
            Console.WriteLine("Client load packet");
            character = db0.LoadCharacter(account, pkt.CharacterId);
            if (character != null)
            {
                if (character.Dead)
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Character is dead."
                    });
                else
                {
                    SendPacket(new CreateResultPacket()
                    {
                        CharacterID = character.CharacterId,
                        ObjectID = RealmManager.Worlds[targetWorld].EnterWorld(entity = new Player(this))
                    });
                    stage = ProtocalStage.Ready;
                }
            }
            else
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Failed to Load character."
                });
        }

        void ProcessChooseNamePacket(ChooseNamePacket pkt)
        {
            bool namechosen = false;
            var cmdx = db0.CreateQuery();
            cmdx.CommandText = "SELECT namechosen FROM accounts WHERE id=@accId";
            cmdx.Parameters.AddWithValue("@accId",account.AccountId);
            object execx = cmdx.ExecuteScalar();
            namechosen = bool.Parse(execx.ToString());

            if (string.IsNullOrEmpty(pkt.Name) ||
                pkt.Name.Length > 10)
            {
                SendPacket(new NameResultPacket()
                {
                    Success = false,
                    Message = "Invalid name"
                });
                return;
            }
            else
            {
                var cmd = db0.CreateQuery();
                cmd.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name;";
                cmd.Parameters.AddWithValue("@name", pkt.Name);
                object x = cmd.ExecuteScalar();
                if (int.Parse(x.ToString()) > 0)
                    SendPacket(new NameResultPacket()
                    {
                        Success = false,
                        Message = "Duplicated name"
                    });
                else
                {
                    db0.ReadStats(account);
                    if (account.Credits < 1000 && namechosen == true)
                        SendPacket(new NameResultPacket()
                        {
                            Success = false,
                            Message = "Not enough credits"
                        });
                    else
                    {
                        if (account.NameChosen == false)
                        {
                            cmd = db0.CreateQuery();
                            cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                            cmd.Parameters.AddWithValue("@accId", account.AccountId);
                            cmd.Parameters.AddWithValue("@name", pkt.Name);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                entity.Name = pkt.Name;
                                entity.NameChosen = true;
                                entity.UpdateCount++;
                                SendPacket(new NameResultPacket()
                                {
                                    Success = true,
                                    Message = "Success!"
                                });
                            }
                            else
                                SendPacket(new NameResultPacket()
                                {
                                    Success = false,
                                    Message = "Internal Error"
                                });
                        }
                        else
                        {
                            cmd = db0.CreateQuery();
                            cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                            cmd.Parameters.AddWithValue("@accId", account.AccountId);
                            cmd.Parameters.AddWithValue("@name", pkt.Name);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                entity.Credits = db0.UpdateCredit(account, -1000);
                                entity.Name = pkt.Name;
                                entity.NameChosen = true;
                                entity.UpdateCount++;
                                SendPacket(new NameResultPacket()
                                {
                                    Success = true,
                                    Message = "Success!"
                                });
                            }
                            else
                                SendPacket(new NameResultPacket()
                                {
                                    Success = false,
                                    Message = "Internal Error"
                                });
                        }
                    }
                }
            }
        }

        void ProcessEscapePacket(EscapePacket pkt)
        {
            Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = World.NEXUS_ID,
                Name = "Nexus",
                Key = Empty<byte>.Array,
            });
        }

        void DisconnectFromRealm()
        {
            RealmManager.AddPendingAction(t =>
            {
                if (Player != null)
                    Player.SaveToCharacter();
                Save();
                RealmManager.Disconnect(this);
            }, PendingPriority.Destruction);
        }
        public void Reconnect(ReconnectPacket pkt)
        {
            RealmManager.AddPendingAction(t =>
            {
                if (Player != null)
                    Player.SaveToCharacter();
                Save();
                RealmManager.Disconnect(this);
                SendPacket(pkt);
            }, PendingPriority.Destruction);
        }
        void CreateGuildPacket()
        {

        }
        //public bool CheckAccountInUse(string uuid)
        //{
        //    int count = 0;
        //    int accId = 0;
        //    using(db.Database dbx = new Database())
        //    {
        //        var cmd = dbx.CreateQuery();
        //        cmd.CommandText = "SELECT id FROM accounts WHERE uuid = @uuid";
        //        cmd.Parameters.AddWithValue("@uuid", uuid);
        //        if (cmd.ExecuteNonQuery() != 0)
        //        {
        //            object id;
        //            id = cmd.ExecuteScalar();
        //            accId = int.Parse(id.ToString());
        //        }
        //    }
        //    foreach (var i in RealmManager.Clients.Values)
        //    {
        //        if (i.Account.AccountId == accId)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            count = count + 1;
        //        }
        //    }
        //    if (count == RealmManager.Clients.ToArray().Length)
        //    {
        //        return false;
        //    }
        //    return false;
        //}
        //public bool CheckAccountInUse(int accId)
        //{
        //    int count = 0;
        //    foreach (var i in RealmManager.Clients.Values)
        //    {
        //        if (i.Account.AccountId == accId)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            count = count + 1;
        //        }
        //    }
        //    if (count == RealmManager.Clients.ToArray().Length)
        //    {
        //        return false;
        //    }
        //    return false;
        //}
        public bool CheckAccountInUse(int accId)
        {
            try
            {
                int count = 0;
                for (int i = 0; i < RealmManager.Worlds.Values.Count; i++)
                {
                    World w = RealmManager.GetWorld(i);
                    foreach (var plr in w.Players.Values)
                    {
                        if (plr.AccountId == accId)
                        {
                            return true;
                        }
                        else
                        {
                            count = count + 1;
                        }
                    }
                    if (count == w.Players.Values.ToArray().Length)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                Console.WriteLine("Error checking if account "+accId+" is in use, check ClientProcessor.cs");
                return false;
            }
        }
    }
}
