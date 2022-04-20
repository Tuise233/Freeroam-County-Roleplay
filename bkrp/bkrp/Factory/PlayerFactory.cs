﻿using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class PlayerFactory : IScript
    {
        public static List<IPlayer> Players = new List<IPlayer>();

        public PlayerFactory()
        {
            EventManager.OnPlayerConnected += EventManager_OnPlayerConnected;
            EventManager.OnPlayerDisconnected += EventManager_OnPlayerDisconnected;
        }

        private void EventManager_OnPlayerDisconnected(IPlayer arg1, string arg2)
        {
            if(Players.Contains(arg1))
            {
                Players.Remove(arg1);
                Log.Server($" [PlayerFactory]ID为{arg1.Id}的玩家被移出PlayerFactory，原因:{arg2}");
            }
        }

        private void EventManager_OnPlayerConnected(IPlayer obj)
        {
            if(!Players.Contains(obj))
            {
                Players.Add(obj);
                Log.Server($" [PlayerFactory]ID为{obj.Id}的玩家被加入PlayerFactory");
            }
        }
    }
}
