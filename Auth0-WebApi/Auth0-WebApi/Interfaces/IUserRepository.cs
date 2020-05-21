using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.Interfaces
{
    public interface IUserRepository<TEntity>
    {
        void Add(TEntity b);
        void Edit(TEntity b);
        void Remove(Guid Id);
        IEnumerable<TEntity> GetUser();
        TEntity FindByEmail(string email);
    }
}
