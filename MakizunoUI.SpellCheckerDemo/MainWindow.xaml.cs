using SinWaveSample;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MakizunoUI.SpellCheckerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double _DemotimeSpan = 60 * 10 * 1000; // 5分
        // 入力停止とみなす時間（ミリ秒）
        //60* 10 * 1000; // 5分

        public MainWindow()
        {
            InitializeComponent();

            drawhelper = new TextBoxStylingHelper();

            _Demotime = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(_DemotimeSpan) };
        }


        private void SpellValidorCombboBox_loaded(object sender, RoutedEventArgs e)
        {

        }

        bool isFirst;
        TextBoxStylingHelper drawhelper;
        private void SpellValidatorComboBox_InputIdle(object sender, EventArgs e)
        {
            TextBox? childText = ChildFinder.FindVisualChild<TextBox>(sender as DependencyObject);

            if (childText is not null)
                if (childText.IsFocused)
                    drawhelper.DrawSinWave(childText, "rules.json", 3);



        }

        private void SpellValidorBox_InputIdle(object sender, EventArgs e)
        {
            if (SpellValidatorTestbox.IsFocused)
                drawhelper.DrawSinWave(SpellValidatorTestbox, "rules.json", 3);

        }
        private readonly DispatcherTimer _Demotime = null!;

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (!isFirst)
            {
                MessageBox.Show("これは巻き角スペルチェッカーのデモ版です。5分後にこのアプリケーションは自動的に終了します ", "巻き角スペルチェッカーのデモ版", MessageBoxButton.OK, MessageBoxImage.Information);
                _Demotime.Start();
                // ウィンドウが表示されたときにタイマーを開始
                isFirst = true; // 初回ロード後はフラグを更新
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _Demotime.Tick += (s, e) =>
            {
                _Demotime.Stop();
                MessageBox.Show("デモ時間が終了しました。アプリケーションを終了します。", "終了", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();


            };

            SpellValidatorTestbox.Text = "巻き角スペルチェッカーのデモ \r\n -codec:v libx265 -vf yadif=0:-1:1 -pix_fmt yuv420p \r\n -acodec aac -threads \r\n \r\n 文章は解析できません。\r\n 区切りは全角・半角スペースを問いません \r\n 1000文字以上のチェックは重くなります。\r\n フォーカスしたコントロールにSinWaveを描画します";
            SpellValidatorComboBox.Text = "巻き角スペルチェッカーのデモ　-codec:v libx265 -vf yadif=0:-1:1  -pix_fmt yuv420p -acodec aac  -threads";

        }
    }
}