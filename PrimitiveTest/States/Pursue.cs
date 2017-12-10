using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            //todo check if enemy is travelling vaguely towards you
            //if so, might as well set velocity to 0 and wait

            bot.acceleration += Behaviours.Pursue(bot);

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
