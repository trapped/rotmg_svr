using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace server.picture
{
    class get : IRequestHandler
    {
        byte[] buff = new byte[0x10000];
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            //warning: maybe has hidden url injection
            string id = query["id"];
            foreach (var i in id)
            {
                if (char.IsLetter(i) || i == '_' || i == '-') continue;

                byte[] status = Encoding.UTF8.GetBytes("<Error>Invalid ID.</Error>");
                context.Response.OutputStream.Write(status, 0, status.Length);
                return;
            }

            string path = Path.GetFullPath("texture/_" + id + ".png");
            if (!File.Exists(path))
            {
                byte[] status = Encoding.UTF8.GetBytes("<Error>Invalid ID.</Error>");
                context.Response.OutputStream.Write(status, 0, status.Length);
                return;
            }

            context.Response.ContentType = "image/png";
            using (var i = File.OpenRead(path))
            {
                int c;
                while ((c = i.Read(buff, 0, buff.Length)) > 0)
                    context.Response.OutputStream.Write(buff, 0, c);
            }
        }
    }
}
