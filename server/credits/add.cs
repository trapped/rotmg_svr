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
                    string guid = (query["guid"]);
                    Console.WriteLine(guid);
                    cmd.CommandText = "UPDATE stats SET credits = credits + "+amount.ToString()+" WHERE accId="+id.ToString();
                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        cmd.CommandText = "INSERT INTO stats VALUES (credits = "+amount.ToString()+", accId = "+id.ToString()+", fame = 0,totalFame = 0, totalCredits = 0)";
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            status = "Internal Error!";
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
                        else
                        {
                            status = "Purchase successful!";
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
                else
                {
                    status = "Account does not exist!";

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
        //void stata(object id, int amount)
        //{
        //    using (var db1 = new Database())
        //    {
        //        var cmd = db1.CreateQuery();
        //        cmd.CommandText = "UPDATE stats SET credits = credits + @amount WHERE accId=@accId";
        //        cmd.Parameters.AddWithValue("@amount", amount);
        //        cmd.Parameters.AddWithValue("@accId", id);
        //        if (cmd.ExecuteNonQuery() != 1)
        //        {
        //            cmd.CommandText = "INSERT INTO stats SET credits = credits + @amount WHERE accId=@accId";
        //            cmd.Parameters.AddWithValue("@amount", amount);
        //            cmd.Parameters.AddWithValue("@accId", id);
        //        }
        //    }
        //}
    }
}