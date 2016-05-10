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
using ATT.Data.AIF;
using ATT.Scripts;
using System.IO;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for UC_AIF.xaml
    /// </summary>
    public partial class UC_AIF : UserControl
    {
        private AIFDbContext _AifDB;
        public UC_AIF() {
            InitializeComponent();
            _AifDB = new AIFDbContext();
            loadMissions();
        }

        private async void loadMissions() {
           var missions = await Task.Run(() => {
              return _AifDB.Missions.ToList();

           });

            this.DataContext = missions;
        }

       
    }
}
