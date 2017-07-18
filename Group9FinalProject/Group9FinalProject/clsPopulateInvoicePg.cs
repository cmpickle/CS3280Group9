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
    class clsPopulateInvoicePg
    {
        /// <summary>
        /// This is the database connection
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// This is the dataset that holds all the items in the inventory
        /// </summary>
        DataSet dsItemsList;

        /// <summary>
        /// This is the integer that keeps track of the total number of items inside the inventory
        /// </summary>
        int iNumItems;
        
        /// <summary>
        /// This is the object of clsSQL
        /// </summary>
        clsSQL SQLQueries;

        /// <summary>
        /// This is the dataset that holds all the items that are in a particular invoice
        /// </summary>
        DataSet dsInvoiceItemList;

        /// <summary>
        /// This is the integer that keeps track of the number of items in a particular invoice
        /// </summary>
        int iNumInvoiceItems;

        /// <summary>
        /// This is the constructor of clsPopulateInvoicePg
        /// </summary>
        public clsPopulateInvoicePg()
        {
            try
            {
                db = new clsDataAccess();
                dsItemsList = new DataSet();
                SQLQueries = new clsSQL();

                iNumItems = 0;

                // retrieve dataset objects
                dsItemsList = db.ExecuteSQLStatement(SQLQueries.SelectAllItems(), ref iNumItems);
                
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// This is the getter function of iNumItems
        /// </summary>
        public int NumItems
        {
            get { return iNumItems; }
        }

        /// <summary>
        /// This function returns a list of clsItem object
        /// </summary>
        /// <returns></returns>
        public BindingList<clsItem> populateChooseItem()
        {
            try
            {
                BindingList<clsItem> ItemList = new BindingList<clsItem>();
                //List<clsItem> ItemList = new List<clsItem>();
                for (int i = 0; i < NumItems; i++)
                {
                    clsItem Item = new clsItem();
                    Item.ItemCode = dsItemsList.Tables[0].Rows[i][0].ToString();
                    Item.ItemDesc = dsItemsList.Tables[0].Rows[i][1].ToString();
                    Item.Cost = Convert.ToDecimal(dsItemsList.Tables[0].Rows[i][2].ToString());
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
        public clsInvoice PopulateInvoiceItem(string InvoiceNum)
        {
            try
            {
                dsInvoiceItemList = db.ExecuteSQLStatement(SQLQueries.SelectInvoiceData(InvoiceNum), ref iNumInvoiceItems);
                ObservableCollection<clsInvoiceItem> InvoiceItemCollection = new ObservableCollection<clsInvoiceItem>();
                for (int i = 0; i < iNumInvoiceItems; i++)
                {
                    clsInvoiceItem InvoiceItem = new clsInvoiceItem();
                    InvoiceItem.LineItemNum = Convert.ToInt32(dsInvoiceItemList.Tables[0].Rows[i][6].ToString());
                    InvoiceItem.ItemDesc = dsInvoiceItemList.Tables[0].Rows[i][4].ToString();
                    InvoiceItem.ItemCost = Convert.ToDecimal(dsInvoiceItemList.Tables[0].Rows[i][5].ToString());
                    InvoiceItemCollection.Add(InvoiceItem);
                }
                clsInvoice currInvoice = new clsInvoice();
                currInvoice.ItemsCollection = InvoiceItemCollection;
                currInvoice.InvoiceNum = dsInvoiceItemList.Tables[0].Rows[1][0].ToString();
                currInvoice.InvoiceDate = Convert.ToDateTime(dsInvoiceItemList.Tables[0].Rows[0][1].ToString());

                currInvoice.TotalCharge = Convert.ToDecimal(dsInvoiceItemList.Tables[0].Rows[0][2].ToString());
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
        public string getLatestInvoiceNum()
        {
            return db.ExecuteScalarSQL(SQLQueries.SelectTheLatestInvNum());
        }

    }
}
