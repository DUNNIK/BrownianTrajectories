using System;
using System.Windows.Forms;

namespace BrownianTrajectories
{
    public partial class MovementMenuForm : Form
    {
        private readonly MovementWindow _movementWindow;

        public MovementMenuForm(MovementWindow movementWindow)
        {
            InitializeComponent();
            _movementWindow = movementWindow;
        }


        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var verySmall = Convert.ToInt32(numericUpDownVerySmall.Text);
                var small = Convert.ToInt32(numericUpDownSmall.Text);
                var normal = Convert.ToInt32(numericUpDownNormal.Text);
                var big = Convert.ToInt32(numericUpDownBig.Text);
                var veryBig = Convert.ToInt32(numericUpDownVeryBig.Text);
                _movementWindow.Movement.Destroy();
                _movementWindow.Movement = new Movement(_movementWindow.Movement.TargetPictureBox, veryBig, big, normal,
                    small, verySmall);
                Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка. Неправилно считались числа");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}