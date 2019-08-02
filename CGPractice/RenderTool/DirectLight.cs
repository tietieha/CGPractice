using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;

namespace CGPractice.RenderTool
{
    class DirectLight : Light
    {
        public Vector3 dir;

        // ctor
        public DirectLight(Vector3 pos, Color lightColor, Vector3 dir) : base(pos, lightColor)
        {
            this.dir = dir;
        }
    }
}
