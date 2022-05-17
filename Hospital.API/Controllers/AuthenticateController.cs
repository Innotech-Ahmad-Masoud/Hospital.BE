using Hospital.Application.Interfaces;
using Hospital.Application.Models.Authenticate;
using Hospital.Application.Results.Authenticate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Hospital.Application.Results;

namespace Hospital.API.Controllers
{
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost(nameof(Login)), AllowAnonymous]
        [ProducesResponseType(typeof(Response<LoginResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authenticateService.Login(model);

            return result.Status switch
            {
                nameof(HttpStatusCode.OK) => Ok(result),
                _ => BadRequest(result.Message)
            };
        }

        [HttpPost(nameof(Register)), AllowAnonymous]
        [ProducesResponseType(typeof(Response<RegisterResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authenticateService.Register(model);

            return result.Status switch
            {
                nameof(HttpStatusCode.Unauthorized) => Unauthorized(),
                nameof(HttpStatusCode.OK) => Ok(result),
                _ => BadRequest(result.Message)
            };
        }

    }
}
