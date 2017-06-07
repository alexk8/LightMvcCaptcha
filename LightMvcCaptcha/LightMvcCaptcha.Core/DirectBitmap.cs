using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LightMvcCaptcha.Core
{
    /// <summary>
    /// This class give access to Bitmap data without any locks. Thx to: https://goo.gl/ZGxaGp
    /// </summary>
    public class DirectBitmap : IDisposable
    {
        /// <summary>
        /// Just a simple Bitmap
        /// </summary>
        public Bitmap Bitmap { get; private set; }
        /// <summary>
        /// Array that represents Bitmap property to get access to point use: Bits[x + y * Width], where (x,y) - some point
        /// Int32 is in PixelFormat.Format32bppPArgb encoded color
        /// </summary>
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        /// <summary>
        /// The Height of Bitmap
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// The Width of Bitmap
        /// </summary>
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}
