using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bx_Core;

namespace Bx_Web.Controllers
{
    public class patchController : Controller
    {
       
        private CustomerManager _customerManager = new CustomerManager();
        private PolicyManager _policyManager = new PolicyManager();
        private ReChargeManager _rechargeManager = new ReChargeManager();
        //
        // GET: /patch/
        public ActionResult patcharea()
        {
            List < Policy > _policyAll = _policyManager.FindList().ToList<Policy>();
            string _area = "";
            string _custCorporateName = "";
            string _custUserName = "";

            foreach (Policy _policy in _policyAll)
            {
                _custUserName = _policy.Creater;
                _area = _customerManager.GetAreaOnUserName(_custUserName);
                _custCorporateName = _customerManager.GetCorporateNameOnUserName(_custUserName);
                _policy.Area = _area;
                _policy.CustomerCorporateName = _custCorporateName;
                _policyManager.Update(_policy);
                

            }
            return View();
        }
        /// <summary>
        /// 
        /// 为recharge表打补丁，根据rechargeType的endwith取policyNumber,对比后，把policyId赋值
        /// </summary>
        /// <returns></returns>
        public ActionResult PatchRechargePolicyId()
        {
            List<ReCharge> _rechargeSearch =null; 
            List<Policy> _policyAll = _policyManager.FindList().ToList<Policy>();
            foreach (Policy _policyItem in _policyAll)
            {
                _rechargeSearch=_rechargeManager.FindList().Where(a=>a.ReChargeType.ToString().ToLower().EndsWith(_policyItem.PolicyNumber.ToLower())).ToList<ReCharge>();
                if(null!=_rechargeSearch && _rechargeSearch.Count()>0)
                {
                    foreach (ReCharge _rechargeItem in _rechargeSearch)
                    {
                        _rechargeItem.PolicyId = _policyItem.PolicyId;
                        _rechargeManager.Update(_rechargeItem);

                    }

                }
               

            }
            return View();
        }
	}
}