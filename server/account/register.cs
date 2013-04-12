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
using System.Globalization;

namespace server.account
{
    class register : IRequestHandler
    {
        public bool IsValidEmail(string strIn)
        {
            var invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            MatchEvaluator DomainMapper = match =>
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            };

            // Use IdnMapping class to convert Unicode domain names. 
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase);
        }

        public void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            using (var db = new Database())
            {
                byte[] status;
                if (!IsValidEmail(query["newGUID"]))
                    status = Encoding.UTF8.GetBytes("<Error>Invalid email</Error>");
                else
                {
                    if (db.HasUuid(query["guid"]) &&
                        db.Verify(query["guid"], "") != null)
                    {
                        if (db.HasUuid(query["newGUID"]))
                            status = Encoding.UTF8.GetBytes("<Error>Duplicated email</Error>");
                        else
                        {
                            var cmd = db.CreateQuery();
                            cmd.CommandText = "UPDATE accounts SET uuid=@newUuid, password=SHA1(@password), guest=FALSE WHERE uuid=@uuid;";
                            cmd.Parameters.AddWithValue("@uuid", query["guid"]);
                            cmd.Parameters.AddWithValue("@newUuid", query["newGUID"]);
                            cmd.Parameters.AddWithValue("@password", query["newPassword"]);
                            if (cmd.ExecuteNonQuery() > 0)
                                status = Encoding.UTF8.GetBytes("<Success />");
                            else
                                status = Encoding.UTF8.GetBytes("<Error>Internal Error</Error>");
                        }
                    }
                    else
                    {
                        if (db.Register(query["newGUID"], query["newPassword"], false) != null)
                            status = Encoding.UTF8.GetBytes("<Success />");
                        else
                            status = Encoding.UTF8.GetBytes("<Error>Internal Error</Error>");
                    }
                }
                context.Response.OutputStream.Write(status, 0, status.Length);
            }
        }
    }
}
