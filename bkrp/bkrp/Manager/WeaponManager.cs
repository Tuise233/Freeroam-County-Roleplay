using AltV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class WeaponManager : IScript
    {
        public static List<WeaponItem> WeaponList = new List<WeaponItem>();
        public WeaponManager()
        {
            WeaponList = JsonReader.ReadList<WeaponItem>(DataReader.GetText("weapons.json"));
            Log.Server("[WeaponManager] 读取武器数据成功");
        }
    }

    class WeaponItem
    {
        public string name { get; set; }
        public int id { get; set; }
        public float weight { get; set; }
        public string data { get; set; }
    }
}
