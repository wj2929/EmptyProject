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
	internal class DataStatisticsDetailDomainService : BaseDomainService, IDataStatisticsDetailDomainService
	{ 
		public DataStatisticsDetailDomainService(IDataStatisticsDetailStore DataStatisticsDetailStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.DataStatisticsDetailStore = DataStatisticsDetailStore;
		}
		private readonly IDataStatisticsDetailStore DataStatisticsDetailStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public DataStatisticsDetail AddDataStatisticsDetail(DataStatisticsDetail AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.DataStatisticsDetailStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<DataStatisticsDetail> AddDataStatisticsDetails(IList<DataStatisticsDetail> Infos)
		{
            Infos.ForEach(t => 
            {
                this.DataStatisticsDetailStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditDataStatisticsDetail(DataStatisticsDetail Info)
		{
			this.DataStatisticsDetailStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public DataStatisticsDetail Single(Guid Id)
		{
			return this.DataStatisticsDetailStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.DataStatisticsDetailStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.DataStatisticsDetailStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(DataStatisticsDetailCriteria DataStatisticsDetailCriteria)
		{
			this.DataStatisticsDetailStore.Remove(DataStatisticsDetailCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<DataStatisticsDetail> All()
		{
			return this.DataStatisticsDetailStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="DataStatisticsDetailCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<DataStatisticsDetail> GetDataStatisticsDetailPaging(DataStatisticsDetailCriteria DataStatisticsDetailCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(DataStatisticsDetailCriteria);

			ReturnPaging<DataStatisticsDetail> returnPaging = new ReturnPaging<DataStatisticsDetail>();
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
		/// <param name="DataStatisticsDetailCriteria"></param>
		/// <returns></returns>
		public int Count(DataStatisticsDetailCriteria DataStatisticsDetailCriteria)
		{
			return GetQueryable(DataStatisticsDetailCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="DataStatisticsDetailCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<DataStatisticsDetail> GetList(DataStatisticsDetailCriteria DataStatisticsDetailCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(DataStatisticsDetailCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(DataStatisticsDetailCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.DataStatisticsDetailStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(DataStatisticsDetailCriteria DataStatisticsDetailCriteria)
		{
			return Count(DataStatisticsDetailCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<DataStatisticsDetail> GetQueryable(DataStatisticsDetailCriteria DataStatisticsDetailCriteria)
        {
            return this.DataStatisticsDetailStore.Find(DataStatisticsDetailCriteria.Query);
        }

		#endregion
	}
}
