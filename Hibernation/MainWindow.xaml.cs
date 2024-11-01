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
using System.Xml;

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
        /// スリープ時間の最大値(分)
        /// </summary>
        private static readonly uint MaxSleepTime = 480;

        /// <summary>
        /// スリープ時間を管理
        /// </summary>
        protected SleepTimes? SleepTimes { get; set; } = null;

        /// <summary>
        /// 現在のスリープ時間をダイアログに反映
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            SleepTimes = new SleepTimes();
            StandbyTextBox.Text = SleepTimes.StandbyTime.ToString();
            StandbySlider.Value = SleepTimes.StandbyTime;
            HibernationTextBox.Text = SleepTimes.HibernationTime.ToString();
            HibernationSlider.Value = SleepTimes.HibernationTime;
        }

        /// <summary>
        /// ステータスにテキストを表示
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="level">表示するテキストの色のレベル</param>
        private void WriteStatus(string text = "", Level level = Level.Normal)
        {
            if (level == Level.Normal)
            {
                StatusTextBlock.Foreground = Brushes.Black;
            }
            else if (level == Level.Caution)
            {
                StatusTextBlock.Foreground = Brushes.GreenYellow;
            }
            else
            {
                StatusTextBlock.Foreground = Brushes.Red;
            }
            StatusTextBlock.Text = text;
            return;
        }

        /// <summary>
        /// セットボタンを有効化/無効化する
        /// </summary>
        /// <remarks>
        /// 無効化する場合にはStandbyTextBoxやHibernationTextBoxを赤枠とする
        /// </remarks>
        /// <param name="enable">有効化(true)/無効化(false)の指定</param>
        private void EnableSetButton(bool enable = true)
        {
            if (enable)
            {
                SetButton.IsEnabled = true;
                StandbyBorder.BorderBrush = Brushes.Black;
                HibernationBorder.BorderBrush = Brushes.Black;
            }
            else
            {
                SetButton.IsEnabled = false;
                StandbyBorder.BorderBrush = Brushes.Red;
                HibernationBorder.BorderBrush = Brushes.Red;
            }
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
            if (SleepTimes != null)
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
            }
            return;
        }

        /// <summary>
        /// スタンバイ時間をセット
        /// </summary>
        /// <param name="time">セットするスタンバイ時間</param>
        private void SetStandbyTime(uint time = 0)
        {
            if (SleepTimes != null)
            {
                EnableSetButton();
                WriteStatus();
                StandbyTextBox.Text = time.ToString();
                StandbySlider.Value = time;
                var rc = SleepTimes.SetStandbyTime(time);
                if (!rc)
                {
                    EnableSetButton(false);
                    WriteStatus(SleepTimes.ErrorMessage, Level.Warning);
                }
            }
            return;
        }

        /// <summary>
        /// 休止時間をセット
        /// </summary>
        /// <param name="time">セットする休止時間</param>
        private void SetHibernationTime(uint time = 0)
        {
            if (SleepTimes != null)
            {
                EnableSetButton();
                WriteStatus();
                HibernationTextBox.Text = time.ToString();
                HibernationSlider.Value = time;
                var rc = SleepTimes.SetHibernationTime(time);
                if (!rc)
                {
                    EnableSetButton(false);
                    WriteStatus(SleepTimes.ErrorMessage, Level.Warning);
                }
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
        /// 時間を示す文字列から0以上の整数を取得
        /// </summary>
        /// <remarks>
        /// 変換後の数値は10刻み<br/>
        /// 変換失敗時にはMaxSleepTimeよりも大きい値が返る
        /// </remarks>
        /// <param name="time_text">時間を示す文字列</param>
        /// <returns>0以上の整数</returns>
        private uint GetSleepTime(in string time_text)
        {
            uint return_time = 1000;    // MaxSleepTimeよりも大きい値
            try
            {
                uint time = UInt32.Parse(time_text);
                if (time >= 0)
                {
                    return_time = (time / 10) * 10;
                }
            }
            catch
            {
                // 何もしない
            }
            return return_time;
        }

        /// <summary>
        /// スタンバイ時間の入力対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StandbyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var time = GetSleepTime(StandbyTextBox.Text);
            if (time <= MaxSleepTime)
            {
                SetStandbyTime(time);
            }
            else
            {
                if (SleepTimes != null)
                {

                    StandbyTextBox.Text = SleepTimes.StandbyTime.ToString();
                    WriteStatus("0～480の数値を入れて下さい", Level.Caution);
                }
            }
            return;
        }

        /// <summary>
        /// 休止時間の入力対応
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HibernationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var time = GetSleepTime(HibernationTextBox.Text);
            if (time <= MaxSleepTime)
            {
                SetHibernationTime(time);
            }
            else
            {
                if (SleepTimes != null)
                {
                    HibernationTextBox.Text = SleepTimes.HibernationTime.ToString();
                    WriteStatus("0～480の数値を入れて下さい", Level.Caution);
                }
            }
            return;
        }

        /// <summary>
        /// スリープ時間をリストア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            WriteStatus();
            Properties.Settings.Default.Reload();
            SetStandbyTime(Properties.Settings.Default.StandbyTime);
            SetHibernationTime(Properties.Settings.Default.HibernationTime);
            WriteStatus("スリープ時間をリストアしました");
            return;
        }

        /// <summary>
        /// スリープ時間をストア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            WriteStatus();
            if (SleepTimes != null)
            {
                Properties.Settings.Default.StandbyTime = SleepTimes.StandbyTime;
                Properties.Settings.Default.HibernationTime = SleepTimes.HibernationTime;
                Properties.Settings.Default.Save();
                WriteStatus("スリープ時間をストアしました");
            }
            return;
        }
    }
}