using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Group9FinalProject
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
#region class fields
        /// <summary>
        /// This object holds all of the searching business logic and search methods
        /// </summary>
        clsSearch search = new clsSearch();
        #endregion

#region constructor
        /// <summary>
        /// The default constructor
        /// </summary>
        public SearchWindow()
        {
            try
            {
                InitializeComponent();

                dgSearchPane.ItemsSource = search.GetInvoices();

                search.GetInvoiceNums().ForEach(num => cboInvoiceNum.Items.Add(num));

                search.GetInvoiceDates().ForEach(date => cboInvoiceDate.Items.Add(date));

                search.GetTotalCost().ForEach(cost => cboTotalCost.Items.Add(cost));
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

#region event handlers
        /// <summary>
        /// The event handler for the Select button
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void btnSearchWindowSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(dgSearchPane.SelectedItem == null)
                {
                    MessageBox.Show("You haven't selected an invoice. Please select an invoice.", "No Invoice Selected", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                search.InvoiceSelected((clsInvoice)dgSearchPane.SelectedItem);

                this.Close();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// The event handler for the cancel button
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void btnSearchWindowCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Sets the instance of the InvoiceInterface for the search class
        /// </summary>
        /// <param name="invoiceInterface"></param>
        public void SetView(InvoiceInterface invoiceInterface)
        {
            search.SetView(invoiceInterface);
        }
#endregion

        #region error handling
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
#endregion
    }
}
