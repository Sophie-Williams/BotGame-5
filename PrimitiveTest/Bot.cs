using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public class Bot
    {
        //private Vector2 position;

        private Circle circle;
        private Vector2 velocity;
        public Vector2 acceleration;
        private double rotation;

        private bool isAiming;
        private bool isFiring;

        private int health;

        private Vector2 target;

        private static Random random;

        private StaticMap staticMap;

        //private int radius = 5;

        public Circle WallAvoidCircle;

        public Team Team;

        private Color teamColour;

        private IState currentState;
        private IState previousState;

        private double fireCooldown;
        private double accuracy;

        public StaticMap GetStaticMap() {  return staticMap; }

        public Bot Enemy;

        private float respawnTimer = 5;

        public IState GetState()
        {
            return currentState;
        }

        public void SetState(IState state)
        {
            if (currentState != null)
            {
                currentState.Exit(this);
                previousState = currentState;
            }
            
            currentState = state;
            currentState.Enter(this);
        }

        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public Color GetColour()
        {
            return teamColour;
        }

        public void Shoot()
        {
            isFiring = true;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health <= 0 && respawnTimer <= 0)
            {
                respawnTimer = 10.0f;
            }

            if (amount > 0)
            {
                accuracy = 0;
            }
            else
            {
                accuracy *= .5f;
            }
        }


        public Bot(Team team, Vector2 position, StaticMap map)
        {
            teamColour = team == Team.Red ? Color.Red : Color.Aqua;

            circle = new Circle(position, 5f, teamColour);
            random = new Random();
            Team = team;
            fireCooldown = 0;
            health = 100;
            respawnTimer = 1;

            target = new Vector2(random.Next(200, 1000), random.Next(200, 1000));

            staticMap = map;
        }

        public Vector2 GetPosition()
        {
            return circle.Position;
        }

        public Vector2 GetTarget()
        {
            return target;
        }

        public void Stop()
        {
            Vector2 temp = new Vector2(velocity.X, velocity.Y);

            if (temp != Vector2.Zero)
            {
                temp.Normalize();
            }
            else
            {
                temp = Vector2.Zero;
            }

            temp.X *= -1;
            temp.Y *= -1;

            temp.X *= 60;
            temp.Y *= 60;

            acceleration = temp;
        }

        public bool FollowPath(List<Node> path)
        {
            //find furthest item in path that has line of sight
            //set target to this position

            for (int i = path.Count-1; i >= 0; i--)
            {
                if (staticMap.IsLineOfSight(circle.Position, path[i].Position))
                {
                    SetTarget(path[i].Position);

                    if (i == 0)
                    {
                        return true;
                    }

                    break;
                }
            }

            return false;
        }

        public bool IsAlive()
        {
            return respawnTimer <= 0;
        }

        public void Update(float gameTime)
        {
            if (respawnTimer > 0)
            {
                respawnTimer -= gameTime;
                velocity = Vector2.Zero;
            }
            else
            {
                
                currentState.Update(this);

                if (acceleration.Length() > 60)
                {
                    acceleration = acceleration.Scale(60);
                }

                acceleration += WallAvoid();

                velocity.X += acceleration.X * gameTime * 5;
                velocity.Y += acceleration.Y * gameTime * 5;

                if (velocity.Length() > 100)
                {
                    velocity = velocity.Scale(100);
                }

                rotation = Math.Atan2(velocity.Y, velocity.X);

                circle.Position.X += velocity.X * gameTime;
                circle.Position.Y += velocity.Y * gameTime;

                foreach (Rectangle rect in staticMap.rectangles)
                {
                    if (staticMap.IntersectsRect(rect, circle))
                    {
                        circle.Position.X -= velocity.X * gameTime;
                        circle.Position.Y -= velocity.Y * gameTime;
                        velocity = Vector2.Zero;
                    }
                }

            }
            
        }

        private Vector2 WallAvoid()
        {
            if (velocity.Length() > 2.0)
            {
                Vector2 tempVel = velocity.Copy();
                
                WallAvoidCircle = new Circle(Vector2.Add(circle.Position, tempVel), 30.0f);
            }
            else
            {
                WallAvoidCircle = new Circle(circle.Position, 30.0f);
            }

            Vector2 returnVec = staticMap.GetNormalToSurface(WallAvoidCircle);

            returnVec = returnVec.Scale(200.0f);

            return returnVec;
        }

        public void SetTarget(float x, float y)
        {
            target.X = x;
            target.Y = y;
        }

        public void SetTarget(Vector2 vec)
        {
            target = vec;
        }

        public void Draw(SpriteBatch batch)
        {
            circle.Draw(batch);

            double angle = Math.Atan2(velocity.Y, velocity.X);

            Vector2 lineEnd = new Vector2((float)Math.Cos(angle) * circle.Radius * 2, (float)Math.Sin(angle) * circle.Radius * 2);

            //DebugDraw.DrawLine(batch, centerPos.X, centerPos.Y, centerPos.X + lineEnd.X, centerPos.Y + lineEnd.Y);
            //DebugDraw.DrawLine(batch, circle.Position, circle.Radius * 2, (float) angle, Color.Red);
            DebugDraw.DrawLine(batch, circle.Position, circle.Position + lineEnd, Color.White);

            //Vector2 centerTarget = new Vector2(target.X - circle.Radius * 2, target.Y - circle.Radius * 2);

            DebugDraw.DrawCircle(batch, target, Color.Red);

            if (WallAvoidCircle != null)
            {
                //WallAvoidCircle.Draw(batch);
            }

            // DebugDraw.DrawText(batch, 0, 0, string.Format("Acceleration : {0}", acceleration.Length()));
            //DebugDraw.DrawText(batch, 0, 50, string.Format("Distance To Target : {0}", circle.Position.Subtract(target).Length()));

        }
    }
}
