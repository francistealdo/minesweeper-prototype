using MinesweeperPrototype.Core.Entities;
using MinesweeperPrototype.Core.Services;

namespace MinesweeperPrototype.Application.Services;

public sealed class GameService
{
    private readonly MinePlacementService _minePlacementService = new();
    private readonly AdjacentMineCalculator _adjacentMineCalculator = new();

    private Board? _board;

    public void StartNewGame(int rows, int columns, int mines)
    {
        _board = new Board(rows, columns, mines);

        _minePlacementService.PlaceMines(_board);
        _adjacentMineCalculator.Calculate(_board);
    }

    public void Reveal(int row, int column)
    {
        if (_board is null)
        {
            throw new InvalidOperationException("No game has been started.");
        }

        if (row < 0 || row >= _board.Rows ||
            column < 0 || column >= _board.Columns)
        {
            return;
        }

        RevealCell(_board, row, column);
    }

    public void ToggleFlag(int row, int column)
    {
        if (_board is null)
        {
            throw new InvalidOperationException("No game has been started.");
        }

        var cell = _board.Cells[row, column];

        if (cell.IsRevealed)
        {
            return;
        }

        cell.IsFlagged = !cell.IsFlagged;
    }

    private static void RevealCell(Board board, int row, int column)
    {
        if (row < 0 || row >= board.Rows ||
            column < 0 || column >= board.Columns)
        {
            return;
        }

        var cell = board.Cells[row, column];

        if (cell.IsRevealed || cell.IsFlagged)
        {
            return;
        }

        cell.IsRevealed = true;

        if (cell.HasMine)
        {
            return;
        }

        if (cell.AdjacentMines > 0)
        {
            return;
        }

        for (var rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (var columnOffset = -1; columnOffset <= 1; columnOffset++)
            {
                if (rowOffset == 0 && columnOffset == 0)
                {
                    continue;
                }

                RevealCell(
                    board,
                    row + rowOffset,
                    column + columnOffset);
            }
        }
    }
}