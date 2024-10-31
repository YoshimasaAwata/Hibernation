using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernation
{
    public class SleepTimes
    {
        public uint SleepTime { get; set; } = 0;
        public uint HibernationTime { get; set; } = 0;

        protected PowerConfig PowerConfig { get; set; } = new PowerConfig();

        public SleepTimes() {
            SleepTime = PowerConfig.GetStandbyTime();
            HibernationTime = PowerConfig.GetHibernationTime();
        }


    }
}
