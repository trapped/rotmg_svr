using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using db;

namespace server.picture
{
    class list : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());
            int num = int.Parse(query["num"]); //items per page
            string creator = query["guid"]; //all, mine, wildshadow
            string clientGUID = query["myGUID"]; //the user's guid/email, might be an hash for guest accounts

            using (Database dbv = new Database())
            {

            }
        }
    }
}
