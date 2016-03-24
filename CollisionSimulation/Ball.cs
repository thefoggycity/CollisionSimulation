using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionSimulation
{
    class Ball
    {
        public Double Mass, R;
        public struct Position
        {
            public Double x;
            public Double y;
            public Position(Double X, Double Y)
            {
                x = X;
                y = Y;
            }
        }
        public Position Pos;
        public struct Momentum
        {
            public Double x;
            public Double y;
            public Momentum(Double X, Double Y)
            {
                x = X;
                y = Y;
            }
        }
        public Momentum P;
        public struct Velocity
        {
            public Double x;
            public Double y;
            public Velocity(Double X, Double Y)
            {
                x = X;
                y = Y;
            }
        }


        public Ball(Double BallMass, Double BallRadius, Position BallPosition, Momentum BallMomentum)
        {
            Mass = BallMass;
            R = BallRadius;
            Pos = BallPosition;
            P = BallMomentum;
        }
        public Ball(Double BallMass, Double BallRadius, Position BallPosition, Velocity BallVelocity)
        {
            Mass = BallMass;
            R = BallRadius;
            Pos = BallPosition;
            P = Calc.GetMomentumByVelocity(Mass, BallVelocity);
        }
        public Ball(Double BallMass, Double BallRadius, Double PosX, Double PosY, Double MomentumX, Double MomentumY)
        {
            Position BallPosition = new Position(PosX, PosY);
            Momentum BallMomentum = new Momentum(MomentumX, MomentumY);
            Mass = BallMass;
            R = BallRadius;
            Pos = BallPosition;
            P = BallMomentum;
        }
        public Ball(Double BallMass, Double BallRadius, Double PosX, Double PosY)
        {
            Position BallPosition = new Position(PosX, PosY);
            Momentum BallMomentum = new Momentum(0, 0);
            Mass = BallMass;
            R = BallRadius;
            Pos = BallPosition;
            P = BallMomentum;
        }
        public Ball(Ball BallToCopy)
        {
            Mass = BallToCopy.Mass;
            R = BallToCopy.R;
            Pos = BallToCopy.Pos;
            P = BallToCopy.P;
        }

        public void SetValue(Ball BallToCopy)
        {
            Mass = BallToCopy.Mass;
            R = BallToCopy.R;
            Pos = BallToCopy.Pos;
            P = BallToCopy.P;
        }
        public void SetMomentum(Double MomentumX, Double MomentumY)
        {
            P.x = MomentumX;
            P.y = MomentumY;
        }
        public void SetMomentumByVelocity(Double Vx, Double Vy)
        {
            P.x = Vx * Mass;
            P.y = Vy * Mass;
        }
        public void SetMomentumByVelocity(Velocity BallVelocity)
        {
            P = Calc.GetMomentumByVelocity(this.Mass, BallVelocity);
        }
        public void SetPosition(Double BallPosX, Double BallPosY)
        {
            Pos.x = BallPosX;
            Pos.y = BallPosY;
        }
        public void Move(Double TimeRate)
        {
            Pos.x += (GetVx() * TimeRate);
            Pos.y += (GetVy() * TimeRate);
        }

        public Momentum CollideWith(ref Ball TargetBall, Double TimeRate)
        {
            Ball tmp = new Ball(this);
            Momentum imp = Calc.Collide(ref tmp, ref TargetBall,TimeRate );
            SetValue(tmp);
            return imp;
        }
        public Momentum CollideWith(ref Ball TargetBall)
        {
            return CollideWith(ref TargetBall, 0);
        }


        public Double GetResultantMomentum()
        {
            return Calc.GetResultantMomentum(this);
        }
        public Double GetResultantVelocity()
        {
            return Calc.GetResultantVelocity(this);
        }
        public Velocity GetVelocity()
        {
            return new Velocity(this.GetVx(), this.GetVy());
        }
        public Double GetVx()
        {
            return P.x / Mass;
        }
        public Double GetVy()
        {
            return P.y / Mass;
        }
        public Double GetDistance(Ball TargetBall)
        {
            return Calc.GetDistance(this, TargetBall);
        }
        public Double GetDistance(Position TargetPosition)
        {
            return Calc.GetDistance(this.Pos, TargetPosition);
        }
        public Double GetDistance(Double PosX, Double PosY)
        {
            return Calc.GetDistance(this.Pos, new Position(PosX, PosY));
        }
        public Double ChkCollision(Ball TargetBall)
        {
            return Calc.ChkCollision(this, TargetBall);
        }
        
        public static class Calc
        {            
            public static Double GetDistance(Position Ball1Pos, Position Ball2Pos)
            {
                return Math.Sqrt(Math.Pow(Ball1Pos.x - Ball2Pos.x, 2) + Math.Pow(Ball1Pos.y - Ball2Pos.y, 2));
            }
            public static Double GetDistance(Ball Ball1, Ball Ball2)
            {
                return GetDistance(Ball1.Pos, Ball2.Pos);
            }

            public static Double GetResultantMomentum(Momentum BallP)
            {
                return Vector.Mode(BallP);
            }
            public static Double GetResultantMomentum(Ball TargetBall)
            {
                return Vector.Mode(TargetBall.P);
            }

            public static Momentum GetMomentumByVelocity(Double BallMass, Double Vx, Double Vy)
            {
                return new Momentum(BallMass * Vx, BallMass * Vy);
            }
            public static Momentum GetMomentumByVelocity(Double BallMass, Velocity BallVelocity)
            {
                return new Momentum(BallMass * BallVelocity.x, BallMass * BallVelocity.y);
            }

            public static Double GetResultantVelocity(Ball TargetBall)
            {
                return Vector.Mode(TargetBall.P) / TargetBall.Mass;
            }

            public static Velocity GetVelocity(Ball TargetBall)
            {
                return new Velocity(TargetBall.P.x / TargetBall.Mass, TargetBall.P.y / TargetBall.Mass);
            }
            public static Velocity GetVelocity(Double BallMass, Momentum BallP)
            {
                return new Velocity(BallP.x / BallMass, BallP.y / BallMass);
            }

            public static Double ChkCollision(Ball Ball1, Ball Ball2)
            {
                return (GetDistance(Ball1, Ball2) - Ball1.R - Ball2.R);
            }

            public static Single CorrectPosition(ref Ball Ball1, ref Ball Ball2, Double TimeRate)
            {
                const UInt16 ACCURACY = 20;
                UInt16 i;
                Double d = ChkCollision(Ball1, Ball2);
                if (d > 0)
                    return -1;
                Velocity v1 = new Velocity
                {
                    x = -TimeRate * Ball1.GetVx() / ACCURACY,
                    y = -TimeRate * Ball1.GetVy() / ACCURACY,
                };
                Velocity v2 = new Velocity
                {
                    x = -TimeRate * Ball2.GetVx() / ACCURACY,
                    y = -TimeRate * Ball2.GetVy() / ACCURACY,
                };
                for (i = 0; i < ACCURACY; i++)
                {
                    Ball1.Pos = Vector.Add(Ball1.Pos, v1);
                    Ball2.Pos = Vector.Add(Ball2.Pos, v2);
                    if (ChkCollision(Ball1, Ball2) >= 0)
                    {
                        Ball1.Pos = Vector.Add(Ball1.Pos, Vector.Multiple(v1, -1));
                        Ball2.Pos = Vector.Add(Ball2.Pos, Vector.Multiple(v2, -1));
                        return (Single)(i * 1.0 / ACCURACY);
                    }
                }
                return -1;
            }

            public static Momentum Collide(ref Ball Ball1, ref Ball Ball2, Double TimeRate)
            {
                Double d = ChkCollision(Ball1, Ball2);
                Single Correction = 0;
                Double k;
                if (d > 0)
                    return new Momentum(0, 0);
                if (TimeRate != 0)
                    Correction = CorrectPosition(ref Ball1, ref Ball2, TimeRate);

                Momentum Imp = new Momentum(Ball2.Pos.x - Ball1.Pos.x, Ball2.Pos.y - Ball1.Pos.y);

                if (Imp.x != 0)
                {
                    k = Imp.y / Imp.x;
                    if (k >= -1 && k <= 1)
                    {
                        Imp.x = 2 * (Ball1.P.x * Ball2.Mass - Ball2.P.x * Ball1.Mass + k * Ball1.P.y * Ball2.Mass - k * Ball2.P.y * Ball1.Mass)
                            / ((Ball1.Mass + Ball2.Mass) * (k * k + 1));
                        Imp.y = k * Imp.x;
                    }
                    else
                    {
                        k = Imp.x / Imp.y;
                        Imp.y = 2 * (Ball1.P.y * Ball2.Mass - Ball2.P.y * Ball1.Mass + k * Ball1.P.x * Ball2.Mass - k * Ball2.P.x * Ball1.Mass)
                            / ((Ball1.Mass + Ball2.Mass) * (k * k + 1));
                        Imp.x = k * Imp.y;
                    }
                }
                else
                {
                    Imp.y = 2 * (Ball2.Mass * Ball1.P.y - Ball1.Mass * Ball2.P.y) / (Ball1.Mass + Ball2.Mass);
                }
                Ball1.P = Vector.Subtract(Ball1.P, Imp);
                Ball2.P = Vector.Add(Ball2.P, Imp);

                if (TimeRate != 0 && Correction > 0)
                {
                    Ball1.Move(TimeRate * Correction);
                    Ball2.Move(TimeRate * Correction);
                }
                return Imp;
            }
            public static Momentum Collide(ref Ball Ball1, ref Ball Ball2)
            {
                return Collide(ref Ball1, ref Ball2, 0);
            }

            public static Momentum Bounce(ref Ball TargetBall, Momentum WallDirection)
            {
                Momentum WallMeta = Vector.GetMeta(WallDirection);
                Momentum imp = Vector.Subtract(Vector.Multiple(WallMeta, Vector.DotMultiple(TargetBall.P, WallMeta)), TargetBall.P);
                TargetBall.P = Vector.Add(TargetBall.P, Vector.Multiple(imp, 2));
                return imp;
            }

            static class Vector
            {
                public static Momentum Multiple(Momentum p, Double Multipler)
                {
                    p.x *= Multipler;
                    p.y *= Multipler;
                    return p;
                }
                public static Velocity Multiple(Velocity v, Double Multipler)
                {
                    v.x *= Multipler;
                    v.y *= Multipler;
                    return v;
                }
                public static Momentum Add(Momentum p1, Momentum p2)
                {
                    return new Momentum(p1.x + p2.x, p1.y + p2.y);
                }
                public static Position Add(Position  pos, Velocity v)
                {
                    return new Position(pos.x + v.x, pos.y + v.y);
                }
                public static Momentum Subtract(Momentum p1, Momentum p2)
                {
                    return new Momentum(p1.x - p2.x, p1.y - p2.y);
                }
                public static Position Subtract(Position pos, Velocity v)
                {
                    return new Position(pos.x - v.x, pos.y - v.y);
                }
                public static Double DotMultiple(Momentum p1, Momentum p2)
                {
                    return (p1.x * p2.x + p1.y * p2.y);
                }
                public static Double Mode(Momentum p)
                {
                    return Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2));
                }
                public static Momentum GetMeta(Momentum p)
                {
                    Double l = Mode(p);
                    return new Momentum(p.x / l, p.y / l);
                }
            }
        }
    }
}
