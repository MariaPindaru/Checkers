using Checkers.Models;
using Checkers.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Checkers.Services
{
    class Utilities
    {
        private static int BoardDimension = 8;

        public static bool InAMultipleJump = false;
        public static Cell SelectedCell { get; set; }
        public static ObservableCollection<ObservableCollection<Cell>> InitGameBoard()
        {
            ObservableCollection<ObservableCollection<Cell>> cells = new ObservableCollection<ObservableCollection<Cell>>();
            for (int line = 0; line < BoardDimension; ++line)
            {
                ObservableCollection<Cell> row = new ObservableCollection<Cell>();
                for(int column = 0; column < BoardDimension; ++column)
                {
                    row.Add(new Cell(line, column));
                }
                cells.Add(row);
            }
            for (int column = 0; column < BoardDimension; ++column)
            {
                if(column % 2 == 0)
                {
                    cells[1][column].CurrentPiece = new Piece(Piece.Color.White);
                    cells[5][column].CurrentPiece = new Piece(Piece.Color.Black);
                    cells[7][column].CurrentPiece = new Piece(Piece.Color.Black);
                }
                else
                {
                    cells[0][column].CurrentPiece = new Piece(Piece.Color.White);
                    cells[2][column].CurrentPiece = new Piece(Piece.Color.White);
                    cells[6][column].CurrentPiece = new Piece(Piece.Color.Black);
                }
            }

            return cells;
        }
        public static void SerializeObjectToXML<T>(T item, string FilePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(FilePath))
            {
                xs.Serialize(wr, item);
            }
        }
        public static T DeserializeObjectToXML<T>(string FilePath)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Declare an object variable of the type to be deserialized.
            T targetObj;

            // A FileStream is needed to read the XML document.
            using (FileStream fs = new FileStream(FilePath, FileMode.Open))
            {
                XmlReader reader = XmlReader.Create(fs);
                // Use the Deserialize method to restore the object's state.
                targetObj = (T)serializer.Deserialize(reader);
            }
            return targetObj;
            //XmlSerializer xs = new XmlSerializer(typeof(T));
            //TextReader reader = new StreamReader(FilePath);
            //item =  (T)xs.Deserialize(reader);
        }
    }
}
