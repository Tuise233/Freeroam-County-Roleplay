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

        /// <summary>
        /// 冻结玩家
        /// </summary>
        /// <param name="player"></param>
        /// <param name="freezeControl">是否冻结玩家控制</param>
        /// <param name="freezePosition">是否冻结玩家位置</param>
        public static void ToggleFreeze(this PlayerEx player, bool freezeControl, bool freezePosition)
        {
            player.Emit("freeze:toggle", freezeControl, freezePosition);
        }

        /// <summary>
        /// 显示鼠标
        /// </summary>
        /// <param name="player"></param>
        /// <param name="state">显示状态</param>
        public static void ShowCursor(this PlayerEx player, bool state)
        {
            player.Emit("cursor:show", state);
        }

        /// <summary>
        /// 创建角色镜头
        /// </summary>
        /// <param name="player"></param>
        public static void CreatePedCamera(this PlayerEx player, int fov)
        {
            player.Emit("camera:start");
            Timer.SetTimeOut(50, () =>
            {
                player.Emit("camera:setFov", fov);
            });
        }

        /// <summary>
        /// 销毁角色镜头
        /// </summary>
        /// <param name="player"></param>
        public static void DestroyPedCamera(this PlayerEx player)
        {
            player.Emit("camera:stop");
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
