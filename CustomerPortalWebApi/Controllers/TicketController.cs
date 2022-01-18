using ClosedXML.Excel;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ITicketService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public TicketController(IHostingEnvironment environment, ITicketService ITicketService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _ITicketService = ITicketService;
            _Checktokenservice = checktokenservice;
            _hostingEnvironment = environment;
        }

        [HttpGet("GetReqOrderNo")]
        public IActionResult GetReqOrderNo()
        {
            try
            {
                string OrdNo = "";
                OrdNo = _ITicketService.GetOrderNo();
                OrdNo = GetReqNo(OrdNo);
                return Ok(_ITicketService.GetReqOrderNo(OrdNo));
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

        [HttpPost("InsertTicketHeader")]
        public ActionResult InsertTicketHeader(TicketModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    string OrdNo = "";
                    OrdNo = _ITicketService.GetOrderNo();
                    OrdNo = GetReqNo(OrdNo);
                    var TicketHeader = new TicketModel()
                    {
                        RefNovtxt = OrdNo,
                        CustomerCodevtxt = model.CustomerCodevtxt,
                        CustomerNamevtxt = model.CustomerNamevtxt,
                        Priorityvtxt = model.Priorityvtxt,
                        Departmentidint = model.Departmentidint,
                        DepartmentNamevtxt = model.DepartmentNamevtxt,
                        Typevtxt = model.Typevtxt,
                        Subjectvtxt = model.Subjectvtxt,
                        Descriptionvtxt = model.Descriptionvtxt,
                        Statusvtxt = "Open",
                        CreatedByvtxt = model.CreatedByvtxt,
                    };

                    var ID = _ITicketService.Create(TicketHeader);
                    var MailStatus = SendMailForTicketRised(ID);
                    return Ok(ID);
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

        [HttpPost("InsertTicketDetail")]
        public ActionResult InsertTicketDetail(TicketModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var TicketDetail = new TicketModel()
                    {
                        CustomerCodevtxt = model.CustomerCodevtxt,
                        HIDbint = model.HIDbint,
                        UserCodevtxt = model.UserCodevtxt,
                        UserTypevtxt = model.UserTypevtxt,
                        Remarksvtxt = model.Remarksvtxt,
                        Departmentidint = model.Departmentidint,
                        DepartmentNamevtxt = model.DepartmentNamevtxt,
                        Statusvtxt = model.Statusvtxt,
                        Priorityvtxt = model.Priorityvtxt,
                        CreatedByvtxt = model.CreatedByvtxt,
                    };
                    var ID = _ITicketService.InsertDetaial(TicketDetail);
                    var mail = SendEmailForActionTaken(ID);
                    return Ok(ID);
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

        [HttpGet("GetTicketHeaderList/{fromdate},{todate},{usercode},{usertype},{PageNo},{PageSize},{status},{KeyWord}")]
        public IActionResult GetTicketHeaderList(string fromdate, string todate, string usercode, string usertype, int PageNo, int PageSize, string status, string KeyWord)
        {
            try
            {
                return Ok(_ITicketService.GetTicketHeaderList(fromdate, todate, usercode, usertype, PageNo, PageSize, status, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetTicketHeaderListCount/{fromdate},{todate},{usercode},{usertype},{status},{KeyWord}")]
        public IActionResult GetTicketHeaderListCount(string fromdate, string todate, string usercode, string usertype, string status, string KeyWord)
        {
            try
            {
                return Ok(_ITicketService.GetTicketHeaderListCount(fromdate, todate, usercode, usertype, status, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Ticket Detail List By Token
        [HttpGet("GetTicketDetailId/{TokenNo}")]
        public IActionResult GetTicketDetailList(string TokenNo)
        {
            try
            {
                return Ok(_ITicketService.GetTicketDetailList(TokenNo));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Ticket Header Detail List By Token
        [HttpGet("GetTicketHeaderDetail/{TokenNo}")]
        public IActionResult GetTicketHeaderDetail(string TokenNo)
        {
            try
            {
                return Ok(_ITicketService.GetTicketHeaderDetail(TokenNo));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("UploadFileForCustomer/{ID},{tokenNo},{UserCode}"), DisableRequestSizeLimit]
        public IActionResult UploadFileForCustomer(int ID, string tokenNo, string UserCode)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Uploads", "TicketAttachment");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string filenameforsave = tokenNo + '_' + ID + '_' + UserCode + '_' + DateTime.Now.ToString("dd-MM-yyyy") + '_' + fileName;
                    var fullPath = Path.Combine(pathToSave, filenameforsave);
                    var dbPath = Path.Combine(folderName, filenameforsave);
                    _ITicketService.AddAttachment(ID, fileName, dbPath, "Customer");
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("UploadFileForEmp/{ID},{tokenNo},{UserCode}"), DisableRequestSizeLimit]
        public IActionResult UploadFileForEmp(int ID, string tokenNo, string UserCode)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Uploads", "TicketAttachment");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string filenameforsave = tokenNo + '_' + ID + '_' + UserCode + '_' + DateTime.Now.ToString("dd-MM-yyyy") + '_' + fileName;
                    var fullPath = Path.Combine(pathToSave, filenameforsave);
                    var dbPath = Path.Combine(folderName, filenameforsave);
                    _ITicketService.AddAttachment(ID, fileName, dbPath, "Emp");

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        //use for download attachments
        [HttpGet("DownloadFile/{ID},{Type}")]
        [Obsolete]
        public IActionResult DownloadFile(int ID, string Type)
        {
            try
            {
                TicketModel model = _ITicketService.GetTicketById(ID, Type);

                if (model.AttachmentFilePathvtxt == null)
                    return Content("filename not present");

                var uploads = _hostingEnvironment.WebRootPath;
                uploads = uploads.Replace("\\wwwroot", "");
                uploads = Path.Combine(uploads, "Uploads\\TicketAttachment");

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), model.AttachmentFilePathvtxt);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var net = new System.Net.WebClient();
                var data = net.DownloadData(filePath);
                var content = new System.IO.MemoryStream(data);
                var contentType = "APPLICATION/octet-stream";
                return File(content, contentType, model.AttachmentFileNamevtxt);
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

        //Use for Customer
        [HttpGet("DownloadTicketHeaderList/{fromdate},{todate},{usercode},{status},{KeyWord}")]
        public IActionResult DownloadTicketHeaderList(string fromdate, string todate, string usercode, string status, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    List<TicketModel> Ticketlist = _ITicketService.DownloadTicketHeaderList(fromdate, todate, usercode, "Customer", status, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Ticket");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Ticket No";
                        worksheet.Cell(currentRow, 3).Value = "Ticket Date";
                        worksheet.Cell(currentRow, 4).Value = "Customer";
                        worksheet.Cell(currentRow, 5).Value = "Subject";
                        worksheet.Cell(currentRow, 6).Value = "Description";
                        worksheet.Cell(currentRow, 7).Value = "Category";
                        worksheet.Cell(currentRow, 8).Value = "Priority";
                        worksheet.Cell(currentRow, 9).Value = "Status";
                        foreach (var Ticket in Ticketlist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = Ticket.RefNovtxt;
                            worksheet.Cell(currentRow, 3).Value = Ticket.RefDatedate;
                            worksheet.Cell(currentRow, 4).Value = Ticket.CustomerCodevtxt + ' ' + Ticket.CustomerNamevtxt;
                            worksheet.Cell(currentRow, 5).Value = Ticket.Subjectvtxt;
                            worksheet.Cell(currentRow, 6).Value = Ticket.Descriptionvtxt;
                            worksheet.Cell(currentRow, 7).Value = Ticket.DepartmentNamevtxt;
                            worksheet.Cell(currentRow, 8).Value = Ticket.Priorityvtxt;
                            worksheet.Cell(currentRow, 9).Value = Ticket.Statusvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DeliveryOrders.xlsx");
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

        [HttpGet("DownloadTicketHeaderListForEmp/{fromdate},{todate},{usercode},{usertype},{status},{KeyWord}")]
        public IActionResult DownloadTicketHeaderListForEmp(string fromdate, string todate, string usercode, string usertype, string status, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    List<TicketModel> Ticketlist = _ITicketService.DownloadTicketHeaderList(fromdate, todate, usercode, usertype, status, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Ticket");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Ticket No";
                        worksheet.Cell(currentRow, 3).Value = "Ticket Date";
                        worksheet.Cell(currentRow, 4).Value = "Customer";
                        worksheet.Cell(currentRow, 5).Value = "Subject";
                        worksheet.Cell(currentRow, 6).Value = "Description";
                        worksheet.Cell(currentRow, 7).Value = "Category";
                        worksheet.Cell(currentRow, 8).Value = "Priority";
                        worksheet.Cell(currentRow, 9).Value = "Status";
                        worksheet.Cell(currentRow, 10).Value = "Assign To";
                        foreach (var Ticket in Ticketlist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = Ticket.RefNovtxt;
                            worksheet.Cell(currentRow, 3).Value = Ticket.RefDatedate;
                            worksheet.Cell(currentRow, 4).Value = Ticket.CustomerCodevtxt + ' ' + Ticket.CustomerNamevtxt;
                            worksheet.Cell(currentRow, 5).Value = Ticket.Subjectvtxt;
                            worksheet.Cell(currentRow, 6).Value = Ticket.Descriptionvtxt;
                            worksheet.Cell(currentRow, 7).Value = Ticket.DepartmentNamevtxt;
                            worksheet.Cell(currentRow, 8).Value = Ticket.Priorityvtxt;
                            worksheet.Cell(currentRow, 9).Value = Ticket.Statusvtxt;
                            worksheet.Cell(currentRow, 10).Value = Ticket.AssignTo;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DeliveryOrders.xlsx");
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

        //use for SendMailForTicketRised pass type as Email
        [HttpGet("SendMailForTicketRised/{ID}")]
        public async Task<IActionResult> SendMailForTicketRised(long ID)
        {
            try
            {
                TicketModel model = _ITicketService.GetTicketById(ID, "Email");
                if (model.CustomerEmail != "")
                {
                    await _ITicketService.SendEmailForTicketRisedToCustomer(model);
                    await _ITicketService.SendEmailForTicketRisedToUser(model);
                    return Ok("Done");
                }
                else
                {
                    return Ok("Invalid Email ID");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }

        //use for SendEmailForActionTaken
        [HttpGet("SendEmailForActionTaken/{ID}")]
        public async Task<IActionResult> SendEmailForActionTaken(long ID)
        {
            try
            {
                TicketModel model = _ITicketService.GetTicketById(ID, "SendEmpEmail");
                if (model.CustomerEmail != "")
                {
                    await _ITicketService.SendEmailForActionTicketRisedToCustomer(model);
                    await _ITicketService.SendEmailForActionTicketRisedToUser(model);
                    return Ok("Done");
                }
                else
                {
                    return Ok("Invalid Email ID");
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                throw;
            }
        }

        [HttpGet("DownloadMISReportCategoryWise/{usercode},{usertype}")]
        public IActionResult DownloadMISReportCategoryWise(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    List<TicketModel> Ticketlist = _ITicketService.MISReportCategoryWise(usercode, usertype);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Ticket");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Category Name";
                        worksheet.Cell(currentRow, 3).Value = "Complaint";
                        worksheet.Cell(currentRow, 6).Value = "Feedback";
                        worksheet.Cell(currentRow, 9).Value = "Total";
                        worksheet.Cell(currentRow, 10).Value = "0 - 7 Days";
                        worksheet.Cell(currentRow, 11).Value = "8 - 15 Days";
                        worksheet.Cell(currentRow, 12).Value = "16 - 30 Days";
                        worksheet.Cell(currentRow, 13).Value = "31 - 60 Days";
                        worksheet.Cell(currentRow, 14).Value = "60 and Above";
                        worksheet.Range("C1:E1").Merge();
                        worksheet.Range("F1:H1").Merge();
                        worksheet.Range("C1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Range("F1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        currentRow++;
                        worksheet.Cell(currentRow, 3).Value = "Low";
                        worksheet.Cell(currentRow, 4).Value = "Normal";
                        worksheet.Cell(currentRow, 5).Value = "High";
                        worksheet.Cell(currentRow, 6).Value = "Low";
                        worksheet.Cell(currentRow, 7).Value = "Normal";
                        worksheet.Cell(currentRow, 8).Value = "High";

                        foreach (var Ticket in Ticketlist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = Ticket.DepartmentNamevtxt;
                            worksheet.Cell(currentRow, 3).Value = Ticket.LowComplaintCount;
                            worksheet.Cell(currentRow, 4).Value = Ticket.NormalComplaintCount;
                            worksheet.Cell(currentRow, 5).Value = Ticket.HighComplaintCount;
                            worksheet.Cell(currentRow, 6).Value = Ticket.LowFeedBackCount;
                            worksheet.Cell(currentRow, 7).Value = Ticket.NormalFeedBackCount;
                            worksheet.Cell(currentRow, 8).Value = Ticket.HighFeedBackCount;
                            worksheet.Cell(currentRow, 9).Value = Ticket.Total;
                            worksheet.Cell(currentRow, 10).Value = Ticket.FirstDays;
                            worksheet.Cell(currentRow, 11).Value = Ticket.SecondDays;
                            worksheet.Cell(currentRow, 12).Value = Ticket.ThirdDays;
                            worksheet.Cell(currentRow, 13).Value = Ticket.FourthDays;
                            worksheet.Cell(currentRow, 14).Value = Ticket.FithDays;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DeliveryOrders.xlsx");
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

        [HttpGet("DownloadMISReportAssignToWise/{usercode},{usertype}")]
        public IActionResult DownloadMISReportAssignToWise(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    List<TicketModel> Ticketlist = _ITicketService.MISReportAssignToWise(usercode, usertype);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Ticket");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Category Name";
                        worksheet.Cell(currentRow, 3).Value = "Complaint";
                        worksheet.Cell(currentRow, 6).Value = "Feedback";
                        worksheet.Cell(currentRow, 9).Value = "Total";
                        worksheet.Cell(currentRow, 10).Value = "0 - 7 Days";
                        worksheet.Cell(currentRow, 11).Value = "8 - 15 Days";
                        worksheet.Cell(currentRow, 12).Value = "16 - 30 Days";
                        worksheet.Cell(currentRow, 13).Value = "31 - 60 Days";
                        worksheet.Cell(currentRow, 14).Value = "60 and Above";
                        worksheet.Range("C1:E1").Merge();
                        worksheet.Range("F1:H1").Merge();
                        worksheet.Range("C1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Range("F1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        currentRow++;
                        worksheet.Cell(currentRow, 3).Value = "Low";
                        worksheet.Cell(currentRow, 4).Value = "Normal";
                        worksheet.Cell(currentRow, 5).Value = "High";
                        worksheet.Cell(currentRow, 6).Value = "Low";
                        worksheet.Cell(currentRow, 7).Value = "Normal";
                        worksheet.Cell(currentRow, 8).Value = "High";

                        foreach (var Ticket in Ticketlist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = Ticket.DepartmentNamevtxt;
                            worksheet.Cell(currentRow, 3).Value = Ticket.LowComplaintCount;
                            worksheet.Cell(currentRow, 4).Value = Ticket.NormalComplaintCount;
                            worksheet.Cell(currentRow, 5).Value = Ticket.HighComplaintCount;
                            worksheet.Cell(currentRow, 6).Value = Ticket.LowFeedBackCount;
                            worksheet.Cell(currentRow, 7).Value = Ticket.NormalFeedBackCount;
                            worksheet.Cell(currentRow, 8).Value = Ticket.HighFeedBackCount;
                            worksheet.Cell(currentRow, 9).Value = Ticket.Total;
                            worksheet.Cell(currentRow, 10).Value = Ticket.FirstDays;
                            worksheet.Cell(currentRow, 11).Value = Ticket.SecondDays;
                            worksheet.Cell(currentRow, 12).Value = Ticket.ThirdDays;
                            worksheet.Cell(currentRow, 13).Value = Ticket.FourthDays;
                            worksheet.Cell(currentRow, 14).Value = Ticket.FithDays;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DeliveryOrders.xlsx");
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

        //Use for MIS Report for CategoryWise
        [HttpGet("MISReportCategoryWise/{usercode},{usertype}")]
        public IActionResult MISReportCategoryWise(string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    return Ok(_ITicketService.MISReportCategoryWise(usercode, usertype));
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

        //Use for MIS Report for AssignToWise
        [HttpGet("MISReportAssignToWise/{usercode},{usertype}")]
        public IActionResult MISReportAssignToWise(string fromdate, string todate, string usercode, string usertype)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    return Ok(_ITicketService.MISReportAssignToWise(usercode, usertype));
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

        //use for MIS Report for CategoryWise list
        [HttpGet("MISReportCategoryWiseList/{usercode},{usertype},{Priority},{Type},{Category},{From},{To},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult MISReportCategoryWiseList(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_ITicketService.MISReportCategoryWiseList(usercode, usertype, Priority, Type, Category, From, To, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for MIS Report for AssignToWise list
        [HttpGet("MISReportAssignToWiseList/{usercode},{usertype},{Priority},{Type},{AssignTo},{From},{To},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult MISReportAssignToWiseList(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, int PageNo, int PageSize, string KeyWord)
        {
            try

            {
                return Ok(_ITicketService.MISReportAssignToWiseList(usercode, usertype, Priority, Type, AssignTo, From, To, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for MIS Report for CategoryWise list Count
        [HttpGet("MISReportCategoryWiseListCount/{usercode},{usertype},{Priority},{Type},{Category},{From},{To},{KeyWord}")]
        public IActionResult MISReportCategoryWiseListCount(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, string KeyWord)
        {
            try
            {
                return Ok(_ITicketService.MISReportCategoryWiseListCount(usercode, usertype, Priority, Type, Category, From, To, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("MISReportCategoryWiseListDownload/{usercode},{usertype},{Priority},{Type},{Category},{From},{To},{KeyWord}")]
        public IActionResult MISReportCategoryWiseListDownload(string usercode, string usertype, string Priority, string Type, string Category, int From, int To, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    List<TicketModel> Ticketlist = _ITicketService.MISReportCategoryWiseListDownload(usercode, usertype, Priority, Type, Category, From, To, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Ticket");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Ticket No";
                        worksheet.Cell(currentRow, 3).Value = "Ticket Date";
                        worksheet.Cell(currentRow, 4).Value = "Customer";
                        worksheet.Cell(currentRow, 5).Value = "Subject";
                        worksheet.Cell(currentRow, 6).Value = "Description";
                        worksheet.Cell(currentRow, 7).Value = "Category";
                        worksheet.Cell(currentRow, 8).Value = "Priority";
                        foreach (var Ticket in Ticketlist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = Ticket.RefNovtxt;
                            worksheet.Cell(currentRow, 3).Value = Ticket.RefDatedate;
                            worksheet.Cell(currentRow, 4).Value = Ticket.CustomerCodevtxt + ' ' + Ticket.CustomerNamevtxt;
                            worksheet.Cell(currentRow, 5).Value = Ticket.Subjectvtxt;
                            worksheet.Cell(currentRow, 6).Value = Ticket.Descriptionvtxt;
                            worksheet.Cell(currentRow, 7).Value = Ticket.DepartmentNamevtxt;
                            worksheet.Cell(currentRow, 8).Value = Ticket.Priorityvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DeliveryOrders.xlsx");
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

        //use for MIS Report for AssignToWise list Count
        [HttpGet("MISReportAssignToWiseListCount/{usercode},{usertype},{Priority},{Type},{AssignTo},{From},{To},{KeyWord}")]
        public IActionResult MISReportAssignToWiseListCount(string usercode, string usertype, string Priority, string Type,
            string AssignTo, int From, int To, string KeyWord)
        {
            try
            {
                return Ok(_ITicketService.MISReportAssignToWiseListCount(usercode, usertype, Priority, Type, AssignTo, From, To, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("MISReportAssignToWiseListDownload/{usercode},{usertype},{Priority},{Type},{AssignTo},{From},{To},{KeyWord}")]
        public IActionResult MISReportAssignToWiseListDownload(string usercode, string usertype, string Priority, string Type, string AssignTo, int From, int To, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                {
                    List<TicketModel> Ticketlist = _ITicketService.MISReportAssignToWiseListDownload(usercode, usertype, Priority, Type, AssignTo, From, To, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Ticket");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "Ticket No";
                        worksheet.Cell(currentRow, 3).Value = "Ticket Date";
                        worksheet.Cell(currentRow, 4).Value = "Customer";
                        worksheet.Cell(currentRow, 5).Value = "Subject";
                        worksheet.Cell(currentRow, 6).Value = "Description";
                        worksheet.Cell(currentRow, 7).Value = "Category";
                        worksheet.Cell(currentRow, 8).Value = "Priority";
                        foreach (var Ticket in Ticketlist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = Ticket.RefNovtxt;
                            worksheet.Cell(currentRow, 3).Value = Ticket.RefDatedate;
                            worksheet.Cell(currentRow, 4).Value = Ticket.CustomerCodevtxt + ' ' + Ticket.CustomerNamevtxt;
                            worksheet.Cell(currentRow, 5).Value = Ticket.Subjectvtxt;
                            worksheet.Cell(currentRow, 6).Value = Ticket.Descriptionvtxt;
                            worksheet.Cell(currentRow, 7).Value = Ticket.DepartmentNamevtxt;
                            worksheet.Cell(currentRow, 8).Value = Ticket.Priorityvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "DeliveryOrders.xlsx");
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
    }
}