namespace SOPS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using SOPS.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                // System.Diagnostics.Debugger.Launch();
            }
            // This method will be called after migrating to the latest version.

            // Generation configuration
            int companyCount = 20;
            int uniqueAddressStreetCount = 2;
            int uniqueAddressCityCount = 5;

            int userCount = 10;
            int uniqueUserNameCount = 5;
            int uniqueUserSurnameCount = 10;

            int productCount = 10;
            int employeeCount = 10;
            int productRatingCount = 10;
            int productCommentCount = 10;
            int companyReportCount = 10;
            int watchedProductCount = 10;
            int existingProductCount = 10;
            int qrCodeCount = 10;
            int companyStatisticsCountPerCompany = 10;

            Random random = new Random();

            // delete everything
            context.CompanyStatistics.RemoveRange(context.CompanyStatistics);
            context.SaveChanges();
            context.QRs.RemoveRange(context.QRs);
            context.SaveChanges();
            context.ProductComments.RemoveRange(context.ProductComments);
            context.SaveChanges();
            context.ExistingProducts.RemoveRange(context.ExistingProducts);
            context.SaveChanges();
            context.ProductRatings.RemoveRange(context.ProductRatings);
            context.SaveChanges();
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();
            context.WatchedProducts.RemoveRange(context.WatchedProducts);
            context.SaveChanges();
            ((DbSet<ApplicationUser>)context.Users).RemoveRange(context.Users);
            context.SaveChanges();
            context.Companies.RemoveRange(context.Companies);
            context.SaveChanges();
            context.Products.RemoveRange(context.Products);
            context.SaveChanges();
            ((DbSet<IdentityRole>)context.Roles).RemoveRange(context.Roles);
            context.SaveChanges();
            context.Statistics.RemoveRange(context.Statistics);
            context.SaveChanges();

            // UserRoles
            context.Roles.Add(new IdentityRole("User"));
            context.Roles.Add(new IdentityRole("Employee"));
            context.Roles.Add(new IdentityRole("Administrator"));
            context.SaveChanges();

            // Company
            for (int i = 0; i < companyCount; ++i)
            {
                List<int> nipDigits = new List<int>(new int[10]);
                List<int> nipWeights = new List<int>(new int[9] { 6, 5, 7, 2, 3, 4, 5, 6, 7 });
                int nipLastDigit = 0;
                for (int j = 0; j < nipWeights.Count; j++)
                {
                    nipDigits[j] = random.Next(10);
                    nipLastDigit += nipDigits[j] * nipWeights[j];
                }
                nipLastDigit = nipLastDigit % 11;
                nipDigits[9] = nipLastDigit;

                Company company = new Company
                {
                    Name = "companyName" + i,
                    Kind = ((random.Next(2) == 0) ? "Production" : "Services"),
                    AddressStreet = "addressStreet" + random.Next(uniqueAddressStreetCount),
                    AddressZipCode = "CT-" + random.Next(99999),
                    AddressCity = "addressCity" + random.Next(uniqueAddressCityCount),
                    Email = "companyName" + i + "@mail.com",
                    NIP = String.Join("", nipDigits),
                    REGON = random.Next(999999999).ToString(),
                    JoinDate = new DateTime(random.Next(2015, 2017), random.Next(12) + 1, random.Next(25) + 1)
                };
                context.Companies.AddOrUpdate(c => c.Name, company);
            }
            context.SaveChanges();

            // User
            var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            for (int i = 0; i < userCount; i++)
            {
                List<int> phoneNumber = new List<int>(new int[10]);
                for (int j = 0; j < phoneNumber.Count; j++)
                {
                    phoneNumber[j] = random.Next(10);
                }
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "user" + i,
                    Name = "name" + random.Next(uniqueUserNameCount),
                    Surname = "surname" + random.Next(uniqueUserSurnameCount),
                    Email = "user" + i + "@email.com",
                    EmailConfirmed = true,
                    PasswordHash = ("user" + i).GetHashCode().ToString(),
                    SecurityStamp = "?",
                    PhoneNumber = String.Join("", phoneNumber.ToArray()),
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };
                manager.CreateAsync(user, user.UserName).Wait();

                if (i == 0)
                {
                    manager.AddToRoleAsync(user.Id, "Administrator").Wait();
                }
            }

            // Employee            
            List<Employee> employees = new List<Employee>(employeeCount);
            for (int i = 0; i < employeeCount; i++)
            {
                Employee employee = new Employee
                {
                    UserId = context.Users.ToList()[random.Next(context.Users.Count())].Id,
                    JoinDate = new DateTime(random.Next(2015, 2018), random.Next(12) + 1, random.Next(25) + 1),
                    CompanyId = context.Companies.ToList()[random.Next(context.Companies.Count())].Id
                };
                employees.Add(employee);
                manager.AddToRoleAsync(employee.UserId, "Employee").Wait();
            }
            var distinceEmployees = employees.Distinct(new EmployeeEqualityComparer()).ToArray();
            context.Employees.AddOrUpdate(e => e.UserId, distinceEmployees);
            context.SaveChanges();

            // Product
            for (int i = 0; i < productCount; i++)
            {
                Product product = new Product
                {
                    Name = "product" + i,
                    Barcode = random.Next(9999999).ToString() + random.Next(999999),
                    Description = "description" + i,
                    CreationDate = new DateTime(random.Next(2015, 2017), random.Next(12) + 1, random.Next(25) + 1),//new DateTime(random.Next(2018, 2019), random.Next(1, 13), random.Next(25) + 1),
                    CountryOfOrigin = "country" + random.Next(10),
                    SuggestedPrice = (decimal)random.Next(201),
                    CompanyId = context.Companies.ToList()[random.Next(context.Companies.Count())].Id
                };
                context.Products.AddOrUpdate(p => p.Name, product);
            }
            context.SaveChanges();

            // ProductRating
            List<ProductRating> productRatings = new List<ProductRating>(productRatingCount);
            for (int i = 0; i < productRatingCount; i++)
            {
                ProductRating productRating = new ProductRating
                {
                    Rating = (float)random.NextDouble() * 10,
                    UserId = context.Users.ToList()[random.Next(context.Users.Count())].Id,
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id,
                    Added = new DateTime(2018, 12, random.Next(28) + 1)
                };
                if (!productRatings.Any(pr => pr.UserId.Equals(productRating.UserId) && pr.ProductId.Equals(productRating.ProductId)))
                {
                    productRatings.Add(productRating);
                }
            }
            context.ProductRatings.AddOrUpdate(pr => pr.UserId, productRatings.ToArray());
            context.SaveChanges();

            // WatchedProduct
            var watchedProductList = new List<WatchedProduct>(watchedProductCount);
            for (int i = 0; i < watchedProductCount; i++)
            {
                WatchedProduct watchedProduct = new WatchedProduct
                {
                    ApplicationUserId = context.Users.ToList()[random.Next(context.Users.Count())].Id,
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id
                };
                if (watchedProductList.Any(wp => wp.ApplicationUserId == watchedProduct.ApplicationUserId && wp.ProductId == watchedProduct.ProductId) == false)
                {
                    watchedProductList.Add(watchedProduct);
                }
            }
            context.WatchedProducts.AddRange(watchedProductList);
            context.SaveChanges();

            // ProductComment
            for (int i = 0; i < productCommentCount; i++)
            {
                var productComment = new ProductComment
                {
                    ApplicationUserId = context.Users.ToList()[random.Next(context.Users.Count())].Id,
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id,
                    Comment = "Comment" + i,
                    Date = new DateTime(random.Next(2016, 2018), random.Next(12) + 1, random.Next(25) + 1)
                };
                context.ProductComments.AddOrUpdate(productComment);
            }
            context.SaveChanges();

            // CompanyReport
            for (int i = 0; i < companyReportCount; i++)
            {
                CompanyReport companyReport = new CompanyReport
                {
                    Content = "content" + i,
                    CompanyId = context.Companies.ToList()[random.Next(context.Companies.Count())].Id
                };
                context.CompanyReports.AddOrUpdate(cr => cr.Content, companyReport);
            }
            context.SaveChanges();

            // ExistingProduct
            for (int i = 0; i < existingProductCount; i++)
            {
                ExistingProduct existingProduct = new ExistingProduct
                {
                    ExpirationDate = new DateTime(random.Next(2018, 2030), random.Next(12) + 1, random.Next(25) + 1),
                    CreationDate = new DateTime(random.Next(2018, 2019), random.Next(1, 13), random.Next(25) + 1),
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id                   
                };
                existingProduct.GenerateSecret();
                context.ExistingProducts.AddOrUpdate(existingProduct);
            }
            context.SaveChanges();

            // QR
            List<QR> qrs = new List<QR>(qrCodeCount);
            for (int i = 0; i < qrCodeCount; i++)
            {
                QR qr = new QR
                {
                    ExistingProductId = context.ExistingProducts.ToList()[random.Next(context.ExistingProducts.Count())].Id,
                    Version = 1,
                    Content = new byte[10]
                };
                qrs.Add(qr);
                //context.QRs.AddOrUpdate(q => q.ExistingProductId, qr);
            }
            var distinctQrs = qrs.Distinct(new QREqualityComparer()).ToArray();
            context.QRs.AddOrUpdate(q => q.ExistingProductId, distinctQrs);
            context.SaveChanges();

            // CompanyStatistics
            var companyStatistics = new List<CompanyStatistics>(companyCount * companyStatisticsCountPerCompany);
            foreach (var company in context.Companies)
            {
                for (int j = 0; j < companyStatisticsCountPerCompany; j++)
                {
                    var statistics = new CompanyStatistics()
                    {
                        CompanyId = company.Id,
                        Date = DateTime.Now.AddDays(-j).Date,
                        RegistredProducts = random.Next(10),
                    };
                    companyStatistics.Add(statistics);
                }
            }
            context.CompanyStatistics.AddRange(companyStatistics);
            context.SaveChanges();

            // Statistics
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var currentMonthBeginning = new DateTime(currentYear, currentMonth, 1);

            context.Statistics.Add(new Statistics
            {
                LastMonthCompanyCount = context.Companies.Where(c => c.JoinDate < currentMonthBeginning).ToList().Count(),
                LastMonthProductCount = context.Products.Where(p => p.CreationDate < currentMonthBeginning).ToList().Count()
            });
            context.SaveChanges();
        }
    }

    // Comparers for generator
    class QREqualityComparer : IEqualityComparer<QR>
    {
        public bool Equals(QR x, QR y)
        {
            return x.ExistingProductId == y.ExistingProductId;
        }

        public int GetHashCode(QR obj)
        {
            return obj.ExistingProductId.GetHashCode();
        }
    }
    class EmployeeEqualityComparer : IEqualityComparer<Employee>
    {
        public bool Equals(Employee x, Employee y)
        {
            return x.UserId.Equals(y.UserId);
        }

        public int GetHashCode(Employee obj)
        {
            return obj.UserId.GetHashCode();
        }
    }
}

