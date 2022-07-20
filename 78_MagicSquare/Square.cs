using System.Collections.Generic;
using System.Linq;

namespace _78_MagicSquare
{
    internal class Square
    {
        private readonly List<List<int>> cells;

        public Square(int size)
        {
            this.Size = size;
            this.cells = new List<List<int>>();
            for (var i = 0; i < size; i++)
            {
                var row = new List<int>();
                for (var j = 0; j < size; j++)
                {
                    row.Add(0);
                }
                this.cells.Add(row);
            }
        }

        public IEnumerable<IEnumerable<int>> Cells => this.cells;

        public int Size { get; }

        public int CenterCellIndex => this.Size / 2;

        public bool AreAllCellsFilled => this.cells.All(o => o.All(e => e != 0));

        public void SetNumberInCell(int number, int row, int column)
        {
            if (this.GetOutOfSquareStatus(row, column) != OutOfSquareStatus.NotOut)
            {
                return;
            }
            this.cells[row][column] = number;
        }

        public bool IsCellFilled(int row, int column)
        {
            if(this.GetOutOfSquareStatus(row, column) != OutOfSquareStatus.NotOut)
            {
                return false;
            }
            return this.cells[row][column] != 0;
        }

        public OutOfSquareStatus GetOutOfSquareStatus(int row, int column)
        {
            if (row >= this.Size && column >= this.Size)
            {
                return OutOfSquareStatus.OutFromRightAndBottom;
            }
            if (row >= this.Size)
            {
                return OutOfSquareStatus.OutFromBottom;
            }
            if (column >= this.Size)
            {
                return OutOfSquareStatus.OutFromRight;
            }
            if (column < 0)
            {
                return OutOfSquareStatus.OutFromLeft;
            }

            return OutOfSquareStatus.NotOut;
        }

        public int GetLeftEndEmptyCellIndex(int rowIndex)
        {
            if (rowIndex >= this.Size || rowIndex < 0)
            {
                return -1;
            }

            for (var i = 0; i < this.Size; i++)
            {
                if (this.cells[rowIndex][i] == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetRightEndEmptyCellIndex(int rowIndex)
        {
            if (rowIndex >= this.Size || rowIndex < 0)
            {
                return -1;
            }

            for (var i = this.Size - 1; i >= 0; i--)
            {
                if (this.cells[rowIndex][i] == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetTopEmptyCellIndex(int columnIndex)
        {
            if (columnIndex >= this.Size || columnIndex < 0)
            {
                return -1;
            }

            for (var i = 0; i < this.Size; i++)
            {
                if (this.cells[i][columnIndex] == 0)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
