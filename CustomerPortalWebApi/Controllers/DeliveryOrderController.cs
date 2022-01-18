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
    public class DeliveryOrderController : ControllerBase
    {
        private readonly IDeliveryOrderService _deliveryOrderService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public DeliveryOrderController(IDeliveryOrderService deliveryOrderService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _deliveryOrderService = deliveryOrderService;
            _Checktokenservice = checktokenservice;
        }

        //Dont Use
        [HttpGet("GetDeliveryOrder/{SoldToPartyCodevtxt},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetDeliveryOrder(string SoldToPartyCodevtxt, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_deliveryOrderService.GetDeliveryOrder(SoldToPartyCodevtxt, PageNo, PageSize, KeyWord));
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

        //Use for Delivery Main View Search
        [HttpGet("GetDeliveryOrderSearch/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetDeliveryOrderSearch(string fromdate, string todate, string status, string SoldToPartyCodevtxt, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_deliveryOrderService.GetDeliveryOrderSearch(fromdate, todate, status, SoldToPartyCodevtxt, PageNo, PageSize, KeyWord));
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

        //Use for Status Wise Count like partially,fully completed
        [HttpGet("GetDeliveryOrderStatusCount/{fromdate},{todate},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult GetDeliveryOrderStatusCount(string fromdate, string todate, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    return Ok(_deliveryOrderService.GetDeliveryOrderStatusCount(fromdate, todate, SoldToPartyCodevtxt, KeyWord));
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
        [HttpGet("Excel/{fromdate},{todate},{status},{SoldToPartyCodevtxt},{KeyWord}")]
        public IActionResult Excel(string fromdate, string todate, string status, string SoldToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCodevtxt))
                {
                    List<DeliveryOrder> deleiveryorderslist = _deliveryOrderService.GetDeliveryOrderDownload(fromdate, todate, status, SoldToPartyCodevtxt, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Delivery Order");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "SoldTo PartyCode";
                        worksheet.Cell(currentRow, 3).Value = "SoldTo PartyName";
                        worksheet.Cell(currentRow, 4).Value = "ShipTo Code";
                        worksheet.Cell(currentRow, 5).Value = "ShipTo Name";
                        worksheet.Cell(currentRow, 6).Value = "Sales OrderNo";
                        worksheet.Cell(currentRow, 7).Value = "Delivery OrderNo";
                        worksheet.Cell(currentRow, 8).Value = "Invoice Number";
                        worksheet.Cell(currentRow, 9).Value = "Delivery Quantity";
                        worksheet.Cell(currentRow, 10).Value = "Delivery Date";
                        worksheet.Cell(currentRow, 11).Value = "Transporter";
                        worksheet.Cell(currentRow, 12).Value = "TruckNo";
                        worksheet.Cell(currentRow, 13).Value = "LR Number";
                        worksheet.Cell(currentRow, 14).Value = "LR Date";
                        worksheet.Cell(currentRow, 15).Value = "Delivery Status";
                        worksheet.Cell(currentRow, 16).Value = "Destination Name";
                        foreach (var deleiverys in deleiveryorderslist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = deleiverys.SoldToPartyCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = deleiverys.SoldTopPartyNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = deleiverys.ShipToPartyCodevtxt;
                            worksheet.Cell(currentRow, 5).Value = deleiverys.ShipToPartyNamevtxt;
                            worksheet.Cell(currentRow, 6).Value = deleiverys.SalesOrderNovtxt;
                            worksheet.Cell(currentRow, 7).Value = deleiverys.DeliveryOrderNovtxt;
                            worksheet.Cell(currentRow, 8).Value = deleiverys.InvoiceNumbervtxt;
                            worksheet.Cell(currentRow, 9).Value = deleiverys.Qtyint;
                            worksheet.Cell(currentRow, 10).Value = deleiverys.DeliveryDatedate;
                            worksheet.Cell(currentRow, 11).Value = deleiverys.Transportervtxt;
                            worksheet.Cell(currentRow, 12).Value = deleiverys.TruckNovtxt;
                            worksheet.Cell(currentRow, 13).Value = deleiverys.Lrnumbervtxt;
                            worksheet.Cell(currentRow, 14).Value = deleiverys.Lrdatedate;
                            worksheet.Cell(currentRow, 15).Value = deleiverys.DeliveryStatusvtxt;
                            worksheet.Cell(currentRow, 16).Value = deleiverys.dest_nm;
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

        //Use for Detail view
        [HttpGet("GetDeliveryOrderByOrderNo/{Orderno}")]
        public IActionResult GetDeliveryOrderByOrderNo(string Orderno)
        {
            try
            {
                return Ok(_deliveryOrderService.GetDeliveryOrderByOrderNo(Orderno));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("getAllDeliveryOrderDataBySalesOrderNo/{Orderno}")]
        public IActionResult getAllDeliveryOrderDataBySalesOrderNo(string Orderno)
        {
            try
            {
                return Ok(_deliveryOrderService.getAllDeliveryOrderDataBySalesOrderNo(Orderno));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Header View Data
        [HttpGet("GetDeliveryOrderHeaderByOrderNo/{Orderno}")]
        public IActionResult GetDeliveryOrderHeaderByOrderNo(string Orderno)
        {
            try
            {
                return Ok(_deliveryOrderService.GetDeliveryOrderHeaderByOrderNo(Orderno));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Delivery Count
        [HttpGet("GetDeliveryOrderCount/{fromdate},{todate},{status},{SoldToPartyCode},{KeyWord}")]
        public long GetDeliveryOrderCount(string fromdate, string todate, string status, string SoldToPartyCode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), SoldToPartyCode))
                {
                    return _deliveryOrderService.GetDeliveryCount(fromdate, todate, status, SoldToPartyCode, KeyWord);
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

        [HttpGet("getAllOrdersCountforCustomerDashboard/{UserCode}")]
        public long getAllOrdersCountforCustomerDashboard(string UserCode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), UserCode))
                {
                    return _deliveryOrderService.GetDeliveryCount(UserCode);
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

        [HttpPut("SetDeliveryStatus")]
        public IActionResult SetDeliveryStatus(DeliveryOrder model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenByID(authorize[1].Trim(), model.Idbint))
                {
                    var DeliveryOrder = new DeliveryOrder()
                    {
                        DeliveryOrderNovtxt = model.DeliveryOrderNovtxt,
                        Status = model.Status,
                        Remark = model.Remark,
                        OrderRecivedDate = model.OrderRecivedDate
                    };
                    return Ok(_deliveryOrderService.SetDeliveryStatus(DeliveryOrder));
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

        //Use for ShipTo Delivery Orders Count
        [HttpGet("GetShipToDeliveryOrderCount/{fromdate},{todate},{status},{ShipToPartyCode},{KeyWord}")]
        public long GetShipToDeliveryOrderCount(string fromdate, string todate, string status, string ShipToPartyCode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), ShipToPartyCode))
                {
                    return _deliveryOrderService.GetShipToDeliveryOrderCount(fromdate, todate, status, ShipToPartyCode, KeyWord);
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

        //Use for ShipTo Delivery Orders Search
        [HttpGet("GetShipToDeliveryOrderSearch/{fromdate},{todate},{status},{ShipToPartyCodevtxt},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetShipToDeliveryOrderSearch(string fromdate, string todate, string status, string ShipToPartyCodevtxt, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), ShipToPartyCodevtxt))
                {
                    return Ok(_deliveryOrderService.GetShipToDeliveryOrderSearch(fromdate, todate, status, ShipToPartyCodevtxt, PageNo, PageSize, KeyWord));
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

        //Use for ShipTo Delivery Orders Export to Excel
        [HttpGet("ShipToDeliveryOrdersExportTOExcel/{fromdate},{todate},{status},{ShipToPartyCodevtxt},{KeyWord}")]
        public IActionResult ShipToDeliveryOrdersExportTOExcel(string fromdate, string todate, string status, string ShipToPartyCodevtxt, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), ShipToPartyCodevtxt))
                {
                    List<DeliveryOrder> deleiveryorderslist = _deliveryOrderService.GetShipToDeliveryOrderDownload(fromdate, todate, status, ShipToPartyCodevtxt, KeyWord);
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Delivery Order");
                        var currentRow = 1;
                        var srno = 1;
                        worksheet.Cell(currentRow, 1).Value = "Sr No";
                        worksheet.Cell(currentRow, 2).Value = "SoldTo PartyCode";
                        worksheet.Cell(currentRow, 3).Value = "SoldTo PartyName";
                        worksheet.Cell(currentRow, 4).Value = "ShipTo Code";
                        worksheet.Cell(currentRow, 5).Value = "ShipTo Name";
                        worksheet.Cell(currentRow, 6).Value = "Sales OrderNo";
                        worksheet.Cell(currentRow, 7).Value = "Delivery OrderNo";
                        worksheet.Cell(currentRow, 8).Value = "Invoice Number";
                        worksheet.Cell(currentRow, 9).Value = "Delivery Quantity";
                        worksheet.Cell(currentRow, 10).Value = "Delivery Date";
                        worksheet.Cell(currentRow, 11).Value = "Transporter";
                        worksheet.Cell(currentRow, 12).Value = "TruckNo";
                        worksheet.Cell(currentRow, 13).Value = "LR Number";
                        worksheet.Cell(currentRow, 14).Value = "LR Date";
                        worksheet.Cell(currentRow, 15).Value = "Delivery Status";
                        foreach (var deleiverys in deleiveryorderslist)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = srno++;
                            worksheet.Cell(currentRow, 2).Value = deleiverys.SoldToPartyCodevtxt;
                            worksheet.Cell(currentRow, 3).Value = deleiverys.SoldTopPartyNamevtxt;
                            worksheet.Cell(currentRow, 4).Value = deleiverys.ShipToPartyCodevtxt;
                            worksheet.Cell(currentRow, 5).Value = deleiverys.ShipToPartyNamevtxt;
                            worksheet.Cell(currentRow, 6).Value = deleiverys.SalesOrderNovtxt;
                            worksheet.Cell(currentRow, 7).Value = deleiverys.DeliveryOrderNovtxt;
                            worksheet.Cell(currentRow, 8).Value = deleiverys.InvoiceNumbervtxt;
                            worksheet.Cell(currentRow, 9).Value = deleiverys.Qtyint;
                            worksheet.Cell(currentRow, 10).Value = deleiverys.DeliveryDatedate;
                            worksheet.Cell(currentRow, 11).Value = deleiverys.Transportervtxt;
                            worksheet.Cell(currentRow, 12).Value = deleiverys.TruckNovtxt;
                            worksheet.Cell(currentRow, 13).Value = deleiverys.Lrnumbervtxt;
                            worksheet.Cell(currentRow, 14).Value = deleiverys.Lrdatedate;
                            worksheet.Cell(currentRow, 15).Value = deleiverys.DeliveryStatusvtxt;
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