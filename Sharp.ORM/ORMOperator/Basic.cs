using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
namespace Sharp.ORM
{
    partial class ORMOperator
    {

        protected internal static int Add(object o,string dbSuffix)
        {
            Type type = o.GetType();
            PropertyInfo[] p = type.GetProperties();
            //表名
            string TableName = ORMHelper.GetTableName(type);

            StringBuilder sb = new StringBuilder();
            string Paramter = string.Empty;

            sb.Append(" Insert into " + TableName);
            sb.Append(" ( ");

            List<KeyValue> list = new List<KeyValue>();

            //获得所有的字段            
            foreach (PropertyInfo item in p)
            {
                //无值
                if(item.GetValue(o) == null)
                    continue;

                if (ORMHelper.IsIgnoreField(item, o))
                    continue;

                if (item.IsDefined(typeof(KeyFieldAttribute), false) == true) {
                    //主键
                    list.Add(ORMHelper.GetKeyFieldValue(item, o));
                    continue;
                }
                
                //非主键
                list.Add(ORMHelper.GetFieldValue(item, o));

            }

            //Insert字段
            sb.Append(string.Join(",",list.Select(e =>e.Key).ToList()));

            sb.Append(" ) values ( ");

            //Insert值
            sb.Append(string.Join(",", list.Select(e =>"@"+e.Key).ToList()));

            sb.Append(" ) ");

            SqlParameter[] SqlParam = list.ConvertAll(e => new SqlParameter("@"+e.Key, e.Value)).ToArray();

            return SqlHelper.ExecuteNonQuery(sb.ToString(),dbSuffix, SqlParam);
        }

        protected internal static int Delete(object o, string dbSuffix)
        {
            Type type = o.GetType();
            PropertyInfo[] p = type.GetProperties();

            //表名
            string TableName = ORMHelper.GetTableName(type);


            //寻找主键
            string KeyField = string.Empty;
            string KeyFieldValue = string.Empty;
            foreach (PropertyInfo item in p)
            {

                if (item.IsDefined(typeof(KeyFieldAttribute), false) == true) {
                    KeyFieldAttribute keyFieldArrt = Attribute.GetCustomAttribute(item, typeof(KeyFieldAttribute)) as KeyFieldAttribute;

                    if (item.GetValue(o) == null)
                    {
                        throw new Exception("Delete方法中主键不能为Null。");
                    }

                    KeyField = string.IsNullOrWhiteSpace(keyFieldArrt.KeyFieldName) ? item.Name : keyFieldArrt.KeyFieldName;
                    KeyFieldValue = item.GetValue(o).ToString();
                    break;
                }
            } 

            if (string.IsNullOrWhiteSpace(KeyField) || string.IsNullOrWhiteSpace(KeyFieldValue))
                throw new Exception("没有找到主键。");

            SqlParameter[] SqlParam = new SqlParameter[] { new SqlParameter("@"+KeyField, KeyFieldValue) };

            string sql = "delete from " + TableName + " where " + KeyField + " = @" + KeyField;

            return SqlHelper.ExecuteNonQuery(sql, dbSuffix, SqlParam);
        }

        protected internal static int Update(object o, string dbSuffix)
        {
            Type type = o.GetType();
            PropertyInfo[] p = type.GetProperties();

            //表名
            string TableName = ORMHelper.GetTableName(type);


            StringBuilder sb = new StringBuilder();
            string Paramter = string.Empty;

            sb.Append(" Update  " + TableName);
            sb.Append(" set ");

            //准备字段
            List<KeyValue> list = new List<KeyValue>();
            string KeyField = string.Empty;
            string KeyFieldValue = string.Empty;
            //获得所有的字段
            foreach (PropertyInfo item in p)
            {
                if (item.GetValue(o) == null)
                    continue;

                if (ORMHelper.IsIgnoreField(item, o))
                    continue;

                if (item.IsDefined(typeof(KeyFieldAttribute), false) == true) {
                    KeyFieldAttribute keyFieldArrt = Attribute.GetCustomAttribute(item, typeof(KeyFieldAttribute)) as KeyFieldAttribute;

                    //主键
                    KeyField = string.IsNullOrWhiteSpace(keyFieldArrt.KeyFieldName) ? item.Name : keyFieldArrt.KeyFieldName;
                    KeyFieldValue = item.GetValue(o).ToString();
                    continue;
                }

                list.Add(ORMHelper.GetFieldValue(item, o));
            }

            //异常情况排查
            if (list.Count == 0)
                throw new Exception("你没有给任何非主键字段赋值。");

            if (string.IsNullOrWhiteSpace(KeyField) || string.IsNullOrWhiteSpace(KeyFieldValue))
                throw new Exception("没有找到主键，或主键未赋值。");

            //拼写Update字段项
            sb.Append(string.Join(",", list.Select(e => e.Key + " = @" + e.Key).ToArray()));

            sb.Append(" where ");
            sb.Append(KeyField + " = @" + KeyField);

            //拼写完了条件才能添加主键的值
            list.Add(new KeyValue(KeyField, KeyFieldValue));

            SqlParameter[] SqlParam = list.ConvertAll(e => new SqlParameter("@"+e.Key, e.Value)).ToArray();

            return SqlHelper.ExecuteNonQuery(sb.ToString(), dbSuffix, SqlParam);
        }

        protected internal static List<T> SimpleSelect<T>(object o, string dbSuffix) where T : class,new()
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

                if (item.IsDefined(typeof(KeyFieldAttribute), false) == true) {
                    //主键
                    list.Add(ORMHelper.GetKeyFieldValue(item, o));
                    continue;
                }
                
                //非主键
                list.Add(ORMHelper.GetFieldValue(item, o));

            }

            if(list.Count <1)
                throw new Exception("调用SimpleSelect方法时，至少要给一个字段赋值过。");

            StringBuilder sb = new StringBuilder();
            sb.Append("select * from " + TableName + " where ");
            sb.Append(string.Join(" and ",list.Select(e => e.Key +" =@"+e.Key).ToList()));

            SqlParameter[] SqlParam = list.ConvertAll(e => new SqlParameter("@" + e.Key, e.Value)).ToArray();

            var ds = SqlHelper.ExecuteDataSet(CommandType.Text,dbSuffix, sb.ToString(), SqlParam);
            
            return ORMHelper.DataTableToObj<T>(ds.Tables[0], p);
        }

        protected internal static T SimpleSelectByKey<T>(object o, string dbSuffix) where T : class,new()
        {
            Type type = o.GetType();
            PropertyInfo[] p = type.GetProperties();
            //表名
            string TableName = ORMHelper.GetTableName(type);

            string sql = "select * from "+TableName;

            KeyValue ky = null;
            //获得所有的字段
            foreach (PropertyInfo item in p)
            {
                if (item.IsDefined(typeof(KeyFieldAttribute), false) != true)
                    continue;

                if (ORMHelper.IsIgnoreField(item, o))
                    continue;
                
                //主键未赋值过，抛异常
                if (item.GetValue(o) == null)
                    throw new Exception("调用SimpleSelectByKey方法时，主键必须赋值。");

                ky = ORMHelper.GetKeyFieldValue(item, o);
                break;
            }

            if(ky == null)
                throw new Exception("调用SimpleSelectByKey方法时，未找到主键。");

            sql += " where " + ky.Key + " = @" + ky.Key;

            SqlParameter[] SqlParam = new SqlParameter[] { new SqlParameter("@" + ky.Key, ky.Value) };

            var ds = SqlHelper.ExecuteDataSet(CommandType.Text, dbSuffix, sql, SqlParam);
            
            return ORMHelper.GetObj<T>(ds.Tables[0], p);
        }

        protected internal static List<T> SelectAllTableByObj<T>() where T : class,new()
        {
            var type = typeof(T);
            string sql = "Select * From " + ORMHelper.GetTableName(type);

            var ds = SqlHelper.ExecuteDataSet(CommandType.Text, string.Empty, sql);
            
            return ORMHelper.DataTableToObj<T>(ds.Tables[0], type.GetProperties());

        }

        protected internal static DataTable SelectAllTableByDataTable<T>() where T : class,new()
        {
            var type = typeof(T);
            string sql = "Select * From " + ORMHelper.GetTableName(type);

            var ds = SqlHelper.ExecuteDataSet(CommandType.Text, string.Empty, sql);

            return ds.Tables[0];

        }


    }
}
