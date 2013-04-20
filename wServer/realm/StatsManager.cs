using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.logic;

namespace wServer.realm
{
    class StatsManager
    {
        Player player;
        float damagereceived;
        public StatsManager(Player player)
        {
            this.player = player;
        }
        //from wiki

        int GetStats(int id)
        {
            return player.Stats[id] + player.Boost[id];
        }

        public float GetAttackDamage(int min, int max)
        {
            var att = GetStats(2);
            if (player.HasConditionEffect(ConditionEffects.Paralyzed))
                att = 0;

            var ret = player.Random.Next(min, max) * (0.5f + att / 50f);

            if (player.HasConditionEffect(ConditionEffects.Damaging))
                ret *= 1.5f;

            return ret;
        }
        public static float GetDefenseDamage(Entity host, int dmg, int def)
        {
            if (host.HasConditionEffect(ConditionEffects.Armored))
                def *= 2;
            if (host.HasConditionEffect(ConditionEffects.ArmorBroken))
                def = 0;

            float limit = dmg * 0.15f;

            float ret;
            if (dmg - def < limit) ret = limit;
            else ret = dmg - def;

            if (host.HasConditionEffect(ConditionEffects.Invulnerable) ||
                host.HasConditionEffect(ConditionEffects.Invincible))
                ret = 0;
            return ret;
        }
        public float GetDefenseDamage(int dmg, bool noDef)
        {
            var def = GetStats(3);
            if (player.HasConditionEffect(ConditionEffects.Armored))
                def *= 2;
            if (player.HasConditionEffect(ConditionEffects.ArmorBroken) ||
                noDef)
                def = 0;

            float limit = dmg * 0.15f;

            float ret;
            if (dmg - def < limit) ret = limit;
            else ret = dmg - def;

            if (player.HasConditionEffect(ConditionEffects.Invulnerable) ||
                player.HasConditionEffect(ConditionEffects.Invincible))
                ret = 0;
            AddRage(dmg);
            return ret;
        }
        public float GetSpeed()
        {
            var ret = 4 + 5.6f * (GetStats(4) / 75f);
            if (player.HasConditionEffect(ConditionEffects.Speedy))
                ret *= 1.5f;
            if (player.HasConditionEffect(ConditionEffects.Slowed))
                ret = 4;
            if (player.HasConditionEffect(ConditionEffects.Paralyzed))
                ret = 0;
            return ret;
        }
        public float GetHPRegen()
        {
            float dis;
            var vit = GetStats(5);
            if (player.HasConditionEffect(ConditionEffects.Sick))
                vit = 0;
            if (player.ObjectType == 797)
            {
                dis = 2.5f;
                return 1 + vit / dis;
            }
            else if (player.ObjectType == 798)
            {
                dis = 2.5f;
                return 1 + vit / dis;
            }
            else if (player.ObjectType == 805)
            {
                dis = 3f;
                return 1 + vit / dis;
            }
            else
            {
                dis = 8f;
                return 1 + vit / dis;
            }
        }
        public float GetMPRegen()
        {
            var wis = GetStats(6);
            float dis;
            if (player.HasConditionEffect(ConditionEffects.Quiet))
                return 0;
            if (player.ObjectType == 784)
            {
                dis = 8f;
                return 0.6f + wis / dis;
            }
            else if (player.ObjectType == 799)
            {
                dis = 8f;
                return 0.6f + wis / dis;
            }
            else if (player.ObjectType == 798)
            {
                dis = 16.7f;
                return 0.6f + wis / dis;
            }
            else
            {
                dis = 16.7f;
                return 0.6f + wis / dis;
            }
        }
        public float GetDex()
        {
            var dex = GetStats(7);
            if (player.HasConditionEffect(ConditionEffects.Dazed))
                dex = 0;

            var ret = 1.5f + 6.5f * (dex / 75f);
            if (player.HasConditionEffect(ConditionEffects.Berserk))
                ret *= 1.5f;
            if (player.HasConditionEffect(ConditionEffects.Stunned))
                ret = 0;
            return ret;
        }

        public static int StatsNameToIndex(string name)
        {
            switch (name)
            {
                case "MaxHitPoints": return 0;
                case "MaxMagicPoints": return 1;
                case "Attack": return 2;
                case "Defense": return 3;
                case "Speed": return 4;
                case "HpRegen": return 5;
                case "MpRegen": return 6;
                case "Dexterity": return 7;
            } return -1;
        }
        public static string StatsIndexToName(int index)
        {
            switch (index)
            {
                case 0: return "MaxHitPoints";
                case 1: return "MaxMagicPoints";
                case 2: return "Attack";
                case 3: return "Defense";
                case 4: return "Speed";
                case 5: return "HpRegen";
                case 6: return "MpRegen";
                case 7: return "Dexterity";
            } return null;
        }
        public float GetRageRegen()
        {
            var wis = GetStats(6);
            int degenvalue = -5;
            float regen;
            if (HasEnemyNearby(player) == true) //seems bugged
            {
                regen = (wis / 10);
            }
            else
            {
                regen = degenvalue;
            }
            Console.WriteLine("int regen = " + regen.ToString());
            return regen;
        }
        public static bool HasEnemyNearby(Entity entity)
        {
            foreach (var i in entity.Owner.EnemiesCollision.HitTest(entity.X, entity.Y, 16))
            {
                var d = logic.Behavior.Dist(i, entity);
                if (d < 16 * 16)
                return true;
            }
            return false;
        }
        public int getgainmultiplier(int multiplier, int value)
        {
            return (value / 100) * multiplier;
        }
        public void AddRage(float dmg)
        {
            int gainmultiplied = CalculateRage(dmg);
            player.MP = player.MP + gainmultiplied;
            player.UpdateCount++;
        }
        public int CalculateRage(float dmg)
        {
            int gainmultiplied = 0;
            if (HasEnemyNearby(player) == true)
            {
                int gainmultiplier = 10;
                return gainmultiplied = getgainmultiplier(gainmultiplier, (int)dmg);
            }
            else
            {
                int gainmultiplier = 0;
                return gainmultiplied = getgainmultiplier(gainmultiplier, (int)dmg);
            }
        }
        public void DegenMana(int degenvalue, RealmTime time)
        {
            player.MP = player.MP - degenvalue;
            if (player.MP < 0)
                player.MP = 0;
            player.UpdateCount++;
        }
    }
}
