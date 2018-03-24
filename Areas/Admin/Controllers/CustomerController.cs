using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Common;
using Bx_Core;
using Bx_Web.Models;
using Bx_Core.Author;

namespace Bx_Web.Areas.Admin.Controllers
{
     [AdminLoggedAuthor]
    public class CustomerController : Controller
    {
        private CustomerManager cusManager = new CustomerManager();
        //
        // GET: /Admin/Customer/
        public ActionResult Index()
        {
            List<Customer> lstCustomer = cusManager.FindList().ToList();
            return Content(lstCustomer.Count().ToString());
        }
        /// <summary>
        /// 终端客户列表
        /// </summary>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <param name="contact"></param>
        /// <param name="corpname"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult list()
        {
            return View();
        }
        [HttpPost]
        public ActionResult list(string search, int limit,string sortname, int pageNumber, string order)
        
             
        {
            Paging<Customer> _pagingCustomer = new Paging<Customer>();
            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
            else _pagingCustomer.PageIndex = 1;
            if ( limit > 0) _pagingCustomer.PageSize = (int)limit;
            else _pagingCustomer.PageSize = 10;

            var _paging = cusManager.FindPageList(_pagingCustomer, search,_pagingCustomer.PageIndex,_pagingCustomer.PageSize ,sortname,order);

            

           
            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
        }


        /// <summary>
        /// 系统管理手动添加终端用户
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Customer cust) 
        {

            Response _resp = new Response();
            if (ModelState.IsValid)
            {
                if (cusManager.HasAccounts(cust.username))
                {
                    _resp.Status = 0;
                    _resp.Message = "帐号已存在";
                }
                else if (cusManager.HasPhone(cust.Phone))
                {
                    _resp.Status = 0;
                    _resp.Message = "手机号码已存在";
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
                    _cust.password =Security.SHA256( cust.password);
                    _cust.Phone = cust.Phone;
                    _cust.Area = cust.Area;


                    _resp.Message = "恭喜您！账号注册成功！";
                    _resp.Url = Url.Action("login");
                    _resp = cusManager.Add(_cust);
                }
            }
            else
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
            }
            return Json(_resp);
        }

        /// <summary>
        /// 删除终端客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Response _resp = new Response();
            if(id<1)
            {
                _resp.Status = 0;
                _resp.Message = "错误！参数不正确.";
                return Json(_resp);
            }
            _resp = cusManager.Delete(id);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！删除终端客户成功.";
            }
            return Json(_resp);
 
        }


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
            Customer _cust = cusManager.Find(id);
            if (_cust == null)
            {
                _resp.Status = 0;
                _resp.Message = "错误！终端客户不存在.";
                return Json(_resp);
 
            }
            _cust.active = !_cust.active;
            _resp= cusManager.Update(_cust);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！终端用户已禁用";
            }


            return Json(_resp);
        }

        public ActionResult Edit(int id)
        {
            Customer _cust = cusManager.Find(id);
            if (_cust == null)
            { //id的终端不存在
                Prompt _prm = new Prompt();
                _prm.Title = "修改出错!";
                _prm.Message = "出错啦!不存在对应该ID的终端客户";
                return View("ErrorPop",_prm);

            }
            return View(_cust);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection fcs)
        {
            Response _resp=new Response();

            


            Customer _cust = cusManager.Find(id);
            if (_cust == null)
            {
                _resp.Status = 0;
                _resp.Message = "错误！终端用户不存在";
                return Json(_resp);
 
            }
            TryUpdateModel(_cust, new string[] { "Contacts", "Email", "CorporateName", "Address", "active", "Phone","area" });


            if (!ModelState.IsValid)
            {
                _resp.Status = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
                return Json(_resp);
 
            }
            _resp = cusManager.Update(_cust);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！修改终端用户数据成功";
            }


            return Json(_resp);

 
        }

        public ActionResult Detail(int id) 
        {
            Customer _cust = cusManager.Find(id);
            if (_cust == null)
            { //id的终端不存在
                Prompt _prm = new Prompt();
                _prm.Title = "查询详情出错!";
                _prm.Message = "出错啦!不存在对应该ID的终端客户";
                return View("ErrorPop", _prm);

            }
            return View(_cust);
           

        }

        public ActionResult ExportToExcel()
        {
            Response _res = new Response();
           
            List<Customer> lstCustomer =cusManager.FindList().ToList();
            if (lstCustomer.Count == 0)
            {
                _res.Status = 0;
                _res.Message = "终端客户数为0，不能导出空数据";
                return Json(_res);
            }
            else
            { 

                //Dictionary<string, string> dic_colum = new Dictionary<string, string>();
                //dic_colum.Add("username", "用户名");
                //dic_colum.Add("Contacts", "联系人");
                //dic_colum.Add("Phone", "电话");
                //dic_colum.Add("Email","Email");
                //dic_colum.Add("CorporateName", "公司名称");
                //dic_colum.Add("Address", "地址");
                //dic_colum.Add("CreateDateTime", "生成时间");
                //dic_colum.Add("LoginTime", "登录时间");
                

                //string[] columns = { "username", "Contacts", "Phone", "Email", "CorporateName", "Address", "CreateDateTime", "LoginTime" };
                //string[] newcolumns = {  "用户名", "联系人", "电话", "Email", "公司名称", "地址", "生成时间", "登录时间" };
                //byte[] filecontent = ExcelExportHelper.ExportExcel(lstCustomer, columns, "终端客户", true, newcolumns);


                string[] columns = { "用户名", "联系人", "手机号码", "Email", "公司名称", "详细地址", "创建时间", "登录时间" };
                //string[] newcolumns = { "用户名", "联系人", "电话", "Email", "公司名称", "地址", "生成时间", "登录时间" };
                byte[] filecontent = ExcelExportHelperPlus.ExportExcel(lstCustomer, columns, "终端客户", true);
                string strFilename = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx";
                return File(filecontent, ExcelExportHelperPlus.ExcelContentType, strFilename);


            }
        }

        public JsonResult ExitsAcoount(string username)
        {
            if (cusManager.HasAccounts(username))
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            else {
                return Json("登录账号已存在");
            }
        }

    public JsonResult ExitsPhone(string phone)
        {
            if (cusManager.HasPhone(phone))
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            else {
                return Json("手机号码已存在");
            }
        }

    [HttpPost]
    public ActionResult RestPassWord(int id=0)
    {
        Response _resp = new Response();

        _resp = cusManager.ResetPassWordByAdmin(id);


        return Json(_resp);
 
    }
	}}
