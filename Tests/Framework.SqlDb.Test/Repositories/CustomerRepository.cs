using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using ZSharp.Framework.SqlDb;

namespace Framework.SqlDb.Test
{
    public class CustomerRepository : EfRepository<EFCustomer, int>, ICustomerRepository
    {
        private readonly EFTestContext db;
        public CustomerRepository()
            : base(new EfRepositoryContext(new EFTestContext()))
        {
            db = ((this.RepoContext as EfRepositoryContext).DbContext) as EFTestContext;
        }

        public EFCustomer GetCustomFullInfo(int id)
        {
            var customer = db.EFCustomer.AsNoTracking()
                .Where(p => p.Id == id)
                .Include(p => p.EFNote.Select(c => c.ChildNote))
                .FirstOrDefault();
            return customer;
        }

        public EFCustomer GetCustomerByName(string name)
        {
            var customer = db.EFCustomer
                .Where(p => p.UserName == name)
                .Include(p => p.EFNote).First();
            return customer;
        }

        public List<EFNote> GetNoteList()
        {
            var list = db.EFNote.ToList();
            return list;
        }
    }
}
