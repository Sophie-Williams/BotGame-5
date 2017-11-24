using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public enum ShapeType
    {
        Filled,
        Outline
    }

    public class Circle
    {
        public float Radius;

        public Vector2 Position;

        public Color Colour;

        public Circle(Vector2 position, float radius, Color? colour = null)
        {
            Position = position;
            Radius = radius;

            Colour = colour ?? Color.White;
        }

        public void Draw(SpriteBatch batch)
        {
            //batch.Draw(texture, new Vector2(100, 100), Color.White);

            DebugDraw.DrawCircle(batch, Position, Radius, Colour);
        }
    }
}
