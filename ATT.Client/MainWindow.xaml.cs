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
using ATT.Scripts;

namespace ATT.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
       

        public MainWindow() {
            InitializeComponent();

            uc_ATTUpload.SetScript(new ScriptEngine<MSGIDTask, MSGIDTaskData>(), uc_ATTUploadConfig.MsgIdTaskData,fy_ATTUploadConfig);
            
        }

       

       
    }

   
}
