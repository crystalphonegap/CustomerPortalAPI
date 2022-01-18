using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CustomerPortalWebApi.Models;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PriceApprovalController : ControllerBase
    {
        private readonly IPriceApproval _PriceApproval;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public PriceApprovalController(IPriceApproval PriceApproval, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _PriceApproval = PriceApproval;
            _Checktokenservice = checktokenservice;
        }



        //use for Insert PriceApproval 
        [HttpPost("InsertPriceApproval")]
        public IActionResult InsertPriceApproval(PriceApprovalModel model)
        {
            try
            {
                return Ok(_PriceApproval.InsertPriceApproval(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        //use for Insert PriceApproval 
        [HttpPost("InsertFinalPriceApproval")]
        public IActionResult InsertFinalPriceApproval(PriceApprovalModel model)
        {
            try
            {
                return Ok(_PriceApproval.InsertFinalPriceApproval(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest(ex.Message);
            }
        }

        //use for Get PriceApproval List
        [HttpGet("GetPriceApprovalList/{Statusvtxt},{Createdby},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetPriceApprovalList(string Statusvtxt, string Createdby, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_PriceApproval.GetPriceApprovalData(Statusvtxt, Createdby,PageNo,PageSize,KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Get PriceApproval List Count
        [HttpGet("GetPriceApprovalListCount/{Statusvtxt},{Createdby},{KeyWord}")]
        public long GetPriceApprovalListCount(string Statusvtxt, string Createdby, string KeyWord)
        {
            try
            {
                return _PriceApproval.GetPriceApprovalDataCount(Statusvtxt, Createdby,KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Get PriceApproval By Id
        [HttpGet("GetPriceApprovalById/{id}")]
        public IActionResult GetPriceApprovalById(long id)
        {
            try
            {
                return Ok(_PriceApproval.GetPriceApprovalDataById(id));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for download excel file of Customer SalesData
        [HttpGet("PriceApprovalDownloadExcel/{Statusvtxt},{Createdby},{KeyWord}")]
        public IActionResult PriceApprovalDownloadExcel(string Statusvtxt, string Createdby, string KeyWord)
        {
            try
            {
                List<PriceApprovalModel> sales = _PriceApproval.DownloadPriceApprovalData(Statusvtxt,Createdby, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("PriceApproval");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Customer Code";
                    worksheet.Cell(currentRow, 3).Value = "Customer Name";
                    worksheet.Cell(currentRow, 4).Value = "ConsigneeCodevtxt";
                    worksheet.Cell(currentRow, 5).Value = "ConsigneeNamevtxt";
                    worksheet.Cell(currentRow, 6).Value = "Gradevtxt";
                    worksheet.Cell(currentRow, 7).Value = "TTENamevtxt";
                    worksheet.Cell(currentRow, 8).Value = "Tradedcl";
                    worksheet.Cell(currentRow, 9).Value = "NonTradedcl";
                    worksheet.Cell(currentRow, 10).Value = "Statusvtxt";
                    foreach (var sale in sales)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = sale.CustomerCodevtxt;
                        worksheet.Cell(currentRow, 3).Value = sale.CustomerNamevtxt;
                        worksheet.Cell(currentRow, 4).Value = sale.ConsigneeCodevtxt;
                        worksheet.Cell(currentRow, 5).Value = sale.ConsigneeNamevtxt;
                        worksheet.Cell(currentRow, 6).Value = sale.Gradevtxt;
                        worksheet.Cell(currentRow, 7).Value = sale.TTENamevtxt;
                        worksheet.Cell(currentRow, 8).Value = sale.Tradedcl;
                        worksheet.Cell(currentRow, 9).Value = sale.NonTradedcl;
                        worksheet.Cell(currentRow, 10).Value = sale.Statusvtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "PriceApproval.xlsx");
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
