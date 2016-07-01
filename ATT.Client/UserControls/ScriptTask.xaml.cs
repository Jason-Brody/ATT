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
using System.Collections.ObjectModel;
using ATT.Client.ViewModels;
using ATT.Data;

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
        private Timer _countTimer;
        private DateTime _start;
        private ObservableCollection<MissionVM> _missions;
        

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
            _countTimer = new Timer(1000);
            _countTimer.Elapsed += _countTimer_Elapsed;
            _missions = new ObservableCollection<MissionVM>();
            dg_Status.ItemsSource = _missions;
        }

        private void _countTimer_Elapsed(object sender, ElapsedEventArgs e) {

            

            foreach (var item in _missions) {
                if (!item.IsComplete) {
                    item.TimeUsed = DateTime.Now.Subtract(item.Start).ToString(@"hh\:mm\:ss");
                }
            }

            if (_timer.Enabled) {
                tbl_Time.Dispatcher.Invoke(() => {
                    DateTime dt = _start.AddMilliseconds(int.Parse(_timer.Interval.ToString()));
                    tbl_Time.Text = dt.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss");

                });
            }

            

            


            
                
            
        }

        public void SetScript(IScriptEngine<ProgressInfo> Script, ScheduleData Data,Flyout fy) {
            script = Script;
            _data = Data;
            this.DataContext = _data;
            _fy = fy;
        }
      

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {

            if(_data.ExpireDate < _data.Start) {
                stop();
                return;
            }
            runTask();
        }

        private async void btn_Run_Click(object sender, RoutedEventArgs e) {
            //_timer.Interval = _data.Interval*3600*1000;
            _timer.Interval = 30 * 1000;
            _timer.Start();
            _countTimer.Start();
            _data.GetPrevious();
            btn_Stop.IsEnabled = true;

            if (Cb_IsRun.IsChecked == true) {
                await Task.Run(() => runTask());
            } else {
                _start = DateTime.Now;
            }

            
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e) {
            stop();
        }

        private void runTask() {

            _data.GetNext();

            var mission = new Missions() {
                StartDt = _data.Start,
                StartHour = _data.Start.Hour
            };

            using (var db = new ATTDbContext()) {
                db.Missions.Add(mission);
                db.SaveChanges();
            }

            _data.Mid = mission.Id;

            _start = DateTime.Now;

            _data.Mid = mission.Id;
            

            var newMission = new MissionVM();
            newMission.Id = mission.Id;
            newMission.Start = _start;
            dg_Status.Dispatcher.BeginInvoke(new Action(() => _missions.Add(newMission)));
           
            script.Run(_data);
            

            newMission.IsComplete = true;
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
