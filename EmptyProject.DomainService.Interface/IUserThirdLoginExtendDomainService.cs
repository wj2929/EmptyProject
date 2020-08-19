
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
    public interface IUserThirdLoginExtendDomainService
    {

        #region AutoCode
        /// <summary>
        /// 添加一条信息
        /// </summary>		
        UserThirdLoginExtend AddUserThirdLoginExtend(UserThirdLoginExtend Info);

        /// <summary>
        /// 添加多条信息
        /// </summary>		
        IList<UserThirdLoginExtend> AddUserThirdLoginExtends(IList<UserThirdLoginExtend> Info);

        /// <summary>
        /// 编辑一条信息
        /// </summary>		
        void EditUserThirdLoginExtend(UserThirdLoginExtend Info);

        /// <summary>
        /// 读取一条数据，如数据不存在，返回null
        /// </summary>		
        UserThirdLoginExtend Single(Guid Id);

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
        void Removes(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria);

        /// <summary>
        /// 获得所有信息
        /// </summary>		
        IList<UserThirdLoginExtend> All();

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
        bool IsExist(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="UserThirdLoginExtendCriteria"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        ReturnPaging<UserThirdLoginExtend> GetUserThirdLoginExtendPaging(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria, int PageNum = 1, int PageSize = 20);

        /// <summary>
        /// 统计数量
        /// </summary>
        /// <param name="WasteCertificateCriteria"></param>
        /// <returns></returns>
        int Count(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="UserThirdLoginExtendCriteria"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        IList<UserThirdLoginExtend> GetList(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria, int PageNum = 1, int PageSize = int.MaxValue);
        #endregion

        /// <summary>
        /// 获取绑定用户名
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <returns></returns>
        string GetBindUserName(string ThirdLoginOpenId, string ThirdLoginKey);

        /// <summary>
        /// 绑定用户名
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <param name="BindUserName"></param>
        void BindUserName(string ThirdLoginOpenId, string ThirdLoginKey, string BindUserName);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <param name="ExtendedAttributes"></param>
        void Save(string ThirdLoginOpenId, string ThirdLoginKey, string ExtendedAttributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <returns></returns>
        UserThirdLoginExtend Single(string ThirdLoginOpenId, string ThirdLoginKey);


    }
}
