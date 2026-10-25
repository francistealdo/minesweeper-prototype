using MinesweeperPrototype.Core.Entities;
using MinesweeperPrototype.Core.Enums;
using MinesweeperPrototype.Core.Services;

namespace MinesweeperPrototype.Application.Services;

public sealed class GameService
{
    private readonly MinePlacementService _minePlacementService = new();
    private readonly AdjacentMineCalculator _adjacentMineCalculator = new();
    private Board? _board;
    public Board? Board => _board;
    public GameStatus Status { get; private set; }

    public void StartNewGame(int rows, int columns, int mines)
    {
        _board = new Board(rows, columns, mines);

        _minePlacementService.PlaceMines(_board);
        _adjacentMineCalculator.Calculate(_board);

        Status = GameStatus.InProgress;
    }

    public void Reveal(int row, int column)
    {
        if (Status is GameStatus.Won or GameStatus.Lost)
        {
            return;
        }

        if (_board is null)
        {
            throw new InvalidOperationException("No game has been started.");
        }

        if (row < 0 || row >= _board.Rows || column < 0 || column >= _board.Columns)
        {
            return;
        }

        RevealCell(_board, row, column);

        if (Status == GameStatus.Lost)
        {
            return;
        }

        if (HasWon(_board))
        {
            Status = GameStatus.Won;
        }
    }

    public void ToggleFlag(int row, int column)
    {
        if (_board is null)
        {
            throw new InvalidOperationException("No game has been started.");
        }

        if (Status is GameStatus.Won or GameStatus.Lost)
        {
            return;
        }

        if (row < 0 || row >= _board.Rows ||
            column < 0 || column >= _board.Columns)
        {
            return;
        }

        var cell = _board.Cells[row, column];

        if (cell.IsRevealed)
        {
            return;
        }

        cell.IsFlagged = !cell.IsFlagged;

        if (HasWon(_board))
        {
            Status = GameStatus.Won;
        }
    }

    public void Timeout()
    {
        if (_board is null)
        {
            return;
        }

        if (Status is GameStatus.Won or GameStatus.Lost)
        {
            return;
        }

        Status = GameStatus.Lost;

        RevealAllMines(_board);
    }

    #region Private Methods

    private void RevealCell(Board board, int row, int column)
    {
        if (row < 0 || row >= board.Rows || column < 0 || column >= board.Columns)
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
            Status = GameStatus.Lost;
            RevealAllMines(board);
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

                RevealCell(board, row + rowOffset, column + columnOffset);
            }
        }
    }

    private static bool HasWon(Board board)
    {
        for (var row = 0; row < board.Rows; row++)
        {
            for (var column = 0; column < board.Columns; column++)
            {
                var cell = board.Cells[row, column];

                if (cell.HasMine && !cell.IsFlagged)
                {
                    return false;
                }

                if (!cell.HasMine && !cell.IsRevealed)
                {
                    return false;
                }

                if (!cell.HasMine && cell.IsFlagged)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static void RevealAllMines(Board board)
    {
        for (var row = 0; row < board.Rows; row++)
        {
            for (var column = 0; column < board.Columns; column++)
            {
                var cell = board.Cells[row, column];

                if (cell.HasMine)
                {
                    cell.IsRevealed = true;
                }
            }
        }
    }

    #endregion
}