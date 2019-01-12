using SOPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Areas.Document.ViewModels
{
    public class EmployeeReportViewModel
    {
        public string CompanyName { get; set; }
        public DateTime DateTime { get; set; }
        public int CompanyReportNumber { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ProductCount), ResourceType = typeof(Resources.EmployeeReport))]
        public int ProductCount { get; set; }
        public int ProductCountLastMonthDifference { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ProductAveragePrice), ResourceType = typeof(Resources.EmployeeReport))]
        public int ProductAveragePrice { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ExistingProductCount), ResourceType = typeof(Resources.EmployeeReport))]
        public int ExistingProductCount { get; set; }
        public int ExistingProductCountMonthDifference { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ExistingProductSumPrice), ResourceType = typeof(Resources.EmployeeReport))]
        public int ExistingProductSumPrice { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.EmploteeCount), ResourceType = typeof(Resources.EmployeeReport))]
        public int EmployeeCount { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.EmployeeCountYearChange), ResourceType = typeof(Resources.EmployeeReport))]
        public int EmployeeCountYearChange { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.CommentCount), ResourceType = typeof(Resources.EmployeeReport))]
        public int CommentCount { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ProductMostCommented), ResourceType = typeof(Resources.EmployeeReport))]
        public string ProductMostCommented { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.RatingCount), ResourceType = typeof(Resources.EmployeeReport))]
        public int RatingCount { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ProductBestRated), ResourceType = typeof(Resources.EmployeeReport))]
        public string ProductBestRated { get; set; }

        [Display(Name = nameof(Resources.EmployeeReport.ProductAverageRating), ResourceType = typeof(Resources.EmployeeReport))]
        public string ProductAverageRating { get; set; }

        public string ChartLocation { get; set; }

        public static EmployeeReportViewModel CreateViewModel(Company company, DateTime dateTime)
        {
            var products = company.Products;

            var companyExistingProducts = new List<ExistingProduct>();
            foreach (var product in company.Products)
            {
                companyExistingProducts.AddRange(product.ExistingProducts);
            }

            float productAvgRating = 0;
            int ratingCount = 0;
            float bestRate = 0;
            int bestRated = 0;
            var productRatings = new List<ProductRating>();
            foreach (var product in company.Products)
            {
                productRatings.AddRange(product.ProductRatings);
                ratingCount += product.ProductRatings.Count();
            }

            foreach (var rating in productRatings)
            {
                if (rating.Rating > bestRate)
                {
                    bestRate = rating.Rating;
                    bestRated = rating.ProductId;
                }
                productAvgRating += rating.Rating;
            }
            productAvgRating /= productRatings.Count();

            string productAvgRatingStr = productAvgRating.ToString();
            if (productAvgRatingStr.Length > 3)
            {
                productAvgRatingStr = productAvgRatingStr.Substring(0, 3);
            }

            string bestRatedStr = "";
            if (company.Products.Any())
            {
                var bestRatedProduct = company.Products.Find(p => p.Id == bestRated);
                if (bestRatedProduct != null)
                {
                    bestRatedStr = bestRatedProduct.Name;
                }
            }

            //var productComments = new List<ProductComment>();
            var maxComments = 0;
            var mostCommented = "";
            var totalComments = 0;
            foreach (var product in company.Products)
            {
                if (product.ProductComments.Count() > maxComments)
                {
                    maxComments = product.ProductComments.Count();
                    mostCommented = product.Name;
                }
                totalComments += product.ProductComments.Count();
            }

            var productCount = products.Count;
            var productCountDifference = productCount - products.Where(p => p.CreationDate < dateTime.AddMonths(-1)).Count();
            var productAveragePrice = 0;
            if (productCount != 0)
            {
                productAveragePrice = (int)products.Sum(p => p.SuggestedPrice) / productCount;
            }

            var existingProductDifference = companyExistingProducts.Count() - companyExistingProducts.Where(cp => cp.CreationDate < dateTime.AddMonths(-1)).Count();

            decimal sumPrice = 0;
            foreach(var existinProduct in companyExistingProducts)
            {
                sumPrice += company.Products.Find(cp => cp.Id == existinProduct.ProductId).SuggestedPrice;
            }

            var employeeYearChange = company.Employees.Count() - company.Employees.Where(e => e.JoinDate < dateTime.AddYears(-1)).Count();


            var vm = new EmployeeReportViewModel()
            {
                CompanyName = company.Name,
                DateTime = dateTime,
                CompanyReportNumber = company.CompanyReports.Count() + 1,
                ProductCount = products.Count,
                ProductCountLastMonthDifference = productCountDifference,
                ProductAveragePrice = productAveragePrice,
                ExistingProductCount = companyExistingProducts.Count(),
                ExistingProductCountMonthDifference = existingProductDifference,
                ExistingProductSumPrice = (int)sumPrice,
                EmployeeCount = company.Employees.Count(),
                EmployeeCountYearChange = employeeYearChange,
                CommentCount = totalComments,
                ProductMostCommented = mostCommented,
                ProductAverageRating = "",//productAvgRatingStr,
                ProductBestRated = "",//bestRatedStr,
                RatingCount = ratingCount,
                ChartLocation = System.AppContext.BaseDirectory + "Areas\\Document\\Views\\Default\\Chart.jpg"
            };

            return vm;
        }
    }
}