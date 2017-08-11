using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    class clsInventory
    {
        /// <summary>
        /// This is the inventory letter 
        /// </summary>
        private string sInventoryLetter;

        /// <summary>
        /// This is the inventory description
        /// </summary>
        private string sInventoryDesc;

        /// <summary>
        /// Individual item cost
        /// </summary>
        private decimal dItemCost;

        /// <summary>
        /// This is the getter and setter function for attribute sInventoryLetter
        /// </summary>
        public string InventoryLetter
        {
            get { return sInventoryLetter; }
            set { sInventoryLetter = value; }
        }

        /// <summary>
        /// This is the getter and setter function for attribute sInventoryDesc
        /// </summary>
        public string ItemDesc
        {
            get { return sInventoryDesc; }
            set { sInventoryDesc = value; }
        }

        /// <summary>
        /// This is the getter and setter function for attribute dItemCost
        /// </summary>
        public decimal ItemCost
        {
            get { return dItemCost; }
            set { dItemCost = value; }
        }
    }
}
