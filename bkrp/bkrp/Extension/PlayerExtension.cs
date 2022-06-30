using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Args;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public static class PlayerMethod
    {
        public static void SendSuccessNotification(this PlayerEx player, string msg)
        {
            player?.Emit("SendSuccessNotification", msg);
        }
        public static void SendErrorNotification(this PlayerEx player, string msg)
        {
            player?.Emit("SendErrorNotification", msg);
        }
        public static void SendInfoNotification(this PlayerEx player, string msg)
        {
            player?.Emit("SendInfoNotification", msg);
        }
        public static void SendWarnNotification(this PlayerEx player, string msg)
        {
            player?.Emit("SendWarnNotification", msg);
        }
    }

    public class PlayerEx : Player
    {
        public PlayerEx(ICore server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id)
        {
            Account = new AccountModel();
            Character = new CharacterModel();
        }
        //New Property
        public AccountModel Account { get; set; }
        public CharacterModel Character { get; set; }
    }
}
