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
        public DateTime DateTime { get; set; }

        public static EmployeeReportViewModel CreateViewModel(Company company, DateTime dateTime)
        {
            var vm = new EmployeeReportViewModel()
            {
                CompanyName = company.Name,
                DateTime = dateTime
            };

            return vm;
        }
    }
}