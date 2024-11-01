using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hibernation
{
    public class SleepTimes
    {
        /// <summary>
        /// 設定用スタンバイ時間
        /// </summary>
        public uint StandbyTime { get; set; } = 0;
        /// <summary>
        /// 設定用休止時間
        /// </summary>
        public uint HibernationTime { get; set; } = 0;
        /// <summary>
        /// 現在設定されているスタンバイ時間
        /// </summary>
        protected uint CurrentStandbyTime { get; set; } = 0;
        /// <summary>
        /// 現在設定されている休止時間
        /// </summary>
        protected uint CurrentHibernationTime { get; set; } = 0;

        /// <summary>
        /// powerconfigコマンドを用いたスリープ時間の設定、取得
        /// </summary>
        protected PowerConfig PowerConfig { get; set; } = new PowerConfig();

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// 現在のスリープ時間を取得
        /// </summary>
        public SleepTimes()
        {
            CurrentStandbyTime = PowerConfig.GetStandbyTime();
            CurrentHibernationTime = PowerConfig.GetHibernationTime();
            StandbyTime = CurrentStandbyTime;
            HibernationTime = CurrentHibernationTime;
        }

        /// <summary>
        /// スタンバイ時間のセット
        /// </summary>
        /// <param name="time">スタンバイ時間</param>
        /// <returns>成否</returns>
        public bool SetStandbyTime(uint time)
        {
            bool rc = true;
            if ((time == 0) || (HibernationTime == 0) || (time < HibernationTime))
            {
                StandbyTime = time;
            }
            else
            {
                ErrorMessage = "スタンバイ時間が休止時間より長い";
                rc = false;
            }
            return rc;
        }

        /// <summary>
        /// スタンバイ時間のセット
        /// </summary>
        /// <param name="time">スタンバイ時間</param>
        /// <returns>成否</returns>
        public bool SetHibernationTime(uint time)
        {
            bool rc = true;
            if ((time == 0) || (time >= StandbyTime))
            {
                HibernationTime = time;
            }
            else
            {
                ErrorMessage = "休止時間がスタンバイ時間がより短い";
                rc = false;
            }
            return rc;
        }

        /// <summary>
        /// スリープ時間をpowercfgコマンドで設定
        /// </summary>
        /// <returns>設定結果</returns>
        /// <value>true: 成功</value>
        /// <value>false: 失敗</value>
        public bool SetSleepTime()
        {
            bool rc = true;
            if (CurrentStandbyTime != StandbyTime)
            {
                var standby_time = PowerConfig.SetStanbyTime((int)StandbyTime);
                if (standby_time != StandbyTime)
                {
                    ErrorMessage = PowerConfig.ErrorMessage;
                    rc = false;
                }
            }
            if (rc && (CurrentHibernationTime != HibernationTime))
            {
                var hibernation_time = PowerConfig.SetHibernationTime((int)HibernationTime);
                if (hibernation_time != HibernationTime)
                {
                    ErrorMessage = PowerConfig.ErrorMessage;
                    rc = false;
                }
            }
            return rc;
        }
    }
}
