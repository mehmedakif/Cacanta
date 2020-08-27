using Cacanta.WebUI.Entity;
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
        public Category GetByName(string name)
        {
            return CacantaContext.Categories
                .Where(i => i.CategoryName == name)
                .FirstOrDefault();
        }
    }
}
