using System.ComponentModel.DataAnnotations;

namespace Bx_Web.Models
{
    public class ChangePwdViewModel
    {
         /// <summary>
        /// 原始密码
        /// </summary>
        [Required(ErrorMessage = "原始密码未填")]
        [Display(Name = "原始密码")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "原始密码必须由6-16个英文数字组成")]
        [DataType(DataType.Password)]
        public string Oldpwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码未填")]
        [Display(Name = "新密码")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "新密码必须由6-16个英文数字组成")]
        [DataType(DataType.Password)]
        
        
        public string Newpwd { get; set; }

        /// <summary>
        /// 新密码确认
        /// </summary>
        [Required(ErrorMessage = "新密码未填")]
        [Display(Name = "新密码确认")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "新密码必须由6-16个英文数字组成")]
        [Compare("Newpwd")]
        [DataType(DataType.Password)]
        public string Repwd { get; set; }
    }
}