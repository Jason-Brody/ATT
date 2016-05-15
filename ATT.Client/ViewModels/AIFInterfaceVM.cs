using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Client.ViewModels
{
    public class AIFInterfaceVM : INotifyPropertyChanged
    {
        public AIFInterfaceVM() { }

        public AIFInterfaceVM(ATT.Data.AIF.Interfaces inf) {
            this.AIFInterface = inf;
        }

        private bool _isChecked;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsChecked {
            get { return _isChecked; }
            set {
                if (_isChecked != value) {
                    _isChecked = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
                }
            }
        }

        public ATT.Data.AIF.Interfaces AIFInterface { get; set; }
    }
}
