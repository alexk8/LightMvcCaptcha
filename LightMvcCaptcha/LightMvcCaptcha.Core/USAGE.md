STARTUP

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession();
            app.UseCaptcha("/captcha-image");
		}

CONTROLLER
        [HttpPost]
        [Captcha("captchacode" , "sorry, try again")]
        public IActionResult Index(TestCaptchaModel model)
        {
            if (!ModelState.IsValid) return View(model);
            //success,continue processing
        }

VIEW
	@using LightMvcCaptcha.Core
    
	<form method="post">
        @Html.CaptchaFor(x => x.captchacode)
        <br />
        <div asp-validation-summary="ModelOnly" class="text-danger"></div>
        <br />
        <input asp-for="captchacode" autocomplete="off" />
        <span asp-validation-for="captchacode" class="text-danger"></span>
        <br />

        <input type="submit" value="POST" />
    </form>

