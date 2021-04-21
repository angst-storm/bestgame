using System.ComponentModel;

namespace TimeCollapse.View
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TestForm
            // 
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Name = "TestForm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TestForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TestForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TestForm_KeyUp);
            this.ResumeLayout(false);
        }
    }
}