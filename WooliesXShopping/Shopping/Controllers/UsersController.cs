using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopping.Models;
using Shopping.Services;


namespace Shopping.Controllers
{
    [Route("api")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET api/user
        [HttpGet("user")]
        public ActionResult<UserInfo> GetUserInfo()
        {
            _logger.LogInformation("api/user has been called...");

            var response = _userService.GetUserInfo();

            _logger.LogInformation($"api/user response {response.Name} {response.Token}");

            return response;
        }
    }
}
