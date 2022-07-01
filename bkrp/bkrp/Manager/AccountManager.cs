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
    class AccountManager : IScript
    {
        public AccountManager()
        {
            EventManager.OnPlayerConnected += EventManager_OnPlayerConnected;
            EventManager.OnPlayerDisconnected += EventManager_OnPlayerDisconnected;
            EventManager.OnPlayerLogin += EventManager_OnPlayerLogin;
            EventManager.OnPlayerComeInWorld += EventManager_OnPlayerComeInWorld;
        }

        private void EventManager_OnPlayerComeInWorld(PlayerEx player)
        {
            ChatBox.SendChatMsgToAll(ChatColor.Color_System, $"[系统] 玩家 {player.Character.name} 进入服务器");
        }

        private void EventManager_OnPlayerDisconnected(PlayerEx player, string reason)
        {
            ChatBox.SendChatMsgToAll(ChatColor.Color_System, $"[系统] 玩家 {player.Character.name} 退出服务器");
        }

        private void EventManager_OnPlayerLogin(PlayerEx player, AccountModel model)
        {
            player.Account = model;
        }

        private void EventManager_OnPlayerConnected(PlayerEx player)
        {
            if(!player.HasMetaData("login"))
            {
                player.Emit("chat:load");
                player.Emit("account:load");
            }
        }

        [ClientEvent("RegisterAccount")]
        public void RegisterAccount(IPlayer player, string username, string password, string email)
        {
            Log.Server($"[AccountManager] 注册账号 | {username} | {password} | {email}");
            Database.ExecuteSql($"SELECT * FROM `account` WHERE BINARY `username`='{username}' OR `email`='{email}'", (reader, err) =>
            {
                if(err)
                {
                    player.Emit("account:displayErr", "数据库异常抛出");
                    return;
                }
                else
                {
                    while(reader.Read())
                    {
                        player.Emit("account:displayErr", "您输入的用户名或邮箱已被注册");
                        return;
                    }

                    //注册账号
                    Database.ExecuteSql($"INSERT INTO `account` (`username`, `password`, `email`) VALUES ('{username}', '{password}', '{email}')", (err) =>
                        {
                            if (err)
                            {
                                player.Emit("account:displayErr", "数据库异常抛出");
                                return;
                            }
                            else
                            {
                                //注册成功
                                Database.ExecuteSql($"SELECT * FROM `account` WHERE `username`='${username}'", (reader, err) =>
                                {
                                    if(err)
                                    {
                                        player.Emit("account:displayErr", "数据库异常抛出");
                                        return;
                                    }
                                    else
                                    {
                                        AccountModel model = new AccountModel
                                        {
                                            id = reader.GetInt32("id"),
                                            username = reader.GetString("username"),
                                            password = reader.GetString("password"),
                                            email = reader.GetString("email")
                                        };
                                        EventManager.Call_OnPlayerLogin(player as PlayerEx, model);
                                        SelectCharacter(player as PlayerEx);
                                    }
                                });
                            }
                        });
                }
            });
        }

        [ClientEvent("LoginAccount")]
        public void LoginAccount(IPlayer player, string username, string password)
        {
            Log.Server($"[AccountManager] 登录账号 | {username} | {password}");
            Database.ExecuteSql($"SELECT * FROM `account` WHERE BINARY `username`='{username}'", (reader, err) =>
            {
                if(err)
                {
                    player.Emit("account:displayErr", "数据库异常抛出");
                    return;
                }
                else
                {
                    bool exist = false;
                    while(reader.Read())
                    {
                        exist = true;
                        if(reader.GetString("password") == password)
                        {
                            AccountModel model = new AccountModel
                            {
                                id = reader.GetInt32("id"),
                                username = reader.GetString("username"),
                                password = reader.GetString("password"),
                                email = reader.GetString("email")
                            };
                            EventManager.Call_OnPlayerLogin(player as PlayerEx, model);
                            SelectCharacter(player as PlayerEx);
                        }
                        else
                        {
                            player.Emit("account:displayErr", "您输入的账号或密码有误");
                        }
                    }

                    if(exist == false)
                    {
                        player.Emit("account:displayErr", $"未找到用户名未{username}的帐号");
                    }
                }
            });
        }

        public static void SelectCharacter(PlayerEx player)
        {
            player.Emit("account:destroy");
            Database.ExecuteSql($"SELECT * FROM `character` WHERE `userid`='{player.Account.id}'", (reader, error) =>
            {
                if (error)
                {
                    Log.Error("SelectCharacter数据库操作异常抛出");
                    return;
                }
                bool exist = false;
                while(reader.Read())
                {
                    exist = true;
                    CharacterManager.LoadCharacter(player, false);
                    break;
                }

                if(exist == false)
                {
                    player.Dimension = 100 + player.Account.id;
                    player.Position = new Position(428.59f, -980.91f, 30.71f);
                    player.Rotation = new Rotation(0f, 0f, 1.42f);
                    player.Model = 0x5E3DA4A4;
                    CharacterManager.OpenCharacterView(player);
                }
            });
        }
    }
}
