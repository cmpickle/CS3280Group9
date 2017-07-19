using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    /// <summary>
    /// This class holds the invoice number, invoice date, and total charge
    /// </summary>
    public class clsInvoice
    {
        /// <summary>
        /// This is the invoice number for the invoice
        /// </summary>
        private int iInvoiceNum;

        /// <summary>
        /// This is the invoice date for the invoice
        /// </summary>
        private DateTime dtInvoiceDate;

        /// <summary>
        /// This is the total charge for the invoice
        /// </summary>
        private decimal dTotalCharge;

        /// <summary>
        /// This is the invoice items collection for the invoice
        /// </summary>
        private ObservableCollection<clsInvoiceItem> ocItemsCollection;

        /// <summary>
        /// This is the getter and setter function for attribute sInvoiceNum
        /// </summary>
        public int InvoiceNum
        {
            get { return iInvoiceNum; }
            set { iInvoiceNum = value; }
        }

        /// <summary>
        /// This is the getter and setter function for attribute dtInvoiceDate
        /// </summary>
        public DateTime InvoiceDate
        {
            get { return dtInvoiceDate; }
            set { dtInvoiceDate = value; }
        }

        /// <summary>
        /// This is the getter and setter function for attribute dTotalCharge
        /// </summary>
        public decimal TotalCharge
        {
            get { return dTotalCharge; }
            set { dTotalCharge = value; }
        }

        /// <summary>
        /// This is the getter and setter function for attribute ocItemsCollection
        /// </summary>
        public ObservableCollection<clsInvoiceItem> ItemsCollection
        {
            get { return ocItemsCollection; }
            set { ocItemsCollection = value; }
        }
    }
}
