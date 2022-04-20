using AltV.Net;
using System;

namespace bkrp
{
    public class Clock
    {
        //public event Action OnHalfHour = null;
        public static event Action<int> OnADay = null;
        public static event Action<int> OnSixHour = null;
        public static event Action<int> OnAnHour = null;

        public static event Action OnTenMinute = null;
        public static event Action OnAMinute = null;
        public static event Action OnFifteenSecond = null;
        public static event Action OnTenSecond = null;
        public static event Action OnFiveSecond = null;
        public static event Action OnSecond = null;

        public int LastHour { get; private set; } = -1;
        public int TotalSecond { get; private set; } = -1;

        public int CurrentDay => DateTime.Now.Day;
        public int CurrentHour => DateTime.Now.Hour;
        public int CurrentMinute => DateTime.Now.Minute;
        public int CurrentSecond => DateTime.Now.Second;

        public Clock()
        {
            Initialize.OnInitializeEnd += () =>
            {
                SetHoursTimer();
                SetSecondTimer();
            };
        }

        public void SetSecondTimer()
        {
            Timer.SetInterval(1000, () =>
            {
                try
                {
                    TotalSecond++;
                    if (TotalSecond % (24 * 60 * 60) == 0)
                    {
                        TotalSecond = 0;
                    }
                    OnSecond?.Invoke();
                    if (TotalSecond % 5 == 0)
                    {
                        OnFiveSecond?.Invoke();
                    }
                    if (TotalSecond % 10 == 0)
                    {
                        OnTenSecond?.Invoke();
                    }
                    if (TotalSecond % 15 == 0)
                    {
                        OnFifteenSecond?.Invoke();
                    }

                    if (TotalSecond % 60 == 0)
                    {
                        OnAMinute?.Invoke();
                    }
                    if (TotalSecond % 600 == 0)
                    {
                        OnTenMinute?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Clock Seconds Timer Error!");
                    Log.Error(ex.Message);
                    Log.Error(ex.StackTrace);
                }
            });
        }
        public void SetHoursTimer()
        {
            Timer.SetTimeOut(WaitForHourMS(), () =>
            {
                Timer.SetInterval(1000 * 60 * 60, (r) =>
                {
                    try
                    {
                        void HourEvent()
                        {
                            OnAnHour?.Invoke(CurrentHour);
                            if (CurrentHour % 6 == 0)
                            {
                                OnSixHour?.Invoke(CurrentHour);
                            }
                            if (CurrentHour == 6)
                            {
                                Log.Server($"按天结算 报时:{CurrentDay}日" + CurrentHour + "小时");
                                OnADay?.Invoke(CurrentDay);
                                r.Stop();
                                Log.Server("重新开始计时器!");
                                SetHoursTimer();
                            }
                            LastHour = CurrentHour;
                        }

                        if (LastHour == CurrentHour)
                        {
                            Log.Server($"上一次报时:{LastHour},此次报时:H{CurrentHour}:{CurrentMinute}:{CurrentSecond}, 报时出现偏差:{WaitForHourMSStr()}, 正在调整.");
                            Timer.SetTimeOut(WaitForHourMS(), () =>
                            {
                                Log.Server($"偏差纠正报时: {CurrentHour}");
                                HourEvent();
                            });
                        }
                        else
                        {
                            Log.Server("整点报时:" + CurrentHour + "小时. 上一次报时:" + LastHour);
                            HourEvent();
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("Clock Hour Time Error!");
                        Log.Error(ex.Message);
                        Log.Error(ex.StackTrace);
                    }
                });
            });
        }



        public int WaitForHourMS()
        {
            var wait = (59 - CurrentMinute) * 60 * 1000 + (60 - CurrentSecond) * 1000;
            Log.Loading($"距离整点小时还差{(59 - CurrentMinute)}分钟{(60 - CurrentSecond)}秒");
            return wait;
        }
        public string WaitForHourMSStr()
        {
            var wait = (59 - CurrentMinute) * 60 * 1000 + (60 - CurrentSecond) * 1000;
            return $"距离整点小时还差{(59 - CurrentMinute)}分钟{(60 - CurrentSecond)}秒";
        }



    }
}
