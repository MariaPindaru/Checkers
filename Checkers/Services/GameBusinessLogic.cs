using Checkers.Models;
using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Checkers.Services
{
    public class GameBusinessLogic : INotifyPropertyChanged
    {
        public GameBusinessLogic() { }
        public GameBusinessLogic(ref ObservableCollection<ObservableCollection<Cell>> cells)
        {
            this.board = cells;
            this.firstPlayer = new Player(Piece.Color.Black);
            this.secondPlayer = new Player(Piece.Color.White);
            this.FirstPlayer.IsMyTurn = true;
            this.IsGameOver = false;
            this.MultipleJumps = false;
            this.Statistics = new Statistics("Statistics.xml");
        }

        private Player firstPlayer, secondPlayer;
        public Player FirstPlayer
        {
            set
            {
                firstPlayer = value;
            }
            get
            {
                return firstPlayer;
            }
        }
        public Player SecondPlayer
        {
            set
            {
                secondPlayer = value;
            }
            get
            {
                return secondPlayer;
            }
        }

        private bool isGameOver;
        public bool IsGameOver
        {
            set
            {
                isGameOver = value;
                NotifyPropertyChanged("IsGameOver");
            }
            get
            {
                return isGameOver;
            }
        }
        private bool multipleJumps;
        public bool MultipleJumps
        {
            set
            {
                multipleJumps = value;

                NotifyPropertyChanged("MultipleJumps");
            }
            get
            {
                return multipleJumps;
            }
        }

        private ObservableCollection<ObservableCollection<Cell>> board;
        public ObservableCollection<ObservableCollection<Cell>> Board
        {
            set
            {
                board = value;
            }
            get
            {
                return board;
            }
        }
        public string PlayerToMove
        {
            set
            {

            }
            get
            {
                if (isGameOver == true)
                    return CurrentPlayer().Username + " won!";
                return CurrentPlayer().Username + ", it's your turn!";
            }
        }
        private Statistics statistics;
        [XmlIgnore]
        public Statistics Statistics
        {
            set
            {
                statistics = value;
                NotifyPropertyChanged("Statistics");
            }
            get
            {
                return statistics;
            }
        }

        private List<Cell> possibleMoves;
        private Dictionary<Cell, Cell> possibleJumps;
        private Player CurrentPlayer()
        {
            if (firstPlayer.IsMyTurn)
                return firstPlayer;
            return secondPlayer;
        }
        private void SwichPlayers()
        {
            firstPlayer.ChangeTurn();
            secondPlayer.ChangeTurn();

            int piecesLeft = 0;
            board.ToList().ForEach(row => row.ToList().ForEach(cell =>
            {
                if (cell.CurrentPiece != null && cell.CurrentPiece.PieceColor == CurrentPlayer().PlayerColor) 
                    ++piecesLeft;
            }));
            if (piecesLeft == 0)
            {
                isGameOver = true;
                firstPlayer.ChangeTurn();
                secondPlayer.ChangeTurn();
                if (CurrentPlayer().PlayerColor == Piece.Color.Black)
                    Statistics.BlackWins++;
                else
                    Statistics.WhiteWins++;
            }

            NotifyPropertyChanged("PlayerToMove");
        }
        private bool Promovable(Cell cell)
        {
            if (!cell.CurrentPiece.King && cell.CurrentPiece.PieceColor == Piece.Color.Black && cell.Position.X == 0)
                return true;
            if (!cell.CurrentPiece.King && cell.CurrentPiece.PieceColor == Piece.Color.White && cell.Position.X == 7)
                return true;
            return false;
        }
        private Tuple<Cell, Cell> LeftUpMove(Cell cell, int step = 1)
        {
            int x = cell.Position.X, y = cell.Position.Y;
            if (x > step - 1 && y > step - 1 && board[x - step][y - step].CurrentPiece == null)
            {
                if(step == 2 && board[x - step + 1][y - step + 1].CurrentPiece != null && board[x - step + 1][y - step + 1].CurrentPiece.PieceColor != cell.CurrentPiece.PieceColor)
                    return new Tuple<Cell, Cell>(board[x - step][y - step], board[x - step + 1][y - step + 1]);
                if(step == 1)
                    return new Tuple<Cell, Cell>(board[x - step][y - step], null);
            }
            return null;
        }
        private Tuple<Cell, Cell> LeftDownMove(Cell cell, int step = 1)
        {
            int x = cell.Position.X, y = cell.Position.Y;
            if (x < 8 - step && y > step - 1 && board[x + step][y - step].CurrentPiece == null)
            {
                if (step == 2 && board[x + step - 1][y - step + 1].CurrentPiece != null && board[x + step - 1][y - step + 1].CurrentPiece.PieceColor != cell.CurrentPiece.PieceColor)
                    return new Tuple<Cell, Cell>(board[x + step][y - step], board[x + step - 1][y - step + 1]);
                if(step == 1)
                    return new Tuple<Cell, Cell>(board[x + step][y - step], null);
            }
            return null;
        }
        private Tuple<Cell, Cell> RightDownMove(Cell cell, int step = 1)
        {
            int x = cell.Position.X, y = cell.Position.Y;
            if (x < 8 - step && y < 8 - step && board[x + step][y + step].CurrentPiece == null)
            {
                if (step == 2 && board[x + step - 1][y + step - 1].CurrentPiece != null && board[x + step - 1][y + step - 1].CurrentPiece.PieceColor != cell.CurrentPiece.PieceColor)
                    return new Tuple<Cell, Cell>(board[x + step][y + step], board[x + step - 1][y + step - 1]);
                if(step == 1)
                    return new Tuple<Cell, Cell>(board[x + step][y + step], null);
            }
            return null;
        }
        private Tuple<Cell, Cell> RightUpMove(Cell cell, int step = 1)
        {
            int x = cell.Position.X, y = cell.Position.Y;
            if (x > step - 1 && y < 8 - step && board[x - step][y + step].CurrentPiece == null)
            {
                if (step == 2 && board[x - step + 1][y + step - 1].CurrentPiece != null && board[x - step + 1][y + step - 1].CurrentPiece.PieceColor != cell.CurrentPiece.PieceColor)
                    return new Tuple<Cell, Cell>(board[x - step][y + step], board[x - step + 1][y + step - 1]);
                if(step == 1)
                    return new Tuple<Cell, Cell>(board[x - step][y + step], null);
            }
            return null;
        }
        private List<Cell> GetMoves(Cell cell, int step = 1)
        {
            if (cell.CurrentPiece.King || cell.CurrentPiece.PieceColor == Piece.Color.Black)
            {
                if (RightUpMove(cell, step) is Tuple<Cell, Cell> rightUp)
                {
                    if (rightUp.Item2 == null)
                        possibleMoves.Add(rightUp.Item1);
                    else
                        possibleJumps[rightUp.Item1] = rightUp.Item2;
                }
                if (LeftUpMove(cell, step) is Tuple<Cell, Cell> leftUp)
                {
                    if(leftUp.Item2 == null)
                        possibleMoves.Add(leftUp.Item1);
                    else
                        possibleJumps[leftUp.Item1] = leftUp.Item2;
                }
            }
            if (cell.CurrentPiece.King || cell.CurrentPiece.PieceColor == Piece.Color.White)
            {
                if (RightDownMove(cell, step) is Tuple<Cell, Cell> rightDown)
                {
                    if(rightDown.Item2 == null)
                        possibleMoves.Add(rightDown.Item1);
                    else
                        possibleJumps[rightDown.Item1] = rightDown.Item2;
                }
                if (LeftDownMove(cell, step) is Tuple<Cell, Cell> lelfDown)
                {
                    if(lelfDown.Item2 == null)
                        possibleMoves.Add(lelfDown.Item1);
                    else
                        possibleJumps[lelfDown.Item1] = lelfDown.Item2;
                }
            }
            return possibleMoves;
        }
        private void GetPossibleMoves(Cell selectedCell)
        {
            possibleJumps = new Dictionary<Cell, Cell>();
            possibleMoves = new List<Cell>();

            GetMoves(selectedCell);
            GetMoves(selectedCell, 2);

            possibleMoves.ForEach(x => x.IsSelected = true);
            possibleJumps.ToList().ForEach(x => x.Key.IsSelected = true);
        }
        private void HandleMove(Cell currentCell)
        {
            if (Utilities.SelectedCell != null)
            {
                if (currentCell.CurrentPiece == null)
                {
                    if (possibleMoves.Contains(currentCell) && !Utilities.InAMultipleJump)
                    {
                        currentCell.CurrentPiece = Utilities.SelectedCell.CurrentPiece;
                        Utilities.SelectedCell.CurrentPiece = null;

                        if (Promovable(currentCell))
                            currentCell.CurrentPiece.King = true;

                        SwichPlayers();
                    }
                    else if (possibleJumps.ContainsKey(currentCell))
                    {
                        currentCell.CurrentPiece = Utilities.SelectedCell.CurrentPiece;
                        Utilities.SelectedCell.CurrentPiece = null;
                        possibleJumps[currentCell].CurrentPiece = null;

                        if (Promovable(currentCell))
                            currentCell.CurrentPiece.King = true;

                        if (multipleJumps)
                        {
                            board.ToList().ForEach(row => row.ToList().ForEach(cell => cell.IsSelected = false));
                            possibleJumps = new Dictionary<Cell, Cell>();
                            GetMoves(currentCell, 2);
                            if (possibleJumps.Count > 0)
                            {
                                Utilities.InAMultipleJump = true;
                                possibleJumps.ToList().ForEach(x => x.Key.IsSelected = true);
                                Utilities.SelectedCell = currentCell;
                                Utilities.SelectedCell.IsSelected = true;
                                return;
                            }
                            else
                            {
                                Utilities.InAMultipleJump = false;
                            }
                        }
                        SwichPlayers();
                    }
                }
                if (Utilities.InAMultipleJump)
                    return;
                board.ToList().ForEach(row => row.ToList().ForEach(cell => cell.IsSelected = false));
                Utilities.SelectedCell = null;
                return;
            }

            if (currentCell.CurrentPiece != null && CurrentPlayer().PlayerColor == currentCell.CurrentPiece.PieceColor)
            {
                Utilities.SelectedCell = currentCell;
                Utilities.SelectedCell.IsSelected = true;
                GetPossibleMoves(Utilities.SelectedCell);
            }
        }
        public void ClickAction(Cell obj)
        {
            if(!isGameOver)
                HandleMove(obj);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
