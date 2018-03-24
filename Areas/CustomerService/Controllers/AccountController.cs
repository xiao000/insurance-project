using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Core.ViewModels;
using Bx_Web.Models;
using Bx_Common;
using Bx_Core;
using System.Drawing;
using Bx_Core.Author;

namespace Bx_Web.Areas.CustomerService.Controllers
{
    [CustomerServicerAuthor]
    public class AccountController : Controller
    {
        private CustomerServiceManager _customerServiceManager = new CustomerServiceManager();
        private LogInOutLogManager _logInOutManager = new LogInOutLogManager();


        //登录登出
        #region //登录
        
        
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
                            _res = _customerServiceManager.Verify(lv.username, _passowrd);
                            if (_res.Status == 1)
                            {
                                var _admin = _customerServiceManager.FindByAccount(lv.username);
                                //判断登录账号是否被禁用
                                if (!_admin.active)
                                {
                                    _res.Status = 0;
                                    _res.Message = "账号被禁用";
                                    //登录失败记录日志
                                    _logInOutManager.AddLogFail(lv.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                                    return Json(_res);
                                }
                               
                                Session.Add("AdminAccountId", _admin.CustomerServiceId);
                                Session.Add("username", _admin.username);
                                Session.Add("Role", "CustService");//系统客服
                                Session.Add("Area", _admin.Area);
                                _admin.LoginTime = DateTime.Now;
                                _admin.LoginIP = Request.UserHostAddress;
                                _admin.LoginCount = _admin.LoginCount + 1;
                                _customerServiceManager.Update(_admin);
                                _res.Url = Url.Action("list", "policy");
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


                //登出
                       [HttpPost]
                public ActionResult Logout()
                {
                    string strUsername = Session["username"].ToString();
                    Session.Clear();
                    Response _response = new Response();
                    _response.Status = 1;
                    _response.Url = Url.Action("Login", "account");
                    _response.Message = "恭喜您！退出成功！";
                    //成功退出记录日志
                    _logInOutManager.AddLogOut(strUsername, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                    return Json(_response);

                }


        #endregion



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




                       public ActionResult Changepwd()
                       {
                           return View();
                       }



                            [HttpPost]
                            [ValidateAntiForgeryToken]
                       public ActionResult Changepwd(ChangePwdViewModel changPwd)
                       {
                           Response _response = new Response();
                           Response _comparePwd;
                           if (!ModelState.IsValid)
                           {
                               _response.Status = 0;
                               _response.Message = General.GetModelErrorString(ModelState);
                               return Json(_response);


                           }
                           else
                           {
                               _comparePwd = _customerServiceManager.Verify(Session["username"].ToString(), Security.SHA256(changPwd.Oldpwd));

                               if (_comparePwd.Status == 0)
                               {
                                   _response.Status = 0;
                                   _response.Message = "原始密码不正确";
                                   return Json(_response);


                               }
                               else
                               {
                                   int _custId;
                                   if (!int.TryParse(Session["AdminAccountId"].ToString(), out _custId))
                                   {
                                       _response.Status = 0;
                                       _response.Message = "ID有问题！";
                                       return Json(_response);


                                   }
                                   else
                                   {
                                       _response = _customerServiceManager.ChangePassword(_custId, Security.SHA256(changPwd.Newpwd));
                                       if (_response.Status == 1)
                                       {
                                           _response.Message = "恭喜您，密码修改成功！";
                                           Session.Clear();
                                           _response.Url = Url.Action("Login", "Account");

                                       }



                                   }

                               }


                           }
                           return Json(_response);
 
                       }


    }

}