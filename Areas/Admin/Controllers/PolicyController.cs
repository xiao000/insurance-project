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

namespace Bx_Web.Areas.Admin.Controllers
{
     [AdminLoggedAuthor]
    public class PolicyController : Controller
    {
        //初
        #region //初

            PolicyManager _policymanager = new PolicyManager();
            PolicyFileManager _policyFileManager = new PolicyFileManager();
            UnPolicyReasonManager _unPolicyReasonManager = new UnPolicyReasonManager();
            PolicyBackReasonManager _policyBackReasonManager = new PolicyBackReasonManager();//这句及之上的几句,到最后要分析一下使用的位置,并不是所有的ACTION都会用到,放到能用到的ACTION里效率会更高
            PolicyChangeReasonManager _policyChangeReasonManger = new PolicyChangeReasonManager();
            CustomerProductManager _custpromanager = new CustomerProductManager();
            PolicyEleManager _policyEleManager = new PolicyEleManager();
            CustomerManager _custManager = new CustomerManager();
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

                        string listwhat = "";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //列未审核的
                    [HttpPost]
                    public ActionResult List_WaitPolicy(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_waitpolicy";
                        _pagingCustomer = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                       



                        return Json(new { total = _pagingCustomer.TotalNumber, rows = _pagingCustomer.Items });

                    }
                    //列审核失败的
                    [HttpPost]
                    public ActionResult List_APolicyErr(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        //如果排序为空，代表默认排序，指定此API默认排序字段
                        if (null==sortname||string.IsNullOrEmpty(sortname.Trim()))
                        {
                            sortname = "APolicyErrTime";

                        }
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_apolicyerr";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //列已付款
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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //列待受理
                    [HttpPost]
                    public ActionResult List_WaitAccept(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_waitaccept";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //列待a审核退保
                    [HttpPost]
                    public ActionResult List_PolicyWaitBack(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        //如果排序为空，代表默认排序，指定此API默认排序字段
                        if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                        {
                            sortname = "PolicyWaitBackTime";

                        }
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_policywaitback";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //A审核退保失败
                    [HttpPost]
                    public ActionResult List_APolicyBackErr(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        //如果排序为空，代表默认排序，指定此API默认排序字段
                        if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                        {
                            sortname = "APolicyBackErrTime";

                        }
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_apolicybackErr";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //列待a审核变更
                    [HttpPost]
                    public ActionResult List_PolicyWaitChange(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        //如果排序为空，代表默认排序，指定此API默认排序字段
                        if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                        {
                            sortname = "PolicyWaitChangeTime";

                        }
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_policywaitchange";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
                    //列A审核变更失败
                    [HttpPost]
                    public ActionResult List_APolicyChangedErr(string search, int limit, string sortname, int pageNumber, string order)
                    {

                        Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        //如果排序为空，代表默认排序，指定此API默认排序字段
                        if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                        {
                            sortname = "APolicyChangedErrTime";

                        }
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "list_apolicychangederr";
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }
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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                        var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }


        #endregion



        //变更
        #region //变更



                    /// <summary>
                    /// 被admin审核不通过
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult PolicyErr(FormCollection fc, int id = 0)
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
                        _resp = _policymanager.ChangeToAPolicyErr(_unPolicyReason, id);
                        return Json(_resp);
                    }

                    /// <summary>
                    /// 被admin审核通过,由admin发起
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult BWaitPolicy(int id = 0)
                    {
                        Response _resp = new Response();

                        _resp = _policymanager.ChangeToBWaitPolicy(id);
                        return Json(_resp);

                    }
                    /// <summary>
                    /// a保单退保通过
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult BPolicyWaitBack(FormCollection fc, string bxCorporateName, string cstUsername, int policyId = 0)
                    {
                        Response _resp = new Response();

                        //2018-0130-1047 这里增加了上传a端变更附件的功能
                        if (null == bxCorporateName || null == cstUsername || policyId == 0 || string.IsNullOrEmpty(bxCorporateName.Trim()) || string.IsNullOrEmpty(cstUsername.Trim()))
                        {
                            _resp.Status = 0;
                            _resp.Message = "错误!参数不正确";
                            return Json(_resp);
                        }



                        UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                        if (files.Count == 0)
                        {
                            _resp.Status = 0;
                            _resp.Message = "错误!必须上传保单退保附件";
                            return Json(_resp);


                        }


                        for (int i = 0; i < files.Count; i++)
                        {
                            _resp = _policyBackAnnexManager.AddFileAndRecord(_uploadConfig, files[i], "imagePdf", Session["username"].ToString(), bxCorporateName, cstUsername, policyId);

                            if (_resp.Status != 1)
                            {
                                return Json(_resp);

                            }
                        }



                        _resp = _policymanager.ChangeToBPolicyWaitBack(policyId);
                        return Json(_resp);

                    }

                    /// <summary>
                    /// a保单退保拒绝
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult APolicyBackErr(FormCollection fc, int id = 0)
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
                        _resp = _policymanager.ChangeToAPolicyBackErr(_unPolicyReason, id);
                        return Json(_resp);
                    }
                    /// <summary>
                    /// a保单变更通过
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult BPolicyWaitChange(FormCollection fc, string bxCorporateName, string cstUsername, int policyId = 0)
                    {
                        Response _resp = new Response();
                        //2018-0130-1047 这里增加了上传a端变更附件的功能
                        if (null == bxCorporateName || null == cstUsername||policyId==0||string.IsNullOrEmpty(bxCorporateName.Trim())||string.IsNullOrEmpty(cstUsername.Trim()))
                        {
                            _resp.Status = 0;
                            _resp.Message = "错误!参数不正确";
                            return Json(_resp);
                        }



                        UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                        if(files.Count==0)
                        {
                            _resp.Status = 0;
                            _resp.Message = "错误!必须上传保单变更附件";
                            return Json(_resp);


                        }
                       

                        for (int i = 0; i < files.Count; i++)
                        {
                            _resp = _policyChangedAnnexMannager.AddFileAndRecord(_uploadConfig, files[i], "imagePdf", Session["username"].ToString(), bxCorporateName, cstUsername, policyId);

                            if (_resp.Status != 1)
                            {
                                return Json(_resp);

                            }
                        }

                        _resp = _policymanager.ChangeToBPolicyWaitChanged(policyId);
                        return Json(_resp);
 
                    }
                            /// <summary>
                            /// 禁用保单
                            /// </summary>
                            /// <param name="id"></param>
                            /// <returns></returns>

                    [HttpPost]
                    public ActionResult APolicyChangedErr(int id = 0)
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
                        _resp = _policymanager.ChangeToAPolicyChangedErr (_unPolicyReason, id);
                        return Json(_resp);

                    }


                    [HttpPost]
                    public ActionResult PolicyForbid(int id = 0)
                    {
                        Response _resp = new Response();
                        _resp = _policymanager.ChangeToPolicyForbid(id);
                        return Json(_resp);
                    }



                    //已投递纸质保单

                    [HttpPost]
                    public ActionResult PaperPost(int id = 0)
                    {
                        Response _resp = new Response();
                        _resp = _policymanager.PaperPostByAdmin(id);
                        return Json(_resp);
                    }





                    //增 添加变更附件

                    public ActionResult ChangedAnnex(int Id=0)
                    {
                        Policy _policy = null;
                        _policy = _policymanager.Find(Id);
                        if (null != _policy)
                        {
                            ViewBag.bxCorporatename = _policy.BxCorporateName;
                            ViewBag.cstUsername = _policy.Creater;
                            ViewBag.policyId = Id;
                            return View("ChangedAnnex");

                        }
                        else {
                            Response _resp = new Response();
                            _resp.Status = 0;
                            _resp.Message = "错误!未找到相关保单";
                            return View("_Error",_resp);
                        }

                        
 

                    }


                    

        #endregion

        //详情
                    public ActionResult Detail(int id = 0)
                    {
                        Response _resp = new Response();

                        Policy _policy = _policymanager.GetDetailByAdmin(id);
                        if (_policy == null)
                        {
                            _resp.Status = 0;
                            _resp.Message = "错误！由于归属或权限问题不能查看该保单";
                            return View("error", _resp);
                        }

                        //获取保单相关的文件信息,并加载到VIEWBAG中
                        ViewBag.PolicyFileList = _policyFileManager.GetPolicyListById(_policy.PolicyId).Count > 0 ? _policyFileManager.GetPolicyListById(_policy.PolicyId) : null; ;
                        //获取相关保单的审核失败的全部原因,并加载到viewbag中
                        ViewBag.APolicyErrReasonList = _unPolicyReasonManager.GetALLPolicyErrListByAdmin(_policy.PolicyId);
                        //获取相关保单的退单失败原因,并加载到viewbag中
                        ViewBag.APolicyBackErrList = _unPolicyReasonManager.GetAPolicyBackErrListByAdmin(_policy.PolicyId);
                        //获取相关保单的变更失败原因,并加载到viewbag中
                        ViewBag.APolicyChangedErrList = _unPolicyReasonManager.GetAPolicyChangedErrListByAdmin(_policy.PolicyId);
                        //获取相关保单变更原因
                        ViewBag.PolicyChangeReasonList = _policyChangeReasonManger.GetPolicyChangeReasonByAdmin(_policy.PolicyId);

                        ViewBag.PolicyBackReasonList = _policyBackReasonManager.GetAPolicyWaitBackListByAdmin(_policy.PolicyId);
                        //获取相关保单的退保拒绝原因,并加载到viewbag中
                        ViewBag.APolicyBackErrReasonList = _unPolicyReasonManager.GetAPolicyBackErrListByAdmin(_policy.PolicyId);

                        //获取保单回单列表
                        ViewBag.PolicyEleList = _policyEleManager.GetPolicyEleListByIdByAdmin(_policy.PolicyId);

                        //获取保单变更a端回单列表

                        ViewBag.PolicyChangedAnnexList = _policyChangedAnnexMannager.GetPolicyChangedAnnexlst(_policy.PolicyId).Count>0?_policyChangedAnnexMannager.GetPolicyChangedAnnexlst(_policy.PolicyId):null;

                        //获取保单退保a端回单列表

                        ViewBag.PolicyBackAnnexList = _policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId).Count > 0 ? _policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId) : null;

                        int intCount = _policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId).Count;


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
                        return Json(_policymanager.CountByAdmin());
                    }

                    
                    /// <summary>
                    /// 根据登录帐号返回用户信息，由a调用
                    /// </summary>
                    /// <param name="userNamer"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult CountOnUserByAdmin(string userNamer)
                    {
                        Response _resp=new Response();
                        ////确认登录账号存在
                        //if(!_custManager.HasAccounts(userNamer))
                        //{
                        //    _resp.Status = 0;
                        //    _resp.Message = "错误！登录帐号不存在";
                        //    return Json(_resp);

                        //}

                        ////取该登录帐号下的保单表表

                        //Paging<Policy> _pagingCustomer = new Paging<Policy>();
                        //if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        //else _pagingCustomer.PageIndex = 1;
                        //if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        //else _pagingCustomer.PageSize = 10;
                        //string search = "";
                        //string listwhat = "";
                        //var _paging = _policymanager.FindPageListByAdmin(_pagingCustomer, search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        //return Json(new { total = _paging.TotalNumber, rows = _paging.Items });


                        return  Json(_resp);
 
                    }

                    //导policy

                    #region //导policy
                    
                    
                    /// <summary>
                                /// 导出某个状态(现在限制为已生效)的保单
                                /// </summary>
                                /// <returns></returns>
                                /// 
                    //此部分已于2018-0223-1048时对policymanager中的ExportPolicyListByAdmin做了修改，增加了调用参数，此功能在这个C中未开放
                    //[HttpPost]
                    //public ActionResult ExportSuccess()
                    //{
                    //    Response _resp = new Response();
                    //    _resp = _policymanager.ExportPolicyListByAdmin( PolicyStatus.Policyed);
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
                        _resp = _policymanager.ExportPolicyListByAdmin(startcon2, endcon2, exportType, exportsearch);
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

                    //回单明细

                    public ActionResult PolicyeleDetail(int id=0)
                    {
                        Response _resp = new Response();
                        Policy _policy = _policymanager.GetDetailByAdmin( id);
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




                //退保回单
                    public ActionResult BackAnnex(int Id=0)
                    {
                        Policy _policy = null;
                        _policy = _policymanager.Find(Id);
                        if (null != _policy)
                        {
                            ViewBag.bxCorporatename = _policy.BxCorporateName;
                            ViewBag.cstUsername = _policy.Creater;
                            ViewBag.policyId = Id;
                            return View("BackAnnex");

                        }
                        else
                        {
                            Response _resp = new Response();
                            _resp.Status = 0;
                            _resp.Message = "错误!未找到相关保单";
                            return View("_Error", _resp);
                        }
                        
                    }


        //根据权限显示文件内容
        #region //根据权限显示文件内容
                    
                            /// <summary>
                            /// 显示保单回单
                            /// </summary>
                            /// <param name="policyEleId"></param>
                            /// <returns></returns>
                    public ActionResult GetPolicyEle(int policyEleId=0)
                    {
                        string strFilePath = _policyEleManager.GetPolicyElePathByAdmin(policyEleId);

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
                        string strFilePath = _policyChangedAnnexMannager.GetPolicyChangedAnnexPathByAdmin(policyChangedAnnexId);

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
                        string strFilePath = _policyBackAnnexManager.GetPolicyBackAnnexPathByAdmin(PolicyBackAnnexId);

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
    }
}