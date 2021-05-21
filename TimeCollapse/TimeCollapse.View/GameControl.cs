using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;
using TimeCollapse.View.Assets;
using static TimeCollapse.Models.FieldOfViewCalculator;

namespace TimeCollapse.View
{
    public sealed class GameControl : UserControl
    {
        private readonly Dictionary<Explorer, (Animation, IEnumerator<Bitmap>)> animationsEnumerators = new();
        private readonly Game game;
        private readonly MainForm mainForm;
        public readonly Timer UpdateTimer;
        private Bitmap background;
        private List<Bitmap> explorerJumpLeft;
        private List<Bitmap> explorerJumpRight;
        private List<Bitmap> explorerLeft;
        private List<Bitmap> explorerRight;
        private List<Bitmap> explorerWalkLeft;
        private List<Bitmap> explorerWalkRight;
        private Bitmap portal;
        private int timerTick;

        public GameControl(MainForm form)
        {
            ImportAssets();

            mainForm = form;
            BackgroundImage = background;
            ClientSize = new Size(1024, 768);

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();

            game = Game.TestGame;

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
            g.DrawImage(portal, game.ActualMap.ActualStage.Target);
            foreach (var explorer in game.AllExplorers)
            {
                g.DrawPolygon(new Pen(Color.Goldenrod, 3), GetFieldOfView(game, explorer));
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

        private void ImportAssets()
        {
            var explorerRightImage = new Bitmap(Image.FromFile(@"Assets\Explo.png"));
            explorerRight = new List<Bitmap> {explorerRightImage};

            var explorerLeftImage = new Bitmap(Image.FromFile(@"Assets\Explo.png"));
            explorerLeftImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            explorerLeft = new List<Bitmap> {explorerLeftImage};

            var explorerWalkRightImage = new Bitmap(Image.FromFile(@"Assets\ExploWalk.png"));
            explorerWalkRight = SplitImage(explorerWalkRightImage, new Size(16 * 3, 32 * 3)).ToList();

            explorerWalkRightImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            explorerWalkLeft = SplitImage(explorerWalkRightImage, new Size(16 * 3, 32 * 3)).ToList();

            var explorerJumpImage = new Bitmap(Image.FromFile(@"Assets\ExploJump.png"));
            explorerJumpRight = SplitImage(explorerJumpImage, new Size(16 * 3, 32 * 3)).ToList();

            explorerJumpImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            explorerJumpLeft = SplitImage(explorerJumpImage, new Size(16 * 3, 32 * 3)).ToList();

            portal = new Bitmap(Image.FromFile(@"Assets\Portal.png"));

            background = new Bitmap(Image.FromFile(@"Assets\Background.png"));
        }

        private static IEnumerable<Bitmap> SplitImage(Bitmap image, Size part)
        {
            for (var i = 0; i < image.Size.Height / part.Height; i++)
            for (var j = 0; j < image.Size.Width / part.Width; j++)
                yield return image.Clone(new Rectangle(new Point(part.Width * j, part.Height * i), part),
                    image.PixelFormat);
        }
    }
}