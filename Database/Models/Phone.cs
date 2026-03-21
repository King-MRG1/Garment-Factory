using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Database.Models
{
    public class Phone : IUserOwned
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int? Worker_Id { get; set; }
        public Worker? Worker { get; set; }
        public int? Trader_Id { get; set; }
        public Trader? Trader { get; set; }
        public string UserId { get; set; }
    }
}
