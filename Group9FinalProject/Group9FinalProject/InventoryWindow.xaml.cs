using System;
using System.Collections.Generic;
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
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {   /// <summary>
        /// Connects to the clsInventory class
        /// </summary>
        clsPopulateInventoryPg clsInventory;
        
        /// <summary>
        /// The code behind the GUI. Pulls info in from other classes.
        /// </summary>
        public InventoryWindow()
        {
            try
            {
                InitializeComponent();
                clsInventory = new clsPopulateInventoryPg();
                dgInventory.ItemsSource = clsInventory.theInventory();
                clsInventory.GetInventoryCode().ForEach(num => cboInventoryCode.Items.Add(num));
                clsInventory.GetInventoryCost().ForEach(num => cboInventoryCost.Items.Add(num));

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// When and item is selected and this button is pressed, the row will be deleted. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Text in the text boxes will be added to a new row if this button is pressed
        /// An error will be given if trying to save information to an already existing Item code (primary key)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// When and item is selected and this button is pressed, it pulls the selected info into the 
        /// text boxes and can be saved. Items that can be saved are item's description and cost only
        /// if an existing item code already exists. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// This saves the current selected row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Clears the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbItemCodeInp.Text = "";
                tbItemDescInp.Text = "";
                tbItemCostInp.Text = "";
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
