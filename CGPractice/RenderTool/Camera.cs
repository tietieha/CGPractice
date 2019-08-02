using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;

namespace CGPractice.RenderTool
{
    class Camera
    {
        // 位置
        public Vector3 eye { get; set; }
        // 观察点位置
        public Vector3 look { get; set; }
        // 观察角
        public float fov { get; set; }
        // 宽高比
        public float aspect { get; set; }
        // 近剪裁面
        public float viewNear { get; set; }
        // 远剪裁面
        public float viewFar { get; set; }
        // 上方向
        public Vector3 up { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="eye">摄像机位置</param>
        /// <param name="look">观察点</param>
        /// <param name="fov">观察角度</param>
        /// <param name="aspect">宽高比</param>
        /// <param name="up">上方向</param>
        /// <param name="viewNear">近裁切</param>
        /// <param name="viewFar">远裁切</param>
        public Camera(Vector3 eye, Vector3 look, Vector3 up, float fov, float aspect, float viewNear, float viewFar)
        {
            this.eye = eye;
            this.look = look;
            this.up = up;
            this.fov = fov;
            this.aspect = aspect;
            this.viewNear = viewNear;
            this.viewFar = viewFar;
        }

    }
}
