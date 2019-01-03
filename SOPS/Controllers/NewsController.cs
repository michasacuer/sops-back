using SOPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOPS.Controllers
{
    public class NewsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Random random = new Random();

        public IEnumerable<News> GetNews()
        {
            var news = new List<News>(20);

            var products = db.Products.OrderByDescending(p => p.CreationDate).Take(10);
            var companies = db.Companies;

            foreach (var product in products)
            {
                var productNews = new News
                {
                    Header = product.Name + " registered",
                    EventDate = product.CreationDate,
                    IconName = "layers",
                    Content = "Company " + companies.First(c => c.Id == product.CompanyId).Name + " has registered a product named " + product.Name + ". From now on the product is available to search, rate, and discuss. Our users can also look for that product details."
                };

                news.Add(productNews);
            }

            var newestCompanies = companies.OrderByDescending(c => c.JoinDate).Take(10);
            foreach(var company in newestCompanies)
            {
                var companyNews = new News
                {
                    Header = company.Name + " joined SPOS",
                    EventDate = company.JoinDate,
                    IconName = "business",
                    Content = "We are welcoming a new company known as " + company.Name + ". This company has been accepted as a member of this site and is now available of registering products"
                };

                news.Add(companyNews);
            }

            news = news.OrderByDescending(n => n.EventDate).Take(10).ToList();

            for(int i = 0; i < 3; ++i)
            {
                var newsToReplace = random.Next(2, 10);
                news[newsToReplace] = new News
                {
                    Header = "Article on some topic",
                    EventDate = news[newsToReplace].EventDate,
                    IconName = "note",
                    Content = "Some article placeholder text. The article is about some topic. It should be a short informative text. The information provided in the article should concern curret situation on the site"
                };
            }

            return news;
        }
    }
}
