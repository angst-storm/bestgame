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
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 7));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 93));
            table.Controls.Add(InitializeControlTable(), 0, 0);
            table.Controls.Add(new MapConstructor(this) {Dock = DockStyle.Fill}, 0, 1);
            Controls.Add(table);
        }

        public ComboBox Details { get; private set; }
        public ComboBox Stages { get; private set; }

        private TableLayoutPanel InitializeControlTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));

            table.Controls.Add(ExitButton(), 0, 0);

            table.Controls.Add(DetailsTable(), 1, 0);

            table.Controls.Add(StagesTable(), 2, 0);
            
            table.Controls.Add(new Control(), 3, 0);

            var resultText = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            table.Controls.Add(TextOperationsTable(resultText), 4, 0);

            table.Controls.Add(resultText, 5, 0);

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
            exitButton.Click += (sender, args) => mainForm.ToMainMenu();
            return exitButton;
        }

        private TableLayoutPanel DetailsTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60));

            var label = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Azure,
                Text = @"Элементы:"
            };
            table.Controls.Add(label, 0, 0);

            Details = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Azure
            };
            Details.Items.AddRange(new object[] {"Блок", "Стартовый прямоугольник", "Целевой прямоугольник"});
            Details.SelectedIndex = 0;
            table.Controls.Add(Details, 0, 1);

            return table;
        }

        private TableLayoutPanel StagesTable()
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));

            var label = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.Azure,
                Text = @"Стадии:"
            };
            table.Controls.Add(label, 0, 0);

            Stages = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Azure
            };
            Stages.Items.Add(0);
            Stages.SelectedIndex = 0;
            table.Controls.Add(Stages, 0, 1);

            var increment = new Button
            {
                Size = new Size(Stages.Size.Height, Stages.Size.Height),
                Text = @"+",
                BackColor = Color.Azure,
                TextAlign = ContentAlignment.MiddleCenter
            };
            increment.Click += (sender, args) =>
            {
                Stages.Items.Add(Stages.Items.Count);
                Stages.SelectedIndex = Stages.Items.Count - 1;
            };
            table.Controls.Add(increment, 1, 1);

            return table;
        }

        private TableLayoutPanel TextOperationsTable(TextBox textBox)
        {
            var table = new TableLayoutPanel {Dock = DockStyle.Fill};
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            
            var refresh = new Button
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                Text = @"Обновить"
            };
            table.Controls.Add(refresh, 0, 0);
            
            var copy = new Button
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Azure,
                Text = @"Скопировать"
            };
            copy.Click += (sender, args) => Clipboard.SetText(textBox.Text);
            table.Controls.Add(copy, 0, 1);
            
            return table;
        }
    }
}