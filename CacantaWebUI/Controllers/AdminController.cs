using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cacanta.WebUI.Repository.Abstract;
using Cacanta.WebUI.Models;
using Cacanta.WebUI.Entity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Cacanta.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IUnitOfWork unitofWork;

        public AdminController(IUnitOfWork _unitofWork)
        {
            unitofWork = _unitofWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var entity = unitofWork.Categories.GetAll()
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Product)
                                .Where(i => i.CategoryId == id)
                                .Select(i => new AdminEditCategoryModel()
                                {
                                    CategoryId = i.CategoryId,
                                    CategoryName = i.CategoryName,
                                    Products = i.ProductCategories.Select(a => new AdminEditCategoryProduct()
                                    {
                                        ProductId = a.ProductId,
                                        ProductName = a.Product.ProductName,
                                        Image = a.Product.Image,
                                        IsApproved = a.Product.IsApproved,
                                        IsFeatured = a.Product.IsFeatured,
                                        IsHome = a.Product.IsHome
                                    }).ToList()
                                }).FirstOrDefault();

            return View(entity);
        }

        [HttpPost]
        public IActionResult EditCategory(Category entity)
        {
            if (ModelState.IsValid)
            {
                unitofWork.Categories.Edit(entity);
                unitofWork.SaveChanges();

                return RedirectToAction("CatalogList");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult RemoveProduct(int id)
        {
            if (ModelState.IsValid)
            {

                unitofWork.Products.RemoveProduct(id);
                unitofWork.SaveChanges();
                return RedirectToAction("CatalogList");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var entity = unitofWork.Categories.GetAll()
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Product)
                                .Where(i => i.CategoryId == id)
                                .Select(i => new AdminEditCategoryModel()
                                {
                                    CategoryId = i.CategoryId,
                                    CategoryName = i.CategoryName,
                                    Products = i.ProductCategories.Select(a => new AdminEditCategoryProduct()
                                    {
                                        ProductId = a.ProductId,
                                        ProductName = a.Product.ProductName,
                                        Image = a.Product.Image,
                                        IsApproved = a.Product.IsApproved,
                                        IsFeatured = a.Product.IsFeatured,
                                        IsHome = a.Product.IsHome
                                    }).ToList()
                                }).FirstOrDefault();

            return View(entity);
        }

        [HttpPost]
        public IActionResult EditProduct(Category entity)
        {
            if (ModelState.IsValid)
            {
                unitofWork.Categories.Edit(entity);
                unitofWork.SaveChanges();

                return RedirectToAction("CatalogList");
            }

            return View("Error");
        }

        [HttpGet]
        //Kategoriden ürün silme
        public IActionResult RemoveFromCategory(int ProductId, int CategoryId)
        {
            if (ModelState.IsValid)
            {

                unitofWork.Categories.RemoveFromCategory(ProductId, CategoryId);
                unitofWork.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        //Kategori silme
        public IActionResult RemoveCategory(int CategoryId)
        {
            
                unitofWork.Categories.RemoveCategory(CategoryId);
                unitofWork.SaveChanges();
                return RedirectToAction("CatalogList");
        }

        //Kategorileri listeleme
        public IActionResult CatalogList()
        {
            var model = new CatalogListModel()
            {
                Categories = unitofWork.Categories.GetAll().ToList(),
                Products = unitofWork.Products.GetAll().ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Yeni kategori ekleme
        public IActionResult AddCategory(Category entity)
        {
            if (ModelState.IsValid)
            {
                unitofWork.Categories.Add(entity);
                unitofWork.SaveChanges();

                return View(entity);
            }
            return BadRequest();
        }

        [HttpGet]
        //Yeni kategori ekleme
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(Product entity, IFormFile file, List<string> _selectedCategories)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", file.FileName);
                    var path_tn = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products\\tn", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        entity.Image = file.FileName;
                    }

                    using (var stream = new FileStream(path_tn, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                
                foreach (var item in _selectedCategories)
                {
                    
                    Category category= unitofWork.Categories.GetByName(item.ToString());
                    ProductCategory _productcategory = new ProductCategory()
                    {
                        Product = entity,
                        Category = category
                    };
                    entity.ProductCategories.Add(_productcategory);
                }
                entity.DateAdded = DateTime.Now;
                unitofWork.Products.Add(entity);
                unitofWork.SaveChanges();
                return RedirectToAction("CatalogList");
            }

            return View(entity);
        }
    }
}