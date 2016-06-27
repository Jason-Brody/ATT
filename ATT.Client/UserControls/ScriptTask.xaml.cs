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
using ScriptRunner.Interface;
using ATT.Scripts;
using System.Timers;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class ScriptTask : UserControl 
    {

        private IScriptEngine<ProgressInfo> script;
        private Timer _timer;
        private ScheduleData _data;
        private Flyout _fy;

        public string Title {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ScriptTask), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ScriptTask() {
            InitializeComponent();
            _timer = new Timer();
            _timer.Elapsed += _timer_Elapsed;
        }

        public void SetScript(IScriptEngine<ProgressInfo> Script, ScheduleData Data,Flyout fy) {
            script = Script;
            _data = Data;
            this.DataContext = _data;
            _fy = fy;
        }
      

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {

            if(_data.ExpireDate.Current < _data.Start.Current) {
                stop();
                return;
            }

            //script.Run(_data);

            
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e) {
            _timer.Interval = 2000;
            _timer.Start();
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e) {
            stop();
        }


        private void stop() {
            _timer.Stop();
            btn_Stop.Dispatcher.Invoke(() => btn_Stop.IsEnabled = false);
        }

        private void btn_Settings_Click(object sender, RoutedEventArgs e) {
            _fy.IsOpen = !_fy.IsOpen;
        }
    }
}
