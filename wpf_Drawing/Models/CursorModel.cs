using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace wpf_Drawing.Models
{
    class CursorModel : INotifyPropertyChanged
    {
        private string _x;
        private string _y;
        private string _action;

        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public string X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
