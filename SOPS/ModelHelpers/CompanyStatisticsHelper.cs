using SOPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOPS.ModelHelpers
{
    public static class CompanyStatisticsHelper
    {
        private static DateTime GetCurrentDate()
        {
            return DateTime.Now.Date;
        }
        public static void IncrementRegistredProducts(this ApplicationDbContext context, int companyId)
        {
            var currentDate = GetCurrentDate();
            var currentStatistics = context.CompanyStatistics.Find(currentDate, companyId);
            if (currentStatistics == null)
            {
                var companyStatistics = new CompanyStatistics()
                {
                    Date = currentDate,
                    CompanyId = companyId,
                    RegistredProducts = 1
                };
                currentStatistics = context.CompanyStatistics.Add(companyStatistics);
            }
            else
            {
                currentStatistics.RegistredProducts += 1;               
            }
        }
    }
}