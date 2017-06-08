using System.ComponentModel;
using LightMvcCaptcha.Core;

namespace LightMvcCaptcha.Web.ViewModels
{
    public class CaptchaViewModel
    {
        [DisplayName("FontFamily - The font that will be used to generate the captcha")]
        public string FontFamily { get; set; } = Core.Captcha.Font.FontFamily.Name;
        [DisplayName("FontSize - Font Size")]
        public float FontSize { get; set; } = Core.Captcha.Font.Size;
        [DisplayName("Chars - List of characters that will be used to generate the captcha")]
        public string Chars { get; set; } = Core.Captcha.Chars;
        [DisplayName("Length - The number of characters in the captcha")]
        public uint Length { get; set; } = Core.Captcha.Length;
        [DisplayName("CharsSpacing - The distance between characters in pixels")]
        public uint CharsSpacing { get; set; } = Core.Captcha.CharsSpacing;
        [DisplayName("MaxRotationAngle - Maximum rotation angle (-angle, angle) for each character")]
        public uint MaxRotationAngle { get; set; } = Core.Captcha.MaxRotationAngle;
        [DisplayName("WaveDistortionAmplitude - Wave distortion algorithm amplitude multiplier, in percents")]
        public uint WaveDistortionAmplitude { get; set; } = Core.Captcha.WaveDistortionAmplitude;
        [DisplayName("WaveDistortionPeriod - Wave distortion algorithm period multiplier, in percents")]
        public uint WaveDistortionPeriod { get; set; } = Core.Captcha.WaveDistortionPeriod;
        [DisplayName("LineNoiseCount - Sets the number of lines that will be drawn")]
        public uint LineNoiseCount { get; set; } = Core.Captcha.LineNoiseCount;
        [DisplayName("EllipseNoiseCount - The number of ellipses that will be drawn")]
        public uint EllipseNoiseCount { get; set; } = Core.Captcha.EllipseNoiseCount;

        [DisplayName("WaveDistortionEnabled - Enables or disables using Wave distortion algorithm")]
        public bool WaveDistortionEnabled { get; set; } = Core.Captcha.WaveDistortionEnabled;
        [DisplayName("LineNoiseEnabled - Enables or disables using line noise")]
        public bool LineNoiseEnabled { get; set; } = Core.Captcha.LineNoiseEnabled;
        [DisplayName("EllipseNoiseEnabled - Enables or disables using ellipse noise")]
        public bool EllipseNoiseEnabled { get; set; } = Core.Captcha.EllipseNoiseEnabled;
        [DisplayName("HighQuality - Sets Graphics class in high quality draw mode. 20% slower")]
        public bool HighQuality { get; set; } = Core.Captcha.HighQuality;


        [Captcha(ErrorMessage = "Wrong captcha")]
        public string Captcha { get; set; }
    }
}