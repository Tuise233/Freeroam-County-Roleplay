using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public class TypeConverStruct
    {
        //类型名
        public string Name;
        //类型
        public Type Type;
        //描述
        public string Discription;

        //字符串转类型
        public Func<string, bool> StringToType;
        //类型转字符串
        public Func<Type, bool> TypeToString;


        public TypeConverStruct(string name, string discription, Type type)
        {
            Name = name;
            Discription = discription;
            Type = type;
            StringToType = (s) => s.ToLower() == Name.ToLower();
            TypeToString = (t) => t == Type;
        }

        public TypeConverStruct(string name, string discription, Type type, Func<string, bool> stringToType, Func<Type, bool> typeToString)
        {
            Name = name;
            Discription = discription;
            Type = type;
            StringToType = stringToType;
            TypeToString = typeToString;
        }
    }
}
