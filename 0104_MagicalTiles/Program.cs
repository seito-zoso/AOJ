using System;
using System.Collections.Generic;
using System.Linq;

namespace _0104_MagicalTiles
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputs = GetInputs();
            var movementResults = GetMovementResults(inputs);
            ShowOutputs(movementResults);
        }

        #region 入力処理

        private class Input
        {
            public Input(int height, int width, IEnumerable<string> tilesStatus)
            {
                this.Height = height;
                this.Width = width;
                this.TilesStatus = tilesStatus;
            }

            public int Width { get; }

            public int Height { get; }

            public IEnumerable<string> TilesStatus { get; }
        }

        private static IEnumerable<Input> GetInputs()
        {
            var inputs = new List<Input>();
            while (true)
            {
                var input = GetInput();
                if (CheckIfEndInput(input))
                {
                    return inputs;
                }
                inputs.Add(input);
            }
        }

        private static bool CheckIfEndInput(Input input)
        {
            return input.Height == 0 && input.Width == 0;
        }

        private static Input GetInput()
        {
            var (height, width) = GetSize();
            var tilesStatus = GetTilesStatus(height);
            return new Input(height, width, tilesStatus);
        }

        private static (int, int) GetSize()
        {
            var numbers = ReadSpaceSeparatedIntegerNumbers().ToList();
            return (numbers[0], numbers[1]);
        }

        private static IEnumerable<string> GetTilesStatus(int height)
        {
            var tilesStatus = new List<string>();
            for (var i = 0; i < height; i++)
            {
                var tileStatusInRow = GetTileStatusInRow();
                tilesStatus.Add(tileStatusInRow);
            }

            return tilesStatus;
        }

        private static string GetTileStatusInRow()
        {
            return ReadString();
        }

        private static string ReadString()
        {
            return Console.ReadLine();
        }

        private static IEnumerable<int> ReadSpaceSeparatedIntegerNumbers()
        {
            const string Space = " ";
            var readLine = Console.ReadLine();
            var splitLine = readLine.Split(Space);
            return splitLine.Select(int.Parse);
        }

        private static IEnumerable<Input> GetSampleInputs()
        {
            return new List<Input>()
                       {
                           new Input(
                               10,
                               10,
                               new List<string>()
                                   {
                                       ">>>v..>>>v",
                                       "...v..^..v",
                                       ">>>>>>^..v",
                                       ".........v",
                                       ".v<<<<...v",
                                       ".v.v.^...v",
                                       ".v.v.^<<<<",
                                       ".v.v.....v",
                                       ".v...^...v",
                                       ".>>>>^....",
                                   }),
                           new Input(
                               6,
                               10,
                               new List<string>()
                                   {
                                       ">>>>>>>>>v",
                                       ".........v",
                                       ".........v",
                                       ">>>>v....v",
                                       "^...v....v",
                                       "^<<<<<<<<<",
                                   }),
                       };
        }

        #endregion


        #region 演算処理

        private class MovementResult
        {
            // 変な組み合わせ入れれないようにctorをprivateにしたが
            private MovementResult(bool isLoop, Position position)
            {
                this.IsLoop = isLoop;
                this.Position = position;
            }

            public static MovementResult CreateLoopResult()
            {
                return new MovementResult(true, null);
            }

            public static MovementResult CreateNonLoopResult(Position position)
            {
                return new MovementResult(false, position);
            }

            public bool IsLoop { get; }

            public Position Position { get; }
        }

        private static IEnumerable<MovementResult> GetMovementResults(IEnumerable<Input> inputs)
        {
            return inputs.Select(GetMovementResult);
        }

        private static MovementResult GetMovementResult(Input input)
        {
            var tiles = CreateMagicalTiles(input).ToList();
            var startPosition = GetStartPosition();
            return MoveUntilDetectLoopOrStop(tiles, startPosition);
        }

        private static IEnumerable<IList<MagicalTile>> CreateMagicalTiles(Input input)
        {
            foreach (var tileRow in input.TilesStatus)
            {
                yield return ConvertInputStringToMagicalTilesRow(tileRow).ToList();
            }
        }

        private static IEnumerable<MagicalTile> ConvertInputStringToMagicalTilesRow(string tileRowString)
        {
            foreach (var directionCharacter in tileRowString.ToCharArray())
            {
                var direction = ConvertInputCharacterToDirection(directionCharacter);
                yield return new MagicalTile(direction);
            }
        }

        private static Direction ConvertInputCharacterToDirection(char directionCharacter)
        {
            switch (directionCharacter)
            {
                case '.':
                    return Direction.None;
                case '>':
                    return Direction.East;
                case '<':
                    return Direction.West;
                case 'v':
                    return Direction.South;
                case '^':
                    return Direction.North;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Position GetStartPosition()
        {
            return new Position(0, 0);
        }

        private static MovementResult MoveUntilDetectLoopOrStop(IList<IList<MagicalTile>> tiles, Position currentPosition)
        {
            while (true)
            {
                var currentTile = GetTile(tiles, currentPosition);
                if (currentTile.Passed)
                {
                    return MovementResult.CreateLoopResult();
                }

                currentTile.Passed = true;

                var nextPosition = GetNextPosition(currentTile, currentPosition);
                var arriveGoal = PositionEquals(currentPosition, nextPosition);
                if (arriveGoal) return MovementResult.CreateNonLoopResult(currentPosition);
                currentPosition = nextPosition;
            }
        }
        private static MagicalTile GetTile(IList<IList<MagicalTile>> tiles, Position position)
        {
            return tiles[position.Y][position.X];
        }

        private static Position GetNextPosition(MagicalTile tile, Position position)
        {
            switch (tile.ArrowDirection)
            {
                case Direction.None:
                    return position;
                case Direction.East:
                    return new Position(position.X + 1, position.Y);
                case Direction.West:
                    return new Position(position.X - 1, position.Y);
                case Direction.South:
                    return new Position(position.X, position.Y + 1);
                case Direction.North:
                    return new Position(position.X, position.Y - 1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool PositionEquals(Position aPosition, Position bPosition)
        {
            return aPosition.X == bPosition.X && aPosition.Y == bPosition.Y;
        }

        #endregion

        #region 出力処理
        
        private static void ShowOutputs(IEnumerable<MovementResult> outputs)
        {
            foreach (var output in outputs)
            {
                if (output.IsLoop)
                {
                    Console.WriteLine("Loop");
                }
                else
                {
                    Console.WriteLine($"{output.Position.X} {output.Position.Y}");
                }
            }
        }

        #endregion

    }
}
