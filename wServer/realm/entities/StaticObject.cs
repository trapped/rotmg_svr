using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace wServer.realm.entities
{
    public class StaticObject : Entity
    {
        //Stats
        public bool Vulnerable { get; private set; }
        public bool Static { get; private set; }
        public bool Hittestable { get; private set; }
        public int HP { get; set; }
        public bool Dying { get; private set; }

        public static int? GetHP(XElement elem)
        {
            var n = elem.Element("MaxHitPoints");
            if (n != null)
                return Utils.FromString(n.Value);
            else
                return null;
        }

        static bool IsInteractive(short objType)
        {
            ObjectDesc desc;
            if (XmlDatas.ObjectDescs.TryGetValue(objType, out desc))
                return !(desc.Static && !desc.Enemy && !desc.EnemyOccupySquare);
            else
                return false;
        }
        public StaticObject(short objType, int? life, bool stat, bool dying, bool hittestable)
            : base(objType, IsInteractive(objType))
        {
            if (Vulnerable = life.HasValue)
                HP = life.Value;
            Dying = dying;
            Static = stat;
            Hittestable = hittestable;
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            if (!Vulnerable)
                stats[StatsType.HP] = int.MaxValue;
            else
                stats[StatsType.HP] = HP;
            base.ExportStats(stats);
        }
        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.HP) HP = (int)val;
            base.ImportStats(stats, val);
        }

        protected bool CheckHP()
        {
            if (HP <= 0)
            {
                if (ObjectDesc != null &&
                    (ObjectDesc.EnemyOccupySquare || ObjectDesc.OccupySquare))
                    Owner.Obstacles[(int)(X - 0.5), (int)(Y - 0.5)] = 0;
                Owner.LeaveWorld(this);
                return false;
            }
            return true;
        }

        public override void Tick(RealmTime time)
        {
            if (Vulnerable)
            {
                if (Dying)
                {
                    HP -= time.thisTickTimes;
                    UpdateCount++;
                }
                CheckHP();
            }

            base.Tick(time);
        }
    }
}
