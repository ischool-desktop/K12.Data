using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace K12.Data.Configuration
{
    public static class Behavior_Extends
    {
        /// <summary>
        /// 非同步儲存資料，適用於不希望擋住畫面運作...。
        /// </summary>
        /// <param name="config"></param>
        public static void SaveAsync(this ConfigData config)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Callback), config);
        }

        private static void Callback(object state)
        {
            ConfigData config = state as ConfigData;
            config.Save();
        }
    }
}
