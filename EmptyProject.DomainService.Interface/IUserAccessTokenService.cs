using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RouteNotice.Domain;
using System.Collections;
using BaseCommon.Core.Page;

namespace RouteNotice.DomainService.Interface
{
    public interface IUserAccessTokenService
    {
        #region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		UserAccessTokenInfo AddUserAccessToken(UserAccessTokenInfo info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<UserAccessTokenInfo> AddUserAccessTokens(IList<UserAccessTokenInfo> info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditUserAccessToken(UserAccessTokenInfo info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		UserAccessTokenInfo Single(Guid Id);

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
		void Removes(UserAccessTokenSearchCondition UserAccessTokenSearchCondition);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<UserAccessTokenInfo> All();

        /// <summary>
        /// 检查Id是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool IsExist(Guid Id);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="WasteCertificateSearchCondition"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        ReturnPaging<UserAccessTokenInfo> GetUserAccessTokenPaging(UserAccessTokenSearchCondition UserAccessTokenSearchCondition, int PageSize = 20, int PageNum = 1);
        
        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="WasteCertificateSearchCondition"></param>
        /// <returns></returns>
        int Count(UserAccessTokenSearchCondition UserAccessTokenSearchCondition);        

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="WasteCertificateSearchCondition"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        IList<UserAccessTokenInfo> GetList(UserAccessTokenSearchCondition UserAccessTokenSearchCondition, int PageSize = int.MaxValue, int PageNum = 1);
		#endregion
	}
}
