using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PrimitiveTest.States
{
    public class Pursue : IState
    {
        public void Enter(Bot bot)
        {
            
        }

        public void Update(Bot bot)
        {
            //double distance = (mTargetPosition - position).magnitude();

            //double time = distance / MAXBOTSPEED * persuitType;

            //Vector2D target = mTargetPosition + mTargetVelocity * time;

            //return Seek(position, velocity);

            float distance = Vector2.Subtract(bot.Enemy.GetPosition(),bot.GetPosition()).Length();

            float time = distance / 100.0f;

            Vector2 target = bot.Enemy.GetPosition() + bot.Enemy.GetVelocity() * time;

            Vector2 desiredVelocity = Vector2.Subtract(target, bot.GetPosition());
            desiredVelocity.Normalize();
            desiredVelocity = desiredVelocity.Scale(100.0f);

            bot.acceleration += desiredVelocity;

            if (!bot.GetStaticMap().IsLineOfSight(bot.GetPosition(), bot.Enemy.GetPosition()))
            {
                bot.SetState(new Wander());
            }

        }

        public void Exit(Bot bot)
        {
            
        }

        public override string ToString()
        {
            return "Pursue";
        }
    }
}
