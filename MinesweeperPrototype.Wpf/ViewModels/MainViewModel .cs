using CommunityToolkit.Mvvm.ComponentModel;
using MinesweeperPrototype.Application.Services;
using System.Collections.ObjectModel;

namespace MinesweeperPrototype.Wpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly GameService _gameService;

        public ObservableCollection<CellViewModel> Cells { get; }
            = [];

        public MainViewModel(GameService gameService)
        {
            _gameService = gameService;
        }
    }
}
