namespace WindowsFormsApp1
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Contracts;
    using Helpers;

    public partial class MainForm : Form
    {
        private int[] _Array;
        private int i;
        private int j;
        private int _SwapingVariable;
        private bool _CanSwap;
        private readonly IDrawer _Drawer = new Drawer();

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            bool upArrow = false;
            bool downArrow = false;

            InitializeArrayIfNullOrEmpty();
            Invalidate();
            Application.DoEvents();
            Graphics graphics = CreateGraphics();

            VisualizeSorting(ref upArrow, ref downArrow, graphics);

        }

        private void InitializeArrayIfNullOrEmpty()
        {
            if (!_Array.IsNullOrEmpty()) return;

            i = 0;
            j = 0;
            _CanSwap = false;
            _Array = textBoxNums.Text.ParseToIntArray();

            textBoxNums.Visible = false;
        }

        private void VisualizeSorting(ref bool upArrow, ref bool downArrow, Graphics graphics)
        {
            if (i < _Array.Length)
            {

                if (j >= 0 && _CanSwap && _Array[j] > _Array[j + 1])
                {
                    _SwapingVariable = _Array[j];
                    _Array[j] = _Array[j + 1];
                    _Array[j + 1] = _SwapingVariable;
                    _Drawer.DrawArrayBoxes(_Array, graphics, ClientRectangle, 0, 3, Enumerable.Range(0, i + 1).ToArray(), new int[1] { j - 1 });
                    _Drawer.DrawArrow(graphics, ClientRectangle, new Metrics(j, j + 1, 0, 0), false);

                    j--;
                }
                else if (_CanSwap)
                {
                    _Drawer.DrawArrayBoxes(_Array, graphics, ClientRectangle, 0, 3, Enumerable.Range(0, i + 1).ToArray(), new int[1] { j + 1 });

                    _CanSwap = false;
                    downArrow = true;
                }
                else
                {
                    i++;
                    _Drawer.DrawArrayBoxes(_Array, graphics, ClientRectangle, 0, 3, Enumerable.Range(0, i + 1).ToArray(), Array.Empty<int>());

                    if (i == _Array.Length)
                    {
                        return;
                    }

                    _SwapingVariable = _Array[i];
                    _CanSwap = true;
                    upArrow = true;
                    j = i - 1;
                }


                _Drawer.DrawArrayBoxes(new int[1] { _Array[j + 1] }, graphics, ClientRectangle, j + 1, 1, Array.Empty<int>(), upArrow ? new int[1] : Array.Empty<int>());

                if (!upArrow && !downArrow)
                {
                    return;
                }

                _Drawer.DrawArrow(graphics, ClientRectangle, new Metrics(j + 1, j + 1, upArrow ? 0 : 1, downArrow ? 0 : 1), true);
            }
            else
            {
                _Array = null;
                textBoxNums.Visible = true;
            }
        }
    }
}
