using MinesweeperPrototype.Application.Services;
using MinesweeperPrototype.Wpf.ViewModels;
using System.Windows;

namespace MinesweeperPrototype.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel(
            new GameService());
    }
}