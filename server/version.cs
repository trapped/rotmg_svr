using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace server
{
    class flversion : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            byte[] status = File.ReadAllBytes("pixel_editor/version.txt");
            context.Response.ContentType = "text/*";
            context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}
