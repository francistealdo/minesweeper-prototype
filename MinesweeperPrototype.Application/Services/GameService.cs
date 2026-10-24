using MinesweeperPrototype.Core.Entities;

namespace MinesweeperPrototype.Application.Services
{
    public sealed class GameService
    {
        private Board? _board;

        public void StartNewGame(
            int rows,
            int columns,
            int mines)
        {
            _board = new Board(rows, columns, mines);

            // inicializa células
            // posiciona minas
            // calcula números
        }

        public void Reveal(int row, int column)
        {
        }

        public void ToggleFlag(int row, int column)
        {
        }
    }
}
