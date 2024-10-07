using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernation
{
    class PowerConfig
    {
        public String Text { get; set; } = String.Empty;
        public PowerConfig() { }

        protected bool CallPowerCfg(String Arg)
        {
            bool rc = true;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "powercfg";
            info.Arguments = Arg;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;

            try
            {
                var cmd = Process.Start(info);
                if (cmd != null)
                {
                    Text = cmd.StandardOutput.ReadToEnd();
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
