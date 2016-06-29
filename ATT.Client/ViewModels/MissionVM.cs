using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Client.ViewModels
{
    public class MissionVM:INotifyPropertyChanged
    {
        private bool isComplete;

        public int Id { get; set; }

        public DateTime Start { get; set; }

        public bool IsComplete {
            get { return isComplete; }
            set {
                if(isComplete != value) {
                    isComplete = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsComplete)));
                }
            }
        }

        private string timeUsed;
        public string TimeUsed {
            get {
                return timeUsed;
            }
            set {
                if(timeUsed != value) {
                    timeUsed = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(TimeUsed)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
