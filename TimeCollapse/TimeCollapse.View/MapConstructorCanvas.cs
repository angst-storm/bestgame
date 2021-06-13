using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class MapConstructorCanvas : UserControl
    {
        private readonly MapConstructor mapConstructor;
        private readonly float sx;
        private readonly float sy;
        private bool draw;
        private Rectangle drawableRectangle;
        private Point drawStartPoint;

        public MapConstructorCanvas(MapConstructor mapConstructor)
        {
            this.mapConstructor = mapConstructor;

            var res = Screen.PrimaryScreen.Bounds.Size;
            sx = res.Width / 1920f;
            sy = res.Height / 1080f;

            BackgroundImage = BackGroundDots();

            mapConstructor.Stages.SelectedIndexChanged += (sender, args) => Invalidate();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        public HashSet<Rectangle> Blocks { get; set; } = new();
        public HashSet<Rectangle> TimeAnomalies { get; set; } = new();

        private ConstructorDetail ActiveDetail =>
            mapConstructor.Details.SelectedItem.ToString() switch
            {
                "Блок" => ConstructorDetail.Block,
                "Временные аномалии" => ConstructorDetail.TimeAnomaly,
                "Стартовый прямоугольник" => ConstructorDetail.StartRectangle,
                "Целевой прямоугольник" => ConstructorDetail.TargetRectangle,
                _ => throw new InvalidOperationException()
            };

        private ConstructorStage ActiveStage => mapConstructor.StagesList[mapConstructor.Stages.SelectedIndex];

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var eLocation = new Point((int) (e.Location.X / sx), (int) (e.Location.Y / sy));
            if (e.Button == MouseButtons.Right)
            {
                if (drawableRectangle.Contains(eLocation)) RemoveDrawableRectangle();
            }
            else
            {
                if (!TryTakeExistingRectangle(eLocation, out drawableRectangle))
                {
                    draw = true;
                    drawStartPoint = new Point(RoundedTo(16, eLocation.X), RoundedTo(16, eLocation.Y));
                }
            }

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var eLocation = new Point((int) (e.Location.X / sx), (int) (e.Location.Y / sy));
            if (!draw) return;

            drawableRectangle = new Rectangle(drawStartPoint,
                new Size(RoundedTo(16, eLocation.X - drawStartPoint.X),
                    RoundedTo(16, eLocation.Y - drawStartPoint.Y)));
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!draw) return;
            if (drawableRectangle.Width != 0 && drawableRectangle.Height != 0)
                SaveDrawableRectangle();
            draw = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.ScaleTransform(sx, sy);
            if (Blocks.Count > 0) g.FillRectangles(new SolidBrush(Color.DimGray), Blocks.ToArray());
            if (TimeAnomalies.Count > 0) g.FillRectangles(new SolidBrush(Color.BlueViolet), TimeAnomalies.ToArray());
            if (ActiveStage.Spawn != Rectangle.Empty)
                g.FillRectangle(new SolidBrush(Color.Goldenrod), ActiveStage.Spawn);
            if (ActiveStage.Target != Rectangle.Empty)
                g.FillRectangle(new SolidBrush(Color.DarkGreen), ActiveStage.Target);
            if (!drawableRectangle.IsEmpty) g.FillRectangle(new SolidBrush(Color.Firebrick), drawableRectangle);
        }

        private void SaveDrawableRectangle()
        {
            switch (ActiveDetail)
            {
                case ConstructorDetail.Block:
                    Blocks.Add(drawableRectangle);
                    break;
                case ConstructorDetail.TimeAnomaly:
                    TimeAnomalies.Add(drawableRectangle);
                    break;
                case ConstructorDetail.StartRectangle when drawableRectangle.Size == Explorer.DefaultColliderSize:
                    ActiveStage.Spawn = drawableRectangle;
                    break;
                case ConstructorDetail.StartRectangle:
                    mapConstructor.PrintException(
                        $"Стартовый прямоугольник должен быть размера {Explorer.DefaultColliderSize.Width}x{Explorer.DefaultColliderSize.Height}");
                    drawableRectangle = Rectangle.Empty;
                    break;
                case ConstructorDetail.TargetRectangle:
                    ActiveStage.Target = drawableRectangle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RemoveDrawableRectangle()
        {
            switch (ActiveDetail)
            {
                case ConstructorDetail.Block:
                    Blocks.Remove(drawableRectangle);
                    break;
                case ConstructorDetail.TimeAnomaly:
                    TimeAnomalies.Remove(drawableRectangle);
                    break;
                case ConstructorDetail.StartRectangle:
                    ActiveStage.Spawn = Rectangle.Empty;
                    break;
                case ConstructorDetail.TargetRectangle:
                    ActiveStage.Target = Rectangle.Empty;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            drawableRectangle = Rectangle.Empty;
        }

        private bool TryTakeExistingRectangle(Point point, out Rectangle rect)
        {
            rect = Rectangle.Empty;

            switch (ActiveDetail)
            {
                case ConstructorDetail.Block when Blocks.Any(b => b.Contains(point)):
                    rect = Blocks.First(b => b.Contains(point));
                    return true;
                case ConstructorDetail.Block:
                    return false;
                case ConstructorDetail.TimeAnomaly when TimeAnomalies.Any(b => b.Contains(point)):
                    rect = TimeAnomalies.First(b => b.Contains(point));
                    return true;
                case ConstructorDetail.TimeAnomaly:
                    return false;
                case ConstructorDetail.StartRectangle when ActiveStage.Spawn.Contains(point):
                    rect = ActiveStage.Spawn;
                    return true;
                case ConstructorDetail.StartRectangle:
                    return false;
                case ConstructorDetail.TargetRectangle when ActiveStage.Target.Contains(point):
                    rect = ActiveStage.Target;
                    return true;
                case ConstructorDetail.TargetRectangle:
                    return false;
                default:
                    throw new InvalidOperationException();
            }
        }

        private Bitmap BackGroundDots()
        {
            var dots = new Bitmap(1920, 1080);
            var g = Graphics.FromImage(dots);
            g.ScaleTransform(sx, sy);
            g.Clear(Color.Silver);
            for (var x = 0; x < 1920; x += 16)
            for (var y = 0; y < 1080; y += 16)
                g.FillEllipse(new SolidBrush(Color.DimGray), x - 1, y - 1, 2, 2);

            return dots;
        }

        private static int RoundedTo(int to, int value)
        {
            var x = value % to;
            if (x <= to / 2) return value - x;
            return value + to - x;
        }
    }
}