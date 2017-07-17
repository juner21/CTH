using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Sharp.ORM
{
    public static partial class SqlHelper
    {
        private static string CurrentDB;
        private static string[] ListDB;


        static SqlHelper()
        {
            var r = ConfigurationManager.AppSettings["SharpORMDB"].ToString();

            if (string.IsNullOrWhiteSpace(r))
                return;

            ListDB = r.Split(',');
            CurrentDB = ListDB[0];
        }
        

        public static string ConnStr(string dbSuffix) {

            var dome = "Data Source=47.92.134.179;Initial Catalog={0};Persist Security Info=True;User ID=acer";
            if (string.IsNullOrWhiteSpace(dbSuffix))
                return string.Format(dome, CurrentDB);

            foreach (var item in ListDB)
            {
                if (item.EndsWith(dbSuffix))
                    return string.Format(dome, item);
            }

            return string.Format(dome, CurrentDB);
        }

        public static int ExecuteNonQuery(string cmdText,string dbSuffix, params SqlParameter[] cmdParms)
        {
            return ExecuteNonQuery(CommandType.Text, dbSuffix, cmdText, cmdParms);

        }


        public static int ExecuteNonQuery(CommandType cmdType, string dbSuffix, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(ConnStr(dbSuffix)))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }

        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string dbSuffix, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static object ExecuteScalar(string cmdText, string dbSuffix, params SqlParameter[] cmdParms)
        {
            return ExecuteScalar(CommandType.Text, dbSuffix,cmdText, cmdParms);

        }

        public static object ExecuteScalar(CommandType cmdType, string dbSuffix, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(ConnStr(dbSuffix)))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string dbSuffix, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr(dbSuffix)))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                    return ds;
                }
            }

            finally
            {
                ds.Dispose();
            }
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string dbSuffix, string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr(dbSuffix)))
                {
                    cmd.Connection = conn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);
                    return ds;
                }
            }

            finally
            {
                ds.Dispose();
            }
        }
        

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

    }
}
