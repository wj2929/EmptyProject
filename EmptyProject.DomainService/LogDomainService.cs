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
using EmptyProject.Core.WebOnly.Request;

namespace EmptyProject.DomainService
{
	internal class LogDomainService : BaseDomainService, ILogDomainService
	{ 
		public LogDomainService(ILogStore LogStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.LogStore = LogStore;
		}
		private readonly ILogStore LogStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public Log AddLog(Log AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.LogStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<Log> AddLogs(IList<Log> Infos)
		{
            Infos.ForEach(t => 
            {
                this.LogStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditLog(Log Info)
		{
			this.LogStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public Log Single(Guid Id)
		{
			return this.LogStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.LogStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.LogStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(LogCriteria LogCriteria)
		{
			this.LogStore.Remove(LogCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<Log> All()
		{
			return this.LogStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="LogCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<Log> GetLogPaging(LogCriteria LogCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(LogCriteria);

			ReturnPaging<Log> returnPaging = new ReturnPaging<Log>();
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
		/// <param name="LogCriteria"></param>
		/// <returns></returns>
		public int Count(LogCriteria LogCriteria)
		{
			return GetQueryable(LogCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="LogCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<Log> GetList(LogCriteria LogCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(LogCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(LogCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.LogStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(LogCriteria LogCriteria)
		{
			return Count(LogCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        public IQueryable<Log> GetQueryable(LogCriteria LogCriteria)
        {
            return this.LogStore.Find(LogCriteria.Query);
        }

		#endregion


        /// <summary>
        /// 记录用户登录系统日志
        /// </summary>
        /// <param name="UserName"></param>
        public void RecordUserLoginLog(string UserName) 
        {
            this.LogStore.Add(new Log()
            {
                UserName = UserName,
                LogType = Domain.LogType.登录系统,
                ClientIp = RequestHelper.GetClientIP(),
                ClientUA = RequestHelper.GetClientUA(),
                Content = "登录系统"
            });
            this.SaveChanage();
        }

        /// <summary>
        /// 记录用户调用服务日志
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="ServiceKey"></param>
        /// <param name="Description"></param>
        /// <param name="ExtendedAttributes"></param>
        public void RecordUserServiceAccessLog(string UserName, string ServiceKey, string Description, IList<ExtendedAttribute> ExtendedAttributes)
        {
            Log LogInfo = new Log()
            {
                UserName = UserName,
                LogType = Domain.LogType.服务调用,
                ClientIp = RequestHelper.GetClientIP(),
                ClientUA = RequestHelper.GetClientUA(),
                Content = Description,
                Memo =ServiceKey
            };
            LogInfo.SetDicExtendedAttributes(ExtendedAttributes.ToDictionary(t => t.Key, t => t));
            this.LogStore.Add(LogInfo);
            this.SaveChanage();
        }

        /// <summary>
        /// 记录系统错误日志
        /// </summary>
        /// <param name="UserName"></param>
        public void RecordErrorLoginLog(string UserName, string ErrorMessage, IList<ExtendedAttribute> ExtendedAttributes)
        {
            Log LogInfo = new Log()
            {
                UserName = UserName,
                LogType = Domain.LogType.系统错误,
                ClientIp = RequestHelper.GetClientIP(),
                ClientUA = RequestHelper.GetClientUA(),
                Content = ErrorMessage,
                Memo = string.Empty
            };
            LogInfo.SetDicExtendedAttributes(ExtendedAttributes.ToDictionary(t => t.Key, t => t));
            this.LogStore.Add(LogInfo);
            this.SaveChanage();
        }
	}
}
