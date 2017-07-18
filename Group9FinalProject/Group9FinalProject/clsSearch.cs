using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group9FinalProject
{
    class clsSearch
    {
        clsDataAccess dataAccess = new clsDataAccess();
        clsSQL sql = new clsSQL();

        public List<String> GetInvoices()
        {
            List<String> result = new List<String>();

            int iRet = 0;
            DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoices(), ref iRet);
            for (int i = 0; i < iRet; ++i) {
                result.Add(ds.Tables[0].Rows[i][0].ToString() + " " + ds.Tables[0].Rows[i].ItemArray[1].ToString());
            }

            return result;
        }

        public List<String> GetInvoiceNums()
        {
            List<String> result = new List<String>();

            int iRet = 0;
            DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoiceNumbers(), ref iRet);
            for (int i = 0; i < iRet; ++i)
            {
                result.Add(ds.Tables[0].Rows[i][0].ToString());
            }

            return result;
        }

        public List<String> GetInvoiceDates()
        {
            List<String> result = new List<String>();

            int iRet = 0;
            DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoiceDate(), ref iRet);
            for (int i = 0; i < iRet; ++i)
            {
                result.Add(ds.Tables[0].Rows[i][0].ToString());
            }

            return result;
        }

        public List<String> GetTotalCost()
        {
            List<String> result = new List<String>();

            int iRet = 0;
            DataSet ds = dataAccess.ExecuteSQLStatement(sql.SelectAllInvoiceTotalCost(), ref iRet);
            for (int i = 0; i < iRet; ++i)
            {
                result.Add(ds.Tables[0].Rows[i][0].ToString());
            }

            return result;
        }
    }
}
