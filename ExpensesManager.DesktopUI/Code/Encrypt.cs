using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.DesktopUI.Code
{
    public static class Encrypt
    {
        public static string GetHash(string text)
        {
            using (MD5 cryptoProvider = new MD5CryptoServiceProvider())
            {
                byte[] result = cryptoProvider.ComputeHash(Encoding.Default.GetBytes(text));

                StringBuilder stringHash = new StringBuilder();

                for (int i = 0; i < result.Length; i++)
                {
                    stringHash.Append(result[i].ToString("x2"));
                }

                return stringHash.ToString();
            }
        }
    }
}
