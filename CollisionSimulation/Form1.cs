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
        const Double    DEFAULT_SIZE = 2,
                        DEFAULT_TIMERATE = 0.2,
                        DEFAULT_ORI_X = 200,
                        DEFAULT_ORI_Y = 100;

        VisibleBall[] balls;
        Double tr, sz;
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

        private void button1_Click(object sender, EventArgs e)
        {
            balls = new VisibleBall[]
            {
                new VisibleBall(0.5, 14.15, Color.MediumVioletRed, 3, 55, 15, -7),
                new VisibleBall(1, 14.15, Color.Lime, 80, 0, -12, 5),
                new VisibleBall(0.8, 14.15, Color.Aquamarine, 10, -5, 20, 10)
            };

            O.x = DEFAULT_ORI_X;
            O.y = DEFAULT_ORI_Y;
            tr = DEFAULT_TIMERATE;

            if (checkBox1.Checked)
                sz = DEFAULT_SIZE;
            else
                sz = 0;

            this.BackgroundImageLayout = ImageLayout.Center;
            this.BackgroundImage = bmp;

            button1.Text = "Restart";
            button2.Text = "Pause";
            button2.Enabled = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (VisibleBall b in balls)
                b.Move(tr);

            for (int i = 0; i < balls.GetUpperBound(0); i++)
                for (int j = i + 1; j <= balls.GetUpperBound(0); j++)
                    if (VisibleBall.ChkCollision(balls[i], balls[j]) <= 0)
                        VisibleBall.Collide(ref balls[i], ref balls[j], tr);

            bmp = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(bmp);

            foreach(VisibleBall b in balls)
                b.Draw(ref g, O, sz);

            this.BackgroundImage = bmp;
            this.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                button2.Text = "Continue";
            }
            else
            {
                timer1.Enabled = true;
                button2.Text = "Pause";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                sz = DEFAULT_SIZE;
            else
                sz = 0;
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

            public void Draw(ref Graphics Gra, Double ShiftX, Double ShiftY, Double InfoSize)
            {
                SolidBrush brsh = new SolidBrush(Col);
                Gra.FillEllipse(brsh, (float)(Pos.x - R + ShiftX), (float)(Pos.y - R + ShiftY), (float)(2 * R), (float)(2 * R));
                if (InfoSize != 0)
                {
                    const Double FontSizeFactor = 4, LineSizeFactor = 3;
                    brsh.Color = Color.Black;
                    Pen p = new Pen(brsh, 1);
                    PointF PtCentre =  new PointF((float)(Pos.x + ShiftX), (float)(Pos.y + ShiftY));
                    Gra.DrawString(String.Format("M={0:f}, R={1:f}\nPos=({2:f},{3:f})\nV=({4:f},{5:f})",Mass,R,Pos.x,Pos.y,GetVx(),GetVy ()),
                        new Font(FontFamily.GenericSansSerif, (float)(FontSizeFactor * InfoSize)),
                        brsh, PointF.Add(PtCentre, new SizeF((float)R, (float)R)));
                    Gra.DrawLine(p, PtCentre, new PointF()
                    {
                        X = (float)(PtCentre.X + GetVx() * LineSizeFactor),
                        Y = (float)(PtCentre.Y + GetVy() * LineSizeFactor)
                    });
                    p.Dispose();
                }
                brsh.Dispose();
            }
            public void Draw(ref Graphics Gra, Double ShiftX, Double ShiftY)
            {
                Draw(ref Gra, ShiftX, ShiftY, 0);
            }
            public void Draw(ref Graphics Gra, Position OriginPoint)
            {
                Draw(ref Gra, OriginPoint.x, OriginPoint.y, 0);
            }
            public void Draw(ref Graphics Gra, Position OriginPoint, Double InfoSize)
            {
                Draw(ref Gra, OriginPoint.x, OriginPoint.y, InfoSize);
            }
            public void Draw(ref Graphics Gra)
            {
                Draw(ref Gra, 0, 0, 0);
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
