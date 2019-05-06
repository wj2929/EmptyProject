using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using BC.Core;
using BC.DDD;
using BC.DDD.Specification;
using EmptyProject.Domain;
using EmptyProject.Store.Interface;
using EmptyProject.DomainService.Interface;
using EmptyProject.Domain.QueryObject;

namespace EmptyProject.DomainService
{
	internal class AttachmentDomainService : BaseDomainService, IAttachmentDomainService
	{ 
		public AttachmentDomainService(IAttachmentStore AttachmentStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.AttachmentStore = AttachmentStore;
		}
		private readonly IAttachmentStore AttachmentStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public Attachment AddAttachment(Attachment AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.AttachmentStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<Attachment> AddAttachments(IList<Attachment> Infos)
		{
            Infos.ForEach(t => 
            {
                this.AttachmentStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditAttachment(Attachment Info)
		{
			this.AttachmentStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public Attachment Single(Guid Id)
		{
			return this.AttachmentStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.AttachmentStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.AttachmentStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(AttachmentCriteria AttachmentCriteria)
		{
			this.AttachmentStore.Remove(AttachmentCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<Attachment> All()
		{
			return this.AttachmentStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="AttachmentCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<Attachment> GetAttachmentPaging(AttachmentCriteria AttachmentCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(AttachmentCriteria);

			ReturnPaging<Attachment> returnPaging = new ReturnPaging<Attachment>();
			Module_Page _Pages = new Module_Page();
			_Pages.PageNum = PageNum;
			_Pages.AllCount = q.Count();
			_Pages.PageSize = PageSize;
			_Pages.Compute();
			returnPaging.Module_Page = _Pages;
			returnPaging.PageListInfos = q.OrderByDescending(c => c.CreateDate).Skip(_Pages.First).Take(_Pages.Max).ToList();
			return returnPaging;
		}

		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="AttachmentCriteria"></param>
		/// <returns></returns>
		public int Count(AttachmentCriteria AttachmentCriteria)
		{
			return GetQueryable(AttachmentCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="AttachmentCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<Attachment> GetList(AttachmentCriteria AttachmentCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(AttachmentCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(AttachmentCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.AttachmentStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(AttachmentCriteria AttachmentCriteria)
		{
			return Count(AttachmentCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<Attachment> GetQueryable(AttachmentCriteria AttachmentCriteria)
        {
            return this.AttachmentStore.Find(AttachmentCriteria.Query);
        }

		#endregion

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public IList<Attachment> GetAttachments(Guid[] Ids)
        {
            return this.AttachmentStore.Find(a => Ids.Contains(a.Id)).ToList();
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="RelationId"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IList<Attachment> GetAttachments(Guid RelationId, string Type)
        {
            return this.AttachmentStore.Find(a => a.RelationId == RelationId && a.Type == Type).ToList();
        }

        /// <summary>
        /// 设置默认附件
        /// </summary>
        /// <param name="Id"></param>
        public void SetDefault(Guid Id)
        {
            Attachment Info = this.AttachmentStore.Single(Id);
            if (Info != null && !Info.Default)
            {
                Info.Default = true;
                this.AttachmentStore.Edit(Info);
                this.SaveChanage();
            }
        }

        /// <summary>
        /// 取消默认附件
        /// </summary>
        /// <param name="Ids"></param>
        public void CancelDefaults(Guid[] Ids)
        {
            IList<Attachment> Infos = this.AttachmentStore.Find(a => Ids.Contains(a.Id) && a.Default).ToList();
            if (Infos.Count > 0)
            {
                foreach (var Item in Infos)
                {
                    Item.Default = false;
                    this.AttachmentStore.Edit(Item);
                }
                this.SaveChanage();
            }
        }

        /// <summary>
        /// 取消默认附件
        /// </summary>
        /// <param name="Ids"></param>
        public void CancelDefault(Guid Id)
        {
            Attachment Info = this.AttachmentStore.Single(Id);
            if (Info != null && Info.Default)
            {
                Info.Default = false;
                this.AttachmentStore.Edit(Info);
                this.SaveChanage();
            }
        }

        /// <summary>
        /// 关联附件
        /// </summary>
        /// <param name="RelationId"></param>
        /// <param name="Ids"></param>
        public void RelationAttachments(Guid RelationId, Guid[] Ids)
        {
            if (Ids.Length > 0)
            {
                IList<Attachment> Infos = this.AttachmentStore.Find(a => Ids.Contains(a.Id) && (a.RelationId == null || a.RelationId.Value != RelationId)).ToList();
                if (Infos.Count > 0)
                {
                    foreach (var Item in Infos)
                    {
                        Item.RelationId = RelationId;
                        this.AttachmentStore.Edit(Item);
                    }
                    this.SaveChanage();
                }
            }
        }

        /// <summary>
        /// 清理孤立附件列表（未被关联RelationId的数据）
        /// </summary>
        /// <returns></returns>
        public void CleanIsolationAttachments()
        {
            Removes(this.AttachmentStore.Find(a => a.RelationId == null).Select(a => a.Id).ToArray());
        }

        /// <summary>
        /// 取消关联附件
        /// </summary>
        /// <param name="RelationId"></param>
        /// <param name="Ids"></param>
        public void CancelRelationAttachments(Guid RelationId, string Type)
        {
            IList<Attachment> Infos = this.AttachmentStore.Find(a => a.RelationId != null && a.RelationId.Value == RelationId && a.Type == Type).ToList();
            if (Infos.Count > 0)
            {
                foreach (var Item in Infos)
                {
                    Item.RelationId = null;
                    this.AttachmentStore.Edit(Item);
                }
                this.SaveChanage();
            }

        }

        /// <summary>
        /// 改变指定Id的RelationId、Type
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ChanageRelationId"></param>
        /// <param name="ChangeType"></param>
        public void Change(Guid Id, Guid ChanageRelationId, string ChangeType)
        {
            Attachment Info = this.AttachmentStore.Single(Id);
            if (Info != null)
            {
                Info.RelationId = ChanageRelationId;
                Info.Type = ChangeType;
                this.AttachmentStore.Edit(Info);
                this.SaveChanage();
            }
        }
	}
}
