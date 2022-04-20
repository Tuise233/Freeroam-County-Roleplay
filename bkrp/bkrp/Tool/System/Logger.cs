using AltV.Net;
using AltV.Net.ColoredConsole;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;


namespace bkrp
{
    public static class Log
    {
        private static bool Init = false;
        //private static string OutPutPath => ResourceLoader.RootPath + "Log/";

        //private static StreamWriter DefaultDebug;
        //private static StreamWriter SqlDebug;
        //private static StreamWriter ErrorDebug;

        public static void InitStart()
        {
            if (!Init)
            {
                Init = true;

                //Console.InputEncoding = System.Text.Encoding.UTF8;
                //Console.OutputEncoding = System.Text.Encoding.UTF8;

                //string data = DateTime.Now.ToShortDateString() + "H" + DateTime.Now.Hour;
                //var f = OutPutPath + data + "Debug.txt";
                //ResourceLoader.Check(f);
                //DefaultDebug = new StreamWriter(f, true, System.Text.Encoding.UTF8,int.MaxValue);

                //var f1 = OutPutPath + data + "SqlDebug.txt";
                //ResourceLoader.Check(f1);
                //SqlDebug = new StreamWriter(f1, true, System.Text.Encoding.UTF8, int.MaxValue);

                //var f2 = OutPutPath + data + "ErrorDebug.txt";
                //ResourceLoader.Check(f2);
                //ErrorDebug = new StreamWriter(f2, true, System.Text.Encoding.UTF8, int.MaxValue);

                //Clock.OnSixHour += (i) =>
                {
                    //End();
                    //Console.InputEncoding = System.Text.Encoding.UTF8;
                    //Console.OutputEncoding = System.Text.Encoding.UTF8;

                    //string data = DateTime.Now.ToShortDateString() + "H" + DateTime.Now.Hour;
                    //var f = OutPutPath + data + "Debug.txt";
                    //ResourceLoader.Check(f);
                    //DefaultDebug = new StreamWriter(f, true, System.Text.Encoding.UTF8);

                    //var f1 = OutPutPath + data + "SqlDebug.txt";
                    //ResourceLoader.Check(f1);
                    //SqlDebug = new StreamWriter(f1, true, System.Text.Encoding.UTF8);

                    //var f2 = OutPutPath + data + "ErrorDebug.txt";
                    //ResourceLoader.Check(f2);
                    //ErrorDebug = new StreamWriter(f2, true, System.Text.Encoding.UTF8);

                    //Console.Clear();
                };
            }
        }
        public static void End()
        {
            var t = DateTime.Now;
            string target = "./LogCopy/" + t.Year.ToString() + "-" + t.Month.ToString() + "-" + t.Day.ToString() + "-" + t.ToString("HHmmss") + "-copylog.txt";
            //ResourceLoader.Check(target);
            File.Copy("./server.log", target, true);

            //DefaultDebug?.Close();
            //DefaultDebug?.Dispose();
            //DefaultDebug = null;

            //SqlDebug?.Close();
            //SqlDebug?.Dispose();
            //SqlDebug = null;

            //ErrorDebug?.Close();
            //ErrorDebug?.Dispose();
            //ErrorDebug = null;
        }

        //private static void Out(string msg)
        //{
        //    //DefaultDebug?.WriteLine_Text(msg);
        //    Alt.LogInfo(msg);
        //}
        //private static void OutItem(string msg)
        //{
        //    Alt.LogInfo(msg);
        //    //DefaultDebug?.Write_Text(msg);
        //}
        //private static void SqlOut(string msg)
        //{
        //    Alt.LogFast(msg);
        //    //SqlDebug?.WriteLine_Text(msg);
        //}
        //private static void ErrorOut(string msg)
        //{
        //    Alt.LogError(msg);
        //    //DefaultDebug?.WriteLine_Text(msg);
        //    //ErrorDebug?.WriteLine_Text(msg);
        //}




        public static void Sql(string msg)
        {
            ColoredMessage m;
            m += TextColor.Blue;
            m += msg;
            Alt.LogColored(m);
            //msg = Time + msg;
            //SqlOut(msg);
            //Console.ForegroundColor = ConsoleColor.DarkGray;
            //Console.WriteLine(msg);
            //Console.ResetColor();
        }
        private static string LastMsg = null;
        public static void Loading(string msg)
        {
            if (LastMsg == msg)
                msg = "|";

            ColoredMessage m;
            m += TextColor.Cyan;
            m += msg;
            Alt.LogColored(m);
            //msg = Time + msg;
            //Out(msg);
            //if (Setting.Log.LogLoading)
            //{
            //    Console.ForegroundColor = ConsoleColor.DarkCyan;
            //    Console.WriteLine(msg);
            //    Console.ResetColor();
            //}
        }
        public static void Error(string msg)
        {
            if (LastMsg == msg)
                msg = "|";

            //msg = Time + msg;
            //ErrorOut(msg);
            Alt.LogError(msg);
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(msg);
            //Console.ResetColor();
        }
        public static void Warning(string msg)
        {
            if (LastMsg == msg)
                msg = "|";

            Alt.LogWarning(msg);
            //msg = Time + msg;
            //ErrorOut(msg);
            //Console.ForegroundColor = ConsoleColor.DarkYellow;
            //Console.WriteLine(msg);
            //Console.ResetColor();
        }
        public static void Command(string msg)
        {
            if (LastMsg == msg)
                msg = "|";

            ColoredMessage m;
            m += TextColor.Magenta;
            m += msg;
            Alt.LogColored(m);
            //msg = Time + msg;
            //Out(msg);
            //if (Setting.Log.LogCommand)
            //{
            //    Console.ForegroundColor = ConsoleColor.DarkMagenta;
            //    Console.WriteLine(msg);
            //    Console.ResetColor();
            //}
        }
        public static void Server(string msg)
        {
            if (LastMsg == msg)
                msg = "|";

            ColoredMessage m;
            m += TextColor.Yellow;
            m += msg;
            Alt.LogColored(m);
            //msg = Time + msg;
            //Out(msg);
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Console.WriteLine(msg);
            //Console.ResetColor();
        }
        public static void Client(string msg)
        {
            if (LastMsg == msg)
                msg = "|";

            ColoredMessage m;
            m += TextColor.Green;
            m += msg;
            Alt.LogColored(m);
            //msg = Time + msg;
            //Out(msg);
            //if (Setting.Log.LogClient)
            //{
            //    Console.ForegroundColor = ConsoleColor.DarkGreen;
            //    Console.WriteLine(msg);
            //    Console.ResetColor();
            //}
        }


        public static string Time
        {
            get
            {
                return /*'[' + DateTime.Now.ToShortTimeString() + ']'*/"";
            }
        }

        //public static void WriteLine_Text(this StreamWriter stream, string text)
        //{
        //    try
        //    {
        //        stream?.WriteLine(text);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message); ;
        //    }
        //}
        //public static void Write_Text(this StreamWriter stream, string text)
        //{
        //    try
        //    {
        //        stream?.Write(text);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message); ;
        //    }
        //}
    }

}