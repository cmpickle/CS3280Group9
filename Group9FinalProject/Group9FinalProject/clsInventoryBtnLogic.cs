using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    /// <summary>
    /// This class does the logic behind the buttons in InventoryWindow
    /// </summary>
    
    class clsInventoryBtnLogic
    {
        #region Attributes
        /// <summary>
        /// Connects to the clsDataAccess class
        /// </summary>
        clsDataAccess db = new clsDataAccess();

        /// <summary>
        /// Connection to the clsSQL class
        /// </summary>
        clsSQL SQLQueries = new clsSQL();

        /// <summary>
        /// DataSet type
        /// </summary>
        DataSet ds = new DataSet();

        /// <summary>
        /// Obtains a list from LineItems table, ItemCodes column
        /// </summary>
        List<String> LineItem_ItemCodes = new List<String>();

        /// <summary>
        /// Obtains information from clsPopulateInventoryPg
        /// </summary>
        clsPopulateInventoryPg clsPopInventory = new clsPopulateInventoryPg();
        #endregion

        #region Add Button
        /// <summary>
        /// Logic for the add button. Adds items to the inventory list. 
        /// </summary>
        /// <param name="primKey"></param>
        /// <param name="itemDesc"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public Boolean AddInventoryRow(string primKey, string itemDesc, decimal cost)
        {
            try
            {
                clsPopulateInventoryPg clsPopInv = new clsPopulateInventoryPg();

                // Returned if row was added or not
                Boolean rowAdded = false;

                // In case a row needs to be returned
                int rowAddedToDb;

                // Counts if there are matching primary key values. If 1 or more it wont try to add.
                int count = 0;
                List<string> curPK = clsPopInv.GetInventoryCode();
                foreach (string code in curPK)
                {
                    if (primKey == code)
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    rowAddedToDb = db.ExecuteNonQuery(SQLQueries.AddInventoryItem(primKey, itemDesc, cost));
                    rowAdded = true;
                }

                return rowAdded;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        #endregion

        #region Save button
        /// <summary>
        /// Checks if the Primary key letter(s) already exists in the inventory database.
        /// If so, the user will be promt if they want to overwrite the current data or not. 
        /// If not, it will write it to a new row
        /// </summary>
        /// <param name="primKey"></param>
        /// <returns></returns>
        public Boolean SaveButtonCheck(string primKey)
        {
            clsPopulateInventoryPg clsPopInv = new clsPopulateInventoryPg();

            // Checks if primary key matches an existing primary key
            Boolean PKexists = false;
            // Gets current list of primary keys that already exist 
            List<string> curPK = clsPopInv.GetInventoryCode();

            foreach(string code in curPK)
            {
                if (primKey == code)
                {
                    PKexists = true;
                }
            }
            return PKexists;
        }

        /// <summary>
        /// Updates a row that already has data in it. 
        /// </summary>
        /// <param name="primKey"></param>
        /// <param name="itemDesc"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public Boolean SaveButton_SaveRow(string primKey, string itemDesc, decimal cost)
        {
            // Indicates if a row was updated or not
            Boolean rowsChg = false;

            // Number of rows updated
            int rowsUpdated;

            rowsUpdated = db.ExecuteNonQuery(SQLQueries.SaveInventoryChanges(primKey, itemDesc, cost));
            if (rowsUpdated > 0)
            {
                rowsChg = true;
            }

            return rowsChg;
        }

        /// <summary>
        /// Add a row to the inventory list. Only allows it if there is no matching 
        /// item code.
        /// </summary>
        /// <param name="primKey"></param>
        /// <param name="itemDesc"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        /// 
        #endregion       

        #region Delete button
        /// <summary>
        /// This is the logic behind the delete button on the InventoryWindow page.
        /// Takes the selected row, sends the primary key letter, compares it with LineItems 
        /// invoice items, if there are associations between those ItemCodes and invoices
        /// the user is unable to delete the item. Otherwise the user can.
        /// </summary>
        /// <param name="itemCodeSelected"></param>
        /// <returns></returns>
        public Boolean DeleteButtonLogic(string itemCodeSelected)
        {
            try
            {
                //Retrieves lineItem table ItemCodes
                LineItem_ItemCodes = clsPopInventory.GetLineItems_ItemCodes();

                //If count >= 1, there is a value in LineItems that matches
                int delCount = 0;

                //Number of rows changed if needed
                int num = 0;

                //Returns if the delete was successful or not
                Boolean result = true;   

                foreach (string ic in LineItem_ItemCodes)
                {
                    if (ic.Equals(itemCodeSelected))
                    {
                        delCount++;
                    }
                }

                if (delCount > 0)
                {
                    result = false;
                }
                else
                {   
                    //Executes the delete item statement
                    num = db.ExecuteNonQuery(SQLQueries.DeleteInventoryRow(itemCodeSelected));
                }
                return result;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            
        }

        /// <summary>
        /// Gets the list of invoices that an item code is currently associated with.
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string getExistingInvoicesOfItemCode(string itemCode)
        {
            try
            {
                List<String> Ret = new List<String>();
                string toRet = "";
                int num = 0;
                int cnt = 0;
                ds = db.ExecuteSQLStatement(SQLQueries.RetrieveLineItems_MatchingIC_InvNum(itemCode), ref num);
                for (int i = 0; i < num; ++i)
                    {
                        Ret.Add(ds.Tables[0].Rows[i][0].ToString());
                    }
                foreach (string st in Ret)
                {
                    toRet += st;
                    if (num > 0 && cnt != num-1)
                    {
                        toRet += ", ";
                        cnt++;
                    }
                }

                toRet += ".";

                return toRet;
             }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        #endregion
    }
}
