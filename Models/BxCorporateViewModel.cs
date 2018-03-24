using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Bx_Web.Models
{
    public class BxCorporateViewModel
    {
        
        [Required(ErrorMessage="{0} 未添写")]
        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]{2,20}$", ErrorMessage = "必须由2-20个字符组成")]
        [Display(Name = "保险公司名称")]
        public string BxCorporateName { get; set; }

        [Display(Name = "备注")]


        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]{0,300}$", ErrorMessage = "必须由0-300个字符组成")]
        public string BxCorRemark { get; set; }

      


    }
}