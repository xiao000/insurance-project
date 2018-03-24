using Bx_Common;
using Bx_Core;
using Bx_Core.Author;
using Bx_Core.Config;
using Bx_Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Bx_Web.Areas.Bx.Controllers
{
    [BxLoggedAuthor]
    public class PolicyController : Controller
    {
        //初
        #region //初


                PolicyManager _policymanager = new PolicyManager();
                UnPolicyReasonManager _unPolicyReasonManager = new UnPolicyReasonManager();
                PolicyFileManager _policyFileManager = new PolicyFileManager();
                PolicyBackReasonManager _policyBackReasonManager = new PolicyBackReasonManager();
                PolicyChangeReasonManager _policyChangeReasonManger = new PolicyChangeReasonManager();
                CustomerProductManager _custpromanager = new CustomerProductManager();
                PolicyEleManager _policyEleManager = new PolicyEleManager();
                PolicyChangedAnnexManager _policyChangedAnnexMannager = new PolicyChangedAnnexManager();
                PolicyBackAnnexManager _policyBackAnnexManager = new PolicyBackAnnexManager();
                PolicyChangedAnnexByBxManager _policyChangedAnnexByBxManager = new PolicyChangedAnnexByBxManager();
                
        #endregion



        // 列
        #region // 列


                            public ActionResult List()
                            {
                                return View();
                            }

                            [HttpPost]
                            public ActionResult List(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_all";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);


                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列未审核的
                            //[HttpPost]
                            //public ActionResult List_WaitPolicy(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_waitpolicy";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //列审核失败的
                            //[HttpPost]
                            //public ActionResult List_APolicyErr(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_apolicyerr";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //列待bx审核
                            [HttpPost]
                            public ActionResult List_BWaitPolicy(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "BWaitPolicyTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_bwaitpolicy";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列bx审核失败
                            [HttpPost]
                            public ActionResult List_BPolicyErr(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "BPolicyErrTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_bpolicyerr";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列已审核待付款
                            [HttpPost]
                            public ActionResult List_PolicyWaitPay(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "PolicyWaitPayTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_policywaitpay";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列待受理
                            [HttpPost]
                            public ActionResult List_PolicyPayed(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "PolicyPayedTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_policypayed";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列待受理
                            //[HttpPost]
                            //public ActionResult List_WaitAccept(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_waitaccept";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //列已生效
                            [HttpPost]
                            public ActionResult List_AllPolicyed(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_allpolicyed";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列待a审核退保
                            //[HttpPost]
                            //public ActionResult List_PolicyWaitBack(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_policywaitback";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //A审核退保失败
                            //[HttpPost]
                            //public ActionResult List_APolicyBackErr(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_apolicybackErr";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //列待B审核退保
                            [HttpPost]
                            public ActionResult List_BPolicyWaitBack(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "BPolicyWaitBackTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_bpolicywaitback";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列B审核退保失败
                            [HttpPost]
                            public ActionResult List_BPolicyBackErr(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "BPolicyBackErrTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_bpolicybackErr";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列已退保
                            [HttpPost]
                            public ActionResult List_PolicyBack(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "PolicyBackTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_policyback";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列待a审核变更
                            //[HttpPost]
                            //public ActionResult List_PolicyWaitChange(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_policywaitchange";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //列A审核变更失败
                            //[HttpPost]
                            //public ActionResult List_APolicyChangedErr(string search, int limit, string sortname, int pageNumber, string order)
                            //{

                            //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            //    else _pagingCustomer.PageIndex = 1;
                            //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            //    else _pagingCustomer.PageSize = 10;

                            //    string listwhat = "list_apolicychangederr";
                            //    var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            //}
                            //列待B审核变更
                            [HttpPost]
                            public ActionResult List_BPolicyWaitChange(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "BPolicyWaitChangeTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_bpolicywaitchange";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //B审核变更失败
                            [HttpPost]
                            public ActionResult List_BPolicyChangedErr(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "BPolicyChangedErrTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_bpolicychangederr";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列已变更
                            [HttpPost]
                            public ActionResult List_PolicyChanged(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "PolicyChangedTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_policychanged";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }
                            //列禁用
                            [HttpPost]
                            public ActionResult List_PolicyForbid(string search, int limit, string sortname, int pageNumber, string order)
                            {

                                Paging<Policy> _pagingCustomer = new Paging<Policy>();
                                //如果排序为空，代表默认排序，指定此API默认排序字段
                                if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                                {
                                    sortname = "PolicyForbidTime";

                                }
                                if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                                else _pagingCustomer.PageIndex = 1;
                                if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                                else _pagingCustomer.PageSize = 10;

                                string listwhat = "list_policyforbid";
                                var _paging = _policymanager.FindPageListByBx(_pagingCustomer, Session["BxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                                return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                            }

                #endregion

        //详情

                        public ActionResult Detail(int id=0)
                        {
                            Response _resp = new Response();

                            Policy _policy = _policymanager.GetDetailByBx(Session["BxCorporateName"].ToString(),id);
                            if (_policy == null)
                            {
                                _resp.Status = 0;
                                _resp.Message = "错误！由于归属或权限问题不能查看该保单";
                                return Json(_resp, JsonRequestBehavior.AllowGet);
                            }

                            //获取保单相关的文件信息,并加载到VIEWBAG中
                            ViewBag.PolicyFileList = _policyFileManager.GetPolicyListById(_policy.PolicyId).Count > 0 ? _policyFileManager.GetPolicyListById(_policy.PolicyId) : null; 
                            //获取相关表单的审核失败原因,并加载到viewbag中
                            ViewBag.APolicyErrReasonList = _unPolicyReasonManager.GetBPolicyErrListByBx(Session["BxCorporateName"].ToString(), _policy.PolicyId);
                            //获取相关表单的退单失败原因,并加载到viewbag中
                            ViewBag.APolicyBackErrList = _unPolicyReasonManager.GetBPolicyBackErrListByBx(Session["BxCorporateName"].ToString(), _policy.PolicyId);
                            //获取相关表单的变更失败原因,并加载到viewbag中
                            ViewBag.APolicyChangedErrList = _unPolicyReasonManager.GetBPolicyChangedErrListByBx(Session["BxCorporateName"].ToString(), _policy.PolicyId);
                            //获取相关保单变更原因
                            ViewBag.PolicyBackReasonList = _policyBackReasonManager.GetAPolicyWaitBackListByBx(Session["BxCorporateName"].ToString(), _policy.PolicyId);

                            ViewBag.PolicyChangeReasonList = _policyChangeReasonManger.GetPolicyChangeReasonByBx(Session["BxCorporateName"].ToString(), _policy.PolicyId);

                            //int _tmpint = _policyChangeReasonManger.GetPolicyChangeReasonByBx(Session["username"].ToString(), _policy.PolicyId).Count();

                            //获取保单回单列表
                            ViewBag.PolicyEleList = _policyEleManager.GetPolicyEleListByIdByBx(_policy.PolicyId);

                            //获取保单变更a端回单列表
                            ViewBag.PolicyChangedAnnexList = _policyChangedAnnexMannager.GetPolicyChangedAnnexlst(_policy.PolicyId).Count > 0 ? _policyChangedAnnexMannager.GetPolicyChangedAnnexlst(_policy.PolicyId) : null;


                            //获取保单退保a端回单列表

                            ViewBag.PolicyBackAnnexList = _policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId).Count > 0 ?_policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId) : null;

                            return View(_policy);
                        }


        //统

                        /// <summary>
                        /// 返回各种状态的保单统计
                        /// </summary>
                        /// <returns></returns>
                        [HttpPost]
                        public ActionResult Count()
                        {
                            return Json(_policymanager.CountByBx(Session["BxCorporateName"].ToString()));
                        }


        //变更
        #region //变更


                        /// <summary>
                        /// 被bx审核不通过
                        /// </summary>
                        /// <param name="id"></param>
                        /// <returns></returns>
                        [HttpPost]
                        public ActionResult BPolicyErr(FormCollection fc, int id = 0)
                        {
                            Response _resp = new Response();
                            UnPolicyReason _unPolicyReason = new UnPolicyReason();
                            //验证未通过原因
                            if (!TryUpdateModel(_unPolicyReason, new string[] { "APolicyErrReason" }))
                            {
                                _resp.Status = 0;
                                _resp.Message = General.GetModelErrorString(ModelState);

                                return Json(_resp);
                            }

                            _unPolicyReason.Creater = Session["username"].ToString();
                            _unPolicyReason.CorporateName = Session["BxCorporateName"].ToString();
                            _resp = _policymanager.ChangeToBPolicyErr(Session["BxCorporateName"].ToString(), _unPolicyReason, id);
                            return Json(_resp);
                        }



                        /// <summary>
                        /// 被bx审核通过,由bx发起
                        /// </summary>
                        /// <param name="id"></param>
                        /// <returns></returns>
                        //[HttpPost]
                        //public ActionResult PolicyWaitPay(int id = 0)
                        //{
                        //    Response _resp = new Response();

                        //    _resp = _policymanager.ChangeToPolicyed(Session["BxCorporateName"].ToString(), id);
                        //    return Json(_resp);

                        //}

                        //注意之前是没有这个API的，后来付款前置，增加了这个API,功能为通过后，直接生效

                        [HttpPost]
                        public ActionResult Policyed(int id = 0)
                        {
                            Response _resp = new Response();

                            _resp = _policymanager.ChangeToPolicyed(Session["BxCorporateName"].ToString(), id);
                            return Json(_resp);

                        }

                        public ActionResult Accepted(int id = 0)
                        {
                            Response _resp = new Response();

                            _resp = _policymanager.ChangeToPolicyed(Session["BxCorporateName"].ToString(), id);
                            return Json(_resp);
                        }


                        public ActionResult PolicyBack(int id = 0)
                        {
                            Response _resp = new Response();

                            _resp = _policymanager.ChangeToPolicyBack(Session["BxCorporateName"].ToString(), id);
                            return Json(_resp);

                        }

                        [HttpPost]
                        public ActionResult BPolicyBackErr(FormCollection fc, int id = 0)
                        {
                            Response _resp = new Response();
                            UnPolicyReason _unPolicyReason = new UnPolicyReason();
                            //验证未通过原因
                            if (!TryUpdateModel(_unPolicyReason, new string[] { "APolicyErrReason" }))
                            {
                                _resp.Status = 0;
                                _resp.Message = General.GetModelErrorString(ModelState);

                                return Json(_resp);
                            }

                            _unPolicyReason.Creater = Session["username"].ToString();
                            _unPolicyReason.CorporateName = Session["BxCorporateName"].ToString();
                            _resp = _policymanager.ChangeToBPolicyBackErr(Session["BxCorporateName"].ToString(), _unPolicyReason, id);
                            return Json(_resp);

                        }
                            /// <summary>
                            /// 保单变更b通过
                            /// </summary>
                            /// <param name="id"></param>
                            /// <returns></returns>
                            [HttpPost]
                        public ActionResult PolicyChanged(int id = 0)
                        {
                            Response _resp = new Response();

                            _resp = _policymanager.ChangeToPolicyChanged(Session["BxCorporateName"].ToString(), id);
                            return Json(_resp);
 
                        }
                            
                            /// <summary>
                            /// 保单变更b拒绝
                            /// </summary>
                            /// <param name="fc"></param>
                            /// <param name="id"></param>
                            /// <returns></returns>
                            [HttpPost]
                        public ActionResult BPolicyChangedErr(FormCollection fc, int id = 0)
                        {
                            Response _resp = new Response();
                            UnPolicyReason _unPolicyReason = new UnPolicyReason();
                            //验证未通过原因
                            if (!TryUpdateModel(_unPolicyReason, new string[] { "APolicyErrReason" }))
                            {
                                _resp.Status = 0;
                                _resp.Message = General.GetModelErrorString(ModelState);

                                return Json(_resp);
                            }

                            _unPolicyReason.Creater = Session["username"].ToString();
                            _unPolicyReason.CorporateName = Session["BxCorporateName"].ToString();
                            _resp = _policymanager.ChangeToBPolicyChangedErr(Session["BxCorporateName"].ToString(), _unPolicyReason, id);
                            return Json(_resp);
 
                        }

        #endregion


                            //导policy
                            #region //导policy

                            /// <summary>
                            /// 导出某个保险公司下某个状态(现在限制为已生效)的保单
                            /// </summary>
                            /// <returns></returns>
                            //[HttpPost]
                            //public ActionResult ExportSuccess()
                            //{
                            //    Response _resp = new Response();
                            //    _resp = _policymanager.ExportPolicyListByBx(Session["BxCorporateName"].ToString(), PolicyStatus.Policyed);
                            //    if (_resp.Status == 0)
                            //    {
                            //        return Json(_resp);
                            //    }

                            //    byte[] filecontent = _resp.Data;
                            //    _resp.Data = null;
                            //    string strFilename = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx";
                            //    return File(filecontent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFilename);
                            //}

                            public ActionResult export()
                            {
                                return View();
                            }



                            /// <summary>
                            /// 返回一个含有过滤结果的数据流（excel格式）
                            /// </summary>
                            /// <param name="startcon">起始时间</param>
                            /// <param name="endcon">结束时间</param>
                            /// <param name="exportType">保单类型</param>
                            /// <param name="export_search">搜索字符串</param>
                            /// <returns></returns>

                            [HttpPost]
                            public ActionResult export(DateTime? startcon2, DateTime? endcon2, string exportType, string exportsearch)
                            {
                                Response _resp = new Response();
                                _resp = _policymanager.ExportPolicyListByBx(startcon2, endcon2, exportType,  Session["BxCorporateName"].ToString());
                                if (_resp.Status == 0)
                                {
                                    return View("exportErr");
                                }

                                byte[] filecontent = _resp.Data;
                                _resp.Data = null;
                                string strFilename = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx";
                                return File(filecontent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFilename);

                            }
                            
                            #endregion




                        


    //获

                        /// <summary>
                        /// 根据保险产品ID获得保险条款内容
                        /// </summary>
                        /// <param name="id"></param>
                        /// <returns></returns>
                        public string GetBxClauses(int id = 0)
                        {
                            return _custpromanager.GetBxClauses(id);
                        }


                        public ActionResult Policyele(int id=0)
                        {
                            ViewBag.Id = id;
                            return View();
                        }



      //投保回单
            #region //投保保单
                        
                        
                        //增
                        [HttpPost]
                        public ActionResult PolicyEle(int Id=0)
                        {
                            Response _resp = new Response();
                            _resp = _policymanager.CheckIsSonByBx(Id, Session["BxCorporateName"].ToString());

                            if (_resp.Status==0)
                            {
                                _resp.Status=0;
                                _resp.Message="归属问题，禁止操作";
                                return Json(_resp);

                            }
                            Policy _policy = _resp.Data;

                            UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                            int _custperdayupload = _uploadConfig.CustPerDayUpload;
                            //这个地方是每天上传图片数量的验证,已经取消了,有问题,有时间要打开
                            //if (_policyfilemanager.GetTodayAddCountByCust(Session["username"].ToString()) >= _custperdayupload)
                            //{


                            //    _resp.Status = 0;
                            //    _resp.Message = "错误！今日上传文件数量已经超过系统指定标准，文件上传失败";
                            //    return Json(_resp);
                            //}



                            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                            for (int i = 0; i < files.Count;i++ )
                            {
                                _resp = _policyEleManager.AddFileAndRecord(_uploadConfig, files[i], "imagePdf", Session["username"].ToString(), Session["BxCorporateName"].ToString(),_policy.Creater, _policy.PolicyId);

                                if (_resp.Status!=1)
                                {
                                    return Json(_resp);

                                }
                            }

                            
                            return Json(_resp);//不管对错,都直接返回结果
 
                        }

                        //删除
                        [HttpPost]
                        public ActionResult PolicyEleDelete(int id = 0)//这里的是policyele的ID
                            {
                            Response _resp = new Response();
                            //取policy的id
                            int _intPolicyId = _policyEleManager.GetPolicyIdByBx(id);
                            
                                PolicyStatus? _policyStatus = _policymanager.GetPolicyStatusByCust(_intPolicyId);
                                if (null == _policyStatus)
                                {
                                    _resp.Status = 0;
                                    _resp.Message = "错误!相关保单的状态不正确";
                                    return Json(_resp);
                                }
                                _resp = _policyEleManager.DeleteById(Session["BxCorporateName"].ToString(), _policyStatus, id);
                                return Json(_resp);







                            }

                        //改
                        [HttpGet]
                        public ActionResult PolicyEleEdit(int id=0)
                        {
                            Response _resp = new Response();
                            _resp = _policymanager.CheckIsSonByBx(id, Session["BxCorporateName"].ToString());

                            if (_resp.Status == 0)
                            {
                                _resp.Status = 0;
                                _resp.Message = "归属问题，禁止操作";
                                return Json(_resp,JsonRequestBehavior.AllowGet);

                            }

                            ViewBag.policyEleList = _policyEleManager.GetPolicyEleListByIdByBx(id).Count()>0?_policyEleManager.GetPolicyEleListByIdByBx(id):null;
                            return View();
                        }


                        [HttpPost]
                        public ActionResult PolicyEleEdit(FormCollection fc,int id=0)
                        {
                            Response _resp = new Response();
                            _resp = _policymanager.CheckIsSonByBx(id, Session["BxCorporateName"].ToString());
                            if (_resp.Status == 0)
                            {
                                _resp.Status = 0;
                                _resp.Message = "归属问题，禁止操作";
                                return Json(_resp);

                            }
                            Policy _policy = _resp.Data;


                            

                            PolicyStatus? _policyStatus = _policymanager.GetPolicyStatusByCust(_policy.PolicyId);
                            if (null == _policyStatus)
                            {
                                _resp.Status = 0;
                                _resp.Message = "错误!相关保单的状态不正确";
                                return Json(_resp);
                            }

                            //依次删除相关保单下的所有回单

                            //取回单列表

                            //List<PolicyEle> listPolicyEle = _policyEleManager.GetPolicyEleListByIdByBx(_policy.PolicyId);

                            //foreach (PolicyEle item in listPolicyEle)
                            //{
                            //    _resp = _policyEleManager.DeleteById(Session["BxCorporateName"].ToString(), _policyStatus, item.PolicyeleId);

                            //    if (_resp.Status==0)
                            //    {

                            //         return Json(_resp);

                            //    }

                            //}

                            
                           




                            UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                            int _custperdayupload = _uploadConfig.CustPerDayUpload;


                            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                            for (int i = 0; i < files.Count; i++)
                            {
                                _resp = _policyEleManager.AddFileAndRecord(_uploadConfig, files[i], "imagePdf", Session["username"].ToString(), Session["BxCorporateName"].ToString(), _policy.Creater, _policy.PolicyId);

                                if (_resp.Status != 1)
                                {
                                    return Json(_resp);

                                }
                            }

                            return Json(_resp);
                        
                        }


                        //取消（删除）之前的所有回单，用于上传回单的取消按钮
                            
                        public ActionResult DeleteAllPolicyEle(int id=0)//这里的id是policy的ID
                        {

                            Response _resp = new Response();
                            _resp = _policymanager.CheckIsSonByBx(id, Session["BxCorporateName"].ToString());

                            if (_resp.Status == 0)
                            {
                                _resp.Status = 0;
                                _resp.Message = "归属问题，禁止操作";
                                return Json(_resp);

                            }

                            return null;
                        }

                        
                        //详情

                        public ActionResult PolicyeleDetail(int id=0)
                        {
                            Response _resp = new Response();
                            Policy _policy = _policymanager.GetDetailByBx(Session["BxCorporateName"].ToString(), id);
                            if (_policy == null)
                            {
                                _resp.Status = 0;
                                _resp.Message = "错误！由于归属或权限问题不能查看该保单";
                                return View("error", _resp);
                            }

                            //获取保单回单列表
                            
                            ViewBag.PolicyEleList = _policyEleManager.GetPolicyEleListByIdByCust(_policy.PolicyId);
                            ViewBag.PolicyChangedAnnexByBx = _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id) != null ? _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id) : null;
                            
                            return View();
                        }


            #endregion
        //                //投保回单两个按钮
        // #region //回单两个按钮

        //                //确认回单
        //                    [HttpPost]
        //                public ActionResult PolicyEleOk(int id=0)//这里的ID是policy的ID
        //                {
        //                    Response _resp = new Response();
        //                    _resp = _policyEleManager.ConfirmPolicyEle(Session["BxCorporateName"].ToString(), id);
        //                    return Json(_resp);
                        
        //                }
        //                    [HttpPost]
        //                public ActionResult PolicyEleCancel(int id = 0)//这里的ID是policy的ID
        //                {
        //                    Response _resp = new Response();
        //                    _resp = _policyEleManager.CancelPolicyEle(Session["BxCorporateName"].ToString(), id);
        //                    return Json(_resp);

        //                }



        //#endregion



                        //根据权限显示文件内容
                        #region //根据权限显示文件内容

                        /// <summary>
                        /// 显示保单回单
                        /// </summary>
                        /// <param name="policyEleId"></param>
                        /// <returns></returns>
                        public ActionResult GetPolicyEle(int policyEleId = 0)
                        {
                            string strFilePath = _policyEleManager.GetPolicyElePathByBx(Session["BxCorporateName"].ToString(),policyEleId);

                            strFilePath = Server.MapPath(strFilePath);
                            FileStream fs = System.IO.File.OpenRead(strFilePath);
                            string strExt = Path.GetExtension(strFilePath);
                            switch (strExt)
                            {
                                case "jpg":
                                    return File(fs, "image/jpeg");


                                case "png":
                                    return File(fs, "image/png");

                                case "pdf":
                                    return File(fs, "application/pdf");


                                default:
                                    strFilePath = @"/images/404.jpg";
                                    strFilePath = Server.MapPath(strFilePath);
                                    fs = System.IO.File.OpenRead(strFilePath);
                                    return File(fs, "image/jpeg");
                            }
                        }



                        /// <summary>
                        /// 显示变更回单
                        /// </summary>
                        /// <param name="policyChangedAnnex"></param>
                        /// <returns></returns>
                        public ActionResult GetPolicyChangedAnnex(int policyChangedAnnexId = 0)
                        {
                            string strFilePath = _policyChangedAnnexMannager.GetPolicyChangedAnnexPathByBx(Session["BxCorporateName"].ToString(), policyChangedAnnexId);

                            strFilePath = Server.MapPath(strFilePath);
                            FileStream fs = System.IO.File.OpenRead(strFilePath);
                            string strExt = Path.GetExtension(strFilePath);
                            switch (strExt)
                            {
                                case "jpg":
                                    return File(fs, "image/jpeg");


                                case "png":
                                    return File(fs, "image/png");

                                case "pdf":
                                    return File(fs, "application/pdf");


                                default:
                                    strFilePath = @"/images/404.jpg";
                                    strFilePath = Server.MapPath(strFilePath);
                                    fs = System.IO.File.OpenRead(strFilePath);
                                    return File(fs, "image/jpeg");
                            }
                        }

                        /// <summary>
                        /// 显示退保回单
                        /// </summary>
                        /// <param name="PolicyBackAnnex"></param>
                        /// <returns></returns>

                        public ActionResult GetPolicyBackAnnex(int PolicyBackAnnexId = 0)
                        {
                            string strFilePath = _policyBackAnnexManager.GetPolicyBackAnnexPathByBx(Session["BxCorporateName"].ToString(), PolicyBackAnnexId);

                            strFilePath = Server.MapPath(strFilePath);
                            FileStream fs = System.IO.File.OpenRead(strFilePath);
                            string strExt = Path.GetExtension(strFilePath);
                            switch (strExt)
                            {
                                case "jpg":
                                    return File(fs, "image/jpeg");


                                case "png":
                                    return File(fs, "image/png");

                                case "pdf":
                                    return File(fs, "application/pdf");


                                default:
                                    strFilePath = @"/images/404.jpg";
                                    strFilePath = Server.MapPath(strFilePath);
                                    fs = System.IO.File.OpenRead(strFilePath);
                                    return File(fs, "image/jpeg");
                            }
                        }





                        #endregion




                        public ActionResult ChangedPolicyEle()
                        {
                            int id = 0;
                            if (int.TryParse(Request["id"].ToString(),out id)==false)
                            {
                                id = 0;
                            }
                           // ViewBag.PolicyChangedAnnex = _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id)!=null?_policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id):null;
                            ViewBag.PolicyEleList = _policyEleManager.GetPolicyEleListByIdByCust(id)!=null?_policyEleManager.GetPolicyEleListByIdByCust(id):null;

                            return View();
                        }

                            [HttpPost]
                        public ActionResult ChangedPolicyEle(int id=0)//这里的id是POLICYID
                        {
                            

                            Response _resp = new Response();
                            _resp = _policymanager.CheckIsSonByBx(id, Session["BxCorporateName"].ToString());

                            if (_resp.Status == 0)
                            {
                                _resp.Status = 0;
                                _resp.Message = "归属问题，禁止操作";
                                return Json(_resp);

                            }
                            Policy _policy = _resp.Data;

                            UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                            int _custperdayupload = _uploadConfig.CustPerDayUpload;
                            



                            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                            for (int i = 0; i < files.Count; i++)
                            {
                                _resp = _policyChangedAnnexByBxManager.AddFileAndRecord(_uploadConfig, files[i], "imagePdf", Session["username"].ToString(), Session["BxCorporateName"].ToString(), _policy.PolicyId);

                                if (_resp.Status != 1)
                                {
                                    return Json(_resp);

                                }
                            }


                            return Json(_resp);//不管对错,都直接返回结果
 
                        }

                        public ActionResult ChangedPolicyEleEdit()
                        {
                            int id = 0;
                            if (int.TryParse(Request["id"].ToString(), out id) == false)
                            {
                                id = 0;
                            } 
                            ViewBag.PolicyChangedAnnexByBx = _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id) != null ? _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id) : null;
                            ViewBag.policyEleList = _policyEleManager.GetPolicyEleListByIdByBx(id).Count() > 0 ? _policyEleManager.GetPolicyEleListByIdByBx(id) : null;
                            return View();
                        }


                        [HttpPost]
                        public ActionResult ChangedPolicyEleEdit(int id = 0)//这里的id是POLICYID
                        {


                            Response _resp = new Response();
                            _resp = _policymanager.CheckIsSonByBx(id, Session["BxCorporateName"].ToString());

                            if (_resp.Status == 0)
                            {
                                _resp.Status = 0;
                                _resp.Message = "归属问题，禁止操作";
                                return Json(_resp);

                            }
                            Policy _policy = _resp.Data;

                            UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                            int _custperdayupload = _uploadConfig.CustPerDayUpload;




                            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                            for (int i = 0; i < files.Count; i++)
                            {
                                _resp = _policyChangedAnnexByBxManager.AddFileAndRecord(_uploadConfig, files[i], "imagePdf", Session["username"].ToString(), Session["BxCorporateName"].ToString(), _policy.PolicyId);

                                if (_resp.Status != 1)
                                {
                                    return Json(_resp);

                                }
                            }


                            return Json(_resp);//不管对错,都直接返回结果

                        }
                        
                        [HttpPost]
                        public ActionResult ChangedPolicyeleDelete(int id = 0)
                        {
                            Response _resp = new Response();

                            int _intPolicyId =_policyChangedAnnexByBxManager.GetPolicyIdByBx(id);
                            
                                PolicyStatus? _policyStatus = _policymanager.GetPolicyStatusByCust(_intPolicyId);
                                if (null == _policyStatus)
                                {
                                    _resp.Status = 0;
                                    _resp.Message = "错误!相关保单的状态不正确";
                                    return Json(_resp);
                                }
                                //_resp = _policyEleManager.DeleteById(Session["BxCorporateName"].ToString(), _policyStatus, id);
                                //return Json(_resp);

                                _resp = _policyChangedAnnexByBxManager.DeleteById(Session["BxCorporateName"].ToString(), _policyStatus, id);
                            return Json(_resp);
                        }
    }       
}