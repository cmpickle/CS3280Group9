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
using System.Collections.ObjectModel;

namespace Group9FinalProject
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        /// <summary>
        /// This is the clsPopulateInvoicePg object that populate the data in the controls of InvoiceWindow
        /// </summary>
        clsPopulateInvoicePg Populate;

        /// <summary>
        /// This keeps track of the current invoice that the InvoiceWindow should display
        /// </summary>
        clsInvoice currInvoice;

        /// <summary>
        /// This is the object of SearchWindow
        /// </summary>
        SearchWindow SearchWindowPg;

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

                btnAdd.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
                btnSave.IsEnabled = false;

                // populate the data grid with the latest invoice data when the user first open the program
                string LatestInvNum = Populate.getLatestInvoiceNum();
                currInvoice = Populate.PopulateInvoiceItem(LatestInvNum);
               
                dgAddedItems.ItemsSource = currInvoice.ItemsCollection;

                // populate the invoice data into labels
                lblInvoiceDate.Content = "Invoice Date:  " + String.Format("{0:MM/dd/yyyy}", currInvoice.InvoiceDate); ;
                lblInvoiceNum.Content = "Invoice Number:  " + currInvoice.InvoiceNum;
                txboInvoiceTotal.Text = " Total:  $ " + string.Format("{0:#.00}", currInvoice.TotalCharge);

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This is triggered when the user hits the menu item Search For Invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miSearchInovice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // has to discuss how to pass the Invoice object back to the main window to be displayed
                SearchWindowPg = new SearchWindow();
                this.Hide();
                SearchWindowPg.ShowDialog();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the Edit Invoice button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // enable the controls that allow user to edit the invoice
                btnSave.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;
                btnAdd.IsEnabled = true;
                cboItems.IsEnabled = true;

                // disable some controls that aren't accessible until the Save Invoice button is clicked
                btnAddInvoice.IsEnabled = false;
                btnEditInvoice.IsEnabled = false;

                // reload the combo box that contains all the items in inventory
                cboItems.Items.Clear();
                cboItems.ItemsSource = Populate.populateChooseItem();

                // to be continued
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
