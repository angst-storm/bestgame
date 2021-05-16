using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;
using static TimeCollapse.Models.FieldOfViewCalculator;

namespace TimeCollapse.View
{
    public sealed class GameControl : UserControl
    {
        private readonly Image astronautLeft;
        private readonly Image astronautRight;
        private readonly Game game;
        private readonly MainForm mainForm;
        public readonly Timer UpdateTimer;
        private int timerTick;

        public GameControl(MainForm form)
        {
            mainForm = form;

            astronautRight =
                new Bitmap(Image.FromFile(
                    @"C:\Users\serez\OneDrive\Рабочий стол\Учебные материалы\ПРОГА\Ulearn\bestgame\TimeCollapse\Assets/AstroStay Right.png"));

            astronautLeft =
                new Bitmap(Image.FromFile(
                    @"C:\Users\serez\OneDrive\Рабочий стол\Учебные материалы\ПРОГА\Ulearn\bestgame\TimeCollapse\Assets/AstroStay Right.png"));
            astronautLeft.RotateFlip(RotateFlipType.RotateNoneFlipX);

            BackgroundImage =
                new Bitmap(Image.FromFile(
                    @"C:\Users\serez\OneDrive\Рабочий стол\Учебные материалы\ПРОГА\Ulearn\bestgame\TimeCollapse\Assets/GameBackground.png"));

            game = Game.TestGame;
            ClientSize = new Size(1024, 768);

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            UpdateTimer = new Timer {Interval = 10};
            UpdateTimer.Tick += UpdateTimerTick;
            UpdateTimer.Start();
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            game.Update(timerTick++);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangles(new SolidBrush(Color.DarkSlateGray), game.ActualMap.Blocks.ToArray());
            g.FillRectangle(new SolidBrush(Color.Green), game.ActualMap.ActualStage.Target);
            foreach (var explorer in game.AllExplorers)
            {
                g.DrawPolygon(new Pen(Color.Goldenrod, 3), GetFieldOfView(game, explorer));
                g.DrawImage(explorer.TurnedRight ? astronautRight : astronautLeft, explorer.Collider);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                game.PresentExplorer.Jump = true;
            if (e.KeyCode == Keys.A)
                game.PresentExplorer.LeftRun = true;
            if (e.KeyCode == Keys.D)
                game.PresentExplorer.RightRun = true;
            if (e.KeyCode == Keys.Escape)
                mainForm.PauseGame();
        }

        protected override void OnKeyUp(KeyEventArgs e)
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