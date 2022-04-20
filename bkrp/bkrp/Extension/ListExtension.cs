using System;
using System.Collections.Generic;

namespace bkrp
{
    public static class ListExtension
    {
        public static void RemoveAndAdd<T>(this List<T> list, T remove, T add)
        {
            if (list.Contains(remove))
            {
                list.Remove(remove);
            }
            if (!list.Contains(add))
            {
                list.Add(add);
            }
        }
        public static void Add_ifnotExist<T>(this List<T> list, T add)
        {
            if (!list.Contains(add))
            {
                list.Add(add);
            }
        }
        public static void Remove_ifExist<T>(this List<T> list, T remove)
        {
            if (list.Contains(remove))
            {
                list.Remove(remove);
            }
        }
        public static void Add_ifnotExist<T, T2>(this Dictionary<T, T2> list, T add, T2 value)
        {
            if (!list.ContainsKey(add))
            {
                list.Add(add, value);
            }
        }
        public static void Remove_ifExist<T, T2>(this Dictionary<T, T2> list, T remove)
        {
            if (list.ContainsKey(remove))
            {
                list.Remove(remove);
            }
        }
        public static void Add_orReplace<T, T2>(this Dictionary<T, T2> list, T add, T2 value)
        {
            list[add] = value;
        }

        public static T Random<T>(this List<T> list)
        {
            var count = list.Count;
            if (count == 0) return default;
            Random r = new Random(DateTime.Now.Second);
            int idnex = r.Next(0, count);
            return list[idnex];
        }

        public static List<T> And<T>(this List<T> list, T obj)
        {
            var l = new List<T>(list);
            l.Add_ifnotExist(obj);
            return l;
        }
        public static List<T> And<T>(this List<T> list, T[] obj)
        {
            var l = new List<T>(list);
            l.AddRange(obj);
            return l;
        }
        public static T[] And<T>(this T[] list, T obj)
        {
            var l = new List<T>(list);
            l.Add_ifnotExist(obj);
            return l.ToArray();
        }
        public static T[] And<T>(this T[] list, T[] obj)
        {
            var l = new List<T>(list);
            l.AddRange(obj);
            return l.ToArray();
        }
    }
}
