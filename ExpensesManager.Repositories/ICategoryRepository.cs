using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManager.Repositories
{
    interface ICategoryRepository
    {
        IEnumerable<Category> SelectAll();

        int InsertNewCategory(Category category);
    }
}
