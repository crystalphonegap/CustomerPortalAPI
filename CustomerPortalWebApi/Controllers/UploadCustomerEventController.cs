using ClosedXML.Excel;
using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UploadCustomerEventController  : ControllerBase
    {

         private readonly IChecktokenservice _Checktokenservice;
         private readonly IUploadCustomerevent _UploadCustomereventservice;
        private readonly ILogger _ILogger;


        public UploadCustomerEventController(IUploadCustomerevent UploadCustomereventservice, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _Checktokenservice = checktokenservice;
            _UploadCustomereventservice = UploadCustomereventservice;
            

        }

         //----- Added By Sunil New Upload Image 11 Nov 2021 -- CustomerEventFilesModels

        [AllowAnonymous]
        [HttpPost("uploadDealerEventFile")]
        //[HttpPost("uploadDealerEventFile")]
             public async Task<IActionResult> uploadDealerFile([FromForm] UploadCustomerEvent shopPhoto)
             {
                 
                 string Token = Request.Headers["Authorization"];
                 string[] authorize = Token.Split(" ");
                var userId = _Checktokenservice.GetUserId(authorize[1]);
                var usercode = _Checktokenservice.GetUserCode(authorize[1].ToString());
                var userRole = _Checktokenservice.GetUserRole(authorize[1].ToString());
                 

                try
                {
                var CustomerEvents = new CustomerEventFilesModels()
                {
                   CustomerCodevtxt= usercode,
                   EventTypevtxt="Deepawali",
                   //Filevtxt=shopPhoto.SiteFile,
                   CreatedByvtxt=userId


                };
              var dbresult= await  _UploadCustomereventservice.InsertCustomerEvents(CustomerEvents, shopPhoto.SiteFile);
                //return Ok(CustomerEvents);

                if (dbresult != 0)

                {
                     //var UpdatePhoto=  _UploadCustomereventservice.UpdateCustomerEvents(CustomerEvents, shopPhoto.SiteFile,dbresult);
                    return Ok("File Upload Sucess fully");
                }
                else
                {
                    return Ok("File not Upload Sucess fully");
                };
                //
                    //
                    
                
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
            
            }

        //--------- End  Added By Sunil New Upload Image 10 Nov 2021


    }
}