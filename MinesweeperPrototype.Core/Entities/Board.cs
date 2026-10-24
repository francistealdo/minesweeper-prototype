namespace MinesweeperPrototype.Core.Entities
{
    public sealed class Board
    {
        public int Rows { get; }
        public int Columns { get; }
        public int MineCount { get; }
        public Cell[,] Cells { get; }

        public Board(int rows, int columns, int mineCount)
        {
            Rows = rows;
            Columns = columns;
            MineCount = mineCount;

            Cells = new Cell[rows, columns];
        }
    }
}
