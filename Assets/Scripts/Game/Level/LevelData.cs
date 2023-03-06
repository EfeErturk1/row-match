namespace Game.Level
{
    public class LevelData
    {
        public int levelNo;
        public int gridWidth;
        public int gridHeight;
        public int moveCount;
        public char[] grid;
        
        public LevelData()
        {
            this.levelNo = 0;
            this.gridWidth = 0;
            this.gridHeight = 0;
            this.moveCount = 0;
            this.grid = new char[gridHeight * gridWidth];
        }
        
        
        public LevelData(int levelNo, int gridWidth, int gridHeight, int moveCount, char[] grid)
        {
            this.levelNo = levelNo;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
            this.moveCount = moveCount;
            this.grid = grid;
        }
    }
}