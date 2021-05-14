using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;
using static TimeCollapse.Models.FieldOfViewCalculator;

namespace TimeCollapse.View
{
    public partial class TestForm : Form
    {
        private readonly Image astronautLeft;
        private readonly Image astronautRight;
        private readonly Game game;
        private int timerTick;

        public TestForm()
        {
            astronautRight =
                Image.FromFile(
                    @"C:\Users\serez\OneDrive\Рабочий стол\Учебные материалы\ПРОГА\Ulearn\bestgame\TimeCollapse\Assets/AstroStay Right.png");
            astronautLeft =
                Image.FromFile(
                    @"C:\Users\serez\OneDrive\Рабочий стол\Учебные материалы\ПРОГА\Ulearn\bestgame\TimeCollapse\Assets/AstroStay Right.png");
            astronautLeft.RotateFlip(RotateFlipType.RotateNoneFlipX);

            game = Game.TestGame;
            var updateTimer = new Timer {Interval = 10};
            updateTimer.Tick += UpdateTimerTick;
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
            updateTimer.Start();
        }

        private void TestForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangles(new SolidBrush(Color.Black), game.ActualMap.Blocks.ToArray());
            g.FillRectangle(new SolidBrush(Color.Green), game.ActualMap.ActualStage.Target);
            foreach (var explorer in game.AllExplorers)
            {
                g.DrawPolygon(new Pen(Color.Goldenrod, 3), GetFieldOfView(game, explorer));
                g.DrawImage(explorer.TurnedRight ? astronautRight : astronautLeft, explorer.Collider);
            }
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            game.Update(timerTick++);
            Refresh();
        }

        private void TestForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                game.PresentExplorer.Jump = true;
            if (e.KeyCode == Keys.A)
                game.PresentExplorer.LeftRun = true;
            if (e.KeyCode == Keys.D)
                game.PresentExplorer.RightRun = true;
        }

        private void TestForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                game.PresentExplorer.Jump = false;
            if (e.KeyCode == Keys.A)
                game.PresentExplorer.LeftRun = false;
            if (e.KeyCode == Keys.D)
                game.PresentExplorer.RightRun = false;
        }
    }
}