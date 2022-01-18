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
    public class BroadCastController : ControllerBase
    {
        private readonly IBroadCastservice _IBroadCastservice;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public BroadCastController(IBroadCastservice BroadCastservice, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _IBroadCastservice = BroadCastservice;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetBroadCastByDate/{Date}")]
        public IActionResult GetBroadCastByDate(string Date)
        {
            try
            {
                return Ok(_IBroadCastservice.GetBroadCastByDate(Date));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("Insert")]
        public ActionResult Insert(BroadCastModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var BroadCast = new BroadCastModel()
                    {
                        IDbint = 0,
                        Titlevtxt = model.Titlevtxt,
                        Messagevtxt = model.Messagevtxt,
                        Typevtxt = model.Typevtxt,
                        StartDatedate = model.StartDatedate,
                        EndDatedate = model.EndDatedate,
                        Statusbit = model.Statusbit,
                        CreatedByvtxt = model.CreatedByvtxt
                    };
                    return Ok(_IBroadCastservice.Insert(BroadCast));
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

        [HttpPost("InsertDetail")]
        public ActionResult InsertDetail(HierarchyModel model)
        {
            try
            {
                var Content = new HierarchyModel()
                {
                    HeaderIDbint = model.HeaderIDbint,
                    Codevtxt = model.Codevtxt,
                    Namevtxt = model.Namevtxt
                };
                return Ok(_IBroadCastservice.InsertDetail(Content));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public ActionResult Update(BroadCastModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var BroadCast = new BroadCastModel()
                    {
                        IDbint = model.IDbint,
                        Titlevtxt = model.Titlevtxt,
                        Typevtxt = model.Typevtxt,
                        Messagevtxt = model.Messagevtxt,
                        StartDatedate = model.StartDatedate,
                        EndDatedate = model.EndDatedate,
                        Statusbit = model.Statusbit,
                        CreatedByvtxt = model.CreatedByvtxt
                    };
                    return Ok(_IBroadCastservice.update(BroadCast));
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

        [HttpGet("GetBroadCastByID/{ID}")]
        public IActionResult GetBroadCastByID(long ID)
        {
            try
            {
                return Ok(_IBroadCastservice.GetBroadcastById(ID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetBroadCastListCount/{KeyWord}")]
        public long GetBroadCastListCount(string KeyWord)
        {
            try
            {
                return _IBroadCastservice.GetBroadcastListCount(KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetBroadCastList/{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetBroadCastList(int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_IBroadCastservice.GetBroadcastList(PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for get broadcast details by id
        [HttpGet("GetBroadCastDetailsByID/{ID}")]
        public IActionResult GetBroadCastDetailsByID(long ID)
        {
            try
            {
                return Ok(_IBroadCastservice.GetBroadcastDetailsById(ID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
    }
}