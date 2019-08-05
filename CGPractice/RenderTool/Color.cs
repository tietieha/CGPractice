using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;
using CGPractice.Util;

namespace CGPractice.RenderTool
{
    public struct Color
    {
        private float _r;
        private float _g;
        private float _b;

        // ctor
        public Color(float r, float g, float b)
        {
            this._r = MathUtil.Range(r, 0, 1);
            this._g = MathUtil.Range(g, 0, 1);
            this._b = MathUtil.Range(b, 0, 1);
        }

        public Color(System.Drawing.Color color)
        {
            this._r = MathUtil.Range(color.R / 255, 0, 1);
            this._g = MathUtil.Range(color.G / 255, 0, 1);
            this._b = MathUtil.Range(color.B / 255, 0, 1);
        }

        // get/set
        public float r
        {
            get { return MathUtil.Range(_r, 0, 1); }
            set { _r = MathUtil.Range(value, 0, 1); }
        }
        public float g
        {
            get { return MathUtil.Range(_g, 0, 1); }
            set { _g = MathUtil.Range(value, 0, 1); }
        }
        public float b
        {
            get { return MathUtil.Range(_b, 0, 1); }
            set { _b = MathUtil.Range(value, 0, 1); }
        }

        public System.Drawing.Color TranstoSysColor()
        {
            float rr = this.r * 255;
            float gg = this.g * 255;
            float bb = this.b * 255;
            //Log.debug(rr.ToString(), gg.ToString(), bb.ToString());
            return System.Drawing.Color.FromArgb((int) rr, (int) gg, (int) bb);
        }

        // operator
        public static Color operator *(Color a, Color b)
        {
            Color c = new Color();
            c.r = a.r * b.r;
            c.g = a.g * b.g;
            c.b = a.b * b.b;
            return c;
        }

        public static Color operator *(float a, Color b)
        {
            Color c = new Color();
            c.r = a * b.r;
            c.g = a * b.g;
            c.b = a * b.b;
            return c;
        }
        public static Color operator *(Color a, float b)
        {
            Color c = new Color();
            c.r = a.r * b;
            c.g = a.g * b;
            c.b = a.b * b;
            return c;
        }

        public static Color operator +(Color a, Color b)
        {
            Color c = new Color();
            c.r = a.r + b.r;
            c.g = a.g + b.g;
            c.b = a.b + b.b;
            return c;
        }

        public static Color operator -(Color a, Color b)
        {
            Color c = new Color();
            c.r = a.r - b.r;
            c.g = a.g - b.g;
            c.b = a.b - b.b;
            return c;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\n[Color]");
            sb.Append(r);
            sb.Append(", ");
            sb.Append(g);
            sb.Append(", ");
            sb.Append(b);
            return sb.ToString();
        }
    }
}
