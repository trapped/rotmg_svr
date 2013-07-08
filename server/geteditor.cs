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
    class geteditor : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            string req = context.Request.Url.AbsolutePath;
            Regex rx = new Regex("TextureMaker(?<version>.*).swf");
            string ver = rx.Match(req).Groups["version"].Captures[0].Value;
            byte[] status = File.ReadAllBytes("pixel_editor/TextureMaker" + ver + ".swf");
            context.Response.ContentType = "*/*";
            context.Response.OutputStream.Write(status, 0, status.Length);
            context.Response.Close();
        }
    }
}
