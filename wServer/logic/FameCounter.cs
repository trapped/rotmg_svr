using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.realm;

namespace wServer.logic
{
    public class FameCounter
    {
        Player player;
        public Player Host { get { return player; } }

        FameStats stats;
        public FameCounter(Player player)
        {
            this.player = player;
            this.stats = player.Client.Character.FameStats;
        }

        HashSet<Projectile> projs = new HashSet<Projectile>();
        public void Shoot(Projectile proj)
        {
            stats.Shots++;
            projs.Add(proj);
        }

        public void Hit(Projectile proj, Enemy enemy)
        {
            if (projs.Contains(proj))
            {
                projs.Remove(proj);
                stats.ShotsThatDamage++;
            }
        }

        public void Killed(Enemy enemy, bool killer)
        {
            if (enemy.ObjectDesc.God)
                stats.GodAssists++;
            else
                stats.MonsterAssists++;
            if (player.Quest == enemy)
                stats.QuestsCompleted++;
            if (killer)
            {
                if (enemy.ObjectDesc.God)
                    stats.GodKills++;
                else
                    stats.MonsterKills++;

                if (enemy.ObjectDesc.Cube)
                    stats.CubeKills++;
                if (enemy.ObjectDesc.Oryx)
                    stats.OryxKills++;
            }
        }
        public void LevelUpAssist(int count)
        {
            stats.LevelUpAssists += count;
        }

        public void TileSent(int num)
        {
            stats.TilesUncovered += num;
        }

        public void Teleport()
        {
            stats.Teleports++;
        }

        public void UseAbility()
        {
            stats.SpecialAbilityUses++;
        }

        public void DrinkPot()
        {
            stats.PotionsDrunk++;
        }

        int elapsed = 0;
        public void Tick(RealmTime time)
        {
            elapsed += time.thisTickTimes;
            if (elapsed > 1000 * 60)
            {
                elapsed -= 1000 * 60;
                stats.MinutesActive++;
            }
        }
    }
}
