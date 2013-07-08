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
    class sfx : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            string req = context.Request.Url.AbsolutePath;
            Regex rx = new Regex("/sfx/(?<filename>.*).mp3");
            string filename = rx.Match(req).Groups["filename"].Captures[0].Value;
            byte[] status = File.ReadAllBytes("pixel_editor/sfx/" + filename + ".mp3");
            context.Response.ContentType = "*/*";
            context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
    class music : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            string req = context.Request.Url.AbsolutePath;
            Regex rx = new Regex("/(?<filename>.*).mp3");
            string filename = rx.Match(req).Groups["filename"].Captures[0].Value;
            byte[] status = File.ReadAllBytes("pixel_editor/sfx/" + filename + ".mp3");
            context.Response.ContentType = "*/*";
            context.Response.OutputStream.Write(status, 0, status.Length);
        }
    }
}
