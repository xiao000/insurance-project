using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Common;
using Bx_Core.Author;
using Bx_Core;
using Bx_Web.Models;


namespace Bx_Web.Controllers
{
    [CustomerLoggedAuthor]
    public class CustomerController : Controller
    {
        //初
        #region //初       
        
                private CustomerManager cusManager = new CustomerManager();

        #endregion

        //改
        public ActionResult Edit(){
            //找到当前用户信息，并装添
            if (Session["username"] == null  )
            {
                Response _response = new Response();
                _response.Status = 0;
                _response.Message = "出错啦，请重新登录！";
                //return View(_response);
                RedirectToAction("Error", _response);
 
            }
            Customer _customer = cusManager.FindByAccount(Session["username"].ToString());
            if (_customer != null)
            {
                return View(_customer);
            }
            return View();

        
        
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Edit(EditCustomerViewModel edtcust) {
            Response _response = new Response();
            _response.Status = 0;
            _response.Message = "资料添写失败";
            if (!ModelState.IsValid)
            {

                _response.Status = 0;
                _response.Message = General.GetModelErrorString(ModelState);

            }
            else 
            {
                Customer _customer = cusManager.FindByAccount(Session["username"].ToString());
                _customer.Contacts = edtcust.Contacts;
                //取消掉了修改公司名的功能
               // _customer.CorporateName = edtcust.CorporateName;
                _customer.Email = edtcust.Email;
                _customer.Address = edtcust.Address;
                

                _response = cusManager.Update(_customer);
                _response.Url = Url.Action("edit");
                _response.Message = "恭喜您！修改成功！";
                
            }

            return Json(_response);
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


            }
            else
            {
                _comparePwd = cusManager.Verify(Session["username"].ToString(), Security.SHA256(changPwd.Oldpwd));

                if (_comparePwd.Status == 0)
                {
                    _response.Status = 0;
                    _response.Message = "原始密码不正确";


                }
                else
                {
                    int _custId;
                    if(!int.TryParse(Session["CustomerID"].ToString(),out _custId))
                    {
                    _response.Status = 0;
                    _response.Message = "ID有问题！";
                    
                    }else
                    {
                         _response = cusManager.ChangePassword(_custId,Security.SHA256(changPwd.Newpwd));
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

        public ActionResult Error()
        {
            return View();
        }
            
       
	}
}