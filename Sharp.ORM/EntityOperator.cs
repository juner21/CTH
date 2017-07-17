using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.ORM
{
    public static class EntityOperator
    {
        public static int Add(object o,string dbSuffix)
        {
            if (o == null)
                return 0;
            return ORM.ORMOperator.Add(o,dbSuffix);
        }
        public static int Delete(object o, string dbSuffix)
        {
            if (o == null)
                return 0;
            return ORM.ORMOperator.Delete(o, dbSuffix);
        }
        public static int Update(object o, string dbSuffix)
        {
            if (o == null)
                return 0;
            return ORM.ORMOperator.Update(o, dbSuffix);
        }

        public static List<T> SimpleSelect<T>(object o, string dbSuffix) where T : class,new() 
        {
            if (o == null)
                return new List<T>();
            return ORM.ORMOperator.SimpleSelect<T>(o, dbSuffix);
        }

        public static T SimpleSelectByKey<T>(object o, string dbSuffix) where T : class,new()
        {
            if (o == null)
                return new T();
            return ORM.ORMOperator.SimpleSelectByKey<T>(o, dbSuffix);
        }

        public static List<T> SelectTableAllByObj<T>() where T : class,new()
        {
            return ORM.ORMOperator.SelectAllTableByObj<T>();
        }

        public static DataTable SelectAllTableByDataTable<T>() where T : class,new()
        {
            return ORM.ORMOperator.SelectAllTableByDataTable<T>();
        }
        
        public static DataTable QueryPaging<T>(object o,string dbSuffix, int ipageindex, int ipagesize, out int ireccount, out int ipagecount) where T : class,new()
        {
            return ORM.ORMOperator.QueryPaging(o, dbSuffix,  ipageindex,  ipagesize, out  ireccount, out  ipagecount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ipageindex"></param>
        /// <param name="ipagesize"></param>
        /// <param name="ireccount"></param>
        /// <param name="ipagecount"></param>
        /// <param name="sqlParam">请传入匿名类</param>
        /// <returns></returns>
        public static DataTable QueryPagingBySql(string sql,string dbSuffix, int ipageindex, int ipagesize, out int ireccount, out int ipagecount, object sqlParam)
        {
            return ORM.ORMOperator.QueryPagingBySql(sql, dbSuffix, ipageindex, ipagesize, out ireccount, out ipagecount,sqlParam);
        }
    }
}
