namespace WindowsFormsApp1.Contracts
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;

    public class Drawer : IDrawer
    {
        public Drawer()
        {
            
        }

        public void DrawArrow(Graphics graphics,
            Rectangle rectangle,
            Metrics metrics,
            bool arrowStart)
        {
            float width = rectangle.Width;
            float height = rectangle.Height;
            Line line = new Line(width, height, metrics);
            Pen pen = new Pen(Color.Red, 7);


            if (arrowStart)
            {
                pen.StartCap = LineCap.ArrowAnchor;
            }
            else
            {
                pen.EndCap = LineCap.ArrowAnchor;
            }


            graphics.DrawLine(
                pen,
                line.X1,
                line.Y1,
                line.X2,
                line.Y2);
        }

        public void DrawArrayBoxes(int[] arr,
            Graphics graphics,
            Rectangle clientRectangle,
            int cell,
            int line,
            int[] grayRange,
            int[] pinkRange)
        {
            float width = clientRectangle.Width;
            float height = clientRectangle.Height;

            for (int index = 0; index < arr.Length; ++index)
            {
                RectangleF rectangleF = new RectangleF(
                    width / 30 + (width / 15) * (index + cell) + 6 * (index + cell),
                    height / 4 + 30 * line,
                    width / 15,
                    height / 7);

                graphics.FillRectangle(Brushes.White, rectangleF);

                if (grayRange.Contains(index))
                {
                    graphics.FillRectangle(Brushes.LimeGreen, rectangleF);
                }


                if (pinkRange.Contains(index))
                {
                    graphics.FillRectangle(Brushes.Pink, rectangleF);
                }

 

                graphics.DrawString(
                    arr[index].ToString(),
                    new Font("Time New Rome", 16),
                    Brushes.Black,
                    new RectangleF(
                        width / 30 + width / 15 * (index + cell) + 6 * (index + cell),
                        height / 4 + 30 * line + 25,
                        width / 15,
                        height / 7 + 25),
                    StringFormat.GenericDefault);


                graphics.DrawRectangle(
                    new Pen(Brushes.Black),
                    rectangleF.X,
                    rectangleF.Y,
                    rectangleF.Width,
                    rectangleF.Height);
            }
        }
    }
}
