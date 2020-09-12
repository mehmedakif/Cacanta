using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cacanta.WebUI.Entity
{
    public class ProductCategory
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        //public ProductCategory(int _CategoryId, Category _Category, int _ProductId, Product _Product)
        //{
        //    CategoryId = _CategoryId;
        //    Category = _Category;
        //    ProductId = _ProductId;
        //    Product = _Product;
        //}
    }

}
