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

        public PlayerCharacter()
        {
            this.uid = -1;
            this.name = "";
            this.money = 0;
            this.bank = 0;
            this.age = 0;
            this.level = 0;
            this.exp = 0;
        }
    }
}
