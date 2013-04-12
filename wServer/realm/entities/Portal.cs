using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace wServer.realm.entities
{
    public class Portal : StaticObject
    {
        public Portal(short objType, int? life)
            : base(objType, life, false, true, false)
        {
            Usable = true;
        }

        public bool Usable { get; set; }
        public World WorldInstance { get; set; }

        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.PortalUsable)
                val = (int)val != 0;
            base.ImportStats(stats, val);
        }
        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.PortalUsable] = Usable ? 1 : 0;
            base.ExportStats(stats);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            return false;
        }
    }
}
