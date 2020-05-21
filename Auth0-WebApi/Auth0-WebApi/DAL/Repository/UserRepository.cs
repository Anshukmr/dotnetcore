using Auth0.WebApi.Interfaces;
using Auth0.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.DAL.Repository
{
    public class UserRepository : IUserRepository<User>
    {
        CompanyDbContext context = new CompanyDbContext();
        public void Add(User b)
        {
            context.User.Add(b);
            context.SaveChanges();
        }

        public void Edit(User b)
        {
            context.Entry(b).State = EntityState.Modified;
            context.SaveChanges();
        }

        public User FindByEmail(string email)
        {
            var c = (from r in context.User where r.Email.ToString().ToUpper() == email.ToString().ToUpper() select r).FirstOrDefault();
            return c;
        }

        public IEnumerable<User> GetUser()
        {
            return context.User;
        }

        public void Remove(Guid Id)
        {
            User b = context.User.Find(Id);
            context.User.Remove(b);
            context.SaveChanges();
        }
    }
}
