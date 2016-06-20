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
using System.Windows.Shapes;
using AIF.Data;
using AIF.Client.ViewModels;
using System.Data.Entity;
using MahApps.Metro.Controls;

namespace AIF.Client
{
    

    /// <summary>
    /// Interaction logic for AIFMission.xaml
    /// </summary>
    public partial class AIFMission : MetroWindow
    {
        private Missions _mission;
        private AIFDbContext _db;
        private List<Tasks> _tasks;

       

        public AIFMission() {
            InitializeComponent();
        }

        public AIFMission(Missions mission):this() {
            _db = new AIFDbContext();
            _mission = mission;
            createMission();
            
        }

        private async void createMission() {
            if (_mission.Id <= 0) {
                var interfaces = await Task.Run(() => {
                    return _db.Interfaces.ToList();
                });
                foreach(var i in interfaces) {
                    _mission.Tasks.Add(new Tasks() { Interfaces = i });
                }
                _db.Missions.Add(_mission);

            } 
            //else {
                 
            //    _mission = _db.Missions.Include(m => m.Tasks.Select(s => s.Interfaces)).Single(m => m.Id == _mission.Id);
            //}
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

        private void Window_Closed(object sender, EventArgs e) {
            _db.Dispose();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            _db.SaveChanges();
            this.Close();
        }
    }
}
