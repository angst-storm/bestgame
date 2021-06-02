using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public sealed class ConstructorControl : UserControl
    {
        private readonly MainForm mainForm;
        private readonly MapConstructor mapConstructor;
        private TextBox messages;
        private TextBox name;

        public ConstructorControl(MainForm form)
        {
            mainForm = form;
            ClientSize = mainForm.Size;
            BackColor = Color.FromArgb(18, 62, 64);

            RecoverCustomMaps();

            var table = new TableLayoutPanel
            {
                Location = new Point(),
                Size = ClientSize
            };
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 108));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, ClientSize.Height - 108));
            table.Controls.Add(InitializeControlTable(), 0, 0);

            mapConstructor = new MapConstructor(this) {Dock = DockStyle.Fill};
            table.Controls.Add(Screen.PrimaryScreen.Bounds.Size == new Size(1920, 1080)
                ? mapConstructor
                : new Label
                {
                    Text =
                        @"К сожалению, пока конструктор доступен только пользователям с разрешением экрана 1920 на 1080",
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Azure,
                    Dock = DockStyle.Fill
                }, 0, 1);
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

        private void PrintGoodMessage(string message)
        {
            messages.BackColor = Color.Azure;
            messages.ForeColor = Color.Green;
            messages.Text = message;
        }

        private void CompileMap()
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

            Map.AllMaps.Add(new Map(name.Text, mapConstructor.Blocks, mapConstructor.TimeAnomalies,
                StagesList.Select(cs => new Stage(cs.Spawn.Location, cs.Target))));

            var sb = new StringBuilder();
            sb.Append(name.Text);
            sb.Append(';');
            foreach (var block in mapConstructor.Blocks)
                sb.Append($"{block.X}.{block.Y}.{block.Width}.{block.Height},");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(';');
            foreach (var anomaly in mapConstructor.TimeAnomalies)
                sb.Append($"{anomaly.X}.{anomaly.Y}.{anomaly.Width}.{anomaly.Height},");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(';');
            foreach (var stage in StagesList.Select(cs => new Stage(cs.Spawn.Location, cs.Target)))
                sb.Append(
                    $"{stage.Spawn.X}.{stage.Spawn.Y}.{stage.Target.X}.{stage.Target.Y}.{stage.Target.Width}.{stage.Target.Height},");
            sb.Remove(sb.Length - 1, 1);

            var sw = File.AppendText(@"UserMaps.txt");
            sw.WriteLine(sb.ToString());
            sw.Close();

            PrintGoodMessage($"Новая карта \"{name.Text}\" создана");
        }

        private void RecoverCustomMaps()
        {
            var maps = File.ReadLines(@"UserMaps.txt");
            foreach (var map in maps)
            {
                var tokens = map.Split(";");
                var mapName = tokens[0];
                var blocks = tokens[1].Split(",")
                    .Select(s => s.Split("."))
                    .Select(s => s.Select(int.Parse).ToArray())
                    .Select(s => new Rectangle(s[0], s[1], s[2], s[3]));
                var anomaly = tokens[2].Split(",")
                    .Select(s => s.Split("."))
                    .Select(s => s.Select(int.Parse).ToArray())
                    .Select(s => new Rectangle(s[0], s[1], s[2], s[3]));
                var stages = tokens[3].Split(",")
                    .Select(s => s.Split("."))
                    .Select(s => s.Select(int.Parse).ToArray())
                    .Select(s => new Stage(new Point(s[0], s[1]), new Rectangle(s[2], s[3], s[4], s[5])));
                Map.AllMaps.Add(new Map(mapName, blocks, anomaly, stages));
            }
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
                BackColor = Color.Azure
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
                DisplayMember = "Number"
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
            save.Click += (sender, args) => CompileMap();
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
                BackColor = Color.Azure,
                Enabled = false
            };
            table.Controls.Add(open, 0, 0);

            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            var delete = new Button
            {
                Dock = DockStyle.Fill,
                Text = "Удалить",
                ForeColor = Color.Black,
                BackColor = Color.Azure,
                Enabled = false
            };
            table.Controls.Add(delete, 0, 1);

            group.Controls.Add(table);
            return group;
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