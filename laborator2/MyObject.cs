using OpenTK;
using OpenTK.Graphics.ES10;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace lab2
{
    class MyObject
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        

        public MyObject(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public void Render()
        {
            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(Color);
            GL.Vertex2(Position.X - 0.1f, Position.Y + 0.1f);
            GL.Vertex2(Position.X, Position.Y - 0.1f);
            GL.Vertex2(Position.X + 0.1f, Position.Y + 0.1f);

            GL.End();
        }
    }
}
