using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFogadas
{
    internal class UserBets
    {
        public int BetID { get; set; }
        public DateTime BetDate { get; set; }
        public float Odds { get; set; }
        public int Amount { get; set; }
        public int BettorsID { get; set; }
        public int EventID { get; set; }
        public bool Status { get; set; }
    }
}
