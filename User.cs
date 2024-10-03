using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFogadas
{
    internal class User
    {
        public int BettorsID { get; set; }
        public string Username { get; set; }
        public int Balance { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }
        public string Privilage { get; set; }
    }
}
