using System;
using System.Collections.Generic;
using System.Linq;

namespace _0718_MoveBalls
{
    class Program
    {
        static void Main(string[] args)
        {
            var (numberOfBalls, operationList) = GetNumberOfBallsAndOperationList();
            var boxNumbersBallsIn = MoveBalls(numberOfBalls, operationList);
            DisplayIntegerNumbers(boxNumbersBallsIn);
        }

        #region 入力処理

        private static (int numberOfBalls, IReadOnlyList<(int ballNumber, int boxNumber)> operationList) GetNumberOfBallsAndOperationList()
        {
            var (numberOfBalls, numberOfOperations) = ReadSpaceSeparatedTwoIntegerNumbers();
            var operationList = GetOperationList(numberOfOperations);

            return (numberOfBalls, operationList);
        }

        private static IReadOnlyList<(int ballNumber, int boxNumber)> GetOperationList(int numberOfOperations)
        {
            var operationList = new List<(int, int)>();
            for (var i = 0; i < numberOfOperations; i++)
            {
                var operation = ReadSpaceSeparatedTwoIntegerNumbers();
                operationList.Add(operation);
            }

            return operationList;
        }

        private static (int, int) ReadSpaceSeparatedTwoIntegerNumbers()
        {
            const string Space = " ";
            var readLine = Console.ReadLine();
            var splitLine = readLine.Split(Space);

            return (int.Parse(splitLine[0]), int.Parse(splitLine[1]));
        }

        #endregion

        #region 演算処理

        private static IEnumerable<int> MoveBalls(int numberOfBalls, IReadOnlyList<(int ballNumber, int boxNumber)> operationList)
        {
            var boxes = InitializeBoxesContainingBalls(numberOfBalls);
            foreach (var operation in operationList)
            {
                MoveBall(boxes, operation.ballNumber, operation.boxNumber);
            }

            return GetBoxNumbersBallsIn(numberOfBalls, boxes);
        }

        private static IList<Box> InitializeBoxesContainingBalls(int numberOfBalls)
        {
            var boxes = new List<Box>();

            for (var i = 0; i < numberOfBalls; i++)
            {
                var ball = new Ball(i + 1); // ballのnewはここでいいの？
                var box = new Box(i + 1);
                box.Balls.Add(ball);
                boxes.Add(box);
            }

            return boxes;
        }

        private static void MoveBall(IList<Box> boxes, int ballNumber, int boxNumber)
        {
            var (ball, fromBox) = FindBallAndContainingBox(boxes, ballNumber);
            fromBox.Balls.Remove(ball);

            var toBox = boxes[boxNumber - 1];
            toBox.Balls.Add(ball);
        }

        private static (Ball, Box) FindBallAndContainingBox(IEnumerable<Box> boxes, int ballNumber)
        {
            Ball ball = null;
            var containingBox = boxes.FirstOrDefault(
                box =>
                    {
                        ball = box.Balls.FirstOrDefault(x => x.Number == ballNumber);
                        return ball != null;
                    });

            return (ball, containingBox);
        }

        private static IEnumerable<int> GetBoxNumbersBallsIn(int numberOfBalls, IList<Box> boxes)
        {
            for (var i = 0; i < numberOfBalls; i++)
            {
                var ballNumber = i + 1;
                yield return boxes.FirstOrDefault(box => box.Balls.Any(ball => ball.Number == ballNumber)).Number;
            }
        }

        #endregion

        #region 内部クラス

        private class Box
        {
            public Box(int number)
            {
                this.Number = number;
                this.Balls = new List<Ball>();
            }

            public int Number { get; }

            public ICollection<Ball> Balls { get; }
        }

        private class Ball
        {
            public Ball(int number)
            {
                this.Number = number;
            }

            public int Number { get; }
        }

        #endregion

        #region 出力処理

        private static void DisplayIntegerNumbers(IEnumerable<int> numbers)
        {
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }
        }

        #endregion

    }
}
