using AutoMapper;
using Auth0.WebApi.Helper;
using Auth0.WebApi.Interfaces;
using Auth0.WebApi.Models;
using Auth0.WebApi.Models.Response;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.BusinessServices
{
    public class DocumentsService: IDocumentsService
    {
        private readonly IDocumentRepository<Document> _dataRepository;
        private readonly IUserRepository<User> _userRepository;
        private readonly ICompanyRepository<Company> _companyRepository;
        private readonly IMapper _mapper;

        //User Role:
        //Editor
        //Viewer
        //Admin
        public DocumentsService(IDocumentRepository<Document> dataRepository, IUserRepository<User> userRepository, ICompanyRepository<Company> companyRepository, IMapper mapper)
        {
            _dataRepository = dataRepository;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        //GET
        public bool UserHasPermission(string user)
        {
            var userInfo = _userRepository.FindByEmail(user);
            if(userInfo.Role ==1 || userInfo.Role == 2 || userInfo.Role == 3)
                return true;
            else
                return false;
        }


        //PUT-
        public bool UserHasFullEditPermission(Document dco, string user)
         {

            var userInfo = _userRepository.FindByEmail(user);
            if (userInfo.Role == 3 || (userInfo.Role == 1 && dco.Author == user) && dco.Published ==1 && dco.Shared ==1)
                return true;
            else
                return false;
        }

          // DELETE-
         public bool UserHasFullDeletePermission(Document dco, string user)
          {
            var userInfo = _userRepository.FindByEmail(user);
            if (userInfo.Role == 3 || (userInfo.Role ==1 && dco.Author == user))
                return true;
            else
                return false;
        }

        public DocumentOutput GetDocumentReadMode(Document doc)
        {
            //Generate READ Mode SAS Token.
            var companyInfo = _companyRepository.FindByCompanyID(doc.CompanyId);
            var sastoken = AzureStorageHelper.GetBlobSasToken(companyInfo.AzureStorageAccount, companyInfo.BlobContainer, doc.Title, SharedAccessBlobPermissions.Read);

            var document = _mapper.Map<DocumentOutput>(doc);

            document.Path = doc.URI + sastoken;
            return document;
        }

        public DocumentOutput GetDocumentWriteMode(Document doc)
        {
            //Generate READ Mode SAS Token.
            var companyInfo = _companyRepository.FindByCompanyID(doc.CompanyId);
            var sastoken = AzureStorageHelper.GetBlobSasToken(companyInfo.AzureStorageAccount, companyInfo.BlobContainer, doc.Title, SharedAccessBlobPermissions.Write);

            var document = _mapper.Map<DocumentOutput>(doc);
        
            document.Path = doc.URI + sastoken;
            return document;
        }

        public bool IsAdmin(string user)
        {
            var userInfo = _userRepository.FindByEmail(user);
            if (userInfo.Role == 3)
                return true;
            else
                return false;
        }
    }
}
