using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using db;
using System.Web;

namespace server.credits
{
    class add : IRequestHandler
    {
        public void HandleRequest(HttpListenerContext context)
        {
            string status;
            using (var db = new Database())
            {
                var query = HttpUtility.ParseQueryString(context.Request.Url.Query);

                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT id FROM accounts WHERE uuid=@uuid";
                cmd.Parameters.AddWithValue("@uuid", query["guid"]);
                object id = cmd.ExecuteScalar();

                if (id != null)
                {
                    int amount = int.Parse(query["jwt"]);
                    cmd = db.CreateQuery();
                    cmd.CommandText = "UPDATE stats SET credits = credits + @amount WHERE accId=@accId";
                    cmd.Parameters.AddWithValue("@accId", (int)id);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    int result = (int)cmd.ExecuteNonQuery();
                    if (result > 0)
                        status = "Your purchase was successful!";
                    else
                        status = "Internal error!";
                }
                else
                    status = "Account does not exist!";
            }

            var res = Encoding.UTF8.GetBytes(
@"<html>
    <head>
        <title>Purchase!</title>
    </head>
    <body style='background: #333333'>
        <h1 style='color: #EEEEEE; text-align: center'>
            " + status + @"
        </h1>
    </body>
</html>");
            context.Response.OutputStream.Write(res, 0, res.Length);
        }
    }
}