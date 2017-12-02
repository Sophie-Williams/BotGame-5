using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PrimitiveTest.States
{
    public class Seek : IState
    {
        public Seek()
        {
            
        }

        public void Enter(Bot bot)
        {
            bot.SetTarget(bot.Enemy.GetPosition());
        }

        public void Update(Bot bot)
        {
            //Vector2D desiredVelocity = (mTargetPosition - position).unitVector() * MAXBOTSPEED;

            //Vector2D behaviourAccn = desiredVelocity - velocity;

            //return behaviourAccn;

            Vector2 desiredVelocity = Vector2.Subtract(bot.Enemy.GetPosition(), bot.GetPosition());
            desiredVelocity.Normalize();
            desiredVelocity = desiredVelocity.Scale(100.0f);

            bot.acceleration += desiredVelocity;

            if (bot.GetStaticMap().IsLineOfSight(bot.GetPosition(), bot.Enemy.GetPosition()))
            {
                bot.SetState(new Pursue());
            }
        }

        public void Exit(Bot bot)
        {
        }

        public override string ToString()
        {
            return "Seek";
        }
    }
}
