# LightMvcCaptcha
Fully customizable highspeed captcha for asp.net mvc<br>
Captcha image generation speed is about 215 captchas per second or ~4.5 milliseconds per single one. *Tested on 2.5 Ghz cpu, single thread mode*<br>
Here you can test it: [captcha.cygwin255.ml](http://captcha.cygwin255.ml) and deside do you need it or not :)

## How to use?
You need to perform 5 simple steps to embed LightMvcCaptcha to your project
1. Add a nuget package *OR compile it and add dll to your project by yourself*:<br>
**`Install-Package LightMvcCaptcha.Core`**<br>
[https://www.nuget.org/packages/LightMvcCaptcha.Core](https://www.nuget.org/packages/LightMvcCaptcha.Core)
<br><br>
2. Inherit any Controller from `CaptchaController` abstract class:<br>
`public class MyController : CaptchaController`
<br><br>
3. Set attribute `[Captcha]` for any *string* type property in your viewmodel, for e.g. "YourProperty":<br>
`[Captcha(ErrorMessage = "Wrong captcha")]`<br>
`public string YourProperty { get; set; }`
<br><br>
4. In the view create a captcha image by using extension `CaptchaFor`, or use overloaded version to specify html tags that will be applied to result **img**: <br>
`@Html.CaptchaFor(x => x.YourProperty)`<br>
`@Html.CaptchaFor(x => x.YourProperty, new { class = "fancy-image" })`
<br><br>
5. Then use `TextBoxFor` to be able to answer a captcha. **Note**: add `ValidationMessageFor` to see when entered captcha is wrong:<br>
`@Html.TextBoxFor(x => x.YourProperty)`<br>
`@Html.ValidationMessageFor(x => x.YourProperty)`

## I want to change some settings
You can either:
1. Compile LightMvcCaptcha.Web project from this repository and play with variables in easy way
2. Or you can use my website page to do the same without any compiling stuff : [captcha.cygwin255.ml](http://captcha.cygwin255.ml) <br>
But special customization like events and functions explained in the next chapter:

# Customizable?
Here is full list of static setting in **LightMvcCaptcha.Core.Captcha** class:
1. `Font`: The font that will be used to generate the captcha
1. `Chars`: List of characters that will be used to generate the captcha
1. `Length`: The number of characters in the captcha
1. `CharsSpacing`: The distance between characters in pixels
1. `MaxRotationAngle`: Maximum rotation angle (-angle, angle) for each character.
1. `WaveDistortionEnabled`: Enables or disables using Wave distortion algorithm
1. `WaveDistortionAmplitude`: Wave distortion algorithm amplitude multiplier, in percents
1. `WaveDistortionPeriod`: Wave distortion algorithm period multiplier, in percents
1. `LineNoiseEnabled`: Enables or disables using line noise
1. `LineNoiseCount`: Sets the number of lines that will be drawn
1. `EllipseNoiseEnabled`: Enables or disables using ellipse noise
1. `EllipseNoiseCount`: The number of ellipses that will be drawn
1. `BackgroundColor`: The background color of the captcha.

If you want to change them just replace default values inside your **Global.asax -> Application_Start**<br>
## Lets focus on events:
There are 3 events (also static):
* `BeforeCaptchaDrawEvent`: This event will be called right AFTER filling captcha with BackgroundColor and BEFORE drawing key<br>
`public static event Action<Graphics, Random, Size> BeforeCaptchaDrawEvent;`
<br><br>
* `AfterCaptchaDrawEvent`: This event will be called right AFTER drawing key and BEFORE line noise or wave distortion algorithms<br>
`public static event Action<Graphics, Random, Size> AfterCaptchaDrawEvent;`
<br><br>
* `CaptchaCreatedEvent`: This event will be called right AFTER line noise or wave distortion algorithms<br>
`public static event Action<Graphics, Random, Size> CaptchaCreatedEvent;`
<br><br>
### Parameters:
`Graphics`: For drawing of couse, you dont need to dispose it after use<br>
`Random`: Thread-safe instance of Random class, use them for any randomization stuff<br>
`Size`: To know the dimmensions of bitmap that `Graphics` parameter use<br>
<br>
You can use them as you want: to add some custom graphic noise or whatever<br>
All changes should be placed in **Global.asax -> Application_Start**
## Functions
There are 3 algorithms that backed into captcha generation:
1. **Wave Distortion**: transforms image to create wave-effect
2. **Line Noise**: draws random placed lines
3. **Ellipse Noise**: draws random placed and colored ellipses<br><br>
To set color of every line, ellipse and character that will be drawn you can use:
* `CharColorFunction`: The function that will be used to get the color of every character<br>
`public static Func<Random, Brush> CharColorFunction { get; set; }`
* `LineColorFunction`: The function that will be used to get the color of every line if LineNoiseEnabled is true  <br>
`public static Func<Random, Pen> LineColorFunction { get; set; } `
* `EllipseColorFunction`: The function that will be used to get the color of every ellipse if EllipseNoiseEnabled is true<br>
`public static Func<Random, Brush> EllipseColorFunction { get; set; }`<br>
`Random` is thread-safe instance of Random class, use them for any randomization stuff<br>
**By Default** all functions above are returning random color<br>
All changes should be placed in **Global.asax -> Application_Start**

At the end, how to add event:
`Captcha.BeforeCaptchaDrawEvent += (g, rnd, size) => { /*Your code there*/ };`
Fin
