using System;
using System.Collections.Generic;
using System.Linq;

namespace _78_MagicSquare
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var magicSquaresSizes = GetMagicSquaresSizes();
            var magicSquares = CreateMagicSquares(magicSquaresSizes);
            OutputSquares(magicSquares);
        }

        private static IEnumerable<int> GetMagicSquaresSizes()
        {
            var sizes = new List<int>();
            while (true)
            {
                var readLine = Console.ReadLine();
                var number = int.Parse(readLine);
                if (number == 0)
                {
                    break;
                }
                sizes.Add(number);
            }

            return sizes;
        }

        private static IEnumerable<Square> CreateMagicSquares(IEnumerable<int> magicSquaresSizes)
        {
            return magicSquaresSizes.Select(CreateMagicSquare).ToList();
        }

        private static Square CreateMagicSquare(int squareSize)
        {
            var magicSquare = new Square(squareSize);
            FillMagicSquareCell(magicSquare, 1, magicSquare.CenterCellIndex + 1, magicSquare.CenterCellIndex);

            return magicSquare;
        }

        private static void FillMagicSquareCell(Square magicSquare, int number, int row, int column)
        {
            if (magicSquare.AreAllCellsFilled)
            {
                return;
            }

            var status = magicSquare.GetOutOfSquareStatus(row, column);
            switch (status)
            {
                case OutOfSquareStatus.NotOut:
                    if (magicSquare.IsCellFilled(row, column) == false)
                    {
                        magicSquare.SetNumberInCell(number, row, column);
                        PutNextNumberInRightDownCell(magicSquare, number, row, column);
                    }
                    else
                    {
                        PutNumberInLeftDownCell(magicSquare, number, row, column);
                    }
                    break;

                case OutOfSquareStatus.OutFromRight:
                    PutNumberInLeftEndEmptyCell(magicSquare, number, row);
                    break;

                case OutOfSquareStatus.OutFromLeft:
                    PutNumberInRightEndEmptyCell(magicSquare, number, row);
                    break;

                case OutOfSquareStatus.OutFromBottom:
                    PutNumberInTopEmptyCell(magicSquare, number, column);
                    break;

                case OutOfSquareStatus.OutFromRightAndBottom:
                    var squareInsideColumn = column - 1;
                    PutNumberInTopEmptyCell(magicSquare, number, squareInsideColumn);
                    break;
            }
        }

        private static void PutNextNumberInRightDownCell(Square magicSquare, int number, int row, int column)
        {
            FillMagicSquareCell(magicSquare, number + 1, row + 1, column + 1);
        }

        private static void PutNumberInLeftDownCell(Square magicSquare, int number, int row, int column)
        {
            FillMagicSquareCell(magicSquare, number, row + 1, column - 1);
        }
        
        private static void PutNumberInTopEmptyCell(Square magicSquare, int number, int column)
        {
            FillMagicSquareCell(magicSquare, number, magicSquare.GetTopEmptyCellIndex(column), column);
        }

        private static void PutNumberInRightEndEmptyCell(Square magicSquare, int number, int row)
        {
            FillMagicSquareCell(magicSquare, number, row, magicSquare.GetRightEndEmptyCellIndex(row));
        }

        private static void PutNumberInLeftEndEmptyCell(Square magicSquare, int number, int row)
        {
            FillMagicSquareCell(magicSquare, number, row, magicSquare.GetLeftEndEmptyCellIndex(row));
        }

        private static void OutputSquares(IEnumerable<Square> squares)
        {
            foreach (var square in squares)
            {
                OutputSquare(square);
            }

            Console.ReadLine();
        }

        private static void OutputSquare(Square square)
        {
            foreach (var rowCells in square.Cells)
            {
                foreach (var cell in rowCells)
                {
                    /* マス目に入る各数字は右詰 4 桁で出力してください */
                    Console.Write($"{cell,4}");
                }

                Console.WriteLine();
            }
        }
    }
}
