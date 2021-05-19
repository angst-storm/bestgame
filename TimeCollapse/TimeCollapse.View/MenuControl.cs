using System;
using System.Drawing;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public class MenuControl : UserControl
    {
        private readonly MainForm mainForm;

        public MenuControl(MainForm form)
        {
            mainForm = form;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "MenuControl";
            ClientSize = new Size(1024, 768);
            BackgroundImage = new Bitmap(Image.FromFile(@"Assets/Background.png"));

            var startButton = new Button
            {
                Name = "StartButton",
                Location = new Point(362, 210),
                Size = new Size(300, 100),
                BackColor = Color.DarkSlateGray,
                Text = @"Start Game",
                Font = new Font("Palatino Linotype", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204)
            };
            startButton.Click += (sender, args) => mainForm.StartGame();
            
            Controls.Add(startButton);
            ResumeLayout(false);
        }
    }
}