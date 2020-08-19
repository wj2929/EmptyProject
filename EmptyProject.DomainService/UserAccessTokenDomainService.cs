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
using EmptyProject.Core.Security;

namespace EmptyProject.DomainService
{
	internal class UserAccessTokenDomainService : BaseDomainService, IUserAccessTokenDomainService
	{ 
		public UserAccessTokenDomainService(IUserAccessTokenStore UserAccessTokenStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.UserAccessTokenStore = UserAccessTokenStore;
		}
		private readonly IUserAccessTokenStore UserAccessTokenStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public UserAccessToken AddUserAccessToken(UserAccessToken AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.UserAccessTokenStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<UserAccessToken> AddUserAccessTokens(IList<UserAccessToken> Infos)
		{
            Infos.ForEach(t => 
            {
                this.UserAccessTokenStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditUserAccessToken(UserAccessToken Info)
		{
			this.UserAccessTokenStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public UserAccessToken Single(Guid Id)
		{
			return this.UserAccessTokenStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.UserAccessTokenStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.UserAccessTokenStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(UserAccessTokenCriteria UserAccessTokenCriteria)
		{
			this.UserAccessTokenStore.Remove(UserAccessTokenCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<UserAccessToken> All()
		{
			return this.UserAccessTokenStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="UserAccessTokenCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<UserAccessToken> GetUserAccessTokenPaging(UserAccessTokenCriteria UserAccessTokenCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(UserAccessTokenCriteria);

			ReturnPaging<UserAccessToken> returnPaging = new ReturnPaging<UserAccessToken>();
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
		/// <param name="UserAccessTokenCriteria"></param>
		/// <returns></returns>
		public int Count(UserAccessTokenCriteria UserAccessTokenCriteria)
		{
			return GetQueryable(UserAccessTokenCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="UserAccessTokenCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<UserAccessToken> GetList(UserAccessTokenCriteria UserAccessTokenCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(UserAccessTokenCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(UserAccessTokenCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.UserAccessTokenStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(UserAccessTokenCriteria UserAccessTokenCriteria)
		{
			return Count(UserAccessTokenCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<UserAccessToken> GetQueryable(UserAccessTokenCriteria UserAccessTokenCriteria)
        {
            return this.UserAccessTokenStore.Find(UserAccessTokenCriteria.Query);
        }

		#endregion

        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        public BaseReturnInfo Validation(string AccessToken)
        {
            UserAccessToken Info = this.UserAccessTokenStore.Single(u => u.AccessToken == AccessToken);
            if (Info == null)
                return BaseReturnInfo.Error("不存在该令牌");
            else
            {
                if (DateTime.Now > Info.Expires)
                    return BaseReturnInfo.Error("令牌已过期");
                else
                    return BaseReturnInfo.Success("");
            }
        }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        public BaseReturnInfo RefreshToken(string AccessToken, int AccessTokenExpiresDay)
        {
            UserAccessToken Info = this.UserAccessTokenStore.Single(u => u.AccessToken == AccessToken);
            if (Info == null)
                return BaseReturnInfo.Error("身份令牌不存在！");
            else
            {
                Info.AccessToken = SignatureHelper.SHA1(DateTime.Now.Ticks.ToString());
                Info.Expires = DateTime.Now.AddDays(AccessTokenExpiresDay);
                this.UserAccessTokenStore.Edit(Info);
                this.SaveChanage();
                return new BaseReturnInfo() { State = true, DataObject = Info };
            }
        }

        /// <summary>
        /// 保存令牌
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="AccessTokenExpiresDay"></param>
        /// <returns></returns>
        public UserAccessToken SaveAccessToken(string UserName, int AccessTokenExpiresDay)
        {
            UserAccessToken Info = this.UserAccessTokenStore.Single(u => u.UserName == UserName);
            if (Info != null)
            {
                if (DateTime.Now <= Info.Expires)
                    return Info;
                else
                {
                    Info.Expires = Info.Expires.Value.AddDays(AccessTokenExpiresDay);
                    this.UserAccessTokenStore.Edit(Info);
                    this.SaveChanage();
                    return Info;
                }
            }
            else
            {
                Info = new UserAccessToken
                {
                    AccessToken = SignatureHelper.SHA1(DateTime.Now.Ticks.ToString()),
                    Expires = DateTime.Now.AddDays(AccessTokenExpiresDay),
                    UserName = UserName
                };
                Info = this.UserAccessTokenStore.Add(Info);
                this.SaveChanage();
                return Info;
            }
        }

        /// <summary>
        /// 获取用户令牌
        /// </summary>
        /// <param name="UserNames"></param>
        /// <returns></returns>
        public IList<UserAccessToken> GetAccessTokens(string[] UserNames)
        {
            return this.UserAccessTokenStore.Find(t => UserNames.Contains(t.UserName)).ToList();
        }

        /// <summary>
        /// 通过令牌获取用户名
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        public string GetUserName(string AccessToken)
        {
            UserAccessToken Info = this.UserAccessTokenStore.Single(u => u.AccessToken == AccessToken);
            return Info == null ? string.Empty : (Info.Expires > DateTime.Now ? Info.UserName : string.Empty);
        }


        /// <summary>
        /// 通过令牌获取用户名
        /// </summary>
        /// <param name="AccessToken"></param>
        /// <returns></returns>
        public BaseReturnInfo GetUserNameWithReturnInfo(string AccessToken)
        {
            UserAccessToken Info = this.UserAccessTokenStore.Single(u => u.AccessToken == AccessToken);
            if (Info == null)
                return BaseReturnInfo.Error("身份令牌不存在！");
            else
                return Info.Expires > DateTime.Now ? BaseReturnInfo.Success(Info.UserName) : BaseReturnInfo.Error("身份令牌过期！");
        }
	}
}
