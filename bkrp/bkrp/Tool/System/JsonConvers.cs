using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace bkrp
{
    public interface IJsonParser
    {
        System.Type ParserType { get; }
        string ConvertoJson(object obj);
    }

    public static class JsonConvers
    {
        public static Dictionary<System.Type, IJsonParser> Parser = new Dictionary<System.Type, IJsonParser>();

        /// <summary>
        /// 设置解析器
        /// </summary>
        public static void SetJsonParser(IJsonParser jsonParser)
        {
            Parser[jsonParser.ParserType] = jsonParser;
        }

        /// <summary>
        /// 嵌套对象,entityName
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static string ConvertJson(List<object> lists, string entityName)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                //当传进来的entityName=Emtpty的时候，随机获取lists[index]项的类型
                if (entityName.Equals(string.Empty))
                {
                    object obj = lists[0];
                    entityName = obj.GetType().ToString();
                }
                result.Append($"{{\"{entityName}\":");
                result.Append(ListToJson(lists));
                result.Append('}');
                return result.ToString();

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return "Conver Error";
            }
        }



        public static string ToJson(this object obj)
        {
            int deep = 0;
            return ObjectToJson(obj, ref deep);
        }
        public static string ObjectToJson(object item, ref int deep)
        {
            try
            {
                if (item == null)
                {
                    return "null";
                }
                var tt = item.GetType();
                if (Parser.ContainsKey(tt))
                {
                    try
                    {
                        return Parser[tt].ConvertoJson(item);
                    }
                    catch (Exception ex)
                    {
                        Log.Server($"JSON解析器 [{tt.Name}] 无法正常解析:{ex.Message}");
                    }
                }
                if (tt == typeof(string))//字符串
                {
                    return $"\"{item.ToString()}\"";
                }
                if (tt == typeof(bool))//布尔值
                {
                    return ((bool)item) ? "true" : "false";
                }
                if (!tt.IsClass)//不是类类型
                {
                    return item.ToString();
                }
                else //是类类型对象
                {
                    //是容器  普通类类型数组解析
                    if (item is IList<object>)
                    {
                        return ListToJson((item as IList<object>));
                    }
                    else
                    if (item is IDictionary<object, object>)
                    {
                        return DictionaryToJson((item as IDictionary<object, object>));
                    }
                    else
                    if (item is IEnumerable<object>)
                    {
                        return EnumerableToJson((item as IEnumerable<object>));
                    }
                    else //增加Int类型的解析
                    if (item is IList<int>)
                    {
                        return ListToJson((item as IList<int>));
                    }
                    else
                    if (item is IDictionary<int, object>)
                    {
                        return DictionaryToJson((item as IDictionary<int, object>));
                    }
                    else
                    if (item is IEnumerable<int>)
                    {
                        return EnumerableToJson((item as IEnumerable<int>));
                    }
                    else
                    {
                        try
                        {
                            //是对象
                            return ClassJson(item, ref deep);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("不能转换此种类型至JSON, 请补全解析检测, 或使用装箱模式 在JsonConvers.cs 102" + item.GetType().Name);
                            Log.Error(ex.Message);
                            return item.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Server("ObjectToJson Error" + ex.Message);
                return "";
            }
        }
        public static string ListToJson<T>(IList<T> lists)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                result.Append('[');
                foreach (var item in lists)
                {
                    int deep = 0;
                    result.Append(ObjectToJson(item, ref deep) + ",");
                }
                if (result[result.Length - 1] == ',')
                    result.Remove(result.Length - 1, 1);
                result.Append(']');
                return result.ToString();

            }
            catch (Exception ex)
            {
                Log.Server("ListToJson Error: " + ex.Message);
                return "";
            }
        }
        public static string DictionaryToJson<T, T2>(IDictionary<T, T2> lists)
        {
            try
            {
                StringBuilder result = new StringBuilder("{");
                foreach (var item in lists)
                {
                    int deep = 0;
                    result.Append($"\"{item.Key.ToString()}\":{ObjectToJson(item.Value, ref deep)},");
                }
                if (result[result.Length - 1] == ',')
                    result.Remove(result.Length - 1, 1);
                result.Append('}');
                return result.ToString();

            }
            catch (Exception ex)
            {
                Log.Server("DictionaryToJson Error: " + ex.Message);
                return "";
            }
        }
        public static string EnumerableToJson<T>(IEnumerable<T> lists)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                result.Append('[');
                foreach (var item in lists)
                {
                    int deep = 0;
                    result.Append(ObjectToJson(item, ref deep) + ",");
                }
                if (result[result.Length - 1] == ',')
                    result.Remove(result.Length - 1, 1);
                result.Append(']');
                return result.ToString();

            }
            catch (Exception ex)
            {
                Log.Server("EnumerableToJson Error: " + ex.Message);
                return "";
            }
        }
        public static string ClassJson(object item, ref int deep)
        {
            try
            {
                deep += 1;
                if (deep > 10)
                {
                    Log.Error("克隆深度超过极限");
                    return "Too Deep!";
                }
                StringBuilder result = new StringBuilder("{");
                PropertyInfo[] parms = item.GetType().GetProperties();
                foreach (var p in parms)//获取所有对象
                {
                    if (p.GetCustomAttribute(typeof(JsonConverIgnoreAttribute)) != null) continue;
                    var value = p.GetValue(item);
                    result.Append("\"" + p.Name.ToString() + "\":" + ObjectToJson(value, ref deep) + ',');
                }
                FieldInfo[] parmsf = item.GetType().GetFields();
                foreach (var p in parmsf)
                {
                    if (p.GetCustomAttribute(typeof(JsonConverIgnoreAttribute)) != null) continue;
                    var value = p.GetValue(item);
                    result.Append("\"" + p.Name.ToString() + "\":" + ObjectToJson(value, ref deep) + ',');
                }
                if (result[result.Length - 1] == ',')
                    result.Remove(result.Length - 1, 1);
                result.Append('}');
                return result.ToString();
            }
            catch (Exception ex)
            {
                Log.Server("ListToJson Error" + ex.Message);
                return "";
            }
        }
    }

    public class JsonConverIgnoreAttribute : Attribute
    {
        public JsonConverIgnoreAttribute()
        {

        }

    }
}
