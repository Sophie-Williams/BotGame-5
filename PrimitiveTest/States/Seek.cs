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
            bot.acceleration += Behaviours.Seek(bot);

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
