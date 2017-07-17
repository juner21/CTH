using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.ORM
{
    partial class ORMOperator
    {

        protected internal static DataTable QueryPaging(object o,string dbSuffix, int ipageindex, int ipagesize, out int ireccount, out int ipagecount)
        {

            Type type = o.GetType();
            PropertyInfo[] p = type.GetProperties();
            //表名
            string TableName = ORMHelper.GetTableName(type);

            List<KeyValue> list = new List<KeyValue>();
            //获得所有的字段
            foreach (PropertyInfo item in p)
            {
                if (item.GetValue(o) == null)
                    continue;

                if (ORMHelper.IsIgnoreField(item, o))
                    continue;

                if (item.IsDefined(typeof(KeyFieldAttribute), false) == true)
                {
                    //主键
                    list.Add(ORMHelper.GetKeyFieldValue(item, o));
                    continue;
                }


                //非主键
                list.Add(ORMHelper.GetFieldValue(item, o));

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("select * from " + TableName + " where 1=1 and ");

            sb.Append(string.Join(" and ", list.Select(e => e.Key + " =@" + e.Key).ToList()));

            SqlParameter[] SqlParam = list.ConvertAll(e => new SqlParameter("@" + e.Key, e.Value)).ToArray();

            return SqlHelper.QueryPaging(sb.ToString(),dbSuffix, ipageindex, ipagesize, out ireccount, out ipagecount, SqlParam);
        }

        protected internal static DataTable QueryPagingBySql(string sql,string dbSuffix, int ipageindex, int ipagesize, out int ireccount, out int ipagecount, object sqlParam)
        {
            //获取属性
            PropertyInfo[] p = sqlParam.GetType().GetProperties();

            SqlParameter[] SqlParam = p.ToList().ConvertAll(e => new SqlParameter("@" + e, e)).ToArray();

            return SqlHelper.QueryPaging(sql, dbSuffix, ipageindex, ipagesize, out ireccount, out ipagecount, SqlParam);
        }

    }
}
