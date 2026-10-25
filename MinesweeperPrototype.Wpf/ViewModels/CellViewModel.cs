using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MinesweeperPrototype.Wpf.ViewModels;

public partial class CellViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isRevealed;

    [ObservableProperty]
    private bool isFlagged;

    [ObservableProperty]
    private int adjacentMines;

    [ObservableProperty]
    private bool hasMine;

    public int Row { get; init; }
    public int Column { get; init; }

    public IRelayCommand RevealCommand { get; }
    public IRelayCommand ToggleFlagCommand { get; }

    public bool IsClosed => !IsRevealed;

    public CellViewModel(Action<int, int> reveal, Action<int, int> toggleFlag)
    {
        RevealCommand = new RelayCommand(() => reveal(Row, Column));
        ToggleFlagCommand = new RelayCommand(() => toggleFlag(Row, Column));
    }

    public string DisplayContent
    {
        get
        {
            if (IsFlagged && !IsRevealed)
            {
                return "🚩";
            }

            if (!IsRevealed)
            {
                return string.Empty;
            }

            if (HasMine)
            {
                return "💣";
            }

            if (AdjacentMines == 0)
            {
                return string.Empty;
            }

            return AdjacentMines.ToString();
        }
    }

    #region PropertyChanged Callbacks

    partial void OnIsRevealedChanged(bool value)
    {
        OnPropertyChanged(nameof(DisplayContent));
        OnPropertyChanged(nameof(IsClosed));
    }

    partial void OnIsFlaggedChanged(bool value)
    {
        OnPropertyChanged(nameof(DisplayContent));
    }

    partial void OnAdjacentMinesChanged(int value)
    {
        OnPropertyChanged(nameof(DisplayContent));
    }

    partial void OnHasMineChanged(bool value)
    {
        OnPropertyChanged(nameof(DisplayContent));
    }

    #endregion
}