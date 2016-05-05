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

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for ITGTrack.xaml
    /// </summary>
    public partial class LHTrack : ATTBaseUI<LHTrackData>
    {

        

        public LHTrack() {
            InitializeComponent();
            base.Load();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            base.Save();
        }
    }
}
