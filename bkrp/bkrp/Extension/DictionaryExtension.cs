using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    public static class DictionaryExtension
    {
        public static string Trim(this string s)
        {
            return s.Trim();
        }

        public static T Get<T>(this Dictionary<string, string> dic, string id, T defaultValue = default)
        {
            id = Trim(id);
            if (dic != null && dic.TryGetValue(id, out string value))
            {
                return Trim(value).ConverTo<T>(defaultValue);
            }
            else
            {
                //if (Setting.Command.ExDebuging)
                    //Log.Error($"can not find key {id} in dic {(dic == null ? "Null" : $"{ dic.GetType().Name}{ dic.Count}")} by [Get <{typeof(T).Name}>]");
                return defaultValue;
            }
        }
        public static string Get(this Dictionary<string, string> dic, string id, string defaultValue = null)
        {
            id = Trim(id);
            if (dic != null && dic.TryGetValue(id, out string value))
            {
                return Trim(value);
            }
            else
            {
                //if (Setting.Command.ExDebuging)
                    //Log.Error($"can not find key {id} in dic {(dic == null ? "Null" : $"{ dic.GetType().Name}{ dic.Count}")} by [Get String]");
                return defaultValue;
            }
        }


        public static string At(this Dictionary<string, string> dic, string id, string defaultValue = null)
        {
            if (dic != null && dic.TryGetValue(id, out string value))
            {
                return value;
            }
            else
            {
                //if (Setting.Command.ExDebuging)
                    //Log.Error($"can not find key {id} in dic {(dic == null ? "Null" : $"{ dic.GetType().Name}{ dic.Count}")} by [At String]");
                return defaultValue;
            }
        }
        public static T At<T>(this Dictionary<string, string> dic, string id, T defaultValue = default)
        {
            if (dic != null && dic.TryGetValue(id, out string value))
            {
                return value.ConverTo<T>(defaultValue);
            }
            else
            {
                //if (Setting.Command.ExDebuging)
                    //Log.Error($"can not find key {id} in dic {(dic == null ? "Null" : $"{dic.GetType().Name}{dic.Count}")} by [At <{typeof(T).Name}>]");
                return defaultValue;
            }
        }
    }
}
