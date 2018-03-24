using Bx_Common;
using Bx_Core;
using Bx_Core.Types;
using Bx_Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Bx_Web.Controllers
{
    public class AccountController : Controller
    {

        //初
        #region //初

            private CustomerManager cusManager = new CustomerManager();
            private LogInOutLogManager _logInOutManager = new LogInOutLogManager();
        
        #endregion



        //注册
        #region //注册

                public ViewResult Register()
                {
                    return View();
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public JsonResult Register(RegisterViewModel cust)
                {
                    Response _res = new Response();


                    if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != cust.validatecode.ToUpper())
                    //if (Session["VerificationCode"] == null || Session["VerificationCode"].ToString() != cust.validatecode.ToUpper())
                    {
                        ModelState.AddModelError("VerificationCode", "验证码不正确");
                        _res.Status = 0;
                        _res.Message = "验证码不正确";
                        return Json(_res);
                    }

                    if (ModelState.IsValid)
                    {
                        if (cusManager.HasAccounts(cust.username))
                        {
                            _res.Status = 0;
                            _res.Message = "帐号已存在";
                            return Json(_res);
                        }
                        else if (cusManager.HasPhone(cust.Phone))
                        {
                            _res.Status = 0;
                            _res.Message = "手机号码已存在";
                            return Json(_res);
                        }
                        else
                        {
                            Customer _cust = new Customer();
                            _cust.username = cust.username;
                            _cust.active = true;
                            _cust.Address = cust.Address;
                            _cust.Contacts = cust.Contacts;
                            _cust.CorporateName = cust.CorporateName;
                            _cust.CreateDateTime = DateTime.Now;
                            _cust.Email = cust.Email;
                            _cust.LoginIP = Request.UserHostAddress.ToString();
                            _cust.LoginTime = DateTime.Now;
                            _cust.parent_id = 0;
                            _cust.password = Security.SHA256(cust.password);
                            _cust.Phone = cust.Phone;



                            _res = cusManager.Add(_cust);
                            if (_res.Status == 1)
                            {
                                _res.Message = "恭喜您！账号注册成功！";
                                _res.Url = Url.Action("login");
                                return Json(_res);
                            }
                        }
                    }
                    else
                    {
                        _res.Status = 0;
                        _res.Message = General.GetModelErrorString(ModelState);
                        return Json(_res);
                    }
                    return Json(_res);
                }
            
        #endregion



        //验证码
        #region 验证码



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



        #endregion



        //登录
        #region //登录


                    public ViewResult Login()
                    {
                        return View();
                    }


                    [AllowAnonymous]
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public ActionResult Login(LoginViewModel loginvw)
                    {
                        Response _res = new Response();
                        CustomerLog _customerLog = new CustomerLog();
                        if (null == TempData["VerificationCode"] || TempData["VerificationCode"].ToString() != loginvw.validatecode.ToUpper())
                        //if (Session["VerificationCode"] == null || Session["VerificationCode"].ToString() != loginvw.validatecode.ToUpper())
                        {
                            // ModelState.AddModelError("VerificationCode", "验证码不正确");
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

                            //string _passowrd = Security.SHA256(loginViewModel.Password);
                            string _passowrd = Security.SHA256(loginvw.password);
                            _res = cusManager.Verify(loginvw.username, _passowrd);
                            if (_res.Status == 1)
                            {
                                var _admin = cusManager.FindByAccount(loginvw.username);
                                //判断登录账号是否被禁用
                                if (!_admin.active)
                                {
                                    _res.Status = 0;
                                    _res.Message = "账号被禁用";
                                    //登录失败记录日志
                                    _logInOutManager.AddLogFail(loginvw.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                                    return Json(_res);
                                }
                                Session.Add("CustomerID", _admin.CustomerID);
                                Session.Add("username", _admin.username);
                                Session.Add("CustCorporateName", _admin.CorporateName);
                                Session.Add("area", _admin.Area);
                                Session.Add("Role", "Customer");
                                _admin.LoginTime = DateTime.Now;
                                _admin.LoginCount += 1;
                                _admin.LoginIP = Request.UserHostAddress;
                                cusManager.Update(_admin);
                                _res.Url = Url.Action("list", "policy");
                                _res.Message = "恭喜您！登录成功！";
                                //登录成功记录日志
                                _logInOutManager.AddLogIn(_admin.username, "使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());

                                return Json(_res);
                            }

                        }

                        //登录失败记录日志
                        _logInOutManager.AddLogFail(loginvw.username, "错误消息:" + _res.Message + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());


                        return Json(_res);
                    }




                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public ActionResult Logout()
                    {
                        string strUsername = Session["username"]==null?"用户已超时退出":Session["username"].ToString();

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



        //帐号存在判断
        #region /帐号存在判断


            public JsonResult ExitsAcoount(string username)
            {
                if (cusManager.HasAccounts(username))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json("登录账号已存在");
                }
            }

        #endregion


            //验证手机号码是否存在
            #region 验证手机号码是否存在



            /// <summary>
            /// 远程验证手机号码是否存在
            /// </summary>
            /// <param name="phone"></param>
            /// <returns></returns>
            /// 
            [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
            public JsonResult ExitsPhone(string phone)
            {
                if (!cusManager.HasPhone(phone))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json("手机号码已存在", JsonRequestBehavior.AllowGet);
                }
            }

            #endregion


            //验证帐号是否存在
            #region //验证帐号是否存在
            
           

            [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
            public JsonResult ExitsUser(string username)
            {
                if (!cusManager.HasAccounts(username))
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("帐号已存在", JsonRequestBehavior.AllowGet);
                }
            }

            #endregion
    }
	}
