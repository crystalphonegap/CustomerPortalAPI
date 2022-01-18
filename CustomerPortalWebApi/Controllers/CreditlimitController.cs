using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CreditlimitController : ControllerBase
    {
        private readonly ICreditlimitService _creditlimitService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public CreditlimitController(ICreditlimitService creditlimitService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _creditlimitService = creditlimitService;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetCreditlimit/{CustomerCode}")]
        public Decimal GetCreditlimit(string CustomerCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), CustomerCode))
                {
                    return _creditlimitService.GetCreditlimit(CustomerCode);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetAvailableCreditlimit/{CustomerCode}")]
        public Decimal GetAvailableCreditlimit(string CustomerCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), CustomerCode))
                {
                    return _creditlimitService.GetAvailableCreditlimit(CustomerCode);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }
    }
}