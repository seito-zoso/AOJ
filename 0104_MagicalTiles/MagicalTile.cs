namespace _0104_MagicalTiles
{
    internal class MagicalTile
    {
        public MagicalTile(Direction arrowDirection)
        {
            this.ArrowDirection = arrowDirection;
        }

        public Direction ArrowDirection { get; }

        public bool Passed { get; set; }
    }
}
