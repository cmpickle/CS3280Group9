using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Group9FinalProject
{
    /// <summary>
    /// This is the class that helps the InvoiceWindow populate data in its controls
    /// </summary>
    class clsInvoicePage
    {
        /// <summary>
        /// This is the database connection
        /// </summary>
        clsDataAccess db;
        
        /// <summary>
        /// This is the object of clsSQL
        /// </summary>
        clsSQL SQLQueries;

        /// <summary>
        /// This is the constructor of clsInvoicePage
        /// </summary>
        public clsInvoicePage()
        {
            try
            {
                db = new clsDataAccess();
                SQLQueries = new clsSQL();
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This function returns a list of clsItem object
        /// </summary>
        /// <returns></returns>
        public BindingList<clsItem> populateChooseItem()
        {
            try
            {
                DataSet ds = new DataSet();
                int iRet = 0;
                ds =  db.ExecuteSQLStatement(SQLQueries.SelectAllItems(), ref iRet);

                BindingList<clsItem> ItemList = new BindingList<clsItem>();

                for (int i = 0; i < iRet; i++)
                {
                    clsItem Item = new clsItem();
                    Item.ItemCode = ds.Tables[0].Rows[i][0].ToString();
                    Item.ItemDesc = ds.Tables[0].Rows[i][1].ToString();
                    Item.Cost = Convert.ToDecimal(ds.Tables[0].Rows[i][2].ToString());
                    ItemList.Add(Item);
                }
                return ItemList;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This function returns a particular clsInvoice object based on the invoice number parameter
        /// </summary>
        /// <param name="InvoiceNum"></param>
        /// <returns></returns>
        public clsInvoice PopulateInvoice(int InvoiceNum)
        {
            try
            {
                DataSet ds = new DataSet();
                int iRet = 0;

                ds = db.ExecuteSQLStatement(SQLQueries.SelectInvoiceData(InvoiceNum), ref iRet);

                ObservableCollection<clsInvoiceItem> InvoiceItemCollection = new ObservableCollection<clsInvoiceItem>();

                for (int i = 0; i < iRet; i++)
                {
                    clsInvoiceItem InvoiceItem = new clsInvoiceItem();

                    InvoiceItem.ItemDesc = ds.Tables[0].Rows[i][3].ToString();
                    InvoiceItem.ItemCost = Convert.ToDecimal(ds.Tables[0].Rows[i][4].ToString());
                    InvoiceItem.LineItemNum = Convert.ToInt32(ds.Tables[0].Rows[i][5].ToString());
                    InvoiceItem.ItemCode = ds.Tables[0].Rows[i][6].ToString();

                    InvoiceItemCollection.Add(InvoiceItem);
                }

                clsInvoice currInvoice = new clsInvoice();

                currInvoice.ItemsCollection = InvoiceItemCollection;
                currInvoice.InvoiceNum = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                currInvoice.InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[0][1].ToString());
                currInvoice.TotalCharge = Convert.ToDecimal(ds.Tables[0].Rows[0][2].ToString());

                return currInvoice;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }

        /// <summary>
        /// This function returns the invoice number of the latest invoice
        /// </summary>
        /// <returns></returns>
        public int getLatestInvoiceNum()
        {
            return Convert.ToInt32(db.ExecuteScalarSQL(SQLQueries.SelectTheLatestInvNum()));
        }

        /// <summary>
        /// This function saves all the changes of current invoice into the database 
        /// and returns the invoice number of the invoice that it saves change to
        /// </summary>
        /// <param name="Invoice">clsInvoice</param>
        /// <param name="isNewInvoice">Boolean</param>
        /// <returns>InvoiceNum</returns>
        public int SaveChanges(clsInvoice Invoice, bool isNewInvoice)
        {
            try
            {
                int invoiceNum;

                // if this is not a new added invoice
                if (isNewInvoice == false)
                {
                    invoiceNum = Invoice.InvoiceNum;

                    DataSet ds = new DataSet();

                    int iRet = 0;
                    ds = db.ExecuteSQLStatement(SQLQueries.SelectLineItems(Invoice.InvoiceNum), ref iRet);

                    // Clear all the line items that is associated with this invoice
                    for (int i = 0; i < iRet; i++)
                    {
                        int lineItemNum = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString());
                        db.ExecuteNonQuery(SQLQueries.deleteLineItem(Invoice.InvoiceNum, lineItemNum));
                    }

                    // Update the date and total charge for this invoice
                    db.ExecuteNonQuery(SQLQueries.updateInvoice(Invoice.InvoiceNum, Invoice.InvoiceDate, Invoice.TotalCharge));
                }
                // If this is a new invoice
                else
                {
                    db.ExecuteNonQuery(SQLQueries.InsertNewInvoice(Invoice.InvoiceDateString, Invoice.TotalCharge));

                    // Retrieves the invoice number that just got created
                    invoiceNum = Convert.ToInt32(db.ExecuteScalarSQL(SQLQueries.SelectTheLatestInvNum()));
                }

                // Insert all the line items related to this invoice in database
                for (int i = 0; i < Invoice.ItemsCollection.Count; i++)
                {
                    int lineItemNum = Invoice.ItemsCollection[i].LineItemNum;
                    string itemCode = Invoice.ItemsCollection[i].ItemCode;

                    db.ExecuteNonQuery(SQLQueries.InsertNewLineItem(invoiceNum, lineItemNum, itemCode));
                }

                return invoiceNum;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This function deletes a particular invoice from the database 
        /// and returns the updated latest invoice number
        /// </summary>
        /// <param name="Invoice">Invoice</param>
        /// <returns>invoice number</returns>
        public void DeleteInvoice(clsInvoice Invoice)
        {
            try
            {
                DataSet ds = new DataSet();
                int iRet = 0;

                ds = db.ExecuteSQLStatement(SQLQueries.SelectLineItems(Invoice.InvoiceNum), ref iRet);

                // Delete all the line items that is associated with this invoice
                for (int i = 0; i < iRet; i++)
                {
                    int lineItemNum = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString());
                    db.ExecuteNonQuery(SQLQueries.deleteLineItem(Invoice.InvoiceNum, lineItemNum));
                }

                // Delete the invoice
                db.ExecuteNonQuery(SQLQueries.deleteInvoice(Invoice.InvoiceNum));
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This function finds out if there is any invoice exist in the system
        /// </summary>
        /// <returns></returns>
        public bool IsThereInvoice()
        {
            string NumOfInvoices = db.ExecuteScalarSQL(SQLQueries.getNumOfInvoices());

            if (NumOfInvoices == "0")
                return false;
            else
                return true;
        }

    }
}
