using System.Drawing;

namespace WindowsFormsApp1.Contracts
{
    public interface IDrawer
    {
        public void DrawArrow(Graphics graphics,
            Rectangle rectangle,
            Metrics metrics,
            bool arrowStart);

        public void DrawArrayBoxes(int[] arr,
            Graphics graphics,
            Rectangle clientRectangle,
            int cell,
            int line,
            int[] grayRange,
            int[] pinkRange);
    }
}
