namespace MinesweeperPrototype.Core.Entities
{
    public sealed class Cell
    {
        public int Row { get; init; }
        public int Column { get; init; }
        public bool HasMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentMines { get; set; }
    }
}
