using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EmptyProject.Manage.Models
{
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }

    public class ModifyPasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string RePassword { get; set; }
    }
}