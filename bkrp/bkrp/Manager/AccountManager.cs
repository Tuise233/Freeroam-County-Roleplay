using AltV.Net;
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
        }

        private void EventManager_OnPlayerConnected(IPlayer player)
        {
            if(!player.HasMetaData("login"))
            {
                player.Emit("account:load");
            }
        }

        [ClientEvent("RegisterAccount")]
        public void RegisterAccount(IPlayer player, string username, string password, string email)
        {
            Log.Server($"注册帐号: {username} {password} {email}");
            Database.ExecuteSql($"SELECT * FROM `account` WHERE `username`='{username}'", (reader, err) =>
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
                                Log.Server("注册成功");
                            }
                        });
                }
            });
        }

        [ClientEvent("LoginAccount")]
        public void LoginAccount(IPlayer player, string username, string password)
        {
            Log.Server($"登录账号: {username} {password}");
            Database.ExecuteSql($"SELECT * FROM `account` WHERE `username`='{username}'", (reader, err) =>
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
                            Log.Server("登陆成功");
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
    }
}
