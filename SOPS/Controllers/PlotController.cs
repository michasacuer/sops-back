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
    public class PlotController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Plot/5
        [Authorize]
        public HttpResponseMessage GetCompanyStatistics(int companyId)
        {
            var company = db.Companies.Find(companyId);
            if (company == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (!db.IsCurrentUserEmployedInCompanyOrAdministrator(companyId))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var minValue = DateTime.Now.AddDays(-5);
            var maxValue = DateTime.Now;

            var statistics = db.CompanyStatistics.Where(s => s.CompanyId == companyId && s.Date > minValue && s.Date <= maxValue);

            var model = new PlotModel { Title = company.Name };
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(statistics.Min(s =>s.Date).Date),
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