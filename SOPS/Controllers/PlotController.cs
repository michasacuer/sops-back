using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SOPS;
using SOPS.ModelHelpers;
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/Plot")]
    public class PlotController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// wygeneruj wykres ze statystykami firmy
        /// </summary>
        /// <param name="companyId">id firmy</param>
        /// <param name="width">szerokosc wykresu</param>
        /// <param name="height">wysokosc wykresy</param>
        /// <returns></returns>
        [Route("Company")]
        [Authorize(Roles = "Administrator,Employee")]
        public HttpResponseMessage GetCompanyStatistics(int companyId, int width, int height)
        {
            if(width < 0 || width > 2000 || height < 0 || height > 2000)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var company = db.Companies.Find(companyId);
            if (company == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(companyId))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var minValue = DateTime.Now.AddDays(-20);
            var maxValue = DateTime.Now;

            var statistics = db.CompanyStatistics.Where(s => s.CompanyId == companyId && s.Date > minValue && s.Date <= maxValue);

            var model = new PlotModel { Title = company.Name };
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(statistics.Min(s => s.Date).Date),
                Maximum = DateTimeAxis.ToDouble(statistics.Max(s => s.Date).Date),
                StringFormat = "dd/MM/yyyy",
                Title = "Date"
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 11,
                Title = "Registred Products"
            });

            var series = new FunctionSeries();
            foreach (var statistic in statistics)
            {
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(statistic.Date), statistic.RegistredProducts));
            }
            model.Series.Add(series);

            var pngExporter = new OxyPlot.WindowsForms.PngExporter();
            pngExporter.Width = width;
            pngExporter.Height = height;
            var ms = new MemoryStream();
            pngExporter.Export(model, ms);
            ms.Position = 0;

            var response = Request.CreateResponse();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            return response;
        }

        /// <summary>
        /// wygneruj wykres statystyk produktu
        /// </summary>
        /// <param name="productId">id produktu</param>
        /// <param name="width">szerokosc wykresu</param>
        /// <param name="height">wysokosc wykresu</param>
        /// <returns></returns>
        [Route("Product")]
        [Authorize(Roles = "Administrator,Employee")]
        public HttpResponseMessage GetProductRatings(int productId, int width, int height)
        {
            if (width < 0 || width > 2000 || height < 0 || height > 2000)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(productId);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(product.CompanyId))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var minValue = DateTime.Now.AddDays(-20);
            var maxValue = DateTime.Now;

            var ratings = db.ProductRatings.Where(r => r.ProductId == productId && r.Date > minValue && r.Date <= maxValue).ToList();
            var avgs = ratings.GroupBy(r => r.Date.Date).OrderBy(r=>r.Key).Select(g => new
            {
                Day = g.Key,
                Avg = g.Average(r => r.Rating)
            });

            var model = new PlotModel { Title = product.Name };
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
                Title = "Date",
                Minimum = DateTimeAxis.ToDouble(avgs.Min(a => a.Day).Date),
                Maximum = DateTimeAxis.ToDouble(avgs.Max(a => a.Day).Date),
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 11,
                Title = "Average Rating"
            });

            var series = new FunctionSeries();
            foreach (var avg in avgs)
            {
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(avg.Day), (int)avg.Avg));
            }
            model.Series.Add(series);

            var pngExporter = new OxyPlot.WindowsForms.PngExporter();
            pngExporter.Width = width;
            pngExporter.Height = height;
            var ms = new MemoryStream();
            pngExporter.Export(model, ms);
            ms.Position = 0;

            var response = Request.CreateResponse();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}