using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.svrPackets;
using wServer.realm.entities;

namespace wServer.logic.taunt
{
    class RandomTaunt : TauntBase
    {
        public RandomTaunt(double prob, string taunt) { this.prob = prob; this.taunt = taunt; }
        double prob;
        string taunt;

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (rand.NextDouble() > prob) return false;
            Taunt(taunt, false);
            return true;
        }
    }
}
