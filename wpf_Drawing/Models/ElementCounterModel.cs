using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wpf_Drawing.Models
{
    class ElementCounterModel : INotifyPropertyChanged
    {

        private int _counter;

        public int Counter
        {
            get { return _counter; }
            set 
                { 
                    _counter = value;
                    OnPropertyChanged(nameof(Counter));
                }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
