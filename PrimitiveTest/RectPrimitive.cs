using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public class RectPrimitive
    {
        private Texture2D texture;
        private Vector2 center;
        private float width;
        private float height;
        

        private Color[] data;

        public RectPrimitive(GraphicsDevice device, int width, int height, Vector2 position, Color color)
        {
            this.width = width;
            this.height = height;

            center = position;

            data = new Color[width * height];

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = color;
            }

            texture = new Texture2D(device, width, height);
            texture.SetData(data);
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texture, new Vector2(center.X + width/2, center.Y + height/2), Color.White);
           
        }

    }
}
