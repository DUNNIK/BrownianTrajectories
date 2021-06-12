using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BrownianTrajectories
{
    public class Movement
    {
        public Movement(PictureBox pictureBox, int veryBig, int big, int normal, int small, int verySmall)
        {
            TargetPictureBox = pictureBox;
            TargetPictureBox.MouseClick += PictureBoxClick;
            Size = new Size(TargetPictureBox.Width, TargetPictureBox.Height);
            CreateBalls(veryBig, big, normal, small, verySmall);
            PlaceBalls();
            TargetPictureBox.Paint += TargetPictureBoxPaint;
            TargetPictureBox.Invalidate();
            _timer = new Timer
            {
                Interval = 10
            };
            _timer.Tick += TimerTick;
            BallTouchedSide += BallTouchedSideHandler;
            BallTouchedToBall += BallTouchedToBallHandler;
            _timer.Start();
        }

        #region Properties and fields

        public PictureBox TargetPictureBox { get; }
        private readonly Timer _timer;
        private Size Size { get; set; }
        private Ball[] _balls;
        private double Area => Size.Width * Size.Height;
        public event EventHandler<BallTouchedSideArgs> BallTouchedSide;
        public event EventHandler<BallTouchedToBallArgs> BallTouchedToBall;

        #endregion

        #region Methods

        private void CreateBalls(int veryBig, int big, int normal, int small, int verySmall)
        {
            var sum = veryBig + big + normal + small + verySmall;
            _balls = new Ball[sum];
            for (var i = 0; i < sum; i++)
            {
                if (i < verySmall)
                {
                    _balls[i] = new Ball((int) BallSize.VerySmall,
                        Vector.CreateRandomVector((int) BallSpeed.VerySmall))
                    {
                        Color = Brushes.SeaGreen
                    };
                    continue;
                }

                if (i < verySmall + small)
                {
                    _balls[i] = new Ball((int) BallSize.Small,
                        Vector.CreateRandomVector((int) BallSpeed.Small))
                    {
                        Color = Brushes.SeaGreen
                    };
                    continue;
                }

                if (i < verySmall + small + normal)
                {
                    _balls[i] = new Ball((int) BallSize.Normal,
                        Vector.CreateRandomVector((int) BallSpeed.Normal))
                    {
                        Color = Brushes.SeaGreen
                    };
                    continue;
                }

                if (i < verySmall + small + normal + big)
                {
                    _balls[i] = new Ball((int) BallSize.Big,
                        Vector.CreateRandomVector((int) BallSpeed.Big))
                    {
                        Color = Brushes.SeaGreen
                    };
                    continue;
                }

                if (i < sum)
                    _balls[i] = new Ball((int) BallSize.VeryBig,
                        Vector.CreateRandomVector((int) BallSpeed.VeryBig))
                    {
                        Color = Brushes.SeaGreen
                    };
            }
        }

        private void PlaceBalls()
        {
            var sumArea = _balls.Sum(t => 4 * Math.Pow(t.Radius, 2));

            if (sumArea > Area / 2)
                throw new ArgumentOutOfRangeException(message: "Шарики занимают слишком много площади",
                    paramName: nameof(_balls));
            var random = new Random();
            var i = _balls.Length - 1;
            while (i >= 0)
            {
                var radius = (int) Math.Ceiling(_balls[i].Radius);
                var point = new Point(random.Next(radius, Size.Width - radius),
                    random.Next(radius, Size.Height - radius));
                var goodLocation = true;
                for (var k = i + 1; k < _balls.Length; k++)
                    if (Ball.IsIntersected(point, radius, _balls[k].Center, _balls[k].Radius))
                        goodLocation = false;
                if (goodLocation)
                {
                    _balls[i].Center = point;
                    i--;
                }
            }
        }

        private void Draw(Graphics graphics)
        {
            foreach (var t in _balls)
                graphics.FillEllipse(t.Color, (float) t.Center.X - (float) t.Radius,
                    (float) t.Center.Y - (float) t.Radius, height: (float) t.Radius * 2,
                    width: (float) t.Radius * 2);
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Destroy()
        {
            _timer.Stop();
            _timer.Enabled = false;
            TargetPictureBox.Paint -= TargetPictureBoxPaint;
            TargetPictureBox.MouseClick -= PictureBoxClick;
        }

        #endregion

        #region Event Handlers

        private void TargetPictureBoxPaint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            Draw(graphics);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            for (var i = 0; i < _balls.Length; i++)
            {
                if (BallTouchedSide == null) continue;
                var ball = _balls[i];
                ball.Move(_timer.Interval / 1000.0);
                if (ball.Center.Y <= ball.Radius)
                    BallTouchedSide(ball, new BallTouchedSideArgs(Sides.Top, ball.Center.Y,
                        Vector.AngleBetweenVectors(ball.Speed, new Vector(1, 0))));

                if (ball.Center.Y >= Size.Height - ball.Radius)
                    BallTouchedSide(ball, new BallTouchedSideArgs(Sides.Bottom, Size.Height - ball.Center.Y,
                        Vector.AngleBetweenVectors(ball.Speed, new Vector(1, 0))));

                if (ball.Center.X <= ball.Radius)
                    BallTouchedSide(ball, new BallTouchedSideArgs(Sides.Left, ball.Center.X,
                        Vector.AngleBetweenVectors(ball.Speed, new Vector(0, 1))));

                if (ball.Center.X >= Size.Width - ball.Radius)
                    BallTouchedSide(ball, new BallTouchedSideArgs(Sides.Right, Size.Width - ball.Center.X,
                        Vector.AngleBetweenVectors(ball.Speed, new Vector(0, 1))));

                for (var k = 0; k < _balls.Length; k++)
                {
                    if (k == i) continue;

                    if (!ball.IsIntersected(_balls[k]) || BallTouchedToBall == null) continue;
                    BallTouchedToBall(ball, new BallTouchedToBallArgs(ball, _balls[k]));
                }
            }

            TargetPictureBox.Invalidate();
        }

        private void PictureBoxClick(object sender, EventArgs e)
        {
            if (_timer.Enabled)
                _timer.Stop();
            else
                _timer.Start();
        }

        private void BallTouchedSideHandler(object sender, BallTouchedSideArgs e)
        {
            var ball = (Ball) sender;
            var distanceToSize = e.DistanceToSide;
            var distanceForMove = ball.Radius - distanceToSize + 0.001;
            switch (e.Side)
            {
                case Sides.Bottom:
                    ball.Center.Y -= distanceForMove;
                    distanceToSize = Size.Height - ball.Center.Y;
                    //
                    if (distanceToSize < ball.Radius) throw new Exception();
                    ball.Speed.Coordinates.Y = -ball.Speed.Coordinates.Y;
                    break;
                case Sides.Top:
                    ball.Center.Y += distanceForMove;
                    distanceToSize = ball.Center.Y;
                    //
                    if (distanceToSize < ball.Radius) throw new Exception();
                    ball.Speed.Coordinates.Y = -ball.Speed.Coordinates.Y;
                    break;
                case Sides.Left:
                    ball.Center.X += distanceForMove;
                    distanceToSize = ball.Center.X;
                    //
                    if (distanceToSize < ball.Radius) throw new Exception();
                    ball.Speed.Coordinates.X = -ball.Speed.Coordinates.X;
                    break;
                case Sides.Right:
                    ball.Center.X -= distanceForMove;
                    distanceToSize = Size.Width - ball.Center.X;
                    //
                    if (distanceToSize < ball.Radius) throw new Exception();
                    ball.Speed.Coordinates.X = -ball.Speed.Coordinates.X;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void SetNormalDistanceBetweenTouchedBalls(Ball mainBall, Ball targetBall)
        {
            var x = new Vector(mainBall.Center, targetBall.Center);
            var distanceBetweenBalls = Point.DistanceBetweenPoints(mainBall.Center, targetBall.Center);
            var distanceForMove = mainBall.Radius + targetBall.Radius - distanceBetweenBalls + 0.001;

            x *= -(1 / x.Length);
            x *= distanceForMove;

            mainBall.Center.X += x.Coordinates.X;
            mainBall.Center.Y += x.Coordinates.Y;
        }

        private static void BallTouchedToBallHandler(object sender, BallTouchedToBallArgs e)
        {
            var mainBall = e.MainBall;
            var targetBall = e.TargetBall;
            SetNormalDistanceBetweenTouchedBalls(mainBall, targetBall);

            var x = new Vector(mainBall.Center, targetBall.Center);
            x *= 1 / x.Length;

            var y = new Vector(x.Coordinates.Y, -x.Coordinates.X);

            var mainProjectionX = mainBall.Speed.Length * Vector.CosBetweenVectors(mainBall.Speed, x);
            var mainProjectionY = mainBall.Speed.Length * Vector.CosBetweenVectors(mainBall.Speed, y);
            var targetProjectionX = targetBall.Speed.Length * Vector.CosBetweenVectors(targetBall.Speed, x);
            var targetProjectionY = targetBall.Speed.Length * Vector.CosBetweenVectors(targetBall.Speed, y);

            var sumMass = mainBall.Mass + targetBall.Mass;
            var mainSpeed = mainProjectionX * (mainBall.Mass - targetBall.Mass) / sumMass
                            + targetProjectionX * (2 * targetBall.Mass) / sumMass;
            var targetSpeed = mainProjectionX + mainSpeed - targetProjectionX;

            if (mainProjectionX * targetProjectionX >= 0 && mainProjectionX < targetProjectionX)
            {
            }
            else
            {
                var mainVectorProjectionX = x * mainSpeed;
                var mainVectorProjectionY = y * mainProjectionY;
                var targetVectorProjectionX = x * targetSpeed;
                var targetVectorProjectionY = y * targetProjectionY;
                mainBall.Speed = mainVectorProjectionX + mainVectorProjectionY;
                targetBall.Speed = targetVectorProjectionX + targetVectorProjectionY;
            }
        }

        #endregion
    }

    public class Ball
    {
        #region Static members

        public static bool IsIntersected(Point point1, double radius1, Point point2, double radius2)
        {
            return Point.DistanceBetweenPoints(point1, point2) <= radius1 + radius2;
        }

        #endregion

        public bool IsIntersected(Ball ball)
        {
            return Point.DistanceBetweenPoints(Center, ball.Center) <= Radius + ball.Radius;
        }

        public void Move(double time)
        {
            Center.X += Speed.Coordinates.X * time;
            Center.Y += Speed.Coordinates.Y * time;
        }

        #region Properties and fields

        public Point Center { get; set; }
        private double _radius;

        public double Radius
        {
            private set => _radius = value < 0 ? -value : value;
            get => _radius;
        }

        public Vector Speed { get; set; }
        public double Mass => Radius;
        public Brush Color { get; set; }

        #endregion

        #region Constructors

        public Ball(double radius)
        {
            Radius = radius;
            Speed = new Vector();
        }

        public Ball(double radius, Vector speed)
        {
            Radius = radius;
            Speed = speed;
        }

        #endregion
    }

    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        #region Operators

        public static double DistanceBetweenPoints(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) +
                             Math.Pow(point1.Y - point2.Y, 2));
        }

        public static Point operator -(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y);
        }

        public static Point operator +(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y);
        }

        #endregion
    }

    public class Vector
    {
        private static readonly Random Random = new Random();

        public Point Coordinates { get; }
        public double Length => Math.Sqrt(Math.Pow(Coordinates.X, 2) + Math.Pow(Coordinates.Y, 2));

        public static Vector CreateRandomVector(double length)
        {
            var minValue = -length;
            var randomX = Random.NextDouble() * (length - minValue) + minValue;
            var y = Math.Sqrt(Math.Pow(length, 2) - Math.Pow(randomX, 2));
            var isNegative = Convert.ToBoolean(Random.Next(0, 2));
            y = isNegative ? -y : y;
            return new Vector(randomX, y);
        }

        public static double AngleBetweenVectors(Vector vector1, Vector vector2)
        {
            if (vector1.Length == 0.0 || vector2.Length == 0.0) return 0;
            return Math.Acos(vector1 * vector2 / (vector1.Length * vector2.Length));
        }

        public static double CosBetweenVectors(Vector vector1, Vector vector2)
        {
            if (vector1.Length == 0.0 || vector2.Length == 0.0) return 0;
            return vector1 * vector2 / (vector1.Length * vector2.Length);
        }

        public void Rotate(double angle)
        {
            var point = new Point(Coordinates.X, Coordinates.Y);
            Coordinates.X = point.X * Math.Cos(angle) - point.Y * Math.Sin(angle);
            Coordinates.Y = point.X * Math.Sin(angle) + point.Y * Math.Cos(angle);
        }

        #region Operators

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.Coordinates.X + b.Coordinates.X, a.Coordinates.Y + b.Coordinates.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.Coordinates.X - b.Coordinates.X, a.Coordinates.Y - b.Coordinates.Y);
        }

        public static double operator *(Vector a, Vector b)
        {
            return a.Coordinates.X * b.Coordinates.X + a.Coordinates.Y * b.Coordinates.Y;
        }

        public static Vector operator *(Vector vector, double k)
        {
            return new Vector(vector.Coordinates.X * k, vector.Coordinates.Y * k);
        }

        #endregion

        #region Constructors

        public Vector()
        {
        }

        public Vector(Point coordinates)
        {
            Coordinates = coordinates;
        }

        public Vector(double x, double y)
        {
            Coordinates = new Point(x, y);
        }

        public Vector(Point begin, Point end)
        {
            Coordinates = new Point(end.X - begin.X, end.Y - begin.Y);
        }

        #endregion
    }

    public enum BallSize
    {
        VerySmall = 3,
        Small = 5,
        Normal = 12,
        Big = 20,
        VeryBig = 45
    }

    public enum BallSpeed
    {
        VerySmall = 250,
        Small = 200,
        Normal = 150,
        Big = 100,
        VeryBig = 50
    }

    public enum Sides
    {
        Top,
        Bottom,
        Right,
        Left
    }

    public class BallTouchedSideArgs : EventArgs
    {
        public BallTouchedSideArgs(Sides side, double distanceToSide, double angleBetweenSpeedAndSide)
        {
            Side = side;
            DistanceToSide = distanceToSide;
        }

        public Sides Side { get; }
        public double DistanceToSide { get; }
    }

    public class BallTouchedToBallArgs : EventArgs
    {
        public BallTouchedToBallArgs(Ball mainBall, Ball targetBall)
        {
            MainBall = mainBall;
            TargetBall = targetBall;
        }

        public Ball MainBall { get; set; }
        public Ball TargetBall { get; set; }
    }
}