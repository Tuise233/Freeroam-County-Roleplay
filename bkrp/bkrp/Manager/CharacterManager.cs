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
    class CharacterManager : IScript
    {
        public static List<string> hair_style_female = new List<string>();
        public static List<string> hair_style_male = new List<string>();
        public static List<string> eye_colors = new List<string>();
        public static List<string> dad = new List<string>();
        public static List<string> mom = new List<string>();
        public static List<string> shape_mix_list = new List<string>();

        [ClientEvent("RequestCreateCharacter")]
        public void RequestCreateCharacter(PlayerEx player)
        {
            Database.ExecuteSql($"SELECT * FROM `character` WHERE `userid`='{player.Account.id}'", (reader, err) =>
            {
                if(!err)
                {
                    int count = 0;
                    while(reader.Read())
                    {
                        count++;
                    }
                    if(count >= player.Account.max_character)
                    {
                        player.SendErrorNotification($"你最多可以创建{count}个角色");
                        return;
                    }
                    player.Emit("freeze:toggle", true);
                    player.Emit("character:destroy");
                    player.Emit("face:createCamera");
                }
            });
        }

        [ClientEvent("ClientCharCreationBack")]
        public void ClientCharCreationBack(PlayerEx player)
        {
            player.Emit("face:destroy");
            player.Emit("face:destroyCamera");
            AccountManager.SelectCharacter(player);
        }

        [ClientEvent("Display_Creator_part2")]
        public void DisplayPartTwo(PlayerEx player, string name, string forename)
        {
            player.SetMetaData("temp_name", name);
            player.SetMetaData("temp_second_name", forename);

            player.Emit("face:destroy");
            ShowPlayerCreatorTwo(player);
        }

        public static void ShowPlayerCreatorTwo(PlayerEx player)
        {
            List<dynamic> temp_array = new List<dynamic>();

            for (int i = 0; i <= 20; i++)
            {
                player.GetMetaData("temp_facefeature_" + i, out int value);
                temp_array.Add(new { FaceFeatures = value });
            }

            //player.TriggerEvent("Show_Char_Creator_2",temp_array.to);
            player.Emit("face:load2", temp_array.ToJson());
            UpdateVariables(player);
        }

        public static void StartCreateCharacter(PlayerEx player)
        {
            player.SetMetaData("Creator_PrevPos", player.Position);
            player.Dimension = 100 + player.Id;

            if(player.Feature != null)
            {
                SetCreatorClothes(player, player.Feature.Gender);
                SetDefaultFeature(player, 0, true);
            }
            else
            {
                player.Feature = new PlayerFeature();
                SetDefaultFeature(player, 0, true);
            }
            player.SetMetaData("creator_outfit", 0);
            player.Emit("face:setCamera", 0);
            ShowPlayerCreator(player, true);
        }

        public static void ShowPlayerCreator(PlayerEx player, bool reset_variable = false)
        {
            if (reset_variable == true)
            {
                player.SetMetaData("temp_base", 0);
                player.SetMetaData("temp_base2", 0);
                player.SetMetaData("temp_baseblend", 5);
                player.SetMetaData("temp_skin", 5);
                player.SetMetaData("temp_eyes", 0);
                player.SetMetaData("temp_hair", 0);
                player.SetMetaData("temp_haircolor", 0);
                player.SetMetaData("temp_hairhighlightcolor", 0);
                player.SetMetaData("temp_eyebrows", 0);
                player.SetMetaData("temp_beard", 0);

                player.SetMetaData("temp_top", 0);
                player.SetMetaData("temp_pants", 0);
                player.SetMetaData("temp_shoes", 0);

                player.SetMetaData("temp_sex", 0);
                player.SetMetaData("temp_traje", 0);

                player.SetMetaData("temp_name", "");
                player.SetMetaData("temp_second_name", "");

                for (int i = 0; i <= 20; i++)
                {
                    player.SetMetaData("temp_facefeature_" + i, 10);
                }

            }

            List<dynamic> temp_array = new List<dynamic>();

            player.GetMetaData("temp_name", out string forename);
            player.GetMetaData("temp_second_name", out string surname);
            player.GetMetaData("temp_sex", out int sex);
            player.GetMetaData("temp_base", out int base1);
            player.GetMetaData("temp_base2", out int base2);
            player.GetMetaData("temp_baseblend", out int baseblend);
            player.GetMetaData("temp_skin", out int skin);
            player.GetMetaData("temp_eyes", out int eyes);
            player.GetMetaData("temp_hair", out int hair);
            player.GetMetaData("temp_haircolor", out int haircolor);
            player.GetMetaData("temp_hairhighlightcolor", out int hcolor);
            player.GetMetaData("temp_eyebrows", out int eyebrows);
            player.GetMetaData("temp_beard", out int beard);

            temp_array.Add(new
            {
                Forename = forename,
                Surname = surname,
                Gender = sex,
                Base = base1,
                Base2 = base2,
                BaseBlend = baseblend,
                Skin = skin,
                Eyes = eyes,
                Hair = hair,
                HairColor = haircolor,
                HairHighlightColor = hcolor,
                Eyebrows = eyebrows,
                Beard = beard
            });

            player.Emit("face:load", temp_array.ToJson());
            UpdateVariables(player);
        }

        public static void UpdateVariables(PlayerEx player)
        {
            player.GetMetaData("temp_base", out int base1);
            player.GetMetaData("temp_base2", out int base2);
            player.GetMetaData("temp_baseblend", out int blend);
            player.GetMetaData("temp_skin", out int skin);
            player.GetMetaData("temp_eyes", out int eyes);
            player.GetMetaData("temp_hair", out int hair);
            player.GetMetaData("temp_haircolor", out int haircolor);
            player.GetMetaData("temp_hcolor", out int hcolor);
            player.GetMetaData("temp_eyebrows", out int eyebrows);
            player.GetMetaData("temp_beard", out int beard);

            OnClientOnRangeChange(player, "range_base", base1.ToString());
            OnClientOnRangeChange(player, "range_base2", base2.ToString());
            OnClientOnRangeChange(player, "range_baseblend", blend.ToString());
            OnClientOnRangeChange(player, "range_skin", skin.ToString());
            OnClientOnRangeChange(player, "range_eyes", eyes.ToString());
            OnClientOnRangeChange(player, "range_hair", hair.ToString());
            OnClientOnRangeChange(player, "range_haircolor", haircolor.ToString());
            OnClientOnRangeChange(player, "range_haircolor2", hcolor.ToString());
            OnClientOnRangeChange(player, "range_eyebrows", eyebrows.ToString());
            OnClientOnRangeChange(player, "range_beard", beard.ToString());

            for(int i = 0; i < 20; i++)
            {
                player.GetMetaData("temp_facefeature_" + i, out int value);
                OnClientSetFaceFeature(player, i, value);
            }

            player.GetMetaData("temp_traje", out int traje);
            OnClientSetTraje(player, traje);
        }

        public static void OnClientSetTraje(PlayerEx player, int valueIndex)
        {
            player.SetMetaData("creator_outfit", valueIndex);
            player.SetMetaData("temp_traje", valueIndex);
            switch (valueIndex)
            {
                case 0:
                    {
                        player.GetSyncedMetaData("CHARACTER_ONLINE_GENRE", out int gender);
                        if(gender == 0)
                        {
                            player.SetClothes(3, 15, 0, 0);
                            player.SetClothes(4, 21, 0, 0);
                            player.SetClothes(6, 34, 0, 0);
                            player.SetClothes(8, 15, 0, 0);
                            player.SetClothes(11, 15, 0, 0);
                        }
                        else
                        {
                            player.SetClothes(3, 15, 0, 0);
                            player.SetClothes(4, 10, 0, 0);
                            player.SetClothes(6, 35, 0, 0);
                            player.SetClothes(8, 15, 0, 0);
                            player.SetClothes(11, 15, 0, 0);
                        }
                        break;
                    }
                case 1:
                    Outfits.SetUnisexOutfit(player, 1, true);
                    break;
                case 2:
                    Outfits.SetUnisexOutfit(player, 2, true);
                    break;
                case 3:
                    Outfits.SetUnisexOutfit(player, 3, true);
                    break;
                case 4:
                    Outfits.SetUnisexOutfit(player, 4, true);
                    break;
                case 5:
                    Outfits.SetUnisexOutfit(player, 5, true);
                    break;
                case 6:
                    Outfits.SetUnisexOutfit(player, 6, true);
                    break;
                case 7:
                    Outfits.SetUnisexOutfit(player, 7, true);
                    break;
                case 8:
                    Outfits.SetUnisexOutfit(player, 8, true);
                    break;
                case 9:
                    Outfits.SetUnisexOutfit(player, 9, true);
                    break;
                case 10:
                    Outfits.SetUnisexOutfit(player, 10, true);
                    break;
                case 11:
                    Outfits.SetUnisexOutfit(player, 11, true);
                    break;
                case 12:
                    Outfits.SetUnisexOutfit(player, 12, true);
                    break;
            }
        }


        [ClientEvent("ClientSetFaceFeature")]
        public static void OnClientSetFaceFeature(PlayerEx player, int type, int valueIndex)
        {
            float new_value = 0.0f;
            switch (valueIndex)
            {
                case 0: new_value = -1.0f; break;
                case 1: new_value = -0.9f; break;
                case 2: new_value = -0.8f; break;
                case 3: new_value = -0.7f; break;
                case 4: new_value = -0.6f; break;
                case 5: new_value = -0.5f; break;
                case 6: new_value = -0.4f; break;
                case 7: new_value = -0.3f; break;
                case 8: new_value = -0.2f; break;
                case 9: new_value = -0.1f; break;
                case 10: new_value = 0.0f; break;
                case 11: new_value = -0.1f; break;
                case 12: new_value = -0.2f; break;
                case 13: new_value = -0.3f; break;
                case 14: new_value = -0.4f; break;
                case 15: new_value = -0.5f; break;
                case 16: new_value = -0.6f; break;
                case 17: new_value = -0.7f; break;
                case 18: new_value = -0.8f; break;
                case 19: new_value = -0.9f; break;
            }
            player.Feature.Features[type] = new_value;
            player.SetFaceFeature((byte)type, player.Feature.Features[type]);
            player.SetMetaData("temp_facefeature_" + type, valueIndex);
        }

        [ClientEvent("ClientOnRangeChange")]
        public static void OnClientOnRangeChange(PlayerEx player, string type, string valueIndex)
        {
            if (type == "range_gender")
            {
                int value = Convert.ToInt32(valueIndex);
                if (player.Feature == null) return;

                int sex = 0;
                if (value == 0)
                {
                    player.Model = 0x9C9EFFD8;
                    player.SetSyncedMetaData("CHARACTER_ONLINE_GENRE", 1);
                    sex = 1;
                }
                else if (value == 1)
                {
                    player.Model = 0x705E61F2;
                    player.SetSyncedMetaData("CHARACTER_ONLINE_GENRE", 0);
                    sex = 0;
                }


                player.SetMetaData("ChangedGender", true);
                SetDefaultFeature(player, sex, true);
                player.SetMetaData("temp_sex", value);

                UpdateVariables(player);
            }
            else if (type == "range_base")
            {
                int value = Convert.ToInt32(valueIndex);
                player.Feature.Parents.Mother = value + 20;
                ApplyCharacterPreview(player);
                player.SetMetaData("temp_base", value);
            }
            else if (type == "range_base2")
            {
                int value = Convert.ToInt32(valueIndex);
                player.Feature.Parents.Father = value;
                ApplyCharacterPreview(player);
                player.SetMetaData("temp_base2", value);
            }
            else if (type == "range_baseblend")
            {
                int value = Convert.ToInt32(valueIndex);
                float new_value = 0.0f;
                if (value > 9) return;
                switch (value)
                {
                    case 0: new_value = 0.0f; break;
                    case 1: new_value = 0.1f; break;
                    case 2: new_value = 0.2f; break;
                    case 3: new_value = 0.3f; break;
                    case 4: new_value = 0.4f; break;
                    case 5: new_value = 0.5f; break;
                    case 6: new_value = 0.6f; break;
                    case 7: new_value = 0.7f; break;
                    case 8: new_value = 0.8f; break;
                    case 9: new_value = 0.9f; break;
                }
                player.Feature.Parents.Similarity = new_value;
                ApplyCharacterPreview(player);
                player.SetMetaData("temp_baseblend", value);
            }
            else if (type == "range_skin")
            {
                int value = Convert.ToInt32(valueIndex);
                if (value > 9) return;
                float new_value = 0.0f;
                switch (value)
                {
                    case 0: new_value = 0.0f; break;
                    case 1: new_value = 0.1f; break;
                    case 2: new_value = 0.2f; break;
                    case 3: new_value = 0.3f; break;
                    case 4: new_value = 0.4f; break;
                    case 5: new_value = 0.5f; break;
                    case 6: new_value = 0.6f; break;
                    case 7: new_value = 0.7f; break;
                    case 8: new_value = 0.8f; break;
                    case 9: new_value = 0.9f; break;
                }
                player.Feature.Parents.SkinSimilarity = new_value;
                ApplyCharacterPreview(player);
                player.SetMetaData("temp_skin", value);
            }
            else if (type == "range_eyes")
            {
                int value = Convert.ToInt32(valueIndex);
                player.Feature.EyeColor = (byte)value;
                player.SetEyeColor((ushort)player.Feature.EyeColor);
                //NAPI.Player.SetPlayerEyeColor(player, (byte)player.Feature.EyeColor);
                player.SetMetaData("temp_eyes", value);
            }
            else if (type == "range_hair")
            {
                int value = Convert.ToInt32(valueIndex);
                if (value > 72) return;
                player.GetSyncedMetaData("CHARACTER_ONLINE_GENRE", out int gender);
                if(gender == 0)
                {
                    player.Feature.Hair.Hair = Convert.ToInt32(hair_style_male[value]);
                    player.SetClothes(2, (ushort)player.Feature.Hair.Hair, 0, 0);
                }
                else
                {
                    player.Feature.Hair.Hair = Convert.ToInt32(hair_style_female[value]);
                    player.SetClothes(2, (ushort)player.Feature.Hair.Hair, 0, 0);
                }
                player.SetMetaData("temp_hair", value);
            }
            else if (type == "range_haircolor")
            {
                int value = Convert.ToInt32(valueIndex);
                if (value > 30) return;
                player.Feature.Hair.Color = value;
                player.HairColor = (byte)player.Feature.Hair.Color;
                player.HairHighlightColor = (byte)player.Feature.Hair.HighlightColor;
                player.SetMetaData("temp_haircolor", value);
            }
            else if (type == "range_haircolor2")
            {
                int value = Convert.ToInt32(valueIndex);
                player.Feature.Hair.HighlightColor = value;
                player.HairHighlightColor = (byte)player.Feature.Hair.HighlightColor;
                player.SetMetaData("temp_hairhighlightcolor", value);
            }
            else if (type == "range_beard")
            {
                /*
                int value = Convert.ToInt32(valueIndex);
                HeadOverlay headoverlay2 = new HeadOverlay();
                int index = 1;
                if (value == 0) player.Feature.Appearance[index].Value = 255;
                else player.Feature.Appearance[index].Value = value - 1;
                headoverlay2.Index = (byte)player.Feature.Appearance[index].Value;
                headoverlay2.Color = (byte)player.Feature.Appearance[index].Color;
                headoverlay2.Opacity = player.Feature.Appearance[index].Opacity;
                NAPI.Player.SetPlayerHeadOverlay(player, index, headoverlay2);
                */
                int value = Convert.ToInt32(valueIndex);
                int index = 1;
                if (value == 0) player.Feature.Appearance[index].Value = 255;
                else player.Feature.Appearance[index].Value = value - 1;
                player.SetHeadOverlay((byte)index, (byte)player.Feature.Appearance[index].Value, player.Feature.Appearance[index].Opacity);
                player.SetMetaData("temp_beard", value);
            }
            else if (type == "range_eyebrows")
            {
                /*
                int value = Convert.ToInt32(valueIndex);
                HeadOverlay headoverlay2 = new HeadOverlay();
                int index = 2;
                if (value == 0) player.Feature.Appearance[index].Value = 255;
                else player.Feature.Appearance[index].Value = value - 1;
                headoverlay2.Index = (byte)player.Feature.Appearance[index].Value;
                headoverlay2.Color = (byte)player.Feature.Appearance[index].Color;
                headoverlay2.Opacity = player.Feature.Appearance[index].Opacity;
                NAPI.Player.SetPlayerHeadOverlay(player, index, headoverlay2);
                */
                int value = Convert.ToInt32(valueIndex);
                int index = 2;
                if (value == 0) player.Feature.Appearance[index].Value = 255;
                else player.Feature.Appearance[index].Value = value - 1;
                player.SetHeadOverlay((byte)index, (byte)player.Feature.Appearance[index].Value, player.Feature.Appearance[index].Opacity);
                player.SetMetaData("temp_eyebrows", value);
            }
            else if (type == "range_rotation")
            {
                int value = Convert.ToInt32(valueIndex);
                player.Rotation = new Position(0, 0, float.Parse(valueIndex) / 59);
            }
            else if (type == "range_elevation")
            {

            }
        }

        public static void SetCreatorClothes(PlayerEx player, int gender)
        {
            if (player.Feature == null) return;

            for(int i = 0; i < 10; i++)
            {
                player.ClearProps((byte)i);
            }

            if(gender == 0)
            {
                player.SetClothes(3, 15, 0, 0);
                player.SetClothes(4, 21, 0, 0);
                player.SetClothes(6, 34, 0, 0);
                player.SetClothes(8, 15, 0, 0);
                player.SetClothes(11, 15, 0, 0);
            }
            else
            {
                player.SetClothes(3, 15, 0, 0);
                player.SetClothes(4, 10, 0, 0);
                player.SetClothes(6, 35, 0, 0);
                player.SetClothes(8, 15, 0, 0);
                player.SetClothes(11, 15, 0, 0);
            }

            player.SetClothes(2, (ushort)player.Feature.Hair.Hair, 0, 0);
        }

        public static void SetDefaultFeature(PlayerEx player, int gender, bool reset = false)
        {
            if(reset)
            {
                player.Feature = new PlayerFeature();
                player.Feature.Gender = gender;

                player.Feature.Parents.Father = 0;
                player.Feature.Parents.Mother = 21;
                player.Feature.Parents.Similarity = gender == 0 ? 1.0f : 0.0f;
                player.Feature.Parents.SkinSimilarity = gender == 0 ? 1.0f : 0.0f;

                player.Feature.Hair.Hair = 0;
                player.Feature.Hair.Color = 0;
                player.Feature.Hair.HighlightColor = 0;

                player.Feature.EyebrowColor = 0;
                player.Feature.BeardColor = 0;
                player.Feature.EyeColor = 0;
                player.Feature.BlushColor = 0;
                player.Feature.LipstickColor = 0;
                player.Feature.MakeUp = 0;
                player.Feature.ChestHairColor = 0;

                for (int i = 0; i < player.Feature.Appearance.Length; i++)
                {
                    player.SetHeadOverlay((byte)i, 255, 1.0f);
                    player.SetHeadOverlayColor((byte)i, 255, 0, 0);
                }
            }
            ApplyCharacter(player);
            SetCreatorClothes(player, gender);
        }

        public static void ApplyCharacter(PlayerEx player)
        {
            if (player.Feature == null) return;
            int gender = player.Feature.Gender;
            player.SetSyncedMetaData("CHARACTER_ONLINE_GENRE", gender);
            player.SetSyncedMetaData("GENDER", gender);

            if(gender == 0)
            {
                player.SetMetaData("IsMale", true);
            }
            else
            {
                player.SetMetaData("IsMale", false);
            }

            player.SetHeadBlendData((uint)player.Feature.Parents.Mother, (uint)player.Feature.Parents.Father, 0, (uint)player.Feature.Parents.Mother, (uint)player.Feature.Parents.Father,
                0, player.Feature.Parents.Similarity, player.Feature.Parents.SkinSimilarity, 0);

            player.Model = player.Feature.Gender == 0 ? 0x705E61F2 : 0x9C9EFFD8;

            player.SetClothes(2, (ushort)player.Feature.Hair.Hair, 0, 0);
            player.HairColor = (byte)player.Feature.Hair.Color;
            player.HairHighlightColor = (byte)player.Feature.Hair.HighlightColor;

            player.SetEyeColor((ushort)player.Feature.EyeColor);

            for(int i = 0; i < player.Feature.Features.Length; i++)
            {
                player.SetFaceFeature((byte)i, player.Feature.Features[i]);
            }

            for(int i = 0; i < player.Feature.Appearance.Length; i++)
            {
                player.SetHeadOverlay((byte)i, (byte)player.Feature.Appearance[i].Value, (byte)player.Feature.Appearance[i].Opacity);
                player.SetHeadOverlayColor((byte)i, (byte)player.Feature.Appearance[i].Value, (byte)player.Feature.Appearance[i].Color, (byte)player.Feature.Appearance[i].Color);
            }

            player.SetSyncedMetaData("CustomCharacter", player.Feature.ToJson());
        }

        public static void ApplyCharacterPreview(PlayerEx player)
        {
            if (player.Feature == null) return;


            player.SetSyncedMetaData("CHARACTER_ONLINE_GENRE", player.Feature.Gender);

            player.SetHeadBlendData((uint)player.Feature.Parents.Mother, (uint)player.Feature.Parents.Father, 0, (uint)player.Feature.Parents.Mother, (uint)player.Feature.Parents.Father,
                0, player.Feature.Parents.Similarity, player.Feature.Parents.SkinSimilarity, 0);

            player.SetClothes(2, (ushort)player.Feature.Hair.Hair, 0, 0);
            player.HairColor = (byte)player.Feature.Hair.Color;
            player.HairHighlightColor = (byte)player.Feature.Hair.HighlightColor;

            player.SetEyeColor((ushort)player.Feature.EyeColor);

            for (int i = 0; i < player.Feature.Features.Length; i++)
            {
                player.SetFaceFeature((byte)i, player.Feature.Features[i]);
            }

            for (int i = 0; i < player.Feature.Appearance.Length; i++)
            {
                player.SetHeadOverlay((byte)i, (byte)player.Feature.Appearance[i].Value, (byte)player.Feature.Appearance[i].Opacity);
                player.SetHeadOverlayColor((byte)i, (byte)player.Feature.Appearance[i].Value, (byte)player.Feature.Appearance[i].Color, (byte)player.Feature.Appearance[i].Color);
            }

            //TattoBusiness.ApplySavedTattoos(player);
            player.SetSyncedMetaData("CustomCharacter", player.Feature.ToJson());
        }
    }

    class FaceDataSet
    {
        public FaceDataSet()
        {
            CharacterManager.dad.Add("Benjamin");
            CharacterManager.dad.Add("Daniel");
            CharacterManager.dad.Add("Joshua");
            CharacterManager.dad.Add("Noah");
            CharacterManager.dad.Add("Andrew");
            CharacterManager.dad.Add("Juan");
            CharacterManager.dad.Add("Alex");
            CharacterManager.dad.Add("Isaac");
            CharacterManager.dad.Add("Evan");
            CharacterManager.dad.Add("Ethan");
            CharacterManager.dad.Add("Vincent");
            CharacterManager.dad.Add("Angel");
            CharacterManager.dad.Add("Diego");
            CharacterManager.dad.Add("Adrian");
            CharacterManager.dad.Add("Gabriel");
            CharacterManager.dad.Add("Michael");
            CharacterManager.dad.Add("Santiago");
            CharacterManager.dad.Add("Kevin");
            CharacterManager.dad.Add("Louis");
            CharacterManager.dad.Add("Samuel");
            CharacterManager.dad.Add("Anthony");
            CharacterManager.dad.Add("Claude");
            CharacterManager.dad.Add("Niko");
            CharacterManager.dad.Add("John");

            CharacterManager.mom.Add("Hannah");
            CharacterManager.mom.Add("Aubrey");
            CharacterManager.mom.Add("Jasmine");
            CharacterManager.mom.Add("Gisele");
            CharacterManager.mom.Add("Amelia");
            CharacterManager.mom.Add("Isabella");
            CharacterManager.mom.Add("Zoe");
            CharacterManager.mom.Add("Ava");
            CharacterManager.mom.Add("Camila");
            CharacterManager.mom.Add("Violet");
            CharacterManager.mom.Add("Sophia");
            CharacterManager.mom.Add("Evelyn");
            CharacterManager.mom.Add("Nicole");
            CharacterManager.mom.Add("Ashley");
            CharacterManager.mom.Add("Gracie");
            CharacterManager.mom.Add("Brianna");
            CharacterManager.mom.Add("Natalie");
            CharacterManager.mom.Add("Olivia");
            CharacterManager.mom.Add("Elizabeth");
            CharacterManager.mom.Add("Charlotte");
            CharacterManager.mom.Add("Emma");
            CharacterManager.mom.Add("Misty");

            CharacterManager.shape_mix_list.Add("Mãe - ~b~I~w~IIIIIIII ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~II~w~IIIIIII ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~III~w~IIIIII ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~IIII~w~IIIII ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~IIIII~w~IIII ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~IIIIII~w~III ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~IIIIIII~w~II ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~IIIIIIII~w~I ~w~- ~y~Pai");
            CharacterManager.shape_mix_list.Add("Mãe - ~b~IIIIIIIII ~w~- ~y~Pai");

            CharacterManager.hair_style_male.Add("0");
            CharacterManager.hair_style_male.Add("1");
            CharacterManager.hair_style_male.Add("2");
            CharacterManager.hair_style_male.Add("3");
            CharacterManager.hair_style_male.Add("4");
            CharacterManager.hair_style_male.Add("5");
            CharacterManager.hair_style_male.Add("6");
            CharacterManager.hair_style_male.Add("7");
            CharacterManager.hair_style_male.Add("8");
            CharacterManager.hair_style_male.Add("9");
            CharacterManager.hair_style_male.Add("10");
            CharacterManager.hair_style_male.Add("11");
            CharacterManager.hair_style_male.Add("12");
            CharacterManager.hair_style_male.Add("13");
            CharacterManager.hair_style_male.Add("14");
            CharacterManager.hair_style_male.Add("15");
            CharacterManager.hair_style_male.Add("16");
            CharacterManager.hair_style_male.Add("17");
            CharacterManager.hair_style_male.Add("18");
            CharacterManager.hair_style_male.Add("19");
            CharacterManager.hair_style_male.Add("20");
            CharacterManager.hair_style_male.Add("21");
            CharacterManager.hair_style_male.Add("22");
            CharacterManager.hair_style_male.Add("24");
            CharacterManager.hair_style_male.Add("25");
            CharacterManager.hair_style_male.Add("26");
            CharacterManager.hair_style_male.Add("27");
            CharacterManager.hair_style_male.Add("28");
            CharacterManager.hair_style_male.Add("29");
            CharacterManager.hair_style_male.Add("30");
            CharacterManager.hair_style_male.Add("31");
            CharacterManager.hair_style_male.Add("32");
            CharacterManager.hair_style_male.Add("33");
            CharacterManager.hair_style_male.Add("34");
            CharacterManager.hair_style_male.Add("35");
            CharacterManager.hair_style_male.Add("36");
            CharacterManager.hair_style_male.Add("37");
            CharacterManager.hair_style_male.Add("38");
            CharacterManager.hair_style_male.Add("39");
            CharacterManager.hair_style_male.Add("40");
            CharacterManager.hair_style_male.Add("41");
            CharacterManager.hair_style_male.Add("42");
            CharacterManager.hair_style_male.Add("43");
            CharacterManager.hair_style_male.Add("44");
            CharacterManager.hair_style_male.Add("45");
            CharacterManager.hair_style_male.Add("46");
            CharacterManager.hair_style_male.Add("47");
            CharacterManager.hair_style_male.Add("48");
            CharacterManager.hair_style_male.Add("49");
            CharacterManager.hair_style_male.Add("50");
            CharacterManager.hair_style_male.Add("51");
            CharacterManager.hair_style_male.Add("52");
            CharacterManager.hair_style_male.Add("53");
            CharacterManager.hair_style_male.Add("54");
            CharacterManager.hair_style_male.Add("55");
            CharacterManager.hair_style_male.Add("56");
            CharacterManager.hair_style_male.Add("57");
            CharacterManager.hair_style_male.Add("58");
            CharacterManager.hair_style_male.Add("59");
            CharacterManager.hair_style_male.Add("60");
            CharacterManager.hair_style_male.Add("61");
            CharacterManager.hair_style_male.Add("62");
            CharacterManager.hair_style_male.Add("63");
            CharacterManager.hair_style_male.Add("64");
            CharacterManager.hair_style_male.Add("65");
            CharacterManager.hair_style_male.Add("66");
            CharacterManager.hair_style_male.Add("67");
            CharacterManager.hair_style_male.Add("68");
            CharacterManager.hair_style_male.Add("69");
            CharacterManager.hair_style_male.Add("70");
            CharacterManager.hair_style_male.Add("71");
            CharacterManager.hair_style_male.Add("72");
            CharacterManager.hair_style_male.Add("73");

            CharacterManager.hair_style_female.Add("0");
            CharacterManager.hair_style_female.Add("1");
            CharacterManager.hair_style_female.Add("2");
            CharacterManager.hair_style_female.Add("3");
            CharacterManager.hair_style_female.Add("4");
            CharacterManager.hair_style_female.Add("5");
            CharacterManager.hair_style_female.Add("6");
            CharacterManager.hair_style_female.Add("7");
            CharacterManager.hair_style_female.Add("8");
            CharacterManager.hair_style_female.Add("9");
            CharacterManager.hair_style_female.Add("10");
            CharacterManager.hair_style_female.Add("11");
            CharacterManager.hair_style_female.Add("12");
            CharacterManager.hair_style_female.Add("13");
            CharacterManager.hair_style_female.Add("14");
            CharacterManager.hair_style_female.Add("15");
            CharacterManager.hair_style_female.Add("16");
            CharacterManager.hair_style_female.Add("17");
            CharacterManager.hair_style_female.Add("18");
            CharacterManager.hair_style_female.Add("19");
            CharacterManager.hair_style_female.Add("20");
            CharacterManager.hair_style_female.Add("21");
            CharacterManager.hair_style_female.Add("22");
            CharacterManager.hair_style_female.Add("23");
            CharacterManager.hair_style_female.Add("25");
            CharacterManager.hair_style_female.Add("26");
            CharacterManager.hair_style_female.Add("27");
            CharacterManager.hair_style_female.Add("28");
            CharacterManager.hair_style_female.Add("29");
            CharacterManager.hair_style_female.Add("30");
            CharacterManager.hair_style_female.Add("31");
            CharacterManager.hair_style_female.Add("32");
            CharacterManager.hair_style_female.Add("33");
            CharacterManager.hair_style_female.Add("34");
            CharacterManager.hair_style_female.Add("35");
            CharacterManager.hair_style_female.Add("36");
            CharacterManager.hair_style_female.Add("37");
            CharacterManager.hair_style_female.Add("38");
            CharacterManager.hair_style_female.Add("39");
            CharacterManager.hair_style_female.Add("40");
            CharacterManager.hair_style_female.Add("41");
            CharacterManager.hair_style_female.Add("42");
            CharacterManager.hair_style_female.Add("43");
            CharacterManager.hair_style_female.Add("44");
            CharacterManager.hair_style_female.Add("45");
            CharacterManager.hair_style_female.Add("46");
            CharacterManager.hair_style_female.Add("47");
            CharacterManager.hair_style_female.Add("48");
            CharacterManager.hair_style_female.Add("49");
            CharacterManager.hair_style_female.Add("50");
            CharacterManager.hair_style_female.Add("51");
            CharacterManager.hair_style_female.Add("52");
            CharacterManager.hair_style_female.Add("53");
            CharacterManager.hair_style_female.Add("54");
            CharacterManager.hair_style_female.Add("55");
            CharacterManager.hair_style_female.Add("56");
            CharacterManager.hair_style_female.Add("57");
            CharacterManager.hair_style_female.Add("58");
            CharacterManager.hair_style_female.Add("59");
            CharacterManager.hair_style_female.Add("60");
            CharacterManager.hair_style_female.Add("61");
            CharacterManager.hair_style_female.Add("62");
            CharacterManager.hair_style_female.Add("63");
            CharacterManager.hair_style_female.Add("64");
            CharacterManager.hair_style_female.Add("65");
            CharacterManager.hair_style_female.Add("66");
            CharacterManager.hair_style_female.Add("67");
            CharacterManager.hair_style_female.Add("68");
            CharacterManager.hair_style_female.Add("69");
            CharacterManager.hair_style_female.Add("70");
            CharacterManager.hair_style_female.Add("71");
            CharacterManager.hair_style_female.Add("72");
            CharacterManager.hair_style_female.Add("73");
            CharacterManager.hair_style_female.Add("74");
            CharacterManager.hair_style_female.Add("75");
            CharacterManager.hair_style_female.Add("76");
            CharacterManager.hair_style_female.Add("77");

            CharacterManager.eye_colors.Add("Green");
            CharacterManager.eye_colors.Add("Emerald");
            CharacterManager.eye_colors.Add("Light Blue");
            CharacterManager.eye_colors.Add("Ocean Blue");
            CharacterManager.eye_colors.Add("Light Brown");
            CharacterManager.eye_colors.Add("Dark Brown");
            CharacterManager.eye_colors.Add("Hazel");
            CharacterManager.eye_colors.Add("Dark Gray");
            CharacterManager.eye_colors.Add("Light Gray");
            CharacterManager.eye_colors.Add("Pink");
            CharacterManager.eye_colors.Add("Yellow");
            CharacterManager.eye_colors.Add("Blackout");
            CharacterManager.eye_colors.Add("Shades of Gray");
            CharacterManager.eye_colors.Add("Tequila Sunrise");
            CharacterManager.eye_colors.Add("Atomic");
            CharacterManager.eye_colors.Add("Warp");
            CharacterManager.eye_colors.Add("ECola");
            CharacterManager.eye_colors.Add("Space Ranger");
            CharacterManager.eye_colors.Add("Ying Yang");
            CharacterManager.eye_colors.Add("Bullseye");
            CharacterManager.eye_colors.Add("Lizard");
            CharacterManager.eye_colors.Add("Dragon");
            CharacterManager.eye_colors.Add("Extra Terrestrial");
            CharacterManager.eye_colors.Add("Goat");
            CharacterManager.eye_colors.Add("Smiley");
            CharacterManager.eye_colors.Add("Possessed");
            CharacterManager.eye_colors.Add("Demon");
            CharacterManager.eye_colors.Add("Infected");
            CharacterManager.eye_colors.Add("Alien");
            CharacterManager.eye_colors.Add("Undead");
            CharacterManager.eye_colors.Add("Zombie");
        }
    }
}
