using SOPS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOPS.Areas.Document.ViewModels
{
    public class DocumentViewModel
    {
        [Display(Name = nameof(Resources.Product.ProductId), ResourceType = typeof(Resources.Product))]
        public int ProductId { get; set; }

        [Display(Name = nameof(Resources.Product.ExistingProductId), ResourceType = typeof(Resources.Product))]
        public int ExistingProductId { get; set; }

        [Display(Name = nameof(Resources.Product.ProductName), ResourceType = typeof(Resources.Product))]
        public string ProductName { get; set; }

        [Display(Name = nameof(Resources.Product.ProductDescription), ResourceType = typeof(Resources.Product))]
        public string ProductDescription { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyName), ResourceType = typeof(Resources.Product))]
        public string CompanyName { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyAddressStreet), ResourceType = typeof(Resources.Product))]
        public string CompanyAddressStreet { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyAddressZipCode), ResourceType = typeof(Resources.Product))]
        public string CompanyAddressZipCode { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyEmail), ResourceType = typeof(Resources.Product))]
        public string CompanyEmail { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyNIP), ResourceType = typeof(Resources.Product))]
        public string CompanyNIP { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyREGON), ResourceType = typeof(Resources.Product))]
        public string CompanyREGON { get; set; }

        [Display(Name = nameof(Resources.Product.ProductExpirationDate), ResourceType = typeof(Resources.Product))]
        [DataType(DataType.Date)]
        public DateTime ProductExpirationDate { get; set; }

        [Display(Name = nameof(Resources.Product.CompanyCity), ResourceType = typeof(Resources.Product))]
        public string CompanyAddressCity { get; set; }

        [Display(Name = nameof(Resources.Product.ProductCountryOfOrigin), ResourceType = typeof(Resources.Product))]
        public string ProductCountryOfOrigin { get; set; }

        [Display(Name = nameof(Resources.Product.SuggestedPrice), ResourceType = typeof(Resources.Product))]
        public decimal SuggestedPrice { get; set; }

        public static DocumentViewModel CreateViewModel(ExistingProduct existingProduct)
        {
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
    }
}