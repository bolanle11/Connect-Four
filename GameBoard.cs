// GameBoard - this contains the logic for the game board and how it how functions
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    public enum Space { Empty, Player1, Player2 }; //element placed in the game board
    
    public class GameBoard
    {
        //Connect4Board - an array of Space to store the game board
        public Space[,] Connect4Board { get; private set; }

        //rows - the number of rows the gameboard has
        //column - the number of column the gameboard has
        public GameBoard(int rows, int cols)
        {

            Connect4Board = new Space[rows, cols];
            
            //set the game board to spots empty
            for (int row = 0; row < this.Connect4Board.GetLength(0); row++)
                for (int col = 0; col < this.Connect4Board.GetLength(1); col++)
                    this.Connect4Board[row, col] = Space.Empty;
        }//GameBoard

        //Tied - verify if the game has ended in a tie
        public bool Tied()
        {
            for (int col = 0; col < this.Connect4Board.GetLength(1); col++)
                if (Connect4Board[0, col] == Space.Empty)
                    return false;
            return true;
        }//Tied

        //Winner() - determine the winner of the game and return the Space [Player1,Player2,Empty]
        public Space Winner()
        {
            for (int row = 0; row < this.Connect4Board.GetLength(0); row++)
            {
                for (int col = 0; col < this.Connect4Board.GetLength(1); col++)
                {
                    if (Connect4Board[row, col] != Space.Empty &&
                        (VerticalConnectFour(row, col) || HorizontalConnectFour(row, col) || ForwardDiagonalConnectFour(row, col) || BackwardDiagonalConnectFour(row, col)))
                        return Connect4Board[row, col];
                }
            }
            return Space.Empty;
        }//Winner()

        //Winner() - determine the winner of the game given the row and column and return the Space [Player1,Player2,Empty]
        public Space Winner(int row, int col)
        {
            if (Connect4Board[row, col] != Space.Empty &&
                (VerticalConnectFour(row, col) ||
                HorizontalConnectFour(row, col) ||
                ForwardDiagonalConnectFour(row, col) ||
                BackwardDiagonalConnectFour(row, col)))
                return Connect4Board[row, col];
            else
                return Space.Empty;
        }

        #region Winner helpers 
        //VerticalConnectFour - check for four circles Vertically
        //row - the row to check for four circles in a row 
        //col - the column to check for four circles in a row 
        private bool VerticalConnectFour(int row, int col)
        {
            if (Connect4Board[row, col] == Space.Empty)
                return false;
            int count = 1;
            int rowCursor = row - 1;
            while (rowCursor >= 0 && Connect4Board[rowCursor, col] == Connect4Board[row, col])
            {
                count++;
                rowCursor--;
            }
            rowCursor = row + 1;
            while (rowCursor < Connect4Board.GetLength(0) && Connect4Board[rowCursor, col] == Connect4Board[row, col])
            {
                count++;
                rowCursor++;
            }
            if (count < 4)
                return false;
            return true;
        }//VerticalConnectFour

        //HorizontalConnectFour - check for four circles Horizontally
        //row - the row to check for four circles in a row 
        //col - the column to check for four circles in a row 
        private bool HorizontalConnectFour(int row, int col)
        {
            if (Connect4Board[row, col] == Space.Empty)
                return false;
            int count = 1;
            int colCursor = col - 1;
            while (colCursor >= 0 && Connect4Board[row, colCursor] == Connect4Board[row, col])
            {
                count++;
                colCursor--;
            }
            colCursor = col + 1;
            while (colCursor < Connect4Board.GetLength(1) && Connect4Board[row, colCursor] == Connect4Board[row, col])
            {
                count++;
                colCursor++;
            }

            if (count < 4)
                return false;
            return true;
        }//HorizontalConnectFour

        //ForwardDiagonalConnectFour - check for four circles Forward Diagonally
        //row - the row to check for four circles in a row 
        //col - the column to check for four circles in a row 
        private bool ForwardDiagonalConnectFour(int row, int col)
        {
            if (Connect4Board[row, col] == Space.Empty)
                return false;
            int count = 1;
            int rowCursor = row - 1;
            int colCursor = col + 1;
            while (rowCursor >= 0 && colCursor < Connect4Board.GetLength(1) && Connect4Board[rowCursor, colCursor] == Connect4Board[row, col])
            {
                count++;
                rowCursor--;
                colCursor++;
            }
            rowCursor = row + 1;
            colCursor = col - 1;
            while (rowCursor < Connect4Board.GetLength(0) && colCursor >= 0 && Connect4Board[rowCursor, colCursor] == Connect4Board[row, col])
            {
                count++;
                rowCursor++;
                colCursor--;
            }
            //verify that four cirles in a row was found
            if (count < 4)
                return false;
            return true;
        }//ForwardDiagonalConnectFour

        //BackwardDiagonalConnectFour - check for four circles Backward Diagonally
        //row - the row to check for four circles in a row 
        //col - the column to check for four circles in a row 
        private bool BackwardDiagonalConnectFour(int row, int col)
        {
            if (Connect4Board[row, col] == Space.Empty)
                return false;
            int count = 1;
            int rowCursor = row + 1;
            int colCursor = col + 1;
            while (rowCursor < Connect4Board.GetLength(0) && colCursor < Connect4Board.GetLength(1) && Connect4Board[rowCursor, colCursor] == Connect4Board[row, col])
            {
                count++;
                rowCursor++;
                colCursor++;
            }
            rowCursor = row - 1;
            colCursor = col - 1;
            while (rowCursor >= 0 && colCursor >= 0 && Connect4Board[rowCursor, colCursor] == Connect4Board[row, col])
            {
                count++;
                rowCursor--;
                colCursor--;
            }
            //found 4 circles in a row
            if (count < 4)
                return false;
            return true;
        }//BackwardDiagonalConnectFour
        #endregion

        //InsertCircle - inserts the cirlce in the column specified then return a bool value if was successful
        //circle - the player of the cirle
        //column - the column to insert the circle
        public bool InsertCircle(Space circle, int column)
        {
            for (int row = Connect4Board.GetLength(0) - 1; row >= 0; row--)
            {
                //check to see if the column is empty
                if (Connect4Board[row, column] == Space.Empty)
                {
                    Connect4Board[row, column] = circle;
                    return true;
                }
            }
            return false;
        }//InsertCircle

        //CheckColumn - check column 
        //column - the column to check
        public bool CheckColumn(int column)
        {
            if (this.CirlcesInColumn(column) < 6)
            {
                return true;
            }//if
            return false;
        }//CheckColumn

        //CirlcesInColumn - returns the number of circles in a column of the board
        //column - the column to check for amount of circles
        public int CirlcesInColumn(int column)
        {
            int numCircles = 0;//the number of circles in a column

            //calculate the number of cirles in a column
            for (int row = Connect4Board.GetLength(0) - 1; row >= 0; row--)
            {
                if (Connect4Board[row, column] != Space.Empty)
                {
                    numCircles++;
                }
            }

            return numCircles;
        }//CirlcesInColumn

    }//GameBoard
}
