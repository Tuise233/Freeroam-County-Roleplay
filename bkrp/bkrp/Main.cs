using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace bkrp
{
    class Main : IScript
    {
        public Main()
        {
            Alt.OnAnyResourceStart += Alt_OnAnyResourceStart;
            Alt.OnPlayerConnect += Alt_OnPlayerConnect;
            Alt.OnPlayerDisconnect += Alt_OnPlayerDisconnect;
        }

        private void Alt_OnAnyResourceStart(INativeResource resource)
        {
            Log.Server("Altv - Freeroam County Role Play - 2022 启动成功");
            Log.Server("Coded By Brandon_Karl");
        }

        private void Alt_OnPlayerDisconnect(IPlayer player, string reason)
        {
            EventManager.Call_OnPlayerDisconnected(player, reason);
        }

        private void Alt_OnPlayerConnect(IPlayer player, string reason)
        {
            EventManager.Call_OnPlayerConnected(player);
        }
    }
}
