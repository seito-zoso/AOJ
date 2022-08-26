using System;
using System.Collections.Generic;
using System.Linq;

namespace _1586_MonochromeTile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var (tileSize, checkAreas)  = GetTileSizeAndCheckAreas();
            var outputs = CreateMonochromeTileAndCountBlackedTiles(tileSize, checkAreas);
            ShowOutputs(outputs);
        }

        #region 入力処理

        private static ((int width, int height), IEnumerable<int[]>) GetTileSizeAndCheckAreas()
        {
            //var (width, height) = GetRectangleSize();
            var (height, width) = GetRectangleSize(); // AOJ のテスト入力は高さ、横幅の順
            var numberOfDays = ReadIntegerNumber();
            var rectangleAreaCollection = GetRectangleAreaCollection(numberOfDays);
            return ((width, height), rectangleAreaCollection);
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

        #endregion

        #region 演算処理

        private static IEnumerable<int> CreateMonochromeTileAndCountBlackedTiles((int width, int height) tileSize, IEnumerable<int[]> checkAreas)
        {
            var whiteTiles = InitializeTiles(tileSize.width, tileSize.height, Color.White);
            return PaintAllCheckAreasBlack(whiteTiles, checkAreas);
        }

        private static Tile[,] InitializeTiles(int width, int height, Color color)
        {
            var tiles = new Tile[width, height];
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    tiles[i , j] = new Tile(color);
                }
            }

            return tiles;
        }

        private static IEnumerable<int> PaintAllCheckAreasBlack(
            Tile[,] tiles,
            IEnumerable<int[]> checkAreas)
        {
            var totalCount = 0;
            foreach (var checkArea in checkAreas)
            {
                var blackedTileCount = PaintCheckAreaBlackTilesBlack(tiles, checkArea);
                yield return totalCount += blackedTileCount;
            }
        }

        private static int PaintCheckAreaBlackTilesBlack(Tile[,] tiles, int[] checkArea)
        {
            var targetTiles = GetTilesInArea(tiles, checkArea).ToList();
            var araAllTilesWhite = CheckAllTilesColor(targetTiles, Color.White);
            if (araAllTilesWhite  == false)
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
