using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace server
{
    class lang : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            byte[] status = Encoding.UTF8.GetBytes(File.ReadAllText("pixel_editor/lang.txt"));
            context.Response.ContentType = "text/*";
            context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}
