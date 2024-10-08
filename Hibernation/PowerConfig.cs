using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernation
{
    /// <summary>
    /// powercfgコマンドによるスタンバイや休止状態までの時間の管理
    /// </summary>
    class PowerConfig
    {
        private static readonly string GetActiveScheme = "/GETACTIVESCHEME";
        ///<value>スリープのGUIDのエイリアス</value>
        private static readonly string SubSleep = "SUB_SLEEP";
        ///<value>スタンバイのGUIDのエイリアス</value>
        private static readonly string StandbyIdle = "STANDBYIDLE";
        ///<value>休止状態のGUIDのエイリアス</value>
        private static readonly string HibernationIdle = "HIBERNATEIDLE";
        ///<value>電源設定のGUID</value>
        private string SchemeGUID {  get; set; } = string.Empty;
        /// <value>powercfgの出力</value>
        public string OutputText { get; set; } = string.Empty;
        public PowerConfig() { }

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
            catch (Exception)
            {
                rc = false;
            }

            return rc;
        }
    }
}
