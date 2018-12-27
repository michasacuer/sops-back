﻿using System;
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
    }
}