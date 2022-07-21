using System;
using System.Collections.Generic;
using System.Linq;

namespace _1586_MonochromeTile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputs = GetInputs();
            var outputs = GetOutputs(inputs);
            ShowOutputs(outputs);
        }

        #region 演算処理

        // ここの命名
        private static IEnumerable<int> GetOutputs(Inputs inputs)
        {
            // TileRectangleクラスは作らないようにした
            var tiles = InitializeTiles(inputs.Width, inputs.Height);
            return TaroActs(tiles, inputs.CheckAreas);
        }

        private static IEnumerable<IEnumerable<Tile>> InitializeTiles(int width, int height)
        {
            var tiles = new List<IEnumerable<Tile>>();
            for (var i = 0; i < height; i++)
            {
                var tileRow = new List<Tile>();
                for (var j = 0; j < width; j++)
                {
                    var tile = new Tile(Color.White);
                    tileRow.Add(tile);
                }
                tiles.Add(tileRow);
            }

            return tiles;
        }

        private static IEnumerable<int> TaroActs(
            IEnumerable<IEnumerable<Tile>> tiles,
            IEnumerable<IEnumerable<int>> checkAreas)
        {
            var blackedTilesCounts = new List<int>();
            var total = 0;
            foreach (var checkAreaOneDay in checkAreas)
            {
                var oneDayCount = TaroActsOneDay(tiles, checkAreaOneDay);
                blackedTilesCounts.Add(total += oneDayCount);
            }

            return blackedTilesCounts;
        }

        private static int TaroActsOneDay(IEnumerable<IEnumerable<Tile>> tiles, IEnumerable<int> checkAreaOneDay)
        {
            var targetTiles = GetTilesInArea(tiles, checkAreaOneDay).ToList();
            if (CheckIfAllTilesWhite(targetTiles) == false)
            {
                return 0;
            }

            PaintTilesBlack(targetTiles);
            return targetTiles.Count;
        }

        private static IEnumerable<Tile> GetTilesInArea(IEnumerable<IEnumerable<Tile>> tiles, IEnumerable<int> area)
        {
            var tilesList = tiles.ToList();
            var areaList = area.ToList();
            var (left, top, right, bottom) = (areaList[0], areaList[1], areaList[2], areaList[3]);
            for (var i = top - 1; i < bottom; i++)
            {
                for (var j = left - 1; j < right; j++)
                {
                    yield return tilesList[i].ToList()[j];
                }
            }
        }

        private static bool CheckIfAllTilesWhite(IEnumerable<Tile> tiles)
        {
            return tiles.All(x => x.Color == Color.White);
        }

        private static void PaintTilesBlack(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.Color = Color.Black;
            }
        }

        #endregion

        #region 入力処理

        private class Inputs
        {
            public Inputs(int width, int height, IEnumerable<IEnumerable<int>> checkAreas)
            {
                this.Width = width;
                this.Height = height;
                this.CheckAreas = checkAreas;
            }

            public int Width { get; }

            public int Height { get; }
            // ここで純粋な入力にするのか、何かにConvertするのか悩んだ。入力処理の責務範囲
            public IEnumerable<IEnumerable<int>> CheckAreas { get; }
        }

        private static Inputs GetInputs()
        {
            //var (width, height) = GetRectangleSize();
            var (height, width) = GetRectangleSize(); // AOJ のテスト入力は高さ、横幅の順
            var numberOfDays = GetNumberOfDays();
            var numbersIndicatingRectangleAreaCollection = GetNumbersIndicatingRectangleAreaCollection(numberOfDays);
            return new Inputs(width, height, numbersIndicatingRectangleAreaCollection);
        }

        private static int GetNumberOfDays()
        {
            return ReadIntegerNumber();
        }

        private static (int, int) GetRectangleSize()
        {
            var numbers = ReadSpaceSeparatedIntegerNumbers().ToList();
            return (numbers[0], numbers[1]);
        }

        private static int ReadIntegerNumber()
        {
            var readLine = Console.ReadLine();
            int.TryParse(readLine, out var number);
            return number;
        }

        private static IEnumerable<IEnumerable<int>> GetNumbersIndicatingRectangleAreaCollection(int numberOfDays)
        {
            var numbersIndicatingRectangleAreaCollection = new List<IEnumerable<int>>();
            for (var i = 0; i < numberOfDays; i++)
            {
                var numbersIndicatingRectangleArea = GetNumbersIndicatingRectangleArea();
                numbersIndicatingRectangleAreaCollection.Add(numbersIndicatingRectangleArea);
            }

            return numbersIndicatingRectangleAreaCollection;
        }

        private static IEnumerable<int> GetNumbersIndicatingRectangleArea()
        {
            return ReadSpaceSeparatedIntegerNumbers();
        }

        private static IEnumerable<int> ReadSpaceSeparatedIntegerNumbers()
        {
            const string Space = " ";
            var readLine = Console.ReadLine();
            var splitLine = readLine.Split(Space);
            return splitLine.Select(int.Parse);
        }
        
        #endregion

        private static Inputs GetSampleInputs()
        {
            return new Inputs(
                5,
                4,
                new List<IEnumerable<int>>()
                    {
                        new List<int>() { 1, 1, 3, 3 },
                        new List<int>() { 3, 2, 4, 2 },
                        new List<int>() { 4, 3, 5, 4 },
                        new List<int>() { 1, 4, 5, 4 },
                        new List<int>() { 4, 1, 4, 1 },
                    });
        }

        #region 出力処理

        private static void ShowOutputs(IEnumerable<int> outputs)
        {
            foreach (var output in outputs)
            {
                Console.WriteLine(output);
            }
        }

        #endregion
    }



}
