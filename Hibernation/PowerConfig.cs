using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hibernation
{
    /// <summary>
    /// powercfgコマンドによるスタンバイや休止状態までの時間の管理
    /// </summary>
    public class PowerConfig
    {
        ///<value>16進数の数値を抽出するための正規表現</value>
        private static readonly string HexadecimalRegEx = @"0x[0-9a-zA-Z]{8}";
        ///<value>AC電源設定のインデックスを抽出するための正規表現</value>
        private static readonly string ACIndexRegEx = @"AC 電源設定のインデックス: 0x[0-9a-zA-Z]{8}";
        ///<value>GUIDを抽出するための正規表現</value>
        private static readonly string GuidRegEx = @"[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}";
        ///<value>電源管理の詳細設定を取得するオプション</value>
        private static readonly string GetQuery = "/query";
        ///<value>スリープの時間を設定するオプション</value>
        private static readonly string Change = "/change";
        ///<value>アクティブな電源管理の設定を取得するオプション</value>
        private static readonly string GetActiveScheme = "/getactivescheme";
        ///<value>スリープのGUIDのエイリアス</value>
        private static readonly string SubSleep = "SUB_SLEEP";
        ///<value>スタンバイのGUIDのエイリアス</value>
        private static readonly string StandbyIdle = "STANDBYIDLE";
        ///<value>休止状態のGUIDのエイリアス</value>
        private static readonly string HibernationIdle = "HIBERNATEIDLE";
        ///<value>スタンバイの時間指定</value>
        private static readonly string StandbyTimeoutAC = "standby-timeout-ac";
        ///<value>休止状態の時間指定</value>
        private static readonly string HibernationTimeoutAC = "hibernate-timeout-ac";

        ///<value>電源設定のGUID</value>
        protected string SchemeGUID { get; set; } = string.Empty;
        /// <value>powercfgの出力</value>
        protected string OutputText { get; set; } = string.Empty;
        /// <value>エラー発生時のメッセージ</value>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PowerConfig()
        {
            GetActiveSchemeGUID();
        }

        /// <summary>
        /// powercfgコマンドの実行
        /// </summary>
        /// <param name="options">powercfgコマンドのコマンドオプション</param>
        /// <returns>powercfgコマンドの実行結果</returns>
        /// <value>true: 成功</value>
        /// <value>false: 失敗</value>
        protected bool CallPowerCfg(string options)
        {
            bool rc = true;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "powercfg";
            info.Arguments = options;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            try
            {
                var cmd = Process.Start(info);
                if (cmd != null)
                {
                    OutputText = cmd.StandardOutput.ReadToEnd();
                    cmd.WaitForExit();
                }
            }
            catch
            {
                ErrorMessage = "Failed calling powercfg";
                rc = false;
            }

            return rc;
        }

        /// <summary>
        /// アクティブな電源設定のGUIDを取得
        /// </summary>
        protected void GetActiveSchemeGUID()
        {
            try
            {
                bool rc = CallPowerCfg(GetActiveScheme);
                if (rc)
                {
                    Match guid = Regex.Match(OutputText, GuidRegEx);
                    SchemeGUID = guid.Value;
                }
            }
            catch
            {
                // 何もしない
            }
        }

        /// <summary>
        /// 現在のスタンバイ/休止状態までの時間の設定を取得
        /// </summary>
        /// <param name="idle">スタンバイもしくは休止状態の指定</param>
        /// <returns>スタンバイ/休止状態までの時間(分)</returns>
        /// <value>0: 取得失敗(設定なし)</value>
        /// </returns>
        protected uint GetSleepTime(in String idle)
        {
            uint time = 0;
            try
            {
                string options = GetQuery + " " + SchemeGUID + " " + SubSleep + " " + idle;
                bool rc = CallPowerCfg(options);
                if (rc)
                {
                    Match acPowerCfg = Regex.Match(OutputText, ACIndexRegEx);
                    Match hex = Regex.Match(acPowerCfg.Value, HexadecimalRegEx);
                    time = Convert.ToUInt32(hex.Value, 16) / 60;
                }
            }
            catch
            {
                // 何もしない
            }
            return time;
        }

        /// <summary>
        /// 現在のスタンバイ/休止状態までの時間を設定
        /// </summary>
        /// <param name="timeout">スタンバイもしくは休止状態の指定</param>
        /// <param name="time">スタンバイもしくは休止状態までの時間(分)</param>
        /// <returns>スタンバイ/休止状態までの時間(分)</returns>
        /// <value>負数: 設定失敗</value>
        /// </returns>
        protected int SetSleepTime(in String timeout, int time)
        {
            string options = Change + " " + timeout + " " + time.ToString();
            bool rc = CallPowerCfg(options);
            return rc ? time : -1;
        }

        /// <summary>
        /// 現在のスタンバイまでの時間の設定を取得
        /// </summary>
        /// <returns>
        /// <returns>スタンバイまでの時間(分)</returns>
        /// <value>0: 取得失敗(設定なし)</value>
        public uint GetStandbyTime()
        {
            return GetSleepTime(StandbyIdle);
        }

        /// <summary>
        /// スタンバイまでの時間を設定
        /// </summary>
        /// <param name="time">
        /// スタンバイまでの時間(分)<br/>
        /// 負数が指定された場合には0とみなす<br/>
        /// デフォルト値: 0
        /// </param>
        /// <returns>設定した時間(秒)</returns>
        /// <value>負数: 設定失敗</value>
        public int SetStanbyTime(int time = 0)
        {
            int rc = -1;
            if (time < 0)
            {
                time = 0;
            }
            var hibernation = GetHibernationTime();
            if ((time == 0) || (hibernation == 0) || (hibernation >= time))
            {
                rc = SetSleepTime(StandbyTimeoutAC, time);
            }
            else
            {
                ErrorMessage = "スタンバイ時間より休止状態時間が短い";
            }
            return rc;
        }

        /// <summary>
        /// 現在の休止状態までの時間の設定を取得
        /// </summary>
        /// <returns>
        /// <returns>スタンバイ/休止状態までの時間(分)</returns>
        /// <value>0: 取得失敗(設定なし)</value>
        /// </returns>
        public uint GetHibernationTime()
        {
            return GetSleepTime(HibernationIdle);
        }

        /// <summary>
        /// 休止状態までの時間を設定
        /// </summary>
        /// <param name="time">
        /// 休止状態までの時間(分)<br/>
        /// 負数が指定された場合には0とみなす<br/>
        /// デフォルト値: 0
        /// </param>
        /// <returns>設定した時間(秒)</returns>
        /// <value>負数: 設定失敗</value>
        public int SetHibernationTime(int time = 0)
        {
            int rc = -1;
            if (time < 0)
            {
                time = 0;
            }
            var stanby = GetStandbyTime();
            if ((time == 0) || (stanby <= time))
            {
                rc = SetSleepTime(HibernationTimeoutAC, time);
            }
            else
            {
                ErrorMessage = "スタンバイ時間より休止状態時間が短い";
            }
            return rc;
        }
    }
}
