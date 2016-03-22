using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.DesktopUI.Code
{
    public static class CreditCardToStrinConverter
    {
        public static string Convert(string type, long number)
        {
            return string.Format("{0}  ({1})", type, number);
        }

        public static long GetNumber(string creditCard)
        {

            string number = string.Join("", creditCard.ToCharArray().Where(Char.IsDigit));
            return long.Parse(number);
        }
    }
}
