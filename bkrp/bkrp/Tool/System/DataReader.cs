using AltV.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bkrp
{
    class DataReader : IScript
    {
        public static string RootPath = "./resources/gamemodedata/";

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="path">文件目录</param>
        public static void Check(string path)
        {
            var dic = Path.GetDirectoryName(path);
            if(!string.IsNullOrWhiteSpace(dic) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(dic);
            }
            if(!File.Exists(path))
            {
                File.Create(path);
            }
        }

        /// <summary>
        /// 获取文本数据
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <returns></returns>
        public static string GetText(string path)
        {
            Check(RootPath + path);
            var str = File.ReadAllText(RootPath + path);
            if (str == null) str = "";
            return str;
        }

        /// <summary>
        /// 写入文本数据
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <param name="text">写入内容</param>
        public static void SetText(string path, string text)
        {
            Check(RootPath + path);
            File.WriteAllText(RootPath + path, text);
        }

        /// <summary>
        /// 获取字典类型数据
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(string path)
        {
            Check(RootPath + path);
            var str = GetText(path);
            var dictionary = new Dictionary<string, string>();
            if(str != null)
            {
                foreach(var lineitem in str.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var line = lineitem;
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    line = line.Replace("：", ":").Replace("\t", "");
                    var paire = line.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if(paire.Length == 2)
                    {
                        if (string.IsNullOrWhiteSpace(paire[0]) || string.IsNullOrWhiteSpace(paire[1])) continue;
                        dictionary[DictionaryExtension.Trim(paire[0])] = DictionaryExtension.Trim(paire[1]);
                    }
                }
            }
            return dictionary;
        }
    }
}
