namespace WindowsFormsApp1.Contracts
{
    public class Metrics
    {
        public Metrics(int fromCol, int toCol, int fromRow, int toRow)
        {
            this.FromCol = fromCol;
            this.ToCol = toCol;
            this.FromRow = fromRow;
            this.ToRow = toRow;
        }

        public int FromCol { get; set; }
        public int ToCol { get; set; }
        public int FromRow { get; set; }
        public int ToRow { get; set; }
    }
}
