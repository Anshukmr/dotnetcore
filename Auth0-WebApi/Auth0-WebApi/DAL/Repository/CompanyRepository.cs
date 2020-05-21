using MentalVerse.Web.Api.Interfaces;
using MentalVerse.Web.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentalVerse.Web.Api.DAL.Repository
{
    public class CompanyRepository : ICompanyRepository<Company>
    {
        CompanyDbContext context = new CompanyDbContext();
        public void Add(Company b)
        {
            context.Company.Add(b);
            context.SaveChanges();
        }

        public void Edit(Company b)
        {
            context.Entry(b).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Company FindByCompanyID(int? Id)
        {
            var c = (from r in context.Company where r.Id == Id select r).FirstOrDefault();
            return c;
        }

        public IEnumerable<Company> GetCompany()
        {
            return context.Company;
        }

        public void Remove(Guid Id)
        {
            Company b = context.Company.Find(Id);
            context.Company.Remove(b);
            context.SaveChanges();
        }
    }
}
