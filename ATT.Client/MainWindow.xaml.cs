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
using MahApps.Metro.Controls;
using ATT.Scripts;

namespace ATT.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
       

        public MainWindow() {
            InitializeComponent();

            int? a = 1;
            int? b = 1;
            bool result = a == b;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            //List<ATT.Client.Windows.ColMapping> mappings = new List<Windows.ColMapping>();
            //ATT.Client.Windows.MapWin win = new Windows.MapWin(mappings, typeof(ATT.Scripts.MSGIDTaskData), new List<string>() { "A","B","C","D"});
            //win.ShowDialog();
            //fy_Settings.IsOpen = !fy_Settings.IsOpen;
            
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            ATT.Client.UserControls.ConfigLoader.Save();
        }
    }

   
}
