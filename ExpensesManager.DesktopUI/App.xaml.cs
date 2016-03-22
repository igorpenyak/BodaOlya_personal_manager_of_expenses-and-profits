using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExpensesManager.DesktopUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {


            // MessageBox.Show("Some problem happens during progam work. Please check input data or try later\n\n" + e.Exception.FlattenException(), "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Warning);


            MessageBox.Show("Some problem happens during progam work. Please check input data or try later\n\n", "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Warning);

            e.Handled = true;
        }
    }
}
