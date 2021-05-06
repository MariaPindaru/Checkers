using Checkers.Comands;
using Checkers.Models;
using Checkers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Checkers.ViewModels
{
    public class CellVM : BaseVM
    {
        public CellVM() { }
        public CellVM(Cell c, GameBusinessLogic businessLogic)
        {
            cell = c;
            this.businessLogic = businessLogic;
            CellWidth = 58.3333321f;
            CellHeight = 50.8888f;
        }
        private float cellWidth, cellHeight;
        public float CellWidth
        {
            set
            {
                cellWidth = value;
                NotifyPropertyChanged("CellWidth");
            }
            get { return cellWidth; }
        }
        public float CellHeight
        {
            set
            {
                cellHeight = value;
                NotifyPropertyChanged("CellHeight");
            }
            get { return cellHeight; }
        }
        private GameBusinessLogic businessLogic;
        private Cell cell;
        public Cell myCell
        {
            get { return cell; }
            set
            {
                cell = value;
                NotifyPropertyChanged("myCell");
            }
        }
      
        private ICommand clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                if (clickCommand == null)
                {
                    clickCommand = new RelayCommand<Cell>(businessLogic.ClickAction);
                }
                return clickCommand;
            }
        }
    }
}
