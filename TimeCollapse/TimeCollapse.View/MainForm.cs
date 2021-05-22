using System.Windows.Forms;

namespace TimeCollapse.View
{
    public class MainForm : Form
    {
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

            Controls.AddRange(new Control[] {menu, game});
            ResumeLayout(false);

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        public void StartGame()
        {
            menu.Enabled = false;
            menu.Hide();

            game.Enabled = true;
            game.Show();
            game.Focus();
            game.StartGame();
        }

        public void ToMainMenu()
        {
            game.Enabled = false;
            game.Hide();
            
            menu.Enabled = true;
            menu.Show();
            menu.Focus();
        }
    }
}