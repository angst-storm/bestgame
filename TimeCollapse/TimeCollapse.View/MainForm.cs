using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public class MainForm : Form
    {
        private readonly ConstructorControl constructor;
        private readonly GameControl game;
        private readonly MenuControl menu;

        public MainForm()
        {
            Name = "TimeCollapse";
            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;

            SuspendLayout();
            menu = new MenuControl(this) {Enabled = true};
            menu.Show();
            menu.Focus();

            game = new GameControl(this) {Enabled = false};
            game.Hide();

            constructor = new ConstructorControl(this) {Enabled = false};
            constructor.Hide();

            Controls.AddRange(new Control[] {menu, game, constructor});
            ResumeLayout(false);
        }

        public void StartGame()
        {
            menu.Enabled = false;
            menu.Hide();

            game.Enabled = true;
            game.Show();
            game.Focus();
            game.StartGameSeries(Map.Plot);
        }

        public void ToMainMenu()
        {
            game.Enabled = false;
            game.Hide();

            menu.Enabled = true;
            menu.Show();
            menu.Focus();
        }

        public void ToConstructor()
        {
            menu.Enabled = false;
            menu.Hide();

            constructor.Enabled = true;
            constructor.Show();
            constructor.Focus();
        }
    }
}