using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Core;
using Bx_Common;
using Bx_Core.Author;

namespace Bx_Web.Areas.Bx.Controllers
{
    [BxLoggedAuthor]
    public class BxIndustryController : Controller
    {
        private BxIndustryManager _bxhygl = new BxIndustryManager();
     

        /// <summary>
        /// 添加保险行业_show
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 添加保险行业_save
        /// </summary>
        /// <param name="bxi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(BxIndustry bxi)
        {
            Response _resp = new Response();
            if(!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);
            }
            BxIndustry _bxi = new BxIndustry();
            _bxi.BxIndustryName = bxi.BxIndustryName;
            _bxi.BxRemark = bxi.BxRemark;
            _bxi.CreateTime = DateTime.Now;
            _resp=_bxhygl.Add(_bxi);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！添加保险行业成功！";
            }

            return Json(_resp);
        }

        /// <summary>
        /// 编辑保险行业_show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            Response _resp = new Response();
            BxIndustry _bxi = _bxhygl.Find(id);
            if (_bxi==null)
            {
                _resp.Status = 0;
                _resp.Message = "未找到相关的保险行业记录";
                return Json(_resp);

            }
            return View(_bxi);
        }

        /// <summary>
        /// 编辑保险行业_save
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int id, FormCollection fm)
        {
            Response _resp = new Response();
            BxIndustry _bxi = _bxhygl.Find(id);
            if (_bxi == null)
            {
                _resp.Status = 0;
                _resp.Message = _resp.Message = "未找到相关的保险行业记录"; 
                return Json(_resp);
 
 
            }

            if (_bxhygl.ExitsName(fm["BxIndustryName"]))
                {
                    _resp.Status = 0;
                    _resp.Message = _resp.Message = "错误!存在保险行业重名的录记录";
                    return Json(_resp);
                }

            TryUpdateModel(_bxi, new string[] { "BxIndustryName", "BxRemark" });


            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);

            }
            
            _resp = _bxhygl.Update(_bxi);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！修改保险行业数据成功";
            }


            return Json(_resp);
        }

        public ActionResult Delete(int id)
        {
            Response _resp = new Response();
            _resp = _bxhygl.Delete(id);
            if(_resp.Status==1)
            {
                _resp.Message = "恭喜!删除保险行业成功";
            }
            return Json(_resp);
        }



        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public ActionResult List(string search, int limit, string sortname, int pageNumber, string order)
        {
            
            //Paging<BxIndustry> _pagingi = new Paging<BxIndustry>();
            int _inttotal = _bxhygl.FindList().Count();
            List<BxIndustry> result = _bxhygl.FindList().OrderByDescending(a=>a.CreateTime).Skip((pageNumber - 1) * limit).Take(limit).ToList();
            //_pagingi.TotalNumber = result.Count();
            //_pagingi.Items = result;


            return Json(new { total = _inttotal, rows = result });
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        
        {
            Response _resp = new Response();
            BxIndustry _bxi = _bxhygl.Find(id);
            if (_bxi == null)
            {
                _resp.Status = 0;
                _resp.Message = "未找到相关的保险行业记录";
                return Json(_resp);

            }
            return View(_bxi);
 
        }



        public ActionResult GetSelect()
        {
            List<BxIndustry> _bxilist = new List<BxIndustry>();
            _bxilist = _bxhygl.FindList().ToList<BxIndustry>();
            Response _res = new Response();
            var _result = from item in _bxilist
                          select new { text = item.BxIndustryName, value = item.BxIndustryId };
            if (_bxilist.Count == 0)
            {
                _res.Status = 0;
                _res.Message = "未找到保险行业列表";
                return Json(_res,JsonRequestBehavior.AllowGet);
            }
            else
            {
                _res.Status = 1;
                _res.Data = _result;
                return Json(_res,JsonRequestBehavior.AllowGet);
            }
           
        }

      
	}
}