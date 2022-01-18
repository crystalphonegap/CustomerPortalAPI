using ClosedXML.Excel;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

//using System.Web.Http;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CustomerMasterController : ControllerBase
    {
        private readonly ICustomerMasterService _customerMasterService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public CustomerMasterController(ICustomerMasterService customerMasterService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _customerMasterService = customerMasterService;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetCustomerMaster/{Division},{pageNo},{pageSize},{KeyWord}")]
        public IActionResult GetCustomerMaster(string Division, int pageNo, int pageSize, string KeyWord)
        {
            try
            {
                return Ok(_customerMasterService.GetCustomerMaster(Division, pageNo, pageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("GetCustomerDataByUserId/{UserId}")]
        public IActionResult GetCustomerDataByUserId(string UserId)
        {
            try
            {
                return Ok(_customerMasterService.GetCustomerDataByUserId(UserId));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetCustomerCount/{Division}")]
        public long GetCustomerCount(string Division)
        {
            try
            {
                return _customerMasterService.CustomerCount(Division);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetShipTo/{CustomerCode}")]
        public IActionResult GetShipTo(string CustomerCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), CustomerCode))
                {
                    return Ok(_customerMasterService.GetShipTo(CustomerCode));
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

        [HttpGet("GetCustomerByUser/{UserCode}")]
        public IActionResult GetCustomerByUser(string UserCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), UserCode))
                {
                    return Ok(_customerMasterService.GetCustomerByUser(UserCode));
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

        [HttpGet("GetShipToAddress/{ShipToCode}")]
        public IActionResult GetShipToAddress(string ShipToCode)
        {
            try
            {
                return Ok(_customerMasterService.GetShipToAddress(ShipToCode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult ExcelUpload()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
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
                    }
                }
                return Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [AllowAnonymous]
        [HttpGet("CheckValidCustomer/{Customercode},{accesskey}")]
        public IActionResult CheckValidCustomer(string Customercode, string accesskey)
        {
            try
            {
                return Ok(_customerMasterService.CheckValidCustomer(Customercode, accesskey));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        //Update Customer
        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer(CustomerMasterModel customer)
        {
            try
            {
                return Ok(_customerMasterService.update(customer));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Update Customer
        [HttpPut("UpdateCustomerStatus")]
        public IActionResult UpdateCustomerStatus(CustomerMasterModel customer)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customer.CustCodevtxt))
                {
                    return Ok(_customerMasterService.UpdateCustomerStatus(customer));
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

        //use for Get Customers By UserTypewise
        [HttpGet("GetCustomerByUserTypeWise/{usercode},{usertype},{pageno},{pagesize},{keyword},{status},{isActive}")]
        public IActionResult GetCustomerByUserTypeWise(string usercode, string usertype, int pageno, int pagesize, string keyword, Boolean status, Boolean isActive)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_customerMasterService.GetCustomerMasterUserType(usercode, usertype, pageno, pagesize, keyword, status, isActive));
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

        //use for Get Customers By UserTypewise Count
        [HttpGet("GetCustomerByUserTypeWiseCount/{usercode},{usertype},{keyword},{status},{isActive}")]
        public long GetCustomerByUserTypeWiseCount(string usercode, string usertype, string KeyWord, Boolean status, Boolean isActive)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return _customerMasterService.GetCustomerMasterUserTypeCount(usercode, usertype, KeyWord, status, isActive);
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

        //use for export to excel Customers By UserTypewise
        [HttpGet("ExportToExcel/{usercode},{usertype},{KeyWord},{status},{isActive}")]
        public IActionResult ExportToExcel(string usercode, string usertype, string KeyWord, Boolean status, Boolean isActive)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    List<CustomerMasterModel> custs = _customerMasterService.GetCustomerMasterUserTypeDownload(usercode, usertype, KeyWord, status, isActive);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Customers");
                        var currentRow = 1;
                        var srNo = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Customer Code";
                        worksheet.Cell(currentRow, 3).Value = "Customer Name";
                        worksheet.Cell(currentRow, 4).Value = "Mobile";
                        worksheet.Cell(currentRow, 5).Value = "Email";
                        worksheet.Cell(currentRow, 6).Value = "Address";
                        worksheet.Cell(currentRow, 7).Value = "City";

                        foreach (var cust in custs)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srNo++;
                            worksheet.Cell(currentRow, 2).Value = cust.CustCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = cust.CustNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = cust.TelNumber1vtxt;
                            worksheet.Cell(currentRow, 5).Value = cust.Emailvtxt;
                            worksheet.Cell(currentRow, 6).Value = cust.Address1vtxt;
                            worksheet.Cell(currentRow, 7).Value = cust.CityCdvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "CustomerS.xlsx");
                        }
                    }
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

        //use for Get Customers By System Admin Login
        [HttpGet("GetAllCustomersforSystemAdminSearch/{pageno},{pagesize},{status},{division},{keyword}")]
        public IActionResult GetAllCustomersforSystemAdminSearch(int pageno, int pagesize, string status, string division, string keyword)
        {
            try
            {
                return Ok(_customerMasterService.GetCustomerMasterSystemAdminSearch(pageno, pagesize, status, division, keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Get Customers By System Admin Login
        [HttpGet("GetAllCustomersforSystemAdminSearchCount/{status},{division},{keyword}")]
        public long GetAllCustomersforSystemAdminSearchCount(string status, string division, string keyword)
        {
            try
            {
                return _customerMasterService.GetCustomerMasterSystemAdminSearchCount(status, division, keyword);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for export to excel System Admin Login
        [HttpGet("ExportToExcelForSystemAdmin/{status},{division},{keyword}")]
        public IActionResult ExportToExcelForSystemAdmin(string status, string division, string keyword)
        {
            try
            {
                List<CustomerMasterModel> custs = _customerMasterService.GetCustomerMasterSystemAdminDownload(status, division, keyword);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Customers");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Customer Code";
                    worksheet.Cell(currentRow, 3).Value = "Customer Name";
                    worksheet.Cell(currentRow, 4).Value = "Mobile";
                    worksheet.Cell(currentRow, 5).Value = "Email";
                    worksheet.Cell(currentRow, 6).Value = "Address";
                    worksheet.Cell(currentRow, 7).Value = "City";
                    worksheet.Cell(currentRow, 8).Value = "Territory Code";
                    worksheet.Cell(currentRow, 9).Value = ",Territory Name";
                    worksheet.Cell(currentRow, 10).Value = "Branch Code";
                    worksheet.Cell(currentRow, 11).Value = "Branch Name";
                    worksheet.Cell(currentRow, 12).Value = "Region Code";
                    worksheet.Cell(currentRow, 13).Value = "RegionName";
                    worksheet.Cell(currentRow, 14).Value = "SP Code";
                    worksheet.Cell(currentRow, 15).Value = "SP Name";
                    worksheet.Cell(currentRow, 16).Value = "Contact Person";
                    worksheet.Cell(currentRow, 17).Value = "Contact Person Mobile";
                    worksheet.Cell(currentRow, 18).Value = "Price List";
                    worksheet.Cell(currentRow, 19).Value = "Delivery Terms";
                    worksheet.Cell(currentRow, 20).Value = "Password";
                    worksheet.Cell(currentRow, 21).Value = " Status";
                    worksheet.Cell(currentRow, 22).Value = "Registration Status";
                    worksheet.Cell(currentRow, 23).Value = "Access Token";

                    foreach (var cust in custs)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = cust.CustCodevtxt;
                        worksheet.Cell(currentRow, 3).Value = cust.CustNamevtxt;
                        worksheet.Cell(currentRow, 4).Value = cust.TelNumber1vtxt;
                        worksheet.Cell(currentRow, 5).Value = cust.Emailvtxt;
                        worksheet.Cell(currentRow, 6).Value = cust.Address1vtxt;
                        worksheet.Cell(currentRow, 7).Value = cust.CityCdvtxt;
                        worksheet.Cell(currentRow, 8).Value = cust.TerritoryCodevtxt;
                        worksheet.Cell(currentRow, 9).Value = cust.TerritoryNamevtxt;
                        worksheet.Cell(currentRow, 10).Value = cust.Branchvtxt;
                        worksheet.Cell(currentRow, 11).Value = cust.BranchNamevtxt;
                        worksheet.Cell(currentRow, 12).Value = cust.RegionCdvtxt;
                        worksheet.Cell(currentRow, 13).Value = cust.RegionNamevtxt;
                        worksheet.Cell(currentRow, 14).Value = cust.SPCodevtxt;
                        worksheet.Cell(currentRow, 15).Value = cust.SPNamevtxt;
                        worksheet.Cell(currentRow, 16).Value = cust.Contactpersonvtxt;
                        worksheet.Cell(currentRow, 17).Value = cust.ContactpersonMobilevtxt;
                        worksheet.Cell(currentRow, 18).Value = cust.PriceListvtxt;
                        worksheet.Cell(currentRow, 19).Value = cust.DeliveryTerms1vtxt;
                        worksheet.Cell(currentRow, 20).Value = string.IsNullOrEmpty(cust.Password) ? "" : Decrypttxt(cust.Password);
                        worksheet.Cell(currentRow, 21).Value =  cust.IsActivebit==true?"Active":"InActive";
                        worksheet.Cell(currentRow, 22).Value =  cust.Statusbit == true? "Registered " : "Not Registered";
                        worksheet.Cell(currentRow, 23).Value = cust.AccessTokenKeyvtxt;
                    }
                    
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "CustomerS.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        public static string Decrypttxt(string cipherText)
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                try
                {
                    string EncryptionKey = "MAKV2SPBNI99212";
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            cipherText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                    return cipherText;
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        //use for customer ledger
        [HttpGet("GetLedger/{Customercode},{pageno},{pagesize}")]
        public IActionResult GetLedger( string Customercode, int pageno, int pagesize)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), Customercode))
                {
                    return Ok(_customerMasterService.GetCustomerLedger(Customercode, pageno, pagesize));
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

        //use for customer ledgercount
        [HttpGet("GetLedgerCount/{Customercode}")]
        public IActionResult GetLedgerCount( string Customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), Customercode))
                {
                    return Ok(_customerMasterService.GetCustomerLedgerCount(Customercode));
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

        //use for customer ledger Export To Excel
        [HttpGet("GetLedgerExportToExcel/{Customercode}")]
        public IActionResult GetLedgerExportToExcel(string Customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), Customercode))
                {
                    List<CustomerLedger> custs = _customerMasterService.GetCustomerLedgerForExportToExcel(Customercode);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("CustomerLedger");
                        var currentRow = 1;
                        var srNo = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Document No";
                        worksheet.Cell(currentRow, 3).Value = "Document Date";
                        worksheet.Cell(currentRow, 4).Value = "Document Type";
                        worksheet.Cell(currentRow, 5).Value = "Plant";
                        worksheet.Cell(currentRow, 6).Value = "Material";
                        worksheet.Cell(currentRow, 7).Value = "Quantity";
                        worksheet.Cell(currentRow, 8).Value = "Debit";
                        worksheet.Cell(currentRow, 9).Value = "Credit";
                        worksheet.Cell(currentRow, 10).Value = "Balance";

                        foreach (var cust in custs)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srNo++;
                            worksheet.Cell(currentRow, 2).Value = cust.DocumentNovtxt;
                            worksheet.Cell(currentRow, 3).Value = cust.DocumentDatedate;
                            worksheet.Cell(currentRow, 4).Value = cust.DocumentTypevtxt; //
                            worksheet.Cell(currentRow, 5).Value = cust.Plantvtxt;  //
                            worksheet.Cell(currentRow, 6).Value = cust.Materialvtxt;
                            worksheet.Cell(currentRow, 7).Value = cust.Quantitydcl;
                            worksheet.Cell(currentRow, 8).Value = cust.Debitdcl;
                            worksheet.Cell(currentRow, 9).Value = cust.Creditdcl;
                            worksheet.Cell(currentRow, 10).Value = cust.Balancedcl;
                        }
                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "CustomerLedger.xlsx");
                        }
                    }
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


        [AllowAnonymous]
        [HttpGet("GetCustomerDataForKAM/{UserId}")]
        public IActionResult GetCustomerDataForKAM(string UserId)
        {
            try
            {
                return Ok(_customerMasterService.GetCustomerDataforKAM(UserId));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("GetPlantMaster/{customercode}")]
        public IActionResult GetPlantMaster(string customercode)
        {
            try
            {
                return Ok(_customerMasterService.GetPlantMaster(customercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
 
        [HttpGet("getMasonSearch/{PageNo},{Datacount},{Keyword}")]
        public IActionResult getMasonSearch(int PageNo, int Datacount, string Keyword)
        {
            try
            {
                return Ok(_customerMasterService.GetTempMason(PageNo, Datacount, Keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }







        [HttpPost("MasonExcelUpload")]
        public IActionResult MasonExcelUpload()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Uploads", "Mason");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
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
                        List<MasonModel> lstcusts = new List<MasonModel>();
                        for (int i = 1; i < finalRecords.Rows.Count; i++)
                        {
                            MasonModel Mason = new MasonModel();
                            Mason.Idbint = 0;
                            Mason.MasonCodetxt = finalRecords.Rows[i][0].ToString();
                            Mason.MasonNametxt = finalRecords.Rows[i][1].ToString();
                            Mason.MasonMobileNumber = finalRecords.Rows[i][2].ToString();
                            Mason.MasonCustomerCode = finalRecords.Rows[i][3].ToString();
                            _customerMasterService.UploadMason(Mason);
                        }
                    }
                }
                return Ok("All the files are successfully uploaded.");
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }



        //use for downlaod sample sales hierachy
        [HttpGet("DownloadSampleMasonExcel")]
        public IActionResult DownloadSampleMasonExcel()
        {
            try
            {
               
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SalesHierachy");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Mason Code";
                    worksheet.Cell(currentRow, 2).Value = "Mason Name";
                    worksheet.Cell(currentRow, 3).Value = "Mason Mobile Number";
                    worksheet.Cell(currentRow, 4).Value = "Mason Customer Code"; 
                  

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleMason.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

         
        [HttpGet("ExcelMason/{keyword}")]
        public IActionResult ExcelMason(string keyword)
        {
            try
            {
                 
                List<MasonModel> sales = _customerMasterService.GetTempMason(-2,0, keyword);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SalesHierachy");
                    int currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Mason Code";
                    worksheet.Cell(currentRow, 3).Value = "Mason Name";
                    worksheet.Cell(currentRow, 4).Value = "Mason Mobile Number";
                    worksheet.Cell(currentRow, 5).Value = "Mason Customer Code";
                    worksheet.Cell(currentRow, 6).Value = "Mason Customer Name";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = currentRow-1;
                        worksheet.Cell(currentRow, 2).Value = sale.MasonCodetxt;
                        worksheet.Cell(currentRow, 3).Value = sale.MasonNametxt;
                        worksheet.Cell(currentRow, 4).Value = sale.MasonMobileNumber;
                        worksheet.Cell(currentRow, 5).Value = sale.MasonCustomerCode;
                        worksheet.Cell(currentRow, 6).Value = sale.MasonCustomerName;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "Mason.xlsx");
                    }
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