using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class RFCCallController : ControllerBase
    {
        private readonly IRFCCallServices _IRFCCallServices;
        private readonly IOrderService _IOrderService;
        private readonly ILogger _ILogger;
        private readonly IChecktokenservice _Checktokenservice;

        public RFCCallController(IRFCCallServices RFCCallServices, IOrderService OrderService, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _IRFCCallServices = RFCCallServices;
            _IOrderService = OrderService;
            _Checktokenservice = checktokenservice;
        }

        //use for get customer Outstanding

        [HttpGet("GetOutstanding/{customercode}")]
        public long GetOutstanding(string customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return _IRFCCallServices.GetOutStandingFromRFC(customercode);
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

        [HttpGet("invoicePDFDownloadAsync/{invoiceNO},{customercode}")]
        public Task<string> invoicePDFDownloadAsync(string invoiceNO, string customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return _IRFCCallServices.invoicePDFDownloadAsync(invoiceNO);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return null;
            }
        }
        [AllowAnonymous]
        [HttpPost("KAMFirstRequestPriceApproval")]
        public Task<KAMPriceApprovalSAPResponseModel> KAMFirstRequestPriceApproval(KAMPriceApprovalSAPRequestModel model)
        {
            try
            { 
                return _IRFCCallServices.KAMFirstRequestPriceApproval(model);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return KAMFirst(ex.Message);
            }
        }
        private async Task<KAMPriceApprovalSAPResponseModel> KAMFirst(string Text)
        {
            await Task.Delay(10);
            KAMPriceApprovalSAPResponseModel model = new KAMPriceApprovalSAPResponseModel();
            model.STATUS = Text;
            return model;
        }
        private async Task<KAMPriceApprovalSencondSAPResponseModel> KAMSecond(string Text)
        {
            await Task.Delay(10);
            KAMPriceApprovalSencondSAPResponseModel model = new KAMPriceApprovalSencondSAPResponseModel();
            model.STATUS = Text;
            return model;
        }

        [AllowAnonymous]
        [HttpPost("KAMSecondRequestPriceApproval")]
        public Task<KAMPriceApprovalSencondSAPResponseModel> KAMSecondRequestPriceApproval(KAMPriceApprovalSencondSAPRequestModel model)
        {
            try
            {
                return _IRFCCallServices.KAMSecondRequestPriceApproval(model);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return KAMSecond(ex.Message);
            }
        }

        [HttpGet("GetCreditLimitFromRFC/{customercode}")]
        public long GetCreditLimitFromRFC(string customercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return _IRFCCallServices.GetCreditLimitFromRFC(customercode);
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

        //use for get customer wise Outstanding data for Employee
        [HttpGet("GetOutstandingForEmployee/{usertype},{usercode}")]
        public long GetOutstandingForEmployee(string usertype, string usercode)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckToken(authorize[1].Trim(), usercode))
                {
                    return _IRFCCallServices.GetOutStandingFromRFCEmployeeWise(usercode, usertype);
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

        //use for current ledger for customer
        [HttpGet("GetLedger/{customercode},{fromdate},{todate}")]
        public long GetLedger(string customercode, string fromdate, string todate)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), customercode))
                {
                    return _IRFCCallServices.GetLedgerFromRFC(customercode, fromdate, todate);
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

        //use for post web order to RFC
        [HttpPost("PostWebOrdersToRFC")]
        public IActionResult PostWebOrdersToRFC(I_SO_DATA model)
        {
            try
            {
                SAPOrderOutputModel lst = _IRFCCallServices.InsertOrderIntoRFC(model);
                long deleteid = _IOrderService.DeleteSApOrderResponse(model.WEB_ORD);
                SAPET_RETURN[] spret = lst.ET_RETURNs;
                if (spret.Length > 0)
                {
                    for (int i = 0; i < spret.Length; i++)
                    {
                        SAPCreateOrderOutputModel createmodel = new SAPCreateOrderOutputModel();
                        createmodel.OrderNovtxt = model.WEB_ORD;
                        createmodel.OrderIDbint = 0;
                        createmodel.SAPOrderNovtxt = lst.E_ORD_NUMs;
                        createmodel.ErrorIDvtxt = spret[i].ID;
                        createmodel.Numvtxt = spret[i].NUMBER;
                        createmodel.Messagevtxt = spret[i].MESSAGE;
                        _IOrderService.InsertSApOrderResponse(createmodel);
                    }
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for SAP Plant Available stock
        [HttpPost("GetSAPPlantWiseStock")]
        public IActionResult GetSAPPlantWiseStock(I_STR_STK_OV_SRCH model)
        {
            try
            {
                return Ok(_IRFCCallServices.GetAvailableStock(model));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetLedgerforSPCFA/{usercode},{username},{usertype},{fromdate},{todate}")]
        public long GetLedgerforSPCFA(string usercode, string username, string usertype, string fromdate, string todate)
        {
            try
            {
                //string Token = Request.Headers["Authorization"];
                //string[] authorize = Token.Split(" ");
                //if (_Checktokenservice.CheckTokenForCustomer(authorize[1].Trim(), usercode))
                //{
                return _IRFCCallServices.GetLedgerFromRFCforSPCFA(usercode, username, usertype, fromdate, todate);
                //}
                //else
                //{
                //    return 0;
                //}
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }
          
        [HttpPost("InsertCDPayment")]
        public IActionResult InsertCDPayment(List<CDPaymentModel> models)
        {
            try
            {
                string[] result = new string[models.Count];
                int loopcount=0;
                for(int count =0; count < models.Count; count++)
                {

                    if (count == 0)
                    {
                        result[count] = _IRFCCallServices.InsertCDPayment(models[count]);

                    }
                    else
                    {
                        result[count] = _IRFCCallServices.InsertCDPayment(models[count]);
                    }
                    loopcount =1+ count;
                }


                if(loopcount == models.Count)
                {
                    return Ok("Data Inserted successfully");
                }
                else
                {
                    string response = "";

                    for (int count =0; count < loopcount; count++)
                    {
                        if(count == 0)
                        {
                            response =   result[count];
                        }
                        else
                        {
                            response = response + ", " + result[count];
                        }
                    }
                    return Ok(response + ", Data only Inserted");
                } 
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest(ex);
            }
        }
    }
}