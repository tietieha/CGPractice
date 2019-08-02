using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Util;

namespace CGPractice.Math
{
    public struct Matrix4x4
    {
        // 4*4矩阵
        public float[,] _m;

        public Matrix4x4(float _00, float _01, float _02, float _03,
                        float _10, float _11, float _12, float _13,
                        float _20, float _21, float _22, float _23,
                        float _30, float _31, float _32, float _33)
        {
            _m = new float[4, 4];
            _m[0, 0] = _00; _m[0, 1] = _01; _m[0, 2] = _02; _m[0, 3] = _03;
            _m[1, 0] = _10; _m[1, 1] = _11; _m[1, 2] = _12; _m[1, 3] = _13;
            _m[2, 0] = _20; _m[2, 1] = _21; _m[2, 2] = _22; _m[2, 3] = _23;
            _m[3, 0] = _30; _m[3, 1] = _31; _m[3, 2] = _32; _m[3, 3] = _33;
        }

        // 填充
        public void setZero()
        {
            if (_m == null)
                _m = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _m[i, j] = 0;
                }
            }
        }

        // 单位化矩阵
        public void identity()
        {
            if (_m == null)
                _m = new float[4, 4];
            _m[0, 0] = 1; _m[0, 1] = 0; _m[0, 2] = 0; _m[0, 3] = 0;
            _m[1, 0] = 0; _m[1, 1] = 1; _m[1, 2] = 0; _m[1, 3] = 0;
            _m[2, 0] = 0; _m[2, 1] = 0; _m[2, 2] = 1; _m[2, 3] = 0;
            _m[3, 0] = 0; _m[3, 1] = 0; _m[3, 2] = 0; _m[3, 3] = 1;
        }

        // 
        public float this[int i, int j]
        {
            get { return _m[i, j]; }
            set { _m[i, j] = value; }
        }

        // 矩阵乘法
        public static Matrix4x4 operator *(Matrix4x4 A, Matrix4x4 B)
        {
            Matrix4x4 m = new Matrix4x4();
            m.setZero();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        m._m[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return m;
        }

        // 矩阵转置
        public Matrix4x4 Transpose()
        {
            float tmp;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tmp = _m[i, j];
                    _m[i, j] = _m[j, i];
                    _m[j, i] = tmp;
                }
            }
            return this;
        }
        public float Determinate()
        {
            return Determinate(_m, 4);
        }
        private float Determinate(float[,] m, int n)
        {
            if (n == 1)
            {//递归出口
                return m[0, 0];
            }
            else
            {
                float result = 0;
                float[,] tempM = new float[n - 1, n - 1];
                for (int i = 0; i < n; i++)
                {
                    //求代数余子式
                    for (int j = 0; j < n - 1; j++)//行
                    {
                        for (int k = 0; k < n - 1; k++)//列
                        {
                            int x = j + 1;//原矩阵行
                            int y = k >= i ? k + 1 : k;//原矩阵列
                            tempM[j, k] = m[x, y];
                        }
                    }

                    result += (float)System.Math.Pow(-1, 1 + (1 + i)) * m[0, i] * Determinate(tempM, n - 1);
                }
                return result;
            }
        }
        // 归一化
        // 逆矩阵
        public Matrix4x4 Inverse()
        {
            float a = Determinate();
            if (a == 0)
            {
                Log.debug("矩阵不可逆");
                Matrix4x4 m = new Matrix4x4();
                m.identity();
                return m;
            }
            Matrix4x4 adj = GetAdjoint();//伴随矩阵
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    adj._m[i, j] = adj._m[i, j] / a;
                }
            }
            return adj;
        }
        // 伴随矩阵
        public Matrix4x4 GetAdjoint()
        {
            int x, y;
            float[,] tempM = new float[3, 3];
            Matrix4x4 result = new Matrix4x4();
            result.setZero();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int t = 0; t < 3; ++t)
                        {
                            x = k >= i ? k + 1 : k;
                            y = t >= j ? t + 1 : t;

                            tempM[k, t] = _m[x, y];
                        }
                    }
                    result._m[i, j] = (float)System.Math.Pow(-1, (1 + j) + (1 + i)) * Determinate(tempM, 3);
                }
            }
            return result.Transpose();
        }
        // 获取/修改旋转信息
        // 获取/修改缩放信息
        // 获取/修改位置信息

        //
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n[Matrix4x4]\n");
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sb.Append(_m[i, j]);
                    sb.Append(", ");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
