using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionSimulation
{
    public class Ball
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
        public struct Vector
        {
            public enum Type
            {
                Momentum,
                Velocity
            }
            public Double x;
            public Double y;
            public Type ValType;
            public Vector(Double X, Double Y, Type ValueType)
            {
                x = X;
                y = Y;
                ValType = ValueType;
            }
        }
        public Vector P;

        public Ball(Double BallMass, Double BallRadius, Position BallPosition, Vector BallMomentumOrVelocity)
        {
            Mass = BallMass;
            R = BallRadius;
            Pos = BallPosition;
            if (BallMomentumOrVelocity.ValType != Vector.Type.Momentum)
            {
                BallMomentumOrVelocity.x *= BallMass;
                BallMomentumOrVelocity.y *= BallMass;
                BallMomentumOrVelocity.ValType = Vector.Type.Momentum;
            }
            P = BallMomentumOrVelocity;
        }
        public Ball(Double BallMass, Double BallRadius, Double PosX, Double PosY, Double MomentumX, Double MomentumY)
        {
            Mass = BallMass;
            R = BallRadius;
            Pos = new Position(PosX, PosY);
            P = new Vector(MomentumX, MomentumY, Vector.Type.Momentum);
        }
        public Ball(Double BallMass, Double BallRadius, Double PosX, Double PosY)
        {
            Mass = BallMass;
            R = BallRadius;
            Pos = new Position(PosX, PosY);
            P = new Vector(0, 0, Vector.Type.Momentum);
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
        public void SetMomentumByVelocity(Vector BallVelocity)
        {
            if (BallVelocity.ValType == Vector.Type.Velocity)
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

        public Vector CollideWith(ref Ball TargetBall, Double TimeRate)
        {
            Ball tmp = new Ball(this);
            Vector imp = Calc.Collide(ref tmp, ref TargetBall,TimeRate );
            SetValue(tmp);
            return imp;
        }
        public Vector CollideWith(ref Ball TargetBall)
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
        public Vector GetVelocity()
        {
            return new Vector(this.GetVx(), this.GetVy(), Vector.Type.Velocity);
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

            public static Double GetResultantMomentum(Vector BallP)
            {
                return VectorUtil.Mode(BallP);
            }
            public static Double GetResultantMomentum(Ball TargetBall)
            {
                return VectorUtil.Mode(TargetBall.P);
            }

            public static Vector GetMomentumByVelocity(Double BallMass, Double Vx, Double Vy)
            {
                return new Vector(BallMass * Vx, BallMass * Vy, Vector.Type.Momentum);
            }
            public static Vector GetMomentumByVelocity(Double BallMass, Vector BallVelocity)
            {
                    return new Vector(BallMass * BallVelocity.x, BallMass * BallVelocity.y, Vector.Type.Momentum);
            }

            public static Double GetResultantVelocity(Ball TargetBall)
            {
                return VectorUtil.Mode(TargetBall.P) / TargetBall.Mass;
            }

            public static Vector GetVelocity(Ball TargetBall)
            {
                return new Vector(TargetBall.P.x / TargetBall.Mass, TargetBall.P.y / TargetBall.Mass, Vector.Type.Velocity);
            }
            public static Vector GetVelocity(Double BallMass, Vector BallP)
            {
                return new Vector(BallP.x / BallMass, BallP.y / BallMass, Vector.Type.Velocity);
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
                Vector v1 = new Vector
                {
                    x = -TimeRate * Ball1.GetVx() / ACCURACY,
                    y = -TimeRate * Ball1.GetVy() / ACCURACY,
                };
                Vector v2 = new Vector
                {
                    x = -TimeRate * Ball2.GetVx() / ACCURACY,
                    y = -TimeRate * Ball2.GetVy() / ACCURACY,
                };
                for (i = 0; i < ACCURACY; i++)
                {
                    Ball1.Pos = VectorUtil.Add(Ball1.Pos, v1);
                    Ball2.Pos = VectorUtil.Add(Ball2.Pos, v2);
                    if (ChkCollision(Ball1, Ball2) >= 0)
                    {
                        Ball1.Pos = VectorUtil.Add(Ball1.Pos, VectorUtil.Multiple(v1, -1));
                        Ball2.Pos = VectorUtil.Add(Ball2.Pos, VectorUtil.Multiple(v2, -1));
                        return (Single)(i * 1.0 / ACCURACY);
                    }
                }
                return -1;
            }

            public static Vector Collide(ref Ball Ball1, ref Ball Ball2, Double TimeRate)
            {
                Double d = ChkCollision(Ball1, Ball2);
                Single Correction = 0;
                Double k;
                if (d > 0)
                    return new Vector(0, 0,Vector.Type.Momentum);
                if (TimeRate != 0)
                    Correction = CorrectPosition(ref Ball1, ref Ball2, TimeRate);

                Vector Imp = new Vector(Ball2.Pos.x - Ball1.Pos.x, Ball2.Pos.y - Ball1.Pos.y, Vector.Type.Momentum);

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
                Ball1.P = VectorUtil.Subtract(Ball1.P, Imp);
                Ball2.P = VectorUtil.Add(Ball2.P, Imp);

                if (TimeRate != 0 && Correction > 0)
                {
                    Ball1.Move(TimeRate * Correction);
                    Ball2.Move(TimeRate * Correction);
                }
                return Imp;
            }
            public static Vector Collide(ref Ball Ball1, ref Ball Ball2)
            {
                return Collide(ref Ball1, ref Ball2, 0);
            }

            public static Vector Bounce(ref Ball TargetBall, Vector WallDirection)
            {
                Vector WallMeta = VectorUtil.GetMeta(WallDirection);
                Vector imp = VectorUtil.Subtract(VectorUtil.Multiple(WallMeta, VectorUtil.DotMultiple(TargetBall.P, WallMeta)), TargetBall.P);
                TargetBall.P = VectorUtil.Add(TargetBall.P, VectorUtil.Multiple(imp, 2));
                return imp;
            }

            static class VectorUtil
            {
                public static Vector Multiple(Vector Vec, Double Multipler)
                {
                    Vec.x *= Multipler;
                    Vec.y *= Multipler;
                    return Vec;
                }
                public static Vector Add(Vector Vec1, Vector Vec2)
                {
                    return new Vector(Vec1.x + Vec2.x, Vec1.y + Vec2.y, Vec1.ValType);
                }
                public static Position Add(Position  pos, Vector Vec)
                {
                    return new Position(pos.x + Vec.x, pos.y + Vec.y);
                }
                public static Vector Subtract(Vector Vec1, Vector Vec2)
                {
                    return new Vector(Vec1.x - Vec2.x, Vec1.y - Vec2.y, Vec1.ValType);
                }
                public static Position Subtract(Position pos, Vector Vec)
                {
                    return new Position(pos.x - Vec.x, pos.y - Vec.y);
                }
                public static Double DotMultiple(Vector p1, Vector p2)
                {
                    return (p1.x * p2.x + p1.y * p2.y);
                }
                public static Double Mode(Vector p)
                {
                    return Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2));
                }
                public static Vector GetMeta(Vector p)
                {
                    Double l = Mode(p);
                    return new Vector(p.x / l, p.y / l, p.ValType);
                }
            }
        }
    }
}
