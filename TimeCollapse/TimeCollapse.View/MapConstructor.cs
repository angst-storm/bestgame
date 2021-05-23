using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public sealed class MapConstructor : UserControl
    {
        private readonly HashSet<Rectangle> blocks = new();
        private readonly ConstructorControl constructorControl;
        private bool draw;
        private Rectangle drawableRectangle;
        private Point drawStartPoint;

        public MapConstructor(ConstructorControl constructorControl)
        {
            this.constructorControl = constructorControl;
            BackgroundImage = BackGroundDots();

            constructorControl.RefreshButton.Click +=
                (sender, args) => constructorControl.ResultText.Text = CompileMap();

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

        private int ActiveStage => (int) constructorControl.Stages.SelectedItem;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (drawableRectangle.Contains(e.Location))
                {
                    blocks.Remove(drawableRectangle);
                    drawableRectangle = Rectangle.Empty;
                }
            }
            else
            {
                var crossRectangles = blocks.Where(b => b.Contains(e.Location)).ToList();
                if (crossRectangles.Any())
                {
                    drawableRectangle = crossRectangles.First();
                }
                else
                {
                    draw = true;
                    drawStartPoint = new Point(RoundedTo(16, e.Location.X), RoundedTo(16, e.Location.Y));
                    drawableRectangle = Rectangle.Empty;
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
                blocks.Add(drawableRectangle);
            draw = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (blocks.Count > 0) g.FillRectangles(new SolidBrush(Color.DimGray), blocks.ToArray());
            if (!drawableRectangle.IsEmpty) g.FillRectangle(new SolidBrush(Color.Firebrick), drawableRectangle);
        }

        private string CompileMap()
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("new[]");
            strBuilder.AppendLine("{");
            foreach (var block in blocks)
                strBuilder.AppendLine($"    new Rectangle({block.X}, {block.Y}, {block.Width}, {block.Height}),");
            strBuilder.AppendLine("}");
            return strBuilder.ToString();
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