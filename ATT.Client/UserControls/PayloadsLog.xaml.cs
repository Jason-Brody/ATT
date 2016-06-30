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
using System.Collections.ObjectModel;
using ATT.Scripts;

namespace ATT.Client.UserControls
{
    
    

    /// <summary>
    /// Interaction logic for PayloadsLog.xaml
    /// </summary>
    public partial class PayloadsLog : UserControl
    {
        ObservableCollection<ATTLog> _logs;

        public PayloadsLog() {
            InitializeComponent();
            _logs = new ObservableCollection<ATTLog>();
            dg_Logs.ItemsSource = _logs;
            ATTPayLoadsLog.OnLog += l => {
                dg_Logs.Dispatcher.BeginInvoke(new Action(() => _logs.Add(l)));
            };
        }

        private void btn_Export_Click(object sender, RoutedEventArgs e) {

        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e) {
            dg_Logs.Dispatcher.BeginInvoke(new Action(() => _logs.Clear()));
        }
    }
}
