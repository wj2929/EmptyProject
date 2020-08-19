using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class UserAccessToken : EntityWithGuid
    {

        public UserAccessToken()
        {
            this.CreateDate = DateTime.Now;
        }


        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }


        /// <summary>
        /// Expires
        /// </summary>
        public DateTime? Expires { get; set; }
    }
}
