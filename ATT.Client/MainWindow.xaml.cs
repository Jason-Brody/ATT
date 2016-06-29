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
using MahApps.Metro.Controls;
using ATT.Scripts;
using ScriptRunner.Interface;
using ATT.Data;
using System.Data.SqlClient;

namespace ATT.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private ScriptEngine<ATTUpload, ATTUploadData> attuploadScript = new ScriptEngine<ATTUpload, ATTUploadData>();
        private ScriptEngine<PIITrack, PIITrackData> piitrackScript = new ScriptEngine<PIITrack, PIITrackData>();
        private ScriptEngine<LHTrack, LHTrackData> lhtrackScript = new ScriptEngine<LHTrack, LHTrackData>();

        public MainWindow() {
            InitializeComponent();
            ATTUploadData d1 = new ATTUploadData();
            d1.MessageData = uc_ATTUploadConfig.MsgIdTaskData;
            d1.DownloadData = uc_ATTUploadConfig.PayloadsDownloadData;
            d1.UploadData = uc_ATTUploadConfig.PayloadsUploadData;
            d1.UpdateData = new PayloadsUpdateData();
            uc_ATTUpload.SetScript(attuploadScript,d1,fy_ATTUploadConfig);

            PIITrackData d2 = uc_PIITrackConfig.PII;
            uc_PIITrack.SetScript(piitrackScript, d2, fy_PIITrackConfig);

            LHTrackData d3 = uc_LHTrackConfig.LH;
            uc_LHTrack.SetScript(lhtrackScript, d3, fy_LHTrackConfig);

        }

       

       
    }

   
}
