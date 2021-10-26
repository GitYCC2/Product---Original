using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Producks.Data;
using System.Web;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Producks.Undercutter;
using Producks.Web.ViewModels;
using Producks.Repo;
using Producks.Repo.ViewModels;


namespace Producks.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreDb _context;
        private IEnumerable<string> repoBrands;
        private IEnumerable<string> repoCategories;

        public StoreController(StoreDb context)
        {
            _context = context;
            RepoApi rp = new RepoApi(context);
            repoBrands = rp.GetBrandsName();
            repoCategories = rp.GetCategoriesName();
        }

        public ActionResult Index(string CatName = null, string BrandName = null)
        {
            ViewBag.Categories = new SelectList(repoCategories);
            ViewBag.Brands = new SelectList(repoBrands);

            RepoApi rp = new RepoApi(_context);
            var products = rp.GetAllProducts();

            if(CatName != null && CatName != "")
            {
                products = products.Where(p => p.Category == CatName);
            }

            if (BrandName != null && BrandName != "")
            {
                products = products.Where(p => p.Brand == BrandName);
            }

            return View(products);
            //return View(_context.Categories.Where(c => _context.Products.FirstOrDefault(s => s.CategoryId == c. Id && s.Active) != null).ToList());
        }

        public ActionResult Display(int? id)
        {
            if(id == null)
            {
                return new BadRequestResult();
            }

            Producks.Data.Category category = _context.Categories.Find(id);

            if(category == null)
            {
                return new NotFoundResult();
            }

            var products = _context.Products.Where(s => s.CategoryId == category.Id && s.Active).ToList();

            return View(products);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
