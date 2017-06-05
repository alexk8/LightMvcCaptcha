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
        // EVENTS:

        /// <summary>
        /// This event will be called right AFTER filling captcha with BackgroundColor and BEFORE drawing key
        /// </summary>
        public static event Action<Graphics> BeforeCaptchaDraw;

        /// <summary>
        /// This event will be called right AFTER drawing key and BEFORE line noise or wave distortion algorithms
        /// </summary>
        public static event Action<Graphics> AfterCaptchaDraw;

        /// <summary>
        /// This event will be called right AFTER line noise or wave distortion algorithms
        /// </summary>
        public static event Action<Graphics> CaptchaCreated;

        // PUBLIC STATIC VARIABLES:

        /// <summary>
        /// The font that will be used to generate the captcha
        /// </summary>
        public static Font Font { get; set; } = new Font("Lucida Console", 60);

        /// <summary>
        /// List of characters that will be used to generate the captcha
        /// </summary>
        public static string Chars { get; set; } = "WERTUPASDFGHKLZXCVBNM123456789";

        /// <summary>
        /// Key comparison method
        /// </summary>
        public static StringComparison Comparison { get; set; } = StringComparison.CurrentCultureIgnoreCase;

        /// <summary>
        /// The number of characters in the captcha
        /// </summary>
        public static uint CaptchaSize { get; set; } = 6;

        /// <summary>
        /// The distance between characters in pixels
        /// </summary>
        public static uint CaptchaDistance { get; set; } = (uint)(Font.Size*0.8);

        /// <summary>
        /// Maximum rotation angle (-angle, angle) for each character.
        /// </summary>
        public static uint CaptchaMaxAngle { get; set; } = 60;

        /// <summary>
        /// Enables or disables using Wave distortion algorithm
        /// </summary>
        public static bool WaveDistortionEnabled { get; set; } = true;

        /// <summary>
        /// Wave distortion algorithm amplitude multiplier, in percent
        /// Default is 100%
        /// </summary>
        public static uint WaveDistortionAmplitude { get; set; } = 100;

        /// <summary>
        /// Wave distortion algorithm period multiplier, in percent
        /// Default is 100% 
        /// </summary>
        public static uint WaveDistortionPeriod { get; set; } = 100;

        /// <summary>
        /// Enables or disables using line noise
        /// </summary>
        public static bool LineNoiseEnabled { get; set; } = true;

        /// <summary>
        /// Sets the number of lines that will be drawn
        /// </summary>
        public static uint LineNoiseCount { get; set; } = 10;

        //PUBLIC VARIABLES AND METHODS:

        /// <summary>
        /// Bitmap with captcha image, getting disposed after sending
        /// </summary>
        public Bitmap Image { get; }
        /// <summary>
        /// The size of captcha. Depends on Captcha.Font size and Captcha.Generate settings
        /// </summary>
        public Size Size { get; }
        /// <summary>
        /// The string that is illustrated on captcha
        /// </summary>
        public string Key { get; }

        private static readonly Random random = new Random();

        private Captcha(Bitmap img, Size size, string key)
        {
            Image = img;
            Size = size;
            Key = key;
        }

        // STATIC METHODS:

        /// <summary>
        /// Generates a CAPTCHA image
        /// </summary>
        /// <returns>Captcha</returns>
        public static Captcha Generate()
        {
            string key = GetRandomString(CaptchaSize);

            Bitmap captcha = new Bitmap((int)(key.Length * CaptchaDistance * 1.15f), (int)(Font.Size * 1.2f));

            CreateDigits(captcha, key);

            if (LineNoiseEnabled)
                DrawNoise(captcha, LineNoiseCount);

            if (WaveDistortionEnabled)
                WaveTransform(ref captcha);

            using(var g = Graphics.FromImage(captcha))
                CaptchaCreated?.Invoke(g);

            var size = new Size(captcha.Width, captcha.Height);

            return new Captcha(captcha, size, key);
        }

        private static void WaveTransform(ref Bitmap img)
        {
            double F = 0.08 * WaveDistortionPeriod / 100d;
            double A = 1.8 * WaveDistortionAmplitude / 100d;

            Bitmap transImg = new Bitmap(img);

            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                {
                    double _x = x + (A * Math.Sin(2.0 * Math.PI * y * F));
                    double _y = y;

                    transImg.SetPixel(Math.Max(Math.Min(Convert.ToInt32(_x), img.Width - 1), 0), Convert.ToInt32(_y), img.GetPixel(x, y));
                }

            img.Dispose();
            img = transImg;
        }

        private static string GetRandomString(uint size)
        {
            string chars = "";

            lock (random)
            {
                for (int i = 0; i < size; i++)
                    chars += Chars[random.Next(0, Chars.Length)];
            }

            return chars;
        }

        private static void CreateDigits(Bitmap img, string captcha)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.White);

                BeforeCaptchaDraw?.Invoke(g);

                for (int i = 0; i < captcha.Length; i++)
                {
                    string num = captcha[i].ToString();

                    PointF centerOld = g.MeasureString(num, Font).ToPointF();

                    centerOld.X /= 2;
                    centerOld.X += i * CaptchaDistance;

                    centerOld.Y /= 2;


                    using (Matrix matrix = new Matrix())
                    {
                        matrix.RotateAt(random.Next(-((int)CaptchaMaxAngle), (int)CaptchaMaxAngle + 1), centerOld);

                        g.Transform = matrix;

                        g.DrawString(num, Font, Brushes.Black, i * CaptchaDistance, 0);
                    }
                }

                AfterCaptchaDraw?.Invoke(g);
            }
        }

        private static void DrawNoise(Bitmap img, uint count)
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
