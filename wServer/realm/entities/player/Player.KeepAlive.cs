using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.svrPackets;
using wServer.cliPackets;

namespace wServer.realm.entities
{
    public partial class Player
    {
        long lastPong = -1;
        int? lastTime = null;
        long tickMapping = 0;
        Queue<long> ts = new Queue<long>();
        bool KeepAlive(RealmTime time)
        {
            if (lastPong == -1) lastPong = time.tickTimes - 1500;
            if (time.tickTimes - lastPong > 1500)
            {
                ts.Enqueue(time.tickTimes);
                psr.SendPacket(new PingPacket());
            }
            else if (time.tickTimes - lastPong > 3000)
            {
                //psr.Disconnect();
                return false;
            }
            return true;
        }
        public void Pong(PongPacket pkt)
        {
            if (lastTime != null && (pkt.Time - lastTime.Value > 3000 || pkt.Time - lastTime.Value < 0))
                ;//psr.Disconnect();
            else
                lastTime = pkt.Time;
            tickMapping = ts.Dequeue() - pkt.Time;
            lastPong = pkt.Time + tickMapping;
        }
    }
}
