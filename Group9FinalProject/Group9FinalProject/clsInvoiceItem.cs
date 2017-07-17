using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    public class clsInvoiceItem
    {
        string sItemDesc;
        double dItemCost;
        int iLineItemNum;

        public string ItemDesc
        {
            get { return sItemDesc; }
            set { sItemDesc = value; }
        }

        public double ItemCost
        {
            get { return dItemCost; }
            set { dItemCost = value; }
        }

        public int LineItemNum
        {
            get { return iLineItemNum; }
            set { iLineItemNum = value; }
        }
    }
}
