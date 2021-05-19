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

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "PauseControl";
            Size = new Size(100, 50);

            var resumeButton = new Button
            {
                Location = new Point(0, 0),
                Name = "ResumeButton",
                Size = new Size(100, 50),
                Text = @"Resume",
                UseVisualStyleBackColor = true,
                BackColor = Color.DarkSlateGray
            };
            resumeButton.Click += (sender, args) => mainForm.ResumeGame();

            Controls.Add(resumeButton);
            ResumeLayout(false);
        }
    }
}