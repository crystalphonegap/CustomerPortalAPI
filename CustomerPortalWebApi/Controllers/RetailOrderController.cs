using ClosedXML.Excel;
using CustomerPortalWebApi.Entities;
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
    public class RetailOrderController : ControllerBase
    {
        private readonly IRetailOrderService _RetailOrderService;
        private readonly ILogger _ILogger;

        public RetailOrderController(IRetailOrderService RetailOrderService, ILogger ILoggerservice)
        {
            _ILogger = ILoggerservice;
            _RetailOrderService = RetailOrderService;
        }

        [HttpGet("GetRetailOrderDetailsByOrderID/{OrderID}")]
        public IActionResult GetRetailOrderDetailsByOrderID(long OrderID)
        {
            try
            {
                return Ok(_RetailOrderService.GetRetailOrderDetailsByOrderID(OrderID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("InsertRetailOrder")]
        public ActionResult InsertRetailOrder(RetailOrder model)
        {
            try
            {
                var RetailOrder = new RetailOrder()
                {
                    OrderNovtxt = model.OrderNovtxt,
                    OrderDatedate = model.OrderDatedate,
                    MaterialCodevtxt = model.MaterialCodevtxt,
                    MaterialDescvtxt = model.MaterialDescvtxt,
                    Quantitydcl = model.Quantitydcl,
                    RetailerCodevtxt = model.RetailerCodevtxt,
                    UOMvtxt = model.UOMvtxt,
                    TotalOrderQuantityKgsint = model.TotalOrderQuantityKgsint,
                    TotalOrderQuantityMTint = model.TotalOrderQuantityMTint,
                    DeliveryAddressvtxt = model.DeliveryAddressvtxt,
                    Statusvtxt = model.Statusvtxt,
                    CreatedBytxt = model.CreatedBytxt,
                    CreatedDatetxt = DateTime.Now
                };
                return Ok(_RetailOrderService.Insert(RetailOrder));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPut("UpdateRetailOrder")]
        public ActionResult UpdateRetailOrder(RetailOrder model)
        {
            try
            {
                var RetailOrder = new RetailOrder()
                {
                    IDbint = model.IDbint,
                    OrderNovtxt = model.OrderNovtxt,
                    OrderDatedate = model.OrderDatedate,
                    MaterialCodevtxt = model.MaterialCodevtxt,
                    MaterialDescvtxt = model.MaterialDescvtxt,
                    Quantitydcl = model.Quantitydcl,
                    UOMvtxt = model.UOMvtxt,
                    TotalOrderQuantityKgsint = model.TotalOrderQuantityKgsint,
                    TotalOrderQuantityMTint = model.TotalOrderQuantityMTint,
                    DeliveryAddressvtxt = model.DeliveryAddressvtxt,
                    Statusvtxt = model.Statusvtxt,
                    CreatedBytxt = model.CreatedBytxt,
                    CreatedDatetxt = DateTime.Now
                };
                return Ok(_RetailOrderService.update(RetailOrder));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetRetailOrderSearch/{fromdate},{todate},{status},{PageNo},{PageSize},{UserCode},{UserType},{KeyWord}")]
        public IActionResult GetRetailOrderSearch(string fromdate, string todate, string status, int PageNo, int PageSize, string UserCode, string UserType, string KeyWord)
        {
            try
            {
                return Ok(_RetailOrderService.GetRetailOrderSearch(fromdate, todate, status, UserCode, UserType, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetRetailSearch/{status},{PageNo},{PageSize},{UserCode},{UserType},{KeyWord}")]
        public IActionResult GetRetailSearch(string status, int PageNo, int PageSize, string UserCode, string UserType, string KeyWord)
        {
            try
            {
                return Ok(_RetailOrderService.GetRetailSearch(status, UserCode, UserType, PageNo, PageSize, KeyWord, "List"));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetRetailCount/{status},{UserCode},{UserType},{KeyWord}")]
        public IActionResult GetRetailCount(string status, string UserCode, string UserType, string KeyWord)
        {
            try
            {
                return Ok(_RetailOrderService.GetRetailCount(status, UserCode, UserType, KeyWord, "Count"));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetRetailDownload/{status},{UserCode},{UserType},{KeyWord}")]
        public IActionResult GetRetailDownload(string status, string UserCode, string UserType, string KeyWord)
        {
            try
            {
                List<ShipToModel> Retailslist = _RetailOrderService.GetRetailDownload(status, UserCode, UserType, KeyWord, "Download");
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Retail Order");
                    var currentRow = 1;
                    var srno = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Retaile Code";
                    worksheet.Cell(currentRow, 3).Value = "Retaile Name";
                    worksheet.Cell(currentRow, 4).Value = "Address";

                    foreach (var Retail in Retailslist)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srno++;
                        worksheet.Cell(currentRow, 2).Value = Retail.ShipToCodevtxt;
                        worksheet.Cell(currentRow, 3).Value = Retail.ShipToNamevtxt;
                        worksheet.Cell(currentRow, 4).Value = Retail.Addressvtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "Retail.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetRetailOrderSearchCount/{fromdate},{todate},{status},{UserCode},{UserType},{KeyWord}")]
        public long GetRetailOrderSearchCount(string fromdate, string todate, string status, string UserCode, string UserType, string KeyWord)
        {
            try
            {
                return _RetailOrderService.GetRetailOrderSearchCount(fromdate, todate, status, UserCode, UserType, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //Use for Export to excel
        [HttpGet("Excel/{fromdate},{todate},{status},{UserCode},{UserType},{KeyWord}")]
        public IActionResult Excel(string fromdate, string todate, string status, string UserCode, string UserType, string KeyWord)
        {
            try
            {
                List<RetailOrder> Retailorderslist = _RetailOrderService.GetRetailOrderDownload(fromdate, todate, status, UserCode, UserType, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Retail Order");
                    var currentRow = 1;
                    var srno = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Order No";
                    worksheet.Cell(currentRow, 3).Value = "Order Date";
                    //worksheet.Cell(currentRow, 4).Value = "Dealer Code";
                    //worksheet.Cell(currentRow, 5).Value = "Dealer Name";
                    worksheet.Cell(currentRow, 4).Value = "Material Code";
                    worksheet.Cell(currentRow, 5).Value = "Quantity";
                    worksheet.Cell(currentRow, 6).Value = "Unit Of Mesurement";
                    worksheet.Cell(currentRow, 7).Value = "Retailer Code";
                    worksheet.Cell(currentRow, 8).Value = "Retailer Name";
                    foreach (var RetailOrder in Retailorderslist)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srno++;
                        worksheet.Cell(currentRow, 2).Value = RetailOrder.OrderNovtxt;
                        worksheet.Cell(currentRow, 3).Value = RetailOrder.OrderDatedate;
                        //worksheet.Cell(currentRow, 4).Value = RetailOrder.DealerCodevtxt;
                        //worksheet.Cell(currentRow, 5).Value = RetailOrder.DealerNamevtxt;
                        worksheet.Cell(currentRow, 4).Value = RetailOrder.MaterialCodevtxt;
                        worksheet.Cell(currentRow, 5).Value = RetailOrder.Quantitydcl;
                        worksheet.Cell(currentRow, 6).Value = RetailOrder.UOMvtxt;
                        worksheet.Cell(currentRow, 7).Value = RetailOrder.RetailerCodevtxt;
                        worksheet.Cell(currentRow, 8).Value = RetailOrder.RetailerNamevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "RetailOrders.xlsx");
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