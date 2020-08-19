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
	public interface ILogDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		Log AddLog(Log Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<Log> AddLogs(IList<Log> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditLog(Log Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		Log Single(Guid Id);

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
		void Removes(LogCriteria LogCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<Log> All();

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
		bool IsExist(LogCriteria LogCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="LogCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<Log> GetLogPaging(LogCriteria LogCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(LogCriteria LogCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="LogCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<Log> GetList(LogCriteria LogCriteria, int PageNum = 1, int PageSize = int.MaxValue);
        
        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<Log> GetQueryable(LogCriteria LogCriteria);
		#endregion

        /// <summary>
        /// 记录用户登录系统日志
        /// </summary>
        /// <param name="UserName"></param>
        void RecordUserLoginLog(string UserName);

        /// <summary>
        /// 记录用户调用服务日志
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="ServiceKey"></param>
        /// <param name="Description"></param>
        /// <param name="ExtendedAttributes"></param>
        void RecordUserServiceAccessLog(string UserName, string ServiceKey, string Description, IList<ExtendedAttribute> ExtendedAttributes);

        /// <summary>
        /// 记录系统错误日志
        /// </summary>
        /// <param name="UserName"></param>
        void RecordErrorLoginLog(string UserName, string ErrorMessage, IList<ExtendedAttribute> ExtendedAttributes);
	}
}
