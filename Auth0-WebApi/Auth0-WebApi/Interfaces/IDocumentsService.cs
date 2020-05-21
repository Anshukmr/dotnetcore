using Auth0.WebApi.Models;
using Auth0.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Interfaces
{
    public interface IDocumentsService
    {
        bool IsAdmin(string user);
        bool UserHasPermission(string user);
        bool UserHasFullEditPermission(Document dco, string user);
        bool UserHasFullDeletePermission(Document dco, string user);

        DocumentOutput GetDocumentReadMode(Document dco);

        DocumentOutput GetDocumentWriteMode(Document dco);
    }
}
