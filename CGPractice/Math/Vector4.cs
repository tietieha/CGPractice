using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGPractice.Math
{
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        // ctor
        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0;
        }

        public Vector4(Vector3 v, float w)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = w;
        }

        // 长度的平方
        public float sqMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        // 长度
        public float Magnitude
        {
            get
            {
                return (float)System.Math.Sqrt(sqMagnitude);
            }
        }

        // 单位化
        public Vector4 Normaized()
        {
            float length = Magnitude;
            if (length != 0)
            {
                x /= length;
                y /= length;
                z /= length;
            }
            return this;
        }
        // 与矩阵相乘
        public static Vector4 operator *(Vector4 v, Matrix4x4 m)
        {
            Vector4 tmp = new Vector4();
            tmp.x = v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0] + v.w * m[3, 0];
            tmp.y = v.x * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1] + v.w * m[3, 1];
            tmp.z = v.x * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2] + v.w * m[3, 2];
            tmp.w = v.x * m[0, 3] + v.y * m[1, 3] + v.z * m[2, 3] + v.w * m[3, 3];
            return tmp;
        }

        // operate
        public static Vector4 operator +(Vector4 A, Vector4 B)
        {
            Vector4 v = new Vector4();
            v.x = A.x + B.x;
            v.y = A.y + B.y;
            v.z = A.z + B.z;
            v.w = 0;
            return v;
        }

        // operate
        public static Vector4 operator *(float t, Vector4 A)
        {
            Vector4 v = new Vector4();
            v.x = A.x * t;
            v.y = A.y * t;
            v.z = A.z * t;
            v.w = 0;
            return v;
        }


        public static Vector4 operator -(Vector4 A, Vector4 B)
        {
            Vector4 v = new Vector4();
            v.x = A.x - B.x;
            v.y = A.y - B.y;
            v.z = A.z - B.z;
            v.w = 0;
            return v;
        }

        // 点乘
        public static float Dot(Vector4 A, Vector4 B)
        {
            return A.x * B.x + A.y * B.y + A.z * B.z;
        }

        // 叉乘
        public static Vector4 Cross(Vector4 A, Vector4 B)
        {
            Vector4 v = new Vector4();
            v.x = A.y * B.z - A.z * B.y;
            v.y = A.x * B.z - A.z * B.x;
            v.z = A.x * B.y - A.y * B.x;
            v.w = 0;
            return v;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n[Vector4]");
            sb.Append(x);
            sb.Append(", ");
            sb.Append(y);
            sb.Append(", ");
            sb.Append(z);
            sb.Append(", ");
            sb.Append(w);
            return sb.ToString();
        }
    }
}
