using System;

namespace bkrp
{
    public static class Initialize
    {
        private static event Action OnInitialize;

        public static event Action OnDataBaseConnected;

        public static event Action OnDefaultGroupInit;

        public static event Action OnDefaultVehicleInit;

        public static event Action OnInitializeEnd;

        public static void SetOrder()
        {
            OnInitialize += () =>
            {
                //数据库连接后
                OnDataBaseConnected?.Invoke();

                //默认组织初始化
                OnDefaultGroupInit?.Invoke();

                //默认载具初始化
                OnDefaultVehicleInit?.Invoke();
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


