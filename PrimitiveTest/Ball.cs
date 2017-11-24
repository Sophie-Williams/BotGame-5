using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public class Ball
    {
        private Circle circle;

        private Vector2 velocity;

        public Ball(Vector2 position, float radius, Color color)
        {
            circle = new Circle(position, radius, color);

            velocity = GetRandomVelocity();
        }

        public void Update(float dt)
        {
            circle.Position.X += velocity.X * dt;
            circle.Position.Y += velocity.Y * dt;

            if (circle.Position.X > 1200 - circle.Radius)
            {
                circle.Position.X = 1200 - circle.Radius;
                velocity.X = Math.Abs(velocity.X) * -1;
            }

            if (circle.Position.X < 0 + circle.Radius)
            {
                circle.Position.X = circle.Radius;
                velocity.X = Math.Abs(velocity.X);
            }

            if (circle.Position.Y < circle.Radius)
            {
                circle.Position.Y = circle.Radius;

                velocity.Y = Math.Abs(velocity.Y);
            }

            if (circle.Position.Y > 1200 - circle.Radius)
            {
                circle.Position.Y = 1200 - circle.Radius;
                velocity.Y = Math.Abs(velocity.Y) * -1;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            circle.Draw(batch);
        }

        public void CheckCollisions(Ball other)
        {
            float distance = Vector2.Distance(circle.Position, other.circle.Position);

            //float dx = Math.Abs(circle.Position.X - other.circle.Position.X);
            //float dy = Math.Abs(circle.Position.Y - other.circle.Position.Y);

            //float distance = (float)Math.Atan2(dy, dx);

            if (distance < (this.circle.Radius + other.circle.Radius))
            {
                Vector2 v1 = new Vector2(velocity.X, velocity.Y);
                Vector2 v2 = new Vector2(other.velocity.X, other.velocity.Y);

                velocity.X = v2.X;
                velocity.Y = v2.Y;

                other.velocity.X = v1.X;
                other.velocity.Y = v1.Y;

            }
        }

        private static Vector2 GetRandomVelocity()
        {
            float x = Game.random.Next(100, 200);
            float y = Game.random.Next(100, 200);

            float dirX = Game.random.Next(100);
            float dirY = Game.random.Next(100);

            if (dirX > 50)
            {
                x *= -1;
            }

            if (dirY > 50)
            {
                y *= -1;
            }

            return new Vector2(x, y);
        }
    }
}
