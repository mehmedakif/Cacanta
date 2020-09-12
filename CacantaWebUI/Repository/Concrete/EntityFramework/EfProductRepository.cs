using Cacanta.WebUI.Repository.Abstract;
using Cacanta.WebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cacanta.WebUI.Repository.Concrete.EntityFramework
{
    public class EfProductRepository : EfGenericRepository<Product>, IProductRepository
    {
        public EfProductRepository(CacantaContext context) : base(context)
        {
        }
        public CacantaContext CacantaContext
        {
            get { return context as CacantaContext; }
        }

        public IQueryable<Product> Products { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void RemoveProduct(int productid)
        {
            Product ctg = CacantaContext.Products
            .Where(i => i.ProductId == productid)
            .FirstOrDefault();
            CacantaContext.Remove(ctg);
        }
    }
}
