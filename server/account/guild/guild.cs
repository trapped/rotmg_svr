using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;

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
                cmd.CommandText = "SELECT guild FROM accounts WHERE guid=@guid";
                cmd.Parameters.AddWithValue("@guid", guid);
                guildid = int.Parse(cmd.ExecuteScalar().ToString());
            }
            //down here goes the code to retrieve the board text by guild
            string[] lines;
            try
            {
                lines = File.ReadAllLines("/guild/board/boardid" + guildid.ToString() + ".txt");
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
    class listMembers : IRequestHandler //struct: num       offset  ignore              guid    password
    {                                   //values: int       int     int                 string  string
                                        //        members   0       prollycurrentacc    email   passwordnocrypt
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
        }
    }
}
