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
        public string Name { get; set; }
        public string model { get;set; }
        public CharacterModel()
        {
            this.userid = -1;
            this.Name = "undefined";
            this.model = "undefined";
        }
    }
}
