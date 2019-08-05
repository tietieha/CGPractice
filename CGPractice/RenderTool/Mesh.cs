using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;
using CGPractice.Util;

namespace CGPractice.RenderTool
{
    class Mesh
    {
        public string name { get; set; }
        public Vertex[] vertexies { get; set; }
        public Triangle[] triangles { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Material material { get; set; }

        public Vertex[] _verts { get; set; }

        public Mesh(string name, Vector3[] positions, int[] faces, Color[] colors, Vector3[] normals, Point2D[] uvs, Material material)
        {
            this.name = name;
            this.material = material;
            vertexies = new Vertex[faces.Length];
            triangles = new Triangle[faces.Length / 3];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = new Triangle(i * 3, i * 3 + 1, i * 3 + 2, normals[i]);
            }

            int vertexIndex;
            Color color;
            Vector3 pos;
            Point2D uv;
            for (int i = 0; i < faces.Length; i++)
            {
                vertexIndex = faces[i];
                pos = positions[vertexIndex];
                color = colors[vertexIndex];
                uv = uvs[i];
                vertexies[i] = new Vertex(pos, color, uv.x, uv.y);
            }
        }
    }
}
