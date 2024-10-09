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
    class PowerConfig
    {
        ///<value>16進数の数値を抽出するための正規表現</value>
        private static readonly string HexadecimalRegEx = @"0x[0-9a-zA-Z]{8}";
        ///<value>AC電源設定のインデックスを抽出するための正規表現</value>
        private static readonly string ACIndexRegEx = @"AC 電源設定のインデックス: 0x[0-9a-zA-Z]{8}";
        ///<value>GUIDを抽出するための正規表現</value>
        private static readonly string GuidRegEx = @"[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}";
        ///<value>電源管理の詳細設定を取得するオプション</value>
        private static readonly string GetQuery = "/QUERY";
        ///<value>アクティブな電源管理の設定を取得するオプション</value>
        private static readonly string GetActiveScheme = "/GETACTIVESCHEME";
        ///<value>スリープのGUIDのエイリアス</value>
        private static readonly string SubSleep = "SUB_SLEEP";
        ///<value>スタンバイのGUIDのエイリアス</value>
        private static readonly string StandbyIdle = "STANDBYIDLE";
        ///<value>休止状態のGUIDのエイリアス</value>
        private static readonly string HibernationIdle = "HIBERNATEIDLE";
        ///<value>電源設定のGUID</value>
        private string SchemeGUID { get; set; } = string.Empty;
        /// <value>powercfgの出力</value>
        public string OutputText { get; set; } = string.Empty;
        public PowerConfig() { GetActiveSchemeGUID(); }

        /// <summary>
        /// powercfgコマンドの実行
        /// </summary>
        /// <param name="options">powercfgコマンドのコマンドオプション</param>
        /// <returns>
        /// powercfgコマンドの実行結果</br>
        /// true: 成功</br>
        /// false: 失敗
        /// </returns>
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
                /// 何もしない
            }
        }

        /// <summary>
        /// 現在のスタンバイ/休止状態までの時間の設定を取得
        /// </summary>
        /// <param name="idle">スタンバイもしくは休止状態の指定</param>
        /// <returns>
        /// スタンバイ/休止状態までの時間(分)</br>
        /// 取得できない場合には0(設定なし)が返る
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
                    time = Convert.ToUInt32(hex.Value, 16);
                }
            }
            catch
            {
                /// 何もしない
            }
            return time;
        }

        /// <summary>
        /// 現在のスタンバイまでの時間の設定を取得
        /// </summary>
        /// <returns>
        /// スタンバイまでの時間(分)</br>
        /// 取得できない場合には0(設定なし)が返る
        /// </returns>
        public uint GetStandbyTime()
        {
            return GetSleepTime(StandbyIdle);
        }

        /// <summary>
        /// 現在の休止状態までの時間の設定を取得
        /// </summary>
        /// <returns>
        /// 休止状態までの時間(分)</br>
        /// 取得できない場合には0(設定なし)が返る
        /// </returns>
        public uint GetHibernationTime()
        {
            return GetSleepTime(HibernationIdle);
        }
    }
}
