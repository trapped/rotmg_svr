using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db;
using System.Threading;
using System.Diagnostics;
using System.IO;
using wServer.realm.worlds;
using System.Collections.Concurrent;

namespace wServer.realm
{
    public struct RealmTime
    {
        public long tickCount;
        public long tickTimes;
        public int thisTickCounts;
        public int thisTickTimes;
    }

    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Networking,
        Normal,
        Creation,
    }

    public static class RealmManager
    {
        static RealmManager()
        {
            pendings = new ConcurrentQueue<Action<RealmTime>>[5];
            for (int i = 0; i < 5; i++)
                pendings[i] = new ConcurrentQueue<Action<RealmTime>>();

            Worlds[World.TUT_ID] = new Tutorial(true);
            Worlds[World.NEXUS_ID] = Worlds[0] = new Nexus();
            Worlds[World.NEXUS_LIMBO] = new NexusLimbo();
            Worlds[World.VAULT_ID] = new Vault(true);
            Worlds[World.TEST_ID] = new Test();
            Worlds[World.RAND_REALM] = new RandomRealm();
            Worlds[World.BANANA_ID] = new Banana();

            Monitor = new RealmPortalMonitor(Worlds[World.NEXUS_ID] as Nexus);
            
            AddWorld(new GameWorld(1, "Medusa", true));
        }

        public const int MAX_CLIENT = 10;

        static int nextWorldId = 0;
        public static readonly ConcurrentDictionary<int, World> Worlds = new ConcurrentDictionary<int, World>();
        public static readonly ConcurrentDictionary<int, ClientProcessor> Clients = new ConcurrentDictionary<int, ClientProcessor>();

        public static RealmPortalMonitor Monitor { get; private set; }

        public static void AddPendingAction(Action<RealmTime> callback)
        {
            AddPendingAction(callback, PendingPriority.Normal);
        }
        public static void AddPendingAction(Action<RealmTime> callback, PendingPriority priority)
        {
            pendings[(int)priority].Enqueue(callback);
        }
        static readonly ConcurrentQueue<Action<RealmTime>>[] pendings;


        public static bool TryConnect(ClientProcessor psr)
        {
            if (Clients.Count >= MAX_CLIENT)
                return false;
            else
                return Clients.TryAdd(psr.Account.AccountId, psr);
        }
        public static void Disconnect(ClientProcessor psr)
        {
            Clients.TryRemove(psr.Account.AccountId, out psr);
        }

        public static World AddWorld(World world)
        {
            world.Id = Interlocked.Increment(ref nextWorldId);
            Worlds[world.Id] = world;
            if (world is GameWorld)
                Monitor.WorldAdded(world);
            return world;
        }
        public static World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        public const int TPS = 30;
        public const int MsPT = 1000 / TPS;
        public const bool EnableMonitor = false;
        public static void CoreTickLoop()
        {
            //if (EnableMonitor)
            //    new Thread(_ => svrMonitor.Mon.Show()) { IsBackground = true }.Start();

            Stopwatch watch = new Stopwatch();
            long dt = 0;
            long count = 0;

            watch.Start();
            RealmTime t = new RealmTime();
            long xa = 0;
            do
            {
                long times = dt / MsPT;
                dt -= times * MsPT;
                times++;

                long b = watch.ElapsedMilliseconds;

                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).KeyChar)
                    {
                        case 'c':
                            Console.Clear();
                            break;
                        default:
                            Console.WriteLine("Tick| time:" + times + " dt:" + dt + " count:" + count + " time:" + b + " fps:" + count / (b / 1000.0));
                            break;
                    }
                }
                count += times;
                if (times > 3)
                    Console.WriteLine("LAGGED!| time:" + times + " dt:" + dt + " count:" + count + " time:" + b + " fps:" + count / (b / 1000.0));

                t.tickTimes = b;
                t.tickCount = count;
                t.thisTickCounts = (int)times;
                t.thisTickTimes = (int)(times * MsPT);
                xa += t.thisTickTimes;

                foreach (var i in Clients)
                    if (i.Value.Stage == ProtocalStage.Disconnected)
                    {
                        ClientProcessor psr;
                        Clients.TryRemove(i.Key, out psr);
                    }

                foreach (var i in pendings)
                {
                    Action<RealmTime> callback;
                    while (i.TryDequeue(out callback))
                    {
                        try
                        {
                            callback(t);
                        }
                        catch { }
                    }
                }
                TickWorlds1(t);

                Thread.Sleep(MsPT);
                dt += Math.Max(0, watch.ElapsedMilliseconds - b - MsPT);

            } while (true);
        }

        static void TickWorlds1(RealmTime t)    //Continous simulation
        {
            foreach (var i in Worlds.Values.Distinct())
                i.Tick(t);
            if (EnableMonitor)
                svrMonitor.Mon.Tick(t);
        }

        static void TickWorlds2(RealmTime t)    //Discrete simulation
        {
            long counter = t.thisTickTimes;
            long c = t.tickCount - t.thisTickCounts;
            long x = t.tickTimes - t.thisTickTimes;
            while (counter >= MsPT)
            {
                c++; x += MsPT;
                TickWorlds1(new RealmTime()
                {
                    thisTickCounts = 1,
                    thisTickTimes = MsPT,
                    tickCount = c,
                    tickTimes = x
                });
                counter -= MsPT;
            }
        }
    }
}
