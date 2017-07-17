using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    class clsSQL
    {
        /// <summary>
        /// This SQL gets all data on an invoice for a given InvoiceID.
        /// </summary>
        /// <param name="sInvoiceID">The InvoiceID for the invoice to retrieve all data.</param>
        /// <returns>All data for the given invoice.</returns>
        public string SelectInvoiceData(string sInvoiceID)
        {
            string sSQL = "SELECT Invoices.InvoiceNum, InvoiceDate, TotalCharge, ItemDesc.ItemCode, ItemDesc, Cost, LineItemNum "
                + "FROM Invoices, ItemDesc, LineItems "
                + "WHERE Invoices.InvoiceNum = LineItems.InvoiceNum "
                + "AND ItemDesc.ItemCode = LineItems.ItemCode" 
                + "AND Invoices.InvoiceNum = " + sInvoiceID;
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
    }
}
