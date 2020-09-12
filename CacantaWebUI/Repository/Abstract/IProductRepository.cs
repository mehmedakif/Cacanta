using Cacanta.WebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cacanta.WebUI.Repository.Abstract
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void RemoveProduct(int productid);
    }
}
