using System;

namespace bkrp
{
    public static class Initialize
    {
        private static event Action OnInitialize;

        public static event Action OnDataBaseConnected;

        public static event Action OnInitializeEnd;

        public static void SetOrder()
        {
            OnInitialize += () =>
            {
                //数据库连接后
                OnDataBaseConnected?.Invoke();
            };
        }

        public static void Call_Initialize()
        {
            SetOrder();
            OnInitialize?.Invoke();
            OnInitializeEnd?.Invoke();
        }
    }
}


