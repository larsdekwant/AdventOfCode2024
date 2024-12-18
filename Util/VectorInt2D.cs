using System;

namespace AdventOfCode
{
    struct VectorInt2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static readonly VectorInt2D EMPTY = new VectorInt2D(0, 0);

        public static readonly VectorInt2D UP    = new VectorInt2D(0, -1);
        public static readonly VectorInt2D RIGHT = new VectorInt2D(1,  0);
        public static readonly VectorInt2D DOWN  = new VectorInt2D(0,  1);
        public static readonly VectorInt2D LEFT  = new VectorInt2D(-1, 0);

        public static readonly VectorInt2D[] CardinalDirections = [UP, RIGHT, DOWN, LEFT];

        public VectorInt2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public IEnumerable<VectorInt2D> GetCardinalNeighbours()
        {
            int thisX = this.X;
            int thisY = this.Y;
            return CardinalDirections.Select(dir => new VectorInt2D(thisX + dir.X, thisY + dir.Y));
        }

        public static IEnumerable<VectorInt2D> GetCardinalNeighbours(VectorInt2D p)
        {
            return CardinalDirections.Select(dir => new VectorInt2D(p.X + dir.X, p.Y + dir.Y));
        }

        public static VectorInt2D TurnRight90(VectorInt2D v)
        {
            return new VectorInt2D(-v.Y, v.X);
        }

        public void TurnRight90()
        {
            int tmp = X;
            this.X = -Y;
            this.Y = tmp;
        }

        public static VectorInt2D TurnLeft90(VectorInt2D v)
        {
            return new VectorInt2D(v.Y, -v.X);
        }

        public void TurnLeft90()
        {
            int tmp = -X;
            this.X = Y;
            this.Y = tmp;
        }

        public static VectorInt2D operator +(VectorInt2D left, VectorInt2D right)
        {
            return new VectorInt2D(left.X + right.X, left.Y + right.Y);
        }
        public static VectorInt2D operator +(VectorInt2D left, (int X, int Y) right)
        {
            return new VectorInt2D(left.X + right.X, left.Y + right.Y);
        }

        public static VectorInt2D operator -(VectorInt2D left, VectorInt2D right)
        {
            return new VectorInt2D(left.X - right.X, left.Y - right.Y);
        }
        public static VectorInt2D operator -(VectorInt2D left, (int X, int Y) right)
        {
            return new VectorInt2D(left.X - right.X, left.Y - right.Y);
        }

        public static VectorInt2D operator *(VectorInt2D left, VectorInt2D right)
        {
            return new VectorInt2D(left.X * right.X, left.Y * right.Y);
        }
        public static VectorInt2D operator *(VectorInt2D left, (int X, int Y) right)
        {
            return new VectorInt2D(left.X * right.X, left.Y * right.Y);
        }

        public static VectorInt2D operator /(VectorInt2D left, VectorInt2D right)
        {
            return new VectorInt2D(left.X / right.X, left.Y / right.Y);
        }
        public static VectorInt2D operator /(VectorInt2D left, (int X, int Y) right)
        {
            return new VectorInt2D(left.X / right.X, left.Y / right.Y);
        }

        public static bool operator ==(VectorInt2D left, VectorInt2D right)
        {
            return left.X == right.X && left.Y == right.Y;
        }
        public static bool operator ==(VectorInt2D left, (int X, int Y) right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(VectorInt2D left, VectorInt2D right)
        {
            return left.X != right.X || left.Y != right.Y;
        }
        public static bool operator !=(VectorInt2D left, (int X, int Y) right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not VectorInt2D) return false;

            VectorInt2D vec = (VectorInt2D)obj;
            return (vec.X == this.X && vec.Y == this.Y);
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }
    }
}
