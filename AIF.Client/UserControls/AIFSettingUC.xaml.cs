using AIF.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Data.Entity;

namespace AIF.Client.UserControls
{
    public delegate void ProcessingHandler();

    public delegate void ProcessedHandler(Missions m,bool IsNew);
    /// <summary>
    /// Interaction logic for AIFSettingUC.xaml
    /// </summary>
    public partial class AIFSettingUC : UserControl
    {
        private Missions _mission;
        private List<Tasks> _tasks;
        private AIFDbContext _db;
        private bool _isNew;
        public event ProcessedHandler OnProcessed;
        public event ProcessingHandler OnProcessing;

        public AIFSettingUC() {
            InitializeComponent();
            
        }


        public async void CreateMission(Missions mission) {
            _db = new AIFDbContext();
            _mission = mission;
            if (_mission.Id <= 0) {
                _isNew = true;
                var interfaces = await Task.Run(() => {
                    return _db.Interfaces.ToList();
                });
                foreach (var i in interfaces) {
                    _mission.Tasks.Add(new Tasks() { Interfaces = i });
                }
                _db.Missions.Add(_mission);

            } else {
                _isNew = false;
                _mission = _db.Missions.Include(m => m.Tasks.Select(s => s.Interfaces)).Single(m => m.Id == _mission.Id);
            }
            _tasks = _mission.Tasks.ToList();
            dg_Interfaces.DataContext = _tasks;
            this.DataContext = _mission;
        }



        private void cb_Header_Checked(object sender, RoutedEventArgs e) {
            _tasks.ForEach(i => i.IsSelected = true);
            dg_Interfaces.DataContext = null;
            dg_Interfaces.DataContext = _tasks;
        }

        private void cb_Header_Unchecked(object sender, RoutedEventArgs e) {
            _tasks.ForEach(i => i.IsSelected = false);
            dg_Interfaces.DataContext = null;
            dg_Interfaces.DataContext = _tasks;
        }

        private void mi_Select_Click(object sender, RoutedEventArgs e) {
            foreach (var item in dg_Interfaces.SelectedItems) {
                ((Tasks)item).IsSelected = true;
            }
            dg_Interfaces.DataContext = null;
            dg_Interfaces.DataContext = _tasks;
        }

        private void mi_UnSelect_Click(object sender, RoutedEventArgs e) {
            foreach (var item in dg_Interfaces.SelectedItems) {
                ((Tasks)item).IsSelected = false;
            }
            dg_Interfaces.DataContext = null;
            dg_Interfaces.DataContext = _tasks;
        }



        private async void btn_Save_Click(object sender, RoutedEventArgs e) {

            OnProcessing?.Invoke();
            await Task.Delay(5000);
            await _db.SaveChangesAsync();
            
            OnProcessed?.Invoke(_mission,_isNew);
        }
    }
}
