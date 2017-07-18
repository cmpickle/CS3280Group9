using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Group9FinalProject
{
    /// <summary>
    /// This is the class that holds item code, item description, and cost of the Item
    /// </summary>
    class clsItem
    {
        /// <summary>
        /// This is the item code of an item
        /// </summary>
        private string sItemCode;

        /// <summary>
        ///  This is the item description of an item
        /// </summary>
        private string sItemDesc;

        /// <summary>
        /// This is the cost of an item
        /// </summary>
        private decimal dCost;

        /// <summary>
        /// This is the getter and setter function for sItemCode
        /// </summary>
        public string ItemCode
        {
            get { return sItemCode; }
            set { sItemCode = value; }
        }

        /// <summary>
        /// This is the getter and setter function for sItemDesc
        /// </summary>
        public string ItemDesc
        {
            get { return sItemDesc; }
            set { sItemDesc = value; }
        }

        /// <summary>
        /// This is the getter and setter function for sCost
        /// </summary>
        public decimal Cost
        {
            get { return dCost; }
            set { dCost = value; }
        }

        /// <summary>
        /// Override the ToString method so that this string is displayed in the combo box.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return sItemCode + "  " + sItemDesc;
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
