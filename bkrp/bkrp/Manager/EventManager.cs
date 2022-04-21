using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class EventManager
    {
        /// <summary>
        /// 当玩家连接时
        /// </summary>
        public static event Action<PlayerEx> OnPlayerConnected = null;

        /// <summary>
        /// 当玩家断开连接
        /// </summary>
        public static event Action<PlayerEx, string> OnPlayerDisconnected = null;

        /// <summary>
        /// 当玩家结束登录系统
        /// </summary>
        /// <param name="player"></param>
        public static event Action<PlayerEx, AccountModel> OnPlayerLogin = null;

        //调用
        public static void Call_OnPlayerConnected(IPlayer player) => OnPlayerConnected?.Invoke(player as PlayerEx);
        public static void Call_OnPlayerDisconnected(PlayerEx player, string reason) => OnPlayerDisconnected?.Invoke(player, reason);
        public static void Call_OnPlayerLogin(PlayerEx player, AccountModel model) => OnPlayerLogin?.Invoke(player, model);
    }
}
