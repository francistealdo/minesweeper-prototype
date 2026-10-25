using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinesweeperPrototype.Application.Services;
using MinesweeperPrototype.Core.Enums;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace MinesweeperPrototype.Wpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string gameStatusText = string.Empty;

    [ObservableProperty]
    private int elapsedSeconds;

    [ObservableProperty]
    private int bombsLeft;

    [ObservableProperty]
    private string faceIcon = "🙂";

    [ObservableProperty]
    private int secondsRemaining = 120;

    private readonly GameService _gameService;
    private readonly DispatcherTimer _timer;

    public ObservableCollection<CellViewModel> Cells { get; } = [];

    public int Rows { get; private set; }
    public int Columns { get; private set; }

    public IRelayCommand NewGameCommand { get; }

    public string TimerDisplay => SecondsRemaining.ToString("000");
    public string BombsLeftDisplay => BombsLeft.ToString("000");

    public MainViewModel(GameService gameService)
    {
        _gameService = gameService;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Tick += Timer_Tick;

        NewGameCommand = new RelayCommand(StartGame);

        StartGame();
    }

    #region Commands

    private void StartGame()
    {
        _timer.Stop();

        SecondsRemaining = 120;

        Rows = 9;
        Columns = 9;

        _gameService.StartNewGame(rows: Rows, columns: Columns, mines: 10);

        Cells.Clear();

        LoadBoard();

        BombsLeft = _gameService.Board!.MineCount;

        FaceIcon = "🙂";

        _timer.Start();
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

                Cells.Add(new CellViewModel(RevealCell, ToggleFlag)
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

    private void RevealCell(int row, int column)
    {
        _gameService.Reveal(row, column);

        RefreshBoard();
    }

    private void ToggleFlag(int row, int column)
    {
        _gameService.ToggleFlag(row, column);

        RefreshBoard();
    }

    private void RefreshBoard()
    {
        var board = _gameService.Board!;

        foreach (var cellViewModel in Cells)
        {
            var cell = board.Cells[
                cellViewModel.Row,
                cellViewModel.Column];

            cellViewModel.IsRevealed = cell.IsRevealed;
            cellViewModel.IsFlagged = cell.IsFlagged;
            cellViewModel.AdjacentMines = cell.AdjacentMines;
            cellViewModel.HasMine = cell.HasMine;
        }

        GameStatusText = _gameService.Status switch
        {
            GameStatus.Won => "You won!",
            GameStatus.Lost => "Game over!",
            _ => string.Empty
        };

        BombsLeft = board.MineCount - Cells.Count(c => c.IsFlagged);

        FaceIcon = _gameService.Status switch
        {
            GameStatus.Lost => "😵",
            GameStatus.Won => "😎",
            _ => "🙂"
        };

        if (_gameService.Status is GameStatus.Won or GameStatus.Lost)
        {
            _timer.Stop();
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_gameService.Status is GameStatus.Won or GameStatus.Lost)
        {
            _timer.Stop();
            return;
        }

        SecondsRemaining--;

        if (SecondsRemaining <= 0)
        {
            SecondsRemaining = 0;

            _timer.Stop();

            _gameService.Timeout();

            RefreshBoard();
        }
    }

    #endregion

    #region PropertyChanged Callbacks

    partial void OnElapsedSecondsChanged(int value)
    {
        OnPropertyChanged(nameof(TimerDisplay));
    }

    partial void OnBombsLeftChanged(int value)
    {
        OnPropertyChanged(nameof(BombsLeftDisplay));
    }

    partial void OnSecondsRemainingChanged(int value)
    {
        OnPropertyChanged(nameof(TimerDisplay));
    }

    #endregion
}