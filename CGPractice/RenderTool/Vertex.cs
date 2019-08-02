using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;

namespace CGPractice.RenderTool
{
    // 顶点信息
    public struct Vertex
    {
        // 位置
        public Vector3 pos;
        // 顶点色
        public Color color;
        // 光照颜色
        public Color lightColor;
        // 深度矫正
        public float onePerz;

        public Vertex(Vector3 pos, Color color)
        {
            this.pos = pos;
            this.color = color;
            this.lightColor = new Color(1, 1, 1);
            this.onePerz = 1;
        }

    }
}
