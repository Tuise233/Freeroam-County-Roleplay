using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class PlayerFactory : IEntityFactory<PlayerEx>
    {
        public static List<PlayerEx> Players = new List<PlayerEx>();

        public PlayerFactory()
        {
            EventManager.OnPlayerConnected += EventManager_OnPlayerConnected;
            EventManager.OnPlayerDisconnected += EventManager_OnPlayerDisconnected;
        }

        public PlayerEx Create(ICore server, IntPtr entityPointer, ushort id)
        {
            return new PlayerEx(server, entityPointer, id);
        }

        private void EventManager_OnPlayerDisconnected(PlayerEx arg1, string arg2)
        {
            if (Players.Contains(arg1))
            {
                Players.Remove(arg1);
                Log.Server($"[PlayerFactory] 移除玩家 | {arg1.Account.username} | {arg2}");
            }
        }

        private void EventManager_OnPlayerConnected(PlayerEx obj)
        {
            if (!Players.Contains(obj))
            {
                Players.Add(obj);
                Log.Server($"[PlayerFactory] 新增玩家 | {obj.SocialClubId}");
            }
        }
    }
}
