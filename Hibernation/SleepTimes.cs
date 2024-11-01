using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernation
{
    public class SleepTimes
    {
        /// <summary>
        /// スタンバイ時間
        /// </summary>
        public uint StandbyTime { get; set; } = 0;
        /// <summary>
        /// 休止時間
        /// </summary>
        public uint HibernationTime { get; set; } = 0;

        /// <summary>
        /// powerconfigコマンドを用いたスリープ時間の設定、取得
        /// </summary>
        protected PowerConfig PowerConfig { get; set; } = new PowerConfig();

        /// <summary>
        /// 現在のスリープ時間を取得
        /// </summary>
        public SleepTimes() {
            StandbyTime = PowerConfig.GetStandbyTime();
            HibernationTime = PowerConfig.GetHibernationTime();
        }


    }
}
