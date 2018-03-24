using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Web.Models;
using Bx_Core;
using Bx_Common;
using Bx_Core.ViewModels;
using Bx_Core.Author;

namespace Bx_Web.Areas.Admin.Controllers
{
     [AdminLoggedAuthor]
    public class BxAccountController : Controller
    {
        BxAccountManager _bxaccountmanager = new BxAccountManager();

        //
        // GET: /Bx/Account/
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult list(string search, int limit, string sortname, int pageNumber, string order)
        {
            Paging<BxAccount> _pagingCustomer = new Paging<BxAccount>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            var _paging = _bxaccountmanager.FindPageList(_pagingCustomer, search, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
        }

      

    

        


        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(BxAccountViewModel newbxaccount)
        {

            Response _resp = new Response();
            if (ModelState.IsValid)
            {
                if (_bxaccountmanager.HasAccounts(newbxaccount.username))
                {
                    _resp.Status = 0;
                    _resp.Message = "保险公司帐号已存在";
                }
                else if (_bxaccountmanager.HasPhone(newbxaccount.Phone))
                {
                    _resp.Status = 0;
                    _resp.Message = "保险公司手机号码已存在";
                }
                else
                {
                    BxAccount _newbxaccount = new BxAccount();
                    _newbxaccount.username = newbxaccount.username;
                    _newbxaccount.active = true;
                    _newbxaccount.Address = newbxaccount.Address;
                    _newbxaccount.Contacts = newbxaccount.Contacts;
                    _newbxaccount.CorporateName = newbxaccount.CorporateName;
                    _newbxaccount.CreateTime = DateTime.Now;
                    _newbxaccount.Email = newbxaccount.Email;
                    _newbxaccount.LoginIP = "";
                    _newbxaccount.LoginTime = DateTime.Now;
                    _newbxaccount.LoginCount = 0;
                    //_cust.parent_id = 0;
                    _newbxaccount.password =Security.SHA256(newbxaccount.password);
                    _newbxaccount.Phone = newbxaccount.Phone;

                    _resp = _bxaccountmanager.Add(_newbxaccount);
                    if (_resp.Status == 0)
                    {
                       
                    }
                    else
                    {
                        _resp.Message = "恭喜您！账号添加成功！";
 
                    }

                    return Json(_resp);
                    //_resp.Url = Url.Action("login");
                  
                }
            }
            else
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
            }
            return Json(_resp);
        }


        public ActionResult Edit(int id=0)
        {
            //查找是否有存在
            Response _resp = new Response();
            BxAccount _bxaccount = _bxaccountmanager.Find(id);
            if(_bxaccount==null)
            {
                _resp.Status = 0;
                _resp.Message = "ID 出错，不存在相应的保险公司账号";
                return Json(_resp,JsonRequestBehavior.AllowGet);
            }
            return View(_bxaccount);
 
        }

        [HttpPost]
        public ActionResult Edit( FormCollection fcs,int id=0)
        {
            Response _resp = new Response();




            BxAccount _bxaccount = _bxaccountmanager.Find(id);
            if (_bxaccount == null)
            {
                _resp.Status = 0;
                _resp.Message = "错误！保险公司账号不存在";
                return Json(_resp);

            }
            TryUpdateModel(_bxaccount, new string[] { "Contacts", "Email",  "Address",  "Phone" });


            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);

            }
            _resp = _bxaccountmanager.Update(_bxaccount);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！修改保险公司账号数据成功";
            }


            return Json(_resp);

        }


        public ActionResult Detail(int id=0)
        {
            BxAccount _bxaccount = _bxaccountmanager.Find(id);
            if (_bxaccount == null)
            { //id的终端不存在
                Prompt _prm = new Prompt();
                _prm.Title = "查询详情出错!";
                _prm.Message = "出错啦!不存在对应该ID的保险公司账号";
                return View("ErrorPop", _prm);

            }
            return View(_bxaccount);


        }


        /// <summary>
        /// 删除保险公司账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Response _resp = new Response();
            if (id < 1)
            {
                _resp.Status = 0;
                _resp.Message = "错误！参数不正确.";
                return Json(_resp);
            }
            _resp = _bxaccountmanager.Delete(id);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！删除保险公司账号成功.";
            }
            return Json(_resp);

        }

        [HttpPost]
        public ActionResult Forbid(int id=0)
        {
            Response _resp = new Response();
            if (id < 1)
            {
                _resp.Status = 0;
                _resp.Message = "错误！参数不正确.";
                return Json(_resp);
            }
            BxAccount _bxaccount = _bxaccountmanager.Find(id);
            if (_bxaccount == null)
            {
                _resp.Status = 0;
                _resp.Message = "错误！保险公司账号不存在.";
                return Json(_resp);

            }
            _bxaccount.active = !_bxaccount.active;
            _resp = _bxaccountmanager.Update(_bxaccount);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！更改保险公司账号状态已成功";
            }


            return Json(_resp);
        }


        public JsonResult ExitsAcoount(string username)
        {
            if (_bxaccountmanager.HasAccounts(username))
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("登录账号已存在");
            }
        }

        public JsonResult ExitsPhone(string phone)
        {
            if (_bxaccountmanager.HasPhone(phone))
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("手机号码已存在");
            }
        }


        public ActionResult Changepwd(int id=0)
        {
            //查找是否有存在
            Response _resp = new Response();
            BxAccount _bxaccount = _bxaccountmanager.Find(id);
            if (_bxaccount == null)
            {
                _resp.Status = 0;
                _resp.Message = "ID 出错，不存在相应的保险公司账号";
                return Json(_resp,JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Changepwd(BxAccountChangePassWordViewModel newchangepwd,int id=0)
        {
            Response _resp=new Response();
            if(!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);
            }
            _resp = _bxaccountmanager.ChangePassword(id,Security.SHA256( newchangepwd.Newpwd));
            return Json(_resp);
        }
    }
}