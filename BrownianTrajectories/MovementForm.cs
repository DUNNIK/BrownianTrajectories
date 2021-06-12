using System;
using System.Windows.Forms;

namespace BrownianTrajectories
{
    public partial class MovementWindow : Form
    {
        private readonly Form _mainForm;

        public MovementWindow(Form mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
            Movement = new Movement(pictureBox1, 1, 3, 7, 9, 11);
        }

        public Movement Movement { get; set; }

        private void createNewBallsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var movementMenuForm = new MovementMenuForm(this);
            movementMenuForm.ShowDialog();
        }

        private void вернутьсяВНачальноеОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mainForm.Visible = true;
            Visible = false;
        }
    }
}