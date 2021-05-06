using Checkers.Comands;
using Checkers.Models;
using Checkers.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Checkers.ViewModels
{
    [System.Xml.Serialization.XmlInclude(typeof(Cell))]
    [System.Xml.Serialization.XmlInclude(typeof(Piece))]
    class GameVM : BaseVM
    {
        private GameBusinessLogic businessLogic;

        private ObservableCollection<ObservableCollection<CellVM>> gameBoard;
        public ObservableCollection<ObservableCollection<CellVM>> GameBoard
        {
            set
            {
                gameBoard = value;
                NotifyPropertyChanged("GameBoard");
            }
            get
            {
                return gameBoard;
            }
        }
        public GameVM()
        {
            ObservableCollection<ObservableCollection<Cell>> board = Utilities.InitGameBoard();
            this.businessLogic = new GameBusinessLogic(ref board);
            this.GameBoard = CellBoardToCellVMBoard(ref board);
            this.WindowHeight = 700;
            this.WindowWidth = 1200;
        }
        public bool MultipleJumps
        {
            set
            {
                businessLogic.MultipleJumps = value;
            }
        }
        private ObservableCollection<ObservableCollection<CellVM>> CellBoardToCellVMBoard(ref ObservableCollection<ObservableCollection<Cell>> board)
        {
            ObservableCollection<ObservableCollection<CellVM>> result = new ObservableCollection<ObservableCollection<CellVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<CellVM> line = new ObservableCollection<CellVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Cell c = board[i][j];
                    CellVM cellVM = new CellVM(c, businessLogic);
                    line.Add(cellVM);
                }
                result.Add(line);
            }
            return result;
        }
        public GameBusinessLogic GameLogic
        {
            get
            {
                return businessLogic;
            }
            set
            {
                businessLogic = value;
                NotifyPropertyChanged("GameLogic");
            }
        }
        private float windowWidth, windowHeight;
        public float WindowWidth
        {
            set
            {
                windowWidth = value;
                GameBoard.ToList().ForEach(row => row.ToList().ForEach(cell =>
                {
                    cell.CellWidth = windowWidth / 2 / 8;
                    cell.CellWidth = windowHeight / 6 * 4 / 8;
                }));
                NotifyPropertyChanged("WindowWidth");
            }
            get { return windowWidth; }
        }
        public float WindowHeight
        {
            set
            {
                windowHeight = value;
                NotifyPropertyChanged("WindowHeight");
            }
            get { return windowHeight; }
        }
        public string Rules
        {
            get
            {
                return "⦿ Pieces must stay on the dark squares. \n" +
                    "⦿ To capture an opposing piece,\"jump\" over it by moving two diagonal spaces in the direction of the the opposing piece \n" +
                    "⦿ A piece may jump forward over an opponent's pieces in multiple parts of the board to capture them.\n" +
                    "⦿ Keep in mind, the space on the other side of your opponent’s piece must be empty for you to capture it.\n" +
                    "⦿ If your piece reaches the last row on your opponent's side, you may re-take one of your captured pieces and \"crown\"the piece that made it to the Kings Row.Thereby making it a \"King Piece.\"\n" +
                    "⦿ King pieces may still only move one space at a time during a non-capturing move. However, when capturing an opponent's piece(s) it may move diagonally forward or backwards\n" +
                    "⦿ There is no limit to how many king pieces a player may have.\n";
            }
        }
        private bool showStats;
        public bool ShowStats
        {
            set
            {
                showStats = value;
                NotifyPropertyChanged("ShowStats");
            }
            get
            {
                return showStats;
            }
        }
        private bool showAbout;
        public bool ShowAbout
        {
            set
            {
                showAbout = value;
                NotifyPropertyChanged("ShowAbout");
            }
            get
            {
                return showAbout;
            }
        }
        public void RestartGame()
        {
            ObservableCollection<ObservableCollection<Cell>> board = Utilities.InitGameBoard();
            this.GameLogic.Statistics.UpdateFile();
            this.GameLogic = new GameBusinessLogic(ref board);
            this.GameBoard = CellBoardToCellVMBoard(ref board);
        }
        public void SaveToXML()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                Utilities.SerializeObjectToXML<GameBusinessLogic>(businessLogic, saveFileDialog.FileName);
        }
        public void ReadFromXML()
        {
            OpenFileDialog openfile = new OpenFileDialog();
            string folderpath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Resources\\";

            bool? myResult;
            myResult = openfile.ShowDialog();
            if (myResult != null && myResult == true)
            { 
                var logic = Utilities.DeserializeObjectToXML<GameBusinessLogic>(openfile.FileName);
                this.GameLogic.Statistics.UpdateFile();
                this.GameLogic = logic;
                this.GameLogic.Statistics = new Statistics("Statistics.xml");
                var board = GameLogic.Board;
                GameBoard = CellBoardToCellVMBoard(ref board);
            }  
        }
        public void ShowStatistics()
        {
            ShowStats = true;
        }
        public void CloseStatistics()
        {
            ShowStats = false;
        }
        public void ShowAboutPopup()
        {
            ShowAbout = true;
        }
        public void CloseAbout()
        {
            ShowAbout = false;
        }
        private ICommand newGame;
        public ICommand NewGame
        {
            get
            {
                if (newGame == null)
                {
                    newGame = new RelayCommand<Object>(o => RestartGame());
                }
                return newGame;
            }
        }
        private ICommand saveGame;
        public ICommand SaveGame
        {
            get
            {
                if (saveGame == null)
                {
                    saveGame = new RelayCommand<Object>(o => SaveToXML());
                }
                return saveGame;
            }
        }
        private ICommand opneGame;
        public ICommand OpenGame
        {
            get
            {
                if (opneGame == null)
                {
                    opneGame = new RelayCommand<Object>(o => ReadFromXML());
                }
                return opneGame;
            }
        }

        private ICommand statisticsComand;
        public ICommand StatisticsComand
        {
            get
            {
                if (statisticsComand == null)
                {
                    statisticsComand = new RelayCommand<Object>(o => ShowStatistics());
                }
                return statisticsComand;
            }
        }
        private ICommand closeStats;
        public ICommand CloseStats
        {
            get
            {
                if (closeStats == null)
                {
                    closeStats = new RelayCommand<Object>(o => CloseStatistics());
                }
                return closeStats;
            }
        }
        private ICommand aboutComand;
        public ICommand AboutComand
        {
            get
            {
                if (aboutComand == null)
                {
                    aboutComand = new RelayCommand<Object>(o => ShowAboutPopup());
                }
                return aboutComand;
            }
        }
        private ICommand closeAbout;
        public ICommand CloseAboutPopup
        {
            get
            {
                if (closeAbout == null)
                {
                    closeAbout = new RelayCommand<Object>(o => CloseAbout());
                }
                return closeAbout;
            }
        }
    }
}

