using System;
using System.Collections.Generic;
using ZSharp.Framework.Repositories;

namespace Framework.SqlDb.Test
{
    public interface ICustomerRepository : IRepository<EFCustomer, int>
    {
        EFCustomer GetCustomerByName(string name);

        List<EFNote> GetNoteList();

        EFCustomer GetCustomFullInfo(int id);
    }
}
