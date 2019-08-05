using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGPractice.RenderTool
{
    class Material
    {
        /// <summary>
        /// 自发光颜色值
        /// </summary>
        public Color emissive;

        /// <summary>
        /// 漫反射颜色值
        /// </summary>
        public Color diffuse;

        /// <summary>
        /// 环境光颜色值
        /// </summary>
        public Color ambient;

        /// <summary>
        /// 高光颜色值
        /// </summary>
        public Color specular;

        /// <summary>
        /// 光泽度
        /// </summary>
        public float power;

        public Material(Color emissive, Color diffuse, Color specular, float power)
        {
            this.emissive = emissive;
            this.diffuse = diffuse;
            this.ambient = ambient;
            this.specular = specular;
            this.power = power;
        }
    }
}
