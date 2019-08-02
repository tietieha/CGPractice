using System;
using System.Collections.Generic;
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

        public Vertex[] _verts { get; set; }

        public Mesh(string name, Vertex[] vs, int[] indexs, Vector3[] normals)
        {
            this.name = name;
            this.vertexies = vs;
            triangles = new Triangle[indexs.Length / 3];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = new Triangle(indexs[i * 3], indexs[i * 3 + 1], indexs[i * 3 + 2], normals[i]);
            }
        }
    }
}
