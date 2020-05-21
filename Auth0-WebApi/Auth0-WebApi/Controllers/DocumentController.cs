using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using AutoMapper;
using MentalVerse.Web.Api.Helper;
using MentalVerse.Web.Api.Interfaces;
using MentalVerse.Web.Api.Models;
using MentalVerse.Web.Api.Models.Request;
using MentalVerse.Web.Api.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MentalVerse.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DocumentController : ControllerBase
    {       
        private readonly IDocumentsService _documentService;
        private readonly IDocumentRepository<Document> _dataRepository;
        private readonly IMapper _mapper;

        public DocumentController( IDocumentsService documentService, IDocumentRepository<Document> dataRepository, IMapper mapper)
        {          
            _documentService = documentService;
            _dataRepository = dataRepository;
            _mapper = mapper;
        }

        //// GET
        [HttpGet("{companyId}")]
        public async Task<ActionResult<IEnumerable<DocumentOutput>>> GetDocument(int companyId, Guid id, string docType = "")
        {
            // Get user email id from claim.
            UserInfo authoUserInfo = await UserInformation();
       
            if (_dataRepository.IsValidUser(authoUserInfo.Email, companyId) && _documentService.UserHasPermission(authoUserInfo.Email))
            {
                if (id != Guid.Empty)
                {
                    var doc = _dataRepository.FindById(id);
                    var document = _mapper.Map<DocumentOutput>(doc);
                    //
                    document.Path = doc.URI + doc.SASToken;

                    if (document == null)
                    {
                        return NotFound();
                    }
                    List<DocumentOutput> list = new List<DocumentOutput>() { document };
                    return list;
                }
                if (docType != "")
                {
                    var doc = _dataRepository.FindByDocType(docType);
                    var document = _mapper.Map<IEnumerable<DocumentOutput>>(doc);

                    if (document == null)
                    {
                        return NotFound();
                    }
                    return document.ToList();
                }
                else
                {
                    var doc = _dataRepository.FindByCompanyId(companyId);
                    var document = _mapper.Map<IEnumerable<DocumentOutput>>(doc);
                    if (document == null)
                    {
                        return NotFound();
                    }
                    return document.ToList();
                }

            }
            else
                return Unauthorized("Insufficient Privileges.");
        }


        //// PUT:
        [HttpPut("{companyId}/{id}")]
        public async Task<IActionResult> PutDocument(int companyId, Guid id, MentalVerse.Web.Api.Models.Request.DocumentInput document)
        {
            // Get user email id from claim.
            UserInfo authoUserInfo = await UserInformation();

            var documentDto = _mapper.Map<Document>(document);

            if (_dataRepository.IsValidUser(authoUserInfo.Email, companyId) && _documentService.UserHasFullEditPermission(documentDto,authoUserInfo.Email))
            {
                try
                {
                    if (id != documentDto.Id)
                    {
                        return BadRequest();
                    }

                    //TODO:Modify only existing
                    //var docexisting = _dataRepository.FindById(id, "");
                    //_mapper

                    _dataRepository.Edit(documentDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_dataRepository.FindById(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            else
                return Unauthorized("Insufficient Privileges.");
        }

        //// POST:
        [HttpPost("{companyId}")]
        public async Task<ActionResult<Document>> PostDocument(int companyId, DocumentInput document)
        {
            // Get user email id from claim.
            UserInfo authoUserInfo = await UserInformation();
            var documentDto = _mapper.Map<Document>(document);

            if (_dataRepository.IsValidUser(authoUserInfo.Email, companyId))
            {
                try
                {
                    documentDto.CompanyId = companyId;
                    documentDto.Author = authoUserInfo.Email;
                    _dataRepository.Add(documentDto);
                }
                catch (DbUpdateException)
                {
                    if (_dataRepository.FindById(documentDto.Id) !=null) //DocumentExists
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetDocument", new { id = documentDto.Id }, documentDto);
            }
            else
                return Unauthorized("Insufficient Privileges.");
        }

        //// DELETE:
        [HttpDelete("{companyId}/{id}")]
        public async Task<ActionResult<Document>> DeleteDocument(int companyId, Guid id)
        {
            // Get user email id from claim.
            UserInfo authoUserInfo = await UserInformation();

            if (_dataRepository.IsValidUser(authoUserInfo.Email, companyId))
            {
                var document = _dataRepository.FindById(id);
                if (document == null)
                {
                    return NotFound();
                }

                //TODO: IsUser HadFulldeleteAccess- Only Author can delete      
                if(_documentService.UserHasFullDeletePermission(document, authoUserInfo.Email))
                _dataRepository.Remove(id);
                else
                    return Unauthorized("Insufficient Privileges.");
                //TODO: Remove blobs.

                return document;
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
                var userInfo = await apiClient.GetUserInfoAsync(accessToken).ConfigureAwait(false);

                return userInfo;
            }
            return null;
        }
    }
}