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
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using Codaxy.WkHtmlToPdf;
using SOPS.Models;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace SOPS.Controllers
{
    [RoutePrefix("api/Document")]
    public class DocumentController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Get: api/Document/report
        /// <summary>
        /// wygeneruj raport pdf
        /// potrzebna autoryzacja
        /// </summary>
        /// <param name="id">id firmy</param>
        /// <returns></returns>
        [Route("report/{id}")]
        public HttpResponseMessage GetReport(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var plotController = new PlotController();
            plotController.ControllerContext = ControllerContext;

            plotController.GetCompanyStatistics(id, )

            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var relativeUrl = Url.Route("Document_default",
                new { id, controller = "Default", action = "EmployeeReport" });

            var ms = new MemoryStream();
            PdfConvert.ConvertHtmlToPdf(new PdfDocument
            {
                Url = new Uri(new Uri(baseUrl), relativeUrl).ToString()

            }, new PdfOutput
            {
                OutputStream = ms
            });
            ms.Position = 0;

            var response = Request.CreateResponse();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Document.pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }

        // GET: api/Document/5
        /// <summary>
        /// wygeneruj dokument existingproduct pdf
        /// potrzebna autoryzacja
        /// </summary>
        /// <param name="id">id existingproduct</param>
        /// <returns></returns>
        public HttpResponseMessage GetDocument(int id)
        {
            ExistingProduct existingProduct = db.ExistingProducts.Find(id);
            if (existingProduct == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }


            var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var relativeUrl = Url.Route("Document_default",
                new { id, controller="Default", action="Details" });

            var ms = new MemoryStream();
            PdfConvert.ConvertHtmlToPdf(new PdfDocument
            {
                Url = new Uri(new Uri(baseUrl), relativeUrl).ToString()

            }, new PdfOutput
            {
                OutputStream = ms
            });
            ms.Position = 0;

            var response = Request.CreateResponse();
            response.Content = new StreamContent(ms);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Document.pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

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