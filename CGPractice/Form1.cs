using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using CGPractice.Math;
using CGPractice.RenderTool;
using CGPractice.Shape;
using CGPractice.Util;
using Color = System.Drawing.Color;

namespace CGPractice
{
    public partial class Form1 : Form
    {
        private float Modanglex, Modangley;
        private Graphics g;

        private Bitmap _frameBuff;      //用一张bitmap来做帧缓冲
        private Graphics _frameG;
        private float[,] _zBuff;

        // Objects
        private Cube cube;
        private Camera camera;
        private Light ambientLight;

        // 开关
        private LightMode lightMode = LightMode.Off;
        private RenderMode renderMode = RenderMode.Wireframe;

        // 统计数据
        private uint _viewTrisCount;    //测试数据，记录当前显示的三角形数
        public Form1()
        {
            InitializeComponent();
            Log.debug("ctor form1");
            Modanglex = (float)(System.Math.PI / this.MaximumSize.Width / 100);
            Modangley = (float)(System.Math.PI / this.MaximumSize.Height / 100);

            _frameBuff = new Bitmap(this.MaximumSize.Width, this.MaximumSize.Height);
            _zBuff = new float[this.MaximumSize.Width, this.MaximumSize.Height];
            _frameG = Graphics.FromImage(_frameBuff);

            cube = new Cube("testCube");
            camera = new Camera(new Vector3(0, 0, -3), new Vector3(0, 0, 1), new Vector3(0, 1, 0), (float)(System.Math.PI / 4),
                this.MaximumSize.Width / (float)this.MaximumSize.Height, 1f, 500f);
            ambientLight = new Light(new Vector3(-50, 0, 0), new RenderTool.Color(1, 1, 1));

            System.Timers.Timer mainTimer = new System.Timers.Timer(1000 / 30f);
            mainTimer.Elapsed += new ElapsedEventHandler(Tick);
            mainTimer.AutoReset = true;
            mainTimer.Enabled = true;
            mainTimer.Start();
        }

        private float rot = 0;
        private void Tick(object sender, EventArgs e)
        {
            lock (_frameBuff)
            {
                ClearBuff();
                rot += 0.05f;
                Matrix4x4 m = MathUtil.GetRotation(new Vector3(1, 1, 0), rot) * MathUtil.GetTranlate(0, 0, 0) * MathUtil.GetScale(cube.scale);
                //Matrix4x4 m = MathUtil.GetRotationX(cube.rotation.x) * MathUtil.GetRotationY(cube.rotation.y) * MathUtil.GetTranlate(0, 0, 0) * MathUtil.GetScale(cube.scale);
                Matrix4x4 v = MathUtil.GetView(camera);
                Matrix4x4 p = MathUtil.GetProject(camera);
                Draw(m, v, p);
                // 准备
                if (g == null)
                {
                    g = this.CreateGraphics();
                }
                g.Clear(System.Drawing.Color.Black);
                g.DrawImage(_frameBuff, 0, 0);
            }
        }

        private void ClearBuff()
        {
            _frameG.Clear(System.Drawing.Color.Black);
            Array.Clear(_zBuff, 0, _zBuff.Length);
        }

        private void Draw(Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            _viewTrisCount = 0;
            // 遍历点画
            for (int i = 0; i < cube.mesh.triangles.Length; i++)
            {
                DrawTriangle(cube.mesh.triangles[i], m, v, p);
            }
        }

        /// <summary>
        /// 画三角形
        /// </summary>
        /// <param name="t"></param>
        /// <param name="m"></param>
        /// <param name="v"></param>
        /// <param name="p"></param>
        private void DrawTriangle(Triangle t, Matrix4x4 m, Matrix4x4 v, Matrix4x4 p)
        {
            // -----------------几何阶段---------------------
            // 三个顶点
            Vertex v1 = cube.mesh.vertexies[t[0]];
            Vertex v2 = cube.mesh.vertexies[t[1]];
            Vertex v3 = cube.mesh.vertexies[t[2]];
            // 光照
            if (lightMode == LightMode.On)
            {
                Lighting(m, ref v1, t.normal);
                Lighting(m, ref v2, t.normal);
                Lighting(m, ref v3, t.normal);
            }

            // 空间转换
            Transform_MV(m, v, ref v1);
            Transform_MV(m, v, ref v2);
            Transform_MV(m, v, ref v3);
            //Log.debug("mv", v1.pos.ToString());

            // 齐次坐标
            Transform_P(p, ref v1);
            Transform_P(p, ref v2);
            Transform_P(p, ref v3);
            //Log.debug("pro", v1.pos.ToString());

            // 齐次 到 屏幕
            TransformToScreen(ref v1);
            TransformToScreen(ref v2);
            TransformToScreen(ref v3);

            //Log.debug(v1.pos.ToString());
            // 光栅化
            if (renderMode == RenderMode.Wireframe)
            {
                // 线框
                LineDDA(v1, v2);
                LineDDA(v2, v3);
                LineDDA(v3, v1);
            }
            else
            {
                TrisRasterization(v1, v2, v3);
                //trianglesRasterization(v1, v2, v3);
            }
        }

        private void trianglesRasterization(Vertex v1, Vertex v2, Vertex v3)
        {
            // 进行排序，v1总在最上面，v2总在最中间，v3总在最下面
            if (v1.pos.y > v2.pos.y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            if (v2.pos.y > v3.pos.y)
            {
                var temp = v2;
                v2 = v3;
                v3 = temp;
            }

            if (v1.pos.y > v2.pos.y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            // 反向斜率
            float dP1P2, dP1P3;

            // 计算反向斜率
            if (v2.pos.y - v1.pos.y > 0)
                dP1P2 = (v2.pos.x - v1.pos.x) / (v2.pos.y - v1.pos.y);
            else
                dP1P2 = 0;

            if (v3.pos.y - v1.pos.y > 0)
                dP1P3 = (v3.pos.x - v1.pos.x) / (v3.pos.y - v1.pos.y);
            else
                dP1P3 = 0;

            // 对于第一种情况来说，三角形是这样的：
            // P1
            // -
            // --
            // - -
            // -  -
            // -   - P2
            // -  -
            // - -
            // -
            // P3
            if (dP1P2 > dP1P3)
            {
                for (var y = (int)v1.pos.y; y <= (int)v3.pos.y; y++)
                {
                    if (y < v2.pos.y)
                    {
                        ProcessScanLine(y, v1, v3, v1, v2);
                    }
                    else
                    {
                        ProcessScanLine(y, v1, v3, v2, v3);
                    }
                }
            }
            // 对于第二种情况来说，三角形是这样的：
            //       P1
            //        -
            //       --
            //      - -
            //     -  -
            // P2 -   -
            //     -  -
            //      - -
            //        -
            //       P3
            else
            {
                for (var y = (int)v1.pos.y; y <= (int)v3.pos.y; y++)
                {
                    if (y < v2.pos.y)
                    {
                        ProcessScanLine(y, v1, v2, v1, v3);
                    }
                    else
                    {
                        ProcessScanLine(y, v2, v3, v1, v3);
                    }
                }
            }
        }

        // 在两点之间从左到右绘制一条线段
        // papb -> pcpd
        // pa, pb, pc, pd在之前必须已经排好序
        void ProcessScanLine(int y, Vertex pa, Vertex pb, Vertex pc, Vertex pd)
        {
            // 由当前的y值，我们可以计算出梯度
            // 以此再计算出 起始X(sx) 和 结束X(ex)
            // 如果 pa.pos.y == pb.pos.y 或者 pc.pos.y== pd.pos.y 的话，梯度强制为1
            var gradient1 = pa.pos.y != pb.pos.y ? (y - pa.pos.y) / (pb.pos.y - pa.pos.y) : 1;
            var gradient2 = pc.pos.y != pd.pos.y ? (y - pc.pos.y) / (pd.pos.y - pc.pos.y) : 1;

            int sx = (int)MathUtil.Lerp(pa.pos.x, pb.pos.x, gradient1);
            int ex = (int)MathUtil.Lerp(pc.pos.x, pd.pos.x, gradient2);

            // 从左(sx)向右(ex)绘制一条线
            for (var x = sx; x < ex; x++)
            {
                if (x >= 0 && x <= this.MaximumSize.Width && y >= 0 && y <= this.MaximumSize.Height)
                    _frameBuff.SetPixel(x, y, Color.White);
            }
        }


        private void TrisRasterization(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v1.pos.y == v2.pos.y)
            {
                if (v1.pos.y < v3.pos.y)
                {//平顶
                    DrawTriangleTop(v1, v2, v3);
                }
                else
                {//平底
                    DrawTriangleBottom(v3, v1, v2);
                }
            }
            else if (v1.pos.y == v3.pos.y)
            {
                if (v1.pos.y < v2.pos.y)
                {//平顶
                    DrawTriangleTop(v1, v3, v2);
                }
                else
                {//平底
                    DrawTriangleBottom(v2, v1, v3);
                }
            }
            else if (v2.pos.y == v3.pos.y)
            {
                if (v2.pos.y < v1.pos.y)
                {//平顶
                    DrawTriangleTop(v2, v3, v1);
                }
                else
                {//平底
                    DrawTriangleBottom(v1, v2, v3);
                }
            }
            else
            {//分割三角形
                Vertex top;

                Vertex bottom;
                Vertex middle;
                if (v1.pos.y > v2.pos.y && v2.pos.y > v3.pos.y)
                {
                    top = v3;
                    middle = v2;
                    bottom = v1;
                }
                else if (v3.pos.y > v2.pos.y && v2.pos.y > v1.pos.y)
                {
                    top = v1;
                    middle = v2;
                    bottom = v3;
                }
                else if (v2.pos.y > v1.pos.y && v1.pos.y > v3.pos.y)
                {
                    top = v3;
                    middle = v1;
                    bottom = v2;
                }
                else if (v3.pos.y > v1.pos.y && v1.pos.y > v2.pos.y)
                {
                    top = v2;
                    middle = v1;
                    bottom = v3;
                }
                else if (v1.pos.y > v3.pos.y && v3.pos.y > v2.pos.y)
                {
                    top = v2;
                    middle = v3;
                    bottom = v1;
                }
                else if (v2.pos.y > v3.pos.y && v3.pos.y > v1.pos.y)
                {
                    top = v1;
                    middle = v3;
                    bottom = v2;
                }
                else
                {
                    //三点共线
                    return;
                }
                //插值求中间点x
                float middlex = (middle.pos.y - top.pos.y) * (bottom.pos.x - top.pos.x) / (bottom.pos.y - top.pos.y) + top.pos.x;
                float dy = middle.pos.y - top.pos.y;
                float t = dy / (bottom.pos.y - top.pos.y);
                //插值生成左右顶点
                Vertex newMiddle = new Vertex();
                newMiddle.pos.x = middlex;
                newMiddle.pos.y = middle.pos.y;
                MathUtil.ScreenSpaceLerpVertex(ref newMiddle, top, bottom, t);

                //平底
                DrawTriangleBottom(top, newMiddle, middle);
                //平顶
                DrawTriangleTop(newMiddle, middle, bottom);
            }
        }

        private void DrawTriangleTop(Vertex p1, Vertex p2, Vertex p3)
        {
            for (float y = p1.pos.y; y <= p3.pos.y; y += 0.5f)
            {
                int yIndex = (int)(System.Math.Round(y, MidpointRounding.AwayFromZero));
                if (yIndex >= 0 && yIndex < this.MaximumSize.Height)
                {
                    float xl = (y - p1.pos.y) * (p3.pos.x - p1.pos.x) / (p3.pos.y - p1.pos.y) + p1.pos.x;
                    float xr = (y - p2.pos.y) * (p3.pos.x - p2.pos.x) / (p3.pos.y - p2.pos.y) + p2.pos.x;

                    float dy = y - p1.pos.y;
                    float t = dy / (p3.pos.y - p1.pos.y);
                    //插值生成左右顶点
                    Vertex new1 = new Vertex();
                    new1.pos.x = xl;
                    new1.pos.y = y;
                    MathUtil.ScreenSpaceLerpVertex(ref new1, p1, p3, t);
                    //
                    Vertex new2 = new Vertex();
                    new2.pos.x = xr;
                    new2.pos.y = y;
                    MathUtil.ScreenSpaceLerpVertex(ref new2, p2, p3, t);
                    //扫描线填充
                    if (new1.pos.x < new2.pos.x)
                    {
                        ScanlineFill(new1, new2, yIndex);
                        //LineDDA(new1, new2);
                    }
                    else
                    {
                        ScanlineFill(new2, new1, yIndex);
                        //LineDDA(new2, new1);
                    }
                    //Log.debug("top", yIndex.ToString());
                }
            }
        }

        private void DrawTriangleBottom(Vertex p1, Vertex p2, Vertex p3)
        {
            for (float y = p1.pos.y; y <= p2.pos.y; y += 0.5f)
            {
                int yIndex = (int)(System.Math.Round(y, MidpointRounding.AwayFromZero));
                if (yIndex >= 0 && yIndex < this.MaximumSize.Height)
                {
                    float xl = (y - p1.pos.y) * (p2.pos.x - p1.pos.x) / (p2.pos.y - p1.pos.y) + p1.pos.x;
                    float xr = (y - p1.pos.y) * (p3.pos.x - p1.pos.x) / (p3.pos.y - p1.pos.y) + p1.pos.x;

                    float dy = y - p1.pos.y;
                    float t = dy / (p2.pos.y - p1.pos.y);
                    //插值生成左右顶点
                    Vertex new1 = new Vertex();
                    new1.pos.x = xl;
                    new1.pos.y = y;
                    MathUtil.ScreenSpaceLerpVertex(ref new1, p1, p2, t);
                    //
                    Vertex new2 = new Vertex();
                    new2.pos.x = xr;
                    new2.pos.y = y;
                    MathUtil.ScreenSpaceLerpVertex(ref new2, p1, p3, t);
                    //扫描线填充
                    if (new1.pos.x < new2.pos.x)
                    {
                        ScanlineFill(new1, new2, yIndex);
                        //LineDDA(new1, new2);
                    }
                    else
                    {
                        ScanlineFill(new2, new1, yIndex);
                        //LineDDA(new2, new1);
                    }
                    //Log.debug("bottom", yIndex.ToString());
                }
            }
        }

        /// <summary>
        /// 扫描线填充
        /// </summary>
        /// <param name="left">左端点，值已经经过插值</param>
        /// <param name="right">右端点，值已经经过插值</param>
        private void ScanlineFill(Vertex left, Vertex right, int yIndex)
        {
            float dx = right.pos.x - left.pos.x;
            for (float x = left.pos.x; x <= right.pos.x; x += 0.5f)
            {
                int xIndex = (int)(x + 0.5f);
                if (xIndex >= 0 && xIndex < this.MaximumSize.Width)
                {
                    float lerpFactor = 0;
                    if (dx != 0)
                    {
                        lerpFactor = (x - left.pos.x) / dx;
                    }
                    // 1/z’与x’和y'是线性关系的
                    float onePreZ = MathUtil.Lerp(left.onePerz, right.onePerz, lerpFactor);
                    if (onePreZ >= _zBuff[yIndex, xIndex])//使用1/z进行深度测试
                    {//通过测试
                        float w = 1 / onePreZ;
                        _zBuff[yIndex, xIndex] = onePreZ;

                        //插值顶点颜色
                        RenderTool.Color vertColor = MathUtil.Lerp(left.color, right.color, lerpFactor);
                        //插值光照颜色
                        RenderTool.Color lightColor = MathUtil.Lerp(left.lightColor, right.lightColor, lerpFactor); ;


                        if (lightMode == LightMode.On)
                        {//光照模式，需要混合光照的颜色
                            if (RenderMode.Texture == renderMode)
                            {
                                //RenderTool.Color finalColor = texColor * lightColor;
                                //_frameBuff.SetPixel(xIndex, yIndex, finalColor.TransFormToSystemColor());
                            }
                            else if (RenderMode.VertexColor == renderMode)
                            {
                                RenderTool.Color finalColor = vertColor * lightColor;
                                _frameBuff.SetPixel(xIndex, yIndex, finalColor.TranstoSysColor());
                            }
                        }
                        else
                        {
                            if (RenderMode.Texture == renderMode)
                            {
                                //_frameBuff.SetPixel(xIndex, yIndex, texColor.TransFormToSystemColor());
                            }
                            else if (RenderMode.VertexColor == renderMode)
                            {
                                _frameBuff.SetPixel(xIndex, yIndex, vertColor.TranstoSysColor());
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 顶点光照处理
        /// </summary>
        /// <param name="m">世界坐标转换矩阵</param>
        /// <param name="v">顶点</param>
        /// <param name="tNormal"></param>
        private void Lighting(Matrix4x4 m, ref Vertex v, Vector3 tNormal)
        {
            // 顶点在世界位置
            Vector4 lPosWorld = new Vector4(v.pos, 1) * m;
            Vector4 normal = new Vector4(tNormal, 0) * m.Inverse().Transpose();//模型空间法线乘以世界矩阵的逆转置得到世界空间法线
            normal = normal.Normaized();

            // 环境光
            RenderTool.Color ambient = ambientLight.lightColor * 0.5f;

            // 高光
            // 光照方向
            Vector4 lightDir = lPosWorld - new Vector4(ambientLight.pos, 1);
            // 看
            Vector4 viewDir = new Vector4(camera.eye - v.pos, 0).Normaized();
            // 反射
            Vector4 reflectDir = Vector4.Dot(lightDir, normal) * normal;
            double spe = System.Math.Pow(MathUtil.Range(Vector4.Dot(viewDir, reflectDir), 0, 1), 32);
            RenderTool.Color spectorColor = (float)spe * ambientLight.lightColor * 1.0f;

            v.lightColor = ambient + spectorColor;
        }

        // DDA生成直线段
        private void LineDDA(Vertex start, Vertex end)
        {
            float x, y;
            float dx, dy, m;
            dx = end.pos.x - start.pos.x;
            dy = end.pos.y - start.pos.y;

            float lerpFactor = 0;

            RenderTool.Color vcolor, vlcolor;
            if (dx != 0)
            {
                m = dy / dx;
                if (m >= -1 && m <= 1)
                {
                    y = start.pos.y;
                    for (x = start.pos.x; x <= end.pos.x; x++)
                    {
                        lerpFactor = (x - start.pos.x) / dx;
                        vcolor = MathUtil.Lerp(start.color, end.color, lerpFactor);
                        vlcolor = MathUtil.Lerp(start.lightColor, end.lightColor, lerpFactor);

                        //Log.debug(lerpFactor.ToString(), tmp.ToString());
                        _frameBuff.SetPixel((int)x, (int)(y + 0.5f), (vcolor * vlcolor).TranstoSysColor());
                        y += m;
                    }
                }
                else
                {
                    m = 1 / m;
                    x = start.pos.x;
                    for (y = start.pos.y; y <= end.pos.y; y++)
                    {
                        lerpFactor = (y - start.pos.y) / dy;
                        vcolor = MathUtil.Lerp(start.color, end.color, lerpFactor);
                        vlcolor = MathUtil.Lerp(start.lightColor, end.lightColor, lerpFactor);
                        _frameBuff.SetPixel((int)x, (int)y, (vcolor * vlcolor).TranstoSysColor());
                        x += m;
                    }
                }
            }
            else
            {
                // Y轴旋转
                x = start.pos.x;
                y = start.pos.y < end.pos.y ? start.pos.y : end.pos.y;
                int d = (int)System.Math.Abs(dy);
                while (d >= 0)
                {
                    lerpFactor = y / dy;
                    vcolor = MathUtil.Lerp(start.color, end.color, lerpFactor);
                    vlcolor = MathUtil.Lerp(start.lightColor, end.lightColor, lerpFactor);
                    _frameBuff.SetPixel((int)x, (int)y, (vcolor * vlcolor).TranstoSysColor());
                    y++;
                    d--;
                }
            }
        }

        private void Transform_MV(Matrix4x4 m, Matrix4x4 v, ref Vertex vertex)
        {
            Vector4 tmp = (new Vector4(vertex.pos, 1)) * m * v;
            //Log.debug("mv", tmp.ToString());
            if (tmp.w != 0)
                vertex.pos = new Vector3(tmp.x / tmp.w, tmp.y / tmp.w, tmp.z / tmp.w);
            else
                vertex.pos = new Vector3(tmp.x, tmp.y, tmp.z);
        }

        private void Transform_P(Matrix4x4 p, ref Vertex v)
        {
            Vector4 tmp = new Vector4(v.pos, 1) * p;
            v.pos = new Vector3(tmp.x / tmp.w, tmp.y / tmp.w, tmp.z / tmp.w);
            v.onePerz = 1 / tmp.w;
            //v.color *= v.onePerz;
            //v.lightColor *= v.onePerz;
        }
        /// <summary>
        /// 从齐次剪裁坐标系转到屏幕坐标
        /// </summary>
        private void TransformToScreen(ref Vertex v)
        {
            v.pos.x = (v.pos.x + 1) * 0.5f * this.MaximumSize.Width;
            v.pos.y = (1 - v.pos.y) * 0.5f * this.MaximumSize.Height;
        }

        private bool drag = false;
        private Point mouseOff;
        private Point mouseSet;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag = true;
                mouseOff = new Point(-e.X, -e.Y);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);
                cube.rotation.y += -mouseSet.X * Modanglex;
                cube.rotation.x += -mouseSet.Y * Modangley;
                //Log.debug(mouseSet.ToString(), cube.rotation.ToString());
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                drag = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                cube.scale = cube.scale + 1;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if ((cube.scale - 1).Magnitude > 0)
                    cube.scale = cube.scale - 1;
            }
            else if (e.KeyCode == Keys.F1)
            {
                lightMode = lightMode == LightMode.On ? LightMode.Off : LightMode.On;
            }
            else if (e.KeyCode == Keys.F2)
            {
                renderMode = RenderMode.Wireframe;
            }
            else if (e.KeyCode == Keys.F3)
            {
                renderMode = RenderMode.VertexColor;
            }
            else if (e.KeyCode == Keys.F4)
            {
                renderMode = RenderMode.Texture;
            }
        }
    }
}
