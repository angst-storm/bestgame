using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class MenuControl : UserControl
    {
        public MenuControl(MainForm form)
        {
            ClientSize = form.Size;
            BackColor = Color.FromArgb(18, 62, 64);

            SuspendLayout();
            var menu = InitializeMenu();

            var (selectMap, selectMapList) = InitializeMapSelect(menu);
            selectMapList.DoubleClick += (sender, args) =>
                form.StartGame(this, new List<Map> {(Map) selectMapList.SelectedItem});

            menu.Controls.Add(new Label
            {
                Text = @"TimeCollapse",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Pixel Times", (float) menu.Size.Height / 16),
                Dock = DockStyle.Fill
            }, 0, 0);

            menu.Controls.Add(
                MyDefaultButton(@"StartGame", menu.Size.Height / 20, () => form.StartGame(this, Map.Plot)), 0, 1);

            menu.Controls.Add(MyDefaultButton(@"Select Map", menu.Size.Height / 20, () =>
            {
                menu.Enabled = false;
                menu.Hide();
                selectMap.Enabled = true;
                selectMap.Show();
            }), 0, 2);

            menu.Controls.Add(
                MyDefaultButton(@"Map Constructor", menu.Size.Height / 20, () => form.ToConstructor(this)), 0, 3);

            menu.Controls.Add(MyDefaultButton(@"Exit", menu.Size.Height / 20, Application.Exit), 0, 4);

            Controls.AddRange(new Control[] {menu, selectMap});

            ResumeLayout(false);
        }

        private TableLayoutPanel InitializeMenu()
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
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return table;
        }

        public static (TableLayoutPanel, ListBox) InitializeMapSelect(TableLayoutPanel menuTable)
        {
            var table = new TableLayoutPanel
            {
                Enabled = false,
                Location = menuTable.Location,
                Size = menuTable.Size
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
            table.Controls.Add(MyDefaultButton(@"Back", table.Size.Height / 20, () =>
            {
                table.Enabled = false;
                table.Hide();
                menuTable.Enabled = true;
                menuTable.Show();
            }));
            var mapList = new ListBox
            {
                Dock = DockStyle.Fill,
                DataSource = Map.Plot,
                DisplayMember = "Name"
            };
            table.Controls.Add(mapList);
            table.Hide();
            return (table, mapList);
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