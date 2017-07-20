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

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// When and item is selected and this button is pressed, it pulls the selected info into the 
        /// text boxes and can be saved.
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
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

       
    }
}
