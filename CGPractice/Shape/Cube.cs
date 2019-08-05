
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;
using CGPractice.RenderTool;
using CGPractice.Util;
using Color = CGPractice.RenderTool.Color;

namespace CGPractice.Shape
{
    class Cube
    {
        public Mesh mesh;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;

        // 顶点
        public static Vector3[] positions =
        {
            new Vector3(-1, 1, -1),
            new Vector3(-1, -1, -1),
            new Vector3(1, -1, -1),
            new Vector3(1, 1, -1),

            new Vector3(-1, 1, 1),
            new Vector3(-1, -1, 1),
            new Vector3(1, -1, 1),
            new Vector3(1, 1, 1),
        };

        public static Color[] colors =
        {
            new Color(0, 1, 0),
            new Color(0, 0, 0),
            new Color(1, 0, 0),
            new Color(1, 1, 0),

            new Color(0, 1, 1),
            new Color(0, 0, 1),
            new Color(1, 0, 1),
            new Color(1, 1, 1),
        };

        //三角形顶点索引 12个面
        public static int[] faces =
        {
            0, 1, 2,
            0, 2, 3,
            //
            7, 6, 5,
            7, 5, 4,
            //
            0, 4, 5,
            0, 5, 1,
            //
            1, 5, 6,
            1, 6, 2,
            //
            2, 6, 7,
            2, 7, 3,
            //
            3, 7, 4,
            3, 4, 0
        };
        //法线 - 对应三角形
        public static Vector3[] normals =
        {
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1),
            //
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            //
            new Vector3(-1, 0, 0),
            new Vector3(-1, 0, 0),
            //
            new Vector3(0, -1, 0),
            new Vector3(0, -1, 0),
            //
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 0),
            //
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
        };
        //uv坐标
        public static Point2D[] uvs =
        {
            new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1),
            new Point2D(0, 0), new Point2D(1, 1), new Point2D(1, 0),
            //
            new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1),
            new Point2D(0, 0), new Point2D(1, 1), new Point2D(1, 0),
            //
            new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1),
            new Point2D(0, 0), new Point2D(1, 1), new Point2D(1, 0),
            //
            new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1),
            new Point2D(0, 0), new Point2D(1, 1), new Point2D(1, 0),
            //
            new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1),
            new Point2D(0, 0), new Point2D(1, 1), new Point2D(1, 0),
            ///
            new Point2D(0, 0), new Point2D(0, 1), new Point2D(1, 1),
            new Point2D(0, 0), new Point2D(1, 1), new Point2D(1, 0)
        };

        // 材质
        public static Material material = new Material(new Color(0, 0, 0.1f), new Color(0.3f, 0.3f, 0.3f), new Color(1, 1, 1), 99);

        // ctor
        public Cube(string name)
        {
            mesh = new Mesh(name, positions, faces, colors, normals, uvs, material);
            position = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
        }
    }
}
