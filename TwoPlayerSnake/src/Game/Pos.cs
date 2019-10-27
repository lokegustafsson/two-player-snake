using Serilog;
using System;

namespace TwoPlayerSnake.Game
{
    /// <summary>
    /// Represents positions in the game grid. Right is +x, Up is +y
    /// </summary>
#pragma warning disable 660, 661
    readonly struct Pos
    {
#pragma warning restore 660, 661

        internal readonly Mod X;
        internal readonly Mod Y;

        internal Pos(int x, int y) { X = x; Y = y; }
        private Pos(Mod x, Mod y) { X = x; Y = y; }

        internal bool IsAdjacent(Pos other)
        {
            return (X - other.X).Abs() + (Y - other.Y).Abs() == 1;
        }
        internal Pos Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return new Pos(X - 1, Y);
                case Direction.Right: return new Pos(X + 1, Y);
                case Direction.Up: return new Pos(X, Y + 1);
                case Direction.Down: return new Pos(X, Y - 1);
            }
            throw new ArgumentException("Received illegal Direction");
        }

        internal static Pos Random(Random rng)
        {
            return new Pos(rng.Next(Config.GridSize), rng.Next(Config.GridSize));
        }

        public static bool operator ==(Pos a, Pos b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Pos a, Pos b) => a.X != b.X || a.Y != b.Y;


#pragma warning disable 660, 661
        internal readonly struct Mod
        {
#pragma warning restore 660, 661

            private const int MODULUS = Config.GridSize;

            internal readonly int Val;

            private Mod(int val) { Val = val; }

            internal int Abs() => Math.Min(Val, MODULUS - Val);

            public static Mod operator +(Mod a, Mod b) => new Mod((a.Val + b.Val) % MODULUS);
            public static Mod operator -(Mod a, Mod b) => new Mod((a.Val - b.Val + MODULUS) % MODULUS);
            public static Mod operator ++(Mod a) => a + 1;
            public static Mod operator --(Mod a) => a - 1;
            public static bool operator ==(Mod a, Mod b) => a.Val == b.Val;
            public static bool operator !=(Mod a, Mod b) => a.Val != b.Val;
            public static implicit operator Mod(int x) => new Mod((x % MODULUS + MODULUS) % MODULUS);
            public static implicit operator int(Mod x) => x.Val;
        }
    }
}