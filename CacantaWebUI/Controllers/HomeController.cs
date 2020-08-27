using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Cacanta.WebUI.Entity;
using Cacanta.WebUI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CacantaWebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository repository;
        private IUnitOfWork uow;

        public HomeController(IUnitOfWork _uow, IProductRepository _repository)
        {
            repository = _repository;
            uow = _uow;
        }

        public IActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Products = uow.Products.GetAll();
            mymodel.Categories = uow.Categories.GetAll();
            return View(mymodel); 
        }

        public IActionResult Details(int id)
        {
            return View(repository.Get(id));
        }

        public IActionResult Create()
        {
            var prd = new Product() { ProductName = "Yeni Ürün", Price = 1000 };

            uow.Products.Add(prd);
            uow.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}
