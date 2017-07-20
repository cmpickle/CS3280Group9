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
    class clsPopulateInventoryPg
    {   /// <summary>
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
        /// Currently does nothing
        /// </summary>
        int iItemCount;
        
        /// <summary>
        /// This class populates the dataGrid in the InventoryWindow,
        /// as well as, populates the comboBox
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
        public List<clsInventory> theInventory()
        {
            List<clsInventory> inv = new List<clsInventory>();

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
        /// Returns the Inventory code to the combo box
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
        /// Returns ItemCount number.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return iItemCount;
            }
        }
        
        /// <summary>
        /// Returns inventory number
        /// </summary>
        /// <returns></returns>
        public string getLatestInventoryNum()
        {
            return db.ExecuteScalarSQL(SQLQueries.SelectTheLatestInvNum());
        }

        
    }
}
