using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightMvcCaptcha.Core;

namespace LightMvcCaptcha.Web.ViewModels
{
    public class CaptchaViewModel
    {
        public float FontSize { get; set; } = Core.Captcha.Font.Size;
        public string Chars { get; set; } = Core.Captcha.Chars;
        public uint CaptchaSize { get; set; } = Core.Captcha.CaptchaSize;
        public uint CaptchaDistance { get; set; } = Core.Captcha.CaptchaDistance;
        public uint CaptchaMaxAngle { get; set; } = Core.Captcha.CaptchaMaxAngle;
        public uint WaveDistortionAmplitude { get; set; } = Core.Captcha.WaveDistortionAmplitude;
        public uint WaveDistortionPeriod { get; set; } = Core.Captcha.WaveDistortionPeriod;
        public uint LineNoiseCount { get; set; } = Core.Captcha.LineNoiseCount;
        public bool WaveDistortionEnabled { get; set; } = Core.Captcha.WaveDistortionEnabled;
        public bool LineNoiseEnabled { get; set; } = Core.Captcha.LineNoiseEnabled;

        [Captcha(ErrorMessage = "Wrong captcha")]
        public string Captcha { get; set; }
    }
}