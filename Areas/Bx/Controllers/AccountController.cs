using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Web.Models;
using Bx_Core;
using Bx_Common;
using System.Drawing;
using Bx_Core.Author;
using Bx_Core.ViewModels;

namespace Bx_Web.Areas.Bx.Controllers
{
    [BxLoggedAuthor]
    public class AccountController : Controller
    {
       private BxAccountManager _bxaccountmanager = new BxAccountManager();
       private LogInOutLogManager _logInOutManager = new LogInOutLogManager();

       

        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginvw)
        {
            Response _res = new Response();
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != loginvw.validatecode.ToUpper())
            
            {
                
                _res.Status = 0;
                _res.Message = "验证码不正确";
                //登录失败记录日志
                _logInOutManager.AddLogFail(loginvw.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                return Json(_res);
            }
            if (!ModelState.IsValid)
            {
                _res.Status = 0;
                _res.Message = General.GetModelErrorString(ModelState);
                return Json(_res);
            }
            else
            {

                string _passowrd = Security.SHA256(loginvw.password);
               //string _passowrd = (loginvw.password);

                _res = _bxaccountmanager.Verify(loginvw.username, _passowrd);
                if (_res.Status == 1)
                {
                    var _bxaccount = _bxaccountmanager.FindByAccount(loginvw.username);
                    //判断登录账号是否被禁用
                    if (!_bxaccount.active)
                    {
                        _res.Status = 0;
                        _res.Message = "账号被禁用";
                        return Json(_res);
                    }
                    Session.Add("BxAccountId", _bxaccount.BxAccountId);
                    Session.Add("username", _bxaccount.username);
                    Session.Add("Role", "BX");
                    Session.Add("BxCorporateName", _bxaccount.CorporateName);
                    _bxaccount.LoginTime = DateTime.Now;
                    _bxaccount.LoginIP = Request.UserHostAddress;
                    _bxaccount.LoginCount = _bxaccount.LoginCount + 1;
                    _bxaccountmanager.Update(_bxaccount);
                    _res.Url = Url.Action("List", "policy");//转到产品管理
                    //登录成功记录日志
                    _logInOutManager.AddLogIn(_bxaccount.username, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                    _res.Message = "恭喜您！登录成功！";

                    return Json(_res);
                }

            }


            //登录失败记录日志
            _logInOutManager.AddLogFail(loginvw.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
            return Json(_res);
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult VerificationCode()
        {
            string verificationCode = Security.CreateVerificationText(4);
            Bitmap _img = Picture.CreateVerificationImage(verificationCode, 160, 30);
            _img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            //Session["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }

        [HttpPost]
        public ActionResult Logout()
        {
            string strUsername = Session["username"].ToString();
            Session.Clear();
            Response _response = new Response();
            _response.Status = 1;
            _response.Url = Url.Action("Login", "Account", new {area="Bx" });
            _response.Message = "恭喜您！退出成功！";
            //成功退出记录日志
            _logInOutManager.AddLogOut(strUsername, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
            return Json(_response);
        }


        public ActionResult Info()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Info(string search, int limit, string sortname, int pageNumber, string order)
        {
            BxAccount _bxaccount = _bxaccountmanager.FindByAccount(Session["username"].ToString());
            if (_bxaccount == null)
            {
                return Json(new { total = 0, rows = "" });

            }
            else
            {
                //Paging<BxAccount> _pagingCustomer = new Paging<BxAccount>();
                
                var item = new  { _bxaccount.Address, _bxaccount.BxAccountId, _bxaccount.Contacts, _bxaccount.CorporateName, _bxaccount.CreateTime, _bxaccount.Email, _bxaccount.Phone ,_bxaccount.username};
                List<object> io = new List<object>();
                io.Add(item);
                
                return Json(new { total = 1, rows =io });
 
            }


            
        }


        public ActionResult Edit()
        {
            //查找是否有存在
            Response _resp = new Response();
            BxAccount _bxaccount = _bxaccountmanager.FindByAccount(Session["username"].ToString());
            if (_bxaccount == null)
            {
                _resp.Status = 0;
                _resp.Message = "ID 出错，不存在相应的保险公司账号";
                return Json(_resp, JsonRequestBehavior.AllowGet);
            }
            return View(_bxaccount);

        }

        [HttpPost]
        public ActionResult Edit(FormCollection fcs, int id = 0)
        {
            Response _resp = new Response();




            BxAccount _bxaccount = _bxaccountmanager.FindByAccount(Session["username"].ToString());
            if (_bxaccount == null)
            {
                _resp.Status = 0;
                _resp.Message = "错误！保险公司账号不存在";
                return Json(_resp);

            }
            TryUpdateModel(_bxaccount, new string[] { "Contacts", "Email", "Address", "Phone" });


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


        public ActionResult Changepwd(int id = 0)
        {
            //查找是否有存在
            Response _resp = new Response();
            BxAccount _bxaccount = _bxaccountmanager.FindByAccount(Session["username"].ToString());
            if (_bxaccount == null)
            {
                _resp.Status = 0;
                _resp.Message = "ID 出错，不存在相应的保险公司账号";
                return Json(_resp, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Changepwd(BxAccountChangePassWordViewModel newchangepwd, int id = 0)
        {
            Response _resp = new Response();
            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);
            }
            BxAccount _bxaccount = _bxaccountmanager.FindByAccount(Session["username"].ToString());
            _resp = _bxaccountmanager.ChangePassword(_bxaccount.BxAccountId,Security.SHA256( newchangepwd.Newpwd));
            if(_resp.Status==1)
            {
                Session.Clear();
                _resp.Url = Url.Action("login", "account", new { area = "bx" });
                _resp.Message = "恭喜，修改密码成功！";
               

            }
            return Json(_resp);
        }
        public ActionResult Detail(int id = 0)
        {
            BxAccount _bxaccount = _bxaccountmanager.FindByAccount(Session["username"].ToString());
            if (_bxaccount == null)
            { //id的终端不存在
                Prompt _prm = new Prompt();
                _prm.Title = "查询详情出错!";
                _prm.Message = "出错啦!不存在对应该ID的保险公司账号";
                return View("ErrorPop", _prm);

            }
            return View(_bxaccount);


        }

    }
}