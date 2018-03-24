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
    public class BxCorporateController : Controller
    {

        private BxCorporateManager _bxm = new BxCorporateManager();
        //
        // GET: /Admin/BxCorporate/
        public ActionResult List()
        {
            return View();
        }
        [HttpPost]
        public ActionResult list(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<BxCorporate> _pagingCustomer = new Paging<BxCorporate>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            var _paging = _bxm.FindPageList(_pagingCustomer, search, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(BxCorporate bxc)
        {
           return Json( _bxm.Add(bxc));
        }

        [HttpPost]
        public ActionResult Delete(int id=0)

        {
            return Json(_bxm.Delete(id));
        }
        public ActionResult Edit(int id=0)
        {
           BxCorporate _bxc= _bxm.Find(id);
           if (_bxc == null)
           {
               Response _resp = new Response();
               _resp.Status = 0;
               _resp.Message = "未找到相关的保险公司";
               return Json(_resp,JsonRequestBehavior.AllowGet);
           }
           return View(_bxc);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection fcs, int id = 0)
        {
            Response _resp = new Response();
            BxCorporate _bxc =_bxm.Find(id);
            if (_bxc == null)
            {
                _resp.Status = 0;
                _resp.Message = "要修改的保险公司不存在";

            }
            //判断是否有重名的

            if (_bxm.HasAccounts(fcs["BxCorporateName"].ToString()))
            {
                _resp.Status = 0;
                _resp.Message = "要修改的保险公司名称重复";

            }

            TryUpdateModel(_bxc, new string[] { "BxCorporateName", "BxCorRemark" });


            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);

            }
            _resp =_bxm.Update(_bxc);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！修改终端用户数据成功";
            }


            return Json(_resp);
        }

        
        public ActionResult Detail(int id=0)
        {
            Response _resp = new Response();
            _resp = _bxm.Detail(id);
            if (_resp.Status == 0)
            {
                return Json(_resp);
            }
            else
            {
                return View(_resp.Data);
            }
            
        }

        public ActionResult GetSelect()
        {
            List<BxCorporate> _bxilist = new List<BxCorporate>();
            _bxilist = _bxm.FindList().ToList<BxCorporate>();
            Response _res = new Response();
            var _result = from item in _bxilist
                          select new { text = item.BxCorporateName, value = item.BxCorId };
            if (_bxilist.Count == 0)
            {
                _res.Status = 0;
                _res.Message = "未找到保险公司列表";
                return Json(_res, JsonRequestBehavior.AllowGet);
            }
            else
            {
                _res.Status = 1;
                _res.Data = _result;
                return Json(_res, JsonRequestBehavior.AllowGet);
            }

        }
	}

}