using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.RenderTool;
using CGPractice.Util;

namespace CGPractice.Math
{
    class MathUtil
    {
        // 获取平移矩阵
        public static Matrix4x4 GetTranlate(float x, float y, float z)
        {
            return new Matrix4x4(1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                x, y, z, 1);
        }

        //
        public static Matrix4x4 GetRotation(Vector3 v, float theta)
        {
            Matrix4x4 m = new Matrix4x4();
            v.Normaized();
            m.identity();
            float x = v.x;
            float y = v.y;
            float z = v.z;
            float cosT = (float)System.Math.Cos(theta);
            float sinT = (float)System.Math.Sin(theta);
            m[0, 0] = x * x * (1 - cosT) + cosT;
            m[0, 1] = x * y * (1 - cosT) + z * sinT;
            m[0, 2] = x * z * (1 - cosT) - y * sinT;

            m[1, 0] = x * y * (1 - cosT) - z * sinT;
            m[1, 1] = y * y * (1 - cosT) + cosT;
            m[1, 2] = y * z * (1 - cosT) + x * sinT;

            m[2, 0] = x * z * (1 - cosT) + y * sinT;
            m[2, 1] = y * z * (1 - cosT) - x * sinT;
            m[2, 2] = z * z * (1 - cosT) + cosT;

            return m;
        }

        // 获取旋转矩阵X
        public static Matrix4x4 GetRotationX(float r)
        {
            Matrix4x4 m = new Matrix4x4();
            m.identity();
            m[1, 1] = (float)System.Math.Cos(r);
            m[1, 2] = (float)System.Math.Sin(r);
            m[2, 1] = (float)(-System.Math.Sin(r));
            m[2, 2] = (float)System.Math.Cos(r);
            return m;
        }
        // 获取旋转矩阵Y
        public static Matrix4x4 GetRotationY(float r)
        {
            Matrix4x4 m = new Matrix4x4();
            m.identity();
            m[0, 0] = (float)System.Math.Cos(r);
            m[0, 2] = (float)(-System.Math.Sin(r));
            m[2, 0] = (float)System.Math.Sin(r);
            m[2, 2] = (float)System.Math.Cos(r);
            return m;
        }
        // 获取旋转矩阵Z
        public static Matrix4x4 GetRotationZ(float r)
        {
            Matrix4x4 m = new Matrix4x4();
            m.identity();
            m[0, 0] = (float)System.Math.Cos(r);
            m[0, 1] = (float)System.Math.Sin(r);
            m[1, 0] = (float)(-System.Math.Sin(r));
            m[1, 1] = (float)System.Math.Cos(r);
            return m;
        }

        // 获取缩放矩阵
        public static Matrix4x4 GetScale(float x, float y, float z)
        {
            return new Matrix4x4(x, 0, 0, 0,
                                0, y, 0, 0,
                                0, 0, z, 0,
                                0, 0, 0, 1);
        }
        // 获取缩放矩阵
        public static Matrix4x4 GetScale(Vector3 scale)
        {
            return new Matrix4x4(scale.x, 0, 0, 0,
                                0, scale.y, 0, 0,
                                0, 0, scale.z, 0,
                                0, 0, 0, 1);
        }

        // 获取视图矩阵 摄像机空间
        public static Matrix4x4 GetView(Camera camera)
        {
            Vector3 eye = camera.eye;
            Vector3 up = camera.up;
            // 反向平移
            Matrix4x4 t = MathUtil.GetTranlate(-eye.x, -eye.y, -eye.z);
            // 正向
            Vector3 dir = camera.look - camera.eye;
            // x方向
            Vector3 right = Vector3.Cross(camera.up, dir);
            right.Normaized();

            // 求逆， 单位即转置
            Matrix4x4 r = new Matrix4x4(right.x, up.x, dir.x, 0,
                                        right.y, up.y, dir.y, 0,
                                        right.z, up.z, dir.z, 0,
                                        0, 0, 0, 1);
            return t * r;
        }
        // 获取投影矩阵
        public static Matrix4x4 GetProject(Camera camera)
        {
            Matrix4x4 m = new Matrix4x4();
            m.setZero();
            m[0, 0] = (float)(1 / (System.Math.Tan(camera.fov * 0.5f) * camera.aspect));
            m[1, 1] = (float)(1 / System.Math.Tan(camera.fov * 0.5f));
            m[2, 2] = camera.viewFar / (camera.viewFar - camera.viewNear);
            m[2, 3] = 1f;
            m[3, 2] = (camera.viewNear * camera.viewFar) / (camera.viewNear - camera.viewFar);
            return m;
        }

        // float插值
        public static float Lerp(float from, float to, float t)
        {
            t = MathUtil.clamp(t);
            return to * t + (1 - t) * from;
        }

        // 颜色插值
        public static Color Lerp(Color from, Color to, float t)
        {
            t = MathUtil.clamp(t);
            return t * to + (1 - t) * from;
        }

        /// <summary>
        /// 屏幕空间插值生成新顶点，此时已近经过透视除法，z信息已经没有作用
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static void ScreenSpaceLerpVertex(ref Vertex v, Vertex v1, Vertex v2, float t)
        {
            v.onePerz = MathUtil.Lerp(v1.onePerz, v2.onePerz, t);
            //
            //v.u = MathUntil.Lerp(v1.u, v2.u, t);
            //v.v = MathUntil.Lerp(v1.v, v2.v, t);
            //
            v.color = MathUtil.Lerp(v1.color, v2.color, t);
            //
            v.lightColor = MathUtil.Lerp(v1.lightColor, v2.lightColor, t);
        }

        public static float clamp(float tar)
        {
            return MathUtil.Range(tar, 0f, 1.0f);
        }

        public static int Range(int v, int min, int max)
        {
            if (v <= min)
            {
                return min;
            }
            if (v >= max)
            {
                return max;
            }
            return v;
        }

        public static float Range(float v, float min, float max)
        {
            if (v <= min)
            {
                return min;
            }
            if (v >= max)
            {
                return max;
            }
            return v;
        }


    }
}
