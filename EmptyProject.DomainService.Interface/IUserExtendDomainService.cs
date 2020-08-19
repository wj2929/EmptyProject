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
	public interface IUserExtendDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		UserExtend AddUserExtend(UserExtend Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<UserExtend> AddUserExtends(IList<UserExtend> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditUserExtend(UserExtend Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		UserExtend Single(Guid Id);

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
		void Removes(UserExtendCriteria UserExtendCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<UserExtend> All();

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
		bool IsExist(UserExtendCriteria UserExtendCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="UserExtendCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<UserExtend> GetUserExtendPaging(UserExtendCriteria UserExtendCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(UserExtendCriteria UserExtendCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="UserExtendCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<UserExtend> GetList(UserExtendCriteria UserExtendCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

        /// <summary>
        /// 获取UserExtend 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        UserExtend Single(string UserName);

        /// <summary>
        /// 锁定
        /// </summary>		
        void Lock(string UserName);

        /// <summary>
        /// 锁定
        /// </summary>		
        void Locks(string[] UserNames);

        /// <summary>
        /// 解锁
        /// </summary>		
        void UnLock(string UserName);

        /// <summary>
        /// 解锁
        /// </summary>		
        void UnLocks(string[] UserNames);

        /// <summary>
        /// 检查是否有管理权限
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Ids"></param>
        /// <returns></returns>
        bool HasManagePermission(string UserName, string[] CheckPermissionUserNames);

        /// <summary>
        /// 是否锁定
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        bool IsLock(string UserName);
	}
}
