using System.Drawing;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public sealed class ConstructorControl : UserControl
    {
        private readonly MainForm mainForm;

        public ConstructorControl(MainForm form)
        {
            mainForm = form;
            ClientSize = mainForm.Size;
            BackColor = Color.FromArgb(18, 62, 64);

            var table = new TableLayoutPanel
            {
                Location = new Point(),
                Size = ClientSize
            };
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            table.Controls.Add(InitializeControlTable(), 0, 0);

            table.Controls.Add(Screen.PrimaryScreen.Bounds.Size == new Size(1920, 1080)
                ? new MapConstructor(this) {Dock = DockStyle.Fill}
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
        public Button RefreshButton { get; private set; }
        public Button Increment { get; private set; }
        public Button Decrement { get; private set; }
        public TextBox ResultText { get; private set; }

        private TableLayoutPanel InitializeControlTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

            table.Controls.Add(ExitButton(), 0, 0);

            table.Controls.Add(DetailsTable(), 1, 0);

            table.Controls.Add(StagesTable(), 2, 0);

            table.Controls.Add(new Panel(), 3, 0);

            table.Controls.Add(TextOperationsTable(), 4, 0);

            return table;
        }

        private Button ExitButton()
        {
            var exitButton = new Button
            {
                Text = @"Назад",
                BackColor = Color.Azure,
                Dock = DockStyle.Fill
            };
            exitButton.Click += (sender, args) => mainForm.ToMainMenu(this);
            return exitButton;
        }

        private GroupBox DetailsTable()
        {
            var group = new GroupBox {Dock = DockStyle.Fill, Text = @"Элементы", ForeColor = Color.Azure};

            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            table.Controls.Add(new Panel(), 0, 0);
            Details = new ComboBox {Dock = DockStyle.Fill, BackColor = Color.Azure};
            Details.Items.AddRange(new object[] {"Блок", "Стартовый прямоугольник", "Целевой прямоугольник"});
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
            Stages = new ComboBox {Dock = DockStyle.Fill, BackColor = Color.Azure};
            Stages.Items.Add(0);
            Stages.SelectedIndex = 0;
            table.Controls.Add(Stages, 0, 0);

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Stages.Size.Height));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Stages.Size.Height));

            Decrement = new Button
            {
                Size = new Size(Stages.Size.Height, Stages.Size.Height),
                Text = @"-",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            Decrement.Click += (sender, args) =>
            {
                if (Stages.Items.Count > 1)
                    Stages.Items.RemoveAt(Stages.Items.Count - 1);
                Stages.SelectedIndex = Stages.Items.Count - 1;
            };
            table.Controls.Add(Decrement, 1, 0);

            Increment = new Button
            {
                Size = new Size(Stages.Size.Height, Stages.Size.Height),
                Text = @"+",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Black,
                BackColor = Color.Azure
            };
            Increment.Click += (sender, args) =>
            {
                Stages.Items.Add(Stages.Items.Count);
                Stages.SelectedIndex = Stages.Items.Count - 1;
            };
            table.Controls.Add(Increment, 2, 0);

            group.Controls.Add(table);

            return group;
        }

        private TableLayoutPanel TextOperationsTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            ResultText = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            table.Controls.Add(ResultText, 1, 0);

            var buttonsTable = new TableLayoutPanel {Dock = DockStyle.Fill, Margin = new Padding(0)};
            buttonsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            buttonsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            RefreshButton = new Button
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                BackColor = Color.Azure,
                Text = @"Обновить"
            };
            buttonsTable.Controls.Add(RefreshButton, 0, 0);

            var copyButton = new Button
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                BackColor = Color.Azure,
                Text = @"Скопировать"
            };
            copyButton.Click += (sender, args) => Clipboard.SetText(ResultText.Text);
            buttonsTable.Controls.Add(copyButton, 0, 1);

            table.Controls.Add(buttonsTable, 0, 0);

            return table;
        }
    }
}