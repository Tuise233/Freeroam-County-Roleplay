using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public class AccountModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public AccountModel()
        {
            this.id = -1;
            this.username = "undefined";
            this.password = "undefined";
            this.email = "undefined";
        }
    }
}
