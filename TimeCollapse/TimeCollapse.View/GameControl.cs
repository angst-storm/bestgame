using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class GameControl : UserControl
    {
        private readonly Dictionary<Explorer, (Animation, IEnumerator<Bitmap>)> animationsEnumerators = new();
        private readonly MainForm mainForm;
        private readonly Control pause;
        private Bitmap background;
        private List<Bitmap> explorerJumpLeft;
        private List<Bitmap> explorerJumpRight;
        private List<Bitmap> explorerLeft;
        private List<Bitmap> explorerRight;
        private List<Bitmap> explorerWalkLeft;
        private List<Bitmap> explorerWalkRight;
        private Game game;
        private Bitmap portal;
        private int timerTick;
        private Timer updateTimer;

        public GameControl(MainForm form)
        {
            mainForm = form;
            ImportAssets();

            ClientSize = mainForm.Size;
            BackgroundImage = background;

            pause = InitializePause();
            Controls.Add(pause);
            pause.Hide();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        public void StartGame()
        {
            game = Game.TestGame;
            updateTimer = new Timer {Interval = 10};
            updateTimer.Tick += UpdateTimerTick;
            updateTimer.Start();
        }

        private void PauseGame()
        {
            updateTimer.Stop();
            pause.Enabled = true;
            pause.Show();
            pause.Focus();
        }

        private void ResumeGame()
        {
            pause.Enabled = false;
            pause.Hide();
            Focus();
            updateTimer.Start();
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
            g.DrawImage(portal, game.ActualMap.ActualStage.Target);
            foreach (var explorer in game.AllExplorers)
            {
                g.DrawPolygon(new Pen(Color.Goldenrod, 3), explorer.GetFieldOfView());
                g.DrawImage(GetCurrentSprite(explorer), explorer.Collider);
            }
        }

        private Bitmap GetCurrentSprite(Explorer explorer)
        {
            var currentAnimation = (Animation) (Convert.ToInt32(explorer.OnFloor) * 2 +
                                                Convert.ToInt32(explorer.OnFloor && explorer.Go) * 4 +
                                                Convert.ToInt32(explorer.TurnedRight) * 8);
            if (!animationsEnumerators.ContainsKey(explorer) ||
                !animationsEnumerators[explorer].Item2.MoveNext() ||
                currentAnimation != animationsEnumerators[explorer].Item1)
            {
                animationsEnumerators[explorer] = (currentAnimation, (currentAnimation switch
                {
                    Animation.StayRight => explorerRight,
                    Animation.StayLeft => explorerLeft,
                    Animation.WalkRight => explorerWalkRight,
                    Animation.WalkLeft => explorerWalkLeft,
                    Animation.JumpRight => explorerJumpRight,
                    Animation.JumpLeft => explorerJumpLeft,
                    _ => throw new InvalidOperationException()
                }).GetEnumerator());
                animationsEnumerators[explorer].Item2.MoveNext();
            }

            return animationsEnumerators[explorer].Item2.Current;
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
                PauseGame();
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

        private void ImportAssets()
        {
            var explorerRightImage = new Bitmap(Image.FromFile(@"Assets\Explo.png"));
            explorerRight = new List<Bitmap> {explorerRightImage};

            var explorerLeftImage = new Bitmap(Image.FromFile(@"Assets\Explo.png"));
            explorerLeftImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            explorerLeft = new List<Bitmap> {explorerLeftImage};

            var explorerWalkRightImage = new Bitmap(Image.FromFile(@"Assets\ExploWalk.png"));
            explorerWalkRight = SplitImage(explorerWalkRightImage, new Size(16 * 3, 32 * 3), 5).ToList();

            explorerWalkRightImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            explorerWalkLeft = SplitImage(explorerWalkRightImage, new Size(16 * 3, 32 * 3), 5).ToList();

            var explorerJumpImage = new Bitmap(Image.FromFile(@"Assets\ExploJump.png"));
            explorerJumpRight = SplitImage(explorerJumpImage, new Size(16 * 3, 32 * 3), 1).ToList();

            explorerJumpImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            explorerJumpLeft = SplitImage(explorerJumpImage, new Size(16 * 3, 32 * 3), 1).ToList();

            portal = new Bitmap(Image.FromFile(@"Assets\Portal.png"));

            background = new Bitmap(Image.FromFile(@"Assets\Background.png"));
        }

        private static IEnumerable<Bitmap> SplitImage(Bitmap image, Size part, int delay)
        {
            for (var i = 0; i < image.Size.Height / part.Height; i++)
            for (var j = 0; j < image.Size.Width / part.Width; j++)
            for (var k = 0; k < delay; k++)
                yield return image.Clone(new Rectangle(new Point(part.Width * j, part.Height * i), part),
                    image.PixelFormat);
        }

        private TableLayoutPanel InitializePause()
        {
            var tableLocation = new Point(ClientSize.Width / 192, ClientSize.Height / 108);
            var tableSize = new Size(ClientSize.Width / 5, ClientSize.Height / 2);
            var pauseTable = new TableLayoutPanel
            {
                Location = tableLocation,
                Size = tableSize,
                Enabled = false,
                BackColor = Color.FromArgb(18, 62, 64)
            };
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            pauseTable.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            pauseTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pauseTable.Controls.Add(MenuControl.MyDefaultButton(@"Resume", pauseTable.Size.Height / 20, ResumeGame), 0,
                0);
            pauseTable.Controls.Add(
                MenuControl.MyDefaultButton(@"Main Menu", pauseTable.Size.Height / 20, mainForm.ToMainMenu), 0, 1);
            pauseTable.Controls.Add(MenuControl.MyDefaultButton(@"Settings", pauseTable.Size.Height / 20, () => { }), 0,
                2);
            pauseTable.Controls.Add(MenuControl.MyDefaultButton(@"Exit", pauseTable.Size.Height / 20, Application.Exit),
                0, 3);
            pauseTable.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Escape) ResumeGame();
            };
            return pauseTable;
        }
    }
}