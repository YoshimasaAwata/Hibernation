using Hibernation;
using System.Printing;
using Xunit.Abstractions;

namespace HibernationTest
{
    public class PowerConfigTest
    {
        protected ITestOutputHelper Output { get; }

        public PowerConfigTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact(DisplayName = "初期化時にGUIDを正常に取得できるか")]
        public void CanGetGUIDSuccessfully()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err));
        }

        [Fact(DisplayName ="スタンバイ時間を確認")]
        public void SeeStandbyTime()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;
            var time = pc.GetStandbyTime();

            Assert.True(String.IsNullOrEmpty(err));
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "休止時間を確認")]
        public void SeeHibernationTime()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;
            var time = pc.GetHibernationTime();

            Assert.True(String.IsNullOrEmpty(err));
            Output.WriteLine("Standby time is {0}", time);
        }
    }
}
