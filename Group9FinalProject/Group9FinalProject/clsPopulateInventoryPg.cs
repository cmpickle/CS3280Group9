using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Group9FinalProject
{
    /// <summary>
    /// This class populates the dataGrid in the InventoryWindow,
    /// as well as, populates the comboBoxes
    /// </summary>
    class clsPopulateInventoryPg
    {   
        /// <summary>
        /// Connects to the clsDataAccess class
        /// </summary>
        clsDataAccess db;

        /// <summary>
        /// DataSet type
        /// </summary>
        DataSet ds;

        /// <summary>
        /// Connects to the clsSQL class
        /// </summary>
        clsSQL SQLQueries;

        /// <summary>
        /// This is the interface for interacting with the invoice window
        /// This interface will be used to call the 
        /// implemented refresh function in Invoice Window to refresh the updated items info
        /// </summary>
        InvoiceInterface invoiceInterface;       
        
        /// <summary>
        /// clsPopulateInventoryPg constructor
        /// </summary>
        public clsPopulateInventoryPg()
        {
            try
            {
                db = new clsDataAccess();
                ds = new DataSet();
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
        /// Provides the information for the InventoryWindow from the database
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<clsInventory> theInventory()
        {
            ObservableCollection<clsInventory> inv = new ObservableCollection<clsInventory>();

            try
            {
                int num = 0;
                ds = db.ExecuteSQLStatement(SQLQueries.SelectAllInventory(), ref num);
                for (int i = 0; i < num; i++)
                {
                    clsInventory inventory = new clsInventory();
                    inventory.InventoryLetter = ds.Tables[0].Rows[i][0].ToString();
                    inventory.ItemDesc = ds.Tables[0].Rows[i][1].ToString();
                    inventory.ItemCost = Convert.ToDecimal(ds.Tables[0].Rows[i][2].ToString());
                    inv.Add(inventory);
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return inv;
        }

        /// <summary>
        /// Returns the Inventory code to the combo box for searching
        /// </summary>
        /// <returns></returns>
        public List<String> GetInventoryCode()
        {
            List<String> toRet = new List<String>();

            try
            {
                int num = 0;
                ds = db.ExecuteSQLStatement(SQLQueries.SelectAllInventoryItemCode(), ref num);
                for (int i = 0; i < num; ++i)
                {
                    toRet.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return toRet;
        }

        /// <summary>
        /// Returns the item cost to the combo box for searching by price
        /// </summary>
        /// <returns></returns>
        public List<String> GetInventoryCost()
        {
            List<String> toRet = new List<String>();

            try
            {
                int num = 0;
                ds = db.ExecuteSQLStatement(SQLQueries.SelectAllInventoryCost(), ref num);
                for (int i = 0; i < num; ++i)
                {
                    toRet.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return toRet;
        }

        /// <summary>
        /// Retrieves ItemCodes from LineItems
        /// </summary>
        /// <returns></returns>
        public List<String> GetLineItems_ItemCodes()
        {
            List<String> toRet = new List<String>();

            try
            {
                int num = 0;
                ds = db.ExecuteSQLStatement(SQLQueries.RetrieveLineItems_ItemCodes(), ref num);
                for(int i = 0; i < num; i++)
                {
                    toRet.Add(ds.Tables[0].Rows[i][0].ToString());                    
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            return toRet;
        } 
        
        public void RefreshInvoiceItems()
        {
            try
            {
                invoiceInterface.RefreshItems();
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Sets the instance of the InvoiceInterface for the class
        /// </summary>
        /// <param name="invoiceInterface"></param>
        public void SetView(InvoiceInterface invoiceInterface)
        {
            try
            {
                this.invoiceInterface = invoiceInterface;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

    }
}
