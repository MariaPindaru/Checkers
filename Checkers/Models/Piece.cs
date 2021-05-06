using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Checkers.Models
{
    public class Piece : INotifyPropertyChanged
    {
        public enum Color
        {
            White,
            Black
        };
        public Piece() { }

        public Piece(Color c)
        {
            color = c;
            king = false;
            if (color == Color.Black)
                image = @"/Resources/blackP.png";
            else
                image = @"/Resources/whiteP.png";
            NotifyPropertyChanged("Image");
        }
       
        private Color color;
        [XmlElement(Namespace = "PieceColor")]
        public Color PieceColor
        {
            set
            {
                color = value;
            }
            get
            {
                return color;
            }
        }

        private string image;
        public string Image
        {
            set
            {
                image = value;
                NotifyPropertyChanged("Image");
            }
            get
            {
                if (king == true)
                {
                    if (color == Color.Black)
                        return image = @"/Resources/blackK.png";
                    else
                        return image = @"/Resources/whiteK.png";
                }
                return image;
            }
        }

        private bool king;
        public bool King
        {
            set
            {
                king = value;
                NotifyPropertyChanged("King");
                NotifyPropertyChanged("Image");
            }
            get
            {
                return king;
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
