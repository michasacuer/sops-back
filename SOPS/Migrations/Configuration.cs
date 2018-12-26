namespace SOPS.Migrations
{
    using SOPS.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
                //System.Diagnostics.Debugger.Launch();
            }
            // This method will be called after migrating to the latest version.

            // Generation configuration
            int companyCount = 10;
            int uniqueAddressStreetCount = 2;
            int uniqueAddressCityCount = 5;

            int userCount = 20;
            int uniqueUserNameCount = 5;
            int uniqueUserSurnameCount = 10;

            int productCount = 30;
            int employeeCount = 10;
            int productRatingCount = 5;
            int companyReportCount = 10;
            int watchedProductCount = 10;
            int existingProductCount = 30;
            int qrCodeCount = 20;

            Random random = new Random();

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
                    REGON = random.Next(999999999).ToString()
                };
                context.Companies.AddOrUpdate(c => c.Name, company);
            }
            context.SaveChanges();

            // User
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
                context.Users.AddOrUpdate(u => u.UserName, user);
            }
            context.SaveChanges();

            // Employee
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();
            List<Employee> employees = new List<Employee>(employeeCount);
            for (int i = 0; i < employeeCount; i++)
            {
                Employee employee = new Employee
                {
                    UserId = context.Users.ToList()[random.Next(context.Users.Count())].Id,
                    CompanyId = context.Companies.ToList()[random.Next(context.Companies.Count())].Id
                };
                employees.Add(employee);
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
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id
                };
                if (!productRatings.Any(pr => pr.UserId.Equals(productRating.UserId) && pr.ProductId.Equals(productRating.ProductId)))
                {
                    productRatings.Add(productRating);
                }
            }
            context.ProductRatings.RemoveRange(context.ProductRatings);
            context.ProductRatings.AddOrUpdate(pr => pr.Id, productRatings.ToArray());
            context.SaveChanges();

            // WatchedProduct
            context.WatchedProducts.RemoveRange(context.WatchedProducts);
            for (int i = 0; i < watchedProductCount; i++)
            {
                WatchedProduct watchedProduct = new WatchedProduct
                {
                    ApplicationUserId = context.Users.ToList()[random.Next(context.Users.Count())].Id,
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id
                };
                if (!context.WatchedProducts.Any(wp => wp.ApplicationUserId.Equals(watchedProduct.ApplicationUserId) && wp.ProductId.Equals(watchedProduct.ProductId)))
                {
                    context.WatchedProducts.AddOrUpdate(watchedProduct);
                }
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
            context.QRs.RemoveRange(context.QRs);
            context.SaveChanges();
            context.ExistingProducts.RemoveRange(context.ExistingProducts);
            context.SaveChanges();
            for (int i = 0; i < existingProductCount; i++)
            {
                ExistingProduct existingProduct = new ExistingProduct
                {
                    ExpirationDate = new DateTime(random.Next(2018, 2030), random.Next(12) + 1, random.Next(25) + 1),
                    ProductId = context.Products.ToList()[random.Next(context.Products.Count())].Id
                };
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

