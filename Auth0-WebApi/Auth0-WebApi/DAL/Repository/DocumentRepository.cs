using Auth0.WebApi.Interfaces;
using Auth0.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth0.WebApi.DAL.Repository
{
    public class DocumentRepository : IDocumentRepository<Document>
    {
        CompanyDbContext context = new CompanyDbContext();
        public void Add(Document b)
        {
            context.Document.Add(b);
            context.SaveChanges();
        }

        public void Edit(Document b)
        {
            b.ModifiedDate = DateTime.Now;
            context.Entry(b).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Remove(Guid Id)
        {
            Document b = context.Document.Find(Id);
            context.Document.Remove(b);
            context.SaveChanges();
        }

        public IEnumerable<Document> GetDocument()
        {
            return context.Document;
        }

        public Document FindById(Guid Id)
        {
            var c = (from r in context.Document where r.Id.ToString().ToUpper() == Id.ToString().ToUpper() select r).FirstOrDefault();
            return c;
        }
        public IEnumerable<Document> FindByDocType(string docType)
        {
            var c = (from r in context.Document where r.DocType.ToString().ToUpper() == docType.ToString().ToUpper() select r).ToList();
            return c;
        }

        public bool IsValidUser(string email, int companyid)
        {
            var user = (from u in context.User where u.Email.ToString().ToUpper() == email.ToString().ToUpper() select u).FirstOrDefault();        
            return user?.CompanyID == companyid;
        }

        public IEnumerable<Document> FindByCompanyId(int Id)
        {
            var c = (from r in context.Document where r.CompanyId == Id select r).ToList();
            return c;
        }
    }
}
