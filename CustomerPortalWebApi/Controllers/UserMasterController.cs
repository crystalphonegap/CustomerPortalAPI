using ClosedXML.Excel;
using CustomerPortalWebApi.Entities;
using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using CustomerPortalWebApi.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CustomerPortalWebApi.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserMasterController : ControllerBase
    {
        private readonly IUserMasterService _userMasterService;
        private readonly IChecktokenservice _Checktokenservice;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger _ILogger;
        private static bool AngularEncryption = true;

        public UserMasterController(IUserMasterService userMasterService, JwtSettings jwtSettings, ILogger ILoggerservice, IChecktokenservice checktokenservice)
        {
            _ILogger = ILoggerservice;
            _userMasterService = userMasterService;
            _jwtSettings = jwtSettings;
            _Checktokenservice = checktokenservice;
        }

        [HttpGet("GetYear")]
        public IActionResult GetYear()
        {
            try
            {
                DateTime now = DateTime.Today;
                string year = now.ToString("yyyy");
                return Ok(year);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetUserDetails")]
        public IActionResult GetUserDetails()
        {
            try
            {
                return Ok(_userMasterService.ListAll().ToList());
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetUserDetails/{Id}")]
        public IActionResult GetUserDetails(long Id)
        {
            try
            {
                var UserMaster = _userMasterService.GetById(Id);
                return Ok(UserMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }


        [HttpGet("UserDetailById/{Id}")]
        public IActionResult UserDetailById(long Id)
        {
            try
            {
                var UserMaster = _userMasterService.UserDetailById(Id);
                return Ok(UserMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetProfile/{Id}")]
        public IActionResult GetProfile(long Id)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenByID(authorize[1].Trim(), Id))
                {
                    var UserMaster = _userMasterService.GetById(Id);
                if (UserMaster != null)
                {
                    UserMaster = new UserMaster()
                    {
                        Idbint = UserMaster.Idbint,
                        UserCodetxt = UserMaster.UserCodetxt,
                        UserNametxt = UserMaster.UserNametxt,
                        UserTypetxt = UserMaster.UserTypetxt,
                        Divisionvtxt = UserMaster.Divisionvtxt,
                        Mobilevtxt = UserMaster.Mobilevtxt,
                        Emailvtxt = UserMaster.Emailvtxt,
                    };

                    return Ok(UserMaster);
                }
                else
                {
                    return null;
                }
                }
                else
                {
                    return Ok("Un Authorized User");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "Object reference not set to an instance of an object.")
                {
                    _ILogger.Log(ex);
                }
                return BadRequest();
            }
        }


        #region EmployeeUpdate  

        [HttpPut("EmployeeUpdate")]
        public ActionResult EmployeeUpdate(UserMasterModel model)
        {
            try
            {
                var userMaster = new UserMaster()
                {
                    Idbint = model.Idbint,
                    UserCodetxt = model.UserCodetxt,
                    UserNametxt = model.UserNametxt,
                    UserTypetxt = model.UserTypetxt,
                    Divisionvtxt = model.Divisionvtxt,
                    Mobilevtxt = model.Mobilevtxt,
                    Emailvtxt = model.Emailvtxt,
                    Passwordvtxt = Encrypttxt(model.Passwordvtxt),
                    IsActivebit = model.IsActivebit,
                    ParentCodevtxt = model.ParentCodevtxt,
                    ModifyByint = 1,
                    ModifyDatedatetime = DateTime.Now
                };

                _userMasterService.EmployeeUpdate(userMaster);
                return Ok(userMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        #endregion Update User Master


        #region Create User Master

        [HttpPost("Create")]
        public ActionResult Create(UserMasterModel model)
        {
            try
            {
                var userMaster = new UserMaster()
                {
                    UserCodetxt = model.UserCodetxt,
                    UserNametxt = model.UserNametxt,
                    UserTypetxt = model.UserTypetxt,
                    Divisionvtxt = model.Divisionvtxt,
                    Mobilevtxt = model.Mobilevtxt,
                    ParentCodevtxt = model.ParentCodevtxt,
                    Emailvtxt = model.Emailvtxt,
                    IsActivebit = model.IsActivebit,
                    Passwordvtxt = Encrypttxt(model.Passwordvtxt),
                    CreatedByint = 1,
                    CreatedDatedatetime = DateTime.Now,
                    ModifyByint = 1,
                    ModifyDatedatetime = DateTime.Now
                };

                return Ok(_userMasterService.Create(userMaster));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        #endregion Create User Master

        #region Update User Master

        [HttpPut("Update")]
        public ActionResult Update(UserMasterModel model)
        {
            try
            {
                var userMaster = new UserMaster()
                {
                    Idbint = model.Idbint,
                    UserCodetxt = model.UserCodetxt,
                    UserNametxt = model.UserNametxt,
                    UserTypetxt = model.UserTypetxt,
                    Divisionvtxt = model.Divisionvtxt,
                    Mobilevtxt = model.Mobilevtxt,
                    Emailvtxt = model.Emailvtxt,
                    Passwordvtxt = Encrypttxt(model.Passwordvtxt),
                    IsActivebit = model.IsActivebit,
                    ModifyByint = 1,
                    ModifyDatedatetime = DateTime.Now
                };

                _userMasterService.Update(userMaster);
                return Ok(userMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPut("UpdateStatus")]
        public ActionResult UpdateStatus(UserMasterModel model)
        {
            try
            {
                var userMaster = new UserMaster()
                {
                    Idbint = model.Idbint,
                    UserCodetxt = model.UserCodetxt,
                    UserNametxt = model.UserNametxt,
                    UserTypetxt = model.UserTypetxt,
                    Divisionvtxt = model.Divisionvtxt,
                    Mobilevtxt = model.Mobilevtxt,
                    Emailvtxt = model.Emailvtxt,
                    Passwordvtxt = model.Passwordvtxt,
                    IsActivebit = model.IsActivebit,
                    ModifyByint = 1,
                    ModifyDatedatetime = DateTime.Now
                };

                _userMasterService.UpdateStatus(userMaster);
                return Ok(userMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        #endregion Update User Master

        #region EditProfile

        [HttpPut("EditProfile")]
        public ActionResult EditProfile(UserMasterModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenByID(authorize[1].Trim(), model.Idbint))
                {
                    var userMaster = new UserMaster()
                    {
                        Idbint = model.Idbint,
                        UserNametxt = model.UserNametxt,
                        Mobilevtxt = model.Mobilevtxt,
                    };
                    return Ok(_userMasterService.EditProfile(userMaster));
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

        #endregion EditProfile

        #region Change User Password

        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword(UserMasterModel model)
        {
            try
            {
                string Token = Request.Headers["Authorization"];
                string[] authorize = Token.Split(" ");
                if (_Checktokenservice.CheckTokenByID(authorize[1].Trim(), model.Idbint))
                {
                    var userMaster = new UserMaster()
                    {
                        Idbint = model.Idbint,
                        Passwordvtxt = Encrypttxt(model.Passwordvtxt),
                        NewPassword = Encrypttxt(model.NewPassword),
                    };
                    return Ok(_userMasterService.ChangePassword(userMaster));
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

        #endregion Change User Password

        #region Delete User Master

        [HttpDelete("Delete/{ID}")]
        public IActionResult Delete(long ID)
        {
            try
            {
                _userMasterService.Delete(ID);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        public IActionResult Refresh(RefreshTokenRequest requesttoken)
        {
            try
            {
                if (requesttoken.TokenTxt == "undefined" || requesttoken.RefreshTokenTxt == "undefined")
                {
                    return new ObjectResult(new
                    {
                        Issue = "Invalid refresh token",
                    });
                }
                var principal = GetPrincipalFromExpiredToken(requesttoken.TokenTxt);
                var username = principal.Claims.ToList();
                var savedRefreshToken = _userMasterService.GetRefreshToken(username[0].Value, requesttoken.RefreshTokenTxt); //retrieve the refresh token from a data store
                if (savedRefreshToken != requesttoken.RefreshTokenTxt)

                    return new ObjectResult(new
                    {
                        Issue = "Invalid refresh token",
                    });
                //throw new SecurityTokenException("Invalid refresh token");

                var newJwtToken = GenerateToken(principal.Claims);
                var newRefreshToken = GenerateRefreshToken();
                _userMasterService.DeleteRefreshToken(username[0].Value, requesttoken.RefreshTokenTxt);
                _userMasterService.SaveRefreshToken(username[0].Value, newRefreshToken);

                return new ObjectResult(new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        private RefreshToken GenerateRefreshToken1()
        {
            RefreshToken refreshToken = new RefreshToken();
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Tokentxt = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDatedatetime = DateTime.UtcNow.AddHours(12);
            return refreshToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var jwt = new JwtSecurityToken(issuer: "http://localhost:44335",
                audience: "CustomerPortalUsers",
                claims: claims, //the user's claims, for example new Claim[] { new Claim(ClaimTypes.Name, "The username"), //...
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt); //the method is called WriteToken but returns a string
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        #endregion Delete User Master

        [HttpGet("Search/{KeyWord}")]
        public IActionResult Search(string KeyWord)
        {
            try
            {
                return Ok(_userMasterService.Search(KeyWord));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetUserPagination/{pageNo},{pageSize}")]
        public IActionResult GetUserPagination(int pageNo, int pageSize)
        {
            try
            {
                return Ok(_userMasterService.UserPaging(pageNo, pageSize));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetAllUserCount")]
        public long GetAllUserCount()
        {
            try
            {
                return _userMasterService.UserCount();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserMaster userMaster)
        {
            try
            {
                 //var ChangePassword = Decrypttxt("6q15UoUrkjuPauz9og+XvGtAJuDIrb8LtX8KY2ntQmY=");

                IActionResult ret;
                UserAuthenticationObject obj = new UserAuthenticationObject();
                SecurityManager security = new SecurityManager(_userMasterService, _jwtSettings);
                obj = security.ValidateUser(userMaster.UserCodetxt, Encrypttxt(userMaster.Passwordvtxt));

                if (obj.IsAuthenticated)
                {
                    if (obj.IDbint != 0)
                    {
                        RefreshToken refreshToken = GenerateRefreshToken1();
                        refreshToken.UserIDbint = obj.UserCodetxt;
                        //userMaster.RefreshTokens.Add(refreshToken);
                        _userMasterService.SaveRefreshToken(refreshToken.UserIDbint, refreshToken.Tokentxt);
                        obj.RefreshToken = refreshToken.Tokentxt;
                        ret = StatusCode((int)HttpStatusCode.OK, obj);
                    }
                    else
                    {
                        ret = StatusCode((int)HttpStatusCode.OK, obj);
                    }
                }
                else
                {
                    ret = StatusCode((int)HttpStatusCode.NotFound, "user not found");
                }
                return ret;
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("LoginLogs")]
        public IActionResult LoginLogs(UserMaster userMaster)
        {
            try
            {
                return Ok(_userMasterService.LoginLogs(userMaster.UserCodetxt, userMaster.UserNametxt, userMaster.UserTypetxt, userMaster.BrowserName, userMaster.IpAddress));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("DeleteErrorLog/{DelDate}")]
        public IActionResult DeleteErrorLog(string DelDate)
        {
            try
            {
                return Ok(_userMasterService.DeleteErrorLog(DelDate));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpPost("EnterLog")]
        public IActionResult LoginLogs(ErrorModel ErrorModel)
        {
            try
            {
                _ILogger.LogToDB(ErrorModel.ExceptionMessage);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //private RefreshToken GenerateRefreshToken()
        //{
        //    RefreshToken refreshToken = new RefreshToken();
        //    var randomNumber = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(randomNumber);
        //        refreshToken.Tokentxt = Convert.ToBase64String(randomNumber);
        //    }
        //    refreshToken.ExpiryDatedatetime = DateTime.UtcNow.AddMonths(3);
        //    return refreshToken;GetUserDetails
        //}

        [HttpGet("GetAllUserMasterforDivisionalAdminSearch/{pageNo},{pageSize},{keyword}")]
        public IActionResult GetAllUserMasterforDivisionalAdminSearch(int pageNo, int pageSize, string keyword)
        {
            try
            {
                return Ok(_userMasterService.GetAllUserMasterForDivisionalAdminSearch(pageNo, pageSize, keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetError/{fromdate},{todate},{PageNo},{PageSize},{KeyWord}")]
        public IActionResult GetError(string fromdate, string todate, int PageNo, int PageSize, string KeyWord)
        {
            try
            {
                return Ok(_userMasterService.GetError(fromdate, todate, PageNo, PageSize, KeyWord, "List"));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetErrorCount/{fromdate},{todate},{KeyWord}")]
        public IActionResult GetErrorCount(string fromdate, string todate, string KeyWord)
        {
            try
            {
                return Ok(_userMasterService.GetError(fromdate, todate, 0, 0, KeyWord, "count"));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetAllUserMasterByParentCode/{status},{ParentCode},{pageNo},{pageSize},{keyword}")]
        public IActionResult GetAllUserMasterByParentCode(string status, string ParentCode, int pageNo, int pageSize, string keyword)
        {
            try
            {
                return Ok(_userMasterService.GetAllUserMasterByParentCode(status, ParentCode, pageNo, pageSize, keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("GetAllUserMasterCountByParentCode/{status},{ParentCode},{keyword}")]
        public IActionResult GetAllUserMasterCountByParentCode(string status, string ParentCode, string keyword)
        {
            try
            {
                return Ok(_userMasterService.GetAllUserMasterCountByParentCode(status, ParentCode, keyword));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for Usermaster for DivisionalAdmin Count
        [HttpGet("GetAllUserMasterforDivisionalAdminCount/{keyword}")]
        public long GetAllUserMasterforDivisionalAdminCount(string keyword)
        {
            try
            {
                return _userMasterService.GetAllUserMasterForDivisionalAdminCount(keyword);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return 0;
            }
        }

        //use for Usermaster for DivisionalAdmin download
        [HttpGet("ExcelToExcel/{KeyWord}")]
        public IActionResult ExcelToExcel(string KeyWord)
        {
            try
            {
                List<UserMasterModel> userlist = _userMasterService.GetAllUserMasterForDivisionalAdminDownload(KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "User Code";
                    worksheet.Cell(currentRow, 3).Value = "User Name";
                    worksheet.Cell(currentRow, 4).Value = "User Type";
                    worksheet.Cell(currentRow, 5).Value = "Email";
                    worksheet.Cell(currentRow, 6).Value = "Mobile No";
                    worksheet.Cell(currentRow, 7).Value = "Password";
                    worksheet.Cell(currentRow, 8).Value = "Parent Code ";
                    foreach (var users in userlist)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = users.UserCodetxt;
                        worksheet.Cell(currentRow, 3).Value = users.UserNametxt;
                        worksheet.Cell(currentRow, 4).Value = users.UserTypetxt;
                        worksheet.Cell(currentRow, 5).Value = users.Emailvtxt;
                        worksheet.Cell(currentRow, 6).Value = users.Mobilevtxt;
                        worksheet.Cell(currentRow, 7).Value = Decrypttxt(users.Passwordvtxt);
                        worksheet.Cell(currentRow, 8).Value = users.ParentCodevtxt;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "UserList.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for UserMaster Role HeaderBy Usercode
        [HttpGet("GetUserRolesHeader/{usercode}")]
        public IActionResult GetUserRolesHeader(string usercode)
        {
            try
            {
                return Ok(_userMasterService.GetUserRolesHeader(usercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for insert user roles
        [HttpPost("InsertUserRoles")]
        public ActionResult InsertUserRoles(UserRolesHeader model)
        {
            try
            {
                var userMaster = new UserRolesHeader()
                {
                    IDbint = model.IDbint,
                    RoleIDbint = model.RoleIDbint,
                    UserCodevtxt = model.UserCodevtxt,
                    CreatedByvtxt = model.CreatedByvtxt,
                    CreatedDatetimedatetime = DateTime.Now
                };
                _userMasterService.InsertUserRolesHeader(userMaster);
                return Ok(userMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for delete user roles
        [HttpDelete("DeleteUserRoles/{usercode}")]
        public IActionResult DeleteUserRoles(string usercode)
        {
            try
            {
                _userMasterService.DeleteUserRoles(usercode);
                return Ok();
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        //use for UserMaster Role DetailsBy Usercode
        [HttpGet("GetUserRolesDetails/{usercode}")]
        public IActionResult GetUserRolesDetails(string usercode)
        {
            try
            {
                return Ok(_userMasterService.GetUserRolesDetails(usercode));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword(UserMaster model)
        {
            try
            {
                var userMaster = new UserMaster()
                {
                    UserCodetxt = model.UserCodetxt,
                    ResetTokenvtxt = model.ResetTokenvtxt,
                    NewPassword = Encrypttxt(model.NewPassword),
                };
                return Ok(_userMasterService.ResetPassword(userMaster));
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        [HttpGet("ExportToExcelForParent/{ParentCode},{status},{KeyWord}")]
        public IActionResult ExportToExcelForParent(string ParentCode, string status, string KeyWord)
        {
            try
            {
                List<UserMasterModel> userlist = _userMasterService.ExportToExcelForParent(ParentCode, status, KeyWord);
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");
                    var currentRow = 1;
                    var srNo = 1;
                    worksheet.Cell(currentRow, 1).Value = "Sr No";
                    worksheet.Cell(currentRow, 2).Value = "User Code";
                    worksheet.Cell(currentRow, 3).Value = "User Name";
                    worksheet.Cell(currentRow, 4).Value = "User Type";
                    worksheet.Cell(currentRow, 5).Value = "Email";
                    worksheet.Cell(currentRow, 6).Value = "Mobile No";
                    worksheet.Cell(currentRow, 7).Value = "Password";
                    foreach (var users in userlist)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = srNo++;
                        worksheet.Cell(currentRow, 2).Value = users.UserCodetxt;
                        worksheet.Cell(currentRow, 3).Value = users.UserNametxt;
                        worksheet.Cell(currentRow, 4).Value = users.UserTypetxt;
                        worksheet.Cell(currentRow, 5).Value = users.Emailvtxt;
                        worksheet.Cell(currentRow, 6).Value = users.Mobilevtxt;
                        worksheet.Cell(currentRow, 7).Value = Decrypttxt(users.Passwordvtxt);
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "UserList.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogger.Log(ex);
                return BadRequest();
            }
        }

        public static string Encrypttxt(string clearText)
        {
            string temptext = clearText;
            if (AngularEncryption == true)
            {
                temptext = DecryptAngulartxtAES(clearText);
            }
            else
            {
                temptext = clearText;
            }
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(temptext);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    temptext = Convert.ToBase64String(ms.ToArray());
                }
            }
            return temptext;
        }

        public static string Decrypttxt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string DecryptAngulartxtAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("4512631236589784");
            var iv = Encoding.UTF8.GetBytes("4512631236589784");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return decriptedFromJavascript;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        public static string EncrypAngularttxtAES(string plainText)
        {
            var keybytes = Encoding.UTF8.GetBytes("4512631236589784");
            var iv = Encoding.UTF8.GetBytes("4512631236589784");

            var encryoFromJavascript = EncryptStringToBytes(plainText, keybytes, iv);
            return Convert.ToBase64String(encryoFromJavascript);
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}