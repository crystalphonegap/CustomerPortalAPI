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
    public class RoleMasterController : ControllerBase
    {
        private readonly IRoleService _RoleService;
        private readonly ILogger _ILogger;

        public RoleMasterController(IRoleService RoleService, ILogger ILoggerservice)
        {
            _ILogger = ILoggerservice;
            _RoleService = RoleService;
        }

        //Use for Insert Role Header
        [HttpPost("InsertRoleHeader")]
        public ActionResult InsertRoleHeader(RoleMasterModel model)
        {
            try
            {
                var RoleHeader = new RoleMasterModel()
                {
                    IDbint = model.IDbint,
                    RoleNamevtxt = model.RoleNamevtxt,
                    RoleDescriptionvtxt = model.RoleDescriptionvtxt,
                    CreatedByvtxt = model.CreatedByvtxt,
                    CreatedDatedatetime = DateTime.Now
                };
                //_Orderservice.Create(OrderHeader);
                return Ok(_RoleService.Create(RoleHeader));
                //return Ok(OrderHeader);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Update Role Header
        [HttpPut("UpdateRoleHeader")]
        public ActionResult UpdateRoleHeader(RoleMasterModel model)
        {
            try
            {
                var RoleHeader = new RoleMasterModel()
                {
                    IDbint = model.IDbint,
                    RoleNamevtxt = model.RoleNamevtxt,
                    RoleDescriptionvtxt = model.RoleDescriptionvtxt,
                    CreatedByvtxt = model.CreatedByvtxt,
                    CreatedDatedatetime = DateTime.Now
                };
                _RoleService.update(RoleHeader);
                return Ok(RoleHeader);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Delete Role Details
        [HttpDelete("DeleteRoleDetails/{RoleID}")]
        public ActionResult DeleteRoleDetails(long RoleID)
        {
            try
            {
                _RoleService.Delete(RoleID);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for insert Role Details
        [HttpPost("InsertRoleDetails")]
        public ActionResult InsertRoleDetails(RoleDetailsModel model)
        {
            try
            {
                var roledetails = new RoleDetailsModel()
                {
                    RoleIDint = model.RoleIDint,
                    RoleNamevtxt = model.RoleNamevtxt,
                    HeaderIDbint = model.HeaderIDbint,
                };
                _RoleService.InserRoleDetails(roledetails);
                return Ok(roledetails);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Role Header data search
        [HttpGet("GetRoleHeaderSearch/{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetRoleHeaderSearch(int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_RoleService.GetRoleMaster(PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Role Header count
        [HttpGet("GetRoleHeaderSearchCount/{KeyWord}")]
        public long GetRoleHeaderSearchCount(string KeyWord)
        {
            try
            {
                return _RoleService.GetRoleMasterCount(KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Get Role Header data by roleid
        [HttpGet("GetRoleHeaderByRoleID/{RoleID}")]
        public IActionResult GetRoleHeaderByRoleID(long RoleID)
        {
            try
            {
                return Ok(_RoleService.GetRoleMasterByRoleID(RoleID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Get Role Details data by roleid
        [HttpGet("GetRoleDetailsByRoleID/{RoleID}")]
        public IActionResult GetRoleDetailsByRoleID(long RoleID)
        {
            try
            {
                return Ok(_RoleService.GetRoleDetailsByRoleID(RoleID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for get Roles
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                return Ok(_RoleService.GetRoleS());
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Role Header for Checkboxlist
        [HttpGet("GetRolesForcheckBoxlist/{KeyWord}")]
        public IActionResult GetRolesForcheckBoxlist(string KeyWord)
        {
            try
            {
                return Ok(_RoleService.GetRoleMasterForcheckBoxlist(KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetUserRolesDetailsByCustomercode/{CustomerCode}")]
        public IActionResult GetUserRolesDetailsByCustomercode(string CustomerCode)
        {
            try
            {
                var result = _RoleService.GetUserRolesDetailsByCustomercode(CustomerCode);
                if (result == null)
                {
                    CustomerMasterModel model = new CustomerMasterModel();
                    model.Typevtxt = "TR";
                    return Ok(model);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return null;
            }
        }
    }
}