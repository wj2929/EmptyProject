
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
	internal class UserThirdLoginHistoryDomainService : BaseDomainService, IUserThirdLoginHistoryDomainService
	{ 
		public UserThirdLoginHistoryDomainService(IUserThirdLoginHistoryStore UserThirdLoginHistoryStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.UserThirdLoginHistoryStore = UserThirdLoginHistoryStore;
		}
		private readonly IUserThirdLoginHistoryStore UserThirdLoginHistoryStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public UserThirdLoginHistory AddUserThirdLoginHistory(UserThirdLoginHistory AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.UserThirdLoginHistoryStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<UserThirdLoginHistory> AddUserThirdLoginHistorys(IList<UserThirdLoginHistory> Infos)
		{
            Infos.ForEach(t => 
            {
                this.UserThirdLoginHistoryStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditUserThirdLoginHistory(UserThirdLoginHistory Info)
		{
			this.UserThirdLoginHistoryStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public UserThirdLoginHistory Single(Guid Id)
		{
			return this.UserThirdLoginHistoryStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.UserThirdLoginHistoryStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.UserThirdLoginHistoryStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(UserThirdLoginHistoryCriteria UserThirdLoginHistoryCriteria)
		{
			this.UserThirdLoginHistoryStore.Remove(UserThirdLoginHistoryCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<UserThirdLoginHistory> All()
		{
			return this.UserThirdLoginHistoryStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="UserThirdLoginHistoryCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<UserThirdLoginHistory> GetUserThirdLoginHistoryPaging(UserThirdLoginHistoryCriteria UserThirdLoginHistoryCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(UserThirdLoginHistoryCriteria);

			ReturnPaging<UserThirdLoginHistory> returnPaging = new ReturnPaging<UserThirdLoginHistory>();
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
		/// <param name="UserThirdLoginHistoryCriteria"></param>
		/// <returns></returns>
		public int Count(UserThirdLoginHistoryCriteria UserThirdLoginHistoryCriteria)
		{
			return GetQueryable(UserThirdLoginHistoryCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="UserThirdLoginHistoryCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<UserThirdLoginHistory> GetList(UserThirdLoginHistoryCriteria UserThirdLoginHistoryCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(UserThirdLoginHistoryCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(UserThirdLoginHistoryCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.UserThirdLoginHistoryStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(UserThirdLoginHistoryCriteria UserThirdLoginHistoryCriteria)
		{
			return Count(UserThirdLoginHistoryCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<UserThirdLoginHistory> GetQueryable(UserThirdLoginHistoryCriteria UserThirdLoginHistoryCriteria)
        {
            return this.UserThirdLoginHistoryStore.Find(UserThirdLoginHistoryCriteria.Query);
        }

		#endregion
	}
}
