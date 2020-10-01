using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Chess
{
    public partial class Chess : Form
    {
        // This is used to store the CellBounds together with the Cell position
        // so that we can find the Cell position later (after releasing mouse).
        readonly private Dictionary<TableLayoutPanelCellPosition, Rectangle> dict = new Dictionary<TableLayoutPanelCellPosition, Rectangle>();
        readonly private Color[,] _bgColors = new Color[8, 8];
        // private bool _gameLoaded = false;
        private Point _downPoint;
        private bool _moved;

        public Chess()
        {
            InitializeComponent();
            //This will prevent flicker
            typeof(TableLayoutPanel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(BoardPanel, true, null);
        }

        private void Chess_Load(object sender, EventArgs e)
        {

        }

        private void Pieces_Click(object sender, EventArgs e)
        {
            _bgColors[0, 2] = Color.Yellow;
            _bgColors[0, 3] = Color.Yellow;
            BoardPanel.Refresh();
        }

        //MouseDown event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseDown(object sender, MouseEventArgs e)
        {
            Control piece = sender as Control;
            piece.Parent = this;
            piece.BringToFront();
            _downPoint = e.Location;
        }

        //MouseMove event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseMove(object sender, MouseEventArgs e)
        {
            Control piece = sender as Control;
            if (e.Button == MouseButtons.Left)
            {
                piece.Left += e.X - _downPoint.X;
                piece.Top += e.Y - _downPoint.Y;
                _moved = true;
                BoardPanel.Invalidate();
            }
        }
        //MouseUp event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseUp(object sender, MouseEventArgs e)
        {
            Control piece = sender as Control;
            if (_moved)
            {
                SetControl(piece, e.Location);
                piece.Parent = BoardPanel;
                _moved = false;
            }
        }
        //This is used to set the control on the tableLayoutPanel after releasing mouse
        private void SetControl(Control c, Point position)
        {
            Point localPoint = BoardPanel.PointToClient(c.PointToScreen(position));
            var keyValue = dict.FirstOrDefault(e => e.Value.Contains(localPoint));
            if (!keyValue.Equals(default(KeyValuePair<TableLayoutPanelCellPosition, Rectangle>)))
            {
                BoardPanel.SetCellPosition(c, keyValue.Key);
            }
        }

        private void BoardPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            dict[new TableLayoutPanelCellPosition(e.Column, e.Row)] = e.CellBounds;
            /*
            
            if (moved)
            {
                if (e.CellBounds.Contains(BoardPanel.PointToClient(MousePosition)))
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, e.CellBounds);
                }
            }
            */
            using (var b = new SolidBrush(_bgColors[e.Column, e.Row]))
            {

                e.Graphics.FillRectangle(b, e.CellBounds);
            }
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuItemNewGame_Click(object sender, EventArgs e)
        {
            BoardPanel.Show();
            // _gameLoaded = true;
        }
    }
}