using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public class Line
    {
        public float X1, X2, Y1, Y2;

        public float Distance()
        {
            var dx = Math.Abs(X2 - X1);
            var dy = Math.Abs(Y2 - Y1);

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public Line(float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public Line(Vector2 a, Vector2 b)
        {
            X1 = a.X;
            Y1 = a.Y;

            X2 = b.X;
            Y2 = b.Y;
        }

        public Vector2? Intersects(Line other)
        {
            float p0_x = X1;
            float p1_x = X2;

            float p2_x = other.X1;
            float p3_x = other.X2;

            float p0_y = Y1;
            float p1_y = Y2;

            float p2_y = other.Y1;
            float p3_y = other.Y2;
            
            
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x;
            s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x;
            s2_y = p3_y - p2_y;

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                float tX = p0_x + (t * s1_x);
                float tY = p0_y + (t * s1_y);

                return new Vector2(tX, tY);
            }

            return null;
        }

        public static Vector2? Intersects(Line l1, Line l2)
        {
            return l1.Intersects(l2);
        }

        public void Draw(SpriteBatch batch)
        {
            DebugDraw.DrawLine(batch, new Vector2(X1, Y1), new Vector2(X2, Y2));
        }
    }
}
