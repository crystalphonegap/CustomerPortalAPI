using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _EmployeeService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public EmployeeController(IEmployeeService EmployeeService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _EmployeeService = EmployeeService;
            _Checktokenservice = checktokenservice;
        }

        // use for Target vs Actual Sales Data for Employee wise
        [HttpGet("GetTargetVsActualDataForEmployee/{usercode},{usertype},{date},{type}")]
        public IActionResult GetTargetVsActualDataForEmployee(string usercode, string usertype, string date,string type)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetEmployeeWiseTargetVsActualSalesData(usercode, usertype, date, type));
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

        // use for EmployeeWiseReport
        [HttpGet("GetEmployeeWiseReport/{mode},{code},{date},{type}")]
        public IActionResult GetEmployeeWiseReport(string mode, string code, string date, string type)
        {
            try
            {
                return Ok(_EmployeeService.GetEmployeeWiseReport(mode, code, date, type));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // use for EmployeeWiseReport
        [HttpGet("GetAreaNameByAreaCode/{mode},{code}")]
        public IActionResult GetAreaNameByAreaCode(string mode, string code)
        {
            try
            {
                return Ok(_EmployeeService.GetAreaNameByAreaCode(mode, code));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // use for Dashboard count for Employee wise
        [HttpGet("GetEmployeeDashboardCount/{usercode},{usertype},{Type}")]
        public IActionResult GetEmployeeDashboardCount(string usercode, string usertype, string Type)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetEmployeeDashboardCounts(usercode, usertype,Type));
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

        // use for Dashboard count for Employee wise for TSE,BM,RM,ZM,Marketing Head
        [HttpGet("GetEmployeeWiseSalesCount/{usercode},{usertype},{date},{Type}")]
        public IActionResult GetEmployeeWiseSalesCount(string usercode, string usertype, string date,string Type)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetEmployeeWiseSalesCount(usercode, usertype, date, Type));
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

        // use for Dashboard count of Outstanding Data for CF and SP of < 30 days
        [HttpGet("GetOutstandingDataCountforCFSP30Days/{usercode},{usertype}")]
        public IActionResult GetOutstandingDataCountforCFSP30Days(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetCFSPOustingDataCount(usercode, usertype, "30 Days Count"));
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

        // use for Dashboard count of Outstanding Data for CF and SP of > 30 days
        [HttpGet("GetOutstandingDataCountforCFSPg30Days/{usercode},{usertype}")]
        public IActionResult GetOutstandingDataCountforCFSPg30Days(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetCFSPOustingDataCount(usercode, usertype, "g31 Days Count"));
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

        // use for Dashboard List of Outstanding Data for CF and SP of < 30 days
        [HttpGet("GetOutstandingDataListforCFSP30Days/{usercode},{usertype}")]
        public IActionResult GetOutstandingDataListforCFSP30Days(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetCFSPOustingDataList(usercode, usertype, "30 Days List"));
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

        // use for Dashboard List of Outstanding Data for CF and SP of > 30 days
        [HttpGet("GetOutstandingDataListforCFSPg30Days/{usercode},{usertype}")]
        public IActionResult GetOutstandingDataListforCFSPg30Days(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetCFSPOustingDataList(usercode, usertype, "g31 Days List"));
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

        // use for Dashboard List of Dealer  Data for Top 10 for emp only
        [HttpGet("GetTopDealerListInEmployeeDashboard/{usercode},{usertype},{date},{Type},{FillterType}")]
        public IActionResult GetTopDealerListInEmployeeDashboard(string usercode, string usertype, string date, string Type,string FillterType)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetDealerListInEmployeeDashboard(usercode, usertype, date, "Top", Type, FillterType));
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

        // use for Dashboard List of Dealer  Data for bottom 10 for emp only
        [HttpGet("GetbottomDealerListInEmployeeDashboard/{usercode},{usertype},{date},{Type},{FillterType}")]
        public IActionResult GetbottomDealerListInEmployeeDashboard(string usercode, string usertype, string date, string Type,string FillterType)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_EmployeeService.GetDealerListInEmployeeDashboard(usercode, usertype, date, "Bottom", Type, FillterType));
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