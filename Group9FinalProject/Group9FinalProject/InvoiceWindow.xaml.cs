using System;
using System.Collections.Generic;
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
using System.Reflection;

namespace Group9FinalProject
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        clsPopulateInvoicePg Populate;

        /// <summary>
        /// This is the constructor of InvoiceWindow class
        /// </summary>
        public InvoiceWindow()
        {
            try
            { 
                InitializeComponent();
                Populate = new clsPopulateInvoicePg();

                // disabled most of the control when the user first start the program
                cboItems.IsEnabled = false;
                cboItems.Items.Clear(); // Clear the items in the combo box

                txtQuantity.IsEnabled = false;
                btnAdd.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
                btnSave.IsEnabled = false;

                // test populate cboItems
                //cboItems.Items.Clear();
                //cboItems.ItemsSource = Populate.populateChooseItem();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// The event handler for the Search Menu Item
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void miSearchInovice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SearchWindow searchWindow = new SearchWindow();
                searchWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Handle the error.
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }
    }
    
}
