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
            CommandManager.AddCommand("getpos", (player, args) =>
            {
                ChatBox.SendChatMsgToPlayer(player, $"当前位置坐标: {player.Position.X}, {player.Position.Y}, {player.Position.Z}");
                ChatBox.SendChatMsgToPlayer(player, $"当前角度坐标: {player.Rotation.Roll}, {player.Rotation.Pitch}, {player.Rotation.Yaw}");
            });

            CommandManager.AddCommand("giveweapon", (player, args) =>
            {
                string weapon_name = args[0].ToString();
                int weapon_ammo = Convert.ToInt32(args[1]);
                bool exist = false;
                foreach(var weapon in WeaponManager.WeaponList)
                {
                    if(weapon.name == weapon_name)
                    {
                        player.GiveWeapon(Alt.Hash(weapon.data), weapon_ammo, true);
                        ChatBox.SendChatMsgToPlayer(player, ChatColor.Color_System, $"[系统] 成功获得一把 {weapon_name} 且附带弹药 {weapon_ammo} 发");
                        exist = true;
                        break;
                    }
                }

                if(exist == false)
                {
                    ChatBox.SendChatMsgToPlayer(player, ChatColor.Color_System, $"[系统] 未查询到名为 {weapon_name} 的武器");
                }
            });
        }
    }
}
