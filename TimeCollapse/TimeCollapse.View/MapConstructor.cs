using System;
using System.Drawing;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public sealed class MapConstructor : UserControl
    {
        private readonly ConstructorControl control;

        public MapConstructor(ConstructorControl control)
        {
            this.control = control;
            BackColor = Color.Silver;
            PaintBackPoints();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private ConstructorDetail ActiveDetail =>
            control.Details.SelectedItem.ToString() switch
            {
                "Блок" => ConstructorDetail.Block,
                "Стартовый прямоугольник" => ConstructorDetail.StartRectangle,
                "Целевой прямоугольник" => ConstructorDetail.TargetRectangle,
                _ => throw new InvalidOperationException()
            };

        private int ActiveStage => (int) control.Stages.SelectedItem;

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        private void PaintBackPoints()
        {
            var backPoints = new PictureBox {Dock = DockStyle.Fill};
            Controls.Add(backPoints);
            backPoints.Paint += (sender, args) =>
            {
                var g = args.Graphics;
                for (var x = 0; x < 1920; x += 16)
                for (var y = 0; y < 1080; y += 16)
                    g.FillEllipse(new SolidBrush(Color.DimGray), x - 1, y - 1, 2, 2);
            };

            backPoints.Invalidate();
        }
    }
}