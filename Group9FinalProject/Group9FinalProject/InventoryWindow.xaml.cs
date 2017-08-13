using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    {
        #region Attributes
        /// <summary>
        /// Connects to the clsInventory class
        /// </summary>
        clsPopulateInventoryPg clsPopInventory;

        /// <summary>
        /// Populates the datagrid for start and when items are selected on the search bar
        /// </summary>
        IEnumerable<clsInventory> invp;
        #endregion 

        #region Constructor
        /// <summary>
        /// The code behind the GUI. Pulls info in from other classes.
        /// </summary>
        public InventoryWindow()
        {
            try
            {
                InitializeComponent();
                clsPopInventory = new clsPopulateInventoryPg();

                invp = clsPopInventory.theInventory();
                dgInventory.ItemsSource = invp;
                
                clsPopInventory.GetInventoryCode().ForEach(num => cboInventoryCode.Items.Add(num));
                clsPopInventory.GetInventoryCost().ForEach(num => cboInventoryCost.Items.Add(num));

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region Button statements
        /// <summary>
        /// Text in the text boxes will be added to a new row if this button is pressed
        /// An error will be given if trying to save information to an already existing Item code (primary key)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            clsInventoryBtnLogic invBtn = new clsInventoryBtnLogic();

            // Checks if items were added or not
            Boolean ifAdded = false;
            // Says if content of text box was all numbers or not
            Boolean allNum = false;
            // Assigns iCode the value in tbItemCodeInp
            string iCode = tbItemCodeInp.Text.ToUpper();
            // Assigns iDesc the value in tbItemDescInp
            string iDesc = tbItemDescInp.Text;
            // Assigns iCost the value in tbItemCostInp
            string iCost = tbItemCostInp.Text;
            // Is assigned the number value inside tbItemCostInp
            decimal itemCostDec;

            if (Decimal.TryParse(iCost, out itemCostDec))
            {
                allNum = true;
                itemCostDec = Math.Round(itemCostDec, 2);
            }

            if (iCode != "" && iDesc != "" && iCost != "" && allNum == true)
            {
                ifAdded = invBtn.AddInventoryRow(iCode, iDesc, itemCostDec);

                if (ifAdded == false)
                {
                    MessageBox.Show("Item was unable to be added.  Item code already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    cboInventoryCode.Items.Clear();
                    clsPopInventory.GetInventoryCode().ForEach(num => cboInventoryCode.Items.Add(num));
                    cboInventoryCost.Items.Clear();
                    clsPopInventory.GetInventoryCost().ForEach(num => cboInventoryCost.Items.Add(num));

                    cboInventoryCode.SelectedItem = null;
                    tbItemDescInpSrchBar.Text = "";
                    cboInventoryCost.SelectedItem = null;

                    invp = clsPopInventory.theInventory();
                    dgInventory.ItemsSource = invp;
                    tbItemCodeInp.Text = "";
                    tbItemDescInp.Text = "";
                    tbItemCostInp.Text = "";
                    btnDeleteItem.IsEnabled = false;
                    btnEditItem.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields with correct data.");
            }
         }

        
        /// <summary>
        /// When and item is selected and this button is pressed, it pulls the selected info into the 
        /// text boxes and can be saved. Items that can be saved are item's description and cost only
        /// if an existing item code already exists. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsInventory clsI = (clsInventory)dgInventory.SelectedItem;
                tbItemCodeInp.Text = clsI.InventoryLetter;
                tbItemDescInp.Text = clsI.ItemDesc;
                tbItemCostInp.Text = clsI.ItemCost.ToString();
                
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This saves the current selected row.
        /// Also adds a new row if this is pressed instead of the add button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsInventoryBtnLogic invBtn = new clsInventoryBtnLogic();

                // Checks if primary key exists
                Boolean pkExist;
                // Checks if the item was saved properly or not
                Boolean ifUpdated;
                // Checks if the new item was added or not
                Boolean ifAdded;
                // Checks if the value inside itemCost was all numbers or not
                Boolean allNum = false;
                // Assigns primKey to the value in tbItemCodeInp text box
                string primKey = tbItemCodeInp.Text.ToUpper();
                // Assigns itemDesc to the value in tbItemDescInp text box
                string itemDesc = tbItemDescInp.Text;
                // Assigns ItemCost to the value in tbItemCostInp text box
                string itemCost = tbItemCostInp.Text;
                // Will be assigned the number value inside itemCost
                decimal itemCostDec;

                pkExist = invBtn.SaveButtonCheck(primKey);
                
                if (Decimal.TryParse(itemCost, out itemCostDec))
                {
                    allNum = true;
                    itemCostDec = Math.Round(itemCostDec, 2);
                }        
                
                if (primKey != "" && itemDesc != "" && itemCost != "" && allNum == true)
                {
                    if (pkExist == true)
                    {
                        MessageBoxResult res = MessageBox.Show("Item code " + primKey + " already exists. Would you like to overwrite its information?", "Overwrite row " + primKey + "", MessageBoxButton.YesNo);
                        if (res.ToString() == "Yes")
                        {
                            ifUpdated = invBtn.SaveButton_SaveRow(primKey, itemDesc, itemCostDec);
                            if (ifUpdated == false)
                            {
                                MessageBox.Show("Item was unable to be saved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                cboInventoryCode.Items.Clear();
                                clsPopInventory.GetInventoryCode().ForEach(num => cboInventoryCode.Items.Add(num));
                                cboInventoryCost.Items.Clear();
                                clsPopInventory.GetInventoryCost().ForEach(num => cboInventoryCost.Items.Add(num));

                                cboInventoryCode.SelectedItem = null;
                                tbItemDescInpSrchBar.Text = "";
                                cboInventoryCost.SelectedItem = null;

                                invp = clsPopInventory.theInventory();
                                dgInventory.ItemsSource = invp;
                                tbItemCodeInp.Text = "";
                                tbItemDescInp.Text = "";
                                tbItemCostInp.Text = "";
                                btnDeleteItem.IsEnabled = false;
                                btnEditItem.IsEnabled = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxResult res = MessageBox.Show("Item code " + primKey + " does not exist. Would you like to create it?", "Create new row", MessageBoxButton.YesNo);
                        if (res.ToString() == "Yes")
                        {
                            ifAdded = invBtn.AddInventoryRow(primKey, itemDesc, itemCostDec);
                            if (ifAdded == false)
                            {
                                MessageBox.Show("Item was unable to be added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                cboInventoryCode.Items.Clear();
                                clsPopInventory.GetInventoryCode().ForEach(num => cboInventoryCode.Items.Add(num));
                                cboInventoryCost.Items.Clear();
                                clsPopInventory.GetInventoryCost().ForEach(num => cboInventoryCost.Items.Add(num));

                                cboInventoryCode.SelectedItem = null;
                                tbItemDescInpSrchBar.Text = "";
                                cboInventoryCost.SelectedItem = null;

                                invp = clsPopInventory.theInventory();
                                dgInventory.ItemsSource = invp;
                                tbItemCodeInp.Text = "";
                                tbItemDescInp.Text = "";
                                tbItemCostInp.Text = "";
                                btnDeleteItem.IsEnabled = false;
                                btnEditItem.IsEnabled = false;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all fields with correct data.");
                }
            }
            catch (Exception ex)
            {
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
            try
            {
                clsInventoryBtnLogic invBtn = new clsInventoryBtnLogic();
                clsInventory clsI = (clsInventory)dgInventory.SelectedItem;

                // Shows existing invoices if item can't be deleted.
                string invoiceExist;
                
                // Checks if the item was deleted or not
                Boolean succeed = false;

                //Ask if the user wants to delete the selected item or not
                MessageBoxResult res = MessageBox.Show("Are you sure you would like to delete this item?", "Delete row", MessageBoxButton.YesNo);

                if (res.ToString() == "Yes")
                {
                    succeed = invBtn.DeleteButtonLogic(clsI.InventoryLetter);

                    if (succeed == false)
                    {
                        invoiceExist = invBtn.getExistingInvoicesOfItemCode(clsI.InventoryLetter);
                        MessageBox.Show("Item was not deleted. Item code is associated with invoice: " + invoiceExist, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        
                    }
                    else
                    {
                        cboInventoryCode.Items.Clear();
                        clsPopInventory.GetInventoryCode().ForEach(num => cboInventoryCode.Items.Add(num));
                        cboInventoryCost.Items.Clear();
                        clsPopInventory.GetInventoryCost().ForEach(num => cboInventoryCost.Items.Add(num));

                        cboInventoryCode.SelectedItem = null;
                        tbItemDescInpSrchBar.Text = "";
                        cboInventoryCost.SelectedItem = null;

                        invp = clsPopInventory.theInventory();
                        dgInventory.ItemsSource = invp;
                        btnDeleteItem.IsEnabled = false;
                        btnEditItem.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This button closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        /// <summary>
        /// Clears Edit Item bar at the bottom
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

        /// <summary>
        /// This function is triggered when the Inventory Window is closed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InventoryWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                clsPopInventory.RefreshInvoiceItems();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Sets the instance of the InvoiceInterface for the clsPopulateInventoryPg class
        /// </summary>
        /// <param name="invoiceInterface"></param>
        public void SetView(InvoiceInterface invoiceInterface)
        {
            try
            {
                clsPopInventory.SetView(invoiceInterface);
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Clears the search bar items at the top
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSearchBar(object sender, RoutedEventArgs e)
        {
            try
            {
                cboInventoryCode.SelectedItem = null;
                tbItemDescInpSrchBar.Text = "";
                cboInventoryCost.SelectedItem = null;

                invp = clsPopInventory.theInventory();
                dgInventory.ItemsSource = invp;

                btnDeleteItem.IsEnabled = false;
                btnEditItem.IsEnabled = false;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region Datagrid selection
        /// <summary>
        /// Enables the buttons when something is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sltCellChange(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                btnDeleteItem.IsEnabled = true;
                btnEditItem.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }        
        #endregion

        #region Search Bar Items
        /// <summary>
        /// Populates datagrid according to Item Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByItemCode(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                invp = from item in invp where item.InventoryLetter.ToString().Equals(cboInventoryCode.SelectedItem) select item;
                dgInventory.ItemsSource = invp;
                
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Sorts item by cost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByItemCost(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                invp = from item in invp where item.ItemCost.ToString().Equals(cboInventoryCost.SelectedItem) select item;
                dgInventory.ItemsSource = invp;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }        

        /// <summary>
        /// Sorts by item description each time a user types in a letter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByItemDesc(object sender, TextChangedEventArgs e)
        {
            try
            {
                invp = from item in invp where item.ItemDesc.ToLower().Contains((tbItemDescInpSrchBar.Text).ToLower()) select item;
                dgInventory.ItemsSource = invp;
            }
            catch(Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
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
