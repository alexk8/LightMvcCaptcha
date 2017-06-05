using LightMvcCaptcha.Core;

namespace LightMvcCaptcha.Web.ViewModels
{
    public class CaptchaViewModel
    {
        public string FontFamily { get; set; } = Core.Captcha.Font.FontFamily.Name;
        public float FontSize { get; set; } = Core.Captcha.Font.Size;
        public string Chars { get; set; } = Core.Captcha.Chars;
        public uint Length { get; set; } = Core.Captcha.Length;
        public uint CharsSpacing { get; set; } = Core.Captcha.CharsSpacing;
        public uint MaxRotationAngle { get; set; } = Core.Captcha.MaxRotationAngle;
        public uint WaveDistortionAmplitude { get; set; } = Core.Captcha.WaveDistortionAmplitude;
        public uint WaveDistortionPeriod { get; set; } = Core.Captcha.WaveDistortionPeriod;
        public uint LineNoiseCount { get; set; } = Core.Captcha.LineNoiseCount;
        public bool WaveDistortionEnabled { get; set; } = Core.Captcha.WaveDistortionEnabled;
        public bool LineNoiseEnabled { get; set; } = Core.Captcha.LineNoiseEnabled;

        [Captcha(ErrorMessage = "Wrong captcha")]
        public string Captcha { get; set; }
    }
}