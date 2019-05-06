using BC.DDD.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject.Domain
{
    public class Attachment : EntityWithGuid
    {
        public  Attachment()
        {
            this.CreateDate = DateTime.Now;
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        public DateTime CreateDate { get; private set; }

        /// <summary>
		/// Order
		/// </summary>
		public int Order { get; set; }

        /// <summary>
        /// Default
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// RelationId
        /// </summary>
        public Guid? RelationId { get; set; }

        /// <summary>
        /// Ext
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// Directory
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public int Size { get; set; }
    }
}
