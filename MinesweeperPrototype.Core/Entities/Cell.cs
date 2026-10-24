namespace MinesweeperPrototype.Core.Entities
{
    public sealed class Cell
    {
        public bool HasMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentMines { get; set; }
    }
}
