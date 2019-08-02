using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;

namespace CGPractice.RenderTool
{
    // 三角形
    class Triangle
    {
        // 记录一个三角形的三个顶点id
        public int[] v_array = new int[3];
        public Vector3 normal { get; set; }

        public Triangle(int vIndex_0, int vIndex_1, int vIndex_2, Vector3 normal)
        {
            v_array[0] = vIndex_0;
            v_array[1] = vIndex_1;
            v_array[2] = vIndex_2;
            this.normal = normal;
        }

        public int this[int i]
        {
            get { return this.v_array[i]; }
            set { this.v_array[i] = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[triangle]\n");
            sb.Append(v_array[0]);
            sb.Append(", ");
            sb.Append(v_array[1]);
            sb.Append(", ");
            sb.Append(v_array[2]);
            return sb.ToString();
        }
    }
}
