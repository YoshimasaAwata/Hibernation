using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hibernation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// スリープ時間を管理
        /// </summary>
        protected SleepTimes SleepTimes { get; set; } = new SleepTimes();

        /// <summary>
        /// 現在のスリープ時間をダイアログに反映
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            StandbyTextBlock.Text = SleepTimes.StandbyTime.ToString();
            StandbySlider.Value = SleepTimes.StandbyTime;
            HibernationTextBlock.Text = SleepTimes.HibernationTime.ToString();
            HibernationSlider.Value = SleepTimes.HibernationTime;
        }

        /// <summary>
        /// スリープ時間を設定後、ダイアログをクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 何もせずにダイアログをクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// スタンバイ時間をセット
        /// </summary>
        /// <param name="time">セットするスタンバイ時間</param>
        private void SetStandbyTime(uint time = 0)
        {
            if ((time == 0) || (time < SleepTimes.HibernationTime))
            {
                StandbyTextBlock.Text = time.ToString();
                StandbySlider.Value = time;
                SleepTimes.StandbyTime = time;
            }
            else
            {
                StatusTextBlock.Text = "休止時間より長い時間を設定しようとしています";
            }
        }

        /// <summary>
        /// 休止時間をセット
        /// </summary>
        /// <param name="time">セットする休止時間</param>
        private void SetHibernationTime(uint time = 0)
        {
            if ((time == 0) || (time > SleepTimes.StandbyTime))
            {
                HibernationTextBlock.Text = SleepTimes.HibernationTime.ToString();
                StandbySlider.Value = SleepTimes.StandbyTime;
                SleepTimes.StandbyTime = time;
            }
            else
            {
                StatusTextBlock.Text = "休止時間より長い時間を設定しようとしています";
            }
        }

        /// <summary>
        /// スリープ時間を0(スリープなし)とする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            HibernationTextBlock.Text = SleepTimes.HibernationTime.ToString();
            HibernationSlider.Value = SleepTimes.HibernationTime;
        }
    }
}