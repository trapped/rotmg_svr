using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace server.account.guild
{
    class getBoard : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context) //struct: ignore             guid    password
                                                               //values: int                string  string
                                                               //        prollycurrentacc   email   passwordnocrypt
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
            //down here goes the code to identify the guild by the account
            string guid = query["guid"];
            int guildid;
            using (db.Database dbx = new db.Database())
            {
                var cmd = dbx.CreateQuery();
                cmd.CommandText = "SELECT guild FROM accounts WHERE uuid=@guid";
                cmd.Parameters.AddWithValue("@guid", guid);
                guildid = int.Parse(cmd.ExecuteScalar().ToString());
            }
            //down here goes the code to retrieve the board text by guild
            string[] lines;
            try
            {
                using (db.Database dbz = new db.Database())
                {
                    var cmd = dbz.CreateQuery();
                    cmd.CommandText = "SELECT board FROM guilds WHERE id=@id";
                    cmd.Parameters.AddWithValue("@id", guildid);
                    object scalar = cmd.ExecuteScalar();
                    lines = scalar.ToString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                }
            }
            catch
            {
                lines = new string[]
                {
                  "Couldn't retrieve guild board"  
                };
            }
            string boardtext = null;
            foreach (var i in lines)
            {
                if (boardtext != null)
                {
                    boardtext = boardtext + i.ToString() + "\n\r";
                }
                else
                {
                    boardtext = i.ToString() + "\n\r";
                }
            }
            //down here goes the code to eventually check the formatting
            byte[] guildboardtext = Encoding.ASCII.GetBytes(boardtext);
            context.Response.OutputStream.Write(guildboardtext, 0, guildboardtext.Length);
        }
    }
    class setBoard : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            string board;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
            using (db.Database dbx = new db.Database())
            {
                var cmd = dbx.CreateQuery();
                cmd.CommandText = "UPDATE guilds SET board=@board WHERE id=@id";
                var acc = dbx.Verify(query["guid"], query["password"]);
                cmd.Parameters.AddWithValue("@id", acc.Guild.Id);
                board = HttpUtility.HtmlDecode(query["board"]);
                cmd.Parameters.AddWithValue("@board", board);
                cmd.ExecuteNonQuery();
            }
            board = null;
            using (db.Database dbz = new db.Database())
            {
                var cmd = dbz.CreateQuery();
                cmd.CommandText = "SELECT board FROM guilds WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", dbz.Verify(query["guid"], query["password"]).Guild.Id);
                object scalar = cmd.ExecuteScalar();
                string[] lines = scalar.ToString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                foreach (var i in lines)
                {
                    if (board != null)
                    {
                        board = board + i.ToString() + "\n\r";
                    }
                    else
                    {
                        board = i.ToString() + "\n\r";
                    }
                }
            }
            //down here goes the code to eventually check the formatting
            byte[] guildboardtext = Encoding.ASCII.GetBytes(board);
            context.Response.OutputStream.Write(guildboardtext, 0, guildboardtext.Length);
        }
    }
    class listMembers : IRequestHandler //struct: num       offset  ignore              guid    password
    {                                   //values: int       int     int                 string  string
                                        //        members   0       prollycurrentacc    email   passwordnocrypt
                                        //response:
                                        //<Guild name="STRING" id="INT">
                                        //<TotalFame>INT</TotalFame>
                                        //<CurrentFame>INT</Currentfame>
                                        //<HallType>Guild Hall INT</HallType>
                                        //<Member>
                                        //<Name>STRING</Name>
                                        //<Rank>INT</Rank>
                                        //<Fame>INT</Fame>
                                        //</Member>
                                        //</Guild>
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
            XDocument doc;
            XElement guildd;
            using (db.Database dbx = new db.Database())
            {
                Account acc = dbx.Verify(query["guid"], query["password"]);
                guildd = new XElement("Guild", new XAttribute("name", acc.Guild.Name), new XAttribute("id", acc.Guild.Id));
                //get the hall level
                var cmd = dbx.CreateQuery();
                cmd.CommandText = "SELECT level FROM guilds WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", acc.Guild.Id);
                guildd.Add(new XElement("HallType", "Guild Hall "+cmd.ExecuteScalar().ToString()));
                //get the current fame
                cmd = dbx.CreateQuery();
                cmd.CommandText = "SELECT fame FROM guilds WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", acc.Guild.Id);
                guildd.Add(new XElement("CurrentFame", cmd.ExecuteScalar().ToString()));
                //get the total fame
                cmd = dbx.CreateQuery();
                cmd.CommandText = "SELECT totalFame FROM guilds WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", acc.Guild.Id);
                guildd.Add(new XElement("TotalFame", cmd.ExecuteScalar().ToString()));
                //get the members list and data
                cmd = dbx.CreateQuery();
                cmd.CommandText = "SELECT members FROM guilds WHERE id=@id";
                cmd.Parameters.AddWithValue("@id", acc.Guild.Id);
                string[] membersids = cmd.ExecuteScalar().ToString().Split(',');
                foreach (var memberid in membersids)
                {
                    var accc = dbx.GetAccount(int.Parse(memberid));
                    guildd.Add(new XElement("Member",
                        new XElement("Name", accc.Name),
                        new XElement("Rank", accc.Guild.Rank),
                        new XElement("Fame", 0)));
                }
                doc = new XDocument(guildd);
            }
            byte[] bytes = Encoding.ASCII.GetBytes(doc.ToString());
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            context.Response.Close();
        }
    }
}
