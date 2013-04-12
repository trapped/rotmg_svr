using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace wServer.realm.entities
{
    public interface IProjectileOwner
    {
        Projectile[] Projectiles { get; }
        Entity Self { get; }
    }

    public class Projectile : Entity
    {
        public IProjectileOwner ProjectileOwner { get; set; }
        public byte ProjectileId { get; set; }
        public short Container { get; set; }
        public int Damage { get; set; }

        public long BeginTime { get; set; }
        public Position BeginPos { get; set; }
        public float Angle { get; set; }

        public ProjectileDesc Descriptor { get; set; }

        public Projectile(ProjectileDesc desc)
            : base(XmlDatas.IdToType[desc.ObjectId])
        {
            this.Descriptor = desc;
        }

        public void Destroy(bool immediate)
        {
            if (!immediate)
                RealmManager.AddPendingAction(_ => Destroy(true), PendingPriority.Destruction);
            if (Owner != null)
            {
                //ProjectileOwner.Projectiles[ProjectileId] = null;
                Owner.LeaveWorld(this);
            }
        }

        public Position GetPosition(long elapsedTicks)
        {
            double x = BeginPos.X;
            double y = BeginPos.Y;

            double dist = (elapsedTicks / 1000.0) * (Descriptor.Speed / 10.0);
            double period = ProjectileId % 2 == 0 ? 0 : Math.PI;
            if (Descriptor.Wavy)
            {
                var theta = Angle + (Math.PI * 64) * Math.Sin(period + 6 * Math.PI * (elapsedTicks / 1000));
                x += dist * Math.Cos(theta);
                y += dist * Math.Sin(theta);
            }
            else if (Descriptor.Parametric)
            {
                var theta = (double)elapsedTicks / Descriptor.LifetimeMS * 2 * Math.PI;
                var a = Math.Sin(theta) * (ProjectileId % 2 != 0 ? 1 : -1);
                var b = Math.Sin(theta * 2) * (ProjectileId % 4 < 2 ? 1 : -1);
                var c = Math.Sin(Angle);
                var d = Math.Cos(Angle);
                x += (a * d - b * c) * Descriptor.Magnitude;
                y += (a * c + b * d) * Descriptor.Magnitude;
            }
            else
            {
                if (Descriptor.Boomerang)
                {
                    var d = (Descriptor.LifetimeMS / 1000.0) * (Descriptor.Speed / 10.0) / 2;
                    if (dist > d)
                        dist = d - (dist - d);
                }
                x += dist * Math.Cos(Angle);
                y += dist * Math.Sin(Angle);
                if (Descriptor.Amplitude != 0)
                {
                    var d = Descriptor.Amplitude * Math.Sin(period + (double)elapsedTicks / Descriptor.LifetimeMS * Descriptor.Frequency * 2 * Math.PI);
                    x += d * Math.Cos(Angle + Math.PI / 2);
                    y += d * Math.Sin(Angle + Math.PI / 2);
                }
            }
            return new Position() { X = (float)x, Y = (float)y };
        }

        HashSet<Entity> hitted = new HashSet<Entity>();
        SpatialStorage collisionMap;
        public override void Tick(RealmTime time)
        {
            if (collisionMap == null)
                collisionMap = ProjectileOwner is Player ?
                    Owner.EnemiesCollision :
                    Owner.PlayersCollision;

            long elapsedTicks = time.tickTimes - BeginTime;
            if (elapsedTicks > Descriptor.LifetimeMS)
            {
                Destroy(true);
                return;
            }
            long counter = time.thisTickTimes;
            while (counter > RealmManager.MsPT && TickCore(elapsedTicks - counter, time))
                counter -= RealmManager.MsPT;
            if (Owner != null)
                TickCore(elapsedTicks, time);

            base.Tick(time);
        }
        bool TickCore(long elapsedTicks, RealmTime time)
        {
            Position pos = GetPosition(elapsedTicks);
            Move(pos.X, pos.Y);

            if (pos.X < 0 || pos.X > Owner.Map.Width)
            {
                Destroy(true);
                return false;
            }
            else if (pos.Y < 0 || pos.Y > Owner.Map.Height)
            {
                Destroy(true);
                return false;
            }
            else if (Owner.Map[(int)pos.X, (int)pos.Y].TileId == 0xff)
            {
                Destroy(true);
                return false;
            }
            else
            {
                bool penetrateObsta = Descriptor.PassesCover;
                bool penetrateEnemy = Descriptor.MultiHit;

                var objId = Owner.Map[(int)pos.X, (int)pos.Y].ObjType;
                if (objId != 0 &&
                    XmlDatas.ObjectDescs[objId].OccupySquare &&
                    !penetrateObsta)
                {
                    Destroy(true);
                    return false;
                }

                double nearestRadius = double.MaxValue;
                Entity entity = null;
                foreach (var i in collisionMap.HitTest(pos.X, pos.Y, 2))
                {
                    if (i == ProjectileOwner.Self) continue;
                    if (i is Container) continue;
                    if (hitted.Contains(i)) continue;
                    double xSide = (i.X - pos.X) * (i.X - pos.X);
                    double ySide = (i.Y - pos.Y) * (i.Y - pos.Y);
                    if (xSide <= 0.5 * 0.5 && ySide <= 0.5 * 0.5 && xSide + ySide <= nearestRadius)
                    {
                        nearestRadius = xSide + ySide;
                        entity = i;
                    }
                }
                if (entity != null && entity.HitByProjectile(this, time))
                {
                    if ((entity is Enemy && penetrateEnemy) ||
                        (entity is StaticObject && (entity as StaticObject).Static && !(entity is Wall) && penetrateObsta))
                        hitted.Add(entity);
                    else
                    {
                        Destroy(true);
                        return false;
                    }
                    ProjectileOwner.Self.ProjectileHit(this, entity);
                }
            }
            return true;
        }

        public void ForceHit(Entity entity, RealmTime time)
        {
            bool penetrateObsta = Descriptor.PassesCover;
            bool penetrateEnemy = Descriptor.MultiHit;
            Move(entity.X, entity.Y);
            if (entity.HitByProjectile(this, time))
            {
                if ((entity is Enemy && penetrateEnemy) ||
                    (entity is StaticObject && (entity as StaticObject).Static && !(entity is Wall) && penetrateObsta))
                    hitted.Add(entity);
                else
                    Destroy(true);
                ProjectileOwner.Self.ProjectileHit(this, entity);
            }
        }
    }
}
