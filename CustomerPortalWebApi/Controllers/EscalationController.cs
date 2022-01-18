using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EscalationController : ControllerBase
    {
        private readonly ILogger _ILogger;
        private readonly IEscalationService _EscalationService;
        private readonly IChecktokenservice _Checktokenservice;

        public EscalationController(ILogger ILoggerservice, IEscalationService IEscalationService, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _EscalationService = IEscalationService;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetEscalationByID/{Id}")]
        public IActionResult GetEscalationByID(long Id)
        {
            try
            {
                var GetEscalation = _EscalationService.GetEscalationMatrixByID(Id);
                return Ok(GetEscalation);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetEscalationList")]
        public IActionResult GetEscalationList()
        {
            try
            {
                var EscalationMatrix = _EscalationService.GetEscalationMatrix();
                return Ok(EscalationMatrix);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("Insert")]
        public ActionResult Insert(EscalationMatrixModel model)
        {
            try
            {
                return Ok(_EscalationService.InsertEscalationMatrix(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public ActionResult Update(EscalationMatrixModel model)
        {
            try
            {
                return Ok(_EscalationService.UpdateEscalationMatrix(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
    }
}