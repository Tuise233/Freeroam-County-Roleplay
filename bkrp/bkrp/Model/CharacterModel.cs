using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public class CharacterModel
    {
        public int userid { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public int sex { get; set; }
        public int model { get;set; }
        public CharacterModel()
        {
            this.userid = -1;
            this.name = "undefined";
            this.age = 18;
            this.sex = 1;
            this.model = 0;
        }

        public CharacterModel(int userid, string name, int age, int sex, int model)
        {
            this.userid = userid;
            this.name = name;
            this.age = age;
            this.sex = sex;
            this.model = model;
        }
    }
}
