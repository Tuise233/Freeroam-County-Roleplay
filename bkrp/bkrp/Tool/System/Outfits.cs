using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace bkrp
{
    class Outfits : IScript
    {
        public class Outfit
        {
            public Tuple<int, int>[] Components { get; set; }
            public Tuple<int, int>[] Props { get; set; }
        }

        public Outfits()
        {
            OutfitTester_Init();
        }

        const int MaxComponent = 12;
        const int MaxProp = 9;
        static List<Outfit> MaleOutfits = new List<Outfit>();
        static List<Outfit> FemaleOutfits = new List<Outfit>();

        public static void OutfitTester_Init()
        {
            if (!System.IO.File.Exists("scriptmetadata.meta"))
            {
                Log.Error("[Outfits] 读取外置服装数据出错");
                Log.Error("[Outfits] 请使用OpenIV从\"update\\update.rpf\\common\\data\"导出数据");
                return;
            }
            else
            {
                Log.Server("[Outfits] 正在载入scriptmetadata.meta...");
            }

            XmlDocument doc = new XmlDocument();
            doc.Load("scriptmetadata.meta");
            Log.Server("[Outfits] 成功载入scriptmetadata.meta");
            // 200IQ code incoming
            foreach (XmlNode node in doc.SelectNodes("/CScriptMetadata/MPOutfits/*/MPOutfitsData/Item"))
            {
                Outfit newOutfit = new Outfit
                {
                    Components = new Tuple<int, int>[MaxComponent],
                    Props = new Tuple<int, int>[MaxProp]
                };

                // Load components
                XmlNode components = node.SelectSingleNode("ComponentDrawables");
                XmlNode componentTextures = node.SelectSingleNode("ComponentTextures");

                for (int compID = 0; compID < MaxComponent; compID++)
                {
                    newOutfit.Components[compID] = new Tuple<int, int>(Convert.ToInt32(components.ChildNodes[compID].Attributes["value"].Value), Convert.ToInt32(componentTextures.ChildNodes[compID].Attributes["value"].Value));
                }

                // Load props
                XmlNode props = node.SelectSingleNode("PropIndices");
                XmlNode propTextures = node.SelectSingleNode("PropTextures");

                for (int propID = 0; propID < MaxProp; propID++)
                {
                    newOutfit.Props[propID] = new Tuple<int, int>(Convert.ToInt32(props.ChildNodes[propID].Attributes["value"].Value), Convert.ToInt32(propTextures.ChildNodes[propID].Attributes["value"].Value));
                }

                switch (node.ParentNode.ParentNode.Name)
                {
                    case "MPOutfitsDataMale":
                        MaleOutfits.Add(newOutfit);
                        break;

                    case "MPOutfitsDataFemale":
                        FemaleOutfits.Add(newOutfit);
                        break;

                    default:
                        //Log.Server("WTF?");
                        break;
                }
            }

            //Log.Server("Loaded {0} outfits for FreemodeMale01.", MaleOutfits.Count);
            //Log.Server("Loaded {0} outfits for FreemodeFemale01.", FemaleOutfits.Count);
        }

        /*
        public void command_settraje(PlayerEx player, string idOrName, int value)
        {
            if (AccountManage.GetPlayerAdmin(player) < 1)
            {
                Main.SendErrorMessage(player, "您无权使用此命令.");
                return;
            }
            Client target = Main.findPlayer(player, idOrName);

            if (target != null)
            {
                if (target.GetData("status") != true)
                {
                    Main.SendErrorMessage(player, "Este jogador não está conectado.");
                    return;
                }

                if (player != target) NAPI.Chat.SendChatMessageToPlayer(player, "Você deu à ~y~" + AccountManage.GetCharacterName(target) + " o traje de numero " + value + ".");
                else Main.SendInfoMessage(player, "Você recebeu de ~y~" + AccountManage.GetCharacterName(target) + " o traje de numero " + value + ".");


                SetUnisexOutfit(target, value, true);
            }
        }


        [Command("traje")]
        public void CMD_Outfit(PlayerEx player, int ID)
        {
            if (AccountManage.GetPlayerAdmin(player) < 1)
            {
                Main.SendErrorMessage(player, "您无权使用此命令.");
                return;
            }
            switch ((PedHash)player.Model)
            {
                case PedHash.FreemodeMale01:
                    if (ID < 0 || ID >= MaleOutfits.Count)
                    {
                        player.SendChatMessage("无效的ID,有效的ID范围: 0 - " + (MaleOutfits.Count - 1) + ".");
                        return;
                    }

                    for (int i = 0; i < MaxComponent; i++)
                    {
                        if (i == 0) continue;
                        if (i == 2) continue;
                        player.SetClothes(i, MaleOutfits[ID].Components[i].Item1, MaleOutfits[ID].Components[i].Item2);
                    }

                    for (int i = 0; i < MaxProp; i++)
                    {
                        player.ClearAccessory(i);
                        player.SetAccessories(i, MaleOutfits[ID].Props[i].Item1, MaleOutfits[ID].Props[i].Item2);
                    }
                    AccountManage.SaveCharacter(player);
                    break;

                case PedHash.FreemodeFemale01:
                    if (ID < 0 || ID >= FemaleOutfits.Count)
                    {

                        player.SendChatMessage("无效的ID,有效的ID范围: 0 - " + (FemaleOutfits.Count - 1) + ".");
                        return;
                    }

                    for (int i = 0; i < MaxComponent; i++)
                    {
                        if (i == 0) continue;
                        if (i == 2) continue;
                        player.SetClothes(i, FemaleOutfits[ID].Components[i].Item1, FemaleOutfits[ID].Components[i].Item2);
                    }

                    for (int i = 0; i < MaxProp; i++)
                    {
                        player.ClearAccessory(i);
                        player.SetAccessories(i, FemaleOutfits[ID].Props[i].Item1, FemaleOutfits[ID].Props[i].Item2);
                    }

                    AccountManage.SaveCharacter(player);
                    break;

                default:
                    player.SendChatMessage("你当前的角色模型不支持使用该指令.");
                    break;
            }
        }
        

        public static void SetMaleOutfit(PlayerEx player, int ID)
        {
            if (ID < 0 || ID >= MaleOutfits.Count)
            {
                player.SendChatMessage("Invalid outfit ID, valid IDs: 0 - " + (MaleOutfits.Count - 1) + ".");
                return;
            }

            for (int i = 0; i < MaxComponent; i++)
            {
                if (i == 0) continue;
                if (i == 2) continue;
                player.SetClothes(i, MaleOutfits[ID].Components[i].Item1, MaleOutfits[ID].Components[i].Item2);
            }

            for (int i = 0; i < MaxProp; i++)
            {
                player.ClearAccessory(i);
                player.SetAccessories(i, MaleOutfits[ID].Props[i].Item1, MaleOutfits[ID].Props[i].Item2);
            }
        }

        public static void SetFemaleOutfit(PlayerEx player, int ID)
        {
            if (ID < 0 || ID >= FemaleOutfits.Count)
            {

                player.SendChatMessage("Invalid outfit ID, valid IDs: 0 - " + (FemaleOutfits.Count - 1) + ".");
                return;
            }

            for (int i = 0; i < MaxComponent; i++)
            {
                if (i == 0) continue;
                if (i == 2) continue;
                player.SetClothes(i, FemaleOutfits[ID].Components[i].Item1, FemaleOutfits[ID].Components[i].Item2);
            }

            for (int i = 0; i < MaxProp; i++)
            {
                player.ClearAccessory(i);
                player.SetAccessories(i, FemaleOutfits[ID].Props[i].Item1, FemaleOutfits[ID].Props[i].Item2);
            }
        }

        public static void RemovePlayerDutyOutfit(PlayerEx player)
        {
            NAPI.Data.SetEntityData(player, "character_duty_outfit", -1);
            Main.UpdatePlayerClothes(player);
        }
        */

        public static void SetUnisexOutfit(PlayerEx player, int ID, bool save = false)
        {
            switch (player.Model)
            {
                case 0x9C9EFFD8:
                    if (ID < 0 || ID >= MaleOutfits.Count)
                    {
                        //player.SendChatMessage("Invalid outfit ID, valid IDs: 0 - " + (MaleOutfits.Count - 1) + ".");
                        player.SendErrorNotification($"无效的服装ID，有效ID范围: 0~{MaleOutfits.Count - 1}");
                        return;
                    }

                    for (int i = 0; i < MaxComponent; i++)
                    {
                        if (i == 0) continue;
                        if (i == 1)
                        {
                            player.SetClothes((byte)i, 0, 0, 0);
                            if (save == true)
                            {
                                player.SetSyncedMetaData("character_mask", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_mask_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            continue;
                        }
                        if (i == 2) continue;
                        if (save == true)
                        {

                            if (i == 1)
                            {
                                player.SetSyncedMetaData("character_mask", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_mask_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 3)
                            {
                                player.SetSyncedMetaData("character_torso", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_torso_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 4)
                            {
                                player.SetSyncedMetaData("character_leg", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_leg_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 6)
                            {
                                player.SetSyncedMetaData("character_feet", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_feet_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 8)
                            {
                                player.SetSyncedMetaData("character_undershirt", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_undershirt_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 9)
                            {
                                player.SetSyncedMetaData("character_armor", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_armor_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 11)
                            {
                                player.SetSyncedMetaData("character_shirt", MaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_shirt_texture", MaleOutfits[ID].Components[i].Item2);
                            }
                        }
                        player.SetClothes((byte)i, (ushort)MaleOutfits[ID].Components[i].Item1, (byte)MaleOutfits[ID].Components[i].Item2, 0);
                    }

                    for (int i = 0; i < MaxProp; i++)
                    {
                        if (save == true)
                        {
                            if (i == 0)
                            {
                                player.SetSyncedMetaData("character_hats", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_hats_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 1)
                            {
                                player.SetSyncedMetaData("character_glasses", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_glasses_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 2)
                            {
                                player.SetSyncedMetaData("character_ears", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_ears_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 6)
                            {
                                player.SetSyncedMetaData("character_watches", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_watches_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 7)
                            {
                                player.SetSyncedMetaData("character_bracelets", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_bracelets_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                        }

                        if (ID == 193)
                        {
                            if (i == 0)
                            {
                                player.SetSyncedMetaData("character_hats", 0);
                                player.SetSyncedMetaData("character_hats_texture", 0);
                                player.ClearProps((byte)i);
                                //player.ClearAccessory(i);
                                continue;
                            }
                            if (i == 1)
                            {
                                player.SetSyncedMetaData("character_glasses", 0);
                                player.SetSyncedMetaData("character_glasses_texture", 0);
                                player.ClearProps((byte)i);
                                //player.ClearAccessory(i);
                                continue;
                            }
                        }
                        player.ClearProps((byte)i);
                        player.SetProps((byte)i, (ushort)MaleOutfits[ID].Props[i].Item1, (byte)MaleOutfits[ID].Props[i].Item2);
                        //player.ClearAccessory(i);
                        //player.SetAccessories(i, MaleOutfits[ID].Props[i].Item1, MaleOutfits[ID].Props[i].Item2);
                    }

                    break;

                case 0x705E61F2:
                    if (ID < 0 || ID >= FemaleOutfits.Count)
                    {
                        //player.SendChatMessage("Invalid outfit ID, valid IDs: 0 - " + (FemaleOutfits.Count - 1) + ".");
                        player.SendErrorNotification($"无效的服装ID，有效ID范围: 0~{FemaleOutfits.Count - 1}");
                        return;
                    }

                    for (int i = 0; i < MaxComponent; i++)
                    {
                        if (i == 0) continue;
                        if (i == 1)
                        {
                            player.SetClothes((byte)i, 0, 0, 0);
                            continue;
                        }
                        if (i == 2) continue;
                        if (save == true)
                        {

                            if (i == 1)
                            {
                                player.SetSyncedMetaData("character_mask", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_mask_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 3)
                            {
                                player.SetSyncedMetaData("character_torso", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_torso_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 4)
                            {
                                player.SetSyncedMetaData("character_leg", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_leg_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 6)
                            {
                                player.SetSyncedMetaData("character_feet", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_feet_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 8)
                            {
                                player.SetSyncedMetaData("character_undershirt", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_undershirt_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 9)
                            {
                                player.SetSyncedMetaData("character_armor", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_armor_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                            else if (i == 11)
                            {
                                player.SetSyncedMetaData("character_shirt", FemaleOutfits[ID].Components[i].Item1);
                                player.SetSyncedMetaData("character_shirt_texture", FemaleOutfits[ID].Components[i].Item2);
                            }
                        }
                        player.SetClothes((byte)i, (ushort)FemaleOutfits[ID].Components[i].Item1, (byte)FemaleOutfits[ID].Components[i].Item2, 0);
                    }

                    for (int i = 0; i < MaxProp; i++)
                    {
                        if (save == true)
                        {
                            if (i == 0)
                            {
                                player.SetSyncedMetaData("character_hats", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_hats_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 1)
                            {
                                player.SetSyncedMetaData("character_glasses", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_glasses_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 2)
                            {
                                player.SetSyncedMetaData("character_ears", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_ears_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 6)
                            {
                                player.SetSyncedMetaData("character_watches", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_watches_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                            else if (i == 7)
                            {
                                player.SetSyncedMetaData("character_bracelets", FemaleOutfits[ID].Props[i].Item1);
                                player.SetSyncedMetaData("character_bracelets_texture", FemaleOutfits[ID].Props[i].Item2);
                            }
                        }
                        player.ClearProps((byte)i);
                        player.SetProps((byte)i, (ushort)FemaleOutfits[ID].Props[i].Item1, (byte)FemaleOutfits[ID].Props[i].Item2);
                        //player.ClearAccessory(i);
                        //player.SetAccessories(i, FemaleOutfits[ID].Props[i].Item1, FemaleOutfits[ID].Props[i].Item2);
                    }
                    break;

                default:
                    //player.SendChatMessage("This command only works with FreemodeMale01 and FreemodeFemale01.");
                    player.SendErrorNotification("模型错误，无法更换服装");
                    break;
            }
        }
    }
}
