using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class MapConstructor : UserControl
    {
        private readonly HashSet<Rectangle> blocks = new();
        private readonly ConstructorControl constructorControl;
        private readonly List<(Rectangle, Rectangle)> stages = new();
        private bool draw;
        private Rectangle drawableRectangle;
        private Point drawStartPoint;

        public MapConstructor(ConstructorControl constructorControl)
        {
            this.constructorControl = constructorControl;
            BackgroundImage = BackGroundDots();

            constructorControl.RefreshButton.Click += (sender, args) => CompileMap();

            constructorControl.Stages.SelectedIndexChanged += (sender, args) => ChangeStage();
            ChangeStage();

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

        private int ActiveStage => constructorControl.Stages.SelectedIndex;

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
            if (blocks.Count > 0) g.FillRectangles(new SolidBrush(Color.DimGray), blocks.ToArray());
            if (stages[ActiveStage].Item1 != Rectangle.Empty)
                g.FillRectangle(new SolidBrush(Color.Goldenrod), stages[ActiveStage].Item1);
            if (stages[ActiveStage].Item2 != Rectangle.Empty)
                g.FillRectangle(new SolidBrush(Color.DarkGreen), stages[ActiveStage].Item2);
            if (!drawableRectangle.IsEmpty) g.FillRectangle(new SolidBrush(Color.Firebrick), drawableRectangle);
        }

        private void SaveDrawableRectangle()
        {
            switch (ActiveDetail)
            {
                case ConstructorDetail.Block:
                    blocks.Add(drawableRectangle);
                    break;
                case ConstructorDetail.StartRectangle when drawableRectangle.Size == Explorer.DefaultColliderSize:
                    stages[ActiveStage] = (drawableRectangle, stages[ActiveStage].Item2);
                    break;
                case ConstructorDetail.StartRectangle:
                    PrintException(
                        $"Стартовый прямоугольник должен быть размера {Explorer.DefaultColliderSize.Width}x{Explorer.DefaultColliderSize.Height}");
                    drawableRectangle = Rectangle.Empty;
                    break;
                case ConstructorDetail.TargetRectangle:
                    stages[ActiveStage] = (stages[ActiveStage].Item1, drawableRectangle);
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
                    blocks.Remove(drawableRectangle);
                    break;
                case ConstructorDetail.StartRectangle:
                    stages[ActiveStage] = (Rectangle.Empty, stages[ActiveStage].Item2);
                    break;
                case ConstructorDetail.TargetRectangle:
                    stages[ActiveStage] = (stages[ActiveStage].Item1, Rectangle.Empty);
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
                case ConstructorDetail.Block when blocks.Any(b => b.Contains(point)):
                    rect = blocks.First(b => b.Contains(point));
                    return true;
                case ConstructorDetail.Block:
                    return false;
                case ConstructorDetail.StartRectangle when stages[ActiveStage].Item1.Contains(point):
                    rect = stages[ActiveStage].Item1;
                    return true;
                case ConstructorDetail.StartRectangle:
                    return false;
                case ConstructorDetail.TargetRectangle when stages[ActiveStage].Item2.Contains(point):
                    rect = stages[ActiveStage].Item2;
                    return true;
                case ConstructorDetail.TargetRectangle:
                    return false;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void CompileMap()
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("new Map(new[]");
            strBuilder.AppendLine("{");
            foreach (var block in blocks)
                strBuilder.AppendLine($"    new Rectangle({block.X}, {block.Y}, {block.Width}, {block.Height}),");
            strBuilder.AppendLine("}, new[]");
            strBuilder.AppendLine("{");
            foreach (var stage in stages)
            {
                if (stage.Item1 == Rectangle.Empty)
                {
                    PrintException($"На стадии {stages.IndexOf(stage)} не задан стартовый прямоугольник");
                    return;
                }

                if (stage.Item2 == Rectangle.Empty)
                {
                    PrintException($"На стадии {stages.IndexOf(stage)} не задан целевой прямоугольник");
                    return;
                }

                strBuilder.AppendLine(
                    $"    new Stage(new Point({stage.Item1.X}, {stage.Item1.Y})," +
                    $" new Rectangle({stage.Item2.X}, {stage.Item2.Y}, {stage.Item2.Width}, {stage.Item2.Height})),");
            }

            strBuilder.AppendLine("});");
            constructorControl.ResultText.BackColor = constructorControl.ResultText.BackColor;
            constructorControl.ResultText.ForeColor = Color.Black;
            constructorControl.ResultText.Text = strBuilder.ToString();
        }

        private void PrintException(string message)
        {
            constructorControl.ResultText.BackColor = constructorControl.ResultText.BackColor;
            constructorControl.ResultText.ForeColor = Color.Red;
            constructorControl.ResultText.Text = message;
        }

        private void ChangeStage()
        {
            for (var i = 0; i < constructorControl.Stages.Items.Count - stages.Count; i++)
                stages.Add((Rectangle.Empty, Rectangle.Empty));
            Invalidate();
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