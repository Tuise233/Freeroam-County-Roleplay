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
            EventManager.OnPlayerLogin += EventManager_OnPlayerLogin;
        }

        private void EventManager_OnPlayerLogin(PlayerEx player, AccountModel model)
        {
            player.Account = model;
        }

        private void EventManager_OnPlayerConnected(PlayerEx player)
        {
            if(!player.HasMetaData("login"))
            {
                player.Emit("account:load");
            }
        }

        [ClientEvent("RegisterAccount")]
        public void RegisterAccount(IPlayer player, string username, string password, string email)
        {
            Log.Server($"[AccountManager] 注册账号 | {username} | {password} | {email}");
            Database.ExecuteSql($"SELECT * FROM `account` WHERE BINARY `username`='{username}'", (reader, err) =>
            {
                if(err)
                {
                    player.Emit("accont:client-displayErr", "数据库异常抛出");
                    return;
                }
                else
                {
                    while(reader.Read())
                    {
                        player.Emit("account:client-displayErr", "您输入的用户名已被注册");
                        return;
                    }

                    //注册账号
                    Database.ExecuteSql($"INSERT INTO `account` (`username`, `password`, `email`) VALUES ('{username}', '{password}', '{email}')", (err) =>
                        {
                            if (err)
                            {
                                player.Emit("accont:client-displayErr", "数据库异常抛出");
                                return;
                            }
                            else
                            {
                                //注册成功
                                Database.ExecuteSql($"SELECT * FROM `account` WHERE `username`='${username}'", (reader, err) =>
                                {
                                    if(err)
                                    {
                                        player.Emit("accont:client-displayErr", "数据库异常抛出");
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
                    player.Emit("accont:client-displayErr", "数据库异常抛出");
                    return;
                }
                else
                {
                    while(reader.Read())
                    {
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
                            return;
                        }
                        else
                        {
                            player.Emit("account:client-displayErr", "您输入的账号或密码有误");
                            return;
                        }
                    }
                }
            });
        }

        public static void SelectCharacter(PlayerEx player)
        {
            //Database.ExecuteSql($"SELECT * FROM `character` WHERE `uid`='{player.Account.id}'", (reader, err) =>
            //{
            //    if(err)
            //    {
            //        player.SendErrorNotification("数据库异常抛出");
            //        return;
            //    }
            //    while(reader.Read())
            //    {
            //        player.Dimension = 0;
            //        player.Position = new Position(-533.1306f, -219.414f, 37.64975f);
            //        player.Rotation = new Rotation(0f, 0f, 177.7417f / 59);
            //        player.Model = 0xD1FEB884;
            //        player.Emit("account:destroy");
            //        break;
            //    }
            //});
            Database.ExecuteSql($"SELECT * FROM `character` WHERE `userid`='${player.Account.id}'", (reader, error) =>
            {
                if (error)
                {
                    Log.Error("SelectCharacter数据库操作异常抛出");
                    return;
                }
                bool exist = false;
                while(reader.Read())
                {
                    Log.Server("用户有角色");
                    exist = true;
                    break;
                }

                if(exist == false)
                {
                    Log.Server("用户没有创建角色");
                }
            });
        }
    }
}
