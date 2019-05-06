using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EmptyProject.Domain;
using BC.Core;
using BC.DDD;
using EmptyProject.Domain.QueryObject;

namespace EmptyProject.DomainService.Interface
{
	public interface IAttachmentDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		Attachment AddAttachment(Attachment Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<Attachment> AddAttachments(IList<Attachment> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditAttachment(Attachment Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		Attachment Single(Guid Id);

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		void Remove(Guid Id);

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		void Removes(Guid[] Ids);

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		void Removes(AttachmentCriteria AttachmentCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<Attachment> All();

		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		bool IsExist(Guid Id);

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		bool IsExist(AttachmentCriteria AttachmentCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="AttachmentCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<Attachment> GetAttachmentPaging(AttachmentCriteria AttachmentCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(AttachmentCriteria AttachmentCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="AttachmentCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<Attachment> GetList(AttachmentCriteria AttachmentCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        IList<Attachment> GetAttachments(Guid[] Ids);

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="RelationId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        IList<Attachment> GetAttachments(Guid RelationId, string Type);

        /// <summary>
        /// 设置默认附件
        /// </summary>
        /// <param name="Id"></param>
        void SetDefault(Guid Id);

        /// <summary>
        /// 取消默认附件
        /// </summary>
        /// <param name="Ids"></param>
        void CancelDefaults(Guid[] Ids);

        /// <summary>
        /// 取消默认附件
        /// </summary>
        /// <param name="Ids"></param>
        void CancelDefault(Guid Id);

        /// <summary>
        /// 关联附件
        /// </summary>
        /// <param name="RelationId"></param>
        /// <param name="Ids"></param>
        void RelationAttachments(Guid RelationId, Guid[] Ids);

        /// <summary>
        /// 取消关联附件
        /// </summary>
        /// <param name="RelationId"></param>
        /// <param name="Type"></param>
        void CancelRelationAttachments(Guid RelationId, string Type);

        /// <summary>
        /// 清理孤立附件列表（未被关联RelationId的数据）
        /// </summary>
        /// <returns></returns>
        void CleanIsolationAttachments();

        /// <summary>
        /// 改变指定Id的RelationId、Type
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ChanageRelationId"></param>
        /// <param name="ChangeType"></param>
        void Change(Guid Id, Guid ChanageRelationId, string ChangeType);
	}
}
