using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PrimitiveTest
{
    static public class VectorExtensions
    {
        public static Vector2 Subtract(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 Add(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 Scale(this Vector2 vec, float scale)
        {
            vec.Normalize();
            return new Vector2(vec.X * scale, vec.Y * scale);
        }
    }
}
