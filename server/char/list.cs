using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using db;
using System.Xml.Serialization;
using System.IO;
using MySql.Data.MySqlClient;
using System.Web;
using System.Collections.Specialized;

namespace server.@char
{
    class list : IRequestHandler
    {
        Chars chrs;
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db1 = new Database())
            {

                chrs = new Chars()
                {
                    Characters = new List<Char>() { },
                    NextCharId = 2,
                    MaxNumChars = 1,
                    Account = db1.Verify(query["guid"], query["password"]),
                    Servers = new List<ServerItem>() //leave this list empty for the Oryx Sleeping message
                    {
                        new ServerItem()
                        {
                            Name = "Local",
                            Lat = 22.28,
                            Long = 114.16,
                            DNS = "127.0.0.1",//db.confreader.getservers(false).ToString(), //127.0.0.1, CHANGE THIS TO LET YOUR FRIENDS CONNECT
                            Usage = 0.2,
                            AdminOnly = false
                        }
                        //new ServerItem()
                        //{
                        //    Name = "Admin Realm",
                        //    Lat = 22.28,
                        //    Long = 114.16,
                        //    DNS = "127.0.0.1",
                        //    Usage = 0.2,
                        //    AdminOnly = true
                        //}
                    }
                };
                Account dvh = null;
                if (chrs.Account != null)
                {
                    db1.GetCharData(chrs.Account, chrs);
                    db1.LoadCharacters(chrs.Account, chrs);
                    chrs.News = db1.GetNews(chrs.Account);
                    dvh = chrs.Account;
                    if (db1.getRole(chrs.Account) >= (int)Database.Roles.Moderator)
                    {
                        chrs.Account.Admin = true;
                    }
                }
                else
                {
                    chrs.Account = Database.CreateGuestAccount(query["guid"]);
                    chrs.News = db1.GetNews(null);
                }
                if (dvh != null && db1.isWhitelisted(dvh) == false)
                {
                    chrs = Whitelist.whitelisted;
                }
                if (dvh != null && db1.isBanned(dvh) == true)
                {
                    chrs = new Chars();
                }
                MemoryStream ms = new MemoryStream();
                XmlSerializer serializer = new XmlSerializer(chrs.GetType(), new XmlRootAttribute(chrs.GetType().Name) { Namespace = "" });

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;
                XmlWriter xtw = XmlWriter.Create(context.Response.OutputStream, xws);
                serializer.Serialize(xtw, chrs, chrs.Namespaces);
            }
        }
    }
}
