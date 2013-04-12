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

namespace server.account
{
    class verify : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db = new Database())
            {
                var acc = db.Verify(query["guid"], query["password"]);
                if (acc == null)
                {
                    var status = Encoding.UTF8.GetBytes("<Error>Bad login</Error>");
                    context.Response.OutputStream.Write(status, 0, status.Length);
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(acc.GetType(), new XmlRootAttribute(acc.GetType().Name) { Namespace = "" });

                    XmlWriterSettings xws = new XmlWriterSettings();
                    xws.OmitXmlDeclaration = true;
                    xws.Encoding = Encoding.UTF8;
                    XmlWriter xtw = XmlWriter.Create(context.Response.OutputStream, xws);
                    serializer.Serialize(xtw, acc, acc.Namespaces);
                }
            }
        }
    }
}
