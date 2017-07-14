using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp.ORM
{
    public class KeyValue
    {
        public KeyValue(string key, object value)
        {
            Key =  key;
            Value = value;
        }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
