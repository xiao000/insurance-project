using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Common;
using Bx_Core;
using Bx_Web.Models;
using Bx_Core.Author;


namespace Bx_Web.Areas.AdminFinance.Controllers
{
    [AdminCWLoggedAuthor]
    public class CustomerController : Controller
    {
        //初
        #region //初       
        
                private CustomerManager cusManager = new CustomerManager();
                private PolicyManager _policyManager = new PolicyManager();

        #endregion


        
        //列
        #region //列


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
                        public ActionResult list(string search, int limit, string sortname, int pageNumber, string order)
                        {
                            Paging<Customer> _pagingCustomer = new Paging<Customer>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            var _paging = cusManager.FindPageListByAdminCW(_pagingCustomer, search, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);




                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
                        }


                        public ActionResult PolicyList(string username = "")
                        {

                            ViewBag.userNamer = username;

                            return View();

                        }


                        [HttpPost]
                        public ActionResult PolicyList(string search, int limit, string sortname, int pageNumber, string order, string username,string start,string end)
                        {
                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //Paging<PolicyViewModel> _pagingCustomerView = new Paging<PolicyViewModel>();

                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "";
                            string _start = start==null?"":start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username,dtStart,dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
 
                            }

                            //var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                            //_pagingCustomerView = _pagingCustomer;





                            


                        }

                        //列未审核的
                        [HttpPost]
                        public ActionResult PolicyList_WaitPolicy(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_waitpolicy";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列审核失败的
                        [HttpPost]
                        public ActionResult PolicyList_APolicyErr(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_apolicyerr";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列待bx审核
                        [HttpPost]
                        public ActionResult PolicyList_BWaitPolicy(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_bwaitpolicy";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列bx审核失败
                        [HttpPost]
                        public ActionResult PolicyList_BPolicyErr(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_bpolicyerr";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列已审核待付款
                        [HttpPost]
                        public ActionResult PolicyList_PolicyWaitPay(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policywaitpay";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列已付款
                        [HttpPost]
                        public ActionResult PolicyList_PolicyPayed(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policypayed";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列待受理
                        [HttpPost]
                        public ActionResult PolicyList_WaitAccept(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_waitaccept";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列已生效
                        [HttpPost]
                        public ActionResult PolicyList_AllPolicyed(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allpolicyed";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列待a审核退保
                        [HttpPost]
                        public ActionResult PolicyList_PolicyWaitBack(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policywaitback";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //A审核退保失败
                        [HttpPost]
                        public ActionResult PolicyList_APolicyBackErr(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_apolicybackErr";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列待B审核退保
                        [HttpPost]
                        public ActionResult PolicyList_BPolicyWaitBack(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_bpolicywaitback";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列B审核退保失败
                        [HttpPost]
                        public ActionResult PolicyList_BPolicyBackErr(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_bpolicybackErr";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列已退保
                        [HttpPost]
                        public ActionResult PolicyList_PolicyBack(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policyback";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列待a审核变更
                        [HttpPost]
                        public ActionResult PolicyList_PolicyWaitChange(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policywaitchange";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列A审核变更失败
                        [HttpPost]
                        public ActionResult PolicyList_APolicyChangedErr(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_apolicychangederr";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列待B审核变更
                        [HttpPost]
                        public ActionResult PolicyList_BPolicyWaitChange(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_bpolicywaitchange";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //B审核变更失败
                        [HttpPost]
                        public ActionResult PolicyList_BPolicyChangedErr(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_bpolicychangederr";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列已变更
                        [HttpPost]
                        public ActionResult PolicyList_PolicyChanged(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policychanged";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }
                        //列禁用
                        [HttpPost]
                        public ActionResult PolicyList_PolicyForbid(string search, int limit, string sortname, int pageNumber, string order, string username, string start, string end)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_policyforbid";
                            string _start = start == null ? "" : start.ToString();
                            string _end = end == null ? "" : end.ToString();
                            DateTime dtStart = DateTime.Now;
                            DateTime dtEnd = DateTime.Now;

                            if (_start.Length == 0 || _end.Length == 0 || (!DateTime.TryParse(_start, out dtStart)) || (!DateTime.TryParse(_end, out dtEnd)))
                            {
                                var _pagingNoDuring = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username);
                                return Json(new { total = _pagingNoDuring.TotalNumber, rows = _pagingNoDuring.Items });
                            }
                            else
                            {
                                var _paging = _policyManager.FindPageListOnUserNameByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order, username, dtStart, dtEnd);
                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                        }

        #endregion



        //详
        #region //详


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

        #endregion




        //导
        #region //导


                public ActionResult ExportToExcel()
                {
                    Response _res = new Response();

                    List<Customer> lstCustomer = cusManager.FindList().ToList();
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


        #endregion



        //判
        #region //判




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

                    public JsonResult ExitsPhone(string phone)
                    {
                        if (cusManager.HasPhone(phone))
                        {
                            return Json(true, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            return Json("手机号码已存在");
                        }
                    }


        #endregion



        //统
        #region //统

                        /// <summary>
                        /// 返回各种状态的保单统计
                        /// </summary>
                        /// <returns></returns>
                [HttpPost]
                public ActionResult PolicyCount(string username)
                {
                    return Json(_policyManager.CountByAdminFinance(username));
        }

        #endregion

    }
}
