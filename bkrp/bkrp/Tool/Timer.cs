using System;
using System.Collections.Generic;

namespace bkrp
{
    public static class Timer
    {
        private static List<TimerTask> timerList = new List<TimerTask>();

        public static List<TimerTask> TimerList { get => timerList; set => timerList = value; }

        /// <summary>
        /// 延时执行
        /// </summary>
        /// <param name="result"></param>
        /// <param name="callback"></param>
        /// <param name="update_ms"></param>
        /// <returns></returns>
        public static TimerTask WaitFor(Func<bool> result, Action callback, string name = "[...]", int update_ms = 500, uint maxWait = 10)
        {
            //bool debug = Setting.Command.ExDebuging;
            return SetTimer(update_ms, maxWait, (t) =>
            {
                if (t.Times <= 1)
                {
                    Log.Error("等待时间过长,该等待即将被动结束.请检查等待的对象是否存在");
                }
                if (result())
                {
                    /*
                    if (debug)
                    {
                        Log.Loading(name + " - Wait End --- count:" + t.Count);
                    }
                    */
                    callback?.Invoke();
                    t.Stop();
                }
                else
                {
                    /*
                    if (debug)
                    {
                        Log.Loading(name + " - Waiting for --- step:" + t.Count);
                    }
                    */
                }
            });
        }



        /// <summary>
        /// 设置延时执行
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TimerTask SetTimeOut(int ms, Action action) => SetTimer(ms, 2, (t) => { if (t.Count == 2) { action?.Invoke(); } });



        /// <summary>
        /// 设置无限循环定时器
        /// </summary>
        /// <param name="ms">第一次执行后,再次执行等待时间</param>
        /// <param name="count">总执行次数</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static TimerTask SetInterval(int ms, Action action) => SetTimer(ms, 0, action);
        /// <summary>
        /// 设置无限循环定时器
        /// </summary>
        /// <param name="ms">第一次执行后,再次执行等待时间</param>
        /// <param name="count">总执行次数</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static TimerTask SetInterval(int ms, Action<TimerTask> action) => SetTimer(ms, 0, action);



        /// <summary>
        /// 设置定时器
        /// </summary>
        /// <param name="ms">第一次执行后,再次执行等待时间</param>
        /// <param name="times">总执行次数</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static TimerTask SetTimer(int ms, uint times, Action action) => SetTimer(ms, times, (t) => action?.Invoke());
        /// <summary>
        /// 设置定时器
        /// </summary>
        /// <param name="ms">第一次执行后,再次执行等待时间</param>
        /// <param name="times">总执行次数</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        public static TimerTask SetTimer(int ms, uint times, Action<TimerTask> action)
        {
            TimerTask task = new(ms, times, action);
            if (!TimerList.Contains(task))
            {
                TimerList.Add(task);
                task.OnStop += () => ClearTimer(task);
                task.Run();
                return task;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timer"></param>
        public static void ClearTimer(TimerTask timer)
        {
            if (timer == null) return;
            if (TimerList.Contains(timer))
            {
                TimerList.Remove_ifExist(timer);
                if (!timer.IsStop)
                    timer.Stop();
            }
        }
        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timer"></param>
        public static void ClearAllTimer()
        {
            var ls = TimerList?.ToArray();
            if (ls != null)
                if (ls.Length > 0)
                    foreach (var timer in ls)
                    {
                        if (timer != null)
                            if (!timer.IsStop)
                                timer.Stop();
                    }
            TimerList?.Clear();
        }
    }
}
