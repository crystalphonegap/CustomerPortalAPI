using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OutstandingController : ControllerBase
    {
        private readonly IOutstandingService _outstandingService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public OutstandingController(IOutstandingService outstandingService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _outstandingService = outstandingService;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetOutstandingDashBoard/{CustomerCode}")]
        public IActionResult GetOutstandingDashBoard(string CustomerCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), CustomerCode))
                {
                    return Ok(_outstandingService.GetOutstanding(CustomerCode));
                }
                else
                {
                    return Ok("Un Authorized User");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetOutStandingData/{CustomerCode}")]
        public IActionResult GetOutStandingData(string CustomerCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), CustomerCode))
                {
                    return Ok(_outstandingService.GetOutStandingData(CustomerCode));
                }
                else
                {
                    return Ok("Un Authorized User");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
    }
}