using ClosedXML.Excel;
using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class InvoiceMasterController : ControllerBase
    {
        private readonly IInvoiceMasterService _invoiceMasterService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public InvoiceMasterController(IInvoiceMasterService invoiceMasterService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _invoiceMasterService = invoiceMasterService;
            _Checktokenservice = checktokenservice;
        }

        //Don't Use
        [HttpGet("GetInvoice/{SoldToPartyCodevtxt},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetInvoice(string SoldToPartyCodevtxt, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_invoiceMasterService.GetInvoice(SoldToPartyCodevtxt, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Don't Use
        [HttpGet("GetInvoiceCount/{SoldToPartyCode},{KeyWord}")]
        public long GetInvoiceCount(string SoldToPartyCode, string KeyWord)
        {
            try
            {
                return _invoiceMasterService.GetInvoiceCount(SoldToPartyCode, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        // Use For Deatail View Data
        [HttpGet("getAllInvoiceDataByInvoiceNo/{InvoiceNo}")]
        public IActionResult getAllInvoiceDataByInvoiceNo(string InvoiceNo)
        {
            try
            {
                return Ok(_invoiceMasterService.getAllInvoiceDataByInvoiceNo(InvoiceNo));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // Use For Header View Data
        [HttpGet("getInvoiceHeaderDataByInvoiceNo/{InvoiceNo}")]
        public IActionResult getInvoiceHeaderDataByInvoiceNo(string InvoiceNo)
        {
            try
            {
                return Ok(_invoiceMasterService.getInvoiceHeaderDataByInvoiceNo(InvoiceNo));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        // Use For Invoice Search
        [HttpGet("GetInvoiceSearch/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetInvoiceSearch(string fromdate, string todate, string status, string SoldToPartyCodevtxt, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_invoiceMasterService.GetInvoiceSearch(fromdate, todate, status, SoldToPartyCodevtxt, PageNo, PageSize, KeyWord));
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

        //Use For Download
        [HttpGet("GetInvoiceDownload/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult GetInvoiceDownload(string fromdate, string todate, string status, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    List<InvoiceMaster> InvoiceList = _invoiceMasterService.GetInvoiceDownload(fromdate, todate, status, SoldToPartyCodevtxt, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Invoice");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "SoldTo PartyCode";
                        worksheet.Cell(currentRow, 3).Value = "SoldTo PartyName";
                        worksheet.Cell(currentRow, 4).Value = "ShipTo Code";
                        worksheet.Cell(currentRow, 5).Value = "ShipTo Name";
                        worksheet.Cell(currentRow, 6).Value = "Sales OrderNo";
                        worksheet.Cell(currentRow, 7).Value = "Invoice No";
                        worksheet.Cell(currentRow, 8).Value = "Quantity";
                        worksheet.Cell(currentRow, 9).Value = "Invoice Date";
                        worksheet.Cell(currentRow, 10).Value = "Invoice Amount";
                        worksheet.Cell(currentRow, 11).Value = "Invoice Status";
                        foreach (var salesOrder in InvoiceList)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = salesOrder.SoldToPartyCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = salesOrder.SoldTopPartyNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = salesOrder.ShipToPartyCodevtxt;
                            worksheet.Cell(currentRow, 5).Value = salesOrder.ShipToPartyNamevtxt;
                            worksheet.Cell(currentRow, 6).Value = salesOrder.SalesOrderNumbervtxt;
                            worksheet.Cell(currentRow, 7).Value = salesOrder.InvoiceDocumentNovtxt;
                            worksheet.Cell(currentRow, 8).Value = salesOrder.InvoiceQuantityint;
                            worksheet.Cell(currentRow, 9).Value = salesOrder.BillingDatedate;
                            worksheet.Cell(currentRow, 10).Value = salesOrder.BillingValuedcl;
                            worksheet.Cell(currentRow, 11).Value = salesOrder.Statusvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "Invoice.xlsx");
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

        // Use For Search Count
        [HttpGet("GetInvoiceSearchCount/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult GetInvoiceSearchCount(string fromdate, string todate, string status, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_invoiceMasterService.GetInvoiceSearchCount(fromdate, todate, status, SoldToPartyCodevtxt, KeyWord));
                }
                else
                {
                    return Ok(0);
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Status wise count
        [HttpGet("GetInvoiceStatuswiseCount/{fromdate},{todate},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult GetInvoiceStatuswiseCount(string fromdate, string todate, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_invoiceMasterService.GetInvoiceStatuswiseCount(fromdate, todate, SoldToPartyCodevtxt, KeyWord));
                }
                else
                {
                    return Ok(0);
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