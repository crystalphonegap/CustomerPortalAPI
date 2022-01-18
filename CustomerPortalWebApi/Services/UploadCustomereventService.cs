using CustomerPortalWebApi.Interface;
using CustomerPortalWebApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Services
{
    public class UploadCustomereventService : IUploadCustomerevent
    {

        private readonly ICustomerPortalHelper _customerPortalHelper;
        public UploadCustomereventService(ICustomerPortalHelper customerPortalHelper)
        {
             _customerPortalHelper = customerPortalHelper;
        }       
        public async Task<long> InsertCustomerEvents(CustomerEventFilesModels events,  IFormFile siteFile)
        {

            var fileSourceLocation = "EventImg";
            var fileChaildLocation = "DealerDiwali";
            var filedb =  await  UploadedSitePhotoFiles(siteFile,fileSourceLocation,fileChaildLocation,events.CustomerCodevtxt);
            //var ImageURLNew="Uploads\\"+fileFolderLocation+"\\"+filedb; 
             var dbPara = new DynamicParameters();
            dbPara.Add("CustomerCodevtxt", events.CustomerCodevtxt, DbType.String);
            dbPara.Add("EventTypevtxt", events.EventTypevtxt, DbType.String);
            dbPara.Add("Filevtxt", filedb, DbType.String);
            dbPara.Add("CreatedByvtxt", events.CreatedByvtxt, DbType.String);
            dbPara.Add("SourceLocation", fileSourceLocation, DbType.String);
            dbPara.Add("ChaildLocation", fileChaildLocation, DbType.String);
            
            #region using dapper

            var data = _customerPortalHelper.Insert<long>("[dbo].[uspInsertCustomerEventUploadfiles]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }

       /*  public long UpdateCustomerEvents(CustomerEventFilesModels events, IFormFile siteFile, long id)
        {
             var fileSourceLocation = "wwwroot";
            var fileChaildLocation = "DealerEvent";
            var filedb=   UploadedSitePhotoFilesNew(siteFile,fileSourceLocation,fileChaildLocation,events.CustomerCodevtxt,  id);
            //var ImageURLNew="Uploads\\"+fileFolderLocation+"\\"+filedb; 
             var dbPara = new DynamicParameters();
            dbPara.Add("IdBint", id, DbType.Int64);
            dbPara.Add("Filevtxt", filedb, DbType.String);
            dbPara.Add("SourceLocation", fileSourceLocation, DbType.String);
            dbPara.Add("ChaildLocation", fileChaildLocation, DbType.String);
            
            #region using dapper

            var data = _customerPortalHelper.Update<long>("[dbo].[uspUpdateCustomerEventUploadfiles]",
                            dbPara,
                            commandType: CommandType.StoredProcedure);
            return data;

            #endregion using dapper
        }
 */
        public async Task<string> UploadedSitePhotoFiles( IFormFile newFiles1, string LocationFolder,string ChaildLocationFolder,  string CustomerCodevtxt) 
            {
             var fileName="";
                var  file = newFiles1;
               
                 var folderName = Path.Combine(LocationFolder, ChaildLocationFolder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName1 = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    // fileName=fileName1;
                     //fileName=uid+"_"+id+"_"+fileName1;
                     fileName=CustomerCodevtxt+"_"+DateTime.Now.Ticks+fileName1;
                    // fileName=CustomerCodevtxt+"_"+fileName1;
                     var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    //return Ok(new { dbPath });
                };
                return fileName;
            }

           /*  public  string UploadedSitePhotoFilesNew( IFormFile newFiles1, string LocationFolder,string ChaildLocationFolder,  string CustomerCodevtxt, long id) 
            {
             var fileName="";
                var  file = newFiles1;
               
                 var folderName = Path.Combine(ChaildLocationFolder, LocationFolder);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName1 = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    // fileName=fileName1;
                     fileName=CustomerCodevtxt+"_"+id+"_"+fileName1;
                    // fileName=CustomerCodevtxt+"_"+DateTime.Now.Ticks+fileName1;
                    // fileName=CustomerCodevtxt+"_"+fileName1;
                     var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                         file.CopyToAsync(stream);
                    }

                    //return Ok(new { dbPath });
                };
                return fileName;
            } */
            
    }
}