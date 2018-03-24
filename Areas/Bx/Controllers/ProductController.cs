using Bx_Common;
using Bx_Core;
using Bx_Core.Author;
using Bx_Core.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using Microsoft.Security.Application;


namespace Bx_Web.Areas.Bx.Controllers
{
    [BxLoggedAuthor]
    public class ProductController : Controller
    {

        ProductManger _productmanager = new ProductManger();
        BxIndustryManager _bxindustrymanager = new BxIndustryManager();
        BxTypeManager _bxtypemanager = new BxTypeManager();
        ProductLogManager _productLogManager = new ProductLogManager();

        //列
        #region //列


                    public ActionResult List()
                    {
                        return View();
                    }

                    [HttpPost]
                    public ActionResult List(string search, int limit, string sortname, int pageNumber, string order)
                    {
                        Paging<Product> _pagingCustomer = new Paging<Product>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "";
                        var _paging = _productmanager.FindPageList(_pagingCustomer, Session["bxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });
                    }



                            /// <summary>
                            /// 列出未上架
                            /// </summary>
                            /// <param name="search"></param>
                            /// <param name="limit"></param>
                            /// <param name="sortname"></param>
                            /// <param name="pageNumber"></param>
                            /// <param name="order"></param>
                            /// <returns></returns>
                    [HttpPost]
                    public ActionResult List_UnPub(string search, int limit, string sortname, int pageNumber, string order)
                    {
                        Paging<Product> _pagingCustomer = new Paging<Product>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "List_UnPub";
                        var _paging = _productmanager.FindPageList(_pagingCustomer, Session["bxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }



                            /// <summary>
                    /// 列出上架未通过
                    /// </summary>
                    /// <param name="search"></param>
                    /// <param name="limit"></param>
                    /// <param name="sortname"></param>
                    /// <param name="pageNumber"></param>
                    /// <param name="order"></param>
                    /// <returns></returns>
                    public ActionResult List_PubErr(string search, int limit, string sortname, int pageNumber, string order)
                    {
                        Paging<Product> _pagingCustomer = new Paging<Product>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "List_PubErr";
                        var _paging = _productmanager.FindPageList(_pagingCustomer, Session["bxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }



                            /// <summary>
                            /// 列出正在审核中的保险合同
                            /// </summary>
                            /// <param name="search"></param>
                            /// <param name="limit"></param>
                            /// <param name="sortname"></param>
                            /// <param name="pageNumber"></param>
                            /// <param name="order"></param>
                            /// <returns></returns>
                    public ActionResult List_Waitpub(string search, int limit, string sortname, int pageNumber, string order)
                    {
                        Paging<Product> _pagingCustomer = new Paging<Product>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "List_Waitpub";
                        var _paging = _productmanager.FindPageList(_pagingCustomer, Session["bxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }




                            /// <summary>
                    /// 列出已经上架的
                    /// </summary>
                    /// <param name="search"></param>
                    /// <param name="limit"></param>
                    /// <param name="sortname"></param>
                    /// <param name="pageNumber"></param>
                    /// <param name="order"></param>
                    /// <returns></returns>
                    public ActionResult List_Publiced(string search, int limit, string sortname, int pageNumber, string order)
                    {
                        Paging<Product> _pagingCustomer = new Paging<Product>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "List_Publiced";
                        var _paging = _productmanager.FindPageList(_pagingCustomer, Session["bxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }



                            /// <summary>
                            /// 列出已被禁用的
                            /// </summary>
                            /// <param name="search"></param>
                            /// <param name="limit"></param>
                            /// <param name="sortname"></param>
                            /// <param name="pageNumber"></param>
                            /// <param name="order"></param>
                            /// <returns></returns>
                    public ActionResult List_Forbid(string search, int limit, string sortname, int pageNumber, string order)
                    {
                        Paging<Product> _pagingCustomer = new Paging<Product>();
                        if (pageNumber > 0) _pagingCustomer.PageIndex = (int)pageNumber;
                        else _pagingCustomer.PageIndex = 1;
                        if (limit > 0) _pagingCustomer.PageSize = (int)limit;
                        else _pagingCustomer.PageSize = 10;

                        string listwhat = "List_Forbid";
                        var _paging = _productmanager.FindPageList(_pagingCustomer, Session["bxCorporateName"].ToString(), search, listwhat, _pagingCustomer.PageIndex, _pagingCustomer.PageSize, sortname, order);

                        return Json(new { total = _paging.TotalNumber, rows = _paging.Items });

                    }



        #endregion

         //增
         #region //增



                        /// <summary>
                        /// 添加页面
                        /// </summary>
                        /// <returns></returns>
                public ActionResult Add()
                {
                    ViewBag.bxCorporateName = Session["bxCorporateName"];

                    return View();
                }




                [HttpPost]
                [ValidateInput(false)]
                public ActionResult Add(FormCollection fc, ICollection<Bxar> bxars)
                {
                    Response _resp = new Response();
                    Product _product = new Product();
                    _product.BxCreateCorporateName = Session["BxCorporateName"].ToString();
                                        
                    if (!TryUpdateModel(_product, new string[] { "BxIndustry", "BxType", "BxRemark", "BxClauses" }))
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                        return Json(_resp);

                    }
                    
                    List<Bxar> _bxars = new List<Bxar>();

                    if (!TryUpdateModel(_bxars, fc))
                    {
                        _resp.Status = 0;
                        _resp.Message = General.GetModelErrorString(ModelState);
                        return Json(_resp);

                    }




                    _resp = _productmanager.add(_product, _bxars, HttpContext.Session);
                    if (_resp.Status == 1)
                    {
                        //添加成功,写入日志
                        _productLogManager.AddUnPublicLog(Session["BxCorporateName"].ToString(), "添加保险产品"+((int)_resp.Data).ToString()+"; 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                        _resp.Message = "恭喜!添加产品成功!";
                    }
                    return Json(_resp);



                }


                #endregion

         //改

                #region //改


                        public ActionResult Edit(int id = 0)
                        {
                            Response _resp = new Response();
                            _resp = _productmanager.CheckIsSon(id, Session["bxCorporateName"].ToString());
                            if (_resp.Status == 0)
                            {
                                _resp.Data = null;
                                return Json(_resp, JsonRequestBehavior.AllowGet);

                            }


                            //_resp.data下是PRODUCT类数据
                            //获取费率

                            Product _product = _resp.Data as Product;

                            //根据合同状态，确认能否修改

                            if (!_productmanager.CheckCanEdit(_product))
                            {
                                _resp.Status = 0;
                                _resp.Data = null;
                                _resp.Message = "错误！只有未上架的保险合同才能被修改！";
                                return Json(_resp, JsonRequestBehavior.AllowGet);
                            }
                            ViewBag.Bxars = _productmanager.GetBxarList(_product.ProductId);

                            ViewBag.UnPubReason = _productmanager.GetUnPubReason(_product.ProductId);

                            ViewBag.BxIndustry = new SelectList(_bxindustrymanager.GetSelect().Select(a => a.BxIndustryName).ToArray());
                            ViewBag.BxType = new SelectList(_bxtypemanager.GetSelect().Select(a => a.BxTypeName).ToArray());

                            return View(_resp.Data);


                        }

                        [ValidateInput(false)]
                        [HttpPost]
                        public ActionResult Edit(FormCollection fc, ICollection<Bxar> bxar, int id = 0)
                        {
                            Response _resp = new Response();
                            //判断归属
                            _resp = _productmanager.CheckIsSon(id, Session["bxCorporateName"].ToString());
                            if (_resp.Status == 0)
                            {
                                _resp.Data = null;
                                return Json(_resp);

                            }
                            Product _product = _resp.Data;

                            if (!TryUpdateModel(_product, new string[] { "BxRemark", "BxClauses" }))
                            {
                                _resp.Status = 0;
                                _resp.Message = General.GetModelErrorString(ModelState);
                                _resp.Data = null;
                                return Json(_resp);

                            }

                            List<Bxar> _bxars = new List<Bxar>();

                            if (!TryUpdateModel(_bxars, fc))
                            {
                                _resp.Status = 0;
                                _resp.Message = General.GetModelErrorString(ModelState);
                                _resp.Data = null;
                                return Json(_resp);

                            }


                            _resp = _productmanager.Update(_product, _bxars, HttpContext.Session);

                            if (_resp.Status == 1)
                            {
                                //修改的日志
                                _productLogManager.AddEditLog(Session["BxCorporateName"].ToString(), "修改保险产品" + ((int)_resp.Data).ToString() + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
                                _resp.Message = "恭喜！修改保险合同成功";
                            }



                            return Json(_resp);
                        }

                #endregion


        public ActionResult UploadImage()
        {
            Response _resp = new Response();
            string strresult = "";
             String callback = System.Web.HttpContext.Current.Request.QueryString["CKEditorFuncNum"].ToString();  
            UploadConfig _uploadConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("UploadConfig") as Bx_Core.Config.UploadConfig;
             HttpFileCollection files =System.Web.HttpContext.Current.Request.Files;
             for (int iFile = 0; iFile < files.Count; iFile++)
             {
                 HttpPostedFile postedFile = files[iFile];
                 _resp = _productmanager.UploadImg(_uploadConfig, postedFile, "image");
                 if (_resp.Status == 0)
                 {
                     strresult+="<script>alert(' "+ _resp.Message+" ')</script>";
                 }
                 else 
                 {
                     _resp.Data ="http://"+ System.Web.HttpContext.Current.Request.Url.Host.ToString() + ":" + System.Web.HttpContext.Current.Request.Url.Port.ToString()+_resp.Data;
                     strresult+="<script type=\"text/javascript\">";
                     strresult+="window.parent.CKEDITOR.tools.callFunction(" + callback  + ",'" + _resp.Data + "','')";
                     strresult+=("</script>");  
                    
                 }

             }
            
          return Content(strresult);
       
            
           
        }

        //删除
        [HttpPost]
        public ActionResult Delete(int id=0)
        {
            Response _resp = new Response();
            _resp = _productmanager.DeleteByid(id, Session["bxCorporateName"].ToString());
            //删除的日志
            _productLogManager.AddEditLog(Session["BxCorporateName"].ToString(), "删除保险产品:" + ((int)_resp.Data).ToString() + " 使用的ip" + Request.UserHostAddress.ToString(), Request.Url.ToString());
            return Json(_resp);
            


        }

        //详情 
        public ActionResult Detail(int id=0)
        {
            Response _resp = new Response();
            _resp = _productmanager.CheckIsSon(id, Session["bxCorporateName"].ToString());
            if (_resp.Status == 0)
            {
                return Json(_resp, JsonRequestBehavior.AllowGet);

            }

            //_resp.data下是PRODUCT类数据
            //获取费率

            Product _product = _resp.Data as Product;
            ViewBag.Bxars = _productmanager.GetBxarList(_product.ProductId);
            ViewBag.UnPubReason = _productmanager.GetUnPubReason(_product.ProductId);
            ViewBag.BxIndustry = new SelectList(_bxindustrymanager.GetSelect().Select(a => a.BxIndustryName).ToArray());
            ViewBag.BxType = new SelectList(_bxtypemanager.GetSelect().Select(a => a.BxTypeName).ToArray());

            return View(_resp.Data);

 
        }

        public ActionResult WaitPub(int id = 0)
        {
            Response _resp = new Response();
            _resp = _productmanager.WaitPub(Session["BxCorporateName"].ToString(), id);
            if (_resp.Status == 1)
            {
                _resp.Message = "恭喜！保险合同申请上架成功，敬请等待管理员审核";
            }
            return Json(_resp);
        
        }





        [HttpPost]
        public ActionResult Count()
        {
            Response _resp = new Response();
            _resp = _productmanager.Count(Session["BxCorporateName"].ToString());
            return Json(_resp);
        }
            
        }
	}
