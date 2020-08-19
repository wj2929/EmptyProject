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
	public interface IUserAccessTokenDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		UserAccessToken AddUserAccessToken(UserAccessToken Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<UserAccessToken> AddUserAccessTokens(IList<UserAccessToken> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditUserAccessToken(UserAccessToken Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		UserAccessToken Single(Guid Id);

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
		void Removes(UserAccessTokenCriteria UserAccessTokenCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<UserAccessToken> All();

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
		bool IsExist(UserAccessTokenCriteria UserAccessTokenCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="UserAccessTokenCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<UserAccessToken> GetUserAccessTokenPaging(UserAccessTokenCriteria UserAccessTokenCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(UserAccessTokenCriteria UserAccessTokenCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="UserAccessTokenCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<UserAccessToken> GetList(UserAccessTokenCriteria UserAccessTokenCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        BaseReturnInfo Validation(string AccessToken);

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <param name="AccessTokenExpiresDay"></param>
        /// <returns></returns>
        BaseReturnInfo RefreshToken(string AccessToken, int AccessTokenExpiresDay);

        /// <summary>
        /// 保存令牌
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="AccessTokenExpiresDay"></param>
        /// <returns></returns>
        UserAccessToken SaveAccessToken(string UserName, int AccessTokenExpiresDay);

        /// <summary>
        /// 通过令牌获取用户名
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        string GetUserName(string AccessToken);

        /// <summary>
        /// 通过令牌获取用户名
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        BaseReturnInfo GetUserNameWithReturnInfo(string AccessToken);
        
        /// <summary>
        /// 获取用户令牌
        /// </summary>
        /// <param name="UserNames"></param>
        /// <returns></returns>
        IList<UserAccessToken> GetAccessTokens(string[] UserNames);

	}
}
