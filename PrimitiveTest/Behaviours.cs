using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PrimitiveTest
{
    public static class Behaviours
    {
        public static Vector2 Seek(Bot bot)
        {
            Vector2 desiredVelocity = Vector2.Subtract(bot.Enemy.GetPosition(), bot.GetPosition());
            desiredVelocity.Normalize();
            desiredVelocity = desiredVelocity.Scale(100.0f);

            return desiredVelocity;
        }

        public static Vector2 Flee(Bot b)
        {
            return -Seek(b);
        }

        public static Vector2 Pursue(Bot bot)
        {
            float distance = Vector2.Subtract(bot.Enemy.GetPosition(), bot.GetPosition()).Length();

            float time = distance / 100.0f;

            Vector2 target = bot.Enemy.GetPosition() + bot.Enemy.GetVelocity() * time * 0.2f;

            Vector2 desiredVelocity = Vector2.Subtract(target, bot.GetPosition());
            desiredVelocity.Normalize();
            desiredVelocity = desiredVelocity.Scale(100.0f);

            return desiredVelocity;
        }
    }
}
