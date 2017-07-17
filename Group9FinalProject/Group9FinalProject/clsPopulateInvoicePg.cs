using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace Group9FinalProject
{
    class clsPopulateInvoicePg
    {
        clsDataAccess db;
        DataSet dsItemsList;
        int iNumItems;
        clsSQL SQLQueries;
        DataSet dsInvoiceItemList;

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

        public int NumItems
        {
            get { return iNumItems; }
        }

        /// <summary>
        /// This function returns a list of clsItem object
        /// </summary>
        /// <returns></returns>
        public List<clsItem> populateChooseItem()
        {
            try
            {
                List<clsItem> ItemList = new List<clsItem>();
                for (int i = 0; i < NumItems; i++)
                {
                    clsItem Item = new clsItem();
                    Item.ItemCode = dsItemsList.Tables[0].Rows[i][0].ToString();
                    Item.ItemDesc = dsItemsList.Tables[0].Rows[i][1].ToString();
                    Item.Cost = dsItemsList.Tables[0].Rows[i][2].ToString();
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


    }
}
