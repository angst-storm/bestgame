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
            var startButton = new Button
            {
                Location = new Point(20, 20),
                Name = "StartButton",
                Size = new Size(100, 50),
                TabIndex = 0,
                Text = @"Start Game",
                UseVisualStyleBackColor = true
            };
            startButton.Click += StartButtonClick;
            Controls.Add(startButton);

            Name = "MenuControl";
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1024, 768);
            AutoScaleDimensions = new SizeF(8F, 16F);
            ResumeLayout(false);
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            mainForm.StartGame();
        }
    }
}