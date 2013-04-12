using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace server
{
    interface IRequestHandler
    {
        void HandleRequest(HttpListenerContext context);
    }

    static class RequestHandlers
    {
        public static readonly Dictionary<string, IRequestHandler> Handlers = new Dictionary<string, IRequestHandler>()
        {
            { "/crossdomain.xml", new crossdomain() },
            { "/char/list", new @char.list() },
            { "/char/delete", new @char.delete() },
            { "/char/fame", new @char.fame() },
            { "/account/register", new account.register() },
            { "/account/verify", new account.verify() },
            { "/account/forgotPassword", new account.forgotPassword() },
            { "/account/sendVerifyEmail", new account.sendVerifyEmail() },
            { "/account/changePassword", new account.changePassword() },
            { "/account/purchaseCharSlot", new account.purchaseCharSlot() },
            { "/account/setName", new account.setName() },
            { "/credits/getoffers", new credits.getoffers() },
            { "/credits/add", new credits.add() },
            { "/fame/list", new fame.list() },
        };
    }
}
