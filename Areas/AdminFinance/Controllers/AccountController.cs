using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Web.Models;
using Bx_Common;
using Bx_Core;
using Bx_Core.Author;

namespace Bx_Web.Areas.AdminFinance.Controllers
{
    [AdminCWLoggedAuthor]
    public class AccountController : Controller
    {
        private AdminAccountManager _adminaccountmanager = new AdminAccountManager();
        private LogInOutLogManager _logInOutManager = new LogInOutLogManager();
        public ActionResult Changepwd()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Changepwd(ChangePwdViewModel cpvm)
        {
            Response _resp = new Response();
            if (!ModelState.IsValid)
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
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！修改密码成功！";
                Session.Clear();
                _resp.Url = Url.Action("login", "adminaccount", new { area = "admin" });

            }
            return Json(_resp);




        }




        [HttpPost]
        public ActionResult Logout()
        {
            string strUsername = Session["username"].ToString();
            Session.Clear();
            Response _response = new Response();
            _response.Status = 1;
            _response.Url = Url.Action("Login", "adminaccount", new { area="admin"});
            _response.Message = "恭喜您！退出成功！";
            //成功退出记录日志
            _logInOutManager.AddLogOut(strUsername, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
            return Json(_response);

        }
	}
}