using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Core.Author;

namespace Bx_Web.Areas.Admin.Controllers
{
     [AdminLoggedAuthor]
    public class ProductController : Controller
    {
        //
        // GET: /Admin/Product/
        public ActionResult List()
        {
            return View();
        }
	}
}