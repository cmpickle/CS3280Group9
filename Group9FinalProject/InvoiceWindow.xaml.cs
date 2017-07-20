﻿using System;
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
        /// <summary>
        /// This is the clsPopulateInvoicePg object that populate the data in the controls of InvoiceWindow
        /// </summary>
        clsPopulateInvoicePg Populate;

        /// <summary>
        /// This keeps track of the current invoice that the InvoiceWindow should display
        /// </summary>
        clsInvoice currInvoice;

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
                int LatestInvNum = Populate.getLatestInvoiceNum();
                displayInvoice(LatestInvNum);
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

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
                // first check if there is an item is selected in the Choose Item combo box
                if (cboItems.SelectedItem != null)
                {
                    clsItem select = (clsItem)cboItems.SelectedItem;
                    clsInvoiceItem InvoiceItem = new clsInvoiceItem();
                    //InvoiceItem.LineItemNum = Populate.getLastLineItemNum(currInvoice.InvoiceNum) + 1;
                    int index = currInvoice.ItemsCollection.Count;
                    InvoiceItem.LineItemNum = currInvoice.ItemsCollection[index - 1].LineItemNum + 1;
                    InvoiceItem.ItemDesc = select.ItemDesc;
                    InvoiceItem.ItemCost = select.Cost;

                    // add the invoice item to the current invoice's invoice items collection
                    currInvoice.ItemsCollection.Add(InvoiceItem);

                    // add the added invoice item to the dataset
                    Populate.addAnInvoiceItem(currInvoice);

                    // display the updated total charge
                    currInvoice.TotalCharge = select.Cost + currInvoice.TotalCharge;
                    txboInvoiceTotal.Text = " Total:  $ " + string.Format("{0:#.00}", currInvoice.TotalCharge);
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
        /// This function displays a particular invoice based on the invoice number parameter
        /// </summary>
        /// <param name="invoiceNum"></param>
        private void displayInvoice(int invoiceNum)
        {
            try
            {
                currInvoice = Populate.PopulateInvoiceItem(invoiceNum);

                dgAddedItems.ItemsSource = currInvoice.ItemsCollection;

                // populate the invoice data into labels
                lblInvoiceDate.Content = "Invoice Date:  " + String.Format("{0:MM/dd/yyyy}", currInvoice.InvoiceDate); ;
                lblInvoiceNum.Content = "Invoice Number:  " + currInvoice.InvoiceNum.ToString();
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

        /// <summary>
        /// The invoice number that is passed in from the search page
        /// </summary>
        /// <param name="invoiceNum">The invoice number</param>
        public void SetInvoice(int invoiceNum)
        {
            throw new NotImplementedException();
        }

        private void miUpdateInventory_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindow inventoryWindow = new InventoryWindow();
            inventoryWindow.ShowDialog();
        }
    }
    
}