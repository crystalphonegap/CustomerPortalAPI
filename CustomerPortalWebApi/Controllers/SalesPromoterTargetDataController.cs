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
    public class SalesPromoterTargetDataController : ControllerBase
    {
        private readonly ISalesPromoterTargetDataServices _SalesPromoterTargetDataServices;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public SalesPromoterTargetDataController(ISalesPromoterTargetDataServices SalesPromoterTargetDataServices, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _SalesPromoterTargetDataServices = SalesPromoterTargetDataServices;
            _Checktokenservice = checktokenservice;
        }

        //use for download sample file
        [HttpGet("DownloadSampleExcel")]
        public IActionResult DownloadSampleExcel()
        {
            try
            {
                List<SalesPromoterTargetData> sales = new List<SalesPromoterTargetData>();
                SalesPromoterTargetData model = new SalesPromoterTargetData();
                model.SalesPromotorCodevtxt = "";
                model.DealerTargetApptint = 0;
                model.Monthvtxt = "";
                model.DealerActualApptint = 0;
                model.RetailerTargetApptint = 0;
                model.RetailerActualApptint = 0;
                model.Yeartxt = "";
                sales.Add(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("DealerAppointData");
                    var currentRow = 1;

                    worksheet.Cell(currentRow, 1).Value = "Sales Promoter Code";
                    worksheet.Cell(currentRow, 2).Value = "Month";
                    worksheet.Cell(currentRow, 3).Value = "Dealer Target Appointment Count";
                    worksheet.Cell(currentRow, 4).Value = "Dealer Actual Appointment Count";
                    worksheet.Cell(currentRow, 5).Value = "Retailer Target Appointment Count";
                    worksheet.Cell(currentRow, 6).Value = "Retailer Actual Appointment Count";
                    worksheet.Cell(currentRow, 7).Value = "YEAR";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.SalesPromotorCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.Monthvtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.DealerTargetApptint;
                        worksheet.Cell(currentRow, 4).Value = sale.DealerActualApptint;
                        worksheet.Cell(currentRow, 5).Value = sale.RetailerTargetApptint;
                        worksheet.Cell(currentRow, 6).Value = sale.RetailerActualApptint;
                        worksheet.Cell(currentRow, 7).Value = sale.RetailerActualApptint;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleDealerAppointmentData.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for upload excel file
        [HttpPost("SalesPromoterTargetDataExcelUpload"), DisableRequestSizeLimit]
        public IActionResult SalesPromoterTargetDataExcelUpload()
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
                        List<SalesPromoterTargetData> lstTargetSales = new List<SalesPromoterTargetData>();
                        if (finalRecords.Columns.Count == 7)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                string MONTH = Convert.ToString(finalRecords.Rows[i][1]);
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
                                    SalesPromoterTargetData targ = new SalesPromoterTargetData();
                                    targ.SalesPromotorCodevtxt = finalRecords.Rows[i][0].ToString();
                                    targ.DealerTargetApptint = Convert.ToInt32(finalRecords.Rows[i][2].ToString());
                                    targ.DealerActualApptint = Convert.ToInt32(finalRecords.Rows[i][3]);
                                    targ.Monthvtxt = MONTH;
                                    targ.RetailerTargetApptint = Convert.ToInt32(finalRecords.Rows[i][4].ToString());
                                    targ.RetailerActualApptint = Convert.ToInt32(finalRecords.Rows[i][5].ToString());
                                    targ.CreatedByvtxt = "SA003";
                                    targ.Yeartxt= Convert.ToString(finalRecords.Rows[i][6].ToString());
                                    lstTargetSales.Add(targ);
                                }
                            }
                            if (lstTargetSales.Count > 0)
                            {
                                _SalesPromoterTargetDataServices.DeleteSalesPromoterTargetData();
                                for (int k = 0; k < lstTargetSales.Count; k++)
                                {
                                    long j = _SalesPromoterTargetDataServices.InsertSalesPromoterTargetDataIntoTempTable(lstTargetSales[k]);
                                }
                                _SalesPromoterTargetDataServices.InsertSalesPromoterTargetDataIntoMainTable();
                                List<SalesPromoterTargetData> lst = new List<SalesPromoterTargetData>();
                                lst = _SalesPromoterTargetDataServices.GetTempSalesPromoterTargetData();
                                if (lst.Count > 0)
                                {
                                    return Ok("Error in Uploaded File");
                                }
                            }
                        }
                        else
                        {
                            return Ok("Please Select Profer file to upload.");
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

        //use for Download Error File
        [HttpGet("DownloadErrorSalesPromoterTargetData")]
        public IActionResult DownloadErrorSalesPromoterTargetData()
        {
            try
            {
                List<SalesPromoterTargetData> sales = _SalesPromoterTargetDataServices.GetTempSalesPromoterTargetData();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SalesHierachy");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sales Promoter Code";
                    worksheet.Cell(currentRow, 2).Value = "Month";
                    worksheet.Cell(currentRow, 3).Value = "Dealer Target Appointment Count";
                    worksheet.Cell(currentRow, 4).Value = "Dealer Actual Appointment Count";
                    worksheet.Cell(currentRow, 5).Value = "Retailer Target Appointment Count";
                    worksheet.Cell(currentRow, 6).Value = "Retailer Actual Appointment Count";
                    worksheet.Cell(currentRow, 7).Value = "Remarks";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.SalesPromotorCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.Monthvtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.DealerTargetApptint;
                        worksheet.Cell(currentRow, 4).Value = sale.DealerActualApptint;
                        worksheet.Cell(currentRow, 5).Value = sale.RetailerTargetApptint;
                        worksheet.Cell(currentRow, 6).Value = sale.RetailerActualApptint;
                        worksheet.Cell(currentRow, 7).Value = sale.Remarks;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ErrorSalesPromoterTargetData.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // use for List of Dealer Appointment data
        [HttpGet("SalesPromoterTargetData/{pageNo},{pageSize},{keyword}")]
        public IActionResult SalesPromoterTargetData(int pageNo, int pageSize, string keyword)
        {
            try
            {
                return Ok(_SalesPromoterTargetDataServices.GetSalesPromoterTargetDataList(pageNo, pageSize, keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // use for List of Dealer Appointment data
        [HttpGet("SalesPromoterTargetDataCount/{keyword}")]
        public IActionResult SalesPromoterTargetDataCount(string keyword)
        {
            try
            {
                long count = _SalesPromoterTargetDataServices.GetSalesPromoterTargetDataList(0, 0, keyword).Count;
                return Ok(count);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // use for Sales Promoter Dashboard
        [HttpGet("SalesPromoterTargetDataForDashboard/{Usercode},{usertype}")]
        public IActionResult SalesPromoterTargetDataForDashboard(string Usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), Usercode))
                {
                    return Ok(_SalesPromoterTargetDataServices.GetSalesPromoterTargetDataInDashboard(Usercode, usertype));
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