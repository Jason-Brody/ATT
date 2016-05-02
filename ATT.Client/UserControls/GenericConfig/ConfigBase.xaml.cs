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
using System.Collections.ObjectModel;
using ATT.Data;
using System.Data.Entity;

namespace ATT.Client.UserControls.GenericConfig
{
    /// <summary>
    /// Interaction logic for ConfigBase.xaml
    /// </summary>
    public partial class ConfigBase<T> : UserControl where T : class
    {
        protected AttDbContext db;

        protected ObservableCollection<T> _items;

        public ConfigBase() {
            db = new AttDbContext();
        }

        public void Add() {

        }

        public async void LoadDatas(Func<AttDbContext, List<T>> Table) {
            _items = await Task.Run(() => new ObservableCollection<T>(Table(db)));
            this.DataContext = _items;
        }

        public void Save() {
            //db.SaveChanges();
        }


    }
}
