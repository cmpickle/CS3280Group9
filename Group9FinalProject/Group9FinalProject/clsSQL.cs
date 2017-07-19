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
        /// <summary>
        /// This SQL gets all data on an invoice for a given InvoiceID.
        /// </summary>
        /// <param name="sInvoiceID">The InvoiceID for the invoice to retrieve all data.</param>
        /// <returns>All data for the given invoice.</returns>
        public string SelectInvoiceData(int InvoiceNum)
        {
            string sSQL = "SELECT Invoices.InvoiceNum, InvoiceDate, TotalCharge, ItemDesc, Cost, LineItemNum "
                + "FROM Invoices, ItemDesc, LineItems "
                + "WHERE Invoices.InvoiceNum = LineItems.InvoiceNum "
                + "AND ItemDesc.ItemCode = LineItems.ItemCode " 
                + "AND Invoices.InvoiceNum = " + InvoiceNum.ToString();
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
        /// This SQL gets the last line item number in a particular invoice
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public string SelectTheLastLineItemNum(int invoiceNum)
        {
            string sSQL = "SELECT MAX(LineItemNum) FROM LineItems WHERE InvoiceNum = " + invoiceNum.ToString();
            return sSQL;
        }

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
    }
}
