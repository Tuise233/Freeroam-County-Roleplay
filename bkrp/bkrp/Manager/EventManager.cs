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
        public static event Action<IPlayer> OnPlayerConnected = null;

        /// <summary>
        /// 当玩家断开连接
        /// </summary>
        public static event Action<IPlayer, string> OnPlayerDisconnected = null;

        //调用
        public static void Call_OnPlayerConnected(IPlayer player) => OnPlayerConnected?.Invoke(player);
        public static void Call_OnPlayerDisconnected(IPlayer player, string reason) => OnPlayerDisconnected?.Invoke(player, reason);
    }
}
