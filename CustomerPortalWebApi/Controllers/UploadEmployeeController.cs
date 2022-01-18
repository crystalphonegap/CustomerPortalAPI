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
    public class UploadEmployeeController : ControllerBase
    {
        private readonly IUploadEmployeeService _UploadEmployeeService;
        private readonly ILogger _ILogger;

        public UploadEmployeeController(IUploadEmployeeService UploadEmployeeService, ILogger ILoggerservice)
        {
            _ILogger = ILoggerservice;
            _UploadEmployeeService = UploadEmployeeService;
        }

        //use for upload sales hierachy
        [HttpPost("SalesHierachyUpload"), DisableRequestSizeLimit]
        public IActionResult SalesHierachyUpload()
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
                        List<SalesHierachy> lstSalesHierachy = new List<SalesHierachy>();
                        if (finalRecords.Columns.Count == 12)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                SalesHierachy sales = new SalesHierachy();
                                sales.SalesOfficeCodevtxt = finalRecords.Rows[i][0].ToString();
                                sales.SalesOfficeNamevtxt = finalRecords.Rows[i][1].ToString();
                                sales.BranchCodevtxt = finalRecords.Rows[i][2].ToString();
                                sales.BranchNamevtxt = finalRecords.Rows[i][3].ToString();
                                sales.RegionCodevtxt = finalRecords.Rows[i][4].ToString();
                                sales.RegionDescriptionvtxt = finalRecords.Rows[i][5].ToString();
                                sales.ZoneCodevtxt = finalRecords.Rows[i][6].ToString();
                                sales.ZoneDescriptionvtxt = finalRecords.Rows[i][7].ToString();
                                sales.HODCodevtxt = finalRecords.Rows[i][8].ToString();
                                sales.HODNamevtxt = finalRecords.Rows[i][9].ToString();
                                sales.CompanyCodevtxt = finalRecords.Rows[i][10].ToString();
                                sales.CompanyNamevtxt = finalRecords.Rows[i][11].ToString();
                                lstSalesHierachy.Add(sales);
                            }
                            if (lstSalesHierachy.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempSaleshierachy();
                                for (int k = 0; k < lstSalesHierachy.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertSalesHierachyIntoTempTable(lstSalesHierachy[k]);
                                }
                                _UploadEmployeeService.InsertSalesHierachyIntoMainTable();
                                List<SalesHierachy> lst = new List<SalesHierachy>();
                                lst = _UploadEmployeeService.GetTempSalesHierachy();
                                if (lst.Count > 0)
                                {
                                    return Ok("Error in Uploaded File");
                                }
                            }
                        }
                        else
                        {
                            return Ok("Please Select Proper file to upload.");
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

        //use for downlaod sample sales hierachy
        [HttpGet("DownloadSampleSalesHierachyExcel")]
        public IActionResult DownloadSampleSalesHierachyExcel()
        {
            try
            {
                List<SalesHierachy> lstsales = new List<SalesHierachy>();
                SalesHierachy sales = new SalesHierachy();
                sales.SalesOfficeCodevtxt = "";
                sales.SalesOfficeNamevtxt = "";
                sales.BranchCodevtxt = "";
                sales.BranchNamevtxt = "";
                sales.RegionCodevtxt = "";
                sales.RegionDescriptionvtxt = "";
                sales.ZoneCodevtxt = "";
                sales.ZoneDescriptionvtxt = "";
                sales.HODCodevtxt = "";
                sales.HODNamevtxt = "";
                sales.CompanyCodevtxt = "";
                sales.CompanyNamevtxt = "";
                lstsales.Add(sales);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SalesHierachy");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "SalesOffice Code";
                    worksheet.Cell(currentRow, 2).Value = "SalesOffice Name";
                    worksheet.Cell(currentRow, 3).Value = "Branch Code";
                    worksheet.Cell(currentRow, 4).Value = "Branch Name";
                    worksheet.Cell(currentRow, 5).Value = "Region Code";
                    worksheet.Cell(currentRow, 6).Value = "Region Name";
                    worksheet.Cell(currentRow, 7).Value = "Zone Code";
                    worksheet.Cell(currentRow, 8).Value = "Zone Name";
                    worksheet.Cell(currentRow, 9).Value = "HOD Code";
                    worksheet.Cell(currentRow, 10).Value = "HOD Name";
                    worksheet.Cell(currentRow, 11).Value = "Company Code";
                    worksheet.Cell(currentRow, 12).Value = "Company Name";
                    foreach (var sale in lstsales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.SalesOfficeCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.SalesOfficeNamevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.BranchCodevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.BranchNamevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.RegionCodevtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.RegionDescriptionvtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.ZoneCodevtxt;
                        worksheet.Cell(currentRow, 8).Value = sale.ZoneDescriptionvtxt;
                        worksheet.Cell(currentRow, 9).Value = sale.HODCodevtxt;
                        worksheet.Cell(currentRow, 10).Value = sale.HODNamevtxt;
                        worksheet.Cell(currentRow, 11).Value = sale.CompanyCodevtxt;
                        worksheet.Cell(currentRow, 12).Value = sale.CompanyNamevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleSalesHierachy.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for salesheirachy count list
        [HttpGet("GetAllSalesHierachyListsCount/{KeyWord}")]
        public long GetAllSalesHierachyListsCount(string KeyWord)
        {
            try
            {
                return _UploadEmployeeService.GetSalesHierachyCount(KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Sales Heirachy Search
        [HttpGet("GetAllSalesHeirachy/{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetAllSalesHeirachy(int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_UploadEmployeeService.GetSalesHierachy(PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for sales hierachy export to excel
        [HttpGet("SalesHierachyExportToExcel/{KeyWord}")]
        public IActionResult SalesHierachyExportToExcel(string KeyWord)
        {
            try
            {
                List<SalesHierachy> sales = _UploadEmployeeService.DownloadSalesHierachy(KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SalesHierachy");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "SalesOffice Code";
                    worksheet.Cell(currentRow, 2).Value = "SalesOffice Name";
                    worksheet.Cell(currentRow, 3).Value = "Branch Code";
                    worksheet.Cell(currentRow, 4).Value = "Branch Name";
                    worksheet.Cell(currentRow, 5).Value = "Region Code";
                    worksheet.Cell(currentRow, 6).Value = "Region Name";
                    worksheet.Cell(currentRow, 7).Value = "Zone Code";
                    worksheet.Cell(currentRow, 8).Value = "Zone Name";
                    worksheet.Cell(currentRow, 9).Value = "HOD Code";
                    worksheet.Cell(currentRow, 10).Value = "HOD Name";
                    worksheet.Cell(currentRow, 11).Value = "Company Code";
                    worksheet.Cell(currentRow, 12).Value = "Company Name";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.SalesOfficeCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.SalesOfficeNamevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.BranchCodevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.BranchNamevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.RegionCodevtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.RegionDescriptionvtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.ZoneCodevtxt;
                        worksheet.Cell(currentRow, 8).Value = sale.ZoneDescriptionvtxt;
                        worksheet.Cell(currentRow, 9).Value = sale.HODCodevtxt;
                        worksheet.Cell(currentRow, 10).Value = sale.HODNamevtxt;
                        worksheet.Cell(currentRow, 11).Value = sale.CompanyCodevtxt;
                        worksheet.Cell(currentRow, 12).Value = sale.CompanyNamevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SalesHierachy.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for sales hierachy download error file
        [HttpGet("DownloadErrorSalesHierachy")]
        public IActionResult DownloadErrorSalesHierachy()
        {
            try
            {
                List<SalesHierachy> sales = _UploadEmployeeService.GetTempSalesHierachy();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("SalesHierachy");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "SalesOffice Code";
                    worksheet.Cell(currentRow, 2).Value = "SalesOffice Name";
                    worksheet.Cell(currentRow, 3).Value = "Branch Code";
                    worksheet.Cell(currentRow, 4).Value = "Branch Name";
                    worksheet.Cell(currentRow, 5).Value = "Region Code";
                    worksheet.Cell(currentRow, 6).Value = "Region Name";
                    worksheet.Cell(currentRow, 7).Value = "Zone Code";
                    worksheet.Cell(currentRow, 8).Value = "Zone Name";
                    worksheet.Cell(currentRow, 9).Value = "HOD Code";
                    worksheet.Cell(currentRow, 10).Value = "HOD Name";
                    worksheet.Cell(currentRow, 11).Value = "Company Code";
                    worksheet.Cell(currentRow, 12).Value = "Company Name";
                    worksheet.Cell(currentRow, 13).Value = "Remarks";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.SalesOfficeCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.SalesOfficeNamevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.BranchCodevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.BranchNamevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.RegionCodevtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.RegionDescriptionvtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.ZoneCodevtxt;
                        worksheet.Cell(currentRow, 8).Value = sale.ZoneDescriptionvtxt;
                        worksheet.Cell(currentRow, 9).Value = sale.HODCodevtxt;
                        worksheet.Cell(currentRow, 10).Value = sale.HODNamevtxt;
                        worksheet.Cell(currentRow, 11).Value = sale.CompanyCodevtxt;
                        worksheet.Cell(currentRow, 12).Value = sale.CompanyNamevtxt;
                        worksheet.Cell(currentRow, 13).Value = sale.Remarks;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SalesHierachy.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //USE FOR TSI upload
        [HttpPost("TSIUpload"), DisableRequestSizeLimit]
        public IActionResult TSIUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 5)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = finalRecords.Rows[i][4].ToString();
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Territory Sales Executive";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Territory Sales Executive");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Territory Sales Executive");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Territory Sales Executive");
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

        //USE FOR BM upload

        [HttpPost("BMUpload"), DisableRequestSizeLimit]
        public IActionResult BMUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 5)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = finalRecords.Rows[i][4].ToString();
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Branch Manager";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Branch Manager");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Branch Manager");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Branch Manager");
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

        //USE FOR RM upload
        [HttpPost("RMUpload"), DisableRequestSizeLimit]
        public IActionResult RMUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 5)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = finalRecords.Rows[i][4].ToString();
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Regional Manager";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Regional Manager");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Regional Manager");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Regional Manager");
                                if (lst.Count > 0)
                                {
                                    return Ok("Error in Uploaded File");
                                }
                            }
                        }
                        else
                        {
                            return Ok("Please Select Proper file to upload.");
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

        //USE FOR KAM upload
        [HttpPost("KAMUpload"), DisableRequestSizeLimit]
        public IActionResult KAMUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 5)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = finalRecords.Rows[i][4].ToString();
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "KAM";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("KAM");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTempkAM(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMainkAM("KAM");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("KAM");
                                if (lst.Count > 0)
                                {
                                    return Ok("Error in Uploaded File");
                                }
                            }
                        }
                        else
                        {
                            return Ok("Please Select Proper file to upload.");
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


        //USE FOR ZM upload
        [HttpPost("ZMUpload"), DisableRequestSizeLimit]
        public IActionResult ZMUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 5)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = finalRecords.Rows[i][4].ToString();
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Zonal Manager";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Zonal Manager");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Zonal Manager");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Zonal Manager");
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

        //use for OrderAnalyst upload
        [HttpPost("OrderAnalystUpload"), DisableRequestSizeLimit]
        public IActionResult OrderAnalystUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 4)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = "";
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Order Analyst";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Order Analyst");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Order Analyst");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Order Analyst");
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

        //use for MarketingHead upload
        [HttpPost("MarketingHeadUpload"), DisableRequestSizeLimit]
        public IActionResult MarketingHeadUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 4)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = "";
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Marketing Head";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Marketing Head");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Marketing Head");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Marketing Head");
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

        //use for AccountingHead upload
        [HttpPost("AccountingHeadUpload"), DisableRequestSizeLimit]
        public IActionResult AccountingHeadUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 4)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = "";
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Accounting Head";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Accounting Head");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Accounting Head");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Accounting Head");
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

        //use for CF Agent
        [HttpPost("CFAgentUpload"), DisableRequestSizeLimit]
        public IActionResult CFAgentUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 4)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = "";
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "CF Agent";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("CF Agent");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("CF Agent");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("CF Agent");
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

        //use for SalesPromoter
        [HttpPost("SalesPromoterUpload"), DisableRequestSizeLimit]
        public IActionResult SalesPromoterUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 4)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = "";
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Sales Promoter";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Sales Promoter");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Sales Promoter");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Sales Promoter");
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

        //use for downlaod sample TSI,BM,ZM,RM,KAM
        [HttpGet("DownloadSampleEmployeeUploadExcel/{strusertype}")]
        public IActionResult DownloadSampleEmployeeUploadExcel(string strusertype)
        {
            try
            {
                List<UploadEmployeeModel> lstemp = new List<UploadEmployeeModel>();
                UploadEmployeeModel emp = new UploadEmployeeModel();
                emp.UserCodetxt = "";
                emp.UserNametxt = "";
                emp.Mobilevtxt = "";
                emp.Emailvtxt = "";
                emp.ParentCode = "";
                lstemp.Add(emp);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("EmployeeUpload");
                    var currentRow = 1;
                    if (strusertype == "Territory Sales Executive")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Sales Executive Code";
                        worksheet.Cell(currentRow, 2).Value = "Sales Executive Name";
                        worksheet.Cell(currentRow, 3).Value = "Email";
                        worksheet.Cell(currentRow, 4).Value = "Mobile";
                        worksheet.Cell(currentRow, 5).Value = "Sales Office Code";
                    }
                    else if (strusertype == "Branch Manager")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Branch Manager Code";
                        worksheet.Cell(currentRow, 2).Value = "Branch Manager Name";
                        worksheet.Cell(currentRow, 3).Value = "Email";
                        worksheet.Cell(currentRow, 4).Value = "Mobile";
                        worksheet.Cell(currentRow, 5).Value = "Branch Code";
                    }
                    else if (strusertype == "Regional Manager")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Regional Manager Code";
                        worksheet.Cell(currentRow, 2).Value = "Regional Manager Name";
                        worksheet.Cell(currentRow, 3).Value = "Email";
                        worksheet.Cell(currentRow, 4).Value = "Mobile";
                        worksheet.Cell(currentRow, 5).Value = "Region Code";
                    }
                    else if (strusertype == "KAM")
                    {
                        worksheet.Cell(currentRow, 1).Value = "KAM Code";
                        worksheet.Cell(currentRow, 2).Value = "KAM Name";
                        worksheet.Cell(currentRow, 3).Value = "Email";
                        worksheet.Cell(currentRow, 4).Value = "Mobile";
                        worksheet.Cell(currentRow, 5).Value = "Region Code";
                    }
                    else if (strusertype == "Zonal Manager")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Zonal Manager Code";
                        worksheet.Cell(currentRow, 2).Value = "Zonal Manager Name";
                        worksheet.Cell(currentRow, 3).Value = "Email";
                        worksheet.Cell(currentRow, 4).Value = "Mobile";
                        worksheet.Cell(currentRow, 5).Value = "Zone Code";
                    }
                    foreach (var emps in lstemp)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = emps.UserCodetxt;
                        worksheet.Cell(currentRow, 2).Value = emps.UserNametxt;
                        worksheet.Cell(currentRow, 3).Value = emps.Emailvtxt;
                        worksheet.Cell(currentRow, 4).Value = emps.Mobilevtxt;
                        worksheet.Cell(currentRow, 5).Value = emps.ParentCode;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleEmployeeUpload.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for downlaod sample OrderAnalyst,Marketing Head,Accounting Head,Retailer
        [HttpGet("DownloadSampleEmployeeUploadExcelForNoParentCode/{strusertype}")]
        public IActionResult DownloadSampleEmployeeUploadExcelForNoParentCode(string strusertype)
        {
            try
            {
                List<UploadEmployeeModel> lstemp = new List<UploadEmployeeModel>();
                UploadEmployeeModel emp = new UploadEmployeeModel();
                emp.UserCodetxt = "";
                emp.UserNametxt = "";
                emp.Mobilevtxt = "";
                emp.Emailvtxt = "";
                emp.ParentCode = "";
                lstemp.Add(emp);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("EmployeeUpload");
                    var currentRow = 1;

                    if (strusertype == "Accounting Head")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Accounting Head Code";
                        worksheet.Cell(currentRow, 2).Value = "Accounting Head Name";
                    }
                    if (strusertype == "Marketing Head")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Marketing Head Code";
                        worksheet.Cell(currentRow, 2).Value = "Marketing Head Name";
                    }
                    if (strusertype == "CF Agent")
                    {
                        worksheet.Cell(currentRow, 1).Value = "CF Agent Code";
                        worksheet.Cell(currentRow, 2).Value = "CF Agent Name";
                    }
                    if (strusertype == "Sales Promoter")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Sales Promoter Code";
                        worksheet.Cell(currentRow, 2).Value = "Sales Promoter Name";
                    }
                    if (strusertype == "Order Analyst")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Order Analyst Code";
                        worksheet.Cell(currentRow, 2).Value = "Order Analyst Name";
                    }
                    if (strusertype == "Retailer")
                    {
                        worksheet.Cell(currentRow, 1).Value = "Retailer Code";
                        worksheet.Cell(currentRow, 2).Value = "Retailer Name";
                    }
                    worksheet.Cell(currentRow, 3).Value = "Email";
                    worksheet.Cell(currentRow, 4).Value = "Mobile";
                    foreach (var emps in lstemp)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = emps.UserCodetxt;
                        worksheet.Cell(currentRow, 2).Value = emps.UserNametxt;
                        worksheet.Cell(currentRow, 3).Value = emps.Emailvtxt;
                        worksheet.Cell(currentRow, 4).Value = emps.Mobilevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleEmployeeUpload.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for downlaod error file of TSI,BM,ZM,RM,KAM
        [HttpGet("DownloadErrorEmployeeUpload/{strusertype}")]
        public IActionResult DownloadErrorEmployeeUpload(string strusertype)
        {
            try
            {
                List<UploadEmployeeModel> emps = _UploadEmployeeService.GetTempUserMaster(strusertype);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ErrorEmployeeUpload");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "SrNo";
                    if (strusertype == "Territory Sales Executive")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Sales Executive Code";
                        worksheet.Cell(currentRow, 3).Value = "Sales Executive Name";
                        worksheet.Cell(currentRow, 4).Value = "Email";
                        worksheet.Cell(currentRow, 5).Value = "Mobile";
                        worksheet.Cell(currentRow, 6).Value = "Sales Office Code";
                    }
                    else if (strusertype == "Branch Manager")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Branch Manager Code";
                        worksheet.Cell(currentRow, 3).Value = "Branch Manager Name";
                        worksheet.Cell(currentRow, 4).Value = "Email";
                        worksheet.Cell(currentRow, 5).Value = "Mobile";
                        worksheet.Cell(currentRow, 6).Value = "Branch Code";
                    }
                    else if (strusertype == "Regional Manager")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Regional Manager Code";
                        worksheet.Cell(currentRow, 3).Value = "Regional Manager Name";
                        worksheet.Cell(currentRow, 4).Value = "Email";
                        worksheet.Cell(currentRow, 5).Value = "Mobile";
                        worksheet.Cell(currentRow, 6).Value = "Region Code";
                    }
                    else if (strusertype == "KAM")
                    {
                        worksheet.Cell(currentRow, 2).Value = "KAM Code";
                        worksheet.Cell(currentRow, 3).Value = "KAM Name";
                        worksheet.Cell(currentRow, 4).Value = "Email";
                        worksheet.Cell(currentRow, 5).Value = "Mobile";
                        worksheet.Cell(currentRow, 6).Value = "Region Code";
                    }
                    else if (strusertype == "Zonal Manager")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Zonal Manager Code";
                        worksheet.Cell(currentRow, 3).Value = "Zonal Manager Name";
                        worksheet.Cell(currentRow, 4).Value = "Email";
                        worksheet.Cell(currentRow, 5).Value = "Mobile";
                        worksheet.Cell(currentRow, 6).Value = "Zone Code";
                    }
                    worksheet.Cell(currentRow, 7).Value = "Remarks";
                    foreach (var emp in emps)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = emp.UserCodetxt;
                        worksheet.Cell(currentRow, 3).Value = emp.UserNametxt;
                        worksheet.Cell(currentRow, 4).Value = emp.Emailvtxt;
                        worksheet.Cell(currentRow, 5).Value = emp.Mobilevtxt;
                        worksheet.Cell(currentRow, 6).Value = emp.ParentCode;
                        worksheet.Cell(currentRow, 7).Value = emp.Remarks;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ErrorEMployeeUpload.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for downlaod error file of OrderAnalyst,Marketing Head,Accounting Head,Retailer
        [HttpGet("DownloadErrorEmployeeUploadforNoParentCode/{strusertype}")]
        public IActionResult DownloadErrorEmployeeUploadforNoParentCode(string strusertype)
        {
            try
            {
                List<UploadEmployeeModel> emps = _UploadEmployeeService.GetTempUserMaster(strusertype);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ErrorEmployeeUpload");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "SrNo";
                    if (strusertype == "Accounting Head")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Accounting Head Code";
                        worksheet.Cell(currentRow, 3).Value = "Accounting Head Name";
                    }
                    if (strusertype == "Marketing Head")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Marketing Head Code";
                        worksheet.Cell(currentRow, 3).Value = "Marketing Head Name";
                    }
                    if (strusertype == "CF Agent")
                    {
                        worksheet.Cell(currentRow, 2).Value = "CF Agent Code";
                        worksheet.Cell(currentRow, 3).Value = "CF Agent Name";
                    }
                    if (strusertype == "Sales Promoter")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Sales Promoter Code";
                        worksheet.Cell(currentRow, 3).Value = "Sales Promoter Name";
                    }
                    if (strusertype == "Order Analyst")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Order Analyst Code";
                        worksheet.Cell(currentRow, 3).Value = "Order Analyst Name";
                    }
                    if (strusertype == "Retailer")
                    {
                        worksheet.Cell(currentRow, 2).Value = "Retailer Code";
                        worksheet.Cell(currentRow, 3).Value = "Statusvtxt Name";
                    }
                    worksheet.Cell(currentRow, 4).Value = "Email";
                    worksheet.Cell(currentRow, 5).Value = "Mobile";
                    worksheet.Cell(currentRow, 6).Value = "Remarks";
                    foreach (var emp in emps)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = emp.UserCodetxt;
                        worksheet.Cell(currentRow, 3).Value = emp.UserNametxt;
                        worksheet.Cell(currentRow, 4).Value = emp.Emailvtxt;
                        worksheet.Cell(currentRow, 5).Value = emp.Mobilevtxt;
                        worksheet.Cell(currentRow, 6).Value = emp.Remarks;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ErrorEMployeeUpload.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Get SalesOffices
        [HttpGet("GetSalesOffices")]
        public IActionResult GetSalesOffices()
        {
            try
            {
                return Ok(_UploadEmployeeService.GetSalesOffices());
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Get OrderAnalyst Mapping Data
        [HttpGet("GetOrderAnalystMapppingData/{usercode}")]
        public IActionResult GetOrderAnalystMapppingData(string usercode)
        {
            try
            {
                return Ok(_UploadEmployeeService.GetOrderAnalystMappingData(usercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for InsertOrderAnalystData
        [HttpPost("InsertOrderAnalystData")]
        public IActionResult InsertOrderAnalystMappingData(OrderAnalystMappingModel model)
        {
            try
            {
                var MappingData = new OrderAnalystMappingModel()
                {
                    UserCodevtxt = model.UserCodevtxt,
                    CustomerTypevtxt = model.CustomerTypevtxt,
                    SalesOfficeCodevtxt = model.SalesOfficeCodevtxt,
                    SalesOfficeNamevtxt = model.SalesOfficeNamevtxt,
                    Createdbyvtxt = model.Createdbyvtxt,
                    CreatedDatetimedatetime = DateTime.Now
                };
                _UploadEmployeeService.InsertOrderAnalystMapping(MappingData);
                return Ok(MappingData);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for delete OrderAnalystData
        [HttpDelete("Delete/{usercode}")]
        public IActionResult Delete(string usercode)
        {
            try
            {
                _UploadEmployeeService.Delete(usercode);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Retailer Upload
        [HttpPost("RetailerUpload"), DisableRequestSizeLimit]
        public IActionResult RetailerUpload()
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
                        List<UploadEmployeeModel> lstEMployees = new List<UploadEmployeeModel>();
                        if (finalRecords.Columns.Count == 4)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                UploadEmployeeModel empmodel = new UploadEmployeeModel();
                                empmodel.UserCodetxt = finalRecords.Rows[i][0].ToString();
                                empmodel.UserNametxt = finalRecords.Rows[i][1].ToString();
                                empmodel.Emailvtxt = finalRecords.Rows[i][2].ToString();
                                empmodel.Mobilevtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.ParentCode = "";
                                empmodel.Passwordvtxt = "1234";
                                empmodel.UserTypetxt = "Retailer";
                                empmodel.Divisionvtxt = "Cement";
                                empmodel.CreatedByint = 1;
                                empmodel.CreatedDatedatetime = DateTime.Now;
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _UploadEmployeeService.DeleteTempUserMaster("Retailer");
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _UploadEmployeeService.InsertUserMasterIntoTemp(lstEMployees[k]);
                                }
                                _UploadEmployeeService.InsertUserMasterIntoMain("Retailer");
                                List<UploadEmployeeModel> lst = new List<UploadEmployeeModel>();
                                lst = _UploadEmployeeService.GetTempUserMaster("Retailer");
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
    }
}