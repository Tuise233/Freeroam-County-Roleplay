using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace bkrp
{
    class Main : AsyncResource
    {
        private void Alt_OnAnyResourceStart(INativeResource resource)
        {
            Log.Server("Altv - Freeroam County Role Play - 2022 启动成功");
            Log.Server("Coded By Brandon_Karl");
        }

        private void Alt_OnPlayerDisconnect(IPlayer player, string reason)
        {
            EventManager.Call_OnPlayerDisconnected(player as PlayerEx, reason);
        }

        private void Alt_OnPlayerConnect(IPlayer player, string reason)
        {
            Console.WriteLine(player.Id);
            player.Model = 0x705E61F2;
            player.Spawn(new Position(0f, 0f, 0f), 500);
            player.Dimension = 100 + player.Id;
            EventManager.Call_OnPlayerConnected(player);
        }

        public override void OnStart()
        {
            Alt.OnAnyResourceStart += Alt_OnAnyResourceStart;
            Alt.OnPlayerConnect += Alt_OnPlayerConnect;
            Alt.OnPlayerDisconnect += Alt_OnPlayerDisconnect;
        }

        public override void OnStop()
        {
            throw new NotImplementedException();
        }


        //工厂构造
        public override IEntityFactory<IPlayer> GetPlayerFactory() => new PlayerFactory();
    }
}
