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
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MaterialTestCertificateController : ControllerBase
    {
        private readonly IMaterialTestCertificateService _MaterialTestCertificateService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        [Obsolete]
        private IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public MaterialTestCertificateController(IHostingEnvironment environment, IMaterialTestCertificateService MaterialTestCertificate, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _MaterialTestCertificateService = MaterialTestCertificate;
            _Checktokenservice = checktokenservice;
            _hostingEnvironment = environment;
        }

        //use for Get Doc No
        [HttpGet("GetDocNo")]
        public IActionResult GetDocNo()
        {
            try
            {
                string OrdNo = "";
                OrdNo = _MaterialTestCertificateService.GetDocNo();
                OrdNo = GetReqNo(OrdNo);
                return Ok(_MaterialTestCertificateService.GetReqOrderNo(OrdNo));
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

        ////use for Insert Material Certificate
        //[HttpPost("CreateMaterialCertificate/{DocNovtxt},{CertificateNovtxt},{DateOfIssue},{BatchNovtxt},{Depotvtxt},{ValidTillDate}"), DisableRequestSizeLimit]
        //public IActionResult CreateMaterialCertificate(string DocNovtxt, string CertificateNovtxt, string DateOfIssue, string BatchNovtxt, string Depotvtxt,string ValidTillDate)
        //{
        //    try
        //    {
                
        //            var files = Request.Form.Files;
        //            var folderName = Path.Combine("Uploads", "MaterialTestCertificate");
        //            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //            if (files.Count == 0)
        //            {
        //                MaterialTestCertificateModel updatebal = new MaterialTestCertificateModel();
        //                updatebal.DocNovtxt = DocNovtxt;
        //                updatebal.CertificateNovtxt = CertificateNovtxt;
        //                updatebal.DateOfIssue =DateOfIssue;
        //                updatebal.BatchNovtxt = BatchNovtxt;
        //                updatebal.Depotvtxt = Depotvtxt;
        //                updatebal.ValidTillDate = ValidTillDate;
        //                updatebal.AttachmentFileNamevtxt = "";
        //                updatebal.AttachmentFilePathvtxt = "";
        //                _MaterialTestCertificateService.CreateMaterialCertificate(updatebal);
        //            }
        //            else
        //            {
        //                foreach (var file in files)
        //                {
        //                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //                    string filenameforsave = CertificateNovtxt + '_' + BatchNovtxt + '_' + DateTime.Now.ToString("dd-MM-yyyy") + '_' + fileName;
        //                    var fullPath = Path.Combine(pathToSave, filenameforsave);
        //                    var dbPath = Path.Combine(folderName, filenameforsave); //you can add this path to a list and then return all dbPaths to the client if require
        //                    using (var stream = new FileStream(fullPath, FileMode.Create))
        //                    {
        //                        file.CopyTo(stream);
        //                    MaterialTestCertificateModel updatebal = new MaterialTestCertificateModel();
        //                    updatebal.DocNovtxt = DocNovtxt;
        //                    updatebal.CertificateNovtxt = CertificateNovtxt;
        //                    updatebal.DateOfIssue = DateOfIssue;
        //                    updatebal.BatchNovtxt = BatchNovtxt;
        //                    updatebal.Depotvtxt = Depotvtxt;
        //                    updatebal.ValidTillDate = ValidTillDate;
        //                    updatebal.AttachmentFilePathvtxt = pathToSave;
        //                    updatebal.AttachmentFileNamevtxt = filenameforsave;
        //                    _MaterialTestCertificateService.CreateMaterialCertificate(updatebal);
        //                }
        //               }
        //            }

        //            return Ok("file is  uploaded Successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Log(ex);
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //use for Get Get Material Certificate List

        [HttpPost("GetMaterialCertificateList")]
        public IActionResult GetMaterialCertificateList(MaterialTestCertificateModel model)
        {
            try
            {
                model.mode = "List";
                return Ok(_MaterialTestCertificateService.GetMaterialTestCertificates(model));
                
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }




        //use for Insert Material Certificate
        [HttpPost("InsertMaterialCertificate")]
        public IActionResult InsertMaterialCertificate(MaterialTestCertificateModel material)
        {
            try
            {

                return Ok(_MaterialTestCertificateService.InsertMaterialCertificate(material));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, ex.Message);
            }
        }



        //use for Insert Material Certificate
        [HttpPost("InsertMaterialCertificateDetail")]
        public IActionResult InsertMaterialCertificateDetail(List<MaterialTestCertificateDetailModel> model)
        {
            try
            {

                return Ok(_MaterialTestCertificateService.InsertMaterialCertificateDetail(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return StatusCode(500, ex.Message);
            }
        }


        //use for Get Get Material Certificate Count
        [HttpPost("GetMaterialCertificateCount")]
        public long GetMaterialCertificateCount(MaterialTestCertificateModel model)
        {
            try
            {
                model.mode = "Count";
                return _MaterialTestCertificateService.GetMaterialTestCertificates(model).Count;
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Get Get Material Certificate By ID
        [HttpGet("GetMaterialCertificateByID/{id}")]
        public IActionResult GetMaterialCertificateByID(long id)
        {
            try
            {
                MaterialTestCertificateModel model = new MaterialTestCertificateModel();
                model.IDbint = id;
                model.mode = "Detail";
                model.PageNo = 0;
                model.PageSize = 0;
                return Ok(_MaterialTestCertificateService.GetMaterialTestCertificates(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }


        //use for Get Get Material Certificate By ID
        [HttpGet("GetImageByID/{id},{For}")]
        public IActionResult GetImageByID(string id, string For)
        {
            try
            {
                if (For == "MTC")
                {
                    MaterialTestCertificateModel model = new MaterialTestCertificateModel();
                    model.IDbint = Convert.ToInt64(id);
                    model.mode = "Image";
                    model.PageNo = 0;
                    model.PageSize = 0;
                    var result = _MaterialTestCertificateService.GetMaterialTestCertificates(model);
                    byte[] imageBytes = Convert.FromBase64String(result[0].AttachmentBytesvtxt);
                    return Ok(imageBytes);

                }
                if (For == "DealerProfile")
                {

                    var result = _MaterialTestCertificateService.GetDealerProfiledata(id);
                    byte[] imageBytes = Convert.FromBase64String(result[0].AttachmentBytesvtxt);
                    return Ok(imageBytes);

                }

                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest(ex.Message);
            }
        }


        //use for downlaod sample Dealer Profile
        [HttpGet("DownloadSampleDealerProfileExcel")]
        public IActionResult DownloadSampleDealerProfileExcel()
        {
            try
            {
                List<CustomerProfileModel> lstmodel = new List<CustomerProfileModel>();
                CustomerProfileModel model = new CustomerProfileModel();
                model.CustomerCodevtxt = "";
                model.SecurityAmountdcl = 0;
                model.SupplySourcevtxt = "";
                model.Typevtxt = "";
                model.AssociatedWithPrism = "";
                model.NoOfMasonsAssociatedint = 0;
                model.WareHouseSqmt = 0;
                model.Staffvtxt = "";
                model.Potentialvtxt = "";
                model.OtherBusinessvtxt = "";
                model.MasonMeetConductedint = 0;
                lstmodel.Add(model);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("DealerProfile");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Customer Code"; 
                    worksheet.Cell(currentRow, 2).Value = "Type";
                    worksheet.Cell(currentRow, 3).Value = "WareHouse Sqmt";
                    worksheet.Cell(currentRow, 4).Value = "Staff";
                    worksheet.Cell(currentRow, 5).Value = "Potential";
                    worksheet.Cell(currentRow, 6).Value = "Other Business";
                    worksheet.Cell(currentRow, 7).Value = "MasonMeet Conducted";
                    worksheet.Cell(currentRow, 8).Value = "Supply Source";
                    foreach (var sale in lstmodel)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.CustomerCodevtxt; 
                        worksheet.Cell(currentRow, 2).Value = sale.Typevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.WareHouseSqmt;
                        worksheet.Cell(currentRow, 4).Value = sale.Staffvtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.Potentialvtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.OtherBusinessvtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.MasonMeetConductedint;
                        worksheet.Cell(currentRow, 8).Value = sale.SupplySourcevtxt;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "SampleDealerProfile.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }


        //USE FOR DealerProfile Upload
        [HttpPost("DealerProfileUpload"), DisableRequestSizeLimit]
        public IActionResult DealerProfileUpload()
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
                        List<CustomerProfileModel> lstEMployees = new List<CustomerProfileModel>();
                        if (finalRecords.Columns.Count == 8)
                        {
                            for (int i = 1; i < finalRecords.Rows.Count; i++)
                            {
                                CustomerProfileModel empmodel = new CustomerProfileModel();
                                empmodel.CustomerCodevtxt = finalRecords.Rows[i][0].ToString();
                                empmodel.Typevtxt = finalRecords.Rows[i][1].ToString();
                                if (!string.IsNullOrEmpty(  finalRecords.Rows[i][2].ToString())  )
                                {
                                    empmodel.WareHouseSqmt = Convert.ToDecimal(finalRecords.Rows[i][2].ToString());
                                }
                                else
                                {
                                    empmodel.WareHouseSqmt = 0;
                                }
                                empmodel.Staffvtxt = finalRecords.Rows[i][3].ToString();
                                empmodel.Potentialvtxt = finalRecords.Rows[i][4].ToString();
                                empmodel.OtherBusinessvtxt = finalRecords.Rows[i][5].ToString();
                                if (!string.IsNullOrEmpty(finalRecords.Rows[i][6].ToString()))
                                {
                                    empmodel.MasonMeetConductedint = Convert.ToInt32(finalRecords.Rows[i][6].ToString());
                                }
                                else
                                {
                                    empmodel.MasonMeetConductedint = 0;
                                }
                                empmodel.SupplySourcevtxt = finalRecords.Rows[i][7].ToString();
                                empmodel.CreatedByvtxt = "SA001";
                                
                                lstEMployees.Add(empmodel);
                            }
                            if (lstEMployees.Count > 0)
                            {
                                _MaterialTestCertificateService.DeleteTempDealerProfileData();
                                for (int k = 0; k < lstEMployees.Count; k++)
                                {
                                    long j = _MaterialTestCertificateService.InsertDealerProfileDataIntoTemp(lstEMployees[k]);
                                }
                                _MaterialTestCertificateService.InsertDealerProfileDataIntoMain();
                                List<CustomerProfileModel> lst = new List<CustomerProfileModel>();
                                lst = _MaterialTestCertificateService.GetTempDealerProfileData();
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
                return StatusCode(500, ex.Message);
            }
        }


        //use for downlaod error file 
        [HttpGet("DownloadErrorDealerProfileUpload")]
        public IActionResult DownloadErrorDealerProfileUpload()
        {
            try
            {
                List<CustomerProfileModel> custs = _MaterialTestCertificateService.GetTempDealerProfileData();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ErrorDealerProfileUpload");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Customer Code";
                    worksheet.Cell(currentRow, 2).Value = "Security Amount";
                    worksheet.Cell(currentRow, 3).Value = "Type";
                    worksheet.Cell(currentRow, 4).Value = "WareHouse Sqmt";
                    worksheet.Cell(currentRow, 5).Value = "Staff";
                    worksheet.Cell(currentRow, 6).Value = "Potential";
                    worksheet.Cell(currentRow, 7).Value = "Other Business";
                    worksheet.Cell(currentRow, 8).Value = "MasonMeet Conducted";
                    worksheet.Cell(currentRow, 9).Value = "Supply Source";
                    worksheet.Cell(currentRow, 10).Value = "Remarks";
                    foreach (var sale in custs)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.SecurityAmountdcl;
                        worksheet.Cell(currentRow, 3).Value = sale.Typevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.WareHouseSqmt;
                        worksheet.Cell(currentRow, 5).Value = sale.Staffvtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.Potentialvtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.OtherBusinessvtxt;
                        worksheet.Cell(currentRow, 8).Value = sale.MasonMeetConductedint;
                        worksheet.Cell(currentRow, 9).Value = sale.SupplySourcevtxt;
                        worksheet.Cell(currentRow, 10).Value = sale.Remarks;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ErrorDealerProfileUpload.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }


        //use for  Get Static DealerProfile Data
        [HttpGet("GetStaticDealerProfileData/{customercode}")]
        public IActionResult GetStaticDealerProfileData(string customercode)
        {
            try
            {
                return Ok(_MaterialTestCertificateService.GetDealerProfiledata(customercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        //use for  Update User Profile Image 
        [HttpPost("UpdateUserProfileImage")]
        public IActionResult UpdateUserProfileImage(CustomerProfileModel model)
        {
            try
            {
                return Ok(_MaterialTestCertificateService.UpdateUserProfileImage(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for GetCustomerProfileListCount
        [HttpGet("GetCustomerProfileListCount/{keyword}")]
        public IActionResult GetCustomerProfileListCount(string keyword)
        {
            try
            {
                return Ok(_MaterialTestCertificateService.GetCustomerProfileListCount(keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for GetCustomerProfileList
        [HttpGet("GetCustomerProfileList/{PageNo},{PageSize},{keyword}")]
        public IActionResult GetCustomerProfileList(int PageNo, int PageSize, string keyword)
        {
            try
            {
                return Ok(_MaterialTestCertificateService.GetCustomerProfileList(PageNo, PageSize, keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

    

        //use for downlaod customer profile data
        [HttpGet("DownloadCustomerProfileList/{keyword}")]
        public IActionResult DownloadCustomerProfileList(string keyword)
        {
            try
            {
                List<CustomerProfileModel> custs = _MaterialTestCertificateService.GetCustomerProfileListDownload(keyword);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("ErrorDealerProfileUpload");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Customer Code";
                    worksheet.Cell(currentRow, 2).Value = "Security Amount";
                    worksheet.Cell(currentRow, 3).Value = "Type";
                    worksheet.Cell(currentRow, 4).Value = "Associated With Prism";
                    worksheet.Cell(currentRow, 5).Value = "No Of MasonsAssociated";
                    worksheet.Cell(currentRow, 6).Value = "WareHouse Sqmt";
                    worksheet.Cell(currentRow, 7).Value = "Staff";
                    worksheet.Cell(currentRow, 8).Value = "Potential";
                    worksheet.Cell(currentRow, 9).Value = "Other Business";
                    worksheet.Cell(currentRow, 10).Value = "MasonMeet Conducted";
                    foreach (var sale in custs)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 2).Value = sale.SecurityAmountdcl;
                        worksheet.Cell(currentRow, 3).Value = sale.Typevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.AssociatedWithPrism;
                        worksheet.Cell(currentRow, 5).Value = sale.NoOfMasonsAssociatedint;
                        worksheet.Cell(currentRow, 6).Value = sale.WareHouseSqmt;
                        worksheet.Cell(currentRow, 7).Value = sale.Staffvtxt;
                        worksheet.Cell(currentRow, 8).Value = sale.Potentialvtxt;
                        worksheet.Cell(currentRow, 9).Value = sale.OtherBusinessvtxt;
                        worksheet.Cell(currentRow, 10).Value = sale.MasonMeetConductedint;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ErrorDealerProfileUpload.xlsx");
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
        //Use for DeleteMaterialTestCertificate
        [HttpGet("DeleteMaterialTestCertificate/{ID}")]
        public IActionResult DeleteMaterialTestCertificate(int  ID)
        {
            try
            {
                return Ok(_MaterialTestCertificateService.DeleteMaterialTestCertificate(ID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }


       
    }
}
