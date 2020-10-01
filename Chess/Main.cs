using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Chess : Form
    {
        private bool GameLoaded = false;
        private Timer timer1 = new Timer();

        private Point downPoint;
        private bool moved;

        public Chess()
        {
            InitializeComponent();
        }

        private void Chess_Load(object sender, EventArgs e)
        {

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox)
                {
                    //c.Click += PiecesClick;
                    c.MouseDown += Pieces_MouseDown;
                    c.MouseMove += Pieces_MouseMove;
                    c.MouseUp += Pieces_MouseUp;

                }

            }

        }

        //This is used to store the CellBounds together with the Cell position
        //so that we can find the Cell position later (after releasing mouse).
        Dictionary<TableLayoutPanelCellPosition, Rectangle> dict = new Dictionary<TableLayoutPanelCellPosition, Rectangle>();
        //MouseDown event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseDown(object sender, MouseEventArgs e)
        {
            Control button = sender as Control;
            button.Parent = this;
            button.BringToFront();
            downPoint = e.Location;
        }
        //MouseMove event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseMove(object sender, MouseEventArgs e)
        {
            Control button = sender as Control;
            if (e.Button == MouseButtons.Left)
            {
                button.Left += e.X - downPoint.X;
                button.Top += e.Y - downPoint.Y;
                moved = true;
                BoardPanel.Invalidate();
            }
        }
        //MouseUp event handler for all your controls (on the tableLayoutPanel1)
        private void Pieces_MouseUp(object sender, MouseEventArgs e)
        {
            Control button = sender as Control;
            if (moved)
            {
                SetControl(button, e.Location);
                button.Parent = BoardPanel;
                moved = false;
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
            if (moved)
            {
                if (e.CellBounds.Contains(BoardPanel.PointToClient(MousePosition)))
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, e.CellBounds);
                }
            }
        }

        private void PiecesClick(object sender, EventArgs e)
        {

        }

        private void ShowBoard(bool Show)
        {
            BoardPanel.Visible = Show;
        }

        private void menuItemNewGame_Click(object sender, EventArgs e)
        {
            ShowBoard(true);
            GameLoaded = true;
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}