using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace db
{
    public static class confreader
    {
        public static string getservers(bool db)
        {
            String line;
            String serveraddr;
            String dbaddr;
            try
            {
                StreamReader readergg = new StreamReader("config.cfg");
                while ((line = readergg.ReadLine()) != null)
                {
                    if (db == true)
                    {
                        if (line.StartsWith("db"))
                        {
                            return dbaddr = line.Substring(3);
                        }
                    }
                    else
                    {
                        if (line.StartsWith("srv"))
                        {
                            return serveraddr = line.Substring(4);
                        }
                    }
                    //string[] linearray = line.Split(';');
                    //chrs.Servers.Add(new ServerItem()
                    //{
                    //    Name = linearray[0],
                    //    Lat = 22.28,
                    //    Long = 114.16,
                    //    DNS = linearray[1],
                    //    Usage = 0.2,
                    //    AdminOnly = bool.Parse(linearray[2])
                    // });
                }
                if (db == true)
                    return dbaddr = "Server=localhost;Database=rotmg;uid=root;password=botmaker";
                else
                    return serveraddr = "localhost";
            }
            catch
            {
                Console.WriteLine("Problems loading config.cfg. The default data has been used instead");
                if (db == true)
                    return dbaddr = "Server=localhost;Database=rotmg;uid=root;password=botmaker";
                else
                    return serveraddr = "localhost";
            }
        }
    }
}
