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
using System.IO;
using System.Collections.ObjectModel;
using ATT.Client.ViewModels;
using System.Xml.Serialization;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ATT.Robot;
using System.Diagnostics;
using ATT.Data.ATT;
using AIF.Data;
using AIF.Scripts;
using SharedLib;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for UC_AIF.xaml
    /// </summary>
    public partial class UC_AIF : UserControl
    {
        private AIFDbContext _AifDB;
        List<AIFInterfaceVM> viewModels;
        AIFMassUploadData _config;
        string configFile;

        public UC_AIF() {
            InitializeComponent();
            _AifDB = new AIFDbContext();
             configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AIFConfig.xml");
            loadMissions();
            loadConfig();
        }

        private void loadConfig() {
           

            
            if(File.Exists(configFile)) {
                XmlSerializer xs = new XmlSerializer(typeof(AIFMassUploadData));
                using(var fs = new FileStream(configFile, FileMode.Open)) {
                    _config = xs.Deserialize(fs) as AIFMassUploadData;
                }
                
            }

            if (_config == null) {
                _config = new AIFMassUploadData();
                _config.LH1 = new SAPLoginData();
                _config.LH7 = new SAPLoginData();
            }
                

            sp_Config.DataContext = _config;
           
        }

        private async void loadMissions() {
          

            var interfaces = await Task.Run(() => {
                return _AifDB.Interfaces.ToList();
            });
            viewModels= new List<AIFInterfaceVM>();

            interfaces.ForEach(i => viewModels.Add(new AIFInterfaceVM(i)));

          
            
            dg_Interfaces.DataContext = viewModels;
        }

        private void cb_Header_Checked(object sender, RoutedEventArgs e) {
            viewModels.ForEach(i => i.IsChecked = true);
        }

        private void cb_Header_Unchecked(object sender, RoutedEventArgs e) {
            viewModels.ForEach(i => i.IsChecked = false);
        }

        

        private async void btn_Run_Click(object sender, RoutedEventArgs e) {

           
            if (viewModels.Where(i => i.IsChecked).Count() < 1) {
                await (App.Current.MainWindow as MetroWindow).ShowMessageAsync("ERROR", $"Please choose at least 1 interface", MessageDialogStyle.Affirmative);
                return;
            }

            btn_Run.IsEnabled = !btn_Run.IsEnabled;
            startTimer();

            var ts = await createMission();

            tbl_Process.Text = $"0/{ts.Count}";
            int count = 0;
            foreach(var t in ts) {
                await startTask(t.Id);
                count++;
                tbl_Process.Text = $"{count}/{ts.Count}";

            }
            XmlSerializer xs = new XmlSerializer(typeof(AIFMassUploadData));

            using (FileStream fs = new FileStream(configFile, FileMode.Create)) {
                xs.Serialize(fs, _config);
            }
            btn_Run.IsEnabled = !btn_Run.IsEnabled;
            _timer.Stop();

        }

        private System.Timers.Timer _timer;

        private void startTimer() {
            DateTime start = DateTime.Now;
            _timer = new System.Timers.Timer(20);
            _timer.Elapsed += (s, e1) => {
                tbl_Time.Dispatcher.BeginInvoke(new Action(() => {
                    tbl_Time.Text = DateTime.Now.Subtract(start).ToString();
                }));
            };

            _timer.Start();
        }

        private Task startTask(int taskId) {
            return Task.Run(()=> { Program.RunTask(ATTTask.AIFMassUpload, taskId).WaitForExit(); });
        }

        private async Task<List<AIF.Data.Tasks>> createMission() {
            StringBuilder sb = new StringBuilder();

            XmlSerializer xs = new XmlSerializer(typeof(AIFMassUploadData));
            StringWriter sw = new StringWriter(sb);
            xs.Serialize(sw, _config);
            sw.Close();

            AIF.Data.Missions mission = new AIF.Data.Missions();
            mission.ConfigData = sb.ToString();
            mission.IntervalDays = _config.IntervalDays;
            mission.RetryCounts = _config.RetryCounts;
            mission.StartDt = _config.Start;
            mission.EndDt = _config.End;
            mission.DataLimit = _config.DataCounts;
            foreach(var item in viewModels.Where(i => i.IsChecked)) {
                var t = new AIF.Data.Tasks();
                t.DataCount = mission.DataLimit;
                t.InterfaceId = item.AIFInterface.Id;
                mission.Tasks.Add(t);
            }
            _AifDB.Missions.Add(mission);
            await _AifDB.SaveChangesAsync();
            return mission.Tasks.ToList();
        }

      
        private void mi_UnSelect_Click(object sender, RoutedEventArgs e) {
            foreach(var item in dg_Interfaces.SelectedItems) {
                ((AIFInterfaceVM)item).IsChecked = false;
            }
        }

        private void mi_Select_Click(object sender, RoutedEventArgs e) {
            foreach (var item in dg_Interfaces.SelectedItems) {
                ((AIFInterfaceVM)item).IsChecked =true;
            }
        }
    }
}
