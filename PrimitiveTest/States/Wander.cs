using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PrimitiveTest.States
{
    public class Wander : IState
    {
        private List<Node> path;

        public Wander()
        {
        }

        public void Enter(Bot bot)
        {
            bot.SetTarget(new Vector2(1000, 1000));

            path = bot.GetStaticMap().GetShortestPath(bot.GetPosition(), bot.GetTarget());
        }

        public void Update(Bot bot)
        {
            bot.FollowPath(path);

            if (Vector2.Distance(bot.GetPosition(), bot.GetTarget()) < 20)
            {
                //path = bot.GetStaticMap().GetShortestPath(bot.GetPosition(), Game.GetRandomPosition());
            }

            float distance = bot.GetPosition().Subtract(bot.GetTarget()).Length();

            if (distance < 5.0f)
            {
                bot.Stop();
            }
            else
            {
                float speed = distance / 1.0f;

                Vector2 desiredVelocity = bot.GetPosition().Subtract(bot.GetTarget());
                desiredVelocity = desiredVelocity.Scale(speed);

                bot.acceleration = bot.GetVelocity().Subtract(desiredVelocity);
            }

            if (!bot.GetStaticMap().IsLineOfSight(bot.GetPosition(), bot.GetTarget()))
            {
                //reevaulate path
                path = bot.GetStaticMap().GetShortestPath(bot.GetPosition(), bot.GetTarget());
            }
        }

        public void Exit(Bot bot)
        {
            
        }

        public override string ToString()
        {
            return "Wander";
        }
    }
}
