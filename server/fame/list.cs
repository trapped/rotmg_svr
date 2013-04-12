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

namespace server.fame
{
    class list : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            byte[] status = null;

            string span = "";
            switch (query["timespan"])
            {
                case "week":
                    span = "(time >= DATE_SUB(NOW(), INTERVAL 1 WEEK))"; break;
                case "month":
                    span = "(time >= DATE_SUB(NOW(), INTERVAL 1 MONTH))"; break;
                case "all":
                    span = "TRUE"; break;
                default:
                    status = Encoding.UTF8.GetBytes("<Error>Invalid fame list</Error>"); break;
            }
            string ac = "FALSE";
            if (query["accountId"] != null)
                ac = "(accId=@accId AND chrId=@charId)";

            if (status == null)
            {

                XmlDocument doc = new XmlDocument();
                var root = doc.CreateElement("FameList");

                var spanAttr = doc.CreateAttribute("timespan");
                spanAttr.Value = query["timespan"];
                root.Attributes.Append(spanAttr);

                doc.AppendChild(root);

                using (var db = new Database())
                {
                    var cmd = db.CreateQuery();
                    cmd.CommandText = @"SELECT accId, chrId, name, charType, tex1, tex2, items, totalFame 
FROM (SELECT * FROM (SELECT * FROM death ORDER BY totalFame DESC) x GROUP BY accId) y 
WHERE " + span + @" OR " + ac + @" 
LIMIT 10;";
                    if (query["accountId"] != null)
                    {
                        cmd.Parameters.AddWithValue("@accId", query["accountId"]);
                        cmd.Parameters.AddWithValue("@charId", query["charId"]);
                    }
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var elem = doc.CreateElement("FameListElem");

                            var accIdAttr = doc.CreateAttribute("accountId");
                            accIdAttr.Value = rdr.GetInt32("accId").ToString();
                            elem.Attributes.Append(accIdAttr);
                            var chrIdAttr = doc.CreateAttribute("charId");
                            chrIdAttr.Value = rdr.GetInt32("chrId").ToString();
                            elem.Attributes.Append(chrIdAttr);

                            root.AppendChild(elem);

                            var nameElem = doc.CreateElement("Name");
                            nameElem.InnerText = rdr.GetString("name");
                            elem.AppendChild(nameElem);
                            var objTypeElem = doc.CreateElement("ObjectType");
                            objTypeElem.InnerText = rdr.GetString("charType");
                            elem.AppendChild(objTypeElem);
                            var tex1Elem = doc.CreateElement("Tex1");
                            tex1Elem.InnerText = rdr.GetString("tex1");
                            elem.AppendChild(tex1Elem);
                            var tex2Elem = doc.CreateElement("Tex2");
                            tex2Elem.InnerText = rdr.GetString("tex2");
                            elem.AppendChild(tex2Elem);
                            var equElem = doc.CreateElement("Equipment");
                            equElem.InnerText = rdr.GetString("items");
                            elem.AppendChild(equElem);
                            var fameElem = doc.CreateElement("TotalFame");
                            fameElem.InnerText = rdr.GetString("totalFame");
                            elem.AppendChild(fameElem);
                        }
                    }
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                using (XmlWriter wtr = XmlWriter.Create(context.Response.OutputStream))
                    doc.Save(wtr);
            }
        }
    }
}
