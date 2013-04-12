using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{
    public interface IBehaviorHost
    {
        IDictionary<int, object> StateStorage { get; }
        Entity Self { get; }
    }

    public abstract class BehaviorBase
    {
        protected BehaviorBase()
        {
            Key = GetHashCode();
        }
        public int Key { get; set; }

        public static int MAGIC_EYE_KEY = -11;

        public static bool HasPlayerNearby(Entity entity)
        {
            foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, 16))
            {
                var d = Dist(i, entity);
                if (d < 16 * 16)
                    return true;
            }
            return false;
        }
        public static bool HasPlayerNearby(World world, double x, double y)
        {
            foreach (var i in world.PlayersCollision.HitTest(x, y, 16))
            {
                var d = Dist(i.X, i.Y, x, y);
                if (d < 16 * 16)
                    return true;
            }
            return false;
        }

        public static float DistSqr(Entity a, Entity b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }
        public static float Dist(Entity a, Entity b)
        {
            return (float)Math.Sqrt(DistSqr(a, b));
        }
        public static double Dist(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        protected Entity GetNearestEntity(ref float dist, short? objType)   //Null for player
        {
            if (Host.Self.Owner == null) return null;
            Entity ret = null;
            if (objType == null)
                foreach (var i in Host.Self.Owner.PlayersCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy() &&
                        !Host.StateStorage.ContainsKey(MAGIC_EYE_KEY)) continue;
                    var d = Dist(i, Host.Self);
                    if (d < dist)
                    {
                        dist = d;
                        ret = i;
                    }
                }
            else
                foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = Dist(i, Host.Self);
                    if (d < dist)
                    {
                        dist = d;
                        ret = i;
                    }
                }
            return ret;
        }
        protected IEnumerable<Entity> GetNearestEntities(float dist, short? objType)   //Null for player
        {
            if (Host.Self.Owner == null) yield break;
            if (objType == null)
                foreach (var i in Host.Self.Owner.PlayersCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy() &&
                        !Host.StateStorage.ContainsKey(MAGIC_EYE_KEY)) continue;
                    var d = Dist(i, Host.Self);
                    if (d < dist)
                        yield return i;
                }
            else
                foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = Dist(i, Host.Self);
                    if (d < dist)
                        yield return i;
                }
        }
        protected Entity GetNearestEntityByGroup(ref float dist, string group)
        {
            if (Host.Self.Owner == null) return null;
            Entity ret = null;
            foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
            {
                if (i.ObjectDesc == null || i.ObjectDesc.Group != group) continue;
                var d = Dist(i, Host.Self);
                if (d < dist)
                {
                    dist = d;
                    ret = i;
                }
            }
            return ret;
        }
        protected IEnumerable<Entity> GetNearestEntitiesByGroup(float dist, string group)
        {
            if (Host.Self.Owner == null)
                yield break;
            foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
            {
                if (i.ObjectDesc == null || i.ObjectDesc.Group != group) continue;
                var d = Dist(i, Host.Self);
                if (d < dist)
                    yield return i;
            }
        }
        public static Entity GetNearestEntity(Entity entity, ref float dist, bool players, Predicate<Entity> predicate = null)
        {
            if (entity.Owner == null) return null;
            Entity ret = null;
            if (players)
                foreach (var i in entity.Owner.PlayersCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy() ||
                        i == entity) continue;
                    var d = Dist(i, entity);
                    if (d < dist)
                    {
                        if (predicate != null && !predicate(i))
                            continue;
                        dist = d;
                        ret = i;
                    }
                }
            else
                foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, dist))
                {
                    if (i == entity) continue;
                    var d = Dist(i, entity);
                    if (d < dist)
                    {
                        if (predicate != null && !predicate(i))
                            continue;
                        dist = d;
                        ret = i;
                    }
                }
            return ret;
        }

        protected int CountEntity(float dist, short? objType)
        {
            if (Host.Self.Owner == null) return 0;
            int ret = 0;
            if (objType == null)
                foreach (var i in Host.Self.Owner.PlayersCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
                {
                    if (!(i as IPlayer).IsVisibleToEnemy()) continue;
                    var d = Dist(i, Host.Self);
                    if (d < dist)
                        ret++;
                }
            else
                foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = Dist(i, Host.Self);
                    if (d < dist)
                        ret++;
                }
            return ret;
        }
        protected int CountEntity(float dist, string group)
        {
            if (Host.Self.Owner == null) return 0;
            int ret = 0;
            foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, dist))
            {
                if (i.ObjectDesc == null || i.ObjectDesc.Group != group) continue;
                var d = Dist(i, Host.Self);
                if (d < dist)
                    ret++;
            }
            return ret;
        }
        protected float GetSpeedMultiplier(Entity entity)
        {
            float ret = 1;
            if (entity.HasConditionEffect(ConditionEffects.Slowed))
                ret *= 0.5f;
            if (entity.HasConditionEffect(ConditionEffects.Paralyzed) ||
                entity.HasConditionEffect(ConditionEffects.Stasis))
                ret = 0;
            return ret;
        }

        protected IEnumerable<Entity> HitTestPlayer(float x, float y)
        {
            if (Host.Self.Owner == null) yield break;
            foreach (var i in Host.Self.Owner.PlayersCollision.HitTest(x, y))
            {
                double xSide = (i.X - x);
                double ySide = (i.Y - y);
                if (xSide * xSide <= 1 && ySide * ySide <= 1)
                    yield return i;
            }
        }
        protected IEnumerable<Entity> HitTestEnemy(float x, float y)
        {
            if (Host.Self.Owner == null) yield break;
            foreach (var i in Host.Self.Owner.EnemiesCollision.HitTest(x, y))
            {
                double xSide = (i.X - x);
                double ySide = (i.Y - y);
                if (xSide * xSide <= 1 && ySide * ySide <= 1)
                    yield return i;
            }
        }
        protected bool Validate(float x, float y)
        {
            if (Host.Self.Owner == null ||
                Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return false;
            if (x < 0 || x >= Host.Self.Owner.Map.Width ||
                y < 0 || y >= Host.Self.Owner.Map.Height)
                return false;
            if (Host.Self.ObjectDesc.Flying &&
                Host.Self.Owner.Obstacles[(int)x, (int)y] != 2) return true;

            var tile = Host.Self.Owner.Map[(int)x, (int)y];
            var objId = tile.ObjType;
            if (objId != 0 &&
                XmlDatas.ObjectDescs[objId].OccupySquare)
                return false;
            //if (Host is Enemy && (Host as Enemy).Terrain != WmapTerrain.None &&
            //    (Host as Enemy).Terrain != tile.Terrain &&
            //    (Host as Enemy).Terrain == Host.Self.Owner.Map[(int)Host.Self.X, (int)Host.Self.Y].Terrain)
            //    return false;

            if (Host.Self.Owner.Obstacles[(int)x, (int)y] != 0)
                return false;
            return true;
        }
        protected bool ValidateAndMove(float x, float y)
        {
            if (Host.Self.Owner == null ||
                Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return false;
            if (Validate(x, y))
                Host.Self.Move(x, y);
            else if (Validate(Host.Self.X, y))
                Host.Self.Move(Host.Self.X, y);
            else if (Validate(x, Host.Self.Y))
                Host.Self.Move(x, Host.Self.Y);
            else
                return false;
            return true;
        }
        public static bool Validate(Entity entity, float x, float y)
        {
            if (entity.Owner == null ||
                entity.HasConditionEffect(ConditionEffects.Paralyzed)) return false;
            if (x < 0 || x >= entity.Owner.Map.Width ||
                y < 0 || y >= entity.Owner.Map.Height)
                return false;
            if (entity.ObjectDesc.Flying &&
                entity.Owner.Obstacles[(int)x, (int)y] != 2) return true;

            var objId = entity.Owner.Map[(int)x, (int)y].ObjType;
            if (objId != 0 &&
                XmlDatas.ObjectDescs[objId].OccupySquare)
                return false;

            if (entity.Owner.Obstacles[(int)x, (int)y] != 0)
                return false;
            return true;
        }
        public static bool ValidateAndMove(Entity entity, float x, float y)
        {
            if (entity.Owner == null ||
                entity.HasConditionEffect(ConditionEffects.Paralyzed)) return false;
            if (Validate(entity, x, y))
                entity.Move(x, y);
            else if (Validate(entity, entity.X, y))
                entity.Move(entity.X, y);
            else if (Validate(entity, x, entity.Y))
                entity.Move(x, entity.Y);
            else
                return false;
            return true;
        }

        protected void AOE(World world, float radius, short? objType, Action<Entity> callback)   //Null for player
        {
            if (objType == null)
                foreach (var i in world.PlayersCollision.HitTest(Host.Self.X, Host.Self.Y, radius))
                {
                    var d = Dist(i, Host.Self);
                    if (d < radius)
                        callback(i);
                }
            else
                foreach (var i in world.EnemiesCollision.HitTest(Host.Self.X, Host.Self.Y, radius))
                {
                    if (i.ObjectType != objType.Value) continue;
                    var d = Dist(i, Host.Self);
                    if (d < radius)
                        callback(i);
                }
        }
        public static void AOE(World world, Entity self, float radius, bool players, Action<Entity> callback)   //Null for player
        {
            if (players)
                foreach (var i in world.PlayersCollision.HitTest(self.X, self.Y, radius))
                {
                    var d = Dist(i, self);
                    if (d < radius)
                        callback(i);
                }
            else
                foreach (var i in world.EnemiesCollision.HitTest(self.X, self.Y, radius))
                {
                    if (!(i is Enemy)) continue;
                    var d = Dist(i, self);
                    if (d < radius)
                        callback(i);
                }
        }
        public static void AOE(World world, Position pos, float radius, bool players, Action<Entity> callback)   //Null for player
        {
            if (players)
                foreach (var i in world.PlayersCollision.HitTest(pos.X, pos.Y, radius))
                {
                    var d = Dist(i.X, i.Y, pos.X, pos.Y);
                    if (d < radius)
                        callback(i);
                }
            else
                foreach (var i in world.EnemiesCollision.HitTest(pos.X, pos.Y, radius))
                {
                    if (!(i is Enemy)) continue;
                    var d = Dist(i.X, i.Y, pos.X, pos.Y);
                    if (d < radius)
                        callback(i);
                }
        }

        protected IBehaviorHost Host { get; set; }
    }

    public abstract class Behavior : BehaviorBase
    {
        public bool Tick(IBehaviorHost host, RealmTime time)
        {
            this.Host = host;
            return TickCore(time);
        }
        protected abstract bool TickCore(RealmTime time);
    }

    [Flags]
    public enum BehaviorCondition
    {
        OnSpawn = 1,
        OnHit = 2,
        OnDeath = 4,
        Other = 8,
    }
    public abstract class ConditionalBehavior : BehaviorBase
    {
        public abstract BehaviorCondition Condition { get; }

        public bool ConditionMeet(IBehaviorHost host)
        {
            this.Host = host;
            return ConditionMeetCore();
        }
        protected virtual bool ConditionMeetCore() { return false; }

        public void Behave(BehaviorCondition cond, IBehaviorHost host, RealmTime? time, object state)
        {
            this.Host = host;
            BehaveCore(cond, time, state);
        }
        protected abstract void BehaveCore(BehaviorCondition cond, RealmTime? time, object state);

        protected void Taunt(string txt)
        {
            Host.Self.Owner.BroadcastPacket(new svrPackets.TextPacket()
            {
                Name = "#" + (Host.Self.ObjectDesc.DisplayId ?? Host.Self.ObjectDesc.ObjectId),
                ObjectId = Host.Self.Id,
                Stars = -1,
                BubbleTime = 5,
                Recipient = "",
                Text = txt,
                CleanText = ""
            }, null);
        }
    }

    class NullBehavior : Behavior
    {
        private NullBehavior() { }
        public static readonly NullBehavior Instance = new NullBehavior();

        protected override bool TickCore(RealmTime time)
        {
            return true;
        }
    }

    class RunBehaviors : Behavior
    {
        Behavior[] behavs;
        public RunBehaviors(params Behavior[] behaviors)
        {
            behavs = behaviors;
        }

        protected override bool TickCore(RealmTime time)
        {
            foreach (var i in behavs)
                i.Tick(Host, time);
            return true;
        }
    }

    class QueuedBehavior : Behavior
    {
        Behavior[] behavs;
        public QueuedBehavior(params Behavior[] behaviors)
        {
            behavs = behaviors;
        }

        protected override bool TickCore(RealmTime time)
        {
            int idx = 0;
            object obj;
            if (Host.StateStorage.TryGetValue(Key, out obj))
                idx = (int)obj;
            else
                idx = 0;

            bool repeat = false;
            int c = 0;
            bool ret = false;
            do
            {
                repeat = false;
                if (behavs[idx].Tick(Host, time))
                {
                    repeat = true;
                    idx++;
                    if (idx == behavs.Length)
                    {
                        idx = 0;
                        ret = true;
                    }
                }
                c++;
            } while (repeat && c < 2);
            Host.StateStorage[Key] = idx;

            return ret;
        }
    }

    class SetKey : Behavior
    {
        int key;
        int val;
        public SetKey(int key, int val)
        {
            this.key = key;
            this.val = val;
        }

        protected override bool TickCore(RealmTime time)
        {
            Host.StateStorage[key] = val;
            return true;
        }
    }

    class RemoveKey : Behavior
    {
        int key;
        public RemoveKey(Behavior behav)
        {
            this.key = behav.Key;
        }
        public RemoveKey(int key)
        {
            this.key = key;
        }

        protected override bool TickCore(RealmTime time)
        {
            Host.StateStorage.Remove(key);
            return true;
        }
    }

    class Cooldown : Behavior
    {
        int cooldown;
        Behavior behav;
        private Cooldown(int cooldown, Behavior behav)
        {
            this.cooldown = cooldown;
            this.behav = behav;
        }
        static readonly Dictionary<Tuple<int, Behavior>, Cooldown> instances = new Dictionary<Tuple<int, Behavior>, Cooldown>();
        public static Cooldown Instance(int cooldown, Behavior behav = null)
        {
            var key = new Tuple<int, Behavior>(cooldown, behav);
            Cooldown ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Cooldown(cooldown, behav);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = rand.Next(0, cooldown);
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret;
            if (remainingTick <= 0)
            {
                if (behav != null)
                    behav.Tick(Host, time);
                remainingTick = rand.Next((int)(cooldown * 0.95), (int)(cooldown * 1.05));
                ret = true;
            }
            else
                ret = false;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }

    class RandomDelay : Behavior
    {
        int min;
        int max;
        private RandomDelay(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        static readonly Dictionary<Tuple<int, int>, RandomDelay> instances = new Dictionary<Tuple<int, int>, RandomDelay>();
        public static RandomDelay Instance(int min, int max)
        {
            var key = new Tuple<int, int>(min, max);
            RandomDelay ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new RandomDelay(min, max);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = rand.Next(min, max);
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret;
            if (remainingTick <= 0)
            {
                remainingTick = rand.Next(min, max);
                ret = true;
            }
            else
                ret = false;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }

    class CooldownExact : Behavior
    {
        int cooldown;
        Behavior behav;
        private CooldownExact(int cooldown, Behavior behav)
        {
            this.cooldown = cooldown;
            this.behav = behav;
        }
        static readonly Dictionary<Tuple<int, Behavior>, CooldownExact> instances = new Dictionary<Tuple<int, Behavior>, CooldownExact>();
        public static CooldownExact Instance(int cooldown, Behavior behav = null)
        {
            var key = new Tuple<int, Behavior>(cooldown, behav);
            CooldownExact ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new CooldownExact(cooldown, behav);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = cooldown;
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret;
            if (remainingTick <= 0)
            {
                if (behav != null)
                    behav.Tick(Host, time);
                remainingTick = cooldown;
                ret = true;
            }
            else
                ret = false;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }

    class Rand : Behavior
    {
        Behavior[] behavs;
        private Rand(params Behavior[] behavs)
        {
            this.behavs = behavs;
        }
        static readonly Dictionary<int, Rand> instances = new Dictionary<int, Rand>();
        public static Rand Instance(params Behavior[] behavs)
        {
            int key = behavs.Length;
            foreach (var i in behavs)
                key = key * 23 + i.GetHashCode();
            Rand ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Rand(behavs);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            return behavs[rand.Next(0, behavs.Length)].Tick(Host, time);
        }
    }

    class If : Behavior
    {
        Behavior cond;
        Behavior result;
        Behavior no;
        private If(Behavior cond, Behavior result, Behavior no)
        {
            this.cond = cond;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<Behavior, Behavior, Behavior>, If> instances = new Dictionary<Tuple<Behavior, Behavior, Behavior>, If>();
        public static If Instance(Behavior cond, Behavior result, Behavior no = null)
        {
            Tuple<Behavior, Behavior, Behavior> key = new Tuple<Behavior, Behavior, Behavior>(cond, result, no);
            If ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new If(cond, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if (cond.Tick(Host, time))
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }
    class IfNot : Behavior
    {
        Behavior cond;
        Behavior result;
        private IfNot(Behavior cond, Behavior result)
        {
            this.cond = cond;
            this.result = result;
        }
        static readonly Dictionary<Tuple<Behavior, Behavior>, IfNot> instances = new Dictionary<Tuple<Behavior, Behavior>, IfNot>();
        public static IfNot Instance(Behavior cond, Behavior result)
        {
            Tuple<Behavior, Behavior> key = new Tuple<Behavior, Behavior>(cond, result);
            IfNot ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new IfNot(cond, result);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if (!cond.Tick(Host, time))
                return result.Tick(Host, time);
            return false;
        }
    }

    class IfExist : Behavior
    {
        int key;
        Behavior result;
        Behavior no;
        private IfExist(int key, Behavior result, Behavior no)
        {
            this.key = key;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, Behavior, Behavior>, IfExist> instances = new Dictionary<Tuple<int, Behavior, Behavior>, IfExist>();
        public static IfExist Instance(int key, Behavior result, Behavior no = null)
        {
            Tuple<int, Behavior, Behavior> k = new Tuple<int, Behavior, Behavior>(key, result, no);
            IfExist ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new IfExist(key, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if (Host.StateStorage.ContainsKey(key))
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }

    class IfEqual : Behavior
    {
        int key;
        int value;
        Behavior result;
        Behavior no;
        private IfEqual(int key, int value, Behavior result, Behavior no)
        {
            this.key = key;
            this.value = value;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, int, Behavior, Behavior>, IfEqual> instances = new Dictionary<Tuple<int, int, Behavior, Behavior>, IfEqual>();
        public static IfEqual Instance(int key, int value, Behavior result, Behavior no = null)
        {
            Tuple<int, int, Behavior, Behavior> k = new Tuple<int, int, Behavior, Behavior>(key, value, result, no);
            IfEqual ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new IfEqual(key, value, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            object obj;
            int val;
            if (Host.StateStorage.TryGetValue(key, out obj))
                val = (int)obj;
            else
                return false;
            if (val == value)
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }

    class IfGreater : Behavior
    {
        int key;
        int value;
        Behavior result;
        Behavior no;
        private IfGreater(int key, int value, Behavior result, Behavior no)
        {
            this.key = key;
            this.value = value;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, int, Behavior, Behavior>, IfGreater> instances = new Dictionary<Tuple<int, int, Behavior, Behavior>, IfGreater>();
        public static IfGreater Instance(int key, int value, Behavior result, Behavior no = null)
        {
            Tuple<int, int, Behavior, Behavior> k = new Tuple<int, int, Behavior, Behavior>(key, value, result, no);
            IfGreater ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new IfGreater(key, value, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            object obj;
            int val;
            if (Host.StateStorage.TryGetValue(key, out obj))
                val = (int)obj;
            else
                return false;
            if (val > value)
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }

    class HpLesser : Behavior
    {
        int threshold;
        Behavior result;
        Behavior no;
        private HpLesser(int threshold, Behavior result, Behavior no)
        {
            this.threshold = threshold;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, Behavior, Behavior>, HpLesser> instances = new Dictionary<Tuple<int, Behavior, Behavior>, HpLesser>();
        public static HpLesser Instance(int threshold, Behavior result, Behavior no = null)
        {
            Tuple<int, Behavior, Behavior> k = new Tuple<int, Behavior, Behavior>(threshold, result, no);
            HpLesser ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new HpLesser(threshold, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            var enemy = Host as Enemy;
            if (enemy.HP < threshold)
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }
    class DamageLesserEqual : Behavior
    {
        int threshold;
        Behavior result;
        Behavior no;
        private DamageLesserEqual(int threshold, Behavior result, Behavior no)
        {
            this.threshold = threshold;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, Behavior, Behavior>, DamageLesserEqual> instances = new Dictionary<Tuple<int, Behavior, Behavior>, DamageLesserEqual>();
        public static DamageLesserEqual Instance(int threshold, Behavior result, Behavior no = null)
        {
            Tuple<int, Behavior, Behavior> k = new Tuple<int, Behavior, Behavior>(threshold, result, no);
            DamageLesserEqual ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new DamageLesserEqual(threshold, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            var enemy = Host as Enemy;
            if (enemy.ObjectDesc.MaxHP - enemy.HP <= threshold)
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }

    class HpGreaterEqual : Behavior
    {
        int threshold;
        Behavior result;
        Behavior no;
        private HpGreaterEqual(int threshold, Behavior result, Behavior no)
        {
            this.threshold = threshold;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, Behavior, Behavior>, HpGreaterEqual> instances = new Dictionary<Tuple<int, Behavior, Behavior>, HpGreaterEqual>();
        public static HpGreaterEqual Instance(int threshold, Behavior result, Behavior no = null)
        {
            Tuple<int, Behavior, Behavior> k = new Tuple<int, Behavior, Behavior>(threshold, result, no);
            HpGreaterEqual ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new HpGreaterEqual(threshold, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            var enemy = Host as Enemy;
            if (enemy.HP >= threshold)
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }

    class HpLesserCond : ConditionalBehavior
    {
        int threshold;
        Behavior result;
        Behavior no;
        private HpLesserCond(int threshold, Behavior result, Behavior no)
        {
            this.threshold = threshold;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<int, Behavior, Behavior>, HpLesserCond> instances = new Dictionary<Tuple<int, Behavior, Behavior>, HpLesserCond>();
        public static HpLesserCond Instance(int threshold, Behavior result, Behavior no = null)
        {
            Tuple<int, Behavior, Behavior> k = new Tuple<int, Behavior, Behavior>(threshold, result, no);
            HpLesserCond ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new HpLesserCond(threshold, result, no);
            return ret;
        }

        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.Other | BehaviorCondition.OnHit; }
        }

        protected override bool ConditionMeetCore()
        {
            return true;
        }

        protected override void BehaveCore(BehaviorCondition cond, RealmTime? time, object state)
        {
            var enemy = Host as Enemy;
            if (enemy.HP < threshold)
                result.Tick(Host, time.Value);
            else if (no != null)
                no.Tick(Host, time.Value);
        }
    }

    class HpLesserPercent : Behavior
    {
        float threshold;
        Behavior result;
        Behavior no;
        private HpLesserPercent(float threshold, Behavior result, Behavior no)
        {
            this.threshold = threshold;
            this.result = result;
            this.no = no;
        }
        static readonly Dictionary<Tuple<float, Behavior, Behavior>, HpLesserPercent> instances = new Dictionary<Tuple<float, Behavior, Behavior>, HpLesserPercent>();
        public static HpLesserPercent Instance(float threshold, Behavior result, Behavior no = null)
        {
            Tuple<float, Behavior, Behavior> k = new Tuple<float, Behavior, Behavior>(threshold, result, no);
            HpLesserPercent ret;
            if (!instances.TryGetValue(k, out ret))
                ret = instances[k] = new HpLesserPercent(threshold, result, no);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            var enemy = Host as Enemy;
            if (enemy.HP < threshold * enemy.ObjectDesc.MaxHP)
                return result.Tick(Host, time);
            else if (no != null)
                return no.Tick(Host, time);
            return false;
        }
    }

    class Not : Behavior
    {
        Behavior x;
        private Not(Behavior x)
        {
            this.x = x;
        }
        static readonly Dictionary<Behavior, Not> instances = new Dictionary<Behavior, Not>();
        public static Not Instance(Behavior x)
        {
            Not ret;
            if (!instances.TryGetValue(x, out ret))
                ret = instances[x] = new Not(x);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            return !x.Tick(Host, time);
        }
    }

    class And : Behavior
    {
        Behavior x;
        Behavior y;
        private And(Behavior x, Behavior y)
        {
            this.x = x;
            this.y = y;
        }
        static readonly Dictionary<Tuple<Behavior, Behavior>, And> instances = new Dictionary<Tuple<Behavior, Behavior>, And>();
        public static And Instance(Behavior x, Behavior y)
        {
            var key = Tuple.Create(x, y);
            And ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new And(x, y);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            return x.Tick(Host, time) && y.Tick(Host, time);
        }
    }

    class Or : Behavior
    {
        Behavior x;
        Behavior y;
        private Or(Behavior x, Behavior y)
        {
            this.x = x;
            this.y = y;
        }
        static readonly Dictionary<Tuple<Behavior, Behavior>, Or> instances = new Dictionary<Tuple<Behavior, Behavior>, Or>();
        public static Or Instance(Behavior x, Behavior y)
        {
            var key = Tuple.Create(x, y);
            Or ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Or(x, y);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            return x.Tick(Host, time) || y.Tick(Host, time);
        }
    }

    class False : Behavior
    {
        Behavior x;
        private False(Behavior x)
        {
            this.x = x;
        }
        static readonly Dictionary<Behavior, False> instances = new Dictionary<Behavior, False>();
        public static False Instance(Behavior x)
        {
            False ret;
            if (!instances.TryGetValue(x, out ret))
                ret = instances[x] = new False(x);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            x.Tick(Host, time);
            return false;
        }
    }

    class True : Behavior
    {
        Behavior x;
        private True(Behavior x)
        {
            this.x = x;
        }
        static readonly Dictionary<Behavior, True> instances = new Dictionary<Behavior, True>();
        public static True Instance(Behavior x)
        {
            True ret;
            if (!instances.TryGetValue(x, out ret))
                ret = instances[x] = new True(x);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            x.Tick(Host, time);
            return true;
        }
    }

    class Once : Behavior
    {
        Behavior x;
        private Once(Behavior x)
        {
            this.x = x;
        }
        static readonly Dictionary<Behavior, Once> instances = new Dictionary<Behavior, Once>();
        public static Once Instance(Behavior x)
        {
            Once ret;
            if (!instances.TryGetValue(x, out ret))
                ret = instances[x] = new Once(x);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if (!Host.StateStorage.ContainsKey(Key))
            {
                x.Tick(Host, time);
                Host.StateStorage.Add(Key, true);
                return true;
            }
            else
                return false;
        }
    }

    class Timed : Behavior
    {
        int time;
        Behavior behav;
        private Timed(int time, Behavior behav)
        {
            this.time = time;
            this.behav = behav;
        }
        static readonly Dictionary<Tuple<int, Behavior>, Timed> instances = new Dictionary<Tuple<int, Behavior>, Timed>();
        public static Timed Instance(int time, Behavior behav)
        {
            var key = new Tuple<int, Behavior>(time, behav);
            Timed ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Timed(time, behav);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            int remainingTick;
            object o;
            if (!Host.StateStorage.TryGetValue(Key, out o))
                remainingTick = this.time;
            else
                remainingTick = (int)o;

            remainingTick -= time.thisTickTimes;
            bool ret = behav.Tick(Host, time);
            if (remainingTick <= 0)
            {
                remainingTick = this.time;
                ret = true;
            }
            else if (ret)
                remainingTick = this.time;
            Host.StateStorage[Key] = remainingTick;
            return ret;
        }
    }

    class Despawn : Behavior
    {
        private Despawn() { }
        public static readonly Despawn Instance = new Despawn();
        protected override bool TickCore(RealmTime time)
        {
            Host.Self.Owner.LeaveWorld(Host.Self);
            return true;
        }
    }

    class Die : Behavior
    {
        private Die() { }
        public static readonly Die Instance = new Die();
        protected override bool TickCore(RealmTime time)
        {
            var enemy = Host as Enemy;
            enemy.DamageCounter.Death();
            foreach (var i in enemy.CondBehaviors)
                if ((i.Condition & BehaviorCondition.OnDeath) != 0)
                    i.Behave(BehaviorCondition.OnDeath, Host, null, enemy.DamageCounter);
            enemy.Owner.LeaveWorld(enemy);
            return true;
        }
    }

    class IsEntityPresent : Behavior
    {
        float radius;
        short? objType;
        private IsEntityPresent(float radius, short? objType)
        {
            this.radius = radius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, short?>, IsEntityPresent> instances = new Dictionary<Tuple<float, short?>, IsEntityPresent>();
        public static IsEntityPresent Instance(float radius, short? objType)
        {
            var key = new Tuple<float, short?>(radius, objType);
            IsEntityPresent ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new IsEntityPresent(radius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            float dist = radius;
            return GetNearestEntity(ref dist, objType) != null;
        }
    }

    class IsEntityNotPresent : Behavior
    {
        float radius;
        short? objType;
        private IsEntityNotPresent(float radius, short? objType)
        {
            this.radius = radius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, short?>, IsEntityNotPresent> instances = new Dictionary<Tuple<float, short?>, IsEntityNotPresent>();
        public static IsEntityNotPresent Instance(float radius, short? objType)
        {
            var key = new Tuple<float, short?>(radius, objType);
            IsEntityNotPresent ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new IsEntityNotPresent(radius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            float dist = radius;
            return GetNearestEntity(ref dist, objType) == null;
        }
    }

    class EntityLesserThan : Behavior
    {
        float radius;
        int count;
        short? objType;
        private EntityLesserThan(float radius, int count, short? objType)
        {
            this.radius = radius;
            this.count = count;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, int, short?>, EntityLesserThan> instances = new Dictionary<Tuple<float, int, short?>, EntityLesserThan>();
        public static EntityLesserThan Instance(float radius, int count, short? objType)
        {
            var key = new Tuple<float, int, short?>(radius, count, objType);
            EntityLesserThan ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new EntityLesserThan(radius, count, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            return CountEntity(radius, objType) < count;
        }
    }

    class EntityGroupLesserThan : Behavior
    {
        float radius;
        int count;
        string group;
        private EntityGroupLesserThan(float radius, int count, string group)
        {
            this.radius = radius;
            this.count = count;
            this.group = group;
        }
        static readonly Dictionary<Tuple<float, int, string>, EntityGroupLesserThan> instances = new Dictionary<Tuple<float, int, string>, EntityGroupLesserThan>();
        public static EntityGroupLesserThan Instance(float radius, int count, string group)
        {
            var key = new Tuple<float, int, string>(radius, count, group);
            EntityGroupLesserThan ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new EntityGroupLesserThan(radius, count, group);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            return CountEntity(radius, group) < count;
        }
    }

    class WithinSpawn : Behavior
    {
        float radius;
        private WithinSpawn(float radius)
        {
            this.radius = radius;
        }
        static readonly Dictionary<float, WithinSpawn> instances = new Dictionary<float, WithinSpawn>();
        public static WithinSpawn Instance(float radius)
        {
            WithinSpawn ret;
            if (!instances.TryGetValue(radius, out ret))
                ret = instances[radius] = new WithinSpawn(radius);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            float dist = radius;
            return Dist(Host.Self.X, Host.Self.Y,
                (Host.Self as Enemy).SpawnPoint.X,
                (Host.Self as Enemy).SpawnPoint.Y) < radius;
        }
    }

    class MagicEye : Behavior
    {
        private MagicEye() { }
        public static readonly MagicEye Instance = new MagicEye();

        protected override bool TickCore(RealmTime time)
        {
            Host.StateStorage[MAGIC_EYE_KEY] = this;
            return true;
        }
    }
}
