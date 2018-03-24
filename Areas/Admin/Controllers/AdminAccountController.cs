using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Web.Models;
using Bx_Common;
using System.Drawing;
using Bx_Core.Author;

using Bx_Core;
namespace Bx_Web.Areas.Admin.Controllers
{
       [AdminLoggedAuthor]
    public class AdminAccountController : Controller
    {
        private AdminAccountManager _adminaccountmanager = new AdminAccountManager();
        private LogInOutLogManager _logInOutManager = new LogInOutLogManager();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel lv)
        {
            Response _res = new Response();
            if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != lv.validatecode.ToUpper())
            
            {
                
                _res.Status = 0;
                _res.Message = "验证码不正确";
                //登录失败记录日志
                _logInOutManager.AddLogFail(lv.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
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

                string _passowrd = Security.SHA256(lv.password);
                //string _passowrd = (lv.password);
                _res = _adminaccountmanager.Verify(lv.username, _passowrd);
                if (_res.Status == 1)
                {
                    var _admin = _adminaccountmanager.FindByAccount(lv.username);
                    //判断登录账号是否被禁用
                    //if(!_admin.active)
                    //{
                    //    _res.Status = 0;
                    //    _res.Message = "账号被禁用";
                    //    return Json(_res);
                    //}
                    Session.Add("AdminAccountId", _admin.AdminId);
                    Session.Add("username", _admin.username);
                    if(_admin.username.ToLower()=="admins")
                    {
                         _res.Url = Url.Action("list", "policy");
                        Session.Add("Role", "SystemManager");
                    }
                    else if (_admin.username.ToLower() == "admincw")
                    {
                         _res.Url = Url.Action("list", "customer",new {area="AdminFinance"});
                         Session.Add("Role", "SystemCW");
                    }
                    
                    _admin.LoginTime = DateTime.Now;
                    _admin.LoginIP = Request.UserHostAddress;
                    _admin.LoginCount = _admin.LoginCount + 1;
                    _adminaccountmanager.Update(_admin);
                   
                    _res.Message = "恭喜您！登录成功！";
                    //登录成功记录日志
                    _logInOutManager.AddLogIn(_admin.username, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                    return Json(_res);
                }
                else
                {
                    //登录失败记录日志
                    _logInOutManager.AddLogFail(lv.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                    return Json(_res);
                }

            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            string strUsername = Session["username"].ToString();
            Session.Clear();
            Response _response = new Response();
            _response.Status = 1;
            _response.Url = Url.Action("Login","adminaccount");
            _response.Message = "恭喜您！退出成功！";
            //成功退出记录日志
            _logInOutManager.AddLogOut(strUsername, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
            return Json(_response);
        
        }

        public ActionResult Changepwd()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Changepwd(ChangePwdViewModel cpvm)
        {
            Response _resp = new Response();
            if(!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);
            }
            string _passowrd = Security.SHA256(cpvm.Oldpwd);
            _resp = _adminaccountmanager.Verify(Session["username"].ToString(), _passowrd);
            if (_resp.Status == 0)
            {
                _resp.Message = "原始密码不正确";
                return Json(_resp);
 
            }
            int _adminid;
            if (!int.TryParse(Session["AdminAccountId"].ToString(), out _adminid))
            {
                _resp.Status = 0;
                _resp.Message = "错误！参数不正确";
                return Json(_resp);

            }
            else
            {
                
                _resp = _adminaccountmanager.ChangePassword(_adminid, Security.SHA256(cpvm.Newpwd));
                

            }
            if(_resp.Status==1)
            {
                _resp.Message = "恭喜！修改密码成功！";
                Session.Clear();
                _resp.Url = Url.Action("login", "adminaccount", new { area = "admin" });

            }
            return Json(_resp);
            


            
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
    }
}