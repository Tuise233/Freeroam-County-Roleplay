using AltV.Net;
using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class TestCommand : IScript
    {
        public TestCommand()
        {
            CommandManager.AddCommand("getpos", (player, param) =>
            {
                ChatBox.SendChatMsgToPlayer(player, $"当前位置坐标: {player.Position.X}, {player.Position.Y}, {player.Position.Z}");
                ChatBox.SendChatMsgToPlayer(player, $"当前角度坐标: {player.Rotation.Roll}, {player.Rotation.Pitch}, {player.Rotation.Yaw}");
            });
        }
    }
}
