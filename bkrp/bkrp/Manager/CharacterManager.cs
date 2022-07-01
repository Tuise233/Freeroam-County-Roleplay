using AltV.Net;
using AltV.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class CharacterManager : IScript
    {
        public static List<uint> MaleModel = new List<uint>()
        {
            0x5E3DA4A4,
            0x9AB35F63,
            0x739B1EF5,
            0xEF7135AE,
            0xB144F9B9
        };

        public static List<uint> FemaleModel = new List<uint>()
        {
            0x15F8700D,
            0x4161D042,
            0x9FC7F637
        };

        [ClientEvent("ToggleSex")]
        public void ToggleSex(PlayerEx player, int sex)
        {
            if (sex == 0)
            {
                player.Model = FemaleModel[0];
            }
            else
            {
                player.Model = MaleModel[0];
            }
            string msg = sex == 0 ? "切换性别为 ~y~女性" : "切换性别为 ~y~男性";
            player.SendInfoNotification(msg);
        }

        [ClientEvent("RandomModel")]
        public void RandomModel(PlayerEx player, int sex, int model)
        {
            if (sex == 0)
            {
                player.Model = FemaleModel[model];
            }
            else
            {
                player.Model = MaleModel[model];
            }
            player.SendInfoNotification("随机切换角色模型");
        }

        [ClientEvent("CreateCharacter")]
        public void CreateCharacter(PlayerEx player, string name, int age, int sex, int model)
        {
            if (age < 18 || age > 65)
            {
                player.SendErrorNotification("角色年龄范围应在 ~y~18岁~65岁 ~w~之间");
                return;
            }
            Database.ExecuteSql($"SELECT * FROM `character` WHERE `userid`='{player.Account.id}'", (reader, error) =>
            {
                if (error)
                {
                    player.SendErrorNotification("数据库异常抛出");
                    return;
                }
                while (reader.Read())
                {
                    player.SendErrorNotification("该账号已存在角色，请重新连接服务器");
                    return;
                }
            });
            CharacterModel characterModel = new CharacterModel(player.Account.id, name, age, sex, model);
            player.Character = characterModel;
            Database.ExecuteSql($"INSERT INTO `character` (`userid`, `name`, `age`, `sex`, `model`) VALUES ('{player.Account.id}', '{name}', '{age}', '{sex}', '{model}')", (error) =>
            {
                if (error)
                {
                    player.SendErrorNotification("数据库异常抛出");
                    return;
                }
                player.SendSuccessNotification("创建角色成功");
                CloseCharacterView(player);
            });
        }

        public static void LoadCharacter(PlayerEx player, bool register)
        {
            if (!register)
            {
                Database.ExecuteSql($"SELECT * FROM `character` WHERE `userid`='{player.Account.id}'", (reader, error) =>
                {
                    if (error)
                    {
                        player.SendErrorNotification("数据库异常抛出");
                        return;
                    }
                    while(reader.Read())
                    {
                        CharacterModel characterModel = new CharacterModel(reader.GetInt32("userid"), reader.GetString("name"), reader.GetInt32("age"), reader.GetInt32("sex"), reader.GetInt32("model"));
                        player.Character = characterModel;
                        break;
                    }
                });
            }
            player.ToggleFreeze(false, false);
            player.Dimension = 0;
            player.Model = player.Character.sex == 1 ? MaleModel[player.Character.model] : FemaleModel[player.Character.model];
            player.Position = new Position(459.43f, -990.82f, 30.68f);
            player.Rotation = new Rotation(0f, 0f, 1.51f);
            EventManager.Call_OnPlayerComeInWorld(player);
        }

        public static void OpenCharacterView(PlayerEx player)
        {
            player.ToggleFreeze(true, true);
            Timer.SetTimeOut(100, () =>
            {
                player.CreatePedCamera(90);
            });
            player.Emit("character:load");
        }

        public static void CloseCharacterView(PlayerEx player)
        {
            player.Emit("character:destroy");
            player.DestroyPedCamera();
            LoadCharacter(player, true);
        }
    }
}
