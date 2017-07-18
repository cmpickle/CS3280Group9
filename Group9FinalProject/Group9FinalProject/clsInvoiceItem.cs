using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    /// <summary>
    /// This is the class that hold the item description, item cost, and line item number of a invoice item
    /// </summary>
    public class clsInvoiceItem
    {
        /// <summary>
        /// This is the item description
        /// </summary>
        string sItemDesc;

        /// <summary>
        /// This is the cost of a single item
        /// </summary>
        decimal dItemCost;

        /// <summary>
        /// This is the line item number
        /// </summary>
        int iLineItemNum;

        /// <summary>
        /// This is the getter and setter function for sItemDesc
        /// </summary>
        public string ItemDesc
        {
            get { return sItemDesc; }
            set { sItemDesc = value; }
        }

        /// <summary>
        /// This is the getter and setter function for dItemCost
        /// </summary>
        public decimal ItemCost
        {
            get { return dItemCost; }
            set { dItemCost = value; }
        }

        /// <summary>
        /// This is the getter and setter function for iLineItemNum
        /// </summary>
        public int LineItemNum
        {
            get { return iLineItemNum; }
            set { iLineItemNum = value; }
        }
    }
}
