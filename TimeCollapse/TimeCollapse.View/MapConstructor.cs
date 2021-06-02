using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class MapConstructor : UserControl
    {
        public HashSet<Rectangle> Blocks { get; }= new();
        private readonly ConstructorControl constructorControl;
        private readonly List<(Rectangle, Rectangle)> stages = new();
        private bool draw;
        private Rectangle drawableRectangle;
        private Point drawStartPoint;

        public MapConstructor(ConstructorControl constructorControl)
        {
            this.constructorControl = constructorControl;
            BackgroundImage = BackGroundDots();

            constructorControl.Stages.SelectedIndexChanged += (sender, args) => Invalidate();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private ConstructorDetail ActiveDetail =>
            constructorControl.Details.SelectedItem.ToString() switch
            {
                "Блок" => ConstructorDetail.Block,
                "Стартовый прямоугольник" => ConstructorDetail.StartRectangle,
                "Целевой прямоугольник" => ConstructorDetail.TargetRectangle,
                _ => throw new InvalidOperationException()
            };

        private ConstructorStage ActiveStage => constructorControl.StagesList[constructorControl.Stages.SelectedIndex];

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (drawableRectangle.Contains(e.Location)) RemoveDrawableRectangle();
            }
            else
            {
                if (!TryTakeExistingRectangle(e.Location, out drawableRectangle))
                {
                    draw = true;
                    drawStartPoint = new Point(RoundedTo(16, e.Location.X), RoundedTo(16, e.Location.Y));
                }
            }

            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!draw) return;

            drawableRectangle = new Rectangle(drawStartPoint,
                new Size(RoundedTo(16, e.Location.X - drawStartPoint.X),
                    RoundedTo(16, e.Location.Y - drawStartPoint.Y)));
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
            if (Blocks.Count > 0) g.FillRectangles(new SolidBrush(Color.DimGray), Blocks.ToArray());
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
                case ConstructorDetail.StartRectangle when drawableRectangle.Size == Explorer.DefaultColliderSize:
                    ActiveStage.Spawn = drawableRectangle;
                    break;
                case ConstructorDetail.StartRectangle:
                    constructorControl.PrintException(
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

        private static Bitmap BackGroundDots()
        {
            // TODO адаптировать под разрешение
            var dots = new Bitmap(1920, 1080);

            var g = Graphics.FromImage(dots);
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