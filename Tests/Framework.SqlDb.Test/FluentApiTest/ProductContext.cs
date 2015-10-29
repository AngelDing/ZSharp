using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Framework.SqlDb.Test
{
    public class ProductContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EFPerson> People { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<OrderLineNote> OrderLineNotes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryCode);   //主鍵
            modelBuilder.Entity<OrderLine>().HasKey(l => new { l.OrderId, l.ProductId }); //組合主鍵

            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50);
            modelBuilder.Entity<Product>().Property(p => p.ProductId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<EFPerson>().Ignore(p => p.Name);
            modelBuilder.ComplexType<Address>();
            modelBuilder.Entity<Product>().HasRequired(p => p.PrimaryCategory)
                .WithMany(c => c.Products).HasForeignKey(p => p.PrimaryCategoryCode);     //標準一對多
            modelBuilder.Entity<OrderLine>().HasRequired(l => l.Product)
                .WithMany().HasForeignKey(l => l.ProductId);                   //只有一个导航属性的关系
            modelBuilder.Entity<Category>().HasMany(c => c.Products)
                .WithRequired(p => p.PrimaryCategory).HasForeignKey(p => p.PrimaryCategoryCode)
                .WillCascadeOnDelete(false);                                  //關閉級聯刪除
            modelBuilder.Entity<OrderLineNote>().HasRequired(n => n.OrderLine)
                .WithMany(l => l.Notes).HasForeignKey(n => new { n.OrderId, n.ProductId });  //組合外鍵

            modelBuilder.Entity<Order>().HasRequired(c => c.Person)
                .WithMany(t => t.Orders).Map(m => m.MapKey("CustomFkToPersonId"));   //重命名模型中未定义的外键
            modelBuilder.Entity<Category>().Property(c => c.Name).HasColumnName("cat_name");  //自定義列名
            modelBuilder.Entity<Category>().ToTable("MyCategories");    //自定義表名
            //單表繼承
            modelBuilder.Entity<Product>()
                .Map<Product>(m => m.Requires("Type").HasValue("Current"))
                .Map<DiscontinuedProduct>(m => m.Requires("Type").HasValue("Old"));
            //類表繼承
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<DiscontinuedProduct>().ToTable("OldProducts");
            //具體表繼承
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<DiscontinuedProduct>()
                .Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("OldProducts");
                });
        }
    }
    public class Category
    {
        public string CategoryCode { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string PrimaryCategoryCode { get; set; }
        public virtual Category PrimaryCategory { get; set; }
        public string SecondaryCategoryCode { get; set; }
        public virtual Category SecondaryCategory { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
    public class DiscontinuedProduct : Product
    {
        public DateTime DiscontinuedDate { get; set; }
    }
    public class Tag
    {
        public string TagId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
    public class EFPerson
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderLine> Lines { get; set; }
        public EFPerson Person { get; set; }
    }
    public class OrderLine
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public ICollection<OrderLineNote> Notes { get; set; }
    }
    public class OrderLineNote
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Note { get; set; }
        public OrderLine OrderLine { get; set; }
    }
}
