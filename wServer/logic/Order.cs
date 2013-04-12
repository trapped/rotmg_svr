using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic
{
    class OrderGroup : Behavior
    {
        float radius;
        string group;
        Behavior behav;
        private OrderGroup(float radius, string group, Behavior behav)
        {
            this.radius = radius;
            this.group = group;
            this.behav = behav;
        }
        static readonly Dictionary<Tuple<float, string, Behavior>, OrderGroup> instances = new Dictionary<Tuple<float, string, Behavior>, OrderGroup>();
        public static OrderGroup Instance(float radius, string group, Behavior behav)
        {
            var key = new Tuple<float, string, Behavior>(radius, group, behav);
            OrderGroup ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new OrderGroup(radius, group, behav);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            bool ret = false;
            foreach (var i in GetNearestEntitiesByGroup(radius, group))
                ret |= behav.Tick(i, time);
            return ret;
        }
    }

    class OrderEntity : Behavior
    {
        float radius;
        short objType;
        Behavior behav;
        private OrderEntity(float radius, short objType, Behavior behav)
        {
            this.radius = radius;
            this.objType = objType;
            this.behav = behav;
        }
        static readonly Dictionary<Tuple<float, short, Behavior>, OrderEntity> instances = new Dictionary<Tuple<float, short, Behavior>, OrderEntity>();
        public static OrderEntity Instance(float radius, short objType, Behavior behav)
        {
            var key = new Tuple<float, short, Behavior>(radius, objType, behav);
            OrderEntity ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new OrderEntity(radius, objType, behav);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            var d = radius;
            var x = GetNearestEntity(ref d, objType);
            if (x != null)
                return behav.Tick(x, time);
            return false;
        }
    }

    class OrderAllEntity : Behavior
    {
        float radius;
        short objType;
        Behavior behav;
        private OrderAllEntity(float radius, short objType, Behavior behav)
        {
            this.radius = radius;
            this.objType = objType;
            this.behav = behav;
        }
        static readonly Dictionary<Tuple<float, short, Behavior>, OrderAllEntity> instances = new Dictionary<Tuple<float, short, Behavior>, OrderAllEntity>();
        public static OrderAllEntity Instance(float radius, short objType, Behavior behav)
        {
            var key = new Tuple<float, short, Behavior>(radius, objType, behav);
            OrderAllEntity ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new OrderAllEntity(radius, objType, behav);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            var x = GetNearestEntities(radius, objType);
            bool ret = false;
            foreach (var i in x)
            {
                ret = true;
                behav.Tick(i, time);
            }
            return ret;
        }
    }
}
