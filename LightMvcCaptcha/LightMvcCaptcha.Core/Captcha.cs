﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace LightMvcCaptcha.Core
{
    public class Captcha
    {
        // EVENTS:

        /// <summary>
        /// This event will be called right AFTER filling captcha with BackgroundColor and BEFORE drawing key
        /// </summary>
        public static event Action<DirectBitmap, Random> BeforeCaptchaDrawEvent;

        /// <summary>
        /// This event will be called right AFTER drawing key and BEFORE line noise or wave distortion algorithms
        /// </summary>
        public static event Action<DirectBitmap, Random> AfterCaptchaDrawEvent;

        /// <summary>
        /// This event will be called right AFTER line noise or wave distortion algorithms
        /// </summary>
        public static event Action<DirectBitmap, Random> CaptchaCreatedEvent;

        // PUBLIC STATIC VARIABLES:

        /// <summary>
        /// Shortcut for generating random color using Random class instance
        /// </summary>
        public static Func<Random, Color> RandomColorFunction { get; } =
            rnd => Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

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
        public static uint Length { get; set; } = 6;

        /// <summary>
        /// The distance between characters in pixels
        /// </summary>
        public static uint CharsSpacing { get; set; } = 45;

        /// <summary>
        /// Maximum rotation angle (-angle, angle) for each character.
        /// </summary>
        public static uint MaxRotationAngle { get; set; } = 45;

        /// <summary>
        /// Enables or disables using Wave distortion algorithm
        /// </summary>
        public static bool WaveDistortionEnabled { get; set; } = true;

        /// <summary>
        /// Wave distortion algorithm amplitude multiplier, in percents
        /// Default is 100%
        /// </summary>
        public static uint WaveDistortionAmplitude { get; set; } = 70;

        /// <summary>
        /// Wave distortion algorithm period multiplier, in percents
        /// Default is 100% 
        /// </summary>
        public static uint WaveDistortionPeriod { get; set; } = 70;

        /// <summary>
        /// Enables or disables using line noise
        /// </summary>
        public static bool LineNoiseEnabled { get; set; } = true;

        /// <summary>
        /// The number of lines that will be drawn
        /// </summary>
        public static uint LineNoiseCount { get; set; } = 10;

        /// <summary>
        /// Enables or disables using ellipse noise
        /// </summary>
        public static bool EllipseNoiseEnabled { get; set; } = true;

        /// <summary>
        /// The number of ellipses that will be drawn
        /// </summary>
        public static uint EllipseNoiseCount { get; set; } = 10;

        /// <summary>
        /// Sets Graphics class in high quality draw mode. 20% slower
        /// </summary>
        public static bool HighQuality { get; set; } = true;

        /// <summary>
        /// The background color of the captcha. If you want to draw smth as background use BeforeCaptchaDrawEvent
        /// </summary>
        public static Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// The function that will be used to get the color of every character
        /// Default function is creating random color
        /// </summary>
        public static Func<Random, Brush> CharColorFunction { get; set; } =
            rnd => new SolidBrush(RandomColorFunction(rnd));

        /// <summary>
        /// The function that will be used to get the color of every line if LineNoiseEnabled is true
        /// Default function is creating random color
        /// </summary>
        public static Func<Random, Pen> LineColorFunction { get; set; } =
            rnd => new Pen(RandomColorFunction(rnd));

        /// <summary>
        /// The function that will be used to get the color of every ellipse if EllipseNoiseEnabled is true
        /// Default function is creating random color
        /// </summary>
        public static Func<Random, Brush> EllipseColorFunction { get; set; } =
            rnd => new SolidBrush(RandomColorFunction(rnd));


        //PUBLIC VARIABLES AND METHODS:

        /// <summary>
        /// Bitmap with captcha image, getting disposed after sending
        /// </summary>
        public byte[] Image { get; private set; }
        /// <summary>
        /// The size of captcha. Depends on Captcha.Font size and Captcha.Generate settings
        /// </summary>
        public Size Size { get; }
        /// <summary>
        /// The string that is illustrated on captcha
        /// </summary>
        public string Key { get; }

        private Captcha(byte[] img, Size size, string key)
        {
            Image = img;
            Size = size;
            Key = key;
        }

        /// <summary>
        /// After sending captcha image to user the remaining class will live in Session, so we can remove unnessesary element
        /// </summary>
        public void DisposeImage()
        {
            Image = null;
        }
        // STATIC METHODS:

        /// <summary>
        /// Generates a CAPTCHA image
        /// </summary>
        /// <returns>Captcha</returns>
        public static Captcha Generate()
        {
            return Generate(Font, Chars, Length, CharsSpacing,
                MaxRotationAngle, WaveDistortionEnabled, WaveDistortionAmplitude,
                WaveDistortionPeriod, LineNoiseEnabled, LineNoiseCount, 
                EllipseNoiseEnabled, EllipseNoiseCount, HighQuality);
        }

        /// <summary>
        /// Generates a CAPTCHA image
        /// </summary>
        /// <param name="font">The font that will be used to generate the captcha</param>
        /// <param name="chars">List of characters that will be used to generate the captcha</param>
        /// <param name="length">The number of characters in the captcha</param>
        /// <param name="charsSpacing">The distance between characters in pixels</param>
        /// <param name="maxRotationAngle">Maximum rotation angle (-angle, angle) for each character</param>
        /// <param name="waveDistortionEnabled">Enables or disables using Wave distortion algorithm</param>
        /// <param name="waveDistortionAmplitude">Wave distortion algorithm amplitude multiplier, in percents</param>
        /// <param name="waveDistortionPeriod">Wave distortion algorithm period multiplier, in percents</param>
        /// <param name="lineNoiseEnabled">Enables or disables using line noise</param>
        /// <param name="lineNoiseCount">Sets the number of lines that will be drawn</param>
        /// <param name="ellipseNoiseEnabled">Enables or disables using ellipse noise</param>
        /// <param name="ellipseNoiseCount">The number of ellipses that will be drawn</param>
        /// <param name="highQuality">Sets Graphics class in high quality draw mode. 20% slower</param>
        /// <returns>Captcha</returns>
        public static Captcha Generate(Font font, string chars, uint length, uint charsSpacing,
            uint maxRotationAngle, bool waveDistortionEnabled, uint waveDistortionAmplitude,
            uint waveDistortionPeriod, bool lineNoiseEnabled, uint lineNoiseCount,
            bool ellipseNoiseEnabled, uint ellipseNoiseCount, bool highQuality)
        {
            string key = GetRandomString(chars, length);
            SizeF wSize = MeasureStringSize(font, "W", highQuality); // as the most wide letter in most fonts

            DirectBitmap captcha = new DirectBitmap((int) ((length-1) * charsSpacing + wSize.Width),
                (int)(wSize.Height*0.85));

            CreateDigits(captcha, key, font, charsSpacing, maxRotationAngle,
                ellipseNoiseEnabled, ellipseNoiseCount, highQuality);

            if (lineNoiseEnabled)
                DrawLineNoise(captcha, lineNoiseCount, highQuality);

            if (waveDistortionEnabled)
                WaveTransform(ref captcha, waveDistortionPeriod, waveDistortionAmplitude);

            CaptchaCreatedEvent?.Invoke(captcha, RandomThreadSafe.Instance);

            var size = new Size(captcha.Width, captcha.Height);

            byte[] captchaBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                captcha.Bitmap.Save(ms, ImageFormat.Jpeg);
                captchaBytes = ms.ToArray();
            }

            captcha.Dispose();

            return new Captcha(captchaBytes, size, key);
        }

        /// <summary>
        /// Creates graphics instance
        /// </summary>
        /// <param name="bitmap">Original bitmap</param>
        /// <param name="highQuality">Activates high quality drawing mode</param>
        /// <returns>Graphics</returns>
        public static Graphics CreateGraphics(Bitmap bitmap, bool highQuality)
        {
            Graphics g = Graphics.FromImage(bitmap);

            if (highQuality)
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }

            return g;
        }

        private static SizeF MeasureStringSize(Font font, string text, bool highQuality)
        {
            using (var image = new Bitmap(1, 1))
            {
                using (var g = CreateGraphics(image, highQuality))
                {
                    return g.MeasureString(text, font);
                }
            }
        }

        private static void WaveTransform(ref DirectBitmap img, uint waveDistortionPeriod, uint waveDistortionAmplitude)
        {
            double F = 0.08 * waveDistortionPeriod / 100d;
            double A = 1.8 * waveDistortionAmplitude / 100d;

            DirectBitmap transImg = new DirectBitmap(img.Width, img.Height);

            int backgroundColor = BackgroundColor.ToArgb();
            for (int i = 0; i < img.Width * img.Height; i++)
                transImg.Bits[i] = backgroundColor;

            for (int x = 0; x < img.Width; x++)
                for (int y = 0; y < img.Height; y++)
                {
                    int _x = Math.Max(Math.Min(Convert.ToInt32(Math.Round(x + (A * Math.Sin(2.0 * Math.PI * y * F)))), img.Width - 1), 0);
                    int _y = y;

                    transImg.Bits[x + y * img.Width] = img.Bits[_x + _y * img.Width];
                }

            img.Dispose();
            img = transImg;
        }

        private static string GetRandomString(string chars, uint size)
        {
            string captcha = "";

            for (int i = 0; i < size; i++)
                captcha += chars[RandomThreadSafe.Instance.Next(0, chars.Length)];

            return captcha;
        }

        private static void CreateDigits(DirectBitmap img, string captcha, Font font, uint charsSpacing, 
            uint maxRotationAngle, bool ellipseNoiseEnabled, uint ellipseNoiseCount, bool highQuality)
        {
            using (Graphics g = CreateGraphics(img.Bitmap, highQuality))
            {
                g.Clear(BackgroundColor);

                BeforeCaptchaDrawEvent?.Invoke(img, RandomThreadSafe.Instance);

                if (ellipseNoiseEnabled)
                    DrawEllipseNoise(img, g, ellipseNoiseCount);

                for (int i = 0; i < captcha.Length; i++)
                {
                    string num = captcha[i].ToString();

                    PointF centerOld = g.MeasureString(num, font).ToPointF();

                    centerOld.X /= 2;
                    centerOld.X += i * charsSpacing;

                    centerOld.Y /= 2;


                    using (Matrix matrix = new Matrix())
                    {
                        matrix.RotateAt(RandomThreadSafe.Instance.Next(-((int)maxRotationAngle), (int)maxRotationAngle + 1), centerOld);

                        g.Transform = matrix;

                        Brush brush = CharColorFunction?.Invoke(RandomThreadSafe.Instance) ?? Brushes.Black;

                        g.DrawString(num, font, brush, i * charsSpacing, 0);

                        brush.Dispose();
                    }
                }
            }

            AfterCaptchaDrawEvent?.Invoke(img, RandomThreadSafe.Instance);
        }

        private static void DrawLineNoise(DirectBitmap img, uint count, bool highQuality)
        {
            using (Graphics g = CreateGraphics(img.Bitmap, highQuality))
            {
                for (int i = 0; i < count; i++)
                {
                    int x1 = RandomThreadSafe.Instance.Next(0, img.Width);
                    int y1 = RandomThreadSafe.Instance.Next(0, img.Height);

                    int x2 = RandomThreadSafe.Instance.Next(0, img.Width);
                    int y2 = RandomThreadSafe.Instance.Next(0, img.Height);

                    Pen pen = LineColorFunction?.Invoke(RandomThreadSafe.Instance) ?? Pens.Red;

                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private static void DrawEllipseNoise(DirectBitmap img, Graphics g, uint count)
        {
            var rnd = RandomThreadSafe.Instance;
            for (int i = 0; i < count; i++)
                g.FillEllipse(EllipseColorFunction(rnd), rnd.Next(img.Width),
                    rnd.Next(img.Height), rnd.Next(img.Width / 5), rnd.Next(img.Width / 5));
        }
    }
}
