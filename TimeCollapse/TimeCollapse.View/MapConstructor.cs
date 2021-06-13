using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class MapConstructor : UserControl
    {
        private readonly MainForm mainForm;
        private readonly MapConstructorCanvas mapConstructorCanvas;
        private TextBox messages;
        private TextBox name;

        public MapConstructor(MainForm form)
        {
            mainForm = form;
            ClientSize = mainForm.Size;
            BackColor = Color.FromArgb(18, 62, 64);

            var table = new TableLayoutPanel
            {
                Location = new Point(),
                Size = ClientSize
            };
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 108));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, ClientSize.Height - 108));
            table.Controls.Add(InitializeControlTable(), 0, 0);

            mapConstructorCanvas = new MapConstructorCanvas(this) {Dock = DockStyle.Fill};
            table.Controls.Add(mapConstructorCanvas, 0, 1);
            Controls.Add(table);
        }

        public ComboBox Details { get; private set; }
        public ComboBox Stages { get; private set; }
        public BindingList<ConstructorStage> StagesList { get; private set; }

        public void PrintException(string message)
        {
            messages.BackColor = Color.Azure;
            messages.ForeColor = Color.Red;
            messages.Text = message;
        }

        private void PrintConfirmation(string message)
        {
            messages.BackColor = Color.Azure;
            messages.ForeColor = Color.Green;
            messages.Text = message;
        }

        private void SaveMap()
        {
            if (Map.AllMaps.Any(m => string.Compare(m.Name, name.Text, StringComparison.Ordinal) == 0))
            {
                PrintException("Карта с таким названием уже есть в коллекции");
                return;
            }

            foreach (var stage in StagesList)
            {
                if (stage.Spawn == Rectangle.Empty)
                {
                    PrintException($"На стадии {stage.Number} не задан стартовый прямоугольник");
                    return;
                }

                if (stage.Target == Rectangle.Empty)
                {
                    PrintException($"На стадии {stage.Number} не задан целевой прямоугольник");
                    return;
                }
            }

            Map.SaveMap(new Map(name.Text, mapConstructorCanvas.Blocks, mapConstructorCanvas.TimeAnomalies,
                StagesList.Select(cs => new Stage(cs.Spawn.Location, cs.Target))));

            PrintConfirmation($"Новая карта \"{name.Text}\" создана");
        }

        private TableLayoutPanel InitializeControlTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            table.Controls.Add(DetailsTable(), 0, 0);
            table.Controls.Add(StagesTable(), 1, 0);
            table.Controls.Add(CurrentMap(), 2, 0);
            table.Controls.Add(SavedMaps(), 3, 0);
            table.Controls.Add(ExitButton(), 4, 0);
            return table;
        }

        private GroupBox DetailsTable()
        {
            var group = new GroupBox {Dock = DockStyle.Fill, Text = @"Элементы", ForeColor = Color.Azure};
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Details = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Details.Items.AddRange(new object[]
                {"Блок", "Временные аномалии", "Стартовый прямоугольник", "Целевой прямоугольник"});
            Details.SelectedIndex = 0;
            table.Controls.Add(Details, 0, 0);

            group.Controls.Add(table);
            return group;
        }

        private GroupBox StagesTable()
        {
            var group = new GroupBox {Dock = DockStyle.Fill, Text = @"Стадии", ForeColor = Color.Azure};
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            StagesList = new BindingList<ConstructorStage> {new(0)};
            Stages = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                DataSource = StagesList,
                DisplayMember = "Number",
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            table.Controls.Add(Stages, 0, 0);

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Stages.Size.Height));
            var decrement = new Button
            {
                Size = new Size(Stages.Size.Height, Stages.Size.Height),
                Text = @"-",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            decrement.Click += (sender, args) =>
            {
                if (StagesList.Count > 1)
                    StagesList.RemoveAt(StagesList.Count - 1);
            };
            table.Controls.Add(decrement, 1, 0);

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Stages.Size.Height));
            var increment = new Button
            {
                Size = new Size(Stages.Size.Height, Stages.Size.Height),
                Text = @"+",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            increment.Click += (sender, args) =>
            {
                StagesList.Add(new ConstructorStage(StagesList.Count));
                Stages.SelectedIndex = StagesList.Count - 1;
            };
            table.Controls.Add(increment, 2, 0);

            group.Controls.Add(table);
            return group;
        }

        private GroupBox CurrentMap()
        {
            var group = new GroupBox {Dock = DockStyle.Fill, Text = @"Текущая карта", ForeColor = Color.Azure};
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            name = new TextBox
            {
                Dock = DockStyle.Fill,
                Text = "Название",
                BackColor = Color.Azure
            };
            table.Controls.Add(name, 0, 0);

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            var save = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Сохранить",
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            save.Click += (sender, args) => SaveMap();
            table.Controls.Add(save, 0, 1);

            group.Controls.Add(table);
            return group;
        }

        private GroupBox SavedMaps()
        {
            var group = new GroupBox {Dock = DockStyle.Fill, Text = @"Сохраненные карты", ForeColor = Color.Azure};
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            var open = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Открыть",
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            var panel1 = ListOfMaps(
                new Point(ClientSize.Width / 2 - 250, ClientSize.Height / 2 - 150),
                new Size(500, 300), list =>
                {
                    var map = (Map) list.SelectedItem;
                    name.Text = map.Name;
                    mapConstructorCanvas.Blocks = map.Blocks.ToHashSet();
                    mapConstructorCanvas.TimeAnomalies = map.TimeAnomalies.ToHashSet();
                    StagesList.Clear();
                    for (var i = 0; i < map.Stages.Count; i++)
                        StagesList.Add(new ConstructorStage(i)
                        {
                            Spawn = new Rectangle(map.Stages[i].Spawn, Explorer.DefaultColliderSize),
                            Target = map.Stages[i].Target
                        });
                    mapConstructorCanvas.Refresh();
                });
            Controls.Add(panel1);
            panel1.Enabled = false;
            panel1.Hide();
            open.Click += (sender, args) =>
            {
                panel1.Enabled = true;
                panel1.Show();
            };
            table.Controls.Add(open, 0, 0);

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            var delete = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Удалить",
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            var panel2 = ListOfMaps(new Point(ClientSize.Width / 2 - 250, ClientSize.Height / 2 - 150),
                new Size(500, 300),
                list => { Map.DeleteMap((Map) list.SelectedItem); });
            Controls.Add(panel2);
            panel2.Enabled = false;
            panel2.Hide();
            delete.Click += (sender, args) =>
            {
                panel2.Enabled = true;
                panel2.Show();
            };
            table.Controls.Add(delete, 0, 1);

            group.Controls.Add(table);
            return group;
        }

        private Panel ListOfMaps(Point location, Size size, Action<ListBox> okButtonClick)
        {
            var panel = new Panel {Location = location, Size = size};
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 220));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            var list = new ListBox
            {
                Dock = DockStyle.Fill,
                DataSource = Map.AllMaps,
                DisplayMember = "Name"
            };
            table.Controls.Add(list, 0, 0);

            var okButton = new Button
            {
                Dock = DockStyle.Fill,
                Text = "ОК",
                BackColor = Color.Azure
            };
            okButton.Click += (sender, args) =>
            {
                okButtonClick(list);
                panel.Enabled = false;
                panel.Hide();
            };
            table.Controls.Add(okButton, 0, 1);

            var exitButton = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Закрыть",
                BackColor = Color.Azure
            };
            exitButton.Click += (sender, args) =>
            {
                panel.Enabled = false;
                panel.Hide();
            };
            table.Controls.Add(exitButton, 0, 2);

            panel.Controls.Add(table);
            return panel;
        }

        private TableLayoutPanel ExitButton()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill, Margin = Padding.Empty};

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            var group = new GroupBox {Dock = DockStyle.Fill, Text = @"Сообщения", ForeColor = Color.Azure};
            messages = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = ""
            };
            group.Controls.Add(messages);
            table.Controls.Add(group, 0, 0);

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            var exitTable = new TableLayoutPanel {Dock = DockStyle.Fill};
            exitTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            exitTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            exitTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            var exitButton = new Button
            {
                Text = @"Назад",
                BackColor = Color.Azure,
                Dock = DockStyle.Fill
            };
            exitButton.Click += (sender, args) => mainForm.ToMainMenu(this);
            exitTable.Controls.Add(exitButton, 2, 0);

            table.Controls.Add(exitTable, 0, 1);
            return table;
        }
    }
}