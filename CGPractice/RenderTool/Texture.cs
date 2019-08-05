using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGPractice.Math;

namespace CGPractice.RenderTool
{
    class Texture
    {
        public int Width
        {
            get { return bit.Width; }
        }
        public int Height
        {
            get { return bit.Height; }
        }
        public Bitmap bit { get; }

        public Texture(string filename, int width, int height)
        {
            try
            {
                Image img = Image.FromFile(filename);
                bit = new Bitmap(img, width, height);
            }
            catch (Exception)
            {
                bit = new Bitmap(width, height);
                initTexture();
            }
        }

        public System.Drawing.Color ReadTexture(int uIndex, int vIndex)
        {
            int u = MathUtil.Range(uIndex, 0, Width - 1);
            int v = MathUtil.Range(vIndex, 0, Height - 1);
            return bit.GetPixel(u, v);
        }

        public System.Drawing.Color this[int uIndex, int vIndex]
        {
            get
            {
                int u = MathUtil.Range(uIndex, 0, bit.Width - 1);
                int v = MathUtil.Range(vIndex, 0, bit.Height - 1);
                return bit.GetPixel(u, v);
            }
        }

        private void initTexture()
        {
            for (int j = 0; j < bit.Width; j++)
            {
                for (int i = 0; i < bit.Height; i++)
                {
                    bit.SetPixel(j, i, ((j + i) % 32 == 0) ? System.Drawing.Color.White : System.Drawing.Color.Red);
                }
            }
        }
    }
}
