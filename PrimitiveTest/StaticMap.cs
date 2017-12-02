using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrimitiveTest
{
    public class StaticMap
    {
        public List<Rectangle> rectangles;

        private Texture2D texture;

        public List<Node> NodeMap;

        public List<Line> AllLines;

        public StaticMap()
        {
            rectangles = new List<Rectangle>();

            texture = new Texture2D(Game.graphics.GraphicsDevice, 100, 100);

            Color[] data = new Color[100 * 100];

            for (int i = 0; i < 100 * 100; ++i)
            {
                data[i] = Color.White;
            }

            texture.SetData(data);
        }

        public void AddRectangle(Rectangle rect)
        {
            rectangles.Add(rect);
        }

        public void AddRectangle(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, width, height);
            
            AddRectangle(rect);
        }

        public void DrawMap(SpriteBatch batch)
        {
            foreach (Rectangle rect in rectangles)
            {
                DrawRect(batch, rect);
            }
        }

        private void DrawRect(SpriteBatch batch, Rectangle rect)
        {
            batch.Draw(texture, rect, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void GetNodes()
        {
            NodeMap = new List<Node>();
            Rectangle rect = new Rectangle(-500, -500, 2000, 2000);

            GeneratePath(rect);

            CleanPath(); //removes any outside field of view

            GenerateLinesOfSight();
        }

        private void GenerateLinesOfSight()
        {
            //loop through all nodes
            //try make a line between all other nodes
            //if intersects with static map, don't add

            AllLines = new List<Line>();

            for (int i = 0; i < NodeMap.Count - 1; ++i)
            {
                for (int j = 1; j < NodeMap.Count; ++j)
                {
                    var n1 = NodeMap[i];
                    var n2 = NodeMap[j];
                    Line newLine = new Line(n1.Position, n2.Position);

                    if (newLine.Distance() < 400 && newLine.Distance() > 150)
                    {
                        if (IsLineOfSight(newLine))
                        {
                            AllLines.Add(newLine);

                            n1.Neighbours.Add(n2);
                        }
                    }
                }
            }
        }

        private float GetGScore(List<Node> path)
        {
            float total = 0;
            for (int i = 0; i < path.Count - 1; ++i)
            {
                total += Vector2.Distance(path[i].Position, path[i + 1].Position);
            }

            return total;
        }

        public List<Node> GetShortestPath(Vector2 start, Vector2 end)
        {
            foreach (Node node in NodeMap)
            {
                node.Parent = null;
            }

            Node startNode = GetClosestNodeToPoint(start);
            Node endNode = GetClosestNodeToPoint(end);

            List<Node> openPath = new List<Node>();
            List<Node> closedPath = new List<Node>();

            openPath.Add(startNode);

            startNode.gScore = 0;
            startNode.fScore = startNode.gScore + Vector2.Distance(startNode.Position, endNode.Position);
            
            do
            {
                //F - G + H
                //G - movement cost from start to current
                //H - estimated cost from current square to end

                var currentNode = GetLowestFScore(openPath);

                closedPath.Add(currentNode);
                openPath.Remove(currentNode);

                if (closedPath.Contains(endNode)) break; //got to the end

                foreach (var neighbour in currentNode.Neighbours)
                {
                    float cost = currentNode.gScore + Vector2.Distance(currentNode.Position, neighbour.Position);

                    if (openPath.Contains(neighbour) && cost < neighbour.gScore)
                    {
                        openPath.Remove(neighbour);
                    }
                    if (closedPath.Contains(neighbour) && cost < neighbour.gScore)
                    {
                        closedPath.Remove(neighbour);
                    }
                    if (!closedPath.Contains(neighbour) && !openPath.Contains(neighbour))
                    {
                        neighbour.gScore = cost;
                        openPath.Add(neighbour);
                        neighbour.Parent = currentNode;

                        neighbour.fScore = neighbour.gScore + Vector2.Distance(neighbour.Position, endNode.Position);
                    }
                }

            } while (openPath.Any());

            List<Node> shortestPath = new List<Node>();

            shortestPath.Add(endNode);

            Node n = endNode;

            do
            {
                if (n.Parent != null)
                {
                    shortestPath.Add(n.Parent);
                    n = n.Parent;
                }
                
            } while (n.Parent != null);

            shortestPath.Reverse();

            shortestPath.Add(new Node(end));

            return shortestPath;
        }

        private Node GetLowestFScore(List<Node> nodes)
        {
            float lowestScore = 10000000;
            Node n = null;

            foreach (Node node in nodes)
            {
                if (node.fScore < lowestScore)
                {
                    lowestScore = node.fScore;
                    n = node;
                }
            }

            return n;
        }

        private Node GetClosestNodeToPoint(Vector2 p)
        {
            float minDistance = 10000;
            Node shortestNode = null;

            foreach (Node n in NodeMap)
            {
                var distance = Vector2.Distance(p, n.Position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    shortestNode = n;
                }
            }

            return shortestNode;
        }

        public Vector2 GetNormalToSurface(Circle circle)
        {
            foreach (Rectangle rect in rectangles)
            {
                if (IntersectsRect(rect, circle))
                {
                    //find which edge of rect circle is closest to

                    int dl = (int)Math.Abs(rect.Left - circle.Position.X);
                    int dr = (int)Math.Abs(rect.Right - circle.Position.X);

                    int dt = (int)Math.Abs(rect.Top - circle.Position.Y);
                    int db = (int)Math.Abs(rect.Bottom - circle.Position.Y);

                    int smallest = dl;

                    if (smallest > dr)
                        smallest = dr;
                    if (smallest > dt)
                        smallest = dt;
                    if (smallest > db)
                        smallest = db;

                    if (smallest == dl)
                        return new Vector2(-1, 0);
                    if (smallest == dr)
                        return new Vector2(1, 0);
                    if (smallest == dt)
                        return new Vector2(0, -1);
                    if (smallest == db)
                        return new Vector2(0, 1);
                }
            }

            return Vector2.Zero;
        }

        public bool IntersectsRect(Rectangle rect, Circle circle)
        {
            //get centre of rectangle

            float centerX = rect.Left + rect.Width / 2f;
            float centerY = rect.Top + rect.Height / 2f;

            float dx = Math.Abs(circle.Position.X - centerX);
            float dy = Math.Abs(circle.Position.Y - centerY);

            if (dx > (rect.Width / 2f + circle.Radius)) return false;
            if (dy > (rect.Height / 2f + circle.Radius)) return false;

            if (dx <= (rect.Width / 2f))
            {
                return true;
            }

            if (dy <= (rect.Height / 2f))
            {
                return true;
            }

            float cornerDistSq = (dx - rect.Width / 2f) * (dx - rect.Width / 2f) + (dy - rect.Height / 2f) * (dy - rect.Height / 2f);

            if (cornerDistSq <= (circle.Radius * circle.Radius))
            {
                return true;
            }

            return false;
        }

        public bool IsLineOfSight(Vector2 pos1, Vector2 pos2)
        {
            Line l = new Line(pos1, pos2);

            return IsLineOfSight(l);
        }

        public  bool IsLineOfSight(Line line)
        {
            foreach (Rectangle rect in rectangles)
            {
                Line l1, l2, l3, l4;

                l1 = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
                l2 = new Line(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
                l3 = new Line(new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom));
                l4 = new Line(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top));

                if (Line.Intersects(line, l1).HasValue)
                {
                    return false;
                }

                if (Line.Intersects(line, l2).HasValue)
                {
                    return false;
                }

                if (Line.Intersects(line, l3).HasValue)
                {
                    return false;
                }

                if (Line.Intersects(line, l4).HasValue)
                {
                    return false;
                }
            }

            return true;
        }

        private void CleanPath()
        {
            var listToRemove = new List<Node>();
            for (int i = 0; i < NodeMap.Count; ++i)
            {
                var vec = NodeMap[i].Position;
                if (vec.X < 0 || vec.X > 1200 || vec.Y < 0 || vec.Y > 1200)
                {
                    listToRemove.Add(NodeMap[i]);
                }
            }

            for (int i = 0; i < listToRemove.Count; ++i)
            {
                NodeMap.Remove(listToRemove[i]);
            }
        }

        private void GeneratePath(Rectangle rect)
        {
            double width = rect.Width;
            double height = rect.Height;

            if (width * height > 500)
            {
                if (IsInsideBlock(rect))
                {
                    Rectangle[] rects = new Rectangle[4];

                    rects[0] = new Rectangle(rect.Left, rect.Top, rect.Width / 2, rect.Height / 2);
                    rects[1] = new Rectangle(rect.Left + rect.Width / 2, rect.Top, rect.Width / 2, rect.Height / 2);
                    rects[2] = new Rectangle(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2, rect.Width / 2, rect.Height / 2);
                    rects[3] = new Rectangle(rect.Left, rect.Top + rect.Height / 2, rect.Width / 2, rect.Height / 2);

                    for (int i = 0; i < 4; ++i)
                    {
                        GeneratePath(rects[i]);
                    }
                }
                else
                {
                    Node newNode = new Node(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
                    NodeMap.Add(newNode);
                }
            }
        }

        private bool IsInsideBlock(Rectangle rect)
        {
            foreach (Rectangle r in rectangles)
            {
                if (r.Intersects(rect))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
