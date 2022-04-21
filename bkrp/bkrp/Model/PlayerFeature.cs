using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public class PlayerFeature
    {
        public int Gender;
        public ParentData Parents;
        public ClothesData Clothes = new ClothesData();
        public AccessoryData Accessory = new AccessoryData();
        public float[] Features = new float[20];
        public AppearanceItem[] Appearance = new AppearanceItem[13];
        public HairData Hair;

        public int EyebrowColor;
        public int BeardColor;
        public int EyeColor;
        public int BlushColor;
        public int LipstickColor;
        public int MakeUp;
        public int ChestHairColor;

        public PedTattoos PedTatttoos;


        public PlayerFeature()
        {
            Gender = 0;
            Parents = new ParentData(0, 0, 1.0f, 1.0f);
            for (int i = 0; i < Features.Length; i++) Features[i] = 0f;
            for (int i = 0; i < Appearance.Length; i++) Appearance[i] = new AppearanceItem(255, 1.0f, 0);
            Hair = new HairData(0, 0, 0);
        }
    }

    public class ParentData
    {
        public int Father;
        public int Mother;
        public float Similarity;
        public float SkinSimilarity;

        public ParentData(int father, int mother, float sim, float skinsim)
        {
            this.Father = father;
            this.Mother = mother;
            this.Similarity = sim;
            this.SkinSimilarity = skinsim;
        }
    }

    public class ComponentItem
    {
        public int Variation;
        public int Texture;

        public ComponentItem(int variation, int texture)
        {
            Variation = variation;
            Texture = texture;
        }
    }

    public class ClothesData
    {
        public ComponentItem Mask { get; set; }
        public ComponentItem Gloves { get; set; }
        public ComponentItem Torso { get; set; }
        public ComponentItem Leg { get; set; }
        public ComponentItem Bag { get; set; }
        public ComponentItem Feet { get; set; }
        public ComponentItem Accessory { get; set; }
        public ComponentItem Undershit { get; set; }
        public ComponentItem Bodyarmor { get; set; }
        public ComponentItem Decals { get; set; }
        public ComponentItem Top { get; set; }

        public ClothesData()
        {
            Mask = new ComponentItem(0, 0);
            Gloves = new ComponentItem(0, 0);
            Torso = new ComponentItem(15, 0);
            Leg = new ComponentItem(21, 0);
            Bag = new ComponentItem(0, 0);
            Feet = new ComponentItem(34, 0);
            Accessory = new ComponentItem(0, 0);
            Undershit = new ComponentItem(15, 0);
            Bodyarmor = new ComponentItem(0, 0);
            Decals = new ComponentItem(0, 0);
            Top = new ComponentItem(15, 0);
        }
    }

    public class AccessoryData
    {
        public ComponentItem Hat { get; set; }
        public ComponentItem Glasses { get; set; }
        public ComponentItem Ear { get; set; }
        public ComponentItem Watches { get; set; }
        public ComponentItem Bracelets { get; set; }

        public AccessoryData()
        {
            Hat = new ComponentItem(-1, 0);
            Glasses = new ComponentItem(-1, 0);
            Ear = new ComponentItem(-1, 0);
            Watches = new ComponentItem(-1, 0);
            Bracelets = new ComponentItem(-1, 0);
        }
    }

    public class AppearanceItem
    {
        public int Value;
        public float Opacity;
        public int Color;

        public AppearanceItem(int value, float opacity, int color)
        {
            Value = value;
            Opacity = opacity;
            Color = color;
        }
    }

    public class HairData
    {
        public int Hair;
        public int Color;
        public int HighlightColor;

        public HairData(int hair, int color, int highlightcolor)
        {
            Hair = hair;
            Color = color;
            HighlightColor = highlightcolor;
        }
    }

    public struct PedTattoos
    {
        public List<KeyValuePair<string, string>> TorsoTattoos;
        public List<KeyValuePair<string, string>> HeadTattoos;
        public List<KeyValuePair<string, string>> LeftArmTattoos;
        public List<KeyValuePair<string, string>> RightArmTattoos;
        public List<KeyValuePair<string, string>> LeftLegTattoos;
        public List<KeyValuePair<string, string>> RightLegTattoos;
        public List<KeyValuePair<string, string>> BadgeTattoos;
    }
}
