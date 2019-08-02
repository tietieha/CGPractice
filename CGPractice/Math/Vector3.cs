using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGPractice.Math
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        // ctor
        public Vector3(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
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
        public Vector3 Normaized()
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

        // operate
        public static Vector3 operator +(Vector3 A, float s)
        {
            Vector3 v = new Vector3();
            v.x = A.x + s;
            v.y = A.y + s;
            v.z = A.z + s;
            return v;
        }

        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            Vector3 v = new Vector3();
            v.x = A.x + B.x;
            v.y = A.y + B.y;
            v.z = A.z + B.z;
            return v;
        }

        // operate
        public static Vector3 operator -(Vector3 A, float s)
        {
            Vector3 v = new Vector3();
            v.x = A.x - s;
            v.y = A.y - s;
            v.z = A.z - s;
            return v;
        }

        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            Vector3 v = new Vector3();
            v.x = A.x - B.x;
            v.y = A.y - B.y;
            v.z = A.z - B.z;
            return v;
        }

        public static Vector3 operator *(Vector3 v, float s)
        {
            Vector3 tmp = new Vector3();
            tmp.x = v.x * s;
            tmp.y = v.y * s;
            tmp.z = v.z * s;
            return tmp;
        }

        // 点乘
        public static float Dot(Vector3 A, Vector3 B)
        {
            return A.x * B.x + A.y * B.y + A.z * B.z;
        }

        // 叉乘
        public static Vector3 Cross(Vector3 A, Vector3 B)
        {
            Vector3 v = new Vector3();
            v.x = A.y * B.z - A.z * B.y;
            v.y = A.x * B.z - A.z * B.x;
            v.z = A.x * B.y - A.y * B.x;
            return v;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n[Vector3]");
            sb.Append(x);
            sb.Append(", ");
            sb.Append(y);
            sb.Append(", ");
            sb.Append(z);
            return sb.ToString();
        }
    }
}
