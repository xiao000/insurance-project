using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace Bx_Web.Models
{
    public class EditCustomerViewModel
    {
        

        

        /// <summary>
        /// 联系人
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9_]{2,20}$", ErrorMessage = "必须由2-20个字符组成")]
        [Display(Name = "联系人")]
        public string Contacts { get; set; }

        

        /// <summary>
        /// EMAIL
        /// </summary>
        /// 
        [Display(Name = "EMAIL")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        /// 
        [Display(Name = "公司名称")]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9]{0,50}$", ErrorMessage = "最多50个字符")]
        public string CorporateName { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [Display(Name = "详细地址")]
        [RegularExpression(@"^[\u4E00-\u9FA5A-Za-z0-9]{0,50}$", ErrorMessage = "最多50个字符")]
        public string Address { get; set; }


        /// <summary>
        /// 所属区域
        /// </summary>
        [Display(Name = "所属区域")]
        [DisplayName("所属区域")]
        [MaxLength(50, ErrorMessage = "最多50个字符")]
        public string Area { get; set; }
    }
}