using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Checkers.Models
{
    public class Cell : INotifyPropertyChanged
    {
        public enum Color
        {
            White,
            Black
        }
        public Cell() { }
        public Cell(int x, int y)
        {
            isSelected = false;
            if ((x + y) % 2 == 1)
                CellColor = Color.Black;
            else
                CellColor = Color.White;
            Position = new Position(x, y);
            currentPiece = null;
        }
        private Color color;
        [XmlElement(Namespace = "CellColor")]
        public Color CellColor
        {
            set
            {
                color = value;
                if (color == Color.Black)
                    ColorName =  "#774936";
                else
                    ColorName = "#F2F3AE";
                NotifyPropertyChanged("CellColor");
            }
            get
            {
                return color;
            }
        }

        private string colorString;
        public string ColorName
        {
            get
            {
                return colorString;
            }
            set
            {
                colorString = value;
                NotifyPropertyChanged("ColorName");
            }
        }

        private Position position;
        public Position Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                NotifyPropertyChanged("Position");
            }
        }

        private Piece currentPiece;
        public Piece CurrentPiece
        {
            get
            {
                return currentPiece;
            }
            set
            {
                currentPiece = value;
                NotifyPropertyChanged("CurrentPiece");
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            set
            {
                if (value == true)
                {
                    ColorName = "#ff051e";
                }
                else
                {
                    if (color == Color.Black)
                        ColorName = "#774936";
                    else
                        ColorName = "#F2F3AE";
                }
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
            get
            {
                return isSelected;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
