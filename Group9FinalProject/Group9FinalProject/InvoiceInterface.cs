using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    /// <summary>
    /// This interface is used 
    /// </summary>
    public interface InvoiceInterface
    {
        /// <summary>
        /// This method passes the invoice number to the Invoice interface.
        /// 
        /// This is to be used to pass the selected invoice from the search page to the invoice page.
        /// 
        /// It will tell the page which invoice to set as well as issue a refresh of the page data.
        /// </summary>
        /// <param name="invoiceNum">The selected invoice's number</param>
        void SetInvoice(int invoiceNum);
    }
}
