using MahApps.Metro.Controls;
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
using AIF.Data;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AIF.Scripts;
using ScriptRunner.Interface;
using System.Threading;
using MahApps.Metro.Controls.Dialogs;
using Young.Data.DBConnection;
using System.Data.SqlClient;

namespace AIF.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private AIFDbContext _db;
        private ObservableCollection<Missions> _missions;
        private Missions _currentMission;

        public MainWindow() {
            InitializeComponent();


            ProgressDialogController controller = null;
             


            UC_AIFSetting.OnProcessing += async () => {
                controller = await this.ShowProgressAsync("Please wait...", "Saving the data");
                controller.SetIndeterminate();
                fy_Settings.IsEnabled = false;
            };


            UC_AIFSetting.OnProcessed += async (m, i) => {

                loadMissions();

                controller.SetMessage("Success.");
                await Task.Delay(1000);

                if (controller != null)
                    await controller.CloseAsync();
                
               
                fy_Settings.IsEnabled = true;
                fy_Settings.IsOpen = false;


            };

     
           

        }


        //private void setStatus(bool isWorking) {
        //    grid.IsEnabled = !isWorking;
        //}

        private async void loadMissions() {

            var controller = await this.ShowProgressAsync("Please wait...", "Loading data from server");
            controller.SetIndeterminate();
            dg_Missions.DataContext = null;

            await Task.Run(() => {
                _db = new AIFDbContext();
                _missions = new ObservableCollection<Missions>(_db.Missions);
            });

            dg_Missions.DataContext = _missions;
            await controller.CloseAsync();
        }

        private void btn_Create_Click(object sender, RoutedEventArgs e) {
            Missions m = new Missions();
            _currentMission = m;
            UC_AIFSetting.CreateMission(_currentMission);
            fy_Settings.IsOpen = !fy_Settings.IsOpen;
        }

        private void btn_Edit_Click(object sender, RoutedEventArgs e) {
            _currentMission = dg_Missions.SelectedItem as Missions;
            UC_AIFSetting.CreateMission(_currentMission);
            fy_Settings.IsOpen = !fy_Settings.IsOpen;
        }




        private async void btn_Start_Click(object sender, RoutedEventArgs e) {

            if (_currentMission != null) {

                var controller = await this.ShowProgressAsync("Please wait...", "Running AIF upload script.");
                controller.SetIndeterminate();

                Tasks currentTask = null;

                try {
                    AIFMassUploadData d = new AIFMassUploadData();
                    d.LH1 = UC_AccountSetting.LH1;
                    d.LH7 = UC_AccountSetting.LH7;
                    d.Start = _currentMission.StartDt.Value;
                    d.End = _currentMission.EndDt.Value;
                    d.DataCounts = _currentMission.DataLimit.Value;
                    d.MissionId = _currentMission.Id;

                    ScriptEngine<AIFMassUpload, AIFMassUploadData> script = new ScriptEngine<AIFMassUpload, AIFMassUploadData>();

                    
                    while (true) {
                        var myTask = _db.Entry(_currentMission).Collection(m => m.Tasks).Query().Where(t => t.IsSelected);
                        var processedNum = myTask.Where(w => w.IsProcess == true).Count();
                        var totalNum = myTask.Count();
                        currentTask = myTask.Where(w => w.IsProcess == false).Take(1).FirstOrDefault();

                        double progress = double.Parse(processedNum.ToString()) / double.Parse(totalNum.ToString());
                        controller.SetProgress(progress);
                        controller.SetMessage($"{processedNum}/{totalNum}");
                        
                        if (currentTask == null) {
                            _currentMission.IsUpload = true;
                            _db.SaveChanges();
                            
                            break;
                        }


                        currentTask.IsProcess = true;
                        currentTask.ProcessDt = DateTime.Now;
                        _db.SaveChanges();

                        d.TaskId = currentTask.Id;
                        currentTask.IsProcess = true;
                        _db.SaveChanges();

                        await Task.Run(() => script.Run(d));
                       
                    }
                    _currentMission.IsUpload = true;
                    _db.SaveChanges();

                    await controller.CloseAsync();

                    loadMissions();
                }
                catch (Exception ex) {

                    if (currentTask != null) {
                        currentTask.IsProcess = false;
                        
                        _db.SaveChanges();
                    }

                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    await this.ShowMessageAsync("Error", ex.Message);
                }
                finally {
                    if (controller.IsOpen)
                        await controller.CloseAsync();
                }
               
            }
        }

        

        private void btn_Delete_Click(object sender, RoutedEventArgs e) {

        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e) {
            loadMissions();
        }

        private void MetroWindow_Closed(object sender, EventArgs e) {
            _db.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            fy_SAP_Settings.IsOpen = !fy_SAP_Settings.IsOpen;
        }

        private void dg_Missions_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            _currentMission = dg_Missions.SelectedItem as Missions;
        }

        private async void btn_56_Click(object sender, RoutedEventArgs e) {
            if (_currentMission != null) {

                var controller = await this.ShowProgressAsync("Please wait...", "Running Process 56 script.");
                controller.SetIndeterminate();

                try {
                    AIF56Data d = new AIF56Data();
                    d.LH7 = UC_AccountSetting.LH7;
                    d.MissionId = _currentMission.Id;

                    ScriptEngine<AIF56, AIF56Data> script = new ScriptEngine<AIF56, AIF56Data>();
                    script.StepProgress.ProgressChanged += (s, pe) => {

                        double progress = double.Parse(pe.Current.ToString()) / double.Parse(pe.Total.ToString());
                        controller.SetProgress(progress);
                        controller.SetMessage($"{pe.Current}/{pe.Total}");
                    };



                    var msg = await Task.Run(() => script.Run(d));
                    

                    _currentMission.Is56Process = true;
                    await _db.SaveChangesAsync();

                    controller.SetMessage("Finished");

                    await controller.CloseAsync();
                    loadMissions();

                    if (!string.IsNullOrEmpty(msg)) {
                        await this.ShowMessageAsync("Warning", msg);
                    }

                }
                catch (Exception ex) {
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    await this.ShowMessageAsync("Error", ex.Message);
                }
                finally {
                    if (controller.IsOpen)
                        await controller.CloseAsync();
                }
                
            }
        }

        private async void btn_51_Click(object sender, RoutedEventArgs e) {
            if (_currentMission != null) {

                var controller = await this.ShowProgressAsync("Please wait...", "Running Error Track script.");
                controller.SetIndeterminate();

                try {
                    AIFErrorTrackData d = new AIFErrorTrackData();
                    d.LH7 = UC_AccountSetting.LH7;
                    d.MissionId = _currentMission.Id;

                    ScriptEngine<AIFErrorTrack, AIFErrorTrackData> script = new ScriptEngine<AIFErrorTrack, AIFErrorTrackData>();
                    script.StepProgress.ProgressChanged += (s, pe) => {
                        double progress = double.Parse(pe.Current.ToString()) / double.Parse(pe.Total.ToString());
                        controller.SetProgress(progress);
                        controller.SetMessage($"{pe.Current}/{pe.Total}");
                    };

                    var msg = await Task.Run(() => script.Run(d));

                    _currentMission.IsErrorTrack = true;
                    _db.SaveChanges();

                    controller.SetMessage("Finished");
                    await controller.CloseAsync();

                    loadMissions();
                    if (!string.IsNullOrEmpty(msg))
                        await this.ShowMessageAsync("Warning", msg);
                }
                catch (Exception ex) {
                    while (ex.InnerException != null)
                        ex = ex.InnerException;
                    await this.ShowMessageAsync("Error", ex.Message);
                }
                finally {
                    if (controller.IsOpen)
                        await controller.CloseAsync();
                }
                
            }
        }

        private async void btn_Export_Click(object sender, RoutedEventArgs e) {

            var controller = await this.ShowProgressAsync("Please wait...", "Downloading data from server");
            controller.SetIndeterminate();
            try {
                await Task.Run(() => {
                    DBAccess da = new DBAccess(_db.Database.Connection, new SqlCommand());
                    SqlParameter para = new SqlParameter("mid", _currentMission.Id);
                    var datas = da.GetData(new SqlDataAdapter(), "SP_UpdateError", System.Data.CommandType.StoredProcedure, para);

                    datas.Tables[0].TableName = "IDocs";
                    datas.Tables[1].TableName = "Errors";
                    datas.Tables[2].TableName = "Status";

                    Young.Data.ExcelHelper.Current.Create($@"Results\AIF_Result_{_currentMission.Id}.xlsx");
                    Young.Data.ExcelHelper.Current.WriteAll(datas);
                    Young.Data.ExcelHelper.Current.Close();
                });

                await controller.CloseAsync();
                await this.ShowMessageAsync("Successful", "Download Successful");
            }
            catch(Exception ex) {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                await this.ShowMessageAsync("Error", ex.Message);
            }
            finally {
                if(controller.IsOpen)
                    await controller.CloseAsync();
            }
           

            


        }

        private void btn_Open_Click(object sender, RoutedEventArgs e) {
            if (!System.IO.Directory.Exists("Results"))
                System.IO.Directory.CreateDirectory("Results");
            var folder = "Results";
            Process.Start(folder);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
            loadMissions();
        }
    }
}
