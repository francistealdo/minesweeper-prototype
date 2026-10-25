using CommunityToolkit.Mvvm.ComponentModel;
using MinesweeperPrototype.Application.Services;
using System.Collections.ObjectModel;

namespace MinesweeperPrototype.Wpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly GameService _gameService;

    public ObservableCollection<CellViewModel> Cells { get; } = [];

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public MainViewModel(GameService gameService)
    {
        _gameService = gameService;

        StartGame();
    }

    private void StartGame()
    {
        Rows = 9;
        Columns = 9;

        _gameService.StartNewGame(
            rows: Rows,
            columns: Columns,
            mines: 10);

        LoadBoard();
    }

    private void LoadBoard()
    {
        Cells.Clear();

        var board = _gameService.Board!;

        for (var row = 0; row < board.Rows; row++)
        {
            for (var column = 0; column < board.Columns; column++)
            {
                var cell = board.Cells[row, column];

                Cells.Add(new CellViewModel
                {
                    Row = row,
                    Column = column,
                    IsRevealed = cell.IsRevealed,
                    IsFlagged = cell.IsFlagged,
                    AdjacentMines = cell.AdjacentMines
                });
            }
        }
    }
}