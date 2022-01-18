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
    public class ContentController : ControllerBase
    {
        private readonly IContentService _IContentService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public ContentController(IContentService ContentService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _IContentService = ContentService;
            _Checktokenservice = checktokenservice;
        }

        [HttpPost("Insert")]
        public ActionResult Insert(ContentModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var Content = new ContentModel()
                    {
                        IDbint = 0,
                        Titlevtxt = model.Titlevtxt,
                        Contentvtxt = model.Contentvtxt,
                        Typevtxt = model.Typevtxt,
                        StartDatedate = model.StartDatedate,
                        EndDatedate = model.EndDatedate,
                        Statusbit = model.Statusbit,
                        CreatedByvtxt = model.CreatedByvtxt
                    };
                    return Ok(_IContentService.Insert(Content));
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
                return Ok(_IContentService.InsertDetail(Content));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPut("Update")]
        public ActionResult Update(ContentModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var Content = new ContentModel()
                    {
                        IDbint = model.IDbint,
                        Titlevtxt = model.Titlevtxt,
                        Contentvtxt = model.Contentvtxt,
                        Typevtxt = model.Typevtxt,
                        StartDatedate = model.StartDatedate,
                        EndDatedate = model.EndDatedate,
                        Statusbit = model.Statusbit,
                        CreatedByvtxt = model.CreatedByvtxt
                    };
                    return Ok(_IContentService.update(Content));
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

        [HttpGet("GetContentByID/{ID}")]
        public IActionResult GetContentByID(long ID)
        {
            try
            {
                return Ok(_IContentService.GetContentById(ID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetContentByDate/{date}")]
        public IActionResult GetContentByDate(string date)
        {
            try
            {
                return Ok(_IContentService.GetContentByDate(date));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetContentListCount/{KeyWord}")]
        public long GetContentListCount(string KeyWord)
        {
            try
            {
                return _IContentService.GetContentListCount(KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetContentList/{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetContentList(int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_IContentService.GetContentList(PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for get Heirarchy list by type wise
        [HttpGet("GetHeirachywiseByType/{type}")]
        public IActionResult GetHeirachywiseByType(string type)
        {
            try
            {
                return Ok(_IContentService.GetHierachyWiseCodeName(type));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for get content detail list
        [HttpGet("GetContentDetailByID/{ID}")]
        public IActionResult GetContentDetailByID(long ID)
        {
            try
            {
                return Ok(_IContentService.GetContentDetailsById(ID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
    }
}