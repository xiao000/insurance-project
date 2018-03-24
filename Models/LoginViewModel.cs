using System.ComponentModel.DataAnnotations;

namespace Bx_Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "必须输入{0}")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "必须由6-16个英文数字组成")]

        [Display(Name = "用户名")]
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [Display(Name = "密码")]
        [RegularExpression("^[A-Za-z0-9]{6,16}$", ErrorMessage = "必须由6-16个英文数字组成")]
        [DataType(DataType.Password)]
        public string password { get; set; }


        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码")]

        [RegularExpression("^[A-Za-z0-9]{4}$", ErrorMessage = "验证码错误")]
        [Required(ErrorMessage = "验证码错误")]
        public string validatecode { get; set; }
    }
}