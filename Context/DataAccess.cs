using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollTool.Context
{
    public class DataAccess
    {
        private IConfiguration _configuration;

        public DataAccess(IConfiguration configuration) {
            _configuration = configuration;
        }

        private SqlConnection getCnx()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = _configuration.GetConnectionString("cnxPRD");

            return con;
        }

        private DataSet _DoCreateDataSet(SqlCommand myCmd)
        {
            SqlDataAdapter myAdap = new SqlDataAdapter();
            myAdap.SelectCommand = myCmd;

            try
            {
                DataSet data = new DataSet();
                myCmd.Connection = getCnx();
                if (myCmd.Connection.State != ConnectionState.Open)
                    myCmd.Connection.Open();

                myAdap.Fill(data);

                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (myCmd.Connection.State == ConnectionState.Open)
                    myCmd.Connection.Close();
            }
        }

        public Boolean HasInfo(DataSet ds)
        {
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()))
                            return true;
                    }
                }
            }
            return false;
        }

        public DataSet GetTransactionLog(DateTime pDate)
        {
            SqlCommand mycmd = new SqlCommand();

            try
            {
                mycmd.CommandText = "sp_GetTransactionLog";
                mycmd.CommandType = CommandType.StoredProcedure;

                mycmd.Parameters.Add(new SqlParameter("@p_date", pDate));

                return _DoCreateDataSet(mycmd);
            }
            catch
            {
                throw;
            }
        }

        public DataSet GetPayrollResult(DateTime startDate, DateTime endDate)
        {
            SqlCommand mycmd = new SqlCommand();

            try
            {
                mycmd.CommandText = "sp_GetPayrollResult";
                mycmd.CommandType = CommandType.StoredProcedure;

                mycmd.Parameters.Add(new SqlParameter("@p_stdate", startDate));
                mycmd.Parameters.Add(new SqlParameter("@p_enddate", endDate));

                return _DoCreateDataSet(mycmd);
            }
            catch
            {
                throw;
            }
        }
    }
}
