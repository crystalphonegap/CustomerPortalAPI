using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PaymentReceiptController : ControllerBase
    {
        private readonly IPaymentReceiptService _paymentReceiptService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public PaymentReceiptController(IPaymentReceiptService paymentReceiptService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _paymentReceiptService = paymentReceiptService;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetPaymentReceipt")]
        public IActionResult GetPaymentReceipt(PaymentReceipt paymentReceipt)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), paymentReceipt.CustomerCodevtxt))
                {
                    return Ok(_paymentReceiptService.GetPaymentReceipt(paymentReceipt.CustomerCodevtxt, paymentReceipt.PageNo, paymentReceipt.PageSize, paymentReceipt.KeyWord));
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