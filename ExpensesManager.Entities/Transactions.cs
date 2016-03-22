using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Entities
{
    public class Transactions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int CreditCardId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
        public bool TransactionStatus { get; set; }
        public int RepeatableTypeId { get; set; }
        public DateTime LastRepeatDate { get; set; }
        public string CategoryName { get; set; }
        public string CreditCard { get; set; }
        public string RepeatableType { get; set; }
        public string TypeOfTransaction { get; set; }

    }
}
