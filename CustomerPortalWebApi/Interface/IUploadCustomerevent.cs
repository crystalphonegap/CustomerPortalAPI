using CustomerPortalWebApi.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerPortalWebApi.Interface
{
    public interface IUploadCustomerevent
    {
    Task<long> InsertCustomerEvents(CustomerEventFilesModels events, IFormFile siteFile);

     //long UpdateCustomerEvents(CustomerEventFilesModels events, IFormFile siteFile, long id );
    }
}