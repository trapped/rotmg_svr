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
    class delete : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db = new Database())
            {
                var acc = db.Verify(query["guid"], query["password"]);
                byte[] status;
                if (acc == null)
                    status = Encoding.UTF8.GetBytes("<Error>Bad login</Error>");
                else
                {
                    var cmd = db.CreateQuery();
                    cmd.CommandText = @"DELETE FROM characters WHERE accId = @accId AND charId = @charId;";
                    cmd.Parameters.AddWithValue("@accId", acc.AccountId);
                    cmd.Parameters.AddWithValue("@charId", query["charId"]);
                    if (cmd.ExecuteNonQuery() > 0)
                        status = Encoding.UTF8.GetBytes("<Success />");
                    else
                        status = Encoding.UTF8.GetBytes("<Error>Internal Error</Error>");
                }
                context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
