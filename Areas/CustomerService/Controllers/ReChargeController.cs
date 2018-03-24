using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Core;
using Bx_Common;
using Bx_Core.Author;

namespace Bx_Web.Areas.CustomerService.Controllers
{
    [CustomerServicerAuthor]
    public class ReChargeController : Controller
    {
        //初
        ReChargeManager _reChargeManager = new ReChargeManager();
        CustomerManager _cusermoerManager = new CustomerManager();

        //获
        #region //获


        /// <summary>
        /// 查看余额
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReCharge(string userName)
        {
            decimal _decREsul = 0.00M;
            Response _resp = new Response();

            if (!_reChargeManager.ExiteIterm(userName))
            {
                return Json(_decREsul);


            }
            _resp = _reChargeManager.GetReCharge(userName);
            if (_resp.Status == 0)
            {
                return Json(_decREsul);
            }
            _decREsul = _resp.Data;


            return Json(_decREsul);
        }

        #endregion




        //列 
        #region //列

        public ActionResult GetDetail(int id)
        {
            Customer _customer = _cusermoerManager.Find(id);
            string userNamer = null;
            if (null == _customer)
            {
                userNamer = "";

            }
            else
            {
                userNamer = _customer.username;
            }



            ViewBag.userNamer = userNamer;

            return View();

        }

        /// <summary>
        /// 列出某个账号的充值记录
        /// </summary>
        /// <param name="search"></param>
        /// <param name="limit"></param>
        /// <param name="sortname"></param>
        /// <param name="pageNumber"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDetail(string search, int limit, string sortname, int pageNumber, string order, string userName)
        {
            Paging<ReCharge> _pagingCustomer = new Paging<ReCharge>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;


            var _paging = _reChargeManager.FindPageList(_pagingCustomer, userName, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

        }

        #endregion
	}
}