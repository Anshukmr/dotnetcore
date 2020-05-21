using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.Interfaces
{
    public interface IDocumentRepository<TEntity>
    {
        bool IsValidUser(string email, int CompanyID);

        //Document
        void Add(TEntity b);
        void Edit(TEntity b);
        void Remove(Guid Id);
        IEnumerable<TEntity> GetDocument();
        TEntity FindById(Guid Id);
        IEnumerable<TEntity> FindByDocType(string docType);
        IEnumerable<TEntity> FindByCompanyId(int Id);
    }
}
