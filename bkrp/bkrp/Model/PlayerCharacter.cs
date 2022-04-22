using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public class PlayerCharacter
    {
        public int id { get; set; }
        public int uid { get; set; }
        public string name { get; set; }
        public int money { get; set; }
        public int bank { get; set; }
        public int age { get; set; }
        public int level { get; set; }
        public int exp { get; set; }
        public int thirst { get; set; }
        public int hunger { get; set; }

        public PlayerCharacter()
        {
            this.uid = -1;
            this.name = "";
            this.money = 5000;
            this.bank = 10000;
            this.age = 0;
            this.level = 1;
            this.exp = 0;
            this.thirst = 100;
            this.hunger = 100;
        }

        public PlayerCharacter(int uid, string name, int money, int bank, int age, int level, int exp, int thirst, int hunger)
        {
            this.uid = uid;
            this.name = name;
            this.money = money;
            this.bank = bank;
            this.age = age;
            this.level = level;
            this.exp = exp;
            this.thirst = thirst;
            this.hunger = hunger;
        }
    }
}
