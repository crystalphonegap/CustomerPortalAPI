using ClosedXML.Excel;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
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
    public class CFAgentController : ControllerBase
    {
        private readonly ICFAgentServices _ICFAgentServices;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public CFAgentController(ICFAgentServices CFAgentServices, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _ICFAgentServices = CFAgentServices;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetDashBoardCount/{usercode}")]
        public IActionResult GetDashBoardCount(string usercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetCFAgentDashboardCounts(usercode));
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

        [HttpGet("GetAllOrdersByCFCode/{fromdate},{todate},{status},{usertype},{usercode},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetAllOrdersByCFCode(string fromdate, string todate, string status, string usertype, string usercode, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetAllOrderList(fromdate, todate, status, usertype, usercode, PageNo, PageSize, KeyWord));
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

        [HttpGet("GetAllOrdersByCFCodeCount/{fromdate},{todate},{status},{usertype},{usercode},{KeyWord}")]
        public IActionResult GetAllOrdersByCFCodeCount(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetAllOrderCount(fromdate, todate, status, usertype, usercode, KeyWord));
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

        [HttpGet("GetAllPendingOrdersByCFCode/{fromdate},{todate},{usertype},{usercode},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetAllPendingOrdersByCFCode(string fromdate, string todate, string usertype, string usercode, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetAllOrderList(fromdate, todate, "Pending", usertype, usercode, PageNo, PageSize, KeyWord));
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

        [HttpGet("GetAllPendingOrdersByCFCodeCount/{fromdate},{todate},{usertype},{usercode},{KeyWord}")]
        public IActionResult GetAllPendingOrdersByCFCodeCount(string fromdate, string todate, string usertype, string usercode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetAllOrderCount(fromdate, todate, "Pending", usertype, usercode, KeyWord));
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

        //Use for Export to Excel
        [HttpGet("ExportToExcel/{fromdate},{todate},{status},{usertype},{usercode},{KeyWord}")]
        public IActionResult ExportToExcel(string fromdate, string todate, string status, string usertype, string usercode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    List<OrderHeaderModel> orderslist = _ICFAgentServices.GetAllOrderDownload(fromdate, todate, status, usertype, usercode, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Order");
                        var currentRow = 1;
                        var srNo = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "SoldTo PartyCode";
                        worksheet.Cell(currentRow, 3).Value = "SoldTo PartyName";
                        worksheet.Cell(currentRow, 4).Value = "ShipTo Code";
                        worksheet.Cell(currentRow, 5).Value = "ShipTo Name";
                        worksheet.Cell(currentRow, 6).Value = "Web Order No";
                        worksheet.Cell(currentRow, 7).Value = "Web Order Date";
                        worksheet.Cell(currentRow, 8).Value = "Order No";
                        worksheet.Cell(currentRow, 9).Value = "Order Dater";
                        worksheet.Cell(currentRow, 10).Value = "ShipTo Address";
                        worksheet.Cell(currentRow, 11).Value = "Delivery Address";
                        worksheet.Cell(currentRow, 12).Value = "Quantity";
                        worksheet.Cell(currentRow, 13).Value = "Net Value";
                        worksheet.Cell(currentRow, 14).Value = "Status";
                        foreach (var orders in orderslist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srNo++;
                            worksheet.Cell(currentRow, 2).Value = orders.CustomerCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = orders.CustomerNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = orders.ShipToCodevtxt;
                            worksheet.Cell(currentRow, 5).Value = orders.ShipToNamevtxt;
                            worksheet.Cell(currentRow, 6).Value = orders.OrderNovtxt;
                            worksheet.Cell(currentRow, 7).Value = orders.OrderDatedate;
                            worksheet.Cell(currentRow, 8).Value = orders.SAPOrderNovtxt;
                            worksheet.Cell(currentRow, 9).Value = orders.SAPOrderDatedate;
                            worksheet.Cell(currentRow, 10).Value = orders.ShipToAddressvtxt;
                            worksheet.Cell(currentRow, 11).Value = orders.DeliveryAddressvtxt;
                            worksheet.Cell(currentRow, 12).Value = orders.TotalOrderQuantityint;
                            worksheet.Cell(currentRow, 13).Value = orders.TotalNetValuedcl;
                            worksheet.Cell(currentRow, 14).Value = orders.Statusvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "OrderList.xlsx");
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

        [HttpGet("GetAllSubmittedOrdersByCFCode/{fromdate},{todate},{usertype},{usercode},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetAllSubmittedOrdersByCFCode(string fromdate, string todate, string usertype, string usercode, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetAllOrderList(fromdate, todate, "Pending", usertype, usercode, PageNo, PageSize, KeyWord));
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

        //use for SPFA ledger
        [HttpGet("GetSPCFALedger/{usercode},{pageno},{pagesize}")]
        public IActionResult GetSPCFALedger(string usercode, int pageno, int pagesize)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetSPCFALedger(usercode, pageno, pagesize));
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

        //use for SPCFA ledgercount
        [HttpGet("GetSPCFALedgerCount/{usercode}")]
        public IActionResult GetSPCFALedgerCount(string usercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_ICFAgentServices.GetSPCFALedgerCount(usercode));
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

        //use for SPCFA ledger Export To Excel
        [HttpGet("GetSPCFALedgerExportToExcel/{usercode}")]
        public IActionResult GetSPCFALedgerExportToExcel(string usercode)
        {
            try
            {
                List<SPCFALedger> custs = _ICFAgentServices.GetSPCFALedgerExportToExcel(usercode);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Ledger");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Posting Date";
                    worksheet.Cell(currentRow, 3).Value = "Document No";
                    worksheet.Cell(currentRow, 4).Value = "Document Date";
                    worksheet.Cell(currentRow, 5).Value = "Ref No";
                    worksheet.Cell(currentRow, 6).Value = "Act.Inv.Qty";
                    worksheet.Cell(currentRow, 7).Value = "Document Type";
                    worksheet.Cell(currentRow, 8).Value = "Document Desc";
                    worksheet.Cell(currentRow, 9).Value = "TDS Amount";
                    worksheet.Cell(currentRow, 10).Value = "Balance Amount";

                    foreach (var cust in custs)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = cust.PostingDatedate;
                        worksheet.Cell(currentRow, 3).Value = cust.DocumentNovtxt;
                        worksheet.Cell(currentRow, 4).Value = cust.DocumentDatedate; //
                        worksheet.Cell(currentRow, 5).Value = cust.RefDocumentNovtxt;  //
                        worksheet.Cell(currentRow, 6).Value = cust.Quantitydcl;
                        worksheet.Cell(currentRow, 7).Value = cust.DocumentTypevtxt;
                        worksheet.Cell(currentRow, 8).Value = cust.ItemDescvtxt;
                        worksheet.Cell(currentRow, 9).Value = cust.TDSdcl;
                        worksheet.Cell(currentRow, 10).Value = cust.Balancedcl;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "Ledger.xlsx");
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