using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightMvcCaptcha.Core
{
    public class Captcha
    {
        public static Font Font { get; set; } = new Font("Lucida Console", 60);
        public static string Chars { get; set; } = "WERTUPASDFGHKLZXCVBNM123456789";

        private static readonly Random random = new Random();


        public Bitmap Image { get; }
        public Size Size { get; }
        public string Key { get; }

        private Captcha(Bitmap img, Size size, string key)
        {
            Image = img;
            Size = size;
            Key = key;
        }

        /// <summary>
        /// Generates a CAPTCHA image
        /// </summary>
        /// <param name="charsCount">A number of characters presented in captcha</param>
        /// <param name="frequency">The distance betweet characters in pixels</param>
        /// <param name="maxRotate">Sets maximum (-angle, angle) for character rotation. </param>
        /// <returns>Captcha</returns>
        public static Captcha Generate(int charsCount = 6, int frequency = 55, int maxRotate = 50)
        {
            string key = GetRandomString(charsCount);

            Bitmap bmp = CreateDigits(key, frequency, maxRotate);

            DrawNoise(bmp, 10);
            bmp = WaveTransform(bmp);


            var size = new Size(bmp.Width, bmp.Height);
            var ms = new MemoryStream();

            return new Captcha(bmp, size, key);
        }

        private static Bitmap WaveTransform(Bitmap img)
        {
            const double F = 0.08;
            const double A = 1.8;

            Bitmap transImg = new Bitmap(img);

            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                {
                    double _x = x + (A * Math.Sin(2.0 * Math.PI * y * F));
                    double _y = y;

                    transImg.SetPixel(Math.Max(Math.Min(Convert.ToInt32(_x), img.Width - 1), 0), Convert.ToInt32(_y), img.GetPixel(x, y));
                }

            img.Dispose();

            return transImg;
        }

        private static string GetRandomString(int size)
        {
            string chars = "";

            lock (random)
            {
                for (int i = 0; i < size; i++)
                    chars += Chars[random.Next(0, Chars.Length)];
            }

            return chars;
        }

        private static Bitmap CreateDigits(string captcha, int frequency = 40, int maxRotate = 60)
        {
            Bitmap img = new Bitmap((int)(captcha.Length * frequency * 1.15f), (int)(Font.Size * 1.2f));

            DrawNoise(img, 10);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.White);
                for (int i = 0; i < captcha.Length; i++)
                {
                    string num = captcha[i].ToString();

                    PointF centerOld = g.MeasureString(num, Font).ToPointF();

                    centerOld.X /= 2;
                    centerOld.X += i * frequency;

                    centerOld.Y /= 2;


                    using (Matrix matrix = new Matrix())
                    {
                        matrix.RotateAt(random.Next(-maxRotate, maxRotate), centerOld);

                        g.Transform = matrix;

                        g.DrawString(num, Font, Brushes.Black, i * frequency, 0);
                    }
                }
            }

            return img;
        }

        private static void DrawNoise(Bitmap img, int count)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < count; i++)
                {
                    int x1 = random.Next(0, img.Width);
                    int y1 = random.Next(0, img.Height);

                    int x2 = random.Next(0, img.Width);
                    int y2 = random.Next(0, img.Height);

                    g.DrawLine(Pens.Red, x1, y1, x2, y2);
                }
            }
        }
    }
}
