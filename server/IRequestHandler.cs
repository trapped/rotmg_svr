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
            { "/guild/getBoard", new account.guild.getBoard() },
            { "/guild/setBoard", new account.guild.setBoard() },
            { "/guild/listMembers", new account.guild.listMembers() },
            { "/picture/get", new picture.get() },
            { "/picture/list", new picture.list() },
            //{ "/picture/save", new picture.save() },
            { "/version.txt", new flversion() },
            { "/TextureMakerTRAPPED.swf", new geteditor() },
            { "/app/getLanguageStrings", new lang() },
            { "/sfx/button_click.mp3", new sfx() },
            { "/music/sorc.mp3", new music() },
            { "/sfx/death_screen.mp3", new sfx() },
            { "/sfx/enter_realm.mp3", new sfx() },
            { "/sfx/error.mp3", new sfx() },
            { "/sfx/inventory_move_item.mp3", new sfx() },
            { "/sfx/level_up.mp3", new sfx() },
            { "/sfx/loot_appears.mp3", new sfx() },
            { "/sfx/no_mana.mp3", new sfx() },
            { "/sfx/use_key.mp3", new sfx() },
            { "/sfx/use_potion.mp3", new sfx() },
        };
    }
}
