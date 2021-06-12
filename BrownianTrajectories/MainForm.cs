using System;
using System.Windows.Forms;

namespace BrownianTrajectories
{
    public partial class MainForm : Form
    {
        private readonly MovementWindow _movementWindow;

        public MainForm()
        {
            _movementWindow = new MovementWindow(this) {Visible = false};
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void моделированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visible = true;
        }

        private void залипалкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _movementWindow.Visible = true;
            Visible = false;
        }
    }
}