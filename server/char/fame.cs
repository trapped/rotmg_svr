using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using db;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;

namespace server.@char
{
    class fame : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
            using (var db = new Database())
            {
                var acc = db.GetAccount(int.Parse(query["accountId"]));
                var chr = db.LoadCharacter(acc, int.Parse(query["charId"]));

                var cmd = db.CreateQuery();
                cmd.CommandText = @"SELECT time, killer, firstBorn FROM death WHERE accId=@accId AND chrId=@charId;";
                cmd.Parameters.AddWithValue("@accId", query["accountId"]);
                cmd.Parameters.AddWithValue("@charId", query["charId"]);
                int time;
                string killer;
                bool firstBorn;
                using (var rdr = cmd.ExecuteReader())
                {
                    rdr.Read();
                    time = Database.DateTimeToUnixTimestamp(rdr.GetDateTime("time"));
                    killer = rdr.GetString("killer");
                    firstBorn = rdr.GetBoolean("firstBorn");
                }

                using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                    wtr.Write(chr.FameStats.Serialize(acc, chr, time, killer, firstBorn));
            }
        }
    }
}
