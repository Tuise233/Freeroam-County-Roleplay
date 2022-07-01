using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public interface IJsonReader
    {
        System.Type ReaderType { get; }
        object JsontoObject(string json);
    }
    public static class JsonReader
    {
        public static void SetJsonReader(IJsonReader reader)
        {
            Reader.Add(reader.ReaderType, reader);
        }
        public static Dictionary<Type, IJsonReader> Reader { get; } = new Dictionary<Type, IJsonReader>();

        public static T Read<T>(string json, bool usereader = true)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Log.Error($"json 对象 {typeof(T).Name} 解析错误 空白!");
                return default;
            }
            try
            {
                if (usereader && Reader.ContainsKey(typeof(T)))
                {
                    var obj = (T)Reader[typeof(T)].JsontoObject(json);
                    return obj;
                }
                else
                {
                    T obj = JsonMapper.ToObject<T>(json);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"json 对象 {typeof(T).Name} 解析错误! \n{ex}");
                return default;
            }
        }
        public static List<T> ReadList<T>(string json, bool usereader = true, Predicate<T> valid = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                Log.Error($"jsonList 对象{typeof(T).Name} 解析错误 空白!");
                return default;
            }
            Log.Server("读取JSON列表:");
            List<T> List = new List<T>();
            int count = 0;
            int miss = 0;
            try
            {
                JsonData data = JsonMapper.ToObject(json);
                for (int i = 0; i < data.Count; i++)
                {
                    try
                    {
                        T obj = Read<T>(data[i].ToJson(), usereader);
                        if (valid != null && !valid(obj))
                        {
                            miss++;
                        }
                        else
                        {
                            List.Add(obj);
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"json 对象{typeof(T).Name} 解析错误!\n{ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("json解析错误!\n{ex}");
                Log.Error(json);
            }
            Log.Loading($"JSON ReadList<{(typeof(T)).Name}> Load Count: {count}");
            if (valid != null)
                Log.Loading($"Miss {miss}");
            return List;
        }
    }
}
