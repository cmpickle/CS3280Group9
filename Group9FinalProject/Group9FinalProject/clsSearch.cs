using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    /// <summary>
    /// Holds the business logic for searching invoices
    /// </summary>
    class clsSearch
    {
        #region class fields
        /// <summary>
        /// Data access object for making queries on the database
        /// </summary>
        clsDataAccess dataAccess = new clsDataAccess();

        /// <summary>
        /// Holds all of the SQL statements
        /// </summary>
        clsSQL sql = new clsSQL();
        #endregion

        #region constructor
        public clsSearch()
        {
            this.dataAccess = new clsDataAccess();

            this.sql = new clsSQL();
        }
#endregion

        #region public methods
        /// <summary>
        /// Returns a list of all of the invoices in the database
        /// </summary>
        /// <returns></returns>
        public List<clsInvoice> GetInvoices()
        {
            List<clsInvoice> result = new List<clsInvoice>();

            try
            {
                int iRet = 0;
                DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoices(), ref iRet);
                for (int i = 0; i < iRet; ++i)
                {
                    clsInvoice invoice = new clsInvoice();
                    invoice.InvoiceNum = ds.Tables[0].Rows[i][0].ToString();
                    invoice.InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[i][1].ToString()).Date;
                    invoice.TotalCharge = Convert.ToDecimal(ds.Tables[0].Rows[i][2].ToString());
                    result.Add(invoice);
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all of the distinct invoice numbers in the database
        /// </summary>
        /// <returns>List of invoice numbers</returns>
        public List<String> GetInvoiceNums()
        {
            List<String> result = new List<String>();

            try
            {
                int iRet = 0;
                DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoiceNumbers(), ref iRet);
                for (int i = 0; i < iRet; ++i)
                {
                    result.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all of the distinct invoice dates in the database
        /// </summary>
        /// <returns>List of invoice dates</returns>
        public List<String> GetInvoiceDates()
        {
            List<String> result = new List<String>();

            try
            {
                int iRet = 0;
                DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoiceDate(), ref iRet);
                for (int i = 0; i < iRet; ++i)
                {
                    result.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Returns a list of all of the distinct total costs for an invoice in the database
        /// </summary>
        /// <returns>List total costs</returns>
        public List<String> GetTotalCost()
        {
            List<String> result = new List<String>();

            try
            {
                int iRet = 0;
                DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoiceTotalCost(), ref iRet);
                for (int i = 0; i < iRet; ++i)
                {
                    result.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

            return result;
        }
#endregion
    }
}
