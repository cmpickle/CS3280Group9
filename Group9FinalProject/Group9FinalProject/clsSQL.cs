using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    /// <summary>
    /// This is the class that holds all the SQL queries
    /// </summary>
    class clsSQL
    {
        #region Invoice Window Related SQL Statements
        /// <summary>
        /// This SQL gets all data on an invoice for a given InvoiceID.
        /// </summary>
        /// <param name="sInvoiceID">The InvoiceID for the invoice to retrieve all data.</param>
        /// <returns>All data for the given invoice.</returns>
        public string SelectInvoiceData(int InvoiceNum)
        {
            string sSQL = "SELECT Invoices.InvoiceNum, InvoiceDate, TotalCharge, ItemDesc, Cost, LineItemNum, LineItems.ItemCode "
                + "FROM Invoices, ItemDesc, LineItems "
                + "WHERE Invoices.InvoiceNum = LineItems.InvoiceNum "
                + "AND ItemDesc.ItemCode = LineItems.ItemCode " 
                + "AND Invoices.InvoiceNum = " + InvoiceNum.ToString();
            return sSQL;
        }

        /// <summary>
        /// This SQL statement select all the line items related to a particular invoice
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public string SelectLineItems(int InvoiceNum)
        {
            string sSQL = "SELECT LineItemNum FROM LineItems WHERE InvoiceNum = " + InvoiceNum.ToString();
            return sSQL;
        }

        /// <summary>
        /// This SQL gets all the items from the ItemDesc table in the database
        /// </summary>
        /// <returns></returns>
        public string SelectAllItems()
        {
            string sSQL = "SELECT * FROM ItemDesc";
            return sSQL;
        }

        /// <summary>
        /// This SQL gets all data on an item for a given ItemCode
        /// </summary>
        /// <param name="ItemCode">The ItemCode for the item to retrieve all data</param>
        /// <returns></returns>
        public string SelectItemData(string ItemCode)
        {
            string sSQL = "SELECT * FROM ItemDesc WHERE ItemCode = " + ItemCode;
            return sSQL;
        }

        /// <summary>
        /// This SQL gets the InvoiceNum from the latest invoice
        /// </summary>
        /// <returns></returns>
        public string SelectTheLatestInvNum()
        {
            string sSQL = "SELECT MAX(InvoiceNum) FROM Invoices";
            return sSQL;
        }

        /// <summary>
        /// This SQL inserts a new row into the Invoices table
        /// </summary>
        /// <param name="date"></param>
        /// <param name="totalCharge"></param>
        /// <returns></returns>
        public string InsertNewInvoice(string date, decimal totalCharge)
        {
            string sSQL = "INSERT INTO Invoices (InvoiceDate, TotalCharge) Values (#" + date + "#, " + totalCharge.ToString() +");";
            return sSQL;
        }

        /// <summary>
        /// This SQL inserts a new row into the LineItem table
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="lineItemNum"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string InsertNewLineItem(int invoiceNum, int lineItemNum, string itemCode)
        {
            string sSQL = "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values (" + invoiceNum.ToString() + ", " + lineItemNum.ToString() + ", '" + itemCode + "');";
            return sSQL;
        }

        /// <summary>
        /// This SQL update a particular invoice row in Invoices table
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="date"></param>
        /// <param name="totalCharge"></param>
        /// <returns></returns>
        public string updateInvoice(int invoiceNum, DateTime date, decimal totalCharge)
        {
            string sSQL = "UPDATE Invoices SET InvoiceDate = #" + date.ToString() + "#, TotalCharge = " + totalCharge.ToString() + " WHERE InvoiceNum=" + invoiceNum.ToString() + ";";
            return sSQL;
        }

        /// <summary>
        /// This SQL delete a particular line item row in LineItems table
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="lineItemNum"></param>
        /// <returns></returns>
        public string deleteLineItem(int invoiceNum, int lineItemNum)
        {
            string sSQL = "DELETE FROM LineItems WHERE InvoiceNum = " + invoiceNum.ToString() + " AND LineItemNum = " + lineItemNum.ToString();
            return sSQL;
        }

        /// <summary>
        /// This SQL delete a particular invoice row in the Invoices table
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public string deleteInvoice(int invoiceNum)
        {
            string sSQL = "DELETE FROM Invoices WHERE InvoiceNum = " + invoiceNum.ToString();
            return sSQL;
        }

        /// <summary>
        /// This SQL finds out the number of invoices exist in the system
        /// </summary>
        /// <returns></returns>
        public string getNumOfInvoices()
        {
            string sSQL = "SELECT COUNT(*) FROM Invoices";
            return sSQL;
        }
        #endregion

        #region Invoice Selection statements
        /// <summary>
        /// This SQL statement gets all the information about every invoice
        /// </summary>
        /// <returns>SQL string</returns>
        public String SelectAllInvoices()
        {
            string sSQL = "SELECT InvoiceNum, InvoiceDate, TotalCharge FROM Invoices";
            return sSQL;
        }

        /// <summary>
        /// This SQL statement gets every entry that is associated with the given invoice number
        /// </summary>
        /// <param name="number">The invoice number</param>
        /// <returns>SQL string</returns>
        public String SelectInvoiceByNumber(int number)
        {
            string sSQL = String.Format("SELECT InvoiceNum, InvoiceDate, TotalCharge FROM Invoices WHERE InvoiceNum = {0};", number);
            return sSQL;
        }

        /// <summary>
        /// Select all of the distinct invoice numbers from the database
        /// </summary>
        /// <returns>SQL string</returns>
        public String SelectAllInvoiceNumbers()
        {
            String sSQL = "SELECT DISTINCT InvoiceNum FROM Invoices;";
            return sSQL;
        }

        /// <summary>
        /// Select all of the distinct Invoice date values from the database
        /// </summary>
        /// <param name="date"></param>
        /// <returns>SQL string</returns>
        public String SelectAllInvoiceDate()
        {
            String sSQL = "SELECT DISTINCT InvoiceDate FROM Invoices;";
            return sSQL;
        }

        /// <summary>
        /// Select all of the distinct Invoice total cost values from the database
        /// </summary>
        /// <returns>SQL string</returns>
        public String SelectAllInvoiceTotalCost()
        {
            String sSQL = "SELECT DISTINCT TotalCharge FROM Invoices;";
            return sSQL;
        }
        #endregion

        #region Inventory Selection Statements
        /// <summary>
        /// Select all columns in the ItemDesc table
        /// </summary>
        /// <returns></returns>
        public String SelectAllInventory()
        {
            string sSQL = "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc ORDER BY ItemCode ASC";
            return sSQL;
        }

        /// <summary>
        /// Select all items for ItemCode
        /// </summary>
        /// <returns></returns>
        public String SelectAllInventoryItemCode()
        {
            string sSQL = "SELECT DISTINCT ItemCode FROM ItemDesc ORDER BY ItemCode ASC";
            return sSQL;
        }

        /// <summary>
        /// Select an item description depending on what the user types in
        /// </summary>
        /// <returns></returns>
        public String SelectAllInventoryItemDesc(string userInp)
        {
            string sSQL = String.Format("SELECT ItemDesc FROM ItemDesc "
                        + "WHERE ItemDesc LIKE \"{0}\" ORDER BY ItemCode", userInp);
            return sSQL;
        }

        /// <summary>
        /// Select all items in the cost column
        /// </summary>
        /// <returns></returns>
        public String SelectAllInventoryCost()
        {
            string sSQL = "SELECT DISTINCT Cost FROM ItemDesc ORDER BY Cost ASC";
            return sSQL;
        }
        #endregion

        #region Inventory Button Statements
        /// <summary>
        /// Adds an item to the list. Allows adding a primary key value.
        /// </summary>
        /// <returns></returns>
        public String AddInventoryItem(string iCode, string iDesc, decimal theCost)
        {
            string sSQL = String.Format("INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) "
                + "VALUES (\"{0}\", \"{1}\", {2})", iCode, iDesc, theCost);
            return sSQL;
        }

        /// <summary>
        /// Saves changes to a row. Does not update primary key value.
        /// </summary>
        /// <returns></returns>
        public String SaveInventoryChanges(string iCode, string iDesc, decimal theCost)
        {
            string sSQL = String.Format("UPDATE ItemDesc Set ItemDesc = {1}, Cost = {2}"
                + "WHERE ItemCode = {0}", iCode, iDesc, theCost);
            return sSQL;
        }

        /// <summary>
        /// Deletes a row based on primary key value selection
        /// </summary>
        /// <returns></returns>
        public String DeleteInventoryRow(string iCode)
        {
            string sSQL = String.Format("DELETE FROM ItemDesc"
                + "WHERE ItemCode = {0}", iCode);
            return sSQL;
        }
        #endregion
    }
}
