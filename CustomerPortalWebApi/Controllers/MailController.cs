using ClosedXML.Excel;
using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService mailService;
        private readonly ILogger _ILogger;

        public MailController(IMailService mailService, ILogger ILoggerservice)
        {
            _ILogger = ILoggerservice;
            this.mailService = mailService;
        }

         
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(MailRequest mailRequest)
        {
            try
            {
                await mailService.SendEmailAsync(mailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }
         
        [HttpPost("SendMailToCustomer")]
        public async Task<IActionResult> SendMailToCustomer(string CustomerCode, string division)
        {
            try
            {
                await mailService.SendEmailToCustomer(CustomerCode, division);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }

         
        [HttpPost("ExcelUpload"), DisableRequestSizeLimit]
        public async Task<IActionResult> ExcelUpload()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Uploads", "ExcelFiles");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest("File is Empty");
                }
                foreach (var file in files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName); //you can add this path to a list and then return all dbPaths to the client if require
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        IExcelDataReader reader = null;

                        if (file.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else if (file.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            return BadRequest();
                        }

                        DataSet excelRecords = reader.AsDataSet();
                        reader.Close();
                        var finalRecords = excelRecords.Tables[0];
                        List<CustomerMasterModel> lstcusts = new List<CustomerMasterModel>();
                        for (int i = 0; i < finalRecords.Rows.Count; i++)
                        {
                            CustomerMasterModel cust = new CustomerMasterModel();
                            cust.CustCodevtxt = finalRecords.Rows[i][0].ToString();
                            cust.CustNamevtxt = finalRecords.Rows[i][1].ToString();
                            lstcusts.Add(cust);
                        }
                        if (lstcusts.Count > 0)
                        {
                            for (int k = 1; k < lstcusts.Count; k++)
                            {
                                await SendMailToCustomer(lstcusts[k].CustCodevtxt.ToString(), "Cement");
                            }
                        }
                    }
                }
                return Ok("file is  uploaded Successfully.");
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest( ex.Message);
            }
        }

        //use for downlaod sample file for customer upload
        [HttpGet("DownloadSampleCustomerUploadExcel")]
        public IActionResult DownloadSampleCustomerUploadExcel()
        {
            try
            {
                List<CustomerMasterModel> lstcust = new List<CustomerMasterModel>();
                CustomerMasterModel cust = new CustomerMasterModel();
                cust.CustCodevtxt = "";
                cust.CustNamevtxt = "";
                lstcust.Add(cust);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("CustomerUpload");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Customer Code";
                    worksheet.Cell(currentRow, 2).Value = "Customer Name";
                    foreach (var custs in lstcust)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = custs.CustCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = custs.CustNamevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleCustomerUpload.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("SendMailForForgetPassword/{UserCode}")]
        public async Task<string> SendMailForForgetPassword(string UserCode)
        {
            try
            {
                UserMaster OBJ = mailService.GetuserDetails(UserCode);
                if (OBJ.Idbint != -1)
                {
                    var result = await mailService.SendEmailForForgetPassword(OBJ.UserCodetxt, OBJ.UserNametxt, OBJ.Emailvtxt, OBJ.ResetTokenvtxt, OBJ.Mobilevtxt);
                    //return Ok("OTP for paswword reset has been send to your email Id and mobile number, OTP will be valid for 15 min");
                    return result;
                }
                else
                {
                    return "Invalid Email ID or Invalid User Code.";
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet("GetOTPFORLOGIN/{UserCode}")]
        public async Task<OTPSuccessfullModel> GetOTPFORLOGIN(string UserCode)
        {
            try
            {
                
                
                    var result = await mailService.GetOTPFORLOGIN(UserCode);
                    //return Ok("OTP for paswword reset has been send to your email Id and mobile number, OTP will be valid for 15 min");
                    return result;
                
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }


        [AllowAnonymous]
        [HttpGet("SubmitOTP/{MobileNumber},{OTP},{UserCode}")]
        public UserMaster LoginWithOTP(string MobileNumber,string OTP,string UserCode)
        {
            try
            {


                var result =  mailService.LoginWithOTP(MobileNumber,OTP, UserCode);
                //return Ok("OTP for paswword reset has been send to your email Id and mobile number, OTP will be valid for 15 min");
                return result;

            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }


    }
}