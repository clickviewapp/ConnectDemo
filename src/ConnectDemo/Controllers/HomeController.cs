namespace ConnectDemo.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Models;

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("clickview")]
        public IActionResult RedirectToClickView()
        {
            var clientId = _configuration.GetValue<string>("clientId");
            var secretKey = _configuration.GetValue<string>("secretKey");

            const string redirectUri = "https://localhost:5001/postback";
            const string connectUrl = "https://online.clickview.com.au/v1/connect";

            var requestParams = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"redirect_uri", redirectUri}
            };

            var signature = SignatureGenerator.Generate(requestParams, secretKey);

            requestParams.Add("signature", signature);

            var url = connectUrl + QueryString.Create(requestParams);

            _logger.LogInformation("Redirecting to {Url}", url);

            return Redirect(url);
        }

        [HttpPost("postback")]
        public IActionResult Postback([FromForm] PostBackModel model)
        {
            return View(model);
        }
    }
}
