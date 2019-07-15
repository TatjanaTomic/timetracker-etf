using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackerEtf.Extensions;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TimeTrackerEtf.Controllers
{
    public class DummyAuthController: Controller
    {
        private IConfiguration _configuration;
        public DummyAuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/get-token")]
        public IActionResult GetToken(string name="etf-workshop", bool admin = false)
        {
            var jwt = JwtTokenGenerator
            .Generate(name, admin, _configuration["Tokens:Issuer"], _configuration["Tokens:Key"]);

            return Ok(jwt);
        }
    }
}
