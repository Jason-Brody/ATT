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
    public partial class PIITrackConfig : UserControl
    {
        public PIITrackData PII { get; set; }

        public PIITrackConfig() {
            InitializeComponent();
            PII = Utils.GetObjFromXml<PIITrackData>(GlobalConfig.PIITrackDataFile);
            this.DataContext = PII;

        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            PII.SaveToXml(GlobalConfig.PIITrackDataFile);
        }
    }
}
