using MinesweeperPrototype.Core.Entities;

namespace MinesweeperPrototype.Core.Services;

public sealed class AdjacentMineCalculator
{
    public void Calculate(Board board)
    {
        for (var row = 0; row < board.Rows; row++)
        {
            for (var column = 0; column < board.Columns; column++)
            {
                var cell = board.Cells[row, column];

                if (cell.HasMine)
                {
                    continue;
                }

                cell.AdjacentMines = CountAdjacentMines(board, row, column);
            }
        }
    }

    private static int CountAdjacentMines(Board board, int row, int column)
    {
        var count = 0;

        for (var rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (var columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                if (rowOffset == 0 && columnOffset == 0)
                {
                    continue;
                }

                var neighborRow = row + rowOffset;
                var neighborColumn = column + columnOffset;

                if (neighborRow < 0 ||
                    neighborRow >= board.Rows ||
                    neighborColumn < 0 ||
                    neighborColumn >= board.Columns)
                {
                    continue;
                }

                if (board.Cells[neighborRow, neighborColumn].HasMine)
                {
                    count++;
                }
            }
        }

        return count;
    }
}