using Hibernation;
using System.Printing;
using Xunit.Abstractions;
using Xunit.Priority;

namespace HibernationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class PowerConfigTest
    {
        protected ITestOutputHelper Output { get; }

        public PowerConfigTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact(DisplayName = "0:初期化時にGUIDを正常に取得できるか"), Priority(0)]
        public void CanGetGUIDSuccessfully()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err));
        }

        [Fact(DisplayName = "10:スタンバイ時間を確認"), Priority(10)]
        public void SeeStandbyTime()
        {
            var pc = new PowerConfig();
            var err = pc.ErrorMessage;

            var time = pc.GetStandbyTime();

            Assert.True(String.IsNullOrEmpty(err));
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "10:休止時間を確認"), Priority(10)]
        public void SeeHibernationTime()
        {
            var pc = new PowerConfig();
            var err = pc.ErrorMessage;

            var time = pc.GetHibernationTime();

            Assert.True(String.IsNullOrEmpty(err));
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "20:スタンバイ時間の設定確認"), Priority(20)]
        public void SetStanbyTime()
        {
            var pc = new PowerConfig();
            var err1 = pc.ErrorMessage;
            if (String.IsNullOrEmpty(err1))
            {
                var current_time = (int)pc.GetStandbyTime();
                var setting_time = pc.SetStanbyTime(0);
                var err2 = pc.ErrorMessage;
                if (String.IsNullOrEmpty(err2))
                {
                    var get_time = pc.GetStandbyTime();
                    Assert.True(setting_time == 0);
                    setting_time = pc.SetStanbyTime(current_time);
                    Assert.True(setting_time == current_time);
                }
                else
                {
                    Assert.Fail(err2);
                }
            }
            else
            {
                Assert.Fail(err1);
            }
        }

        [Fact(DisplayName = "20:休止時間の設定確認"), Priority(20)]
        public void SetHibernationTime()
        {
            var pc = new PowerConfig();
            var err1 = pc.ErrorMessage;
            if (String.IsNullOrEmpty(err1))
            {
                var current_time = (int)pc.GetHibernationTime();
                var setting_time = pc.SetHibernationTime(0);
                var err2 = pc.ErrorMessage;
                if (String.IsNullOrEmpty(err2))
                {
                    var get_time = pc.GetHibernationTime();
                    Assert.True(setting_time == 0);
                    setting_time = pc.SetHibernationTime(current_time);
                    Assert.True(setting_time == current_time);
                }
                else
                {
                    Assert.Fail(err2);
                }
            }
            else
            {
                Assert.Fail(err1);
            }
        }
    }
}
