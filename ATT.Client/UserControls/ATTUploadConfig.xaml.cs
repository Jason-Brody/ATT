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
    /// Interaction logic for ATTUploadConfig.xaml
    /// </summary>
    public partial class ATTUploadConfig : UserControl
    {
        public ATTUploadConfig(MSGIDTaskData d1,PayloadsDownloaderData d2,PayloadsUploaderData d3) {
            InitializeComponent();
            uc_SAPLogin.DataContext = d1.LH1;
            uc_PayloadsDownload.DataContext = d2;
            uc_PayloadsUpload.DataContext = d3;
        }
    }
}
