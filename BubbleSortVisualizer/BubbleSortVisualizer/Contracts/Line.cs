namespace WindowsFormsApp1.Contracts
{
    public class Line
    {
        public Line(float width, float height, Metrics metrics)
        {
            this.X1 = width / 30 + (width / 15) * metrics.FromCol + metrics.FromCol * 10;
            this.Y1 = height / 2 + ((width / 8) * metrics.FromRow) / 6;
            this.X2 = width / 30 + (width / 15) * metrics.ToCol + metrics.ToCol * 10;
            this.Y2 = height / 2 + ((width / 8) * metrics.ToRow) / 6;
        }

        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
    }
}
