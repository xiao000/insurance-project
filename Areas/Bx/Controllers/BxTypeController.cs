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
    public class BxTypeController : Controller
    {
        private BxTypeManager _bxtm = new BxTypeManager();



        /// <summary>
        /// 添加保险类型_show
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 添加保险类型_save
        /// </summary>
        /// <param name="bxi"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(BxType bxt)
        {
            Response _resp = new Response();
            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);
            }
            BxType _bxt = new BxType();
            _bxt.BxTypeName = bxt.BxTypeName;
            _bxt.BxRemark = bxt.BxRemark;
            _bxt.CreatDateTime = DateTime.Now;
            _resp = _bxtm.Add(_bxt);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！添加保险行业成功！";
            }

            return Json(_resp);
        }




        /// <summary>
        /// 编辑保险类型_show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            Response _resp = new Response();
            BxType _bxt = _bxtm.Find(id);
            if (_bxt == null)
            {
                _resp.Status = 0;
                _resp.Message = "未找到相关的保险类型记录";
                return Json(_resp);

            }
            return View(_bxt);
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
            BxType _bxt = _bxtm.Find(id);
            if (_bxt == null)
            {
                _resp.Status = 0;
                _resp.Message = _resp.Message = "未找到相关的保险类型记录";
                return Json(_resp);


            }
            if(_bxtm.ExitsName(fm["BxTypeName"]))
            {
                _resp.Status = 0;
                _resp.Message = _resp.Message = "错误!存在保险类型重名的记录";
                return Json(_resp);

            
            }

            TryUpdateModel(_bxt, new string[] { "BxTypeName", "BxRemark" });


            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);

            }
            _resp = _bxtm.Update(_bxt);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！修改保险类型数据成功";
            }


            return Json(_resp);
        }

        public ActionResult Delete(int id)
        {
            Response _resp = new Response();
            _resp = _bxtm.Delete(id);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜!删除保险类型成功";
            }
            return Json(_resp);
        }


        public ActionResult List()
        {
            return View();
        }



        [HttpPost]
        public ActionResult List(int limit,int pageNumber)
        {
            int _inttotal = _bxtm.FindList().Count();
            
            List<BxType> result = _bxtm.FindList().OrderByDescending(a=>a.CreatDateTime).Skip((pageNumber-1)*limit).Take(limit).ToList();






            return Json(new { total = _inttotal, rows = result });
        }


        public ActionResult Detail(int id)
        {
            Response _resp = new Response();
            BxType _bxt = _bxtm.Find(id);
            if (_bxt == null)
            {
                _resp.Status = 0;
                _resp.Message = "未找到相关的保险类型记录";
                return Json(_resp);

            }
            return View(_bxt);

        }

        public ActionResult GetSelect()
        {
            List<BxType> _bxilist = new List<BxType>();
            _bxilist = _bxtm.FindList().ToList<BxType>();
            Response _res = new Response();
            var _result = from item in _bxilist
                          select new { text = item.BxTypeName, value = item.BxTypeId };
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
