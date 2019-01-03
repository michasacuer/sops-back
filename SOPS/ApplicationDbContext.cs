using Microsoft.AspNet.Identity.EntityFramework;
using SOPS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SOPS
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<ExistingProduct>().HasOptional(e => e.QR);
        }*/

            public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
                                      
        public DbSet<Employee>          Employees         { get; set; }
        public DbSet<Product>           Products          { get; set; }
        public DbSet<ProductRating>     ProductRatings    { get; set; }
        public DbSet<ProductComment>    ProductComments   { get; set; }
        public DbSet<ProductIssue>      ProductIssues     { get; set; }
        public DbSet<ExistingProduct>   ExistingProducts  { get; set; }
        public DbSet<WatchedProduct>    WatchedProducts   { get; set; }
        public DbSet<Company>           Companies         { get; set; }
        public DbSet<CompanyReport>     CompanyReports    { get; set; }
        public DbSet<CompanyStatistics> CompanyStatistics { get; set; }
        public DbSet<QR>                QRs               { get; set; }
        public DbSet<ShortUrl>          ShortUrls         { get; set; }
        public DbSet<Scan>              Scans             { get; set; }
        public DbSet<Statistics>        Statistics        { get; set; }
    }
}