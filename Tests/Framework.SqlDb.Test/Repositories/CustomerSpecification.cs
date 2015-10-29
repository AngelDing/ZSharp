using ZSharp.Framework.Specifications;

namespace Framework.SqlDb.Test
{
    public static class CustomerSpecification
    {
        public static Specification<EFCustomer> GetCustomerByFilter(CustomerQueryCriteria criteria)
        {
            Specification<EFCustomer> specProfile = new TrueSpecification<EFCustomer>();

            if (!string.IsNullOrEmpty(criteria.UserName))
            {
                specProfile &= new DirectSpecification<EFCustomer>(
                    p => p.UserName.Contains(criteria.UserName));
            }

            if (!string.IsNullOrEmpty(criteria.StreetName))
            {
                specProfile &= new DirectSpecification<EFCustomer>(
                    p => p.Address.Street.Contains(criteria.StreetName));
            }

            if (!string.IsNullOrEmpty(criteria.Email))
            {
                specProfile &= new DirectSpecification<EFCustomer>(
                    p => p.Email.Contains(criteria.Email));
            }

            if (!string.IsNullOrEmpty(criteria.Phone))
            {
                specProfile &= new DirectSpecification<EFCustomer>(
                    p => p.Email == criteria.Phone);
            }

            return specProfile;
        }
    }
}
