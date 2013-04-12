using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace server
{
    class crossdomain : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            byte[] status = Encoding.UTF8.GetBytes(@"<cross-domain-policy>
<allow-access-from domain=""*""/>
</cross-domain-policy>");
            context.Response.ContentType = "text/*";
            context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}
