using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SOPS.Areas.Document.ViewModels;
using SOPS.Models;

namespace SOPS.Areas.Document.Controllers
{
    public class DefaultController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Document/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExistingProduct existingProduct = db.ExistingProducts.Find(id);
            if (existingProduct == null)
            {
                return HttpNotFound();
            }
            db.Entry(existingProduct).Reference(e => e.Product).Load();
            db.Entry(existingProduct.Product).Reference(p => p.Company).Load();

            var vm = DocumentViewModel.CreateViewModel(existingProduct);

            ViewBag.QR_URL = Url.Content("~/api/QR/" + id);
            //ViewBag.PlotURL = Url.Content("~/api/Plot?CompanyId=" + id);

            return View(vm);
        }

        // GET: Document/EmployeeReport/{id}
        public ActionResult EmployeeReport(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company= db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            // db.Entry(existingProduct).Reference(e => e.Product).Load();
            // db.Entry(existingProduct.Product).Reference(p => p.Company).Load();

            var vm = EmployeeReportViewModel.CreateViewModel(company, DateTime.Now);

            return View(vm);
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
