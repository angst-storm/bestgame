using System.Drawing;
using System.Windows.Forms;

namespace TimeCollapse.View
{
    public class MainForm : Form
    {
        private readonly GameControl game;
        private readonly MenuControl menu;
        private readonly PauseControl pause;

        public MainForm()
        {
            SuspendLayout();
            menu = new MenuControl(this) {Enabled = true};
            menu.Show();

            game = new GameControl(this);

            pause = new PauseControl(this);

            Controls.AddRange(new Control[] {menu, game, pause});
            ResumeLayout(false);

            Name = "TimeCollapse";
            ClientSize = new Size(1024, 768);

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
        }

        public void PauseGame()
        {
            game.Enabled = false;
            game.Hide();
            pause.Enabled = true;
            pause.Show();
        }

        public void ResumeGame()
        {
            pause.Enabled = false;
            pause.Hide();
            game.Enabled = true;
            game.Show();
            game.UpdateTimer.Start();
        }
    }
}