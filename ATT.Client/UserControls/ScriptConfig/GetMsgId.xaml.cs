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
using ATT.Scripts;
using ATT.Data.Entity;
using System.Timers;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for GetMsgId.xaml
    /// </summary>
    public partial class GetMsgId : ATTBaseUI<MSGIDTaskData>
    {
        private Timer _scheduleTimer;
        private Timer _showTimer;

        public GetMsgId() {
            InitializeComponent();
            base.Load();

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            base.Save();
        }

        private void btn_RunOnce_Click(object sender, RoutedEventArgs e) {
            //Program.RunTask(ATTTask.GetMessageAll, 0);
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e) {
           
            
            _scheduleTimer = new Timer();
           // _scheduleTimer.Interval = data.Interval * 60 * 1000;
            _scheduleTimer.Elapsed += _timer_Elapsed;
            _showTimer = new Timer();
            _showTimer.Interval = 1000;

           // int totalSeconds = data.Interval * 60;


            //_showTimer.Elapsed += (s, e1) => {
            //    string Msg = $"Will Run Next in {totalSeconds} seconds";
            //    totalSeconds--;
            //    tbl_Msg.Dispatcher.BeginInvoke(new Action(() => {
            //        tbl_Msg.Text = Msg;
            //    }

            //    ));
            //};

            _scheduleTimer.Start();
            _showTimer.Start();
        }

       

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            //Program.RunTask(ATTTask.GetMessageAll, 0);
        }
    }
}
