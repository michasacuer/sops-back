using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SOPS.Attributes;
using SOPS.Models;

namespace SOPS.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompanyController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Company
        /// <summary>
        /// pobierz wszystkie firmy
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Company> GetCompanies()
        {
            return db.Companies.ToList();
        }

        //GET:api/Company/Products
        /// <summary>
        /// pobierz wszystkie firmy z produktami
        /// </summary>
        /// <returns></returns>
        [Route("Products")]
        public IEnumerable<Company> GetCompaniesWithProducts()
        {
            return db.Companies.Include(p => p.Products).ToList();
        }

        //GET: api/company/Profile?id=stringid
        [Route("Profile")]
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployeeCompany(string userId)
        {
            var employee = db.Employees.Include(e => e.Company.Products).SingleOrDefault(e => e.UserId == userId);

            return Ok(employee.Company);
        }

        // GET: api/Companies/5
        /// <summary>
        /// pobierz firme
        /// </summary>
        /// <param name="id">id firmy</param>
        /// <returns></returns>
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }


        // GET: api/Companies/Newest
        /// <summary>
        /// pobierz firmę która została dodana jako ostatnia
        /// </summary>
        /// <returns></returns>
        [Route("Newest")]
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetNewest()
        {
            var newestCompanyDate = db.Companies.Max(c => c.JoinDate);
            var newestCompany = db.Companies.First(c => c.JoinDate == newestCompanyDate);

            return Ok(newestCompany);
        }

        // PUT: api/Companies/5
        /// <summary>
        /// zmodyfikuj firme - trzeba dodac autoryzacje
        /// </summary>
        /// <param name="id">id firmy</param>
        /// <param name="company">nowe dane firmy</param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCompany(int id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != company.Id)
            {
                return BadRequest();
            }

            var local = db.Set<Company>().Local.FirstOrDefault(f => f.Id == company.Id);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }

            db.Entry(company).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Companies
        /// <summary>
        /// dodaj firme - potrzebna autoryzacja
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(Company))]
        public IHttpActionResult PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (company == null)
            {
                return BadRequest("company not specified in request body");
            }

            db.Companies.Add(company);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        /// <summary>
        /// usun firme - potrzebna autoryzacja
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(Company))]
        public IHttpActionResult DeleteCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            db.SaveChanges();

            return Ok(company);
        }

        // GET: api/Company/profile/5
        /// <summary>
        /// pobierz profil firmy - dane firmy+produkty i jej pracownicy
        /// potrzebna autoryzacja
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet]
        [Route("Profile/{id:int}")]
        [Authorize(Roles = "Employee, Administrator")]
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetCompanyProfile(int id)
        {
            var company = db.Companies.Find(id);

            if(company == null)
            {
                return NotFound();
            }

            db.Entry(company).Collection(c => c.Products).Load();
            // db.Entry(company).Collection(c => c.CompanyReports).Load();
            db.Entry(company).Collection(c => c.Employees).Load();

            return Ok(company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int id)
        {
            return db.Companies.Count(e => e.Id == id) > 0;
        }
    }
}