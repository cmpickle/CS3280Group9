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
    public partial class InvoiceWindow : Window, InvoiceInterface
    {
        #region Attributes
        /// <summary>
        /// This is the clsInvoicePage object that populate the data in the controls of InvoiceWindow
        /// </summary>
        clsInvoicePage InvoicePage;

        /// <summary>
        /// This keeps track of the current invoice that the InvoiceWindow should display
        /// </summary>
        clsInvoice currInvoice;

        /// <summary>
        /// This Boolean variable is true when the user is adding a new invoice
        /// </summary>
        bool bIsAddingNewInvoice;

        /// <summary>
        /// This holds the new invoice that is under the process of addition (before saving it)
        /// </summary>
        clsInvoice newInvoice;

        #endregion

        #region Constructor
        /// <summary>
        /// This is the constructor of InvoiceWindow class
        /// </summary>
        public InvoiceWindow()
        {
            try
            { 
                InitializeComponent();
                InvoicePage = new clsInvoicePage();

                cboItems.Items.Clear(); // Clear the items in the combo box
                SetReadOnlyMode();

                // Check if there is any invoice inside the database
                if (InvoicePage.IsThereInvoice())
                {
                    // Populate the data grid with the latest invoice data when the user first open the program
                    int LatestInvNum = InvoicePage.getLatestInvoiceNum();
                    DisplayInvoice(LatestInvNum);
                }
                else
                {
                    SetNoInvoiceLeftMode();
                }

                bIsAddingNewInvoice = false;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region Event Handler

        /// <summary>
        /// This function is triggered when the Edit Invoice button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if there is a current invoice in the system
                if (currInvoice != null)
                {
                    SetEditableMode();
                    // reload the combo box that contains all the items in inventory
                    cboItems.ItemsSource = null;
                    cboItems.Items.Clear();
                    cboItems.ItemsSource = InvoicePage.populateChooseItem();
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the add button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First check if there is an item is selected in the Choose Item combo box
                if (cboItems.SelectedItem != null)
                {
                    clsInvoice tempInvoice = new clsInvoice();
                    tempInvoice = currInvoice;

                    if (bIsAddingNewInvoice)
                        tempInvoice = newInvoice;

                    clsItem select = (clsItem)cboItems.SelectedItem;

                    clsInvoiceItem InvoiceItem = new clsInvoiceItem();
                    
                    InvoiceItem.LineItemNum = tempInvoice.ItemsCollection.Count + 1;
                    InvoiceItem.ItemDesc = select.ItemDesc;
                    InvoiceItem.ItemCost = select.Cost;
                    InvoiceItem.ItemCode = select.ItemCode;

                    // Add the invoice item to the current invoice's invoice items collection
                    tempInvoice.ItemsCollection.Add(InvoiceItem);

                    // Refresh the data grid
                    RefreshDataGrid(tempInvoice);

                    // Display the updated total charge
                    tempInvoice.TotalCharge = select.Cost + tempInvoice.TotalCharge;
                    txboInvoiceTotal.Text = string.Format("{0:#.00}", tempInvoice.TotalCharge);
                }

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the Delete Item button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAddedItems.SelectedItem != null)
                {
                    clsInvoice tempInvoice = new clsInvoice();
                    tempInvoice = currInvoice;

                    if (bIsAddingNewInvoice)
                        tempInvoice = newInvoice;

                    int rowIndex = dgAddedItems.SelectedIndex;

                    clsInvoiceItem InvoiceItem = (clsInvoiceItem)dgAddedItems.SelectedItem;
                    decimal cost = InvoiceItem.ItemCost;

                    tempInvoice.ItemsCollection.RemoveAt(rowIndex);

                    RegenLineItemNum(tempInvoice);
                    RefreshDataGrid(tempInvoice);

                    // Display the updated total charge
                    tempInvoice.TotalCharge = tempInvoice.TotalCharge - cost;
                    txboInvoiceTotal.Text = string.Format("{0:#.00}", tempInvoice.TotalCharge);
                }

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the Cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult r = MessageBox.Show("Are you sure you want to leave the current page and discard any changes that you have made?", 
                    "Leaving Current Page", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (r == MessageBoxResult.No)
                    return;
                else
                {
                    cboItems.SelectedItem = null;
                    DisplayInvoice(currInvoice.InvoiceNum);

                    // If the Cancel button is clicked when system is adding a new invoice
                    if (bIsAddingNewInvoice)
                    {
                        newInvoice = null;
                        bIsAddingNewInvoice = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the Save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if the fields for invoice are filled
                if (dgAddedItems.Items.Count == 0 || dpInvoiceDate.Text == "" ||
                    txboInvoiceTotal.Text == "")
                {
                    MessageBox.Show("Please fill in all the fields for this invoice", "Invalid Action", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // If all the fields are filled in
                else
                {
                    clsInvoice tempInvoice = currInvoice;

                    // This step is only retrieving the up to date Items Collection that is saved in the Invoice
                    // If system is currently in a process of adding a new invoice
                    if (bIsAddingNewInvoice)
                        tempInvoice = newInvoice;

                    // Save all the data that is presenting in the current board
                    tempInvoice.InvoiceDate = Convert.ToDateTime(dpInvoiceDate.Text);
                    tempInvoice.TotalCharge = Convert.ToDecimal(txboInvoiceTotal.Text);

                    int invoiceNum = InvoicePage.SaveChanges(tempInvoice, bIsAddingNewInvoice);

                    // Displays the invoice that has been modified or added
                    DisplayInvoice(invoiceNum);

                    // Sets the newInvoice back to a empty object for next time use
                    newInvoice = null;

                    // Always sets it back to false after the save action is completed
                    bIsAddingNewInvoice = false;

                    MessageBox.Show("Saved", "Saved Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                           MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the Add Invoice button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                newInvoice = new clsInvoice();

                newInvoice.ItemsCollection = new ObservableCollection<clsInvoiceItem>();

                dgAddedItems.ItemsSource = newInvoice.ItemsCollection;

                // Setting the new invoice's total charge to zero
                newInvoice.TotalCharge = 0;

                // Setting the flag for adding new invoice to true
                bIsAddingNewInvoice = true;

                SetEditableMode();

                // Clear all the fields for adding a new invoice
                lblInvoiceNum.Content = "Invoice Number: TBD";
                dpInvoiceDate.Text = DateTime.Today.ToString();
                txboInvoiceTotal.Text = "";

                cboItems.ItemsSource = null;
                cboItems.Items.Clear();
                cboItems.ItemsSource = InvoicePage.populateChooseItem();

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                           MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function is triggered when the Delete Invoice button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make sure that there is a invoice displaying on current window
                if (currInvoice != null)
                {
                    MessageBoxResult r = MessageBox.Show("Are you sure you want to delete this invoice?",
                        "Delete Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (r == MessageBoxResult.No)
                        return;
                    else
                    {
                        InvoicePage.DeleteInvoice(currInvoice);

                        MessageBox.Show("Deleted Successfully", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Check if there is still invoice in the system
                        if (InvoicePage.IsThereInvoice())
                        {
                            int LatestInvoiceNum = InvoicePage.getLatestInvoiceNum();
                            DisplayInvoice(LatestInvoiceNum);
                        }
                        else
                        {
                            SetNoInvoiceLeftMode();
                            currInvoice = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                           MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// This function refreshes the data presenting in the Invoice Window
        /// every time it regains focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoiceWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (InvoicePage.IsThereInvoice())
                {
                    DisplayInvoice(currInvoice.InvoiceNum);
                }
                else
                {
                    SetNoInvoiceLeftMode();
                }
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// This function only allows numbers, decimal point, and back space to be input in the Total Charge text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txboInvoiceTotal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Only allows the number to be entered
                if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    || e.Key == Key.Decimal || e.Key == Key.OemPeriod))
                {
                    // Allow the user to use the backspace, delete, and enter
                    if (!(e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Enter))
                    {
                        e.Handled = true;
                    }
                }
                // Only allows one decimal point to be entered
                if ((e.Key == Key.Decimal || e.Key == Key.OemPeriod) && (sender as TextBox).Text.IndexOf('.') != -1)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                           MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region Helper Functions
        /// <summary>
        /// This function regenerate the line item number for the item collection after deletion
        /// </summary>
        /// <param name="curr"></param>
        private void RegenLineItemNum(clsInvoice Invoice)
        {
            try
            {
                int length = Invoice.ItemsCollection.Count;
                for (int i = 0; i < length; i++)
                {
                    if (Invoice.ItemsCollection[i].LineItemNum != (i + 1))
                        Invoice.ItemsCollection[i].LineItemNum = i + 1;
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
        /// This function refresh the added item data grid
        /// </summary>
        private void RefreshDataGrid(clsInvoice Invoice)
        {
            try
            {
                dgAddedItems.ItemsSource = null;
                dgAddedItems.ItemsSource = Invoice.ItemsCollection;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This function displays a particular invoice based on the invoice number parameter
        /// </summary>
        /// <param name="invoiceNum"></param>
        private void DisplayInvoice(int invoiceNum)
        {
            try
            {
                currInvoice = InvoicePage.PopulateInvoice(invoiceNum);

                dgAddedItems.ItemsSource = currInvoice.ItemsCollection;

                // populate the invoice data into labels
                dpInvoiceDate.Text = String.Format("{0:MM/dd/yyyy}", currInvoice.InvoiceDate);
                lblInvoiceNum.Content = "Invoice Number:  " + currInvoice.InvoiceNum.ToString();
                txboInvoiceTotal.Text = string.Format("{0:#.00}", currInvoice.TotalCharge);

                SetReadOnlyMode();
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// This function sets controls on InvoiceWindow to read-only mode
        /// </summary>
        private void SetReadOnlyMode()
        {
            try
            {
                dpInvoiceDate.IsEnabled = false;
                txboInvoiceTotal.IsEnabled = false;
                cboItems.IsEnabled = false;
                btnAdd.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnEditInvoice.IsEnabled = true;
                btnAddInvoice.IsEnabled = true;
                btnDeleteInvoice.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        private void SetEditableMode()
        {
            try
            {
                // enable the controls that allow user to edit the invoice
                btnSave.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;
                btnAdd.IsEnabled = true;
                cboItems.IsEnabled = true;
                btnCancel.IsEnabled = true;
                dpInvoiceDate.IsEnabled = true;
                txboInvoiceTotal.IsEnabled = true;

                // disable some controls that aren't accessible until the Save/Cancel button is clicked
                btnAddInvoice.IsEnabled = false;
                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This function enables the mode when there is no invoice left in the system 
        /// for the Invoice Window
        /// </summary>
        private void SetNoInvoiceLeftMode()
        {
            try
            {
                SetReadOnlyMode();

                btnEditInvoice.IsEnabled = false;
                btnDeleteInvoice.IsEnabled = false;

                dgAddedItems.ItemsSource = null;
                cboItems.ItemsSource = null;
                cboItems.Items.Clear();
                dpInvoiceDate.Text = "01/01/2000";
                txboInvoiceTotal.Text = "";
                lblInvoiceNum.Content = "Invoice Number:    ";
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region Interface
        /// <summary>
        /// The invoice number that is passed in from the search page
        /// </summary>
        /// <param name="invoiceNum">The invoice number</param>
        public void SetInvoice(int invoiceNum)
        {
            try
            {
                DisplayInvoice(invoiceNum);
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region Navigate To Other Pages

        /// <summary>
        /// The event handler for the Search Menu Item.
        /// This is triggered when the user hits the menu item Search For Invoice
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event args</param>
        private void miSearchInovice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // has to discuss how to pass the Invoice object back to the main window to be displayed
                SearchWindow searchWindow = new SearchWindow();
                searchWindow.SetView(this);
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
        /// This function is triggered when the Update Inventory menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miUpdateInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InventoryWindow inventoryWindow = new InventoryWindow();
                inventoryWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region Error Handling
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
