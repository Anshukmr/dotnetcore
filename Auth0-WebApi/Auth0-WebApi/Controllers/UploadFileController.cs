using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.WebApi.Helper;
using Auth0.WebApi.Interfaces;
using Auth0.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth0.WebApi.Controllers
{

   
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {

        private readonly IDocumentRepository<Document> _dataRepository;
        private readonly ICompanyRepository<Company> _companyRepository;
 
        public UploadFileController(IDocumentRepository<Document> dataRepository, ICompanyRepository<Company> companyRepository)
        {
            _dataRepository = dataRepository;
            _companyRepository = companyRepository;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Post([FromForm] FileUpload upload)
        {
            // Get user email id from claim.
            UserInfo authoUserInfo = await UserInformation();

            if (_dataRepository.IsValidUser(authoUserInfo.Email, upload.companyId))
            {
                //Check existing doc for upload file.
                if (_dataRepository.FindById(upload.Id) == null)
                {
                    return NotFound();
                }
                string uploadSuccess = "failed";              
                var file = upload.file;
                if (file != null)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        var filename = Path.GetFileName(file.FileName);
                        var companyInfo = _companyRepository.FindByCompanyID(upload.companyId);
                        uploadSuccess = await AzureStorageHelper.UploadToBlob(companyInfo.AzureStorageAccount, companyInfo.BlobContainer, filename, upload.Id, "",stream);
                    }
                }
                else
                    NoContent();          

                return uploadSuccess != "failed"
                    ? (ActionResult)new OkObjectResult($"Success. File path :  {uploadSuccess}")
                    : new BadRequestObjectResult("Upload Failed");

            }
            else
                return Unauthorized("Insufficient Privileges.");
        }

        //APIController user claim object
        private async Task<UserInfo> UserInformation()
        {
            // Retrieve the access_token claim which we saved in the OnTokenValidated event
            string accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;

            // If we have an access_token, then retrieve the user's information
            if (!string.IsNullOrEmpty(accessToken))
            {
                var apiClient = new AuthenticationApiClient(Startup.AuthoDomain);
                var userInfo = await apiClient.GetUserInfoAsync(accessToken);

                return userInfo;
            }
            return null;
        }  
    }
}