using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.ORM
{
    static class ORMHelper
    {



        internal static string GetTableName(Type t)
        {
            if(t.IsDefined(typeof(TableAttribute),false) != true)
                return t.Name;

            TableAttribute attr = Attribute.GetCustomAttribute(t, typeof(TableAttribute)) as TableAttribute;
            return attr.TableName;
        }

        internal static List<T> DataTableToObj<T>(DataTable dt, PropertyInfo[] property) where T : class,new() 
        {
            if (dt == null || dt.Rows.Count < 1)
                return new List<T>();

            List<T> list = new List<T>();

            T obj = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj = new T();
                for (int j = 0; j < property.Length; j++)
                {
                    //直接列名和属性对应上，赋值
                    if (dt.Columns.Contains(property[j].Name))
                        property[j].SetValue(obj, dt.Rows[i][property[j].Name]);

                    //主键改名
                    if (property[j].IsDefined(typeof(KeyFieldAttribute), false) == true)
                    {
                        //主键
                        KeyFieldAttribute keyFieldArrt = Attribute.GetCustomAttribute(property[j], typeof(KeyFieldAttribute)) as KeyFieldAttribute;

                        if (keyFieldArrt != null && !string.IsNullOrWhiteSpace(keyFieldArrt.KeyFieldName))
                            property[j].SetValue(obj,dt.Rows[i][keyFieldArrt.KeyFieldName]);

                        continue;
                    }


                    //非主键改名
                    if (property[j].IsDefined(typeof(FieldAttribute), false) == true)
                    {
                        FieldAttribute fieldFieldArrt = Attribute.GetCustomAttribute(property[j], typeof(FieldAttribute)) as FieldAttribute;

                        if (fieldFieldArrt != null && !string.IsNullOrWhiteSpace(fieldFieldArrt.FieldName))
                            property[j].SetValue(obj, dt.Rows[i][fieldFieldArrt.FieldName]);

                        continue;
                    }
                }
                list.Add(obj);

            }

            return list;
        }

        internal static T GetObj<T>(DataTable dt, PropertyInfo[] property) where T : class,new() {
            if (dt == null || dt.Rows.Count < 1)
                return new T();

            T obj = new T();

            for (int j = 0; j < property.Length; j++)
            {
                if (dt.Columns.Contains(property[j].Name))
                    property[j].SetValue(obj, dt.Rows[0][property[j].Name]);
            }

            return obj;
        }

        internal static KeyValue GetKeyFieldValue(PropertyInfo item,object o)
        {
            KeyFieldAttribute attr = Attribute.GetCustomAttribute(item, typeof(KeyFieldAttribute)) as KeyFieldAttribute;

            if (attr != null && !string.IsNullOrWhiteSpace(attr.KeyFieldName))
                return new KeyValue(attr.KeyFieldName,item.GetValue(o));

            return new KeyValue(item.Name, item.GetValue(o));
        }

        internal static KeyValue GetFieldValue(PropertyInfo item, object o)
        {
            FieldAttribute attr = Attribute.GetCustomAttribute(item, typeof(FieldAttribute)) as FieldAttribute;

            if (attr != null && !string.IsNullOrWhiteSpace(attr.FieldName))
                return new KeyValue(attr.FieldName, item.GetValue(o));

            return new KeyValue(item.Name, item.GetValue(o));
        }

        internal static bool IsIgnoreField(PropertyInfo item, object o)
        {
            IgnoreFieldAttribute attr = Attribute.GetCustomAttribute(item, typeof(IgnoreFieldAttribute)) as IgnoreFieldAttribute;

            return attr != null;            
        }
    }
}
