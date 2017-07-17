using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.ORM
{
    partial class SqlHelper
    {

        public static DataTable QueryPaging(string cmdText,string dbSuffix, int ipageindex, int ipagesize, out int ireccount, out int ipagecount, SqlParameter[] SqlParam)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            ireccount = 0;
            ipagecount = 0;
            using (SqlConnection conn = new SqlConnection(ConnStr(dbSuffix)))
            {
                DataTable datatable = new DataTable();
                try
                {
                    if (ipagesize <= 0)
                    {

                        PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, SqlParam);
                        adapter.SelectCommand = cmd;
                        adapter.Fill(datatable);
                        SqlHelper.GenerateOrderKey(datatable, 1);
                        return datatable;
                    }


                    ipageindex = ipageindex == 0 ? 1 : ipageindex;
                    SqlHelper.PagingRecordCount(cmd, conn, cmdText, ipageindex, ipagesize, out ireccount, out ipagecount, SqlParam);


                    cmd.CommandText = cmdText;
                    adapter.SelectCommand = cmd;

                    int istartindex = (ipageindex - 1) * ipagesize + 1;
                    adapter.Fill(istartindex - 1, ipagesize, datatable);
                    SqlHelper.GenerateOrderKey(datatable, istartindex);

                }
                catch (Exception e)
                {
                    throw e;
                }

                return datatable;
            }
        }

        public static void PagingRecordCount(SqlCommand cmd ,SqlConnection conn ,string cmdText, int ipageindex, int ipagesize, out int ireccount, out int ipagecount, SqlParameter[] SqlParam) {

            var index = cmdText.ToUpper().IndexOf("ORDER BY");
            if (index >= 0)
            {
                cmdText = cmdText.Remove(index, cmdText.Length - index);
            }

            var sSQL = "SELECT COUNT(0) AS RecordCount FROM (" + cmdText + ") AS Table1";
            PrepareCommand(cmd, conn, null, CommandType.Text, sSQL, SqlParam);
            cmd.CommandText = sSQL;

            ireccount = Convert.ToInt32(cmd.ExecuteScalar());

            if (ipagesize * (ireccount / ipagesize) == ireccount)
            {
                ipagecount = ireccount / ipagesize;
            }
            else
            {
                ipagecount = ireccount / ipagesize + 1;
            }
        }

        public static void GenerateOrderKey(DataTable table, int istartindex)
        {
            table.Columns.Add(new DataColumn("OrderKey"));
            foreach (DataRow row in table.Rows)
            {
                row["OrderKey"] = istartindex++;
            }
        }
    }
}
