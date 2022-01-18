using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using SapNwRfc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Services
{
    public class RFCCallServices : IRFCCallServices
    {
        private readonly ICustomerPortalHelper _customerPortalHelper;
        private readonly IConfiguration _config;
        private readonly ILogger _ILogger;

        PriceApprovalService approvalService;
        public RFCCallServices(ILogger ILoggerservice, ICustomerPortalHelper customerPortalHelper, IConfiguration config)
        {
            _customerPortalHelper = customerPortalHelper;
            _config = config;
            _ILogger = ILoggerservice;
            approvalService = new PriceApprovalService(customerPortalHelper);
        }

        public long GetOutStandingFromRFC(string customercode)
        {
            try
            {
                using var connection = new SapConnection(_config.GetConnectionString("SAPConnectionString"));
                connection.Connect();
                if (connection.Ping())
                {
                    using ISapFunction someFunction = connection.CreateFunction("ZSDDP_FM_CUSTOMER_AGEING");

                    Z_KUNNR model = new Z_KUNNR();
                    model.KUNNR_FROM = customercode;
                    model.KUNNR_TO = customercode;
                    RFCOutstandingRequest reqstmodel = new RFCOutstandingRequest();
                    reqstmodel.z = model;
                    var result = someFunction.Invoke<SAPOutstandingModel>(reqstmodel).ZCUSTAGE.ToArray();

                    if (result != null)
                    {
                        long rt = InsertOutstanding(result[0]);
                        connection.Dispose();
                        return rt;
                    }
                    else
                    {
                        connection.Dispose();
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                string result = "";
                for (int i = 0; i < 13; i++)
                {
                    result += ex.Message[i];
                }
                if (result == "SAP RFC Error")
                {
                    return 0;
                }
                return 0;
            }
        }

        public long GetCreditLimitFromRFC(string customercode)
        {
            try
            {
                using var connection = new SapConnection(_config.GetConnectionString("SAPConnectionString"));
                connection.Connect();
                if (connection.Ping())
                {
                    using ISapFunction someFunction = connection.CreateFunction("ZSDDP_FM_CREDIT_BAL");

                    Z_KUNNR model = new Z_KUNNR();
                    model.KUNNR_FROM = customercode;
                    model.KUNNR_TO = customercode;
                    RFCOutstandingRequest reqstmodel = new RFCOutstandingRequest();
                    reqstmodel.z = model;
                    var result = someFunction.Invoke<SAPCreditLimitModel>(reqstmodel).ZCRBAL.ToArray();

                    if (result != null)
                    {
                        long rt = InsertCrediLimit(result[0]);
                        connection.Dispose();
                        return rt;
                    }
                    else
                    {
                        connection.Dispose();
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                string result = "";
                for (int i = 0; i < 13; i++)
                {
                    result += ex.Message[i];
                }
                if (result == "SAP RFC Error")
                {
                    return 0;
                }
                return 0;
            }
        }

        public long InsertCrediLimit(ZCRBAL model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CompanyCodevtxt", model.BUKRS, DbType.String);
            dbPara.Add("CustomerCodevtxt", model.KUNNR, DbType.String);
            dbPara.Add("CustomerNamevtxt", model.NAME1, DbType.String);
            dbPara.Add("CreditControlCodevtxt", model.Z_VKBUR, DbType.String);
            dbPara.Add("CreditControlDescvtxt", model.Z_BEZEI, DbType.String);
            dbPara.Add("CreditLimitdcl", model.KLIMK, DbType.Decimal);
            dbPara.Add("OutStandingdcl", model.CRBAL, DbType.Decimal);
            decimal availablecreditlimit = Convert.ToDecimal(model.KLIMK) - Convert.ToDecimal(model.CRBAL);
            dbPara.Add("AvailableCreditLimitdcl", availablecreditlimit, DbType.Decimal);
            dbPara.Add("SystemDateTimedatetime", model.SYDATE, DbType.DateTime);
            dbPara.Add("SystemTimedatetime", model.SYTIME, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerCreditLimit]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long GetLedgerFromRFC(string customercode, string fromdate, string todate)
        {
            //string connectionString = "AppServerHost = 172.20.1.20; SystemNumber = 00; User = SAPSUPPORT; Password = Crystal@20#; Client = 700; Language = EN; PoolSize = 5; Trace = 3";
            try
            {
                using var connection = new SapConnection(_config.GetConnectionString("SAPConnectionString"));
                connection.Connect();

                if (connection.Ping())
                {
                    using ISapFunction someFunction = connection.CreateFunction("ZSDDP_CUSTOMER_LEDGER");

                    DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
                    DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
                    DateTime startdate = Convert.ToDateTime(fdate);
                    DateTime enddate = Convert.ToDateTime(tdate);
                    var resultData = someFunction.Invoke<SAPLedgerClass>(new SAPLedgerRequest { KUNNRs = customercode, FDATs = startdate, TDATs = enddate });

                    var result = resultData.LEDGER_DETAIL.ToArray();
                    var result1 = resultData.LEDGER_HEADER.ToArray();

                    if (result != null && result1 != null)
                    {
                        connection.Disconnect();
                        long rt = 0;
                        DeleteLedger(customercode);
                        InsertOpeningBalnce(Convert.ToDecimal(result1[0].DMBTR), customercode);
                        for (int I = 0; I < result.Length; I++)
                        {
                            rt = InsertLedger(result[I], customercode);
                            rt = 2;
                        }
                        return rt;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                string result = "";

                string wronginput = "SAP RFC Error: RFC_ABAP_EXCEPTION with message";
                string connectionFailed = "SAP RFC Error";
                for (int i = 0; i < wronginput.Length; i++)
                {
                    result += ex.Message[i];
                }
                if (result == wronginput)
                {
                    return 4;
                }
                result = "";
                for (int i = 0; i < connectionFailed.Length; i++)
                {
                    result += ex.Message[i];
                }
                if (result == connectionFailed)
                {
                    return 1;
                }
                return 3;
            }
        }

        public long InsertOutstanding(ZCUSTAGES model)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CompanyCodevtxt", model.BUKRS, DbType.String);
            dbPara.Add("CompanyNamevtxt", model.BUTXT, DbType.String);
            dbPara.Add("CustomerCodevtxt", model.KUNNR, DbType.String);
            dbPara.Add("CustomerNamevtxt", model.KUNNR_NM, DbType.String);
            dbPara.Add("SalesOfficevtxt", model.VKBUR, DbType.String);
            dbPara.Add("SalesOfficeCdvtxt", model.VKBUR_NM, DbType.String);
            dbPara.Add("ControllingAreavtxt", model.KKBER, DbType.String);
            dbPara.Add("ControllingAreaDescriptionvtxt", model.KKBER_NM, DbType.String);
            dbPara.Add("D3dcl", model.D3, DbType.Decimal);
            dbPara.Add("D7dcl", model.D7, DbType.Decimal);
            dbPara.Add("D15dcl", model.D15, DbType.Decimal);
            dbPara.Add("D30dcl", model.D30, DbType.Decimal);
            dbPara.Add("D45dcl", model.D45, DbType.Decimal);
            dbPara.Add("D60dcl", model.D60, DbType.Decimal);
            dbPara.Add("D90dcl", model.D90, DbType.Decimal);
            dbPara.Add("D120dcl", model.D120, DbType.Decimal);
            dbPara.Add("D150dcl", model.D150, DbType.Decimal);
            dbPara.Add("D180dcl", model.D180, DbType.Decimal);
            dbPara.Add("D360dcl", model.D360, DbType.Decimal);
            dbPara.Add("gD360dcl", model.D360_MORE, DbType.Decimal);
            dbPara.Add("GrossOutsatndingAmtdcl", model.GRAMT_BAL, DbType.Decimal);
            dbPara.Add("NetOutstandingAmtdcl", model.NETAMT_BAL, DbType.Decimal);
            dbPara.Add("SystemDateTimedatetime", model.SYDATE, DbType.DateTime);
            dbPara.Add("SystemTimedatetime", model.SYTIME, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerOutstanding]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertOpeningBalnce(decimal amt, string customercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", customercode, DbType.String);
            dbPara.Add("DocumentNovtxt", "", DbType.String);
            dbPara.Add("Plantvtxt", "", DbType.String);
            dbPara.Add("DocumentDatedate", "", DbType.String);
            dbPara.Add("DocumentTypevtxt", "Opening Balance", DbType.String);
            dbPara.Add("Materialvtxt", "", DbType.String);
            dbPara.Add("Quantitydcl", 0, DbType.Decimal);
            dbPara.Add("ChequeDatedate", "", DbType.String);
            dbPara.Add("ChequeNovtxt", "", DbType.String);
            if (amt > 0)
            {
                dbPara.Add("Debitdcl", 0, DbType.Decimal);
                dbPara.Add("Creditdcl", amt, DbType.Decimal);
                dbPara.Add("Balancedcl", 0, DbType.Decimal);
            }
            else
            {
                dbPara.Add("Debitdcl", amt, DbType.Decimal);
                dbPara.Add("Creditdcl", 0, DbType.Decimal);
                dbPara.Add("Balancedcl", 0, DbType.Decimal);
            }

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerLedger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertLedger(LEDGER_DETAIL model, string customercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", customercode, DbType.String);
            dbPara.Add("DocumentNovtxt", model.BELNR, DbType.String);
            dbPara.Add("Plantvtxt", model.WERKS, DbType.String);
            dbPara.Add("DocumentDatedate", Convert.ToDateTime(model.BLDAT).ToString("MM-dd-yyyy"), DbType.String);
            dbPara.Add("DocumentTypevtxt", model.BLART, DbType.String);
            dbPara.Add("Materialvtxt", model.MATWA, DbType.String);
            dbPara.Add("Quantitydcl", model.FKIMG, DbType.Decimal);
            dbPara.Add("ChequeDatedate", Convert.ToDateTime(model.BUDAT).ToString("MM-dd-yyyy"), DbType.String);
            dbPara.Add("ChequeNovtxt", model.BELNR, DbType.String);
            dbPara.Add("Debitdcl", model.DMBTRD, DbType.Decimal);
            dbPara.Add("Creditdcl", model.DMBTRC, DbType.Decimal);
            dbPara.Add("Balancedcl", model.BAL, DbType.Decimal);
            dbPara.Add("Narrationvtxt", model.SGTXT, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerLedger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long DeleteLedger(string customercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", customercode, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[usp_deletecustomerledger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long GetLedgerFromRFCforSPCFA(string usercode, string username, string usertype, string fromdate, string todate)
        {
            try
            {
                //string connectionString = "AppServerHost = 172.20.1.20; SystemNumber = 00; User = SAPSUPPORT; Password = Crystal@20#; Client = 700; Language = EN; PoolSize = 5; Trace = 3";
                using var connection = new SapConnection(_config.GetConnectionString("SAPConnectionString"));
                connection.Connect();
                if (connection.Ping())
                {
                    DateTime fdate = DateTime.ParseExact(fromdate, "dd-MM-yyyy", null);
                    DateTime tdate = DateTime.ParseExact(todate, "dd-MM-yyyy", null);
                    DateTime startdate = Convert.ToDateTime(fdate);
                    DateTime enddate = Convert.ToDateTime(tdate);
                    if (usertype == "CF Agent")
                    {
                        using ISapFunction someFunction = connection.CreateFunction("ZSDDP_CFA_LEDGER");
                        var resultData = someFunction.Invoke<SAPLedgerClassForSPCFA>(new SAPLedgerRequestforSPCFA { LIFNRs = usercode, FDATs = startdate, TDATs = enddate });
                        var result = resultData.LEDGER_DETAIL.ToArray();
                        var result1 = resultData.LEDGER_HEADER.ToArray();
                        if (result != null && result1 != null)
                        {
                            connection.Disconnect();
                            long rt = 0;
                            DeleteLedgerSPCFA(usercode);
                            InsertOpeningBalnceSPCFA(Convert.ToDecimal(result1[0].OPBAL), usercode, username);
                            for (int I = 0; I < result.Length; I++)
                            {
                                rt = InsertLedgerSPCFA(result[I], usercode, username);
                                rt = 2;
                            }
                            return rt;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        using ISapFunction someFunction = connection.CreateFunction("ZSDDP_SP_LEDGER");
                        var resultData = someFunction.Invoke<SAPLedgerClassForSPCFA>(new SAPLedgerRequestforSPCFA { LIFNRs = usercode, FDATs = startdate, TDATs = enddate });
                        var result = resultData.LEDGER_DETAIL.ToArray();
                        var result1 = resultData.LEDGER_HEADER.ToArray();
                        if (result != null && result1 != null)
                        {
                            connection.Disconnect();
                            long rt = 0;
                            DeleteLedgerSPCFA(usercode);
                            InsertOpeningBalnceSPCFA(Convert.ToDecimal(result1[0].OPBAL), usercode, username);
                            for (int I = 0; I < result.Length; I++)
                            {
                                rt = InsertLedgerSPCFA(result[I], usercode, username);
                                rt = 2;
                            }
                            return rt;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                string result = "";

                string wronginput = "SAP RFC Error: RFC_ABAP_EXCEPTION with message";
                string connectionFailed = "SAP RFC Error";
                for (int i = 0; i < wronginput.Length; i++)
                {
                    result += ex.Message[i];
                }
                if (result == wronginput)
                {
                    return 4;
                }
                result = "";
                for (int i = 0; i < connectionFailed.Length; i++)
                {
                    result += ex.Message[i];
                }
                if (result == connectionFailed)
                {
                    return 1;
                }
                return 3;
            }
        }

        public long InsertOpeningBalnceSPCFA(decimal amt, string usercode, string username)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCodevtxt", usercode, DbType.String);
            dbPara.Add("UserNamevtxt", username, DbType.String);
            dbPara.Add("PostingDatedate", "", DbType.String);
            dbPara.Add("DocumentNovtxt", "", DbType.String);
            dbPara.Add("DocumentDatedate", "", DbType.String);
            dbPara.Add("RefDocumentNovtxt", "", DbType.String);
            dbPara.Add("Quantitydcl", 0, DbType.Decimal);
            dbPara.Add("DocumentTypevtxt", "Opening Balance", DbType.String);
            dbPara.Add("TDSdcl", 0, DbType.Decimal);
            dbPara.Add("ItemDescvtxt","", DbType.String);
            dbPara.Add("Balancedcl", amt, DbType.Decimal);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertSPCFALedger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long InsertLedgerSPCFA(SPLEDGER_DETAIL model, string usercode, string username)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("UserCodevtxt", usercode, DbType.String);
            dbPara.Add("UserNamevtxt", username, DbType.String);
            dbPara.Add("PostingDatedate", Convert.ToDateTime(model.BUDAT).ToString("MM-dd-yyyy"), DbType.String);
            dbPara.Add("DocumentNovtxt", model.BELNR, DbType.String);
            dbPara.Add("DocumentDatedate", Convert.ToDateTime(model.BLDAT).ToString("MM-dd-yyyy"), DbType.String);
            dbPara.Add("RefDocumentNovtxt", model.XBLNR, DbType.String);
            dbPara.Add("Quantitydcl", model.FKIMG, DbType.Decimal);
            dbPara.Add("DocumentTypevtxt", model.CD, DbType.String);
            dbPara.Add("TDSdcl", model.WT_QBSHH, DbType.Decimal);
            dbPara.Add("ItemDescvtxt", model.SGTXT, DbType.String);
            dbPara.Add("Balancedcl", model.BAL, DbType.Decimal);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertSPCFALedger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long DeleteLedgerSPCFA(string usercode)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("userCodevtxt", usercode, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[usp_deleteSPCFAledger]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

        public long GetOutStandingFromRFCEmployeeWise(string usercode, string usertype)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("usercode", usercode, DbType.String);
            dbPara.Add("usertype", usertype, DbType.String);

            #region using dapper

            var data = _customerPortalHelper.GetAll<CustomerMasterModel>("[dbo].[uspviewCustomerlistByUserType]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);

            if (data.ToList().Count > 0)
            {
                for (int i = 0; i < data.ToList().Count; i++)
                {
                    long rst = GetOutStandingFromRFC(data[i].CustCodevtxt);
                }
            }
            return 1;

            #endregion using dapper
        }

        public SAPOrderOutputModel InsertOrderIntoRFC(I_SO_DATA requstmodel)
        {
            // string connectionString = "AppServerHost = 172.20.1.20; SystemNumber = 00; User = SAPSUPPORT; Password = Crystal@20#; Client = 700; Language = EN; PoolSize = 5; Trace = 3";
            using var connection = new SapConnection(_config.GetConnectionString("SAPConnectionString"));
            connection.Connect();
            if (connection.Ping())
            {
                using ISapFunction someFunction = connection.CreateFunction("ZSDDP_SALES_ORD_CRT");
                List<I_SO_DATA> lstmodel = new List<I_SO_DATA>();
                I_SO_DATA model = new I_SO_DATA();
                model.ORD_TYPE = requstmodel.ORD_TYPE;
                model.SALES_ORG = requstmodel.SALES_ORG;
                model.DIST_CHNL = requstmodel.DIST_CHNL;
                model.DIVISION = requstmodel.DIVISION;
                model.SOLD_TO = requstmodel.SOLD_TO;
                model.SHIP_TO = requstmodel.SHIP_TO;
                model.PO_DATE = requstmodel.PO_DATE;
                model.PO_NUM = requstmodel.PO_NUM;
                model.DOC_DATE = Convert.ToDateTime(requstmodel.DOC_DATE);
                model.INCO_TERMS1 = requstmodel.INCO_TERMS1;
                model.INCO_TERMS2 = requstmodel.INCO_TERMS2;
                model.PRICE_LIST = requstmodel.PRICE_LIST;
                model.TRANS_GROUP = requstmodel.TRANS_GROUP;
                model.MAT_NUM = requstmodel.MAT_NUM;
                model.QTY = requstmodel.QTY;
                model.DEVLY_PLANT = requstmodel.DEVLY_PLANT;
                model.WEB_ORD = requstmodel.WEB_ORD;
                model.SP_CODE = requstmodel.SP_CODE;
                lstmodel.Add(model);
                InsertOrderIntoRFCRequestModel rfcmodelinsert = new InsertOrderIntoRFCRequestModel();
                rfcmodelinsert.I_SO_DATAS = lstmodel[0];

                var result = someFunction.Invoke<SAPOrderOutputModel>(rfcmodelinsert);

                // var result1 = someFunction.Invoke<SAPOrderOutputModel>(rfcmodelinsert).ET_RETURNs.ToArray();
                SAPOrderOutputModel outmodel = new SAPOrderOutputModel();
                outmodel.E_ORD_NUMs = result.E_ORD_NUMs.ToString();
                outmodel.ET_RETURNs = result.ET_RETURNs.ToArray();
                connection.Disconnect();
                return outmodel;
            }
            else
            {
                SAPOrderOutputModel sAPOrder = new SAPOrderOutputModel();

                return sAPOrder;
            }
        }

        public async Task<string> invoicePDFDownloadAsync(string InvoiceNo)
        {
            var client = new HttpClient();

            client.BaseAddress = (new Uri(_config["InvoiceSAP:url"]));

            InvoiceDownloadModal model = new InvoiceDownloadModal();
            model.invoice = InvoiceNo;
            var plainBasicAuthBytes = System.Text.Encoding.UTF8.GetBytes(_config["InvoiceSAP:userName"] + ":" + _config["InvoiceSAP:password"]);
            var base64BasicAuth = System.Convert.ToBase64String(plainBasicAuthBytes);

            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64BasicAuth);
            HttpResponseMessage responsePost = await client.PostAsJsonAsync(client.BaseAddress.ToString(), model);
            if (responsePost.IsSuccessStatusCode)
            {
                return responsePost.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
            }
            else
            {
                return null;
            }
        }

        public async Task<KAMPriceApprovalSAPResponseModel> KAMFirstRequestPriceApproval(KAMPriceApprovalSAPRequestModel model)
        {
            var client = new HttpClient();

            client.BaseAddress = (new Uri(_config["InvoiceSAP:kamurl"]));
            var plainBasicAuthBytes = System.Text.Encoding.UTF8.GetBytes(_config["InvoiceSAP:kamuserName"] + ":" + _config["InvoiceSAP:kampassword"]);
            var base64BasicAuth = System.Convert.ToBase64String(plainBasicAuthBytes);

            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64BasicAuth);

            HttpResponseMessage responsePost = await client.PostAsJsonAsync(client.BaseAddress.ToString(), model);
            if (responsePost.IsSuccessStatusCode)
            {
                string str= responsePost.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                str = str.Replace("[","");
                str = str.Replace("]", "");
                KAMPriceApprovalSAPResponseModel response = JsonSerializer.Deserialize<KAMPriceApprovalSAPResponseModel>(str);
                return response;
            }
            else
            {
                return null;
            }
        }

        public async Task<KAMPriceApprovalSencondSAPResponseModel> KAMSecondRequestPriceApproval(KAMPriceApprovalSencondSAPRequestModel model)
        {
            var client = new HttpClient();

            client.BaseAddress = (new Uri(_config["InvoiceSAP:kamurl2"]));
            var plainBasicAuthBytes = System.Text.Encoding.UTF8.GetBytes(_config["InvoiceSAP:kamuserName"] + ":" + _config["InvoiceSAP:kampassword"]);
            var base64BasicAuth = System.Convert.ToBase64String(plainBasicAuthBytes);

            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64BasicAuth);

            HttpResponseMessage responsePost = await client.PostAsJsonAsync(client.BaseAddress.ToString(), model);
            if (responsePost.IsSuccessStatusCode)
            {
                string str = responsePost.Content.ReadAsStringAsync().ContinueWith(task => task.Result).Result;
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                KAMPriceApprovalSencondSAPResponseModel response = JsonSerializer.Deserialize<KAMPriceApprovalSencondSAPResponseModel>(str);
                PriceApprovalModel model1 = new PriceApprovalModel();
                model1.IDBint = model.IDBint;
                model1.ApprovalNovtxt = response.APPNO;
                model1.CustomerCodevtxt = response.APPNO;
                model1.Mode = "R";
                this.approvalService.InsertPriceApproval(model1);
                return response;
            }
            else
            {
                return null;
            }
        }

        public List<ET_STK_OVERVIEW> GetAvailableStock(I_STR_STK_OV_SRCH requstmodel)
        {
            using var connection = new SapConnection(_config.GetConnectionString("SAPConnectionString"));
            connection.Connect();
            if (connection.Ping())
            {
                using ISapFunction someFunction = connection.CreateFunction("/ARTEC/PD_BAPI_STOCK_OVERVIEW");
                List<I_STR_STK_OV_SRCH> lstmodel = new List<I_STR_STK_OV_SRCH>();
                I_STR_STK_OV_SRCH model = new I_STR_STK_OV_SRCH();
                model.MATERIAL = requstmodel.MATERIAL;
                model.PLANT_FROM = requstmodel.PLANT_FROM;
                model.PLANT_TO = requstmodel.PLANT_TO;
                model.BATCH_FROM = requstmodel.BATCH_FROM;
                model.BATCH_TO = requstmodel.BATCH_TO;
                model.STOR_LOCATION_FROM = requstmodel.STOR_LOCATION_FROM;
                model.STOR_LOCATION_TO = requstmodel.STOR_LOCATION_TO;
                lstmodel.Add(model);
                SAPPlantwiseStockRequestModel rfcmodelinsert = new SAPPlantwiseStockRequestModel();
                rfcmodelinsert.I_SO_DATAS = lstmodel[0];
                rfcmodelinsert.I_NO_ZERO = "X";

                var result = someFunction.Invoke<SAPPlantwiseStockResultModel>(rfcmodelinsert).ET_STK_OVERVIEWs.ToArray();
                connection.Disconnect();

                //var result = someFunction.Invoke<SAPOrderOutputModel>(new I_SO_DATA { ORD_TYPE = requstmodel.ORD_TYPE, SALES_ORG = requstmodel.SALES_ORG, DIST_CHNL = requstmodel.DIST_CHNL, DIVISION = requstmodel.DIVISION, SOLD_TO = requstmodel.SOLD_TO, SHIP_TO = requstmodel.SHIP_TO, PO_DATE = requstmodel.PO_DATE, PO_NUM = requstmodel.PO_NUM, DOC_DATE = requstmodel.DOC_DATE, INCO_TERMS1 = requstmodel.INCO_TERMS1, INCO_TERMS2 = requstmodel.INCO_TERMS2, PRICE_LIST = requstmodel.PRICE_LIST, TRANS_GROUP = requstmodel.TRANS_GROUP, MAT_NUM = requstmodel.MAT_NUM, QTY = requstmodel.QTY, DEVLY_PLANT = requstmodel.DEVLY_PLANT, WEB_ORD = requstmodel.WEB_ORD }).E_ORD_NUMs.ToArray();

                ///var result = someFunction.Invoke<SAPOrderOutputModel>(new InsertOrderIntoRFCRequestModel {I_SO_DATAs  =model }).E_ORD_NUMs.ToArray();

                return result.ToList();
            }
            else
            {
                List<ET_STK_OVERVIEW> result = new List<ET_STK_OVERVIEW>();
                return result;
            }
        }

        public string InsertCDPayment(CDPaymentModel model)
        {
            var dbPara = new DynamicParameters(); 
            dbPara.Add("Date", Convert.ToDateTime(model.Date).ToString("MM-dd-yyyy"), DbType.String);
            dbPara.Add("CustomerCodevtxt", model.CustomerCodevtxt, DbType.String);
            dbPara.Add("Quantitydcl", model.Quantitydcl, DbType.String);
            dbPara.Add("AdvanceCDdcl", model.AdvanceCDdcl, DbType.Decimal);
            dbPara.Add("DebitNotedcl", model.DebitNotedcl, DbType.String); 
            #region using dapper

            var data = _customerPortalHelper.Insert<string>("[dbo].[uspInsertCDPayment]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }
    }
}