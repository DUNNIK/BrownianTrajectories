using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrownianTrajectories
{
    public partial class MainForm : Form
    {
        private readonly List<Point> _coordinates = new List<Point>();
        private Thread _buttonProcess;
        private readonly MovementWindow _movementWindow;
        private bool _notStop;
        private int _stepForT;
        private class Point
        {
            private double _x;
            private double _y;
            private double _qu;
            private double _offset;
            public Point(double x, double y, double qu, double offset)
            {
                _x = x;
                _y = y;
                _qu = qu;
            }

            public double X => _x;

            public double Y => _y;

            public double Qu => _qu;

            public double Offset => _offset;
        }
        public MainForm()
        {
            _movementWindow = new MovementWindow(this) {Visible = false};
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await CreateChart();
        }

        private async Task CreateChart()
        {
            double distance = 0;
            chart1.Series[0].Points.Clear();
            chart1.BorderWidth = 2;
            _coordinates.Add(new Point(0, 0, CountQu(0), 0));
            chart1.Series[0].Points.AddXY(_coordinates[0].X, _coordinates[0].Y);
            var time = double.Parse(textBox1.Text);
            var iterations = double.Parse(textBox2.Text);
            var iter = 0;
            for (double i = 0; i < time; i += time / iterations)
            {
                await Task.Delay((int) (time / iterations * 1000));
                iter++;
                var currentQu = CountQu(_coordinates[iter - 1].Qu);
                var offset = CountOffset(_coordinates[iter - 1].Offset, time, iterations);
                var currentX = _coordinates[iter - 1].X + CountX(offset, currentQu);
                var currentY = _coordinates[iter - 1].Y + CountY(offset, currentQu);
                _coordinates.Add(new Point(currentX, currentY, currentQu, offset));
                chart1.Series[0].Points.AddXY(_coordinates[iter].X, _coordinates[iter].Y);
                distance += CountCurrentDistance(_coordinates[iter], _coordinates[iter - 1]);
                distanceLabel.Text = Math.Round(distance,2) + " мкм";
            }
        }

        private double CountCurrentDistance(Point current, Point previous)
        {
            return 100 * Math.Sqrt((current.X - previous.X) * (current.X - previous.X) +
                             (current.Y - previous.Y) * (current.Y - previous.Y));
        }
        private double CountX(double offset, double qu)
        {
            return offset * Math.Cos(qu);
        }
        private double CountY(double offset, double qu)
        {
            return offset * Math.Sin(qu);
        }

        private double CountOffset(double previousOffset, double time, double iterations)
        {
            var eps = _random.NextDouble();
            var normalDistribution = 1 / Math.Sqrt(2 * Math.PI) * Math.Exp((double)-(eps - 3) * (eps - 3) / 2);
            var offset = previousOffset + Math.Sqrt(time / (2 * iterations)) * normalDistribution;
            return offset;
        }
        private readonly Random _random = new Random();
        private double CountQu(double previous)
        {
            var y = _random.NextDouble();
            var cosQu = 2 * y - 1;
            var qu = Math.Acos(cosQu);
            return qu + previous;
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
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                return;
            }
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',')
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl( (e.KeyChar)))
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    button1.Focus();
                }
                return;
            }
            e.Handled = true;
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                return;
            }
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',')
            {
                if (textBox2.Text.IndexOf(',') != -1)
                {
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl( (e.KeyChar)))
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    button1.Focus();
                }
                return;
            }
            e.Handled = true;
        }
    }
}