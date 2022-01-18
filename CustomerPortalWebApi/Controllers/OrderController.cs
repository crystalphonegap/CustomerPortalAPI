using ClosedXML.Excel;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _Orderservice;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public OrderController(IOrderService OrderService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _Orderservice = OrderService;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetReqOrderNo")]
        public IActionResult GetReqOrderNo()
        {
            try
            {
                string OrdNo = "";
                OrdNo = _Orderservice.GetOrderNo();
                OrdNo = GetReqNo(OrdNo);
                return Ok(_Orderservice.GetReqOrderNo(OrdNo));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }
        
        [AllowAnonymous]
        [HttpPost("InsertOrderHeader")]
        public ActionResult InsertOrderHeader(OrderHeaderModel model)
        {
            try
            {
                //string Token = Request.Headers["Authorization"];
                //string[] authorize = Token.Split(" ");
                //if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), model.CreatedByvtxt))
                //{
                    string OrdNo = "";
                    OrdNo = _Orderservice.GetOrderNo();
                    OrdNo = GetReqNo(OrdNo);
                    var OrderHeader = new OrderHeaderModel()
                    {
                        OrderNovtxt = OrdNo,
                        OrderDatedate = model.OrderDatedate,
                        RefNovtxt = model.RefNovtxt,
                        RefDatedate = model.RefDatedate,
                        SAPOrderNovtxt = model.SAPOrderNovtxt,
                        SAPOrderDatedate = model.SAPOrderDatedate,
                        CustomerCodevtxt = model.CustomerCodevtxt,
                        CustomerNamevtxt = model.CustomerNamevtxt,
                        Divisionvtxt = model.Divisionvtxt,
                        ShipToCodevtxt = model.ShipToCodevtxt,
                        ShipToNamevtxt = model.ShipToNamevtxt,
                        ShipToAddressvtxt = model.ShipToAddressvtxt,
                        DeliveryAddressvtxt = model.DeliveryAddressvtxt,
                        TotalNetValuedcl = model.TotalNetValuedcl,
                        TotalOrderQuantityint = model.TotalOrderQuantityint,
                        TotalOrderQuantityKgsint = model.TotalOrderQuantityKgsint,
                        TotalOrderQuantityMTint = model.TotalOrderQuantityMTint,
                        SAPStatusvtxt = model.SAPStatusvtxt,
                        OtherCharges1dcl = model.OtherCharges1dcl,
                        OtherCharges2dcl = model.OtherCharges2dcl,
                        OtherCharges3dcl = model.OtherCharges3dcl,
                        OtherCharges4dcl = model.OtherCharges4dcl,
                        PaymentTermsvtxt = model.PaymentTermsvtxt,
                        DeliveryTermsvtxt = model.DeliveryTermsvtxt,
                        Statusvtxt = model.Statusvtxt,
                        CreatedByvtxt = model.CreatedByvtxt,
                        CreatedDatedatetime = DateTime.Now
                    };
                    //_Orderservice.Create(OrderHeader);
                    return Ok(_Orderservice.Create(OrderHeader));
                    //return Ok(OrderHeader);
                //}
                //else
                //{
                //    return Ok("Un Authorized User");
                //}
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPut("UpdateOrderHeader")]
        public ActionResult UpdateOrderHeader(OrderHeaderModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var OrderHeader = new OrderHeaderModel()
                    {
                        IDbint = model.IDbint,
                        OrderNovtxt = model.OrderNovtxt,
                        OrderDatedate = model.OrderDatedate,
                        RefNovtxt = model.RefNovtxt,
                        RefDatedate = model.RefDatedate,
                        SAPOrderNovtxt = model.SAPOrderNovtxt,
                        SAPOrderDatedate = model.SAPOrderDatedate,
                        CustomerCodevtxt = model.CustomerCodevtxt,
                        CustomerNamevtxt = model.CustomerNamevtxt,
                        Divisionvtxt = model.Divisionvtxt,
                        ShipToCodevtxt = model.ShipToCodevtxt,
                        ShipToNamevtxt = model.ShipToNamevtxt,
                        ShipToAddressvtxt = model.ShipToAddressvtxt,
                        DeliveryAddressvtxt = model.DeliveryAddressvtxt,
                        TotalNetValuedcl = model.TotalNetValuedcl,
                        TotalOrderQuantityint = model.TotalOrderQuantityint,
                        TotalOrderQuantityKgsint = model.TotalOrderQuantityKgsint,
                        TotalOrderQuantityMTint = model.TotalOrderQuantityMTint,
                        SAPStatusvtxt = model.SAPStatusvtxt,
                        OtherCharges1dcl = model.OtherCharges1dcl,
                        OtherCharges2dcl = model.OtherCharges2dcl,
                        OtherCharges3dcl = model.OtherCharges3dcl,
                        OtherCharges4dcl = model.OtherCharges4dcl,
                        PaymentTermsvtxt = model.PaymentTermsvtxt,
                        DeliveryTermsvtxt = model.DeliveryTermsvtxt,
                        Statusvtxt = model.Statusvtxt,
                        CreatedByvtxt = model.CreatedByvtxt,
                        CreatedDatedatetime = DateTime.Now
                    };
                    return Ok(_Orderservice.update(OrderHeader));
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

        [HttpPut("UpdateOrderHeaderStatus")]
        public ActionResult UpdateOrderHeaderStatus(OrderHeaderModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), model.CreatedByvtxt))
                {
                    var OrderHeader = new OrderHeaderModel()
                    {
                        IDbint = model.IDbint,
                        Statusvtxt = model.Statusvtxt,
                        CreatedByvtxt = model.CreatedByvtxt,
                        CreatedDatedatetime = DateTime.Now
                    };
                    return Ok(_Orderservice.updateStatus(OrderHeader));
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

        [HttpDelete("DeleteOrderDetails/{OrderID}")]
        public ActionResult DeleteOrderDetails(long OrderID)
        {
            try
            {
                _Orderservice.Delete(OrderID);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("InsertOrderDetails")]
        public ActionResult InsertOrderDetails(OrderDetailsModel model)
        {
            try
            {
                var Orderdetails = new OrderDetailsModel()
                {
                    OrderID = model.OrderID,
                    MaterialCodevtxt = model.MaterialCodevtxt,
                    MaterialDescriptionvtxt = model.MaterialDescriptionvtxt,
                    UoMvtxt = model.UoMvtxt,
                    Quantityint = model.Quantityint,
                    Ratedcl = model.Ratedcl,
                    Amountdcl = model.Amountdcl
                };
                _Orderservice.InsertOrderDetails(Orderdetails);
                return Ok(Orderdetails);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetOrdersByCustomerCode/{fromdate},{todate},{status},{customercode},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetOrdersByCustomerCode(string fromdate, string todate, string status, string customercode, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return Ok(_Orderservice.GetOrderList(fromdate, todate, status, customercode, PageNo, PageSize, KeyWord));
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

        [HttpGet("GetOrdersByCustomerCodeCount/{fromdate},{todate},{status},{customercode},{KeyWord}")]
        public long GetOrdersByCustomerCodeCount(string fromdate, string todate, string status, string customercode, string KeyWord)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return _Orderservice.GetOrderListCount(fromdate, todate, status, customercode, KeyWord);
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

        [HttpGet("GetAllOrdersCount/{fromdate},{todate},{status},{KeyWord}")]
        public long GetAllOrdersCount(string fromdate, string todate, string status, string KeyWord)
        {
            try
            {
                return _Orderservice.GetAllOrderListCount(fromdate, todate, status, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetAllOrderLists/{fromdate},{todate},{status},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetAllOrderLists(string fromdate, string todate, string status, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_Orderservice.GetAllOrderList(fromdate, todate, status, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetOrderHeaderByOrderID/{OrderID}")]
        public IActionResult GetOrderHeaderByOrderID(long OrderID)
        {
            try
            {
                return Ok(_Orderservice.GetOrderHeaderByOrderID(OrderID));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetOrderDetailsByOrderID/{OrderID}")]
        public IActionResult GetOrderDetailsByOrderID(long OrderID)
        {
            try
            {
                return Ok(_Orderservice.GetOrderDetailsByOrderID(OrderID));
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
                    List<OrderHeaderModel> orderslist = _Orderservice.DownloadOrderList(fromdate, todate, status, SoldToPartyCodevtxt, KeyWord);
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

        [HttpGet("GetUOM")]
        public IActionResult GetUOM()
        {
            try
            {
                return Ok(_Orderservice.GetUOM());
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetUOMByID/{ID}")]
        public IActionResult GetUOMByID(int ID)
        {
            try
            {
                return Ok(_Orderservice.GetUOMByID(ID));
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

        //use for get mapped plant list for SP
        [HttpGet("GetMappedPlantList/{usercode}")]
        public IActionResult GetMappedPlantList(string usercode)
        {
            try
            {
                return Ok(_Orderservice.GetMappedPlantList(usercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for update order status for SAP order no
        [HttpPut("UpdateOrderStatus")]
        public long UpdateOrderStatus(OrderHeaderModel model)
        {
            try
            {
                return _Orderservice.updateOrderStatus(model);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for get SAP Order Creation Response
        [HttpGet("GetSAPOrderCreationResponse/{orderid}")]
        public IActionResult GetSAPOrderCreationResponse(string orderid)
        {
            try
            {
                return Ok(_Orderservice.GetSAPOrderResponse(orderid));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("GetOrderReportListCount/{fromdate},{todate},{Region},{Branch},{Territory},{status},{KeyWord}")]
        public long GetOrderReportListCount(string fromdate, string todate, string Region, string Branch, string Territory, string status, string KeyWord)
        {
            try
            {
                return _Orderservice.GetOrderReportListCount(fromdate, todate, Region, Branch, Territory, status, KeyWord);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [HttpGet("GetOrderReportList/{fromdate},{todate},{Region},{Branch},{Territory},{status},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetOrderReportList(string fromdate, string todate, string Region, string Branch, string Territory, string status, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_Orderservice.GetOrderReportList(fromdate, todate, Region, Branch, Territory, status, PageNo, PageSize, KeyWord));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //Use for Export to Excel
        [HttpGet("GetAllOrdersReportDownload/{fromdate},{todate},{Region},{Branch},{Territory},{status},{KeyWord}")]
        public IActionResult GetAllOrdersReportDownload(string fromdate, string todate, string Region, string Branch, string Territory, string status, string KeyWord)
        {
            try
            {
                List<OrderReportModel> reportslist = _Orderservice.GetOrderReportListDownload(fromdate, todate, Region, Branch, Territory, status, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Order");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "Region Code";
                    worksheet.Cell(currentRow, 3).Value = "Region Name";
                    worksheet.Cell(currentRow, 4).Value = "Branch Code";
                    worksheet.Cell(currentRow, 5).Value = "Branch Name";
                    worksheet.Cell(currentRow, 6).Value = "Territory Code";
                    worksheet.Cell(currentRow, 7).Value = "Territory Name";
                    worksheet.Cell(currentRow, 8).Value = "Customer Code";
                    worksheet.Cell(currentRow, 9).Value = "Customer Name";
                    worksheet.Cell(currentRow, 10).Value = "Web Order No";
                    worksheet.Cell(currentRow, 11).Value = "Web Order Date";
                    worksheet.Cell(currentRow, 12).Value = "Order QTY";
                    worksheet.Cell(currentRow, 13).Value = "Status";
                    worksheet.Cell(currentRow, 14).Value = "SAP Order No";
                    worksheet.Cell(currentRow, 15).Value = "SAP Order Date";
                    worksheet.Cell(currentRow, 16).Value = "User Code";
                    worksheet.Cell(currentRow, 17).Value = "User Name";
                    worksheet.Cell(currentRow, 18).Value = "User Type";
                    foreach (var reports in reportslist)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = reports.RegionCodevtxt;
                        worksheet.Cell(currentRow, 3).Value = reports.RegionDescriptionvtxt;
                        worksheet.Cell(currentRow, 4).Value = reports.BranchCodevtxt;
                        worksheet.Cell(currentRow, 5).Value = reports.BranchNamevtxt;
                        worksheet.Cell(currentRow, 6).Value = reports.SalesOfficeCodevtxt;
                        worksheet.Cell(currentRow, 7).Value = reports.SalesOfficeNamevtxt;
                        worksheet.Cell(currentRow, 8).Value = reports.CustCodevtxt;
                        worksheet.Cell(currentRow, 9).Value = reports.CustNamevtxt;
                        worksheet.Cell(currentRow, 10).Value = reports.OrderNovtxt;
                        worksheet.Cell(currentRow, 11).Value = reports.OrderDatedate;
                        worksheet.Cell(currentRow, 12).Value = reports.TotalOrderQuantityint;
                        worksheet.Cell(currentRow, 13).Value = reports.Statusvtxt;
                        worksheet.Cell(currentRow, 14).Value = reports.SAPOrderNovtxt;
                        worksheet.Cell(currentRow, 15).Value = reports.SAPOrderDatedate;
                        worksheet.Cell(currentRow, 16).Value = reports.UserCodetxt;
                        worksheet.Cell(currentRow, 17).Value = reports.UserNametxt;
                        worksheet.Cell(currentRow, 18).Value = reports.UserTypetxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "ReportList.xlsx");
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