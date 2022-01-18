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

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TargetSalesController : ControllerBase
    {
        private readonly ITargetSalesService _TargetSaleservice;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public TargetSalesController(ITargetSalesService TargetService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _TargetSaleservice = TargetService;
            _Checktokenservice = checktokenservice;
        }

        [HttpPost("TargetSalesExcelUpload"), DisableRequestSizeLimit]
        public IActionResult TargetSalesExcelUpload()
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Uploads", "ExcelFiles");
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
                        List<TargetSalesModel> lstTargetSales = new List<TargetSalesModel>();
                        for (int i = 1; i < finalRecords.Rows.Count; i++)
                        {
                            string MONTH = Convert.ToString(finalRecords.Rows[i][0]);
                            if (MONTH != "" && MONTH.Substring(0, 3).Length == 3)
                            {
                                string strmnt = MONTH.Substring(0, 3).ToUpper();
                                switch (strmnt)
                                {
                                    case "JAN":
                                        MONTH = "January";
                                        break;

                                    case "FEB":
                                        MONTH = "February";
                                        break;

                                    case "MAR":
                                        MONTH = "March";
                                        break;

                                    case "APR":
                                        MONTH = "April";
                                        break;

                                    case "MAY":
                                        MONTH = "May";
                                        break;

                                    case "JUN":
                                        MONTH = "June";
                                        break;

                                    case "JUL":
                                        MONTH = "July";
                                        break;

                                    case "AUG":
                                        MONTH = "August";
                                        break;

                                    case "SEP":
                                        MONTH = "September";
                                        break;

                                    case "OCT":
                                        MONTH = "October";
                                        break;

                                    case "NOV":
                                        MONTH = "November";
                                        break;

                                    case "DEC":
                                        MONTH = "December";
                                        break;
                                }

                                TargetSalesModel targ = new TargetSalesModel();
                                targ.CustomerCodevtxt = finalRecords.Rows[i][2].ToString();
                                targ.CustomerNamevtxt = finalRecords.Rows[i][3].ToString();
                                targ.TargetSalesdcl = Convert.ToDecimal(finalRecords.Rows[i][4]);
                                targ.Divisionvtxt = "Cement";
                                targ.Monthvtxt = MONTH;
                                targ.Yearvtxt = Convert.ToInt16(finalRecords.Rows[i][1].ToString());
                                targ.CreatedByvtxt = "SA003";
                                lstTargetSales.Add(targ);
                            }
                        }
                        if (lstTargetSales.Count > 0)
                        {
                            for (int k = 0; k < lstTargetSales.Count; k++)
                            {
                                long j = _TargetSaleservice.InsertTargetSales(lstTargetSales[k]);
                            }
                        }
                    }
                }
                return Ok("file is  uploaded Successfully.");
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Excel/{division},{KeyWord}")]
        public IActionResult Excel(string division, string KeyWord)
        {
            try
            {
                List<TargetSalesModel> sales = _TargetSaleservice.DownloadTargetSales(division, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("TragetSales");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Customer Code";
                    worksheet.Cell(currentRow, 3).Value = "Customer Name";
                    worksheet.Cell(currentRow, 4).Value = "Month";
                    worksheet.Cell(currentRow, 5).Value = "Year";
                    worksheet.Cell(currentRow, 6).Value = "Target Sales (in MT)";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.Monthvtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.Yearvtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.TargetSalesdcl;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "TargetSales.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetAllTargetSalesLists/{Division},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetAllTargetSalesLists(string Division, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_TargetSaleservice.GetTargetSales(Division, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetTargetSalesForDashboardbyMonth/{customercode},{date}")]
        public IActionResult GetTargetSalesForDashboardbyMonth(string customercode, string date)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_TargetSaleservice.GetTargetSalesForDashboard(customercode, date));
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

        

        [HttpGet("GetKAMTargetSalesForDashboard/{usercode},{date}")]
        public IActionResult GetKAMTargetSalesForDashboard(string usercode, string date)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                //if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                //{
                    return Ok(_TargetSaleservice.GetKAMTargetSalesForDashboard(usercode, date));
                //}
                //else
                //{
                //    return Ok("Un Authorized User");
                //}
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetTargetSalesForDashboardbyYear/{customercode},{date}")]
        public IActionResult GetTargetSalesForDashboardbyYear(string customercode, string date)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_TargetSaleservice.GetTargetSalesForDashboardByFinance(customercode, date));
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

        [HttpGet("DownloadSampleExcel")]
        public IActionResult DownloadSampleExcel()
        {
            try
            {
                List<TargetSalesModel> sales = new List<TargetSalesModel>();
                TargetSalesModel model = new TargetSalesModel();
                model.CustomerCodevtxt = "";
                model.CustomerNamevtxt = "";
                model.Monthvtxt = "";
                model.Yearvtxt = DateTime.Now.Year;
                model.TargetSalesdcl = 0;
                sales.Add(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("TragetSales");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Month";
                    worksheet.Cell(currentRow, 2).Value = "Year";
                    worksheet.Cell(currentRow, 3).Value = "Customer Code";
                    worksheet.Cell(currentRow, 4).Value = "Customer Name";
                    worksheet.Cell(currentRow, 5).Value = "Target Sales (in MT)";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.Monthvtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.Yearvtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.TargetSalesdcl;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleTargetSales.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetAllTargetSalesListsCount/{division},{KeyWord}")]
        public long GetAllTargetSalesListsCount(string division, string KeyWord)
        {
            try
            {
                return _TargetSaleservice.GetTargetSalesListCount(division, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }


        //use for download sample customer sales data
        [HttpGet("DownloadCustomerSalesSampleExcel")]
        public IActionResult DownloadCustomerSalesSampleExcel()
        {
            try
            {
                List<TargetSalesModel> sales = new List<TargetSalesModel>();
                TargetSalesModel model = new TargetSalesModel();
                model.CustomerCodevtxt = "";
                model.CustomerNamevtxt = "";
                model.Monthvtxt = "";
                model.Yearvtxt = DateTime.Now.Year;
                model.TargetSalesdcl = 0;
                sales.Add(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("TragetSales");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Month";
                    worksheet.Cell(currentRow, 2).Value = "Year";
                    worksheet.Cell(currentRow, 3).Value = "Customer Code";
                    worksheet.Cell(currentRow, 4).Value = "Customer Name";
                    worksheet.Cell(currentRow, 5).Value = "Target Sales (in MT)";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.Monthvtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.Yearvtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.TargetSalesdcl;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleTargetSales.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for upload  customer sales data
        [HttpPost("CustomerSalesExcelUpload/{Type}"), DisableRequestSizeLimit]
        public IActionResult CustomerSalesExcelUpload(string Type )
        {
            try
            {
                var files = Request.Form.Files;
                var folderName = Path.Combine("Uploads", "ExcelFiles");
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
                        List<CustomerSalesModel> lstTargetSales = new List<CustomerSalesModel>();
                        for (int i = 1; i < finalRecords.Rows.Count; i++)
                        {
                            string MONTH = Convert.ToString(finalRecords.Rows[i][0]);
                            if (MONTH != "" && MONTH.Substring(0, 3).Length == 3)
                            {
                                string strmnt = MONTH.Substring(0, 3).ToUpper();
                                switch (strmnt)
                                {
                                    case "JAN":
                                        MONTH = "January";
                                        break;

                                    case "FEB":
                                        MONTH = "February";
                                        break;

                                    case "MAR":
                                        MONTH = "March";
                                        break;

                                    case "APR":
                                        MONTH = "April";
                                        break;

                                    case "MAY":
                                        MONTH = "May";
                                        break;

                                    case "JUN":
                                        MONTH = "June";
                                        break;

                                    case "JUL":
                                        MONTH = "July";
                                        break;

                                    case "AUG":
                                        MONTH = "August";
                                        break;

                                    case "SEP":
                                        MONTH = "September";
                                        break;

                                    case "OCT":
                                        MONTH = "October";
                                        break;

                                    case "NOV":
                                        MONTH = "November";
                                        break;

                                    case "DEC":
                                        MONTH = "December";
                                        break;
                                }

                                CustomerSalesModel targ = new CustomerSalesModel();
                                targ.CustomerCodevtxt = finalRecords.Rows[i][2].ToString();
                                targ.CustomerNamevtxt = finalRecords.Rows[i][3].ToString();
                                targ.Salesdcl = Convert.ToDecimal(finalRecords.Rows[i][4]);
                                targ.Typevtxt = Type;
                                targ.Monthvtxt = MONTH;
                                targ.Yearvtxt = (finalRecords.Rows[i][1].ToString());
                                targ.CreatedByvtxt = "SA003";
                                lstTargetSales.Add(targ);
                            }
                            else
                            {
                                return Ok("Enter proper month name as JAN ,FEB etc.");
                            }
                        }
                        if (lstTargetSales.Count > 0)
                        {
                            for (int k = 0; k < lstTargetSales.Count; k++)
                            {
                                long j = _TargetSaleservice.InsertCustomerSales(lstTargetSales[k]);
                            }
                        }
                    }
                }
                return Ok("file is  uploaded Successfully.");
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        //use for download excel file of Customer SalesData
        [HttpGet("CustomerSalesExcel/{Type},{KeyWord}")]
        public IActionResult CustomerSalesExcel(string Type, string KeyWord)
        {
            try
            {
                List<CustomerSalesModel> sales = _TargetSaleservice.DownloadCustomerSales(Type, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("CustomerSales");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Sales Type";
                    worksheet.Cell(currentRow, 3).Value = "Customer Code";
                    worksheet.Cell(currentRow, 4).Value = "Customer Name";
                    worksheet.Cell(currentRow, 5).Value = "Month";
                    worksheet.Cell(currentRow, 6).Value = "Year";
                    worksheet.Cell(currentRow, 7).Value = "Sales (in MT)";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = sale.Typevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.Monthvtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.Yearvtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.Salesdcl;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "TargetSales.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }


        //use for Customer Sales List Count
        [HttpGet("GetAllCustomerSalesListsCount/{type},{KeyWord}")]
        public long GetAllCustomerSalesListsCount(string type, string KeyWord)
        {
            try
            {
                return _TargetSaleservice.GetCustomerSalesListCount(type, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Customer Sales List Count
        [HttpGet("GetCustomerSalesData/{type},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetCustomerSalesData(string Type, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_TargetSaleservice.GetCustomerSalesData( Type,  PageNo,  PageSize,  KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return Ok(null);
            }
        }

        //Use for Show Graph for Target Vs Actual Sales in Dealer Profile
        [HttpGet("GetTargetSalesForDealerProfile/{customercode},{date}")]
        public IActionResult GetTargetSalesForDealerProfile(string customercode, string date)
        {
            try
            {
                return Ok(_TargetSaleservice.GetCustomerProfileTranscationDataTargetSales(customercode, date));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Show Graph for NCR and Payment Sales in Dealer Profile 
        [HttpGet("GetNCRorPaymnetSalesForDealerProfile/{customercode},{date},{mode}")]
        public IActionResult GetNCRorPaymnetSalesForDealerProfile(string customercode, string date,string mode)
        {
            try
            {
                return Ok(_TargetSaleservice.GetCustomerProfileTranscationDataNCROrPayment(customercode, date,mode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        //Use for Show Gridview for Sales Break Up  in Dealer Profile 
        [HttpGet("GetSalesBreakUpForDealerProfile/{customercode},{date}")]
        public IActionResult GetSalesBreakUpForDealerProfile(string customercode, string date)
        {
            try
            {
                return Ok(_TargetSaleservice.GetSalesBreakUp(customercode, date));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use GetCustomerProfileTranscationSalesHistory
        [HttpGet("GetCustomerProfileTranscationSalesHistory/{customercode},{date}")]
        public IActionResult GetCustomerProfileTranscationSalesHistory(string customercode, string date)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_TargetSaleservice.GetCustomerProfileTranscationSalesHistory(customercode, date, "SalesHistory"));
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

        //use GetCustomerProfileConsistency
        [HttpGet("GetCustomerProfileConsistency/{customercode},{date}")]
        public IActionResult GetCustomerProfileConsistency(string customercode, string date)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_TargetSaleservice.GetCustomerProfileConsistency(customercode, date));
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
        //use GetCustomerProfileEffective
        [HttpGet("GetCustomerProfileEffective/{customercode},{date}")]
        public IActionResult GetCustomerProfileEffective(string customercode, string date)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_TargetSaleservice.GetCustomerProfileEffective(customercode, date));
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