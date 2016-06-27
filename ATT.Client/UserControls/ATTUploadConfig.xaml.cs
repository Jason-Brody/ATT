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
using ATT.Data;
using ATT.Client.ViewModels;
using FluencyCSharp;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for ATTUploadConfig.xaml
    /// </summary>
    public partial class ATTUploadConfig : UserControl
    {
        private ATTDbContext _db;
        List<InterfaceVM> viewModels;
        public  MSGIDTaskData MsgIdTaskData { get; set; }
        public PayloadsDownloaderData PayloadsDownloadData { get; set; }
        public PayloadsUploaderData PayloadsUploadData { get; set; }

        public ATTUploadConfig() {
            InitializeComponent();
           
            loadData();
            MsgIdTaskData = Utils.GetObjFromXml<MSGIDTaskData>(GlobalConfig.MSGIDTaskDataFile);
            PayloadsDownloadData = Utils.GetObjFromXml<PayloadsDownloaderData>(GlobalConfig.PayloadsDownloaderDataFile);
            PayloadsUploadData = Utils.GetObjFromXml<PayloadsUploaderData>(GlobalConfig.PayloadsUploaderDataFile);
            uc_SAPLogin.DataContext = MsgIdTaskData.LH1;
            uc_PayloadsDownload.DataContext = PayloadsDownloadData;
            uc_PayloadsUpload.DataContext = PayloadsUploadData;
        }

        private async void loadData() {
            _db = new ATTDbContext();
            dg_Interfaces.ItemsSource = null;
            var data = await Task.Run(() => _db.SAPInterfaces.ToList());
            viewModels = new List<InterfaceVM>();
            data.ForEach(s => viewModels.Add(new InterfaceVM(s)));
            dg_Interfaces.ItemsSource =viewModels;
        }

    

        private async void btn_Save_Click(object sender, RoutedEventArgs e) {
            MsgIdTaskData.SaveToXml(GlobalConfig.MSGIDTaskDataFile);
            PayloadsDownloadData.SaveToXml(GlobalConfig.PayloadsDownloaderDataFile);
            PayloadsUploadData.SaveToXml(GlobalConfig.PayloadsUploaderDataFile);
            await _db.SaveChangesAsync();
            
        }

        private void cb_Header_Checked(object sender, RoutedEventArgs e) {
            dg_Interfaces.ItemsSource = null;
            viewModels.ForEach(s => s.Source.IsSelected = true);
            dg_Interfaces.ItemsSource = viewModels;
        }

        private void cb_Header_Unchecked(object sender, RoutedEventArgs e) {
            dg_Interfaces.ItemsSource = null;
            viewModels.ForEach(s => s.Source.IsSelected = false);
            dg_Interfaces.ItemsSource = viewModels;
        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e) {
            loadData();
        }

        private void mi_Select_Click(object sender, RoutedEventArgs e) {
            foreach(var item in dg_Interfaces.SelectedItems) {
                ((InterfaceVM)item).Source.IsSelected = true;
            }
            dg_Interfaces.ItemsSource = null;
            dg_Interfaces.ItemsSource = viewModels;
        }

        private void mi_UnSelect_Click(object sender, RoutedEventArgs e) {
            foreach (var item in dg_Interfaces.SelectedItems) {
                ((InterfaceVM)item).Source.IsSelected = false;
            }
            dg_Interfaces.ItemsSource = null;
            dg_Interfaces.ItemsSource = viewModels;
        }
    }
}
