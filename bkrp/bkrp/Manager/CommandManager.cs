using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class CommandManager : IScript
    {
        public static Dictionary<string, Action<PlayerEx, object[]>> CommandList = new Dictionary<string, Action<PlayerEx, object[]>>();
        public CommandManager()
        {
            
        }

        [ClientEvent("PushCommand")]
        public void PushCommand(PlayerEx player, string command, object[] param)
        {
            //判断是否存在指令
            if(CommandList.ContainsKey(command))
            {
                CommandList[command]?.Invoke(player, param);
            }
        }

        public static void AddCommand(string command, Action<PlayerEx, object[]> action)
        {
            if(CommandList.ContainsKey(command))
            {
                Log.Error($"[CommandManager] 命令{command}已存在，注册命令失败");
                return;
            }

            CommandList.Add(command, action);
        }
    }
}
