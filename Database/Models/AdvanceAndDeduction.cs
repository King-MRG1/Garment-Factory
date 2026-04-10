using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class AdvanceAndDeduction : IUserOwned
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public AdvanceOrDeduction Type { get; set; }
        public bool IsUsed { get; set; }
        public DateOnly Date { get; set; }
        public int Worker_Id { get; set; }
        public Worker Worker { get; set; }
        public string UserId { get; set; }
    }
    public enum AdvanceOrDeduction
    {
        Advance,
        Deduction
    }
}
