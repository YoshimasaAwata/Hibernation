using Hibernation;
using System.Printing;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HibernationTest
{
    public class PowerConfigTest
    {
        static readonly int DefaultTime = 30;
        protected ITestOutputHelper Output { get; }

        public PowerConfigTest(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact(DisplayName = "0:初期化時にGUIDを正常に取得できるか"), Order(0)]
        public void CanGetGUIDSuccessfully()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err), err);
        }

        [Fact(DisplayName = "10:スタンバイ時間を確認"), Order(10)]
        public void SeeStandbyTime()
        {
            var pc = new PowerConfig();

            var time = pc.GetStandbyTime();
            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err), err);
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "10:休止時間を確認"), Order(10)]
        public void SeeHibernationTime()
        {
            var pc = new PowerConfig();

            var time = pc.GetHibernationTime();
            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err), err);
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "20:スタンバイ時間の設定確認"), Order(20)]
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

        [Fact(DisplayName = "20:休止時間の設定確認"), Order(20)]
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
                int temp_time = current_time + DefaultTime;
                target_time = pc.SetHibernationTime(temp_time);
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

        [Fact(DisplayName = "30:休止時間より長いスタンバイ時間をセット"), Order(30)]
        public void SetStandbyMoreThanHibernation()
        {
            SetStandbyMoreThanEqualHibernation(1);
        }

        [Fact(DisplayName = "32:休止時間と同じスタンバイ時間をセット"), Order(32)]
        public void SetStandbyEqualHibernation()
        {
            SetStandbyMoreThanEqualHibernation(0);
        }

        protected void SetHibernationLessThanEqualStandby(int time)
        {
            var pc = new PowerConfig();

            var current_time = (int)pc.GetHibernationTime();
            var target_time = (int)pc.GetStandbyTime();

            bool standby_time_changed = false;
            bool hibernation_time_changed = false;
            if (target_time == 0)
            {
                if (current_time <= time)
                {
                    int setting_test_time = DefaultTime + time + 1;
                    int test_time = pc.SetHibernationTime(setting_test_time);
                    Assert.True(test_time == setting_test_time, "休止時間の事前設定に失敗");
                    hibernation_time_changed = true;
                }
                target_time = pc.SetStanbyTime(DefaultTime);
                standby_time_changed = true;
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
                if (standby_time_changed)
                {
                    pc.SetStanbyTime(0);
                }
                if (hibernation_time_changed)
                {
                    pc.SetHibernationTime(current_time);
                }
            }
        }

        [Fact(DisplayName = "35:スタンバイ時間より短い休止時間をセット"), Order(35)]
        public void SetHibernationLessThanStandby()
        {
            SetHibernationLessThanEqualStandby(1);
        }

        [Fact(DisplayName = "36:スタンバイ時間と同じ休止時間をセット"), Order(36)]
        public void SetHibernationEqualStandby()
        {
            SetHibernationLessThanEqualStandby(0);
        }
    }
}
