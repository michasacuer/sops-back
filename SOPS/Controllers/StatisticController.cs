using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/Statistic")]
    public class StatisticController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Zwraca liczność produktów i firm
        /// </summary>
        /// <returns></returns>tam w d
        [HttpGet]
        [Route("getallcount")]
        [ResponseType(typeof(StatisticGetAllCountViewModel))]
        public IHttpActionResult GetAllCount()
        {
            return Ok(new StatisticGetAllCountViewModel
            {
                CompaniesCount = db.Companies.Count(),
                AllProductsCount = db.Products.Count()
            });
        }

        /// <summary>
        /// Zwraca liste produktów i firm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getalllists")]
        [ResponseType(typeof(StatisticGetAllListViewModel))]
        public IHttpActionResult GetAllList()
        {
            return Ok(new StatisticGetAllListViewModel
            {
                Companies = db.Companies.ToList(),
                Products = db.Products.ToList()
            });
        }

        // GET: api/Statistic/LastMonthCount
        /// <summary>
        /// Zwraca liczbę firm i produktów na początku ostatniego miesiąca
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LastMonthCount")]
        [ResponseType(typeof(Statistics))]
        public IHttpActionResult GetLastMonthCount()
        {
            var statistics = db.Statistics.First();
            return Ok(new Statistics {
                LastMonthCompanyCount = statistics.LastMonthCompanyCount,
                LastMonthProductCount = statistics.LastMonthProductCount
            });
        }
    }
}
