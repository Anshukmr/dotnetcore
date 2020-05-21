using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.Interfaces
{
    public interface ICompanyRepository<TEntity>
    {
        void Add(TEntity b);
        void Edit(TEntity b);
        void Remove(Guid Id);
        IEnumerable<TEntity> GetCompany();
        TEntity FindByCompanyID(int? Id);
    }
}
