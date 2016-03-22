using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.DesktopUI.Code
{
    public class CurrentUser
    {
        private static Users currentUser;

        public static void Initialize(Users user)
        {
            if (currentUser != null)
            {
                throw new InvalidOperationException("Current user has been already specified");
            }
            currentUser = user;
        }

        public static int Id
        {
            get
            {
                return currentUser.Id;
            }
        }

        public static string FirstName
        {
            get
            {
                return currentUser.FirstName;
            }
        }

        public static string LastName
        {
            get
            {
                return currentUser.LastName;
            }
        }

        public static string Login
        {
            get
            {
                return currentUser.Login;
            }
        }
    }
}
