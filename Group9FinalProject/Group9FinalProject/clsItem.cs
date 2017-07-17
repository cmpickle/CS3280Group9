using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Group9FinalProject
{
    class clsItem
    {
        private string sItemCode;
        private string sItemDesc;
        private string sCost;
        private int iNumItems;

        public string ItemCode
        {
            get { return sItemCode; }
            set { sItemCode = value; }
        }

        public string ItemDesc
        {
            get { return sItemDesc; }
            set { sItemDesc = value; }
        }

        public string Cost
        {
            get { return sCost; }
            set { sCost = value; }
        }

        public int NumItems
        {
            get { return iNumItems; }
            set { iNumItems = value; }
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
