using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// CloseUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CloseUserControl : UserControl
    {
        /// <summary>
        /// キャンセルされたかどうか
        /// </summary>
        public bool Cancelled { get; set; } = false;

        public CloseUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            DialogHost.CloseDialogCommand.Execute(null, null);
            return;
        }

        /// <summary>
        /// OKボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
            return;
        }
    }
}
