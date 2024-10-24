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

        [Fact(DisplayName = "0:����������GUID�𐳏�Ɏ擾�ł��邩"), Priority(0)]
        public void CanGetGUIDSuccessfully()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err));
        }

        [Fact(DisplayName ="10:�X�^���o�C���Ԃ��m�F"), Priority(10)]
        public void SeeStandbyTime()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;
            var time = pc.GetStandbyTime();

            Assert.True(String.IsNullOrEmpty(err));
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "10:�x�~���Ԃ��m�F"), Priority(10)]
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
