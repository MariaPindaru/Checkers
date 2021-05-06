using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Services
{
    public class Statistics : INotifyPropertyChanged
    {
        Statistics() { }
        public Statistics(string filePath)
        {
            Statistics old = Utilities.DeserializeObjectToXML<Statistics>(filePath);
            this.BlackWins = old.BlackWins;
            this.WhiteWins = old.WhiteWins;
        }
        private int whiteWins;
        private int blackWins;
        public int WhiteWins
        {
            set
            {
                whiteWins = value;
                NotifyPropertyChanged("WhiteWins");
            }
            get
            {
                return whiteWins;
            }
        }
        public int BlackWins
        {
            set
            {
                blackWins = value;
                NotifyPropertyChanged("BlackWins");
            }
            get
            {
                return blackWins;
            }
        }
        public void UpdateFile()
        {
            Utilities.SerializeObjectToXML<Statistics>(this, "Statistics.xml");
        }
        ~Statistics()
        {
            Utilities.SerializeObjectToXML<Statistics>(this, "Statistics.xml");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
