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
using System.Windows.Shapes;

namespace ATT.Client.Windows
{
    /// <summary>
    /// Interaction logic for MapWin.xaml
    /// </summary>
    public partial class MapWin : Window
    {
        private List<ColMapping> _mappings;
        private Type _entityType;
        private List<string> _cols;

        private MapWin() {
            InitializeComponent();
        }

        public MapWin(List<ColMapping> Mappings,Type t,List<string> Columns):this() {
            Mappings.Clear();
            this._mappings = Mappings;
            this._entityType = t;
            this._cols = Columns;
            this.DataContext = Columns;
            analysisType();
        }

        private void analysisType() {
            foreach(var prop in _entityType.GetProperties().Where(p=>(p.PropertyType.IsPrimitive || p.PropertyType == typeof(string)) && p.CanRead == true && p.CanWrite == true)){
                _mappings.Add(new ColMapping(prop.Name));
            }
            //lv_Mappings.DataContext = _mappings;
            dg_Mappings.DataContext = _mappings;
        }
    }


    public class ColMapping
    {
        public ColMapping() { }

        public ColMapping(string Col) : this(Col, null) { }

        public ColMapping(string Col,string MappedCol) {
            this.Column = Col;
            this.MappedColumn = MappedCol;
        }

        public string Column { get; set; }

        public string MappedColumn { get; set; }
    }

}
