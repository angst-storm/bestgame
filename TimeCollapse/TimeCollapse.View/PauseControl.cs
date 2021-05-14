using System;
using System.Drawing;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public class PauseControl : UserControl
    {
        private readonly MainForm mainForm;

        public PauseControl(MainForm form)
        {
            mainForm = form;
            InitializeComponent();
        }

        private void ResumeButtonClick(object sender, EventArgs e)
        {
            mainForm.ResumeGame();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            var resumeButton = new Button
            {
                Location = new Point(20, 20),
                Name = "ResumeButton",
                Size = new Size(100, 50),
                Text = @"Resume",
                UseVisualStyleBackColor = true
            };
            resumeButton.Click += ResumeButtonClick;
            Controls.Add(resumeButton);

            Name = "PauseControl";
            AutoScaleMode = AutoScaleMode.Font;
            AutoScaleDimensions = new SizeF(8F, 16F);
            ResumeLayout(false);
        }
    }
}