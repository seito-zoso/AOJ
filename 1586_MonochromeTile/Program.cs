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

        #region 内部クラス

        private class Inputs
        {
            public Inputs(int width, int height, IEnumerable<int[]> checkAreas)
            {
                this.Width = width;
                this.Height = height;
                this.CheckAreas = checkAreas;
            }

            public int Width { get; }

            public int Height { get; }

            public IEnumerable<int[]> CheckAreas { get; }
        }

        #endregion

        #region 入力処理

        private static Inputs GetInputs()
        {
            //var (width, height) = GetRectangleSize();
            var (height, width) = GetRectangleSize(); // AOJ のテスト入力は高さ、横幅の順
            var numberOfDays = ReadIntegerNumber();
            var rectangleAreaCollection = GetRectangleAreaCollection(numberOfDays);
            return new Inputs(width, height, rectangleAreaCollection);
        }

        private static (int, int) GetRectangleSize()
        {
            var numbers = ReadSpaceSeparatedIntegerNumbers().ToList();
            return (numbers[0], numbers[1]);
        }

        private static IEnumerable<int[]> GetRectangleAreaCollection(int numberOfDays)
        {
            var rectangleAreaCollection = new List<int[]>();
            for (var i = 0; i < numberOfDays; i++)
            {
                var rectangleArea = ReadSpaceSeparatedIntegerNumbers();
                rectangleAreaCollection.Add(rectangleArea);
            }

            return rectangleAreaCollection;
        }

        private static int ReadIntegerNumber()
        {
            var readLine = Console.ReadLine();
            int.TryParse(readLine, out var number);
            return number;
        }

        private static int[] ReadSpaceSeparatedIntegerNumbers()
        {
            const string Space = " ";
            var readLine = Console.ReadLine();
            var splitLine = readLine.Split(Space);
            return splitLine.Select(int.Parse).ToArray();
        }

        private static Inputs GetSampleInputs()
        {
            return new Inputs(
                5,
                4,
                new List<int[]>()
                    {
                        new [] { 1, 1, 3, 3 },
                        new [] { 3, 2, 4, 2 },
                        new [] { 4, 3, 5, 4 },
                        new [] { 1, 4, 5, 4 },
                        new [] { 4, 1, 4, 1 },
                    });
        }

        #endregion

        #region 演算処理

        // ここの命名
        private static IEnumerable<int> GetOutputs(Inputs inputs)
        {
            // TileRectangleクラスは作らないようにした
            var tiles = InitializeTiles(inputs.Width, inputs.Height);
            return TaroActs(tiles, inputs.CheckAreas);
        }

        private static Tile[,] InitializeTiles(int width, int height)
        {
            var tiles = new Tile[width, height];
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    tiles[i , j] = new Tile(Color.White);
                }
            }

            return tiles;
        }

        private static IEnumerable<int> TaroActs(
            Tile[,] tiles,
            IEnumerable<int[]> checkAreas)
        {
            var total = 0;
            foreach (var checkAreaOneDay in checkAreas)
            {
                var oneDayCount = PaintTilesBlack(tiles, checkAreaOneDay);
                yield return total += oneDayCount;
            }
        }

        private static int PaintTilesBlack(Tile[,] tiles, int[] checkAreaOneDay)
        {
            var targetTiles = GetTilesInArea(tiles, checkAreaOneDay).ToList();
            if (CheckAllTilesColor(targetTiles, Color.White) == false)
            {
                return 0;
            }

            PaintTiles(targetTiles, Color.Black);
            return targetTiles.Count;
        }

        private static IEnumerable<Tile> GetTilesInArea(Tile[,] tiles, int[] area)
        {
            var (left, top, right, bottom) = (area[0], area[1], area[2], area[3]);
            for (var i = top - 1; i < bottom; i++)
            {
                for (var j = left - 1; j < right; j++)
                {
                    yield return tiles[i, j];
                }
            }
        }

        private static bool CheckAllTilesColor(IEnumerable<Tile> tiles, Color color)
        {
            return tiles.All(x => x.Color == color);
        }

        private static void PaintTiles(IEnumerable<Tile> tiles, Color color)
        {
            foreach (var tile in tiles)
            {
                tile.Color = color;
            }
        }

        #endregion

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
