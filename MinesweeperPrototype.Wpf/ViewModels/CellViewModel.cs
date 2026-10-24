using CommunityToolkit.Mvvm.ComponentModel;

namespace MinesweeperPrototype.Wpf.ViewModels
{
    public partial class CellViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isRevealed;

        [ObservableProperty]
        private bool isFlagged;

        [ObservableProperty]
        private int adjacentMines;

        public int Row { get; init; }

        public int Column { get; init; }
    }
}
