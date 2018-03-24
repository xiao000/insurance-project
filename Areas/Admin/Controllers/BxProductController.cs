using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Common;
using Bx_Core;
using Bx_Core.Author;

namespace Bx_Web.Areas.Admin.Controllers
{
     [AdminLoggedAuthor]
    public class BxProductController : Controller
    {
        ProductManger _productmanager = new ProductManger();
        BxIndustryManager _bxindustrymanager = new BxIndustryManager();
        BxTypeManager _bxtypemanager = new BxTypeManager();
        ProductLogManager _productLogManager = new ProductLogManager();


        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public ActionResult List(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<Product> _pagingCustomer = new Paging<Product>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            string listwhat = "list_except_unpub";
            var _paging = _productmanager.FindPageList(_pagingCustomer,  search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
        }

        public ActionResult List_PubErr(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<Product> _pagingCustomer = new Paging<Product>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            string listwhat = "List_PubErr";
            var _paging = _productmanager.FindPageList(_pagingCustomer,  search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

        }


        /// <summary>
        /// 列出正在审核中的保险合同
        /// </summary>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="sortname"></param>
        /// <param name="pageNumber"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult List_Waitpub(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<Product> _pagingCustomer = new Paging<Product>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            string listwhat = "List_Waitpub";
            var _paging = _productmanager.FindPageList(_pagingCustomer,  search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

        }


        /// <summary>
        /// 列出已经上架的
        /// </summary>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="sortname"></param>
        /// <param name="pageNumber"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult List_Publiced(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<Product> _pagingCustomer = new Paging<Product>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            string listwhat = "List_Publiced";
            var _paging = _productmanager.FindPageList(_pagingCustomer,  search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

        }


        /// <summary>
        /// 列出已被禁用的
        /// </summary>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="sortname"></param>
        /// <param name="pageNumber"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult List_Forbid(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<Product> _pagingCustomer = new Paging<Product>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            string listwhat = "List_Forbid";
            var _paging = _productmanager.FindPageList(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

        }

        [HttpPost]
        public ActionResult Count()
        {
            Response _resp = new Response();
            _resp = _productmanager.CountBxproduct();
            return Json(_resp);
        }
        [HttpPost]
        public ActionResult PubErr(FormCollection fc, int id = 0)
        {
            Response _resp = new Response();
            //判断是否填写否决原因

            UnPubReason _unpubreason = new UnPubReason();
            if (!TryUpdateModel(_unpubreason, new string[] { "UnPUbReason" }))
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                _resp.Data = null;
                return Json(_resp);

            }
            _resp = _productmanager.PubErr(_unpubreason, id);
            if (_resp.Status == 1)
            {
                //否决日志
                _productLogManager.AddUnPublicLog(Session["username"].ToString(), "否决保险产品" + id.ToString() + "; 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                _resp.Message = "恭喜！保险合同上架否决成功！";
            }
            return Json(_resp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Publiced(FormCollection fc, ICollection<Bxar> bxar, int productid=0)
        {
            Response _resp = new Response();
            List<Bxar> _bxars = new List<Bxar>();
            if (!TryUpdateModel(_bxars, fc))
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                _resp.Data = null;
                return Json(_resp);

            }
            Product _product=_productmanager.Find(productid);
            if (_product == null)
            {
                _resp.Status = 0;
                _resp.Message = "不存在该ID的保险合同";
                return Json(_resp);
            }
            _resp = _productmanager.Publiced(_bxars, productid);
            if (_resp.Status == 1)
            {
                //上架日志
                _productLogManager.AddPublicedLog(Session["username"].ToString(), "上架保险产品" + productid.ToString() + "; 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                
            }

            return Json(_resp);
 
        }

        [HttpPost]
        public ActionResult ForbidPub(int id = 0)
        {
            Response _resp = new Response();
            _resp = _productmanager.Forbid(id);
            if (_resp.Status == 1)
            {
                //禁用日志
                _productLogManager.AddPublicedLog(Session["username"].ToString(), "禁用保险产品" + id.ToString() + "; 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                _resp.Message = "恭喜,保险合同禁用成功";
            }
            return Json(_resp);
        }

        
        public ActionResult Detail(int id=0)
        {
            Response _resp = new Response();
            Product _product = _productmanager.Find(id);


            if (_product==null)
            {
                _resp.Status = 0;
                _resp.Message = "错误!未找到相关的保险产品合同";
                return Json(_resp, JsonRequestBehavior.AllowGet);

            }

            //_resp.data下是PRODUCT类数据
            //获取费率

            
            ViewBag.Bxars = _productmanager.GetBxarList(_product.ProductId);
            ViewBag.BxIndustry = new SelectList(_bxindustrymanager.GetSelect().Select(a => a.BxIndustryName).ToArray());
            ViewBag.BxType = new SelectList(_bxtypemanager.GetSelect().Select(a => a.BxTypeName).ToArray());
            ViewBag.UnPubReason = _productmanager.GetUnPubReason(_product.ProductId);
            
            return View(_product);
        }
	}
}