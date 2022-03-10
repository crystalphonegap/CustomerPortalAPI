using ClosedXML.Excel;
using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class LoginReportController : ControllerBase
    {
        private readonly ILoginReportService _ILoginReportService;
        private readonly ILogger _ILogger;

        public LoginReportController(ILogger ILoggerservice, ILoginReportService ILoginReportService)
        {
            _ILogger = ILoggerservice;
            _ILoginReportService = ILoginReportService;
        }

        [HttpGet("GetArea/{Type},{KeyWord}")]
        public IActionResult GetArea(string Type, string KeyWord)
        {
            try
            {
                return Ok(_ILoginReportService.GetArea(Type, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

     

        [HttpPost("GetAreaNew")]
        public IActionResult GetAreaNew(terClass model)
        {
            try
            {
                return Ok(_ILoginReportService.GetArea(model.Type, model.KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("LoginReport/{FromDate},{Todate},{pageNo},{pageSize},{Keyword}")]
        public IActionResult LoginReport(string FromDate, string Todate, int pageNo, int pageSize, string Keyword)
        {
            try
            {
                return Ok(_ILoginReportService.LoginReport(FromDate, Todate, pageNo, pageSize, Keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("LoginReportCount/{FromDate},{Todate},{Type},{Keyword}")]
        public IActionResult LoginReportCount(string FromDate, string Todate, string Type, string Keyword)
        {
            try
            {
                return Ok(_ILoginReportService.LoginReportCount(FromDate, Todate, Type, Keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("LoginReportDownload/{FromDate},{Todate},{Keyword}")]
        public IActionResult LoginReportDownload(string FromDate, string Todate, string Keyword)
        {
            try
            {
                List<UserMaster> userlist = _ILoginReportService.LoginReportDownload(FromDate, Todate, Keyword);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Region";
                    worksheet.Cell(currentRow, 3).Value = "Branch";
                    worksheet.Cell(currentRow, 4).Value = "Territory";
                    worksheet.Cell(currentRow, 5).Value = "User Code";
                    worksheet.Cell(currentRow, 6).Value = "User Name";
                    worksheet.Cell(currentRow, 7).Value = "User Type";
                    worksheet.Cell(currentRow, 8).Value = "Date";
                    //worksheet.Cell(currentRow, 6).Value = "System Information";
                    //worksheet.Cell(currentRow, 7).Value = "IP Address";
                    foreach (var users in userlist)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = users.Region;
                        worksheet.Cell(currentRow, 3).Value = users.Branch;
                        worksheet.Cell(currentRow, 4).Value = users.Territory;
                        worksheet.Cell(currentRow, 5).Value = users.UserCodetxt;
                        worksheet.Cell(currentRow, 6).Value = users.UserNametxt;
                        worksheet.Cell(currentRow, 7).Value = users.UserTypetxt;
                        worksheet.Cell(currentRow, 8).Value = users.CreatedDatedatetime;
                        //worksheet.Cell(currentRow, 6).Value = users.BrowserName;
                        //worksheet.Cell(currentRow, 7).Value = users.IpAddress;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "UserList.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("NewLoginReport")]
        public IActionResult NewLoginReport(LoginReportFilterModel model)
        {
            try
            {
                return Ok(_ILoginReportService.NewLoginReport(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("NewLoginReportDownload/{FromDate},{Todate},{Zone},{Region},{Branch},{Territory},{UserType},{Type},{Search}")]
        public IActionResult NewLoginReportDownload(string FromDate, string Todate, string Zone, string Region, string Branch, string Territory, string UserType, string Type, string Search)
        {
            try
            {
                LoginReportFilterModel model = new LoginReportFilterModel()
                {
                    fromDate = FromDate,
                    todate = Todate,
                    Zone = Zone,
                    Region = Region,
                    Branch = Branch,
                    Territory = Territory,
                    UserType = UserType,
                    Type = Type,
                    Search = Search
                };
                DataTable userlist = _ILoginReportService.NewLoginReportDownload(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Region";
                    worksheet.Cell(currentRow, 3).Value = "Branch";
                    worksheet.Cell(currentRow, 4).Value = "Territory";
                    worksheet.Cell(currentRow, 5).Value = "User Code";
                    worksheet.Cell(currentRow, 6).Value = "User Name";
                    worksheet.Cell(currentRow, 7).Value = "User Type";
                    int u = 7;
                    for (int k = 8; k < userlist.Columns.Count; k++)
                    {
                        u = u + 1;
                        worksheet.Cell(currentRow, u).Value = userlist.Columns[k].ColumnName;
                    }

                    for (int i = 0; i < userlist.Rows.Count; i++)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = userlist.Rows[i][2];
                        worksheet.Cell(currentRow, 3).Value = userlist.Rows[i][3];
                        worksheet.Cell(currentRow, 4).Value = userlist.Rows[i][4];
                        worksheet.Cell(currentRow, 5).Value = userlist.Rows[i][5];
                        worksheet.Cell(currentRow, 6).Value = userlist.Rows[i][6];
                        worksheet.Cell(currentRow, 7).Value = userlist.Rows[i][7];
                        int j = 7;
                        for (int z = 8; z < userlist.Columns.Count; z++)
                        {
                            j = j + 1;
                            int value = 0;
                            if (Convert.ToInt32(userlist.Rows[i][z]) == 0)
                            {
                                value = 0;
                            }
                            else
                            {
                                value = 1;
                            }
                            worksheet.Cell(currentRow, j).Value = value;
                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "LoginReport.xlsx");
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