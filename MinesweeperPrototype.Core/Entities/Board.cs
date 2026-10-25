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
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows));
            }

            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns));
            }

            if (mineCount < 0 || mineCount >= rows * columns)
            {
                throw new ArgumentOutOfRangeException(nameof(mineCount));
            }

            Rows = rows;
            Columns = columns;
            MineCount = mineCount;

            Cells = new Cell[rows, columns];

            InitializeCells();
        }

        private void InitializeCells()
        {
            for (var row = 0; row < Rows; row++)
            {
                for (var column = 0; column < Columns; column++)
                {
                    Cells[row, column] = new Cell
                    {
                        Row = row,
                        Column = column
                    };
                }
            }
        }
    }
}