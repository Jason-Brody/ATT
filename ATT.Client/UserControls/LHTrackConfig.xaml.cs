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
using SharedLib;
using FluencyCSharp;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for DataTrackConfig.xaml
    /// </summary>
    public partial class LHTrackConfig : UserControl
    {
        public LHTrackData LH { get; set; }

        public LHTrackConfig() {
            InitializeComponent();
            LH = Utils.GetObjFromXml<LHTrackData>(GlobalConfig.LHTrackDataFile);
            this.DataContext = LH;

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            LH.SaveToXml(GlobalConfig.LHTrackDataFile);
        }
    }
}
