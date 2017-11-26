using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PrimitiveTest
{
    public class Node
    {
        public Vector2 Position;

        public List<Node> Neighbours;
        public Node Parent;

        public float fScore;
        public float gScore;

        public Node(Vector2 p)
        {
            Position = p;
            Neighbours = new List<Node>();
        }

        public Node(float x, float y)
        {
            Position = new Vector2(x, y);
            Neighbours = new List<Node>();
        }
    }
}
