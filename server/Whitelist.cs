using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server
{
    public static class Whitelist
    {
        public static Chars whitelisted = new Chars()
        {
            Characters = new List<Char>()
            {
               
            },
            News = new List<NewsItem>()
            {
                new NewsItem()
                {
                    Date = db.Database.DateTimeToUnixTimestamp(DateTime.Now),
                    Icon = "info",
                    Link = "http://forums.wildshadow.com/",
                    Title = "Not whitelisted",
                    TagLine = "You are not whitelisted."
                }
            },
            NextCharId = 2,
            MaxNumChars = 1,
            Account = new Account()
            {
                Name = "Not whitelisted",
                AccountId = -1,
                Admin = false,
                BeginnerPackageTimeLeft = 0,
                Converted = false,
                Guild = null,
                NameChosen = true,
                NextCharSlotPrice = 100,
                VerifiedEmail = false,
                Credits = 0,
                Stats = new Stats()
                {
                    Fame = 0,
                    TotalFame = 0,
                    BestCharFame = 0,
                    ClassStates = new List<ClassStats>()
                    {
                        new ClassStats()
                        {
                            ObjectType = 782,
                            BestFame = 0,
                            BestLevel = 0
                        }
                    }
                },
                Vault = new VaultData()
                {
                    Chests = new List<VaultChest>()
                    {
                        new VaultChest()
                        {
                            ChestId = 0,
                            _Items = "-1, -1, -1, -1, -1, -1, -1, -1"
                        }
                    }
                }
            },
            Servers = new List<ServerItem>()
            {
                new ServerItem()
                {
                    Name = "Not whitelisted",
                    Lat = 22.28,
                    Long = 114.16,
                    DNS = "000.000.000.000",
                    Usage = 0.2,
                    AdminOnly = false
                }
            }
        };        
    }
}
