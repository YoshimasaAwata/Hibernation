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

            Assert.True(String.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "10:スタンバイ時間を確認"), Priority(10)]
        public void SeeStandbyTime()
        {
            var pc = new PowerConfig();

            var time = pc.GetStandbyTime();
            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err), err);
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "10:休止時間を確認"), Priority(10)]
        public void SeeHibernationTime()
        {
            var pc = new PowerConfig();

            var time = pc.GetHibernationTime();
            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err), err);
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "20:スタンバイ時間の設定確認"), Priority(20)]
        public void SetStanbyTime()
        {
            var pc = new PowerConfig();

            var current_time = (int)pc.GetStandbyTime();

            var setting_time = pc.SetStanbyTime(0);
            var err = pc.ErrorMessage;
            Assert.True(String.IsNullOrEmpty(err), err);

            var get_time = pc.GetStandbyTime();
            Assert.True(get_time == 0, "スタンバイ時間の解除に失敗");

            setting_time = pc.SetStanbyTime(current_time);
            Assert.True(setting_time == current_time, "スタンバイ時間のセットに失敗");
        }

        [Fact(DisplayName = "20:休止時間の設定確認"), Priority(20)]
        public void SetHibernationTime()
        {
            var pc = new PowerConfig();

            var current_time = (int)pc.GetHibernationTime();

            var setting_time = pc.SetHibernationTime(0);
            var err = pc.ErrorMessage;
            Assert.True(String.IsNullOrEmpty(err), err);

            var get_time = pc.GetHibernationTime();
            Assert.True(get_time == 0, "休止時間の解除に失敗");

            setting_time = pc.SetHibernationTime(current_time);
            Assert.True(setting_time == current_time, "スタンバイ時間のセットに失敗");
        }

        protected void SetStandbyMoreThanEqualHibernation(int time)
        {
            var pc = new PowerConfig();

            var current_time = (int)pc.GetStandbyTime();
            var target_time = (int)pc.GetHibernationTime();

            bool time_changed = false;
            if (target_time == 0)
            {
                target_time = pc.SetHibernationTime(30);
                time_changed = true;
            }

            var setting_time = pc.SetStanbyTime(target_time + time);
            try
            {
                Assert.True(setting_time < 0, pc.ErrorMessage);
            }
            catch (Exception ex)
            {
                pc.SetStanbyTime(current_time);
                throw;
            }
            finally
            {
                if (time_changed)
                {
                    pc.SetHibernationTime(0);
                }
            }
        }

        [Fact(DisplayName = "30:休止時間より長いスタンバイ時間をセット"), Priority(30)]
        public void SetStandbyMoreThanHibernation()
        {
            SetStandbyMoreThanEqualHibernation(1);
        }

        [Fact(DisplayName = "32:休止時間と同じスタンバイ時間をセット"), Priority(32)]
        public void SetStandbyEqualHibernation()
        {
            SetStandbyMoreThanEqualHibernation(0);
        }

        protected void SetHibernationLessThanEqualStandby(int time)
        {
            var pc = new PowerConfig();

            var current_time = (int)pc.GetHibernationTime();
            var target_time = (int)pc.GetStandbyTime();

            bool time_changed = false;
            if (target_time == 0)
            {
                target_time = pc.SetStanbyTime(30);
                time_changed = true;
            }

            var setting_time = pc.SetHibernationTime(target_time - time);
            try
            {
                Assert.True(setting_time < 0, pc.ErrorMessage);
            }
            catch (Exception ex)
            {
                pc.SetHibernationTime(current_time);
                throw;
            }
            finally
            {
                if (time_changed)
                {
                    pc.SetStanbyTime(0);
                }
            }
        }

        [Fact(DisplayName = "35:スタンバイ時間より短い休止時間をセット"), Priority(35)]
        public void SetHibernationLessThanStandby()
        {
            SetHibernationLessThanEqualStandby(1);
        }

        [Fact(DisplayName = "36:スタンバイ時間と同じ休止時間をセット"), Priority(36)]
        public void SetHibernationEqualStandby()
        {
            SetHibernationLessThanEqualStandby(0);
        }
    }
}
