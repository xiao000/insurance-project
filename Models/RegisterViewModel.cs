using System;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace Bx_Web.Models
{
    /// <summary>
    /// 登录模型
    /// <remarks>
    /// 创建：2017.11.25
    /// </remarks>
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "必须输入{0}")]
        [RegularExpression("^[A-Za-z0-9_]{6,16}$", ErrorMessage = "必须由6-16个英文数字符组成")]
        [Remote("ExitsUser", "Account")]
        [Display(Name = "用户名")]
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "{0}未填写")]
        [Display(Name = "密码")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "必须由6-16个英文数字组成")]
        [DataType(DataType.Password)]
        
        public string password { get; set; }

        /// <summary>
        /// 密码确认
        /// </summary>
        [Required(ErrorMessage = "{0}未填写")]
        [Display(Name = "确认密码")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "必须由6-16个英文数字组成")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "密码输入不一致")]
        public string repassword { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Required(ErrorMessage = "{0}未填写")]
        [RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]{2,20}$", ErrorMessage = "必须由2-20个字符组成")]
        [Display(Name = "联系人")]
        public string Contacts { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        /// 
        [Required(ErrorMessage = "{0}未填写")]
        [Remote("ExitsPhone", "Account")]
        [Display(Name = "手机号码")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "手机号码不正确")]
        public string Phone { get; set; }

        /// <summary>
        /// EMAIL
        /// </summary>
        /// 
        [Display(Name = "EMAIL")]
        [DataType(DataType.EmailAddress)]
        public string Email{get;set;}

        /// <summary>
        /// 公司名称
        /// </summary>
        /// 
        [Display(Name = "公司名称")]
        //[RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]{0,50}$", ErrorMessage = "最多50个字符")]
        [MaxLength(50,ErrorMessage="最多50个字符")]
        public string CorporateName{get;set;}


        /// <summary>
        /// 详细地址
        /// </summary>
        [Display(Name = "详细地址")]
        //[RegularExpression("^[\u4E00-\u9FA5A-Za-z0-9_]{0,50}$", ErrorMessage = "最多50个字符")]
        [MaxLength(50, ErrorMessage = "最多50个字符")]
        public string Address{get;set;}


        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码")]
        [RegularExpression("^[A-Za-z0-9]{4}$", ErrorMessage = "验证码错误")]
        public string validatecode{get;set;}


        /// <summary>
        /// 记住我
        /// </summary>
        ///[Display(Name = "记住我")]
       /// public bool RememberMe { get; set; }
       


        /// [Display(Name = "负责区域")]
        [DisplayName("负责区域")]
        [MaxLength(50, ErrorMessage = "最多50个字符")]
        public string Area { get; set; }
    }
}