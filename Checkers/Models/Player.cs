using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers.Models
{
    
    public class Player : INotifyPropertyChanged
    {
        Player() { }
        public Player(Piece.Color c)
        {
            color = c;
            isMyTurn = false;
            username = color == Piece.Color.Black ? "Black" : "White";
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                NotifyPropertyChanged("username");
            }
        }

        private bool isMyTurn;
        public bool IsMyTurn
        {
            set
            {
                isMyTurn = value;
                NotifyPropertyChanged("isMyTurn");
            }
            get
            {
                return isMyTurn;
            }
        }
        private Piece.Color color;
        public Piece.Color PlayerColor
        {
            set
            {
                color = value;
                NotifyPropertyChanged("color");
            }
            get
            {
                return color;
            }
        }

        public void ChangeTurn()
        {
            if (isMyTurn)
                isMyTurn = false;
            else
                isMyTurn = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
