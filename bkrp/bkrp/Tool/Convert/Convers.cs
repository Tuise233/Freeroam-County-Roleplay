using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public static class Convers
    {
        //字符串是否能转换为该类型
        public static bool Conver<T>(this string str)
        {
            T obj;
            try
            {
                var raw = FormatString(str, typeof(T), out var result);
                if (!result) return false;
                if (raw == null)
                    obj = default;
                else obj = (T)raw;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + typeof(T).Name);
                return false;
            }
            return true;
        }
        public static bool Conver(this string str, Type T)
        {
            object obj;
            try
            {
                obj = FormatString(str, T, out var result);
                if (!result) return false;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + T.Name);
                return false;
            }
            return true;
        }





        //字符串转换为该类型,默认为default
        public static T ConverTo<T>(this string str, out bool result, T defaults)
        {
            result = true;
            T obj;
            try
            {
                var raw = FormatString(str, typeof(T), out result);
                if (!result)
                    return defaults;
                if (raw == null)
                    obj = defaults;
                else obj = (T)raw;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + typeof(T).Name);
                result = false;
                return defaults;
            }
            return obj;
        }
        public static object ConverTo(this string str, Type T, out bool result, object defaults)
        {
            result = true;
            object obj;
            try
            {
                obj = FormatString(str, T, out result);
                if (!result)
                    return defaults;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + T.Name);
                result = false;
                return defaults;
            }
            return obj;
        }


        //字符串转换为该类型,默认为default
        public static T ConverTo<T>(this string str, out bool result)
        {
            result = true;
            T obj;
            try
            {
                var raw = FormatString(str, typeof(T), out result);
                if (!result)
                    return default;
                if (raw == null)
                    obj = default;
                else obj = (T)raw;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + typeof(T).Name);
                result = false;
                return default;
            }
            return obj;
        }
        public static object ConverTo(this string str, Type T, out bool result)
        {
            result = true;
            object obj;
            try
            {
                obj = FormatString(str, T, out result);
                if (!result)
                    return default;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + T.Name);
                result = false;
                return default;
            }
            return obj;
        }




        //字符串转换为该类型,默认为default
        public static T ConverTo<T>(this string str, T defaults)
        {
            T obj;
            try
            {
                var raw = FormatString(str, typeof(T), out var result);
                if (!result)
                    return defaults;
                if (raw == null)
                    obj = default;
                else obj = (T)raw;
            }
            catch (Exception exp)
            {
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + typeof(T).Name);
                return defaults;
            }
            return obj;
        }
        public static object ConverTo(this string str, Type T, object defaults)
        {
            object obj;
            try
            {
                obj = FormatString(str, T, out var result);
                if (!result)
                    return defaults;
            }
            catch (Exception exp)
            {
                obj = defaults;
                //Log.Warning(L.Msg("type_conver_error    ") + exp.Message + $"\n{exp.StackTrace}" + "  to  " + T.Name);
                return defaults;
            }
            return obj;
        }



        /// <summary>
        /// 将字符串格式化成指定的数据类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Object FormatString(String str, Type type, out bool result)
        {
            result = true;
            if (String.IsNullOrEmpty(str))
            {
                if (type != typeof(string))
                    return null;
                else
                {
                    result = false;
                    return null;
                }
            }
            if (type == null)
                return str;
            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                String[] strs = str.Split(new char[] { ',' });
                Array array = Array.CreateInstance(elementType, strs.Length);
                for (int i = 0, c = strs.Length; i < c; ++i)
                {
                    var v = ConvertSimpleType(strs[i], elementType, out result);
                    if (!result)
                        return null;
                    array.SetValue(v, i);
                }
                return array;
            }
            return ConvertSimpleType(str, type, out result);
        }

        /// <summary>
        /// 对象类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        private static object ConvertSimpleType(string value, Type destinationType, out bool result)
        {
            result = true;
            object returnValue;
            if (value == null && destinationType != typeof(string))
            {
                result = false;
                return value;
            }
            if (destinationType.IsInstanceOfType(value))
            {
                return value;
            }
            string str = value as string;
            if ((str != null) && (str.Length == 0))
            {
                return null;
            }
            //如果目标是 类型
            if (destinationType == typeof(Type))
            {
                Type obj;
                try
                {
                    obj = TypeConverDictionary.GetTypeByString(str);
                }
                catch (Exception)
                {
                    //Log.Warning(L.Msg("type_cant_find  ") + str);
                    result = false;
                    return null;
                }
                return obj;
            }
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                //Log.Warning(L.Msg("type_cant_conver  ") + value.ToString() + "!=" + destinationType);
                result = false;
                return null;
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception ex)
            {
                //Log.Warning(L.Msg("type_conver_error  ") + value?.ToString() + "=>" + destinationType.ToString());
                result = false;
                return null;
            }
            return returnValue;
        }
    }
}
