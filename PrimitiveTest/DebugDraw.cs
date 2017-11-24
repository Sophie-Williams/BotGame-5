using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    
    public static class DebugDraw
    {
        private static Texture2D circleTexture;
        private static int radius = 50;

        private static Texture2D baseTexture;

        private static int lineWidth = 5;
        private static int lineHeight = 200;

        private static Texture2D lineTexture;

        static DebugDraw()
        {

            SetupCircle();

            SetupRect();

            lineTexture = new Texture2D(Game.graphics.GraphicsDevice, 1, 1);
            lineTexture.SetData<Color>(new Color[] {Color.White});
        }

        private static void SetupCircle()
        {
            Color[] data = new Color[radius * radius * 4];

            int diameter = radius * 2;
            float radiusSq = radius * radius;

            for (int x = 0; x < diameter; ++x)
            {
                for (int y = 0; y < diameter; ++y)
                {
                    int index = x * (int)diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);



                    if (pos.LengthSquared() <= radiusSq)
                    {
                        data[index] = Color.White;
                    }
                    else
                    {
                        data[index] = Color.Transparent;
                    }
                }
            }

            circleTexture = new Texture2D(Game.graphics.GraphicsDevice, diameter, diameter);
            circleTexture.SetData(data);
        }

        private static void SetupRect()
        {
            baseTexture = new Texture2D(Game.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            baseTexture.SetData(new[] { Color.White});
        }

        public static void DrawCircle(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(circleTexture, position, Color.White);
        }

        public static void DrawCircle(SpriteBatch batch, Vector2 position, float r, Color col)
        {
            Vector2 transformedPos = new Vector2();
            transformedPos.X = position.X - r;
            transformedPos.Y = position.Y - r;

            float scale;

            //original sprite is 100 x 100 (50 radius)

            scale = r / radius;

            batch.Draw(circleTexture, transformedPos, null, col, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void DrawCircle(SpriteBatch batch, float x, float y)
        {
            //batch.Draw(circleTexture, new Vector2(x, y), Color.White);
            DrawCircle(batch, new Vector2(x, y), 10, Color.White);
        }

        public static void DrawCircle(SpriteBatch batch, Vector2 position, Color colour)
        {
            //batch.Draw(circleTexture, position, colour);
            DrawCircle(batch, position, 10, colour);
        }

        public static void DrawCircle(SpriteBatch batch, float x, float y, Color colour)
        {
            //DrawCircle(batch, new Vector2(x, y), colour);
            DrawCircle(batch, new Vector2(x, y), 10, colour);
        }

        public static void DrawText(SpriteBatch batch, float x, float y, string s)
        {
            batch.DrawString(Game.SpriteFont, s, new Vector2(x, y), Color.White);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(lineTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }
}
