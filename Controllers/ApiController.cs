using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiIntegrationBoilerplate.Models;
using ApiIntegrationBoilerplate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiIntegrationBoilerplate.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ISessionManager _sessionManager;
        private readonly ITokenManager _tokenManager;
        private IIntegrationService _integrationService;
        private Configuration _configuration;

        public ApiController(ISessionManager sessionManager, ITokenManager tokenManager, IIntegrationService integrationService, Configuration configuration)
        {
            _sessionManager = sessionManager;
            _tokenManager = tokenManager;
            _integrationService = integrationService;
            _configuration = configuration;

        }

        [HttpPost("config")]
        public async Task<IActionResult> PostConfig([FromBody] ConfigJson configJson)
        {
            try
            {
                Configuration config = await _integrationService.SetConfigAsync(configJson);
                if (config == null)
                {
                    return BadRequest(new Status("System not configured!"));
                }

                return Ok(new Status("System configured!"));
            }
            catch
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Status("Service unavailable!"));
            }
        }

        [HttpGet("health")]
        public IActionResult GetHealth()
        {
            return Ok(_integrationService.GetHealth());
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (_configuration.ConfigJson == null)
            {
                return BadRequest(new Status("System not configured"));
            }

            try
            {
                List<User> users = await _integrationService.GetAllUsersAsync();
                return Ok(users);
            }
            catch
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Status("Service unavailable!"));
            }
        }

        [HttpPost("hit")]
        public async Task<IActionResult> PostHit([FromBody] Hit hit)
        {
            if (_configuration.ConfigJson == null)
            {
                return BadRequest(new Status("System not configured."));
            }

            HttpStatusCode statusCode;

            try
            {
                statusCode = await _integrationService.PostHitAsync(hit);

                if (statusCode == HttpStatusCode.OK)
                {
                    return Ok(new Status("User access granted!"));
                }

                if (statusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(new Status("User access denied."));
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Status($"Service unavailable! Reason: {e.Message}"));
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Status("Service unavailable!"));
        }
    }
}
