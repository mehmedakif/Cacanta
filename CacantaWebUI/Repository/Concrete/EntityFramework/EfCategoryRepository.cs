using Cacanta.WebUI.Entity;
using Cacanta.WebUI.Models;
using Cacanta.WebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cacanta.WebUI.Repository.Concrete.EntityFramework
{
    public class EfCategoryRepository : EfGenericRepository<Category>, ICategoryRepository
    {
        public EfCategoryRepository(CacantaContext context) : base(context)
        {
        }

        public CacantaContext CacantaContext
        {
            get { return context as CacantaContext; }
        }

        public IEnumerable<CategoryModel> GetAllWithProductCount()
        {
            return GetAll().Select(i => new CategoryModel()
            {
                CategoryId = i.CategoryId,
                CategoryName = i.CategoryName,
                Count = i.ProductCategories.Count()
            });
        }

        public Category GetByName(string name)
        {
            return CacantaContext.Categories
                .Where(i => i.CategoryName == name)
                .FirstOrDefault();
        }

        public void RemoveFromCategory(int productId, int categoryId)
        {
            Product product = CacantaContext.Products
                .Where(i => i.ProductId == productId)
                .FirstOrDefault();
                CacantaContext.Remove(product);
        }
        public void RemoveCategory(int _categoryId)
        {
            Category prd = CacantaContext.Categories
           .Where(i => i.CategoryId == _categoryId)
           .FirstOrDefault();
            CacantaContext.Remove(prd);
        }
    }
}
