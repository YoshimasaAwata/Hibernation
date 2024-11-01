using System.Text;
using System.Text.RegularExpressions;
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
        /// 警告レベル
        /// </summary>
        enum Level
        {
            Normal,         /// <summary>警告なし</summary> 
            Caution,        /// <summary>注意</summary>
            Warning,        /// <summary>警告</summary>
        }

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

            StandbyTextBox.Text = SleepTimes.StandbyTime.ToString();
            StandbySlider.Value = SleepTimes.StandbyTime;
            HibernationTextBox.Text = SleepTimes.HibernationTime.ToString();
            HibernationSlider.Value = SleepTimes.HibernationTime;
        }

        private void WriteStatus(string text, Level level = Level.Normal)
        {
            if (level == Level.Normal)
            {
                StatusTextBlock.Foreground = Brushes.Black;
            }
            else if (level == Level.Caution)
            {
                StatusTextBlock.Foreground = Brushes.Yellow;
            }
            else
            {
                StatusTextBlock.Foreground = Brushes.Red;
            }
            StatusTextBlock.Text = text;
            return;
        }

        /// <summary>
        /// ダイアログをクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            return;
        }

        /// <summary>
        /// 指定のスリープ時間をセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            var rc = SleepTimes.SetSleepTime();
            if (rc)
            {
                WriteStatus("スリープ時間のセットに成功");
            }
            else
            {
                WriteStatus("スリープ時間のセットに失敗", Level.Warning);
            }
            return;
        }

        /// <summary>
        /// スタンバイ時間をセット
        /// </summary>
        /// <param name="time">セットするスタンバイ時間</param>
        private void SetStandbyTime(uint time = 0)
        {
            var rc = SleepTimes.SetStandbyTime(time);
            if (rc)
            {
                StandbyTextBox.Text = time.ToString();
                StandbySlider.Value = time;
            }
            else
            {
                WriteStatus(SleepTimes.ErrorMessage, Level.Warning);
                StandbyTextBox.Text = SleepTimes.StandbyTime.ToString();
                StandbySlider.Value = SleepTimes.StandbyTime;
            }
            return;
        }

        /// <summary>
        /// 休止時間をセット
        /// </summary>
        /// <param name="time">セットする休止時間</param>
        private void SetHibernationTime(uint time = 0)
        {
            var rc = SleepTimes.SetHibernationTime(time);
            if (rc)
            {
                HibernationTextBox.Text = time.ToString();
                HibernationSlider.Value = time;
            }
            else
            {
                WriteStatus(SleepTimes.ErrorMessage, Level.Warning);
                HibernationTextBox.Text = SleepTimes.HibernationTime.ToString();
                HibernationSlider.Value = SleepTimes.HibernationTime;
            }
            return;
        }

        /// <summary>
        /// スリープ時間を0(スリープなし)とする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            SetStandbyTime();
            SetHibernationTime();
            return;
        }

        /// <summary>
        /// スタンバイ時間を0(スタンバイなし)とする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearStandbyButton_Click(object sender, RoutedEventArgs e)
        {
            SetStandbyTime();
        }

        /// <summary>
        /// 休止時間を0(休止なし)とする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearHibernationButton_Click(object sender, RoutedEventArgs e)
        {
            SetHibernationTime();
        }

        /// <summary>
        /// スタンバイ時間をスライダーでセットされた場合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StandbySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var time = (uint)StandbySlider.Value;
            SetStandbyTime(time);
            return;
        }

        /// <summary>
        /// 休止時間をスライダーでセットされた場合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HibernationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var time = (uint)HibernationSlider.Value;
            SetHibernationTime(time);
            return;
        }

        /// <summary>
        /// スタンバイ時間の入力を数字のみに限定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StandbyTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
            return;
        }

        /// <summary>
        /// 休止時間の入力を数字のみに限定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HibernationTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
            return;
        }

        /// <summary>
        /// スタンバイ時間の入力にペーストを許可しない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StandbyTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
            return;
        }

        /// <summary>
        /// 休止時間の入力にペーストを許可しない
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HibernationTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
            return;
        }

        /// <summary>
        /// スタンバイ時間の入力対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StandbyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var time_text = StandbyTextBox.Text;
                var time = UInt32.Parse(time_text);
                SetStandbyTime(time);
            }
            catch
            {
                StandbyTextBox.Text = SleepTimes.StandbyTime.ToString();
                WriteStatus("数値を入れて下さい");
            }
        }

        /// <summary>
        /// 休止時間の入力対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HibernationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var time_text = HibernationTextBox.Text;
                var time = UInt32.Parse(time_text);
                SetHibernationTime(time);
            }
            catch
            {
                HibernationTextBox.Text = SleepTimes.HibernationTime.ToString();
                WriteStatus("数値を入れて下さい");
            }
        }
    }
}