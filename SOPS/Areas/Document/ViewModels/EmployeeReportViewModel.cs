using SOPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.Areas.Document.ViewModels
{
    public class EmployeeReportViewModel
    {
        public string CompanyName { get; set; }

        public static EmployeeReportViewModel CreateViewModel(Company company)
        {
            var vm = new EmployeeReportViewModel()
            {
                CompanyName = company.Name
            };

            return vm;
            /*
            var product = existingProduct.Product;
            var company = product.Company;

            var vm = new DocumentViewModel
            {
                ExistingProductId = existingProduct.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductExpirationDate = existingProduct.ExpirationDate,
                CompanyName = company.Name,
                CompanyAddressStreet = company.AddressStreet,
                CompanyAddressZipCode = company.AddressZipCode,
                CompanyEmail = company.Email,
                CompanyNIP = company.NIP,
                CompanyREGON = company.REGON,
                CompanyAddressCity = company.AddressCity,
                ProductCountryOfOrigin = product.CountryOfOrigin,
                SuggestedPrice = product.SuggestedPrice,
            };

            return vm;
        }
        */
        }
    }
}