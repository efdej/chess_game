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
        private readonly Dictionary<TableLayoutPanelCellPosition, Rectangle> _dict = new Dictionary<TableLayoutPanelCellPosition, Rectangle>();
        private readonly Color[,] _bgColors = new Color[8, 8];
        private Point _downPoint;
        private bool _moved;

        public Chess()
        {
            InitializeComponent();
            //This will prevent flicker
            typeof(TableLayoutPanel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(BoardPanel, true, null);
        }

        private void Chess_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.BoardPanel.Controls)
            {
                if (c is PictureBox)
                {
                    c.Click += Pieces_Click;
                    c.MouseDown += Pieces_MouseDown;
                    c.MouseMove += Pieces_MouseMove;
                    c.MouseUp += Pieces_MouseUp;
                }
            }
        }

        private void Pieces_Click(object sender, EventArgs e)
        {
            var piece = sender as Control;
            var position = BoardPanel.GetPositionFromControl(piece);
            _bgColors[position.Column, position.Row] = Color.Yellow;
            _bgColors[position.Column, position.Row + 1] = Color.Yellow;
            BoardPanel.Refresh();
            _bgColors[position.Column, position.Row] = Color.Empty;
            _bgColors[position.Column, position.Row + 1] = Color.Empty;
        }

        //MouseDown event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control piece)
            {
                piece.Parent = this;
                piece.BringToFront();
                _downPoint = e.Location;
            }
        }

        //MouseMove event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Control piece && e.Button == MouseButtons.Left)
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
            if (sender is PictureBox piece && _moved)
            {
                SwapImages(piece, e.Location);
                piece.Parent = BoardPanel;
                _moved = false;
            }
        }

        //This is used to set the control on the tableLayoutPanel after releasing mouse
        private void SwapImages(PictureBox c, Point position)
        {
            var localPoint = BoardPanel.PointToClient(c.PointToScreen(position));
            var keyValue = _dict.FirstOrDefault(e => e.Value.Contains(localPoint));
            if (!keyValue.Equals(default(KeyValuePair<TableLayoutPanelCellPosition, Rectangle>))
                && BoardPanel.GetControlFromPosition(keyValue.Key.Column, keyValue.Key.Row) is PictureBox target)
            {
                (c.BackgroundImage, target.BackgroundImage) = (target.BackgroundImage, c.BackgroundImage);
                //BoardPanel.SetCellPosition(c, keyValue.Key);
            }
        }

        private void BoardPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            _dict[new TableLayoutPanelCellPosition(e.Column, e.Row)] = e.CellBounds;
            
            if (_moved && e.CellBounds.Contains(BoardPanel.PointToClient(MousePosition)))
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.CellBounds);
            }
            
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
        }
    }
    
    public partial class Chess : Form
    {
        // This is used to store the CellBounds together with the Cell position
        // so that we can find the Cell position later (after releasing mouse).
        private readonly Dictionary<TableLayoutPanelCellPosition, Rectangle> _dict = new Dictionary<TableLayoutPanelCellPosition, Rectangle>();
        private readonly Color[,] _bgColors = new Color[8, 8];
        private Point _downPoint;
        private bool _moved;

        public Chess()
        {
            InitializeComponent();
            //This will prevent flicker
            typeof(TableLayoutPanel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(BoardPanel, true, null);
        }

        private void Chess_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.BoardPanel.Controls)
            {
                if (c is PictureBox)
                {
                    c.Click += Pieces_Click;
                    c.MouseDown += Pieces_MouseDown;
                    c.MouseMove += Pieces_MouseMove;
                    c.MouseUp += Pieces_MouseUp;
                }
            }
        }

        private void Pieces_Click(object sender, EventArgs e)
        {
            var piece = sender as Control;
            var position = BoardPanel.GetPositionFromControl(piece);
            _bgColors[position.Column, position.Row] = Color.Yellow;
            _bgColors[position.Column, position.Row + 1] = Color.Yellow;
            BoardPanel.Refresh();
            _bgColors[position.Column, position.Row] = Color.Empty;
            _bgColors[position.Column, position.Row + 1] = Color.Empty;
        }

        //MouseDown event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Control piece)
            {
                piece.Parent = this;
                piece.BringToFront();
                _downPoint = e.Location;
            }
        }

        //MouseMove event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Control piece && e.Button == MouseButtons.Left)
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
            if (sender is PictureBox piece && _moved)
            {
                SwapImages(piece, e.Location);
                piece.Parent = BoardPanel;
                _moved = false;
            }
        }

        //This is used to set the control on the tableLayoutPanel after releasing mouse
        private void SwapImages(PictureBox c, Point position)
        {
            var localPoint = BoardPanel.PointToClient(c.PointToScreen(position));
            var keyValue = _dict.FirstOrDefault(e => e.Value.Contains(localPoint));
            if (!keyValue.Equals(default(KeyValuePair<TableLayoutPanelCellPosition, Rectangle>))
                && BoardPanel.GetControlFromPosition(keyValue.Key.Column, keyValue.Key.Row) is PictureBox target)
            {
                (c.BackgroundImage, target.BackgroundImage) = (target.BackgroundImage, c.BackgroundImage);
                //BoardPanel.SetCellPosition(c, keyValue.Key);
            }
        }

        private void BoardPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            _dict[new TableLayoutPanelCellPosition(e.Column, e.Row)] = e.CellBounds;
            
            if (_moved && e.CellBounds.Contains(BoardPanel.PointToClient(MousePosition)))
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.CellBounds);
            }
            
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
        }
    }
}
