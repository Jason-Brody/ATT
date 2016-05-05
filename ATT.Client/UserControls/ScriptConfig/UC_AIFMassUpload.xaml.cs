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
using ATT.Robot;
using System.Data.SqlClient;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for UC_AIFMassUpload.xaml
    /// </summary>
    public partial class UC_AIFMassUpload : ATTBaseUI<AIFMassUploadData>
    {
        public UC_AIFMassUpload() {
            InitializeComponent();
            base.Load();
            
        }

      

        private async void btn_Run_Click(object sender, RoutedEventArgs e) {
            var data = base.Save();
            List<int> tasks;
            using(var db = new ATT.Data.AIF.AIFDbContext()) {
                //tasks = db.Database.SqlQuery<int>("exec [dbo].[SP_CreateMission] @xml", new SqlParameter("xml",data)).ToList();
                tasks = db.Tasks.Where(t => t.IsFinished == false).Select(i => i.Id).ToList();
            }
            int count = 1;
            pb.Maximum = tasks.Count;
            await Task.Run(() => {
                foreach (var t in tasks) {
                    pb.Dispatcher.BeginInvoke(new Action(() => {
                        pb.Value = count;
                    }));
                    tb_Progress.Dispatcher.BeginInvoke(new Action(() => {
                        tb_Progress.Text = $"{count}/{tasks.Count}";
                    }));
                    var p = Program.RunTask(Data.Entity.ATTTask.AIFMassUpload, t);
                    p.WaitForExit();
                    count++;
                }
            });
            
        }
    }
}
