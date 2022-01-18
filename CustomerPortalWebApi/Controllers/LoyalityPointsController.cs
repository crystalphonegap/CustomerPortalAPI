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
    public class LoyalityPointsController : ControllerBase
    {
        private readonly ILoyalityPointsService _LoyalityPointsService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public LoyalityPointsController(ILoyalityPointsService LoyalityPointsService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _LoyalityPointsService = LoyalityPointsService;
            _Checktokenservice = checktokenservice;
        }

        [HttpPost("LoyalityPointsExcelUpload/{date}"), DisableRequestSizeLimit]
        public IActionResult LoyalityPointsExcelUpload(string date)
        {
            try
            {
                DateTime fdate = DateTime.ParseExact(date, "dd-MM-yyyy", null);
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
                        List<LoyalityPointsModel> lstmodel = new List<LoyalityPointsModel>();
                        for (int i = 1; i < finalRecords.Rows.Count; i++)
                        {
                                LoyalityPointsModel model = new LoyalityPointsModel();
                                model.CustomerCodevtxt = finalRecords.Rows[i][0].ToString();
                                model.CustomerNamevtxt = finalRecords.Rows[i][1].ToString();
                                model.EarnPoints = Convert.ToInt64(finalRecords.Rows[i][2]);
                                model.UtilizePoints = Convert.ToInt64(finalRecords.Rows[i][3]);
                                model.Divisionvtxt = "Cement";
                                model.CreatedByvtxt = "SA003";
                                model.TillDateDatetime = fdate;
                                lstmodel.Add(model);
                        }
                        if (lstmodel.Count > 0)
                        {
                            for (int k = 0; k < lstmodel.Count; k++)
                            {
                                long j = _LoyalityPointsService.InsertLoyalityPoints(lstmodel[k]);
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
                List<LoyalityPointsModel> model = _LoyalityPointsService.DownloadLoyalityPoints(division, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("LoyalityPoints");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Customer Code";
                    worksheet.Cell(currentRow, 3).Value = "Customer Name";
                    worksheet.Cell(currentRow, 6).Value = "Earn Points";
                    worksheet.Cell(currentRow, 7).Value = "Utilize Points";
                    foreach (var rw in model)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = rw.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 3).Value = rw.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 4).Value = rw.EarnPoints;
                        worksheet.Cell(currentRow, 5).Value = rw.UtilizePoints;
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

        [HttpGet("DownloadSampleExcel")]
        public IActionResult DownloadSampleExcel()
        {
            try
            {
                List<LoyalityPointsModel> sales = new List<LoyalityPointsModel>();
                LoyalityPointsModel model = new LoyalityPointsModel();
                model.CustomerCodevtxt = "";
                model.CustomerNamevtxt = "";
                model.EarnPoints = 0;
                model.UtilizePoints = 0;
                sales.Add(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("LoyalityPoints");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Customer Code";
                    worksheet.Cell(currentRow, 2).Value = "Customer Name";
                    worksheet.Cell(currentRow, 3).Value = "Earn Points";
                    worksheet.Cell(currentRow, 4).Value = "Utilize Points";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.EarnPoints;
                        worksheet.Cell(currentRow, 4).Value = sale.UtilizePoints;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleLoyalityPoints.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetLoyalityPointsLists/{Division},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetLoyalityPointsLists(string Division, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_LoyalityPointsService.GetLoyalityPoints(Division, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetLoyalityPointsCount/{division},{KeyWord}")]
        public long GetLoyalityPointsCount(string division, string KeyWord)
        {
            try
            {
                return _LoyalityPointsService.GetLoyalityPointsListCount(division, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetLoyalityPointsDashboard/{customercode}")]
        public IActionResult GetLoyalityPointsDashboard(string customercode)
        {
            try
            {
                return Ok(_LoyalityPointsService.GetLoyalityPointsDashboard(customercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
    }
}