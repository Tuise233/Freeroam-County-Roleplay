using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class ChatBox : IScript
    {
        public ChatBox()
        {
            EventManager.OnPlayerComeInWorld += (player) =>
            {
                player.Emit("chat:toggleChatBox", true);
            };
        }

        [ClientEvent("PushMessage")]
        public static void PushMessage(PlayerEx player, string message)
        {
            SendChatMsgToAll($"[{player.Account.id}]{player.Account.username}: {message}");
        }

        public static void SendChatMsgToPlayer(PlayerEx player, string message)
        {
            player.Emit("chat:pushMessage", message);
        }

        public static void SendChatMsgToPlayer(PlayerEx player, string color, string message)
        {
            player.Emit("chat:pushMessage", ResolveMessage(color, message));
        }

        public static void SendChatMsgToAll(string message)
        {
            PlayerFactory.Players.ForEach((player) =>
            {
                SendChatMsgToPlayer(player, message);
            });
        }

        public static void SendChatMsgToAll(string color, string message)
        {
            PlayerFactory.Players.ForEach((player) =>
            {
                SendChatMsgToPlayer(player, color, message);
            });
        }

        public static void SendNearByMsgFromPlayer(PlayerEx player, string message)
        {
            PlayerFactory.Players.ForEach((target) =>
            {
                if (player.Position.Distance(target.Position) < 10)
                {
                    SendChatMsgToPlayer(target, ChatColor.Color_NearBy, message);
                }
            });
        }

        public static string ResolveMessage(string color, string message)
        {
            return $"<font color='{color}'>{message}</font>";
        }
    }

    class ChatColor
    {
        //基础颜色
        public const string Color_White = "#FFFFFF";
        public const string Color_Black = "#000000";

        public const string Color_Red = "#FF0000";
        public const string Color_LightRed = "#F08080";
        public const string Color_DeepRed = "#8B1A1A";

        public const string Color_Green = "#00FF00";
        public const string Color_LightGreen = "#7FFFD4";
        public const string Color_DeepGreen = "#228B22";

        public const string Color_Blue = "#0000FF";
        public const string Color_LightBlue = "#5CACEE";
        public const string Color_DeepBlue = "#00008B";


        //功能颜色
        public const string Color_NearBy = "#8968CD";
        public const string Color_System = "#F0E68C";
    }
}
