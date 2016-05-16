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
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.IO;
using ATT.Data.ATT;

namespace ATT.Client.UserControls
{
    /// <summary>
    /// Interaction logic for ATTBaseUI.xaml
    /// </summary>
    public partial class ATTBaseUI<T> : UserControl
    {

        T data;

        public async void Load() {
            //data = await ConfigLoader.GetData<T>();
            //this.DataContext = data;
        }

        public string Save() {
            return ConfigLoader.Save(data);
            
        }


    }

    public interface IConfig
    {
        void Save();
    }

    public static class ConfigLoader
    {
        private static AttDbContext _db;


        public static List<TaskDataConfigs> Configs = null;


        public static int Count;

        public static List<IConfig> MyConfigs = new List<IConfig>();

        static Task Load() {
            _db = new AttDbContext();
            return Task.Run(() => {
                Configs = _db.TaskDataConfigs.ToList();
            });
        }
        static bool IsProcess;

        public static async Task<T> GetData<T>() {
         
            if (Configs != null) {
                var config = Configs.Single(c => c.TypeName == typeof(T).FullName);
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                StringReader sr = new StringReader(config.Data);
                var data = xs.Deserialize(sr);
                sr.Close();
                return (T)data;
            }
            if (!IsProcess) {
                IsProcess = true;
                await Load();
            }
            await Task.Delay(50);
            return await GetData<T>();
        }

        public static string Save<T>(T data) {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            xs.Serialize(sw, data);
            var item = Configs.Single(c => c.TypeName == typeof(T).FullName);
            item.Data = sb.ToString();
            _db.SaveChanges();
            (App.Current.MainWindow as MetroWindow).ShowMessageAsync("Success", "Data Saved");
            return item.Data;
        }

        public static void Save() {
            MyConfigs.ForEach(c => c.Save());
            _db.SaveChanges();

            (App.Current.MainWindow as MetroWindow).ShowMessageAsync("Success", "Data Saved");
        }
    }


}
