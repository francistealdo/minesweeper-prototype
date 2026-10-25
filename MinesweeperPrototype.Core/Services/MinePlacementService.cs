using MinesweeperPrototype.Core.Entities;

namespace MinesweeperPrototype.Core.Services;

public sealed class MinePlacementService
{
    private readonly Random _random = new();

    public void PlaceMines(Board board)
    {
        var minesPlaced = 0;

        while (minesPlaced < board.MineCount)
        {
            var row = _random.Next(board.Rows);
            var column = _random.Next(board.Columns);

            var cell = board.Cells[row, column];

            if (cell.HasMine)
            {
                continue;
            }

            cell.HasMine = true;
            minesPlaced++;
        }
    }
}