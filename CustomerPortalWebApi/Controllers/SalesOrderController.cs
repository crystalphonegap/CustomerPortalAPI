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
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public SalesOrderController(ISalesOrderService salesOrderService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _salesOrderService = salesOrderService;
            _Checktokenservice = checktokenservice;
        }

        //Dont use
        [HttpGet("GetSalesOrder/{SoldToPartyCodevtxt},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetSalesOrder(string SoldToPartyCodevtxt, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_salesOrderService.GetSalesOrder(SoldToPartyCodevtxt, PageNo, PageSize, KeyWord));
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

        //Use For dashboard count
        [HttpGet("GetSalesOrderCount/{SoldToPartyCode},{KeyWord}")]
        public long GetSalesOrderCount(string SoldToPartyCode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCode))
                {
                    return _salesOrderService.GetSalesCount(SoldToPartyCode, KeyWord);
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

        //Use For Details View
        [HttpGet("getAllSalesOrderDataByOrderNo/{Orderno}")]
        public IActionResult getAllSalesOrderDataByOrderNo(string Orderno)
        {
            try
            {
                return Ok(_salesOrderService.getAllSalesOrderDataByOrderNo(Orderno));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use For Header View
        [HttpGet("getAllSalesOrderHeaderDataByOrderNo/{Orderno}")]
        public IActionResult getSalesOrderHeaderDataByOrderNo(string Orderno)
        {
            try
            {
                return Ok(_salesOrderService.getSalesOrderHeaderDataByOrderNo(Orderno));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use For Status wise Count
        [HttpGet("GetSalesOrderStatuswiseCount/{fromdate},{todate},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult GetSalesOrderStatuswiseCount(string fromdate, string todate, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_salesOrderService.GetSalesOrderStatuswiseCount(fromdate, todate, SoldToPartyCodevtxt, KeyWord));
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

        //Use for Search
        [HttpGet("GetSalesOrderSearch/{fromdate},{todate},{status},{PageNo},{PageSize},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult GetSalesOrderSearch(string fromdate, string todate, string status, int PageNo, int PageSize, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_salesOrderService.GetSalesOrderSearch(fromdate, todate, status, SoldToPartyCodevtxt, PageNo, PageSize, KeyWord));
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

        //Use for Export to excel
        [HttpGet("Excel/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult Excel(string fromdate, string todate, string status, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    List<SalesOrder> Salesorderslist = _salesOrderService.GetSalesOrderDownload(fromdate, todate, status, SoldToPartyCodevtxt, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sales Order");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "SoldTo PartyCode";
                        worksheet.Cell(currentRow, 3).Value = "SoldTo PartyName";
                        worksheet.Cell(currentRow, 4).Value = "ShipTo Code";
                        worksheet.Cell(currentRow, 5).Value = "ShipTo Name";
                        worksheet.Cell(currentRow, 6).Value = "Sales OrderNo";
                        worksheet.Cell(currentRow, 7).Value = "Order Quantity";
                        worksheet.Cell(currentRow, 8).Value = "Delivery Quantity";
                        worksheet.Cell(currentRow, 9).Value = "Delivery Date";
                        worksheet.Cell(currentRow, 10).Value = "Salas Order Status";
                        foreach (var salesOrder in Salesorderslist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = salesOrder.SoldToPartyCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = salesOrder.SoldTopPartyNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = salesOrder.ShipToPartyCodevtxt;
                            worksheet.Cell(currentRow, 5).Value = salesOrder.ShipToPartyNamevtxt;
                            worksheet.Cell(currentRow, 6).Value = salesOrder.SalesOrderNumbervtxt;
                            worksheet.Cell(currentRow, 7).Value = salesOrder.OrderQuantityint;
                            worksheet.Cell(currentRow, 8).Value = salesOrder.DeliveryQuantityint;
                            worksheet.Cell(currentRow, 9).Value = salesOrder.DeliveryDatedate;
                            worksheet.Cell(currentRow, 10).Value = salesOrder.SalesOrderStatusvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "SalesOrders.xlsx");
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

        // Use for Search Count
        [HttpGet("GetSalesOrderSearchCount/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{KeyWord}")]
        public long GetSalesOrderSearchCount(string fromdate, string todate, string status, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return _salesOrderService.GetSalesOrderSearchCount(fromdate, todate, status, SoldToPartyCodevtxt, KeyWord);
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

        //search for Blocked order List
        [HttpGet("GetOrderBlockedSalesOrderSearch/{usercode},{usertype},{fromdate},{todate},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetOrderBlockedSalesOrderSearch(string usercode, string usertype, string fromdate, string todate, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return Ok(_salesOrderService.GetBlockedSalesOrderSearch(usercode, usertype, fromdate, todate, PageNo, PageSize, KeyWord));
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

        //search for Blocked order count
        [HttpGet("GetOrderBlockedSalesOrderCount/{usercode},{usertype},{fromdate},{todate},{KeyWord}")]
        public long GetOrderBlockedSalesOrderCount(string usercode, string usertype, string fromdate, string todate, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return _salesOrderService.GetBlockedSalesOrderountC(usercode, usertype, fromdate, todate, KeyWord);
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

        //Download for Blocked order count
        [HttpGet("GetOrderBlockedSalesOrderDownload/{usercode},{usertype},{fromdate},{todate},{KeyWord}")]
        public IActionResult GetOrderBlockedSalesOrderDownload(string usercode, string usertype, string fromdate, string todate, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    List<SalesOrder> Salesorderslist = _salesOrderService.GetBlockedSalesOrderDownload(usercode, usertype, fromdate, todate, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sales Order");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "SoldTo PartyCode";
                        worksheet.Cell(currentRow, 3).Value = "SoldTo PartyName";
                        worksheet.Cell(currentRow, 4).Value = "ShipTo Code";
                        worksheet.Cell(currentRow, 5).Value = "ShipTo Name";
                        worksheet.Cell(currentRow, 6).Value = "Sales OrderNo";
                        worksheet.Cell(currentRow, 7).Value = "Order Quantity";
                        worksheet.Cell(currentRow, 8).Value = "Reason For Block";
                        foreach (var salesOrder in Salesorderslist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = salesOrder.SoldToPartyCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = salesOrder.SoldTopPartyNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = salesOrder.ShipToPartyCodevtxt;
                            worksheet.Cell(currentRow, 5).Value = salesOrder.ShipToPartyNamevtxt;
                            worksheet.Cell(currentRow, 6).Value = salesOrder.SalesOrderNumbervtxt;
                            worksheet.Cell(currentRow, 7).Value = salesOrder.OrderQuantityint;
                            worksheet.Cell(currentRow, 8).Value = salesOrder.OrderBlockListDescvtxt;
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(
                                content,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "BlockedOrderList.xlsx");
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