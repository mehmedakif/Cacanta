using Cacanta.WebUI.Infrastructure;
using Cacanta.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cacanta.WebUI.Components
{
    public class CartSumViewComponent:ViewComponent
    {
        public string Invoke()
        {
            return HttpContext.Session.GetJSon<Cart>("Cart")?.Products.Count().ToString() ?? "0";
        }
    }
}
