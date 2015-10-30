using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ZSharp.Framework.MongoDb;

namespace Framework.MongoDb.Test
{
    public abstract class BaseCustomer : StringKeyMongoEntity
    {
        /// <summary>
        /// 可手動顯示賦值為自增長
        /// </summary>
        public int CustId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Address HomeAddress { get; set; }

        public IList<Order> Orders { get; set; }

        public DateTime CreateDate { get; set; }

        //public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (string.IsNullOrEmpty(FirstName))
        //    {
        //        yield return new ValidationResult("FirstName must have a value", new[] { "FirstName" });
        //    }
        //}
    }

    [CollectionName("CustomersTest")]
    public class Customer : BaseCustomer
    {      
    }

    [CollectionName("CustomersTest2")]
    public class Customer2 : BaseCustomer
    { 
    }

    [CollectionName("CustomersTest3")]
    public class Customer3 : BaseCustomer
    {      
    }

    [CollectionName("CustomersTest4")]
    public class Customer4 : BaseCustomer
    {
    }

    [CollectionName("CustomersTest5")]
    public class Customer5 : BaseCustomer
    {
    }

    [CollectionName("CustomersTest6")]
    public class Customer6 : BaseCustomer
    {
    }

    [CollectionName("CustomersTest7")]
    public class Customer7 : BaseCustomer
    {
    }

    public class Address
    {
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        [BsonIgnoreIfNull]
        public string Country { get; set; }
    }
}
