using LightMvcCaptcha.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LightMvcCaptcha.SpeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const int count = 1000;
            Stopwatch sw = new Stopwatch();
            long done=0;
            Action generatorJob = () =>
            {
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        var captcha = Captcha.Generate();
                        captcha.DisposeImage();
                        Interlocked.Increment(ref done);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            };
            const int threads = 4;
            sw.Start();
            var tasks = Enumerable.Range(1, threads).Select(i => Task.Factory.StartNew(generatorJob)).ToArray();
            while (tasks.Any(t => !t.IsCompleted))
            {
                Thread.Sleep(500);
                Console.Title = $"done {done} of {count*threads}";
            }
            sw.Stop();


            double speed = (double) sw.ElapsedMilliseconds / (count*threads);
            double persecond = Math.Round(1000.0 / speed,0);

            Console.WriteLine($"Total speed: {speed:#.##}ms., or {persecond} per second in {threads} threads");
            Console.ReadKey();
        }
    }
}
