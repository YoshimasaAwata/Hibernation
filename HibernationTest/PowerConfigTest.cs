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

        [Fact(DisplayName = "����������GUID�𐳏�Ɏ擾�ł��邩")]
        public void CanGetGUIDSuccessfully()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;

            Assert.True(String.IsNullOrEmpty(err));
        }

        [Fact(DisplayName ="�X�^���o�C���Ԃ��m�F")]
        public void SeeStandbyTime()
        {
            var pc = new PowerConfig();

            var err = pc.ErrorMessage;
            var time = pc.GetStandbyTime();

            Assert.True(String.IsNullOrEmpty(err));
            Output.WriteLine("Standby time is {0}", time);
        }

        [Fact(DisplayName = "�x�~���Ԃ��m�F")]
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
