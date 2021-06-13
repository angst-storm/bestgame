using System.Collections.Generic;
using System.Windows.Forms;
using TimeCollapse.Models;

namespace TimeCollapse.View
{
    public class MainForm : Form
    {
        private readonly MapConstructor constructor;
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

            constructor = new MapConstructor(this) {Enabled = false};
            constructor.Hide();

            Controls.AddRange(new Control[] {menu, game, constructor});
            ResumeLayout(false);
        }

        public void ToMainMenu(UserControl from)
        {
            from.Enabled = false;
            from.Hide();

            menu.Enabled = true;
            menu.Show();
            menu.Focus();
        }

        public void StartGame(UserControl from, IEnumerable<Map> levels)
        {
            from.Enabled = false;
            from.Hide();

            game.Enabled = true;
            game.Show();
            game.Focus();
            game.StartGameSeries(levels);
        }

        public void ToConstructor(UserControl from)
        {
            from.Enabled = false;
            from.Hide();

            constructor.Enabled = true;
            constructor.Show();
            constructor.Focus();
        }
    }
}