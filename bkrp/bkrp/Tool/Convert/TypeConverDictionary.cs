using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public static class TypeConverDictionary
    {
        public static readonly List<TypeConverStruct> Converts = new List<TypeConverStruct>()
            {
                new TypeConverStruct("Bool",      "[布尔值 true 或 false]",          Type.GetType("System.Boolean", true, true)),
                new TypeConverStruct("Byte",      "[字节值 0 ~ 255]",               Type.GetType("System.Byte", true, true)),
                new TypeConverStruct("SByte",     "[有符号字节值 -128 ~ 127 ]",      Type.GetType("System.SByte", true, true)),
                new TypeConverStruct("Char",      "[字符]",                        Type.GetType("System.Char", true, true)),
                new TypeConverStruct("Decimal",   "[超高精度浮点数]",               Type.GetType("System.Decimal", true, true)),
                new TypeConverStruct("Double",    "[高精度浮点数]",              Type.GetType("System.Double", true, true)),
                new TypeConverStruct("Float",     "[浮点数]",                    Type.GetType("System.Single", true, true)),
                new TypeConverStruct("Int",       "[整数]",                      Type.GetType("System.Int32", true, true)),
                new TypeConverStruct("UInt",      "[无符号整数]",                 Type.GetType("System.UInt32", true, true)),
                new TypeConverStruct("Long",      "[长字节整数]",                 Type.GetType("System.Int64", true, true)),
                new TypeConverStruct("ULong",     "[无符号长字节整数]",                Type.GetType("System.UInt64", true, true)),
                new TypeConverStruct("Short",     "[短字节整数]",                    Type.GetType("System.Int16", true, true)),
                new TypeConverStruct("UShort",    "[无符号短字节整数]",                 Type.GetType("System.UInt16", true, true)),

                new TypeConverStruct("Object",    "[引用类型对象]",                Type.GetType("System.Object", true, true)),
                new TypeConverStruct("String",    "[字符串]",                   Type.GetType("System.String", true, true)),
                new TypeConverStruct("DateTime",  "[时间格式",                   Type.GetType("System.DateTime", true, true)),
                new TypeConverStruct("Guid",      "[序列号]",                   Type.GetType("System.Guid", true, true)),

                new TypeConverStruct("Array",     "[列表]",                   Type.GetType("System.Object[]", true, true),
                    (s)=>{ s= s.ToLower(); return s == "array" || s == "list"; },
                    (t)=>{ return t.IsArray; }),
                new TypeConverStruct("Type",      "[数据类型]",                 typeof(Type),
                    (s)=>{ return s.ToLower() == "type"; },
                    (t)=>{ return t == typeof(Type); })

            };



        /// <summary>
        /// 从字符串转换类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeByString(string type)
        {
            foreach (var item in TypeConverDictionary.Converts)
            {
                if (item.StringToType(type))
                {
                    return item.Type;
                }
            }
            return Type.GetType(type, true, true);
        }

        /// <summary>
        /// 从类型转字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetStringByType(Type type)
        {
            foreach (var item in TypeConverDictionary.Converts)
            {
                if (item.TypeToString(type))
                {
                    return item.Name;
                }
            }
            return type.Name;
        }

        /// <summary>
        /// 获得类型的描述
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string GetTypeDiscription(string typeName)
        {
            foreach (var item in TypeConverDictionary.Converts)
            {
                if (item.Name == typeName)
                {
                    return item.Discription.Trim(new char[] { '[', ']' });
                }
            }
            return typeName;
        }

    }
}
