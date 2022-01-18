using ClosedXML.Excel;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BalanceConfirmationController : ControllerBase
    {
        private readonly IBalanceConfirmationService _BalanceConfirmationService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public BalanceConfirmationController(IHostingEnvironment environment, IBalanceConfirmationService BalanceConfirmationService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _BalanceConfirmationService = BalanceConfirmationService;
            _Checktokenservice = checktokenservice;
            _hostingEnvironment = environment;
        }

        //use for Get Request No
        [HttpGet("GetReqOrderNo")]
        public IActionResult GetReqOrderNo()
        {
            try
            {
                string OrdNo = "";
                OrdNo = _BalanceConfirmationService.GetOrderNo();
                OrdNo = GetReqNo(OrdNo);
                return Ok(_BalanceConfirmationService.GetReqOrderNo(OrdNo));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for download sample BalanceConfirmation
        [HttpGet("DownloadSampleBalanceConf")]
        public IActionResult DownloadSampleBalanceConf()
        {
            try
            {
                List<BalConfirmationModel> lstBalconf = new List<BalConfirmationModel>();
                BalConfirmationModel Balconf = new BalConfirmationModel();
                Balconf.DealerCodevtxt = "";
                lstBalconf.Add(Balconf);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("BalanceConfirmation");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Dealer Code";
                    foreach (var balconf in lstBalconf)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = balconf.DealerCodevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SamplebalanceConfirmation.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for upload BalanceConfirmation
        [HttpPost("UploadBalanceConfirmation/{fromdate},{todate},{expirydate},{createdby},{Type}"), DisableRequestSizeLimit]
        public IActionResult UploadBalanceConfirmation(string fromdate, string todate, string expirydate, string createdby, string Type)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), createdby))
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
                            List<BalConfirmationModel> lstbalconf = new List<BalConfirmationModel>();
                            if (finalRecords.Columns.Count == 1)
                            {
                                string OrdNo = "";
                                OrdNo = _BalanceConfirmationService.GetOrderNo();
                                OrdNo = GetReqNo(OrdNo);
                                for (int i = 1; i < finalRecords.Rows.Count; i++)
                                {
                                    DateTime tempfromdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
                                    DateTime temptodate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
                                    DateTime tempexpirydate = DateTime.ParseExact(expirydate, "dd-MM-yyyy", null);
                                    BalConfirmationModel balconf = new BalConfirmationModel();
                                    balconf.RequestNovtxt = OrdNo;
                                    balconf.FromDatedatetime = Convert.ToDateTime(tempfromdate);
                                    balconf.ToDatedatetime = Convert.ToDateTime(temptodate);
                                    balconf.ExpiryDatedatetime = Convert.ToDateTime(tempexpirydate);
                                    balconf.DealerCodevtxt = finalRecords.Rows[i][0].ToString();
                                    balconf.Typevtxt = Type;
                                    balconf.CreatedByvtxt = createdby;
                                    lstbalconf.Add(balconf);
                                }
                                if (lstbalconf.Count > 0)
                                {
                                    _BalanceConfirmationService.DeleteTempBalConf(createdby);
                                    for (int k = 0; k < lstbalconf.Count; k++)
                                    {
                                        long j = _BalanceConfirmationService.InsertBalConfirmationIntoTempTable(lstbalconf[k]);
                                    }
                                    _BalanceConfirmationService.InsertBalConfirmationIntoMainTable(createdby);
                                    List<BalConfirmationModel> lst = new List<BalConfirmationModel>();
                                    lst = _BalanceConfirmationService.GetTempBalConfirm(createdby);
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
                else
                {
                    return Ok("Un Authorized User");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //use for DownloadBalanceConfirmation download error file
        [HttpGet("DownloadBalanceConfirmation/{createdby}")]
        public IActionResult DownloadBalanceConfirmation(string createdby)
        {
            try
            {
                List<BalConfirmationModel> lstbalconf = _BalanceConfirmationService.GetTempBalConfirm(createdby);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Balance Confirmation");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Dealer Code";
                    worksheet.Cell(currentRow, 2).Value = "Remarks";
                    foreach (var bal in lstbalconf)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = bal.DealerCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = bal.Remarksvtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ErrorBalanceConfirmation.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Get BalConfHeaderData ForAH
        [HttpGet("GetBalConfHeaderDataForAH/{usercode},{PageNo},{PageSize}")]
        public IActionResult GetBalConfHeaderDataForAH(string usercode, int PageNo, int PageSize)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfHeaderforAccountingHead(usercode, PageNo, PageSize));
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

        //use for Get BalConfHeaderData ForAH Count
        [HttpGet("GetBalConfHeaderDataForAHCount/{usercode}")]
        public long GetBalConfHeaderDataForAHCount(string usercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return _BalanceConfirmationService.GetBalanceConfHeaderforAccountingHeadCount(usercode);
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

        //use for Get BalConfDetailData ForAH Count
        [HttpGet("GetBalConfDetailsDataForAH/{idbint}")]
        public IActionResult GetBalConfDetailsDataForAH(long idbint)
        {
            try
            {
                return Ok(_BalanceConfirmationService.GetBalanceConfDetailData(idbint));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        private string GetReqNo(string reqno)
        {
            string NextVistId = "";

            if (reqno == "0")
            {
                NextVistId = "AA001";
            }
            else
            {
                NextVistId = GenerateID(reqno);
            }
            return NextVistId;
        }

        public string GenerateID(string AppId)
        {
            string AppNewID = "";
            string chartAt_1 = AppId.Substring(0, 1);
            string CharAt_2 = AppId.Substring(1, 1);
            string num = AppId.Substring(2);
            byte first_char_byte = Encoding.ASCII.GetBytes(chartAt_1)[0];
            byte second_char_byte = Encoding.ASCII.GetBytes(CharAt_2)[0];
            int first_char_int = Convert.ToInt32(first_char_byte);
            int second_char_int = Convert.ToInt32(second_char_byte);
            int num_int = Convert.ToInt32(num);
            int next_num = 0;
            string nextNum_string = "";
            string Second_next_char = "";
            string first_next_char = "";
            int second_next_char_int = 0;

            if (chartAt_1 == "Z")
            {
                if (CharAt_2 == "Z")
                {
                    if (num_int == 999)
                    {
                        AppNewID = "CanNotGenerate";
                    }
                    else
                    {
                        next_num = num_int + 1;
                        nextNum_string = next_num.ToString();
                        if (nextNum_string.Length == 1)
                        {
                            nextNum_string = "00" + nextNum_string.Trim();
                        }
                        if (nextNum_string.Length == 2)
                        {
                            nextNum_string = "0" + nextNum_string.Trim();
                        }
                        AppNewID = chartAt_1.Trim() + CharAt_2.Trim() + nextNum_string;
                    }
                }
                else
                {
                    if (num_int == 999)
                    {
                        second_next_char_int = second_char_int + 1;
                        char character3 = (char)second_next_char_int;
                        Second_next_char = character3.ToString();
                        nextNum_string = "001";
                        AppNewID = chartAt_1.Trim() + Second_next_char.Trim() + nextNum_string;
                    }
                    else
                    {
                        next_num = num_int + 1;
                        nextNum_string = next_num.ToString();
                        if (nextNum_string.Length == 1)
                        {
                            nextNum_string = "00" + nextNum_string.Trim();
                        }
                        if (nextNum_string.Length == 2)
                        {
                            nextNum_string = "0" + nextNum_string.Trim();
                        }
                        AppNewID = chartAt_1.Trim() + CharAt_2.Trim() + nextNum_string;
                    }
                }
            }
            else if (CharAt_2 == "Z")
            {
                if (num_int == 999)
                {
                    nextNum_string = "001";
                    Second_next_char = "A";
                    int first_char_next_int = first_char_int + 1;
                    char character1 = (char)first_char_next_int;
                    first_next_char = character1.ToString();
                    AppNewID = first_next_char.Trim() + Second_next_char.Trim() + nextNum_string.Trim();
                }
                else
                {
                    next_num = num_int + 1;
                    nextNum_string = next_num.ToString();
                    if (nextNum_string.Length == 1)
                    {
                        nextNum_string = "00" + nextNum_string.Trim();
                    }
                    if (nextNum_string.Length == 2)
                    {
                        nextNum_string = "0" + nextNum_string.Trim();
                    }
                    AppNewID = chartAt_1.Trim() + CharAt_2.Trim() + nextNum_string.Trim();
                }
            }
            else if (num_int == 999)
            {
                second_next_char_int = second_char_int + 1;
                char character2 = (char)second_next_char_int;
                Second_next_char = character2.ToString();
                nextNum_string = "001";
                AppNewID = chartAt_1.Trim() + Second_next_char.Trim() + nextNum_string;
            }
            else
            {
                next_num = num_int + 1;
                nextNum_string = next_num.ToString();
                if (nextNum_string.Length == 1)
                {
                    nextNum_string = "00" + nextNum_string.Trim();
                }
                if (nextNum_string.Length == 2)
                {
                    nextNum_string = "0" + nextNum_string.Trim();
                }
                AppNewID = chartAt_1.Trim() + CharAt_2.Trim() + nextNum_string.ToString().Trim();
            }
            return AppNewID;
        }

        //use for Get BalConfHeaderData For Customer
        [HttpGet("GetBalConfHeaderDataForCustomer/{customercode},{PageNo},{PageSize}")]
        public IActionResult GetBalConfHeaderDataForCustomer(string customercode, int PageNo, int PageSize)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfHeaderforCustomer(customercode, PageNo, PageSize, "List", 0));
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

        //use for Get BalConfHeaderData For Customer Count
        [HttpGet("GetBalConfHeaderDataForCustomerCount/{customercode}")]
        public long GetBalConfHeaderDataForCustomerCount(string customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return _BalanceConfirmationService.GetBalanceConfHeaderforCustomer(customercode, 0, 0, "Count", 0).Count;
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

        //use for Get BalConfHeaderData Header ID For Customer
        [HttpGet("GetBalConfHeaderDataByID/{customercode},{id}")]
        public IActionResult GetBalConfHeaderDataByID(string customercode, long id)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfHeaderforCustomer(customercode, 0, 0, "Detail", id));
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

        //use for Get BalConfDetail Details by ID For Customer
        [HttpGet("GetBalConfDetailDataByID/{customercode},{id}")]
        public IActionResult GetBalConfDetailDataByID(string customercode, long id)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfDetailforCustomer(customercode, "DetailList", id));
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

        //use for update Expiry date of Balance confirmation
        [HttpPut("UpdateExpiryDate")]
        public long UpdateExpiryDate(BalConfirmationEditModel content)
        {
            try
            {
                return _BalanceConfirmationService.UpdateExpiryDate(content);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for update BalanceConfirmation for dealer
        [HttpPost("UpdateBalanceConfirmationByDealer/{id},{RequestNo},{Action},{User},{Remarks}"), DisableRequestSizeLimit]
        public IActionResult UpdateBalanceConfirmationByDealer(long id, string RequestNo, string Action, string User, string Remarks)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), User))
                {
                    var files = Request.Form.Files;
                    var folderName = Path.Combine("Uploads", "BalanceConfirmationFiles");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (files.Count == 0)
                    {
                        LedgerBalanceConfirmationHeader updatebal = new LedgerBalanceConfirmationHeader();
                        updatebal.BalanceConfirmationAction = Action;
                        //if (Action == "A" || Action == "B")
                        if (Action == "A" )
                        {
                            updatebal.BalanceConfirmationStatus = true;
                        }
                        else
                        {
                            updatebal.BalanceConfirmationStatus = false;
                        }
                        updatebal.BalanceConfirmationUser = User;
                        if (Remarks == "undefined")
                        {
                            updatebal.Remarksvtxt = "";
                        }
                        else
                            updatebal.Remarksvtxt = Remarks;
                        updatebal.IDbint = id;
                        updatebal.AttachmentPathvtxt = "";
                        updatebal.AttachmentFileNamevtxt = "";
                        updatebal.AttachmentFilevtxt = "";
                        _BalanceConfirmationService.UpdateCustomerLedgerbalanceconfStatus(updatebal);
                    }
                    else
                    {
                        foreach (var file in files)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            string filenameforsave = RequestNo + '_' + User + '_' + DateTime.Now.ToString("dd-MM-yyyy") + '_' + fileName;
                            var fullPath = Path.Combine(pathToSave, filenameforsave);
                            var dbPath = Path.Combine(folderName, filenameforsave); //you can add this path to a list and then return all dbPaths to the client if require
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
                                LedgerBalanceConfirmationHeader updatebal = new LedgerBalanceConfirmationHeader();
                                updatebal.BalanceConfirmationAction = Action;
                                if (Action == "A" || Action == "B")
                                {
                                    updatebal.BalanceConfirmationStatus = true;
                                }
                                else
                                {
                                    updatebal.BalanceConfirmationStatus = false;
                                }
                                updatebal.BalanceConfirmationUser = User;
                                if (Remarks == "undefined")
                                {
                                    updatebal.Remarksvtxt = "";
                                }
                                else
                                    updatebal.Remarksvtxt = Remarks;
                                updatebal.IDbint = id;
                                updatebal.AttachmentPathvtxt = pathToSave;
                                updatebal.AttachmentFileNamevtxt = filenameforsave;
                                updatebal.AttachmentFilevtxt = file.FileName;
                                _BalanceConfirmationService.UpdateCustomerLedgerbalanceconfStatus(updatebal);
                            }
                        }
                    }

                    return Ok("file is  uploaded Successfully.");
                }
                else
                {
                    return Ok("Un Authorized User");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //use for update Expiry date of Balance confirmation
        [HttpPut("UpdateBalanceConfirmationByDealerDetails")]
        public long UpdateBalanceConfirmationByDealerDetails(LedgerBalanceConfirmationDetails model)
        {
            try
            {
                return _BalanceConfirmationService.UpdateCustomerLedgerbalanceconfDetails(model);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Get BalConfHeaderData Header For Employees
        [HttpGet("GetBalConfHeaderDataForEmployees/{fromdate},{todate},{status},{usertype},{usercode},{pageno},{pagesize},{keyword}")]
        public IActionResult GetBalConfHeaderDataForEmployees(string fromdate, string todate, string status, string usertype, string usercode, int pageno, int pagesize, string keyword)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfHeaderListForEmployee(fromdate, todate, status, usertype, usercode, pageno, pagesize, keyword));
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

        //use for Get BalConfHeaderData Header Count For Employees
        [HttpGet("GetBalConfHeaderDataForEmployeesCount/{fromdate},{todate},{status},{usertype},{usercode},{keyword}")]
        public long GetBalConfHeaderDataForEmployeesCount(string fromdate, string todate, string status, string usertype, string usercode, string keyword)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return _BalanceConfirmationService.GetBalanceConfHeaderListForEmployeeCount(fromdate, todate, status, usertype, usercode, keyword);
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

        //use for Get BalConfHeaderData For SPCFA
        [HttpGet("GetBalConfHeaderDataForSPCFA/{customercode},{PageNo},{PageSize}")]
        public IActionResult GetBalConfHeaderDataForSPCFA(string customercode, int PageNo, int PageSize)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), customercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfHeaderforSPCFA(customercode, PageNo, PageSize, "List", 0));
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

        //use for Get BalConfHeaderData For SPCFA Count
        [HttpGet("GetBalConfHeaderDataForSPCFACount/{customercode}")]
        public long GetBalConfHeaderDataForSPCFACount(string customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), customercode))
                {
                    return _BalanceConfirmationService.GetBalanceConfHeaderforSPCFA(customercode, 0, 0, "Count", 0).Count;
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

        //use for Get BalConfHeaderData Header ID For SPCFA
        [HttpGet("GetBalConfHeaderDataByIDSPCFA/{customercode},{id}")]
        public IActionResult GetBalConfHeaderDataByIDSPCFA(string customercode, long id)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), customercode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfHeaderforSPCFA(customercode, 0, 0, "Detail", id));
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

        //use for Get BalConfDetail Details by ID For SPCFA
        [HttpGet("GetBalConfDetailDataByIDSPCFA/{customercode},{id},{empcode}")]
        public IActionResult GetBalConfDetailDataByIDSPCFA(string customercode, long id, string empcode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), empcode))
                {
                    return Ok(_BalanceConfirmationService.GetBalanceConfDetailforSPCFA(customercode, "DetailList", id));
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

        //use for update BalanceConfirmation for SPCFA
        [HttpPost("UpdateBalanceConfirmationBySPCFA/{id},{RequestNo},{Action},{User},{Remarks}"), DisableRequestSizeLimit]
        public IActionResult UpdateBalanceConfirmationBySPCFA(long id, string RequestNo, string Action, string User, string Remarks)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), User))
                {
                    var files = Request.Form.Files;
                    var folderName = Path.Combine("Uploads", "BalanceConfirmationFiles");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (files.Count == 0)
                    {
                        LedgerBalanceConfirmationHeader updatebal = new LedgerBalanceConfirmationHeader();
                        updatebal.BalanceConfirmationAction = Action;
                        if (Action == "A" || Action == "B")
                        {
                            updatebal.BalanceConfirmationStatus = true;
                        }
                        else
                        {
                            updatebal.BalanceConfirmationStatus = false;
                        }
                        updatebal.BalanceConfirmationUser = User;
                        if (Remarks == "undefined")
                        {
                            updatebal.Remarksvtxt = "";
                        }
                        else
                            updatebal.Remarksvtxt = Remarks;
                        updatebal.IDbint = id;
                        updatebal.AttachmentPathvtxt = "";
                        updatebal.AttachmentFileNamevtxt = "";
                        updatebal.AttachmentFilevtxt = "";
                        _BalanceConfirmationService.UpdateSPCFALedgerbalanceconfStatus(updatebal);
                    }
                    else
                    {
                        foreach (var file in files)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            string filenameforsave = RequestNo + '_' + User + '_' + DateTime.Now.ToString("dd-MM-yyyy") + '_' + fileName;
                            var fullPath = Path.Combine(pathToSave, filenameforsave);
                            var dbPath = Path.Combine(folderName, filenameforsave); //you can add this path to a list and then return all dbPaths to the client if require
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
                                LedgerBalanceConfirmationHeader updatebal = new LedgerBalanceConfirmationHeader();
                                updatebal.BalanceConfirmationAction = Action;
                                if (Action == "A" || Action == "B")
                                {
                                    updatebal.BalanceConfirmationStatus = true;
                                }
                                else
                                {
                                    updatebal.BalanceConfirmationStatus = false;
                                }
                                updatebal.BalanceConfirmationUser = User;
                                if (Remarks == "undefined")
                                {
                                    updatebal.Remarksvtxt = "";
                                }
                                else
                                    updatebal.Remarksvtxt = Remarks;
                                updatebal.IDbint = id;
                                updatebal.AttachmentPathvtxt = pathToSave;
                                updatebal.AttachmentFileNamevtxt = filenameforsave;
                                updatebal.AttachmentFilevtxt = file.FileName;
                                _BalanceConfirmationService.UpdateSPCFALedgerbalanceconfStatus(updatebal);
                            }
                        }
                    }

                    return Ok("file is  uploaded Successfully.");
                }
                else
                {
                    return Ok("Un Authorized User");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        //use for update Expiry date of Balance confirmation
        [HttpPut("UpdateBalanceConfirmationBySPCFADetails")]
        public long UpdateBalanceConfirmationBySPCFADetails(LedgerBalanceConfSPCFADetails model)
        {
            try
            {
                return _BalanceConfirmationService.UpdateSPCFALedgerbalanceconfDetails(model);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetSPCFABalanceConfHeaderListForEmployee/{fromdate},{todate},{status},{usertype},{usercode},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetSPCFABalanceConfHeaderListForEmployee(string fromdate, string todate, string status, string usertype, string usercode, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_BalanceConfirmationService.GetSPCFABalanceConfHeaderListForEmployee(fromdate, todate, status, usertype, usercode, PageNo, PageSize, KeyWord));
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

        [HttpGet("GetSPCFABalanceConfHeaderListForEmployeeCount/{fromdate},{todate},{status},{usertype},{usercode},{KeyWord}")]
        public IActionResult GetSPCFABalanceConfHeaderListForEmployeeCount(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_BalanceConfirmationService.GetSPCFABalanceConfHeaderListForEmployeeCount(fromdate, todate, status, usertype, usercode, KeyWord));
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

        //use for download attachments
        [HttpGet("DownloadFile/{customercode},{id}")]
        [Obsolete]
        public IActionResult DownloadFile(string customercode, int id)
        {
            try
            {
                List<LedgerBalanceConfirmationHeader> model = _BalanceConfirmationService.GetBalanceConfHeaderforSPCFA(customercode, 0, 0, "Detail", id);
                if (model[0].AttachmentFilevtxt == null)
                    return Content("filename not present");

                var uploads = _hostingEnvironment.WebRootPath;
                uploads = uploads.Replace("\\wwwroot", "");
                uploads = Path.Combine(uploads, "Uploads\\BalanceConfirmationFiles");

                var filePath = Path.Combine(uploads, model[0].AttachmentFileNamevtxt);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();
                var net = new System.Net.WebClient();
                var data = net.DownloadData(filePath);
                var content = new System.IO.MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                return File(content, contentType, model[0].AttachmentFilevtxt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //use for download attachments
        [HttpGet("DownloadFileForEmp/{Mode},{ID}")]
        [Obsolete]
        public IActionResult DownloadFileForEmp(string Mode, int ID)
        {
            try
            {
                LedgerBalanceConfirmationHeader model = _BalanceConfirmationService.GetSPCFABalanceConfHeaderDetailForEmployee(Mode, ID);
                if (model.AttachmentFilevtxt == null)
                    return Content("filename not present");

                var uploads = _hostingEnvironment.WebRootPath;
                uploads = uploads.Replace("\\wwwroot", "");
                uploads = Path.Combine(uploads, "Uploads\\BalanceConfirmationFiles");

                var filePath = Path.Combine(uploads, model.AttachmentFileNamevtxt);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();
                var net = new System.Net.WebClient();
                var data = net.DownloadData(filePath);
                var content = new System.IO.MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                return File(content, contentType, model.AttachmentFilevtxt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".JPG", "image/jpeg"},
                {".JPEG", "image/jpeg"},
                {".JFIF", "image/jpeg"},
                {".BMP", "image/jpeg"},
                {".JPG", "image/jpeg"},
                {".SVG", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        //use for Insert Balance Conf Log for Both Customer and Accounting Head
        [HttpPost("InsertBalanceConfLog")]
        public long InsertBalanceConfLog(LedgerBalanceConfirmationLog model)
        {
            try
            {
                return _BalanceConfirmationService.InsertBalanceConfLog(model);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }


        //use for Balance Cong Log View
        [HttpGet("GetBalanceConfLog/{ID}")]
        public IActionResult GetBalanceConfLog(long ID)
        {
            try
            {
                return Ok(_BalanceConfirmationService.GetBalanceConfLog(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}