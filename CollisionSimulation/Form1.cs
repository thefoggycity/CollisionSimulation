using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollisionSimulation
{
    public partial class Form1 : Form
    {
        VisibleBall a, b;
        Double tr;
        Ball.Position O;
        Bitmap bmp;
        Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Ball.Momentum m = Ball.Calc.Collide(ref a, ref b);
            MessageBox.Show(a.GetVx().ToString() + " " + a.GetVy().ToString() + "\n"
                + b.GetVx().ToString() + " " + b.GetVy().ToString() + "\n"
                + m.x.ToString() + " " + m.y.ToString());
             */
            a = new VisibleBall(0.5, 14.15, Color.MediumVioletRed, 0, 60, 15, -7);
            b = new VisibleBall(1, 14.15, Color.Lime, 80, 0, -12, 5);
            O.x = 200;
            O.y = 100;
            tr = 0.2;

            this.BackgroundImageLayout = ImageLayout.Center;
            this.BackgroundImage = bmp;

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a.Move(tr);
            b.Move(tr);
            if (VisibleBall.ChkCollision(a, b) <= 0)
                VisibleBall.Collide(ref a, ref b, tr);

            bmp = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(bmp);
            a.Draw(ref g, O);
            b.Draw(ref g, O);

            this.BackgroundImage = bmp;
            this.Refresh();
        }

        sealed class VisibleBall : Ball
        {
            Color Col;

            public VisibleBall(Double BallMass, Double BallRadius, Color BallColor, Position BallPosition, Momentum BallMomentum)
                : base (BallMass, BallRadius, BallPosition, BallMomentum)
            {
                Col = BallColor;
            }
            public VisibleBall(Double BallMass, Double BallRadius, Color BallColor, Position BallPosition, Velocity BallVelocity)
                : base(BallMass, BallRadius, BallPosition, Calc.GetMomentumByVelocity(BallMass, BallVelocity))
            {
                Col = BallColor;
            }
            public VisibleBall(Double BallMass, Double BallRadius, Color BallColor, Double X, Double Y, Double Vx, Double Vy)
                : base(BallMass, BallRadius, X, Y, BallMass * Vx, BallMass * Vy)
            {
                Col = BallColor;
            }
            public VisibleBall (Ball OriginalBall, Color BallColor)
                : base(OriginalBall)
            {
                Col=BallColor;
            }

            public void Draw(ref Graphics Gra, Double ShiftX, Double ShiftY)
            {
                SolidBrush brsh = new SolidBrush(Col);
                Gra.FillEllipse(brsh, (float)(Pos.x - R + ShiftX), (float)(Pos.y - R + ShiftY), (float)(2 * R), (float)(2 * R));
                brsh.Dispose();
            }
            public void Draw(ref Graphics Gra, Position OriginPoint)
            {
                Draw(ref Gra, OriginPoint.x, OriginPoint.y);
            }
            public void Draw(ref Graphics Gra)
            {
                Draw(ref Gra, 0, 0);
            }

            public Ball Convert()
            {
                return ConvertType(this);
            }

            public static Ball ConvertType(VisibleBall TargetBall)
            {
                return new Ball(TargetBall.Mass, TargetBall.R, TargetBall.Pos, TargetBall.P);
            }
            public static VisibleBall ConvertType(Ball TargetBall, Color BallColor)
            {
                return new VisibleBall(TargetBall, BallColor);
            }

            public static Momentum Collide(ref VisibleBall Ball1, ref VisibleBall Ball2, Double TimeRate)
            {
                Ball tmp1 = Ball1.Convert(),
                    tmp2 = Ball2.Convert();
                Momentum imp = Calc.Collide(ref tmp1, ref tmp2, TimeRate);
                Ball1.SetValue(tmp1);
                Ball2.SetValue(tmp2);
                return imp;

            }
            public static Momentum Collide(ref VisibleBall Ball1, ref VisibleBall Ball2)
            {
                Ball tmp1 = Ball1.Convert(),
                    tmp2 = Ball2.Convert();
                Momentum imp = Calc.Collide(ref tmp1, ref tmp2);
                Ball1.SetValue(tmp1);
                Ball2.SetValue(tmp2);
                return imp;
            }
            public static Double ChkCollision(VisibleBall Ball1, VisibleBall Ball2)
            {
                Ball tmp1 = Ball1.Convert(),
                    tmp2 = Ball2.Convert();
                return Calc.ChkCollision(tmp1, tmp2);
            }
        }
    }
}
