using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.ORM
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute :Attribute
    {
        public string TableName { get; set; }

        public TableAttribute(string tableName) {
            this.TableName = tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class KeyFieldAttribute : Attribute
    {
        /// <summary>
        /// 主键名
        /// </summary>
        public string KeyFieldName { get; set; }
        
        public KeyFieldAttribute()
        {
        }

        public KeyFieldAttribute(string keyFieldName)
        {
            this.KeyFieldName = keyFieldName;
        }
        
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute { 
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        
        public FieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreFieldAttribute : Attribute
    {
    }
}
