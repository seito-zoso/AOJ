using System;
using System.Collections.Generic;
using System.Linq;

namespace _0556_Tile
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputs = GetInputs();
            var outputs = GetOutputs(inputs);
            ShowOutputs(outputs);
        }

        #region 入力処理

        private class Inputs
        {
            public Inputs(int sideLength, IEnumerable<PlanePosition> positionsOfPeeledOffTiles)
            {
                this.SideLength = sideLength;
                this.PositionsOfPeeledOffTiles = positionsOfPeeledOffTiles;
            }
            public int SideLength { get; }
            public IEnumerable<PlanePosition> PositionsOfPeeledOffTiles { get; }
        }

        private static Inputs GetInputs()
        {
            var sideLength = GetSideLength();
            var numberOfPeeledOffTiles = GetNumberOfPeeledOffTiles();
            var positionsOfPeeledOffTiles = GetPositionsOfPeeledOffTiles(numberOfPeeledOffTiles);
            return new Inputs(sideLength, positionsOfPeeledOffTiles);
        }
        
        private static int GetSideLength()
        {
            return ReadIntegerNumber();
        }

        private static int GetNumberOfPeeledOffTiles()
        {
            return ReadIntegerNumber();
        }

        private static IEnumerable<PlanePosition> GetPositionsOfPeeledOffTiles(int numberOfPeeledOffTiles)
        {
            var positionsOfPeeledOffTiles = new List<PlanePosition>();
            for (var i = 0; i < numberOfPeeledOffTiles; i++)
            {
                var position = GetPositionOfPeeledOffTile();
                positionsOfPeeledOffTiles.Add(position);
            }
            return positionsOfPeeledOffTiles;
        }

        private static PlanePosition GetPositionOfPeeledOffTile()
        {
            var positions = ReadSpaceSeparatedIntegerNumbers().ToList();
            return new PlanePosition(positions[0], positions[1]);
        }

        private static int ReadIntegerNumber()
        {
            var readLine = Console.ReadLine();
            int.TryParse(readLine, out var number);
            return number;
        }

        private static IEnumerable<int> ReadSpaceSeparatedIntegerNumbers()
        {
            const string Space = " ";
            var readLine = Console.ReadLine();
            var splitLine = readLine.Split(Space);
            return splitLine.Select(int.Parse);
        }

        #endregion

        #region サンプル入力
        
        private static Inputs GetSampleInputs1()
        {
            return new Inputs(
                11,
                new[]
                    {
                        new PlanePosition(5, 2),
                        new PlanePosition(9, 7),
                        new PlanePosition(4, 4),
                        new PlanePosition(3, 9),
                    });
        }

        private static Inputs GetSampleInputs2()
        {
            return new Inputs(
                16,
                new[]
                    {
                        new PlanePosition(3, 7),
                        new PlanePosition(5, 2),
                        new PlanePosition(11, 6),
                        new PlanePosition(15, 2),
                        new PlanePosition(9, 7),
                        new PlanePosition(8, 12),
                        new PlanePosition(15, 16),
                    });
        }

        private static Inputs GetSampleInputs3()
        {
            return new Inputs(
                10,
                new[]
                    {
                        new PlanePosition(4, 7),
                        new PlanePosition(2, 10),
                        new PlanePosition(6, 2),
                        new PlanePosition(7, 10),
                        new PlanePosition(7, 9),
                        new PlanePosition(1, 1),
                        new PlanePosition(6, 5),
                        new PlanePosition(8, 3),
                    });
        }

        #endregion

        #region 演算処理：出力の取得

        private static IEnumerable<Color> GetOutputs(Inputs inputs)
        {
            var tileMural = CreateTileMural(inputs.SideLength);
            var colors = GetColorsOfPeeledOffTiles(tileMural, inputs.PositionsOfPeeledOffTiles);
            return colors;
        }

        #endregion

        #region 演算処理：タイルの配置

        private static Tile[,] CreateTileMural(int sideLength)
        {
            var tileMural = new Tile[sideLength, sideLength];
            ArrangeTilesInOrderFromOutside(tileMural);
            return tileMural;
        }

        private static void ArrangeTilesInOrderFromOutside(Tile[,] tileMural)
        {
            var sideLength = tileMural.GetLength(0);
            var startPosition = new PlanePosition(0, 0);
            var color = Color.Red;

            while (true)
            {
                if (sideLength <= 0)
                {
                    return;
                }

                ArrangeOutermostTilesClockwise(tileMural, startPosition, sideLength, color);
                sideLength -= 2;
                startPosition = new PlanePosition(startPosition.X + 1, startPosition.Y + 1);
                color = GetNextColor(color);
            }
        }

        private static Color GetNextColor(Color color)
        {
            switch (color)
            {
                case Color.Red:
                    return Color.Blue;
                case Color.Blue:
                    return Color.Yellow;
                case Color.Yellow:
                    return Color.Red;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        private static void ArrangeOutermostTilesClockwise(
            Tile[,] tileMural,
            PlanePosition startPosition,
            int sideLength,
            Color color)
        {
            ArrangeTopSideTiles(tileMural, startPosition, sideLength, color);
            ArrangeRightSideTiles(tileMural, startPosition, sideLength, color);
            ArrangeBottomSideTiles(tileMural, startPosition, sideLength, color);
            ArrangeLeftSideTiles(tileMural, startPosition, sideLength, color);
        }

        private static void ArrangeTopSideTiles(
            Tile[,] tileMural,
            PlanePosition topLeftPosition,
            int sideLength,
            Color color)
        {
            var topRightPosition = new PlanePosition(topLeftPosition.X + sideLength - 1, topLeftPosition.Y);

            ArrangeTilesHorizontally(tileMural, topLeftPosition, topRightPosition, color);
        }

        private static void ArrangeRightSideTiles(
            Tile[,] tileMural,
            PlanePosition topLeftPosition,
            int sideLength,
            Color color)
        {
            var topRightPosition = new PlanePosition(topLeftPosition.X + sideLength - 1, topLeftPosition.Y);
            var bottomRightPosition = new PlanePosition(topLeftPosition.X + sideLength - 1, topLeftPosition.Y + sideLength - 1);

            ArrangeTilesVertically(tileMural, topRightPosition, bottomRightPosition, color);
        }

        private static void ArrangeBottomSideTiles(
            Tile[,] tileMural,
            PlanePosition topLeftPosition,
            int sideLength,
            Color color)
        {
            var bottomRightPosition = new PlanePosition(topLeftPosition.X + sideLength - 1, topLeftPosition.Y + sideLength - 1);
            var bottomLeftPosition = new PlanePosition(topLeftPosition.X, topLeftPosition.Y + sideLength - 1);

            ArrangeTilesHorizontally(tileMural, bottomRightPosition, bottomLeftPosition, color);
        }

        private static void ArrangeLeftSideTiles(
            Tile[,] tileMural,
            PlanePosition topLeftPosition,
            int sideLength,
            Color color)
        {
            var bottomLeftPosition = new PlanePosition(topLeftPosition.X, topLeftPosition.Y + sideLength - 1);

            ArrangeTilesVertically(tileMural, bottomLeftPosition, topLeftPosition, color);
        }

        private static void ArrangeTilesVertically(
            Tile[,] tileMural,
            PlanePosition startPosition,
            PlanePosition endPosition,
            Color color)
        {
            var x = startPosition.X;
            var startY = startPosition.Y;
            var endY = endPosition.Y;

            if (endY < startY)
            {
                (startY, endY) = (endY, startY);
            }

            for (var i = startY; i <= endY; i++)
            {
                tileMural[x, i] = new Tile(color);
            }
        }

        private static void ArrangeTilesHorizontally(
            Tile[,] tileMural,
            PlanePosition startPosition,
            PlanePosition endPosition,
            Color color)
        {
            var y = startPosition.Y;
            var startX = startPosition.X;
            var endX = endPosition.X;

            if (endX < startX)
            {
                (startX, endX) = (endX, startX);
            }

            for (var i = startX; i <= endX; i++)
            {
                tileMural[i, y] = new Tile(color);
            }
        }

        #endregion

        #region 演算処理：剥がれ落ちたタイルの色を取得

        private static IEnumerable<Color> GetColorsOfPeeledOffTiles(
            Tile[,] tileMural,
            IEnumerable<PlanePosition> positionsOfPeeledOffTiles)
        {
            return positionsOfPeeledOffTiles
                .Select(x => GetTileColor(tileMural, x)).ToList();
        }

        private static Color GetTileColor(Tile[,] tileMural, PlanePosition position)
        {
            return tileMural[position.X - 1, position.Y - 1].Color;
        }

        #endregion

        #region 出力処理

        private static void ShowOutputs(IEnumerable<Color> outputs)
        {
            foreach (var output in outputs)
            {
                Console.WriteLine((int)output);
            }
        }

        #endregion 
    }
}
