using ExpensesManager.DesktopUI.Code;
using ExpensesManager.Entities;
using ExpensesManager.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpensesManager.DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string _connectionString = ConfigurationManager.ConnectionStrings["ExpensesManager connection string"].ConnectionString;
        IList<Transactions> _expenseList;
        IList<Transactions> _incomeList;
        IList<Transactions> _planingTransactionList;


        public MainWindow()
        {
            InitializeComponent();

            var loginWindow = new Login();
            loginWindow.ShowDialog();

            if (!(loginWindow.DialogResult.HasValue && loginWindow.DialogResult.Value))
            {

                this.Close();
                return;
            }

            #region Initialize Data on View

            var category = new SqlCategoryRepository(_connectionString).SelectAll();
            var allCategory = category.Select(x => x.Name);

            foreach (var c in allCategory)
            {
                cbCategory.Items.Add(c);
                cbNewExpenseCategory.Items.Add(c);
                cbNewIncomeCategory.Items.Add(c);
                cbNewPlaningTranCategory.Items.Add(c);
            }

            var creditCards = new SqlCreditCardRepository(_connectionString).SelectAll(CurrentUser.Id);

            foreach (var c in creditCards)
            {
                cbNewExpenseCraditCard.Items.Add(CreditCardToStrinConverter.Convert(c.Type, c.Number));
                cbNewIncomeCraditCard.Items.Add(CreditCardToStrinConverter.Convert(c.Type, c.Number));
                cbNewPlaningTranCard.Items.Add(CreditCardToStrinConverter.Convert(c.Type, c.Number));
                cbCreditCard.Items.Add(CreditCardToStrinConverter.Convert(c.Type, c.Number));
            }

            _expenseList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Expense", CurrentUser.Id).ToList();
            _incomeList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Income", CurrentUser.Id).ToList();
            _planingTransactionList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Planing Expense", CurrentUser.Id).ToList();
            DataGridInformationGetter.InitializeDataGrid(dgExpensedGrid, _expenseList);
            DataGridInformationGetter.InitializeDataGrid(dgIncomesGrid, _incomeList);
            DataGridInformationGetter.InitializeDataGrid(dgPlaningTranGrid, _planingTransactionList);


            var repeatableType = new SqlRepeatableExpensesTypeRepository(_connectionString).SelectAll().Select(x => x.Name);

            cbNewExpenseRecurency.ItemsSource = repeatableType;
            cbNewIncomeRecurency.ItemsSource = repeatableType;
            cbNewPlaningTranRecurency.ItemsSource = repeatableType;

            #endregion
        }

        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            var newCategoryName = NewCategory.Text;

            Category category = new Category() { Name = newCategoryName };
            var categoryRepository = new SqlCategoryRepository(_connectionString);
            categoryRepository.InsertNewCategory(category);
            cbCategory.Items.Add(newCategoryName);
            cbNewPlaningTranCategory.Items.Add(newCategoryName);
            cbNewIncomeCategory.Items.Add(newCategoryName);
            cbNewExpenseCategory.Items.Add(newCategoryName);
            MessageBox.Show("Category was successfully added");

        }


        private void cbCategory_DropDownClosed(object sender, System.EventArgs e)
        {

            var category = cbCategory.SelectedItem.ToString();

            if (tcTransactions.SelectedItem == tbiExpenses)
            {
                var expenses = new SqlTransactionRepository(_connectionString).SortTransactionsByCategory(category, "Expense");
                dgExpensedGrid.ItemsSource = expenses;
                dgExpensedGrid.Items.Refresh();
            }

            if (tcTransactions.SelectedItem == tbiIncomes)
            {
                var incomes = new SqlTransactionRepository(_connectionString).SortTransactionsByCategory(category, "Income");
                dgIncomesGrid.ItemsSource = incomes;
                dgIncomesGrid.Items.Refresh();
            }

            if (tcTransactions.SelectedItem == tbiPlaningExpenses)
            {
                var planingExpenses = new SqlTransactionRepository(_connectionString).SortTransactionsByCategory(category, "Planing Expense");
                dgPlaningTranGrid.ItemsSource = planingExpenses;
                dgPlaningTranGrid.Items.Refresh();
            }


        }

        private void btnAddNewExpense_Click(object sender, RoutedEventArgs e)
        {
            decimal cost;
            decimal.TryParse(tbNewExpenseCost.Text.Replace('.', ','), out cost);
            var expense = new Transactions();
            if (cost != 0)
            {
                expense = new Transactions()
                {
                    Name = tbNewExpenseName.Text,
                    Cost = cost,
                    CreationDate = DateTime.Parse(dpNewExpenseCreation.Text),
                    RepeatableType = cbNewExpenseRecurency.Text,
                    TypeOfTransaction = "Expense",
                    CreditCard = cbNewExpenseCraditCard.Text,
                    UserId = CurrentUser.Id,
                    CategoryName = cbNewExpenseCategory.Text
                };
            }
            var newTransaction = new SqlTransactionRepository(_connectionString).InsertNewTransaction(expense);
            _expenseList.Add(expense);
            DataGridInformationGetter.InitializeDataGrid(dgExpensedGrid, _expenseList);


        }

        private void btnAddNewIncome_Click(object sender, RoutedEventArgs e)
        {

            decimal incomeCost;
            decimal.TryParse(tbNewIncomeCost.Text.Replace('.', ','), out incomeCost);
            var incomes = new Transactions();
            if (incomeCost != 0)
            {
                incomes = new Transactions()
                {

                    Name = tbNewIncomeName.Text,
                    Cost = incomeCost,
                    CreationDate = DateTime.Parse(dpNewIncomeCreation.Text),
                    RepeatableType = cbNewIncomeRecurency.Text,
                    TypeOfTransaction = "Income",
                    CreditCard = cbNewIncomeCraditCard.Text,
                    UserId = CurrentUser.Id,
                    CategoryName = cbNewIncomeCategory.Text

                };
            }
            var newTransaction = new SqlTransactionRepository(_connectionString).InsertNewTransaction(incomes);
            _incomeList.Add(incomes);

            DataGridInformationGetter.InitializeDataGrid(dgIncomesGrid, _incomeList);


        }

        private void btnAddNewCReditCard_Click(object sender, RoutedEventArgs e)
        {
            CreditCard creditCard = new CreditCard();
            try
            {
               
                decimal balance=decimal.Parse(tbNewCraditCardBalance.Text);
                long number= long.Parse(tbNewCraditCardNumber.Text);
                creditCard.Number = number;
                creditCard.Type = tbNewCraditCard.Text;
                creditCard.Balance = balance;
                creditCard.UserId = CurrentUser.Id;
                creditCard.DateOf = DateTime.Parse(dpNewCraditCardDate.Text);
                creditCard.Id = new SqlCreditCardRepository(_connectionString).InsertNewCreditCard(creditCard);
            }
            catch
            {
                throw new ArgumentException("Enter correct data");
            }

            if (creditCard.Id == -1)
            {
                MessageBox.Show("This card already exists.");
            }
            else
            {
                cbNewExpenseCraditCard.Items.Add(CreditCardToStrinConverter.Convert(creditCard.Type, creditCard.Number));
                cbNewIncomeCraditCard.Items.Add(CreditCardToStrinConverter.Convert(creditCard.Type, creditCard.Number));
                cbNewPlaningTranCard.Items.Add(CreditCardToStrinConverter.Convert(creditCard.Type, creditCard.Number));
                cbCreditCard.Items.Add(CreditCardToStrinConverter.Convert(creditCard.Type, creditCard.Number));
            }
        }


        private void btnSortByDate_Click(object sender, RoutedEventArgs e)
        {
            var card = cbCreditCard.Text;

            var dateFrom = DateTime.Parse(dpDateFrom.Text);
            var dateTo = DateTime.Parse(dpDateTo.Text);
            var number = CreditCardToStrinConverter.GetNumber(card);

            var expenses = new SqlTransactionRepository(_connectionString).SelectTransactionsByDateCard(number, dateFrom, dateTo, "Expense");
            var planingExpense = new SqlTransactionRepository(_connectionString).SelectTransactionsByDateCard(number, dateFrom, dateTo, "Planing Expense");
            var incomes = new SqlTransactionRepository(_connectionString).SelectTransactionsByDateCard(number, dateFrom, dateTo, "Income");

            DataGridInformationGetter.InitializeDataGrid(dgExpensedGrid, expenses.ToList());
            DataGridInformationGetter.InitializeDataGrid(dgIncomesGrid, incomes.ToList());
            DataGridInformationGetter.InitializeDataGrid(dgPlaningTranGrid, planingExpense.ToList());




        }

        private void dpDateSort_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DateTime date = DateTime.Parse(dpDateSort.Text);

            if (tcTransactions.SelectedItem == tbiExpenses)
            {
                var expenses = new SqlTransactionRepository(_connectionString).SortTransactionsByDate(date, "Expense");

                DataGridInformationGetter.InitializeDataGrid(dgExpensedGrid, expenses.ToList());

            }

            if (tcTransactions.SelectedItem == tbiIncomes)
            {
                var incomes = new SqlTransactionRepository(_connectionString).SortTransactionsByDate(date, "Income");
                DataGridInformationGetter.InitializeDataGrid(dgIncomesGrid, incomes.ToList());

            }

            if (tcTransactions.SelectedItem == tbiPlaningExpenses)
            {
                var planingExpenses = new SqlTransactionRepository(_connectionString).SortTransactionsByDate(date, "Planing Expense");
                DataGridInformationGetter.InitializeDataGrid(dgPlaningTranGrid, planingExpenses.ToList());
            }
        }

        private void btnCalculateDif_Click(object sender, RoutedEventArgs e)
        {
            var dateFrom = DateTime.Parse(dpDateDifFrom.Text);
            var dateTo = DateTime.Parse(dpDateDifTo.Text);
            decimal expensesSum = new SqlTransactionRepository(_connectionString).CalculeteSumTransactions(dateFrom, dateTo, "Expense");
            decimal planingExpenseSum = new SqlTransactionRepository(_connectionString).CalculeteSumTransactions(dateFrom, dateTo, "Planing Expense");

            tblSumExpenses.Text = expensesSum.ToString();
            tblSumPlaningExpenses.Text = planingExpenseSum.ToString();
            tblDifference.Text = (planingExpenseSum - expensesSum).ToString();
        }

        private void dgExpensedGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (dgExpensedGrid.SelectedIndex >= 0)
            {
                var selectedExpense = DataGridInformationGetter.GetDateFromDataGrid(dgExpensedGrid);
                tbNewExpenseName.Text = selectedExpense[1].Text;
                tbNewExpenseCost.Text = selectedExpense[2].Text;
                cbNewExpenseCraditCard.Text = selectedExpense[3].Text;
                cbNewExpenseCategory.Text = selectedExpense[4].Text;
                dpNewExpenseCreation.Text = selectedExpense[5].Text;
                cbNewExpenseRecurency.Text = selectedExpense[6].Text;

            }
        }

        private void btnUpdateNewExpense_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            DateTime.TryParse(dpNewExpenseCreation.Text, out date);
            decimal cost;
            decimal.TryParse(tbNewExpenseCost.Text.Replace('.', ','), out cost);
            var transaction = new Transactions()
            {
                Id = int.Parse(DataGridInformationGetter.GetDateFromDataGrid(dgExpensedGrid)[0].Text),
                Name = tbNewExpenseName.Text,
                Cost = cost,
                CreationDate = DateTime.Parse(dpNewExpenseCreation.Text),
                RepeatableType = cbNewExpenseRecurency.Text,
                TypeOfTransaction = "Expense",
                CreditCard = cbNewExpenseCraditCard.Text,
                UserId = CurrentUser.Id,
                CategoryName = cbNewExpenseCategory.Text
            };
            var selectedTransaction = new SqlTransactionRepository(_connectionString).UpdateTransaction(transaction, tbNewExpenseName.Text, "Expense");
            _expenseList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Expense", CurrentUser.Id).ToList();
            DataGridInformationGetter.InitializeDataGrid(dgExpensedGrid, _expenseList.ToList());

        }

        private void dgIncomesGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgIncomesGrid.SelectedIndex >= 0)
            {
                var selectedIncome = DataGridInformationGetter.GetDateFromDataGrid(dgIncomesGrid);
                tbNewIncomeName.Text = selectedIncome[1].Text;
                tbNewIncomeCost.Text = selectedIncome[2].Text;
                cbNewIncomeCraditCard.Text = selectedIncome[3].Text;
                cbNewIncomeCategory.Text = selectedIncome[4].Text;
                dpNewIncomeCreation.Text = selectedIncome[5].Text;
                cbNewIncomeRecurency.Text = selectedIncome[6].Text;
            }
        }

        private void dgPlaningTranGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgPlaningTranGrid.SelectedIndex >= 0)
            {
                var selectedPlaningTran = DataGridInformationGetter.GetDateFromDataGrid(dgPlaningTranGrid);
                tbNewPlaningName.Text = selectedPlaningTran[1].Text;
                tbNewPlaningTranCost.Text = selectedPlaningTran[2].Text;
                cbNewPlaningTranCard.Text = selectedPlaningTran[3].Text;
                cbNewPlaningTranCategory.Text = selectedPlaningTran[4].Text;
                dpNewPlaninTranCreation.Text = selectedPlaningTran[5].Text;
                cbNewPlaningTranRecurency.Text = selectedPlaningTran[6].Text;
            }
        }

        private void btnUpdateNewIncome_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            DateTime.TryParse(dpNewIncomeCreation.Text, out date);
            decimal cost;
            decimal.TryParse(tbNewIncomeCost.Text.Replace('.', ','), out cost);
            var transaction = new Transactions()
            {
                Id = int.Parse(DataGridInformationGetter.GetDateFromDataGrid(dgIncomesGrid)[0].Text),
                Name = tbNewIncomeName.Text,
                Cost = cost,
                CreationDate = DateTime.Parse(dpNewIncomeCreation.Text),
                RepeatableType = cbNewIncomeRecurency.Text,
                TypeOfTransaction = "Income",
                CreditCard = cbNewIncomeCraditCard.Text,
                UserId = CurrentUser.Id,
                CategoryName = cbNewIncomeCategory.Text
            };
            var selectedTransaction = new SqlTransactionRepository(_connectionString).UpdateTransaction(transaction, tbNewIncomeName.Text, "Income");
            _incomeList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Income", CurrentUser.Id).ToList();
            DataGridInformationGetter.InitializeDataGrid(dgIncomesGrid, _incomeList.ToList());
        }

        private void btnUpdateNewPlaning_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            DateTime.TryParse(dpNewPlaninTranCreation.Text, out date);
            decimal cost;
            decimal.TryParse(tbNewPlaningTranCost.Text.Replace('.', ','), out cost);
            var transaction = new Transactions()
            {
                Id = int.Parse(DataGridInformationGetter.GetDateFromDataGrid(dgPlaningTranGrid)[0].Text),
                Name = tbNewPlaningName.Text,
                Cost = cost,
                CreationDate = DateTime.Parse(dpNewPlaninTranCreation.Text),
                RepeatableType = cbNewPlaningTranRecurency.Text,
                TypeOfTransaction = "Income",
                CreditCard = cbNewPlaningTranCard.Text,
                UserId = CurrentUser.Id,
                CategoryName = cbNewPlaningTranCategory.Text
            };
            var selectedTransaction = new SqlTransactionRepository(_connectionString).UpdateTransaction(transaction, tbNewPlaningName.Text, "Planing Expense");
            _planingTransactionList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Planing Expense", CurrentUser.Id).ToList();
            DataGridInformationGetter.InitializeDataGrid(dgPlaningTranGrid, _planingTransactionList.ToList());

        }

        private void AddPlaningTran_Click(object sender, RoutedEventArgs e)
        {
            decimal planingCost;
            decimal.TryParse(tbNewPlaningTranCost.Text.Replace('.', ','), out planingCost);
            var planingExpense = new Transactions();
            if (planingCost != 0)
            {
                planingExpense = new Transactions()
                {

                    Name = tbNewPlaningName.Text,
                    Cost = planingCost,
                    CreationDate = DateTime.Parse(dpNewPlaninTranCreation.Text),
                    RepeatableType = cbNewPlaningTranRecurency.Text,
                    TypeOfTransaction = "Planing Expense",
                    CreditCard = cbNewPlaningTranCard.Text,
                    UserId = CurrentUser.Id,
                    CategoryName = cbNewPlaningTranCategory.Text

                };
            }
            var newTransaction = new SqlTransactionRepository(_connectionString).InsertNewTransaction(planingExpense);
            _planingTransactionList.Add(planingExpense);
            DataGridInformationGetter.InitializeDataGrid(dgPlaningTranGrid, _planingTransactionList.ToList());

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            _planingTransactionList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Planing Expense", CurrentUser.Id).ToList();
            _incomeList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Income", CurrentUser.Id).ToList();
            _expenseList = new SqlTransactionRepository(_connectionString).SelectAllTransactions("Expense", CurrentUser.Id).ToList();

            DataGridInformationGetter.InitializeDataGrid(dgExpensedGrid, _expenseList.ToList());
            DataGridInformationGetter.InitializeDataGrid(dgIncomesGrid, _incomeList.ToList());
            DataGridInformationGetter.InitializeDataGrid(dgPlaningTranGrid, _planingTransactionList.ToList());
        }
    }

}
