using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public class ShotManager
    {
        private List<Shot> allShots = new List<Shot>();

        private StaticMap staticMap;

        public ShotManager(StaticMap staticMap)
        {
            this.staticMap = staticMap;
        }

        public void AddShot(Vector2 start, Vector2 end)
        {
            allShots.Add(new Shot(start, end));
        }

        public void AddShot(float x1, float y1, float x2, float y2)
        {
            AddShot(new Vector2(x1, y1), new Vector2(x2, y2));
        }

        public void Clear()
        {
            allShots.Clear();
        }

        public void Draw(SpriteBatch b)
        {
            foreach (var shot in allShots)
            {
                shot.Draw(b);
            }
        }

        public void Update(float gameTime)
        {
            List<Shot> toRemove = new List<Shot>();
            foreach (var shot in allShots)
            {
                shot.Update(staticMap, gameTime);

                if (!shot.IsActive)
                {
                    toRemove.Add(shot);
                }
            }

            foreach (var s in toRemove)
            {
                if (!s.IsActive)
                {
                    allShots.Remove(s);
                }
            }
        }
    }

    public class Shot
    {
        public Vector2 Start;
        public Vector2 End;
        public Vector2 Position;

        private Vector2 diff;
        private Vector2 unit;

        public bool IsActive;

        public Shot(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;

            Position = Start;

            diff = End.Subtract(Start);

            unit = diff.Copy();
            unit.Normalize();
            IsActive = true;

        }

        public void Draw(SpriteBatch b)
        {
            Vector2 i, j;

            i = Position - (unit * 3);
            j = Position + (unit * 3);

            DebugDraw.DrawLine(b, i, j, Color.White);
        }

        public void Update(StaticMap map, float gameTime)
        {
            Position += (unit * 500f) * gameTime;

            if (Position.X < -100 || Position.X > 1300 || Position.Y < -100 || Position.Y > 1300)
            {
                IsActive = false;
            }

            foreach (var rect in map.rectangles)
            {
                if (map.IntersectsRect(rect, Position))
                {
                    IsActive = false;
                }
            }

            
        }
    }
}
