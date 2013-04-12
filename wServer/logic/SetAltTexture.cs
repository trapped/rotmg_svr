using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;
using Mono.Game;

namespace wServer.logic
{
    class SetAltTexture : Behavior
    {
        int index;
        private SetAltTexture(int index)
        {
            this.index = index;
        }
        static readonly Dictionary<int, SetAltTexture> instances = new Dictionary<int, SetAltTexture>();
        public static SetAltTexture Instance(int index)
        {
            SetAltTexture ret;
            if (!instances.TryGetValue(index, out ret))
                ret = instances[index] = new SetAltTexture(index);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if ((Host.Self as Enemy).AltTextureIndex != index)
            {
                (Host.Self as Enemy).AltTextureIndex = index;
                Host.Self.UpdateCount++;
            }
            return true;
        }
    }
}
