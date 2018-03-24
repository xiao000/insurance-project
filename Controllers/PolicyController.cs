using Bx_Common;
using Bx_Core;
using Bx_Core.Author;
using Bx_Core.Config;
using Bx_Core.Types;
using Bx_Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bx_Web.Controllers
{
    [CustomerLoggedAuthor]
    public class PolicyController : Controller
    {
        //初
        #region //初


            CustomerProductManager _custpromanager = new CustomerProductManager();
            PolicyFileManager _policyfilemanager = new PolicyFileManager();
            PolicyManager _policymanager = new PolicyManager();
            UnPolicyReasonManager _unPolicyReasonManager = new UnPolicyReasonManager();
            PolicyBackReasonManager _policyBackReasonManager = new PolicyBackReasonManager();
            PolicyChangeReasonManager _policyChangeReasonManager = new PolicyChangeReasonManager();
            PolicyEleManager _policyEleManager = new PolicyEleManager();
            PolicyChangedAnnexManager _policyChangedAnnexMannager = new PolicyChangedAnnexManager();
            PolicyBackAnnexManager _policyBackAnnexManager = new PolicyBackAnnexManager();
            PolicyChangedAnnexByBxManager _policyChangedAnnexByBxManager = new PolicyChangedAnnexByBxManager();
        #endregion


        //增
        #region //增


                    public ActionResult Add()
                    {
                        return View();
                    }
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public ActionResult Add(FormCollection fc)
                    {
                        Response _respform = new Response();//表单内容，添加保单部分
                        Response _respfile = new Response();//文件内容，上传文件部分
                        Response _resp = new Response();
                        Policy _policy = new Policy();
                        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;

                        int testcount = files.Count;

                        if (!TryUpdateModel(_policy, new string[] { "CorporateName", "Contacts", "UserType", "IDCardType", "IDCardNum", "Phone", "PolicyContent", "Province", "City", "Town", "Address", "BxCorporateName", "BxType", "BxIndustry", "PolicyDeadline", "BxAmount", "BxPubRate", "PayAmount", "PaperPolicy", "PolRemark", "PolicyAnnex", "isBxClauses" }))
                        {
                            _respform.Status = 0;
                            _respform.Message = General.GetModelErrorString(ModelState);
                            _respform.Data = null;
                            return Json(_respform);
                        }

                        //为CREATER赋值
                        _policy.Creater = (string)Session["username"].ToString();
                        _policy.PolicyCreateTime = DateTime.Now;


                        //明天线下测 2018-0119-2035

                        //string strArea="";
                        //if(null!=Session["area"])
                        //{
                        //    strArea=Session["area"].ToString();

                        //}
                        //string strCustomerCorporateName = "";

                        //if (null != Session["CustCorporateName"])
                        //{
                        //    strCustomerCorporateName = Session["CustCorporateName"].ToString();

                        //}

                        //_policy.Area = strArea;
                        //_policy.CustomerCorporateName = strCustomerCorporateName;
                            

                        string _strPolicyFilesId = "";

                        if (string.IsNullOrEmpty(Request["picName"]) || string.IsNullOrWhiteSpace(Request["picName"]))
                        {

                            _strPolicyFilesId = "";
                        }
                        else
                        {
                            _strPolicyFilesId = Request["picName"].Trim();
                        }


                        _respform = _policymanager.Add(_policy, _strPolicyFilesId, Session["username"].ToString());

                        if (_respform.Status == 0)//上传表单数据有问题，就不继续
                        {
                            return Json(_respform);
                        }

                        // int policyid = _policy.PolicyId;


                        //if (files.Count > 0)//有图上传则上传图片
                        //{

                        //    UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;

                        //    for (int iFile = 0; iFile < files.Count; iFile++)
                        //    {
                        //        HttpPostedFile postedFile = files[iFile];
                        //        _respfile = _policyfilemanager.Add(_uploadConfig, postedFile, "image", Session["username"].ToString(), policyid);
                        //        if (_respfile.Status == 0)
                        //        {
                        //            break;
                        //        }


                        //    }
                        //    if (_respfile.Status == 0)//文件上传失败，
                        //    {
                        //        //删掉之前的保单
                        //        _policymanager.Delete(Session["username"].ToString(), _policy.PolicyId);
                        //        //删掉之前与保单相关的文件

                        //        return Json(_respfile);


                        //    }


                        //}



                        _resp.Status = 1;
                        _resp.Message = "保单已提交，系统正在审批中，请等待并及时关注保单状态变化！";
                        _resp.Url = "/policy/list";





                        return Json(_resp);


                    }
                    //异步上传文件
                    [HttpPost]
                    public ActionResult picUpload()
                    {
                        Response _resp = new Response();

                        UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
                        int _custperdayupload = _uploadConfig.CustPerDayUpload;
                        //这个地方是每天上传图片数量的验证,已经取消了,有问题,有时间要打开
                        if (_policyfilemanager.GetTodayAddCountByCust(Session["username"].ToString()) >= _custperdayupload)
                        {


                            _resp.Status = 0;
                            _resp.Message = "错误！今日上传文件数量已经超过系统指定标准，文件上传失败";
                            return Json(_resp);
                        }



                        HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;


                        _resp = _policyfilemanager.AddFileAndRecord(_uploadConfig, files[0], "image", Session["username"].ToString());
                        return Json(_resp);//不管对错,都直接返回结果

                    }

        #endregion


         //删
         #region //删


                    /// <summary>
                    /// 根据id删保单
                    /// </summary>
                    /// <param name="id">保单</param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult Delete(int id = 0)
                    {
                        Response _resp = new Response();
                        _resp = _policymanager.Delete(Session["username"].ToString(), id);
                        return Json(_resp);
                    }


                    /// <summary>
                    /// 异步删除上传的文件
                    /// </summary>
                    /// <param name="id"></param>
                    /// <returns></returns>
                    [HttpPost]
                    public ActionResult picDelete(int id = 0)
                    {
                        Response _resp = new Response();
                        //取policy的id
                        int _intPolicyId = _policyfilemanager.GetPolicyIdByCust(id);
                        if(_intPolicyId!=-99999)
                        {
                            PolicyStatus? _policyStatus = _policymanager.GetPolicyStatusByCust(_intPolicyId);
                            if (null == _policyStatus)
                            {
                                _resp.Status = 0;
                                _resp.Message = "错误!相关保单的状态不正确";
                                return Json(_resp);
                            }
                            _resp = _policyfilemanager.DeleteById(Session["username"].ToString(), _policyStatus, id);
                            return Json(_resp);


                        }

                        _resp = _policyfilemanager.DeleteById(Session["username"].ToString(),  id);
                        
                        return Json(_resp);
                    }



                    #endregion


         //改
         #region //改
                
                
                /// <summary>
                /// 根据ID获取保单内容
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public ActionResult Edit(int id = 0)
                {
                    Response _resp=new Response();

                     _resp = _policymanager.Edit(Session["username"].ToString(), id);
                    if (_resp.Status == 0)
                    {
                        //_resp.Status=0;
                       // _resp.Message = "错误！由于归属或权限问题不能修改该保单";
                        _resp.Data = null;
                        //return Json(_resp,JsonRequestBehavior.AllowGet);
                        return View("_Error", _resp);
                    }
                    Policy _policy = _resp.Data;

                    List<PolicyFile> _listPolicyFiles=_policyfilemanager.GetPolicyListById(_resp.Data.PolicyId);
                    //int _inttmp = _listPolicyFiles.Count;
                    //获取保单相关的文件信息,并加载到VIEWBAG中
                    if(_listPolicyFiles.Count > 0)
                    {
                        ViewBag.PolicyFileList = _listPolicyFiles;
                    }
                    else
                    {
                        ViewBag.PolicyFileList = null;
                    }
                        
                    

                    string _strPIcname = "";
                    foreach (PolicyFile item in _listPolicyFiles)
                    {
                        _strPIcname += item.PolicyFileId;
                    }

                    ViewBag.picName = _strPIcname;

                    ViewBag.APolicyErrReasonList = _unPolicyReasonManager.GetAllPolicyErrListByCust(Session["username"].ToString(), _policy.PolicyId);

                    return View(_resp.Data);

                }
                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Edit(FormCollection fc, int id = 0)
                {
                    Response _resp = new Response();
                    Policy _policy = null;
                    _resp = _policymanager.Edit(Session["username"].ToString(), id);
                    if (_resp.Status == 0)
                    {
                        _resp.Data = null;
                        return Json(_resp);
                    }
                    _policy = _resp.Data;
                    _resp.Data = null;
                    if (!TryUpdateModel(_policy, new string[] { "CorporateName", "Contacts", "UserType", "IDCardType", "IDCardNum", "Phone", "PolicyContent", "Province", "City", "Town", "Address", "BxCorporateName", "BxType", "BxIndustry", "PolicyDeadline", "BxAmount", "BxPubRate", "PayAmount", "PaperPolicy", "PolRemark", "PolicyAnnex", "isBxClauses" }))
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                        _resp.Data = null;
                        return Json(_resp);

                    }
                    //缺文件上传功能
                    //把状态改成待审核
                   // _policy.Status = PolicyStatus.WaitPolicy;


                    _resp = _policymanager.ChangeToWaitPolicy(Session["username"].ToString(), _policy,id);
                    if(_resp.Status==0)
                    {
                        return Json(_resp);

                    }



                    string _strPolicyFilesId = "";

                    if (string.IsNullOrEmpty(Request["picName"]) || string.IsNullOrWhiteSpace(Request["picName"]))
                    {

                        _strPolicyFilesId = "";
                    }
                    else
                    {
                        _strPolicyFilesId = Request["picName"].Trim();
                    }
                    
                    _resp = _policymanager.Update(_policy, _strPolicyFilesId, Session["username"].ToString());

                   
                    _resp.Data = null;

                    return Json(_resp);
 
                }
                #endregion


         //详
         #region //详


                public ActionResult Detail(int id = 0)
                {
                    Response _resp = new Response();

                    Policy _policy = _policymanager.GetDetailByCustomer(Session["username"].ToString(), id);
                    if (_policy == null)
                    {
                        _resp.Status = 0;
                        _resp.Message = "错误！由于归属或权限问题不能查看该保单";
                        return View("error", _resp);
                    }

                    //获取保单相关的文件信息,并加载到VIEWBAG中
                    ViewBag.PolicyFileList = _policyfilemanager.GetPolicyListById(_policy.PolicyId).Count>0?_policyfilemanager.GetPolicyListById(_policy.PolicyId):null;
                    //获取相关表单的审核失败原因,并加载到viewbag中
                    ViewBag.APolicyErrReasonList = _unPolicyReasonManager.GetAllPolicyErrListByCust(Session["username"].ToString(), _policy.PolicyId);
                    //获取相关表单的退保失败原因,并加载到viewbag中
                    ViewBag.APolicyBackErrList = _unPolicyReasonManager.GetAllPolicyBackErrListByCust(_policy.PolicyId);
                    //获取相关保单退何原因
                    ViewBag.PolicyBackReasonList = _policyBackReasonManager.GetAPolicyWaitBackListByCustomer(Session["username"].ToString(),_policy.PolicyId);
                    //获取保单变更原因
                    ViewBag.PolicyChangeReasonList = _policyChangeReasonManager.GetPolicyChangeReasonByCustomer(Session["username"].ToString(), _policy.PolicyId);
                    //获取相关表单的变更失败原因,并加载到viewbag中
                    ViewBag.APolicyChangedErrList = _unPolicyReasonManager.GetAllPolicyChangedErrListByCust(Session["username"].ToString(), _policy.PolicyId);

                    //获取保单变更a端回单列表

                    ViewBag.PolicyChangedAnnexList = _policyChangedAnnexMannager.GetPolicyChangedAnnexlst(_policy.PolicyId).Count > 0 ? _policyChangedAnnexMannager.GetPolicyChangedAnnexlst(_policy.PolicyId) : null;


                    //获取保单退保a端回单列表

                    ViewBag.PolicyBackAnnexList = _policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId).Count > 0 ?_policyBackAnnexManager.GetPolicyBackAnnexlst(_policy.PolicyId) : null;

                    return View(_policy);
                }
            #endregion


         //列
         #region //列
                
                
                public ActionResult List()
                {
                    return View();
                }
                [HttpPost]
                public ActionResult List(string search, int limit, string sortname, int pageNumber, string order)
                {



                    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                    //Paging<PolicyViewModel> _pagingCustomerView = new Paging<PolicyViewModel>();

                    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                    else _pagingCustomer.PageIndex = 1;
                    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                    else _pagingCustomer.PageSize = 10;

                    string listwhat = "";
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);
                    //_pagingCustomerView = _pagingCustomer;
                    


                    

                    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                }

                //重写部分

                        //全部待审核
                        [HttpPost]
                        public ActionResult List_AllWaitPolicy(string search, int limit, string sortname, int pageNumber, string order)
                        {
                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allwaitpolicy";
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                        }



                        //全部未通过
                        [HttpPost]
                        public ActionResult List_AllPolicyErr(string search, int limit, string sortname, int pageNumber, string order)
                        {
                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allpolicyerr";
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                        }


                        //全部待审核退保
                        [HttpPost]
                        public ActionResult List_AllPolicyWaitBack(string search, int limit, string sortname, int pageNumber, string order)
                        {
                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allpolicywaitback";
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
 
                        }


                        //列审核退保失败（admin和bx）
                        [HttpPost]
                        public ActionResult List_AllPolicyBackErr(string search, int limit, string sortname, int pageNumber, string order)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allpolicybackErr";
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                        }


                        //待审核变更（a、b）
                        [HttpPost]
                        public ActionResult List_AllPolicyWaitChange(string search, int limit, string sortname, int pageNumber, string order)
                        {
                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allpolicywaitchange";
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
 
                        }

                        //列审核变更失败（admin和bx）
                        [HttpPost]
                        public ActionResult List_AllPolicyChangedErr(string search, int limit, string sortname, int pageNumber, string order)
                        {

                            Paging<Policy> _pagingCustomer = new Paging<Policy>();
                            if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                            else _pagingCustomer.PageIndex = 1;
                            if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                            else _pagingCustomer.PageSize = 10;

                            string listwhat = "list_allpolicychangederr";
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                            var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                            return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                        }



               //重写部分结束

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                }
                //列审核失败的
                [HttpPost]
                public ActionResult List_APolicyErr(string search, int limit, string sortname, int pageNumber, string order)
                {

                    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                    //如果排序为空，代表默认排序，指定此API默认排序字段
                    if (null == sortname || string.IsNullOrEmpty(sortname.Trim()))
                    {
                        sortname = "APolicyErrTime";

                    }
                    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                    else _pagingCustomer.PageIndex = 1;
                    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                    else _pagingCustomer.PageSize = 10;

                    string listwhat = "list_apolicyerr";
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                }

                //列审核失败（admin和bx）
                //[HttpPost]
                //public ActionResult List_AllPolicyErr(string search, int limit, string sortname, int pageNumber, string order)
                //{

                //    Paging<Policy> _pagingCustomer = new Paging<Policy>();
                //    if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                //    else _pagingCustomer.PageIndex = 1;
                //    if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                //    else _pagingCustomer.PageSize = 10;

                //    string listwhat = "list_allpolicyerr";
                //    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                //    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                //}


                
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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

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
                    var _paging = _policymanager.FindPageListByCustomer(_pagingCustomer, Session["username"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                    return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                }
                




                
                



                #endregion
        
        ////变更
        // #region //变更
                
                
        //        /// <summary>
        //        /// 由退回变更为等待审批，由客户端提交
        //        /// </summary>
        //        /// <param name="id"></param>
        //        /// <returns></returns>
        //        public ActionResult SubmitAgin(int id = 0)
        //        {
        //            Response _resp = new Response();

        //            _resp = _policymanager.ChangeToWaitPolicy(Session["username"].ToString(), id);

        //            return Json(_resp);
        //        }







                
        //        #endregion
                    

        //付款    目前取消
        #region //付款

                //[HttpPost]
                //public ActionResult Pay(int pid = 0)
                //{
                //    Response _resp = new Response();
                //    _resp = _policymanager.PolicyPayByCustmoer(Session["username"].ToString(), pid);
                //    return Json(_resp);
                //}

                #endregion

        //待退保
        #region 待退保
                /// <summary>
                /// 
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                [HttpPost]
                public ActionResult PolicyWaitBack(FormCollection fc, int id = 0)
                {
                    Response _resp = new Response();
                    PolicyBackReason _policyWaitBackReason = new PolicyBackReason();
                    //校验变更原因
                    if (!TryUpdateModel(_policyWaitBackReason, new string[] { "BackReason" }))
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                        _resp.Data = null;
                        return Json(_resp);
                    }

                    _resp = _policymanager.ChangeToPolicyWaitBack(Session["username"].ToString(), _policyWaitBackReason, id);

                    return Json(_resp);
                }
                #endregion

        //待变更
                [HttpPost]
                public ActionResult PolicyWaitChange(FormCollection fc, int id = 0)
                {
                    Response _resp = new Response();
                    PolicyChangeReason _policyChangeReason = new PolicyChangeReason();
                    //校验变更原因
                    if (!TryUpdateModel(_policyChangeReason, new string[] { "ChangeReason", "ChangeText" }))
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                        _resp.Data = null;
                        return Json(_resp);
                    }

                    _resp = _policymanager.ChangeToPolicyWaitChange(Session["username"].ToString(), _policyChangeReason, id);

                    return Json(_resp);
 
                }


           //供
          #region //供
                
                
                /// <summary>
                /// 根据保险产品ID获得保险条款内容
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                public string GetBxClauses(int id = 0)
                {
                    return _custpromanager.GetBxClauses(id);
                }

                /// <summary>
                /// 取可用（保险产品已上架）的保险公司名称
                /// </summary>
                /// <returns></returns>
                [HttpPost]
                public ActionResult GetUseableBxCorporateName()
                {
                    Response _resp = new Response();
                    List<string> listresult = null;
                    listresult = _custpromanager.GetPublicedBxCorporateName();
                    if (listresult.Count > 0)
                    {
                        _resp.Status = 1;
                        _resp.Data = listresult;
                        return Json(_resp);

                    }
                    _resp.Message = "获取保险公司名称错误";
                    return Json(_resp);


                }
                /// <summary>
                /// 取可用（保险产品已上架）的保险行业
                /// </summary>
                /// <param name="bxcorpname"></param>
                /// <returns></returns>

                [HttpPost]
                public ActionResult GetUseableBxIndustry(string bxcorpname)
                {
                    Response _resp = new Response();

                    List<string> listresult = null;
                    listresult = _custpromanager.GetPublicedBxIndustry(bxcorpname);
                    if (listresult.Count > 0)
                    {
                        _resp.Status = 1;
                        _resp.Data = listresult;
                        return Json(_resp);

                    }
                    _resp.Message = "获取保险行业错误";
                    return Json(_resp);

                }


                /// <summary>
                /// 取可用（保险产品已上架）的保险类型
                /// </summary>
                /// <param name="bxcorpname"></param>
                /// <param name="bxindustry"></param>
                /// <returns></returns>

                [HttpPost]

                public ActionResult GetUseableBxtype(string bxcorpname, string bxindustry)
                {
                    Response _resp = new Response();

                    List<string> listresult = null;
                    listresult = _custpromanager.GetPublicedBxtype(bxcorpname, bxindustry);
                    if (listresult.Count > 0)
                    {
                        _resp.Status = 1;
                        _resp.Data = listresult;
                        return Json(_resp);

                    }
                    _resp.Message = "获取保险种类错误";
                    return Json(_resp);


                }
                
                /// <summary>
                /// 根据保险公司名称,保险行业,保险类型,返回保险费率列表
                /// </summary>
                /// <param name="bxcorpname"></param>
                /// <param name="bxindustry"></param>
                /// <param name="bxtype"></param>
                /// <returns></returns>
                 [HttpPost]
                public ActionResult GetUseableBxar(string bxcorpname, string bxindustry, string bxtype)
                {

                    Response _resp = new Response();

                    List<object> listresult = null;
                    listresult = _custpromanager.GetPublicedBxar(bxcorpname, bxindustry,bxtype);
                    if (listresult.Count > 0)
                    {
                        _resp.Status = 1;
                        _resp.Data = listresult;
                        return Json(_resp);

                    }
                    _resp.Message = "获取保险费率错误";
                    return Json(_resp);
 
                }

                /// <summary>
                /// 返回各种状态的保单统计
                /// </summary>
                /// <returns></returns>
                [HttpPost]
                 public ActionResult Count()
                 {
                     return Json(_policymanager.CountByCustermer(Session["username"].ToString()));
                 }
                #endregion


        //导policy
                [HttpPost]
                public ActionResult ExportSuccess()
                {
                    Response _resp = new Response();
                    _resp=_policymanager.ExportPolicyListByCust(Session["username"].ToString(), PolicyStatus.Policyed);
                    if(_resp.Status==0)
                    {
                        return Json(_resp);
                    }

                    byte[] filecontent = _resp.Data;
                    _resp.Data = null;
                    string strFilename = DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".xlsx";
                    return File(filecontent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", strFilename);
                }
               

        //回单明细

                public ActionResult PolicyeleDetail(int id=0)
                {
                    Response _resp = new Response();
                    Policy _policy = _policymanager.GetDetailByCustomer(Session["username"].ToString(), id);
                    if (_policy == null)
                    {
                        _resp.Status = 0;
                        _resp.Message = "错误！由于归属或权限问题不能查看该保单";
                        return View("error", _resp);
                    }

                    int inttmp = _policyEleManager.GetPolicyEleListByIdByCust(_policy.PolicyId).Count;
                    //获取保单回单列表
                    ViewBag.PolicyEleList = _policyEleManager.GetPolicyEleListByIdByCust(_policy.PolicyId);
                    ViewBag.PolicyChangedAnnexByBx = _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id) != null ? _policyChangedAnnexByBxManager.GetPolicyChangedAnnexByBxlst(id) : null;
                    return View();
                }





                //根据权限显示文件内容
                #region //根据权限显示文件内容

                /// <summary>
                /// 显示保单回单
                /// </summary>
                /// <param name="policyEleId"></param>
                /// <returns></returns>
                public ActionResult GetPolicyEle(int policyEleId = 0)
                {
                    string strFilePath = _policyEleManager.GetPolicyElePathByCustomer(Session["username"].ToString(), policyEleId);

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
                    string strFilePath = _policyChangedAnnexMannager.GetPolicyChangedAnnexPathByCustomer(Session["username"].ToString(), policyChangedAnnexId);

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
                    string strFilePath = _policyBackAnnexManager.GetPolicyBackAnnexPathByCustomer(Session["username"].ToString(), PolicyBackAnnexId);

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
