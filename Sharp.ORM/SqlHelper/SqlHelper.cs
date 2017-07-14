using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Sharp.ORM
{
    public partial class SqlHelper
    {
        // public static readonly string ConnStr = ConfigurationSettings.AppSettings["ConnStr"].ToString();
        public static readonly string ConnStr = "Data Source = KOFHAN\\SQLEXPRESS;Initial Catalog = TEST; Integrated Security = True";

        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, cmdParms);

        }


        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }

        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteScalar(CommandType.Text, cmdText, cmdParms);

        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
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

        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
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

        public static DataSet ExecuteDataSet(string connstr, CommandType cmdType, string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
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
