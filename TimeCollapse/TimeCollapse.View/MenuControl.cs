using System;
using System.Drawing;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public sealed class MenuControl : UserControl
    {
        public MenuControl(MainForm form)
        {
            ClientSize = form.Size;
            BackColor = Color.FromArgb(18, 62, 64);

            SuspendLayout();
            var table = InitializeTable();

            table.Controls.Add(new Label
            {
                Text = @"TimeCollapse",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Pixel Times", (float) table.Size.Height / 16),
                Dock = DockStyle.Fill
            }, 0, 0);

            table.Controls.Add(MyDefaultButton(@"StartGame", table.Size.Height / 20, form.StartGame), 0, 1);

            table.Controls.Add(MyDefaultButton(@"Settings", table.Size.Height / 20, () => { }), 0, 2);

            table.Controls.Add(MyDefaultButton(@"Exit", table.Size.Height / 20, Application.Exit), 0, 3);

            Controls.Add(table);
            ResumeLayout(false);
        }

        private TableLayoutPanel InitializeTable()
        {
            var tableSize = new Size(ClientSize.Width / 4, ClientSize.Height / 2);
            var tableLocation = new Point(ClientSize.Width / 2 - tableSize.Width / 2,
                ClientSize.Height / 2 - tableSize.Height / 2);
            var table = new TableLayoutPanel
            {
                Location = tableLocation,
                Size = tableSize,
                BackColor = Color.FromArgb(18, 62, 64)
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return table;
        }

        public static Button MyDefaultButton(string text, int textSize, Action action)
        {
            var button = new Button
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Pixel Times", textSize),
                BackColor = Color.DarkSlateGray,
                Dock = DockStyle.Fill
            };
            button.Click += (sender, args) => action();
            return button;
        }
    }
}