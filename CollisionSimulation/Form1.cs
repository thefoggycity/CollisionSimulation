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
        const Double    
            DEFAULT_SIZE = 2,
            DEFAULT_TIMERATE = 0.2,
            DEFAULT_ORI_X = 250,
            DEFAULT_ORI_Y = 180;
        const Boolean DEFAULT_COOR = true;

        public VisibleBall[] balls;
        public Double tr, infosz;
        public Ball.Position O;
        public Boolean Coor;

        Double sz;
        Boolean InitializedFlag = false;
        Bitmap bmp;
        Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            balls = new VisibleBall[]
            {
                new VisibleBall(0.5, 12, Color.MediumVioletRed, 3, 55, 15, -7),
                new VisibleBall(1, 18, Color.Lime, 80, 0, -12, 5),
                new VisibleBall(0.8, 14, Color.Aquamarine, 10, -5, 20, 10),
                new VisibleBall(1, 18, Color.Lime, -20, -5, 6, 12),
                new VisibleBall(0.5, 12, Color.MediumVioletRed, -50, 20, 15, -7),
            };
            O.x = DEFAULT_ORI_X;
            O.y = DEFAULT_ORI_Y;
            tr = DEFAULT_TIMERATE;
            Coor = DEFAULT_COOR;
            infosz = DEFAULT_SIZE;

            this.BackgroundImageLayout = ImageLayout.None;
            bmp = new Bitmap(this.Width, this.Height);
            this.BackgroundImage = bmp;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (InitializedFlag)
            {
                bmp = new Bitmap(this.Width, this.Height);
                g = Graphics.FromImage(bmp);
                this.BackgroundImage = bmp;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

                PlotCoordinate(ref g, O, Color.Black);
                foreach (VisibleBall b in balls)
                    b.Draw(ref g, O, sz);

                this.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            Form IniBall = new Form2()
            {
                balls = this.balls,
                tr = this.tr,
                Ori = this.O,
                infosz = this.infosz,
                Coor = this.Coor,
                GroundSize = this.Size
            };

            DialogResult r = IniBall.ShowDialog(this);
            if (r != DialogResult.OK)
            {
                timer1.Enabled = true;
                return;
            }
            InitializedFlag = true;

            if (checkBox1.Checked)
                sz = infosz;
            else
                sz = 0;

            g = Graphics.FromImage(bmp);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

            button1.Text = "Set Balls";
            button2.Text = "Pause";
            button2.Enabled = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < balls.GetLength(0); i++)
            {
                balls[i].Move(tr);
                if ((balls[i].Pos.x - balls[i].R + O.x <= 0 && balls[i].GetVx() < 0)
                    || (balls[i].Pos.x + balls[i].R + O.x >= this.Width && balls[i].GetVx() > 0))
                    balls[i].SetMomentum(-balls[i].P.x, balls[i].P.y);
                if ((balls[i].Pos.y - balls[i].R + O.y <= 0 && balls[i].GetVy() < 0)
                    || (balls[i].Pos.y + balls[i].R + O.y >= this.Height && balls[i].GetVy() > 0))
                    balls[i].SetMomentum(balls[i].P.x, -balls[i].P.y);
            }

            for (int i = 0; i < balls.GetUpperBound(0); i++)
                for (int j = i + 1; j <= balls.GetUpperBound(0); j++)
                    if (VisibleBall.ChkCollision(balls[i], balls[j]) <= 0)
                        VisibleBall.Collide(ref balls[i], ref balls[j], tr);

            g.Clear(Color.Transparent);
            PlotCoordinate(ref g, O, Color.Black);
            foreach(VisibleBall b in balls)
                b.Draw(ref g, O, sz);
            
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
                sz = infosz;
            else
                sz = 0;

            if (button2.Enabled) 
            { 
                g.Clear(Color.Transparent);
                PlotCoordinate(ref g, O, Color.Black);
                foreach (VisibleBall b in balls)
                    b.Draw(ref g, O, sz);
                this.Refresh();
            }
        }

        private void PlotCoordinate(ref Graphics Gra, Ball.Position Origin, Color Clr)
        {
            if (!Coor)
                return;
            Pen p = new Pen (Clr, 1);
            SolidBrush b = new SolidBrush(Clr);
            Gra.DrawLine(p, new Point(0, (int)Origin.y), new Point(this.Width, (int)Origin.y));
            Gra.DrawLine(p, new Point((int)Origin.x, 0), new Point((int)Origin.x, this.Height));
            if (sz != 0)
                Gra.DrawString(String.Format("({0},{1})", GetMouseRelativePosition().X, GetMouseRelativePosition().Y),
                    new Font(FontFamily.GenericMonospace, (float)(5 * sz)), b, 0, 0);
            p.Dispose();
            b.Dispose();
        }

        private Point GetMouseRelativePosition()
        {
            return new Point(this.PointToClient(MousePosition).X - (int)O.x, this.PointToClient(MousePosition).Y - (int)O.y);
        }

    }

    public sealed class VisibleBall : Ball
    {
        public Color Col;

        public VisibleBall(Double BallMass, Double BallRadius, Color BallColor, Position BallPosition, Vector BallMomentumOrVelocity)
            : base(BallMass, BallRadius, BallPosition, BallMomentumOrVelocity)
        {
            Col = BallColor;
        }
        public VisibleBall(Double BallMass, Double BallRadius, Color BallColor, Double X, Double Y, Double Vx, Double Vy)
            : base(BallMass, BallRadius, X, Y, BallMass * Vx, BallMass * Vy)
        {
            Col = BallColor;
        }
        public VisibleBall(Ball OriginalBall, Color BallColor)
            : base(OriginalBall)
        {
            Col = BallColor;
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
                PointF PtCentre = new PointF((float)(Pos.x + ShiftX), (float)(Pos.y + ShiftY));
                Gra.DrawString(String.Format("M={0:f}, R={1:f}\nPos=({2:f},{3:f})\nV=({4:f},{5:f})", Mass, R, Pos.x, Pos.y, GetVx(), GetVy()),
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
        public override string ToString()
        {
            return String.Format("{0} ball at ({1:f},{2:f}), M={3:f}, R={4:f}, V=({5:f},{6:f})", 
                Col.ToString().Substring(6), Pos.x, Pos.y, Mass, R, GetVx(), GetVy());
        }

        public static Ball ConvertType(VisibleBall TargetBall)
        {
            return new Ball(TargetBall.Mass, TargetBall.R, TargetBall.Pos, TargetBall.P);
        }
        public static VisibleBall ConvertType(Ball TargetBall, Color BallColor)
        {
            return new VisibleBall(TargetBall, BallColor);
        }

        public static Vector Collide(ref VisibleBall Ball1, ref VisibleBall Ball2, Double TimeRate)
        {
            Ball tmp1 = Ball1.Convert(),
                tmp2 = Ball2.Convert();
            Vector imp = Calc.Collide(ref tmp1, ref tmp2, TimeRate);
            Ball1.SetValue(tmp1);
            Ball2.SetValue(tmp2);
            return imp;
        }
        public static Vector Collide(ref VisibleBall Ball1, ref VisibleBall Ball2)
        {
            Ball tmp1 = Ball1.Convert(),
                tmp2 = Ball2.Convert();
            Vector imp = Calc.Collide(ref tmp1, ref tmp2);
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
        public static Vector Bounce(ref VisibleBall TargetBall, Vector WallDirection)
        {
            Ball tmp = TargetBall.Convert();
            Vector imp = Calc.Bounce(ref tmp, WallDirection);
            TargetBall.SetValue(tmp);
            return imp;
        }
    }

}
