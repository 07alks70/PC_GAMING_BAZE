using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_GAMING_BAZE.Models
{
    public class ComputerHostElement : INotifyPropertyChanged
    {

        private string _hostName;
        private int _guestAccId;
        private int _hostId;
        private int _tinme_av;
        private string _releasedTime;
        public string releasedTime
        {
            get { return _releasedTime; }
        }

        public string pcName
        {
            get { return _hostName; }
        }

        public string hostName
        {
            get { return _hostName; }
            set { 
                _hostName = value;
                OnPropertyChange("hostName");
            }
        }

        public int guestAccId
        {
            get { return _guestAccId; }
            set { 
                _guestAccId = value;
                OnPropertyChange("_guestAccId");
            }
        }

        public int hostId
        {
            get { return _hostId; }
            set
            {
                _hostId = value;
                OnPropertyChange("_hostId");
            }
        }

        public int tinme_av
        {
            get { return _tinme_av; }
            set
            {

                TimeSpan t = TimeSpan.FromSeconds(value);

                _releasedTime = string.Format("{0:D2}ч:{1:D2}мин",
                t.Hours,
                t.Minutes);

                _tinme_av = value;
                OnPropertyChange("_tinme_av");
            }
        }

        public ComputerHostElement()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
