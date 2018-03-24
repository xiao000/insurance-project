using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Core;
using Bx_Common;
using Bx_Core.Author;

namespace Bx_Web.Areas.Admin.Controllers
{
        [AdminLoggedAuthor]
    public class CustomerServiceController : Controller
    {
        //初

        private CustomerServiceManager _cusomerServiceManger = new CustomerServiceManager();

        //增
        #region //增

                public ActionResult Add()
                {
                    return View();

                }

                [HttpPost]
                public ActionResult add(CustomerServicer cust)
                {
                    Response _resp = new Response();

                    if (ModelState.IsValid)
                    {
                        if (_cusomerServiceManger.HasAccounts(cust.username))
                        {
                            _resp.Status = 0;
                            _resp.Message = "帐号已存在";
                        }
                
                        else
                        {
                            CustomerServicer _cust = new CustomerServicer();
                            _cust.username = cust.username;
                            _cust.password = Security.SHA256(cust.password);
                            _cust.active = true;
                    
                    
                            _cust.CreateTime = DateTime.Now;
                    
                            _cust.LoginIP = "";
                            _cust.LoginTime = DateTime.Now;
                    
                    
                            _cust.Area = cust.Area;


                            _resp.Message = "恭喜您！账号注册成功！";
                            _resp.Url = Url.Action("login");
                            _resp = _cusomerServiceManger.Add(_cust);
                        }
                    }
                    else
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                    }


                    return Json(_resp);
 
                }

        #endregion

        //删
        #region //删

                [HttpPost]
                public ActionResult Delete(int id=0)
                {
                    Response _resp = new Response();
                   
                    _resp = _cusomerServiceManger.Delete(id);
                    if (_resp.Status == 1)
                    {
                        _resp.Message = "恭喜！删除客服帐号成功.";
                    }
                    return Json(_resp);

                }
        #endregion


                //改
                #region //改

                public ActionResult Edit(int id=0)
                {
                    CustomerServicer _cust = _cusomerServiceManger.Find(id);
                    if (_cust == null)
                    { //id的终端不存在
                        Prompt _prm = new Prompt();
                        _prm.Title = "修改出错!";
                        _prm.Message = "出错啦!不存在对应该ID的客服帐号";
                        return View("ErrorPop", _prm);

                    }
                    return View(_cust);
                }
                [HttpPost]
                public ActionResult Edit(int id, FormCollection fcs)
                {
                    Response _resp = new Response();




                    CustomerServicer _cust = _cusomerServiceManger.Find(id);
                    if (_cust == null)
                    {
                        _resp.Status = 0;
                        _resp.Message = "错误！终端用户不存在";
                        return Json(_resp);

                    }
                    TryUpdateModel(_cust, new string[] { "Area" });


                    if (!ModelState.IsValid)
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                        return Json(_resp);

                    }
                    _resp = _cusomerServiceManger.Update(_cust);
                    if (_resp.Status == 1)
                    {
                        _resp.Message = "恭喜！修改客服帐号数据成功";
                    }


                    return Json(_resp);


                }
                #endregion



                //禁
                #region //禁

                [HttpPost]
                public ActionResult Forbid(int id)
                {
                    Response _resp = new Response();
                    if (id < 1)
                    {
                        _resp.Status = 0;
                        _resp.Message = "错误！参数不正确.";
                        return Json(_resp);
                    }
                    CustomerServicer _cust = _cusomerServiceManger.Find(id);
                    if (_cust == null)
                    {
                        _resp.Status = 0;
                        _resp.Message = "错误！终端客户不存在.";
                        return Json(_resp);

                    }
                    _cust.active = !_cust.active;
                    _resp = _cusomerServiceManger.Update(_cust);
                    if (_resp.Status == 1)
                    {
                        _resp.Message = "恭喜！客服帐号已禁用";
                    }


                    return Json(_resp);
                }
        #endregion



                //详
                #region //详


                public ActionResult Detail(int id)
                {
                    CustomerServicer _cust = _cusomerServiceManger.Find(id);
                    if (_cust == null)
                    { //id的终端不存在
                        Prompt _prm = new Prompt();
                        _prm.Title = "查询详情出错!";
                        _prm.Message = "出错啦!不存在对应该ID的终端客户";
                        return View("ErrorPop", _prm);

                    }
                    return View(_cust);


                }
        #endregion


                //列
                #region //列

                public ActionResult list()
                {
                    return View();
                }
                [HttpPost]
                public ActionResult list(string search, int limit, string sortname, int pageNumber, string order)
                {
                    Paging<CustomerServicer> _pagingCustomer = new Paging<CustomerServicer>();
                    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                    else _pagingCustomer.PageIndex = 1;
                    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                    else _pagingCustomer.PageSize = 10;

                    var _paging = _cusomerServiceManger.FindPageList(_pagingCustomer, search, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
                }

        #endregion
    }
}