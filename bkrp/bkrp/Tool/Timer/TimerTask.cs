using System;
using System.Threading;

namespace bkrp
{
    public class TimerTask
    {
        public static uint TaskNum = 0;
        public TimerTask(int ms, uint times, Action<TimerTask> action)
        {
            try
            {
                this.ms = ms;
                Times = times;
                Action = action;
                IsBegin = false;
                Infinited = times == 0;
                Thread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        var stop = BeginStop?.Invoke();
                        if (stop != null && stop == true)
                        {
                            //结束
                            OnStop?.Invoke();
                            return;
                        }
                        while (Times > 0 || Infinited)
                        {
                            stop = BeginStop?.Invoke();
                            if (stop != null && stop.Value)//停止
                            {
                                //结束
                                OnStop?.Invoke();
                                return;
                            }
                            if (!Infinited)
                                Count += ((uint)1);
                            try
                            {
                                Action?.Invoke(this);
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Timer Excute Error!");
                                Log.Error(ex.Message);
                                Log.Error(ex.StackTrace);
                                return;
                            }
                            if (!Infinited)
                                Times -= 1;
                            stop = BeginStop?.Invoke();
                            if (((!Infinited) && Times <= 0) || (stop != null && stop == true))
                            {
                                //结束
                                OnStop?.Invoke();
                                return;
                            }
                            Thread.Sleep(ms);
                        }
                        //结束
                        OnStop?.Invoke();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);
                        Log.Error(ex.StackTrace);
                    }
                }));
                Name = Thread.Name = $"{TaskNum++}-Timer_ms{ms}_c{Times}";
                Thread.IsBackground = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

        public string Name { get; set; }
        public int ms { get; private set; }
        public uint Times { get; private set; }
        public bool Infinited { get; private set; }
        public Action<TimerTask> Action { get; private set; }

        /// <summary>
        /// 正在执行的次数
        /// </summary>
        public uint Count { get; private set; }
        public bool IsBegin { get; private set; }
        public bool IsStop { get => Thread.ThreadState == ThreadState.Stopped; }
        public event Action OnStop;

        private Thread Thread { get; set; }
        private Func<bool> BeginStop;

        public void Run()
        {
            if (!IsBegin)
            {
                Thread.Start();
                IsBegin = true;
                //Log.Loading("Timer开始:" + Name);
            }
        }
        public void Stop()
        {
            BeginStop = () => true;
        }
    }
}
