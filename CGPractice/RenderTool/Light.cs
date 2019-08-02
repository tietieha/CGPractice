using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;

namespace CGPractice.RenderTool
{
    class Light
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 pos { get; set; }

        /// <summary>
        /// 灯光颜色
        /// </summary>
        public Color lightColor { get; set; }

        public Light(Vector3 pos, Color lightColor)
        {
            this.pos = pos;
            this.lightColor = lightColor;
        }
    }
}
