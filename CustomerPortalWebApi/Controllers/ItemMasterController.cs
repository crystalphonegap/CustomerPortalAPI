using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ItemMasterController : ControllerBase
    {
        private readonly IItemMasterService _itemMasterService;
        private readonly ILogger _ILogger;

        public ItemMasterController(IItemMasterService ItemMasterService, ILogger ILoggerservice)
        {
            _ILogger = ILoggerservice;
            _itemMasterService = ItemMasterService;
        }

        [HttpGet("GetAllItemMaster/{KeyWord}")]
        public IActionResult GetAllItemMaster(string KeyWord)
        {
            try
            {
                return Ok(_itemMasterService.GetAllItems(KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetTopItemMaster")]
        public IActionResult GetTopItemMaster()
        {
            try
            {
                return Ok(_itemMasterService.GetTopfiveItems());
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
    }
}