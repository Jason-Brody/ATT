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
using SharedLib;
using System.Xml.Serialization;
using System.IO;
using MahApps.Metro.Controls;

namespace AIF.Client.UserControls
{
    /// <summary>
    /// Interaction logic for AccountSettingUC.xaml
    /// </summary>
    public partial class AccountSettingUC : UserControl
    {
        public SAPLoginData LH1 { get; set; }
        public SAPLoginData LH7 { get; set; }

        private readonly string LH1File = "LH1.xml";
        private readonly string LH7File = "LH7.xml";

        public AccountSettingUC() {
            InitializeComponent();
            loadConfig();
        }

        private void loadConfig() {
            LH1 = GetConfig<SAPLoginData>(LH1File);
            LH7 = GetConfig<SAPLoginData>(LH7File);

            uc_LH1.DataContext = LH1;
            uc_LH7.DataContext = LH7;
        }
            
        public static T GetConfig<T>(string file) where T:new() {

            if (!File.Exists(file))
                return new T();

            XmlSerializer xs = new XmlSerializer(typeof(T));
            using(var fs = new FileStream(file, FileMode.Open)) {
                return (T)xs.Deserialize(fs);
            }
        }

        public static void SaveConfig(object obj,string file) {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            using (var fs = new FileStream(file, FileMode.Create)) {
                xs.Serialize(fs, obj);
            }
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e) {
            SaveConfig(LH1, LH1File);
            SaveConfig(LH7, LH7File);
            (LogicalTreeHelper.GetParent(this) as Flyout).IsOpen = false;
        }
    }
}
