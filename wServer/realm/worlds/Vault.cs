using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using db;
using wServer.realm.entities;
using System.Collections.Concurrent;

namespace wServer.realm.worlds
{
    public class Vault : World
    {
        public Vault(bool isLimbo, ClientProcessor psr = null)
        {
            Id = VAULT_ID;
            Name = "Vault";
            Background = 2;
            if (!(IsLimbo = isLimbo))
            {
                base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.vault.wmap"));
                Init(psr);
            }
        }

        ConcurrentDictionary<Tuple<Container, VaultChest>, int> vaultChests = new ConcurrentDictionary<Tuple<Container, VaultChest>, int>();
        ClientProcessor psr;
        void Init(ClientProcessor psr)
        {
            this.psr = psr;
            List<IntPoint> vaultChestPosition = new List<IntPoint>();
            IntPoint spawn = new IntPoint(0, 0);

            int w = Map.Width;
            int h = Map.Height;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    var tile = Map[x, y];
                    if (tile.Region == TileRegion.Spawn)
                        spawn = new IntPoint(x, y);
                    else if (tile.Region == TileRegion.Vault)
                        vaultChestPosition.Add(new IntPoint(x, y));
                }
            vaultChestPosition.Sort((x, y) => Comparer<int>.Default.Compare(
                (x.X - spawn.X) * (x.X - spawn.X) + (x.Y - spawn.Y) * (x.Y - spawn.Y),
                (y.X - spawn.X) * (y.X - spawn.X) + (y.Y - spawn.Y) * (y.Y - spawn.Y)));

            var chests = psr.Account.Vault.Chests;
            for (int i = 0; i < chests.Count; i++)
            {
                Container con = new Container(0x0504, null, false);
                var inv = chests[i].Items.Select(_ => _ == -1 ? null : XmlDatas.ItemDescs[(short)_]).ToArray();
                for (int j = 0; j < 8; j++)
                    con.Inventory[j] = inv[j];
                con.Move(vaultChestPosition[0].X + 0.5f, vaultChestPosition[0].Y + 0.5f);
                EnterWorld(con);
                vaultChestPosition.RemoveAt(0);

                vaultChests[new Tuple<Container, VaultChest>(con, chests[i])] = con.UpdateCount;
            }
            foreach (var i in vaultChestPosition)
            {
                SellableObject x = new SellableObject(0x0505);
                x.Move(i.X + 0.5f, i.Y + 0.5f);
                EnterWorld(x);
            }
        }

        public void AddChest(VaultChest chest, Entity original)
        {
            Container con = new Container(0x0504, null, false);
            var inv = chest.Items.Select(_ => _ == -1 ? null : XmlDatas.ItemDescs[(short)_]).ToArray();
            for (int j = 0; j < 8; j++)
                con.Inventory[j] = inv[j];
            con.Move(original.X, original.Y);
            LeaveWorld(original);
            EnterWorld(con);

            vaultChests[new Tuple<Container, VaultChest>(con, chest)] = con.UpdateCount;
        }

        public override World GetInstance(ClientProcessor psr)
        {
            return RealmManager.AddWorld(new Vault(false, psr));
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            foreach (var i in vaultChests)
            {
                if (i.Key.Item1.UpdateCount > i.Value)
                {
                    i.Key.Item2._Items = Utils.GetCommaSepString(i.Key.Item1.Inventory.Take(8).Select(_ => _ == null ? -1 : _.ObjectType).ToArray());
                    psr.Database.SaveChest(psr.Account, i.Key.Item2);
                    vaultChests[i.Key] = i.Key.Item1.UpdateCount;
                }
            }

        }
    }
}
