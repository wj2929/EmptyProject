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
	internal class DataStatisticsDomainService : BaseDomainService, IDataStatisticsDomainService
	{ 
		public DataStatisticsDomainService(IDataStatisticsStore DataStatisticsStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.DataStatisticsStore = DataStatisticsStore;
		}
		private readonly IDataStatisticsStore DataStatisticsStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public DataStatistics AddDataStatistics(DataStatistics AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.DataStatisticsStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<DataStatistics> AddDataStatisticss(IList<DataStatistics> Infos)
		{
            Infos.ForEach(t => 
            {
                this.DataStatisticsStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditDataStatistics(DataStatistics Info)
		{
			this.DataStatisticsStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public DataStatistics Single(Guid Id)
		{
			return this.DataStatisticsStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.DataStatisticsStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.DataStatisticsStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(DataStatisticsCriteria DataStatisticsCriteria)
		{
			this.DataStatisticsStore.Remove(DataStatisticsCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<DataStatistics> All()
		{
			return this.DataStatisticsStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="DataStatisticsCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<DataStatistics> GetDataStatisticsPaging(DataStatisticsCriteria DataStatisticsCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(DataStatisticsCriteria);

			ReturnPaging<DataStatistics> returnPaging = new ReturnPaging<DataStatistics>();
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
		/// <param name="DataStatisticsCriteria"></param>
		/// <returns></returns>
		public int Count(DataStatisticsCriteria DataStatisticsCriteria)
		{
			return GetQueryable(DataStatisticsCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="DataStatisticsCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<DataStatistics> GetList(DataStatisticsCriteria DataStatisticsCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(DataStatisticsCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(DataStatisticsCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.DataStatisticsStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(DataStatisticsCriteria DataStatisticsCriteria)
		{
			return Count(DataStatisticsCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<DataStatistics> GetQueryable(DataStatisticsCriteria DataStatisticsCriteria)
        {
            return this.DataStatisticsStore.Find(DataStatisticsCriteria.Query);
        }

		#endregion
	}
}
