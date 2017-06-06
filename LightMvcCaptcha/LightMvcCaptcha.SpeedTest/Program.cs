using LightMvcCaptcha.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightMvcCaptcha.SpeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int count = 1000;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < count; i++)
            {
                var captcha = Captcha.Generate();
                captcha.DisposeImage();
                Console.Title = i.ToString();
            }
            sw.Stop();

            float speed = (float) sw.ElapsedMilliseconds / count;
            float persecond = 1000f / speed;

            Console.WriteLine($"Total speed: {speed} ms., or {persecond} per second");
        }
    }
}
