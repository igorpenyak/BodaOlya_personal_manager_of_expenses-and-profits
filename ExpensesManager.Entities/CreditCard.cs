using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Entities
{
    public class CreditCard
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; }
        public DateTime DateOf { get; set; }
        public int UserId { get; set; }
    }
}
