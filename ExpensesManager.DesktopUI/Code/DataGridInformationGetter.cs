using ExpensesManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExpensesManager.DesktopUI.Code
{
    public class DataGridInformationGetter
    {
        public static TextBlock[] GetDateFromDataGrid(DataGrid dataGrid)
        {
            var filld = new TextBlock[]
            {
            dataGrid.Columns[0].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock,
            dataGrid.Columns[1].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock,
            dataGrid.Columns[2].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock,
            dataGrid.Columns[3].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock,
            dataGrid.Columns[4].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock,
            dataGrid.Columns[5].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock,
            dataGrid.Columns[6].GetCellContent(dataGrid.Items[dataGrid.SelectedIndex]) as TextBlock
            };

            return filld;
        }
        public static void InitializeDataGrid(DataGrid dataGrid, IList<Transactions> transactions)
        {
            dataGrid.ItemsSource = transactions;
            dataGrid.Items.Refresh();
        }
    }
}