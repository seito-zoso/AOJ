using System.Collections.Generic;

namespace _0556_Tile
{
    internal class TileMural
    {
        public TileMural(int sideLength)
        {
            this.Tiles = new Tile[sideLength, sideLength];
            // すべてnullで生成されるがそれは正常なのか
        }

        public Tile[,] Tiles { get; }
    }
}
