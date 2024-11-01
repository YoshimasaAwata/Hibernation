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
            StandbyTime = PowerConfig.GetStandbyTime();
            HibernationTime = PowerConfig.GetHibernationTime();
        }

        /// <summary>
        /// スタンバイ時間と休止時間が正しく設定されているかチェック
        /// </summary>
        /// <returns>正しくセットされているか</returns>
        /// <value>true: 正しくセットされている</value>
        /// <value>false: 正しくセットされていない</value>
        public bool CheckSleepTime()
        {
            return (StandbyTime == 0) || (HibernationTime == 0) || (StandbyTime <= HibernationTime);
        }

        /// <summary>
        /// スタンバイ時間のセット
        /// </summary>
        /// <param name="time">スタンバイ時間</param>
        /// <returns>成否</returns>
        public bool SetStandbyTime(uint time)
        {
            ErrorMessage = string.Empty;
            StandbyTime = time;
            bool rc = CheckSleepTime();
            if (!rc)
            {
                ErrorMessage = "スタンバイ時間が休止時間より長い";
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
            ErrorMessage = string.Empty;
            HibernationTime = time;
            bool rc = CheckSleepTime();
            if (!rc)
            {
                ErrorMessage = "スタンバイ時間が休止時間より長い";
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
            ErrorMessage = string.Empty;
            bool rc = CheckSleepTime();
            if (rc)
            {
                var hibernation_time = PowerConfig.SetHibernationTime((int)HibernationTime);
                if (hibernation_time != HibernationTime)
                {
                    ErrorMessage = PowerConfig.ErrorMessage;
                    rc = false;
                }
                var standby_time = PowerConfig.SetStanbyTime((int)StandbyTime);
                if (standby_time != StandbyTime)
                {
                    ErrorMessage += PowerConfig.ErrorMessage;
                    rc = false;
                }
            }
            else
            {
                ErrorMessage = "スリープ時間が正しくセットされていません";
            }
            return rc;
        }
    }
}
