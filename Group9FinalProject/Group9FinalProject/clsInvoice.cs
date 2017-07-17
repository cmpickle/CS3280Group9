using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    public class clsInvoice
    {
        private int iInvoiceNum;
        private DateTime dtInvoiceDate;
        private double dTotalCharge;
        private List<clsInvoiceItem> lsItemsCollection;

        public int InvoiceNum
        {
            get { return iInvoiceNum; }
            set { iInvoiceNum = value; }
        }

        public DateTime InvoiceDate
        {
            get { return dtInvoiceDate; }
            set { dtInvoiceDate = value; }
        }

        public double TotalCharge
        {
            get { return dTotalCharge; }
            set { dTotalCharge = value; }
        }

        public List<clsInvoiceItem> ItemsCollection
        {
            get { return lsItemsCollection; }
            set { lsItemsCollection = value; }
        }
    }
}
