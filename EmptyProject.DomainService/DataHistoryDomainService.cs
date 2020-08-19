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
	internal class DataHistoryDomainService : BaseDomainService, IDataHistoryDomainService
	{ 
		public DataHistoryDomainService(IDataHistoryStore DataHistoryStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.DataHistoryStore = DataHistoryStore;
		}
		private readonly IDataHistoryStore DataHistoryStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public DataHistory AddDataHistory(DataHistory AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.DataHistoryStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<DataHistory> AddDataHistorys(IList<DataHistory> Infos)
		{
            Infos.ForEach(t => 
            {
                this.DataHistoryStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditDataHistory(DataHistory Info)
		{
			this.DataHistoryStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public DataHistory Single(Guid Id)
		{
			return this.DataHistoryStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.DataHistoryStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.DataHistoryStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(DataHistoryCriteria DataHistoryCriteria)
		{
			this.DataHistoryStore.Remove(DataHistoryCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<DataHistory> All()
		{
			return this.DataHistoryStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="DataHistoryCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<DataHistory> GetDataHistoryPaging(DataHistoryCriteria DataHistoryCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(DataHistoryCriteria);

			ReturnPaging<DataHistory> returnPaging = new ReturnPaging<DataHistory>();
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
		/// <param name="DataHistoryCriteria"></param>
		/// <returns></returns>
		public int Count(DataHistoryCriteria DataHistoryCriteria)
		{
			return GetQueryable(DataHistoryCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="DataHistoryCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<DataHistory> GetList(DataHistoryCriteria DataHistoryCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(DataHistoryCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(DataHistoryCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.DataHistoryStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(DataHistoryCriteria DataHistoryCriteria)
		{
			return Count(DataHistoryCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<DataHistory> GetQueryable(DataHistoryCriteria DataHistoryCriteria)
        {
            return this.DataHistoryStore.Find(DataHistoryCriteria.Query);
        }

		#endregion
	}
}
