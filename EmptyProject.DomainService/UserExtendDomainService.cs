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
using Z.EntityFramework.Plus;
using TrainServices.WEB;
namespace EmptyProject.DomainService
{
	internal class UserExtendDomainService : BaseDomainService, IUserExtendDomainService
	{ 
		public UserExtendDomainService(IUserExtendStore UserExtendStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.UserExtendStore = UserExtendStore;
		}
		private readonly IUserExtendStore UserExtendStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public UserExtend AddUserExtend(UserExtend AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.UserExtendStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<UserExtend> AddUserExtends(IList<UserExtend> Infos)
		{
            Infos.ForEach(t => 
            {
                this.UserExtendStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditUserExtend(UserExtend Info)
		{
			this.UserExtendStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public UserExtend Single(Guid Id)
		{
			return this.UserExtendStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.UserExtendStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.UserExtendStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(UserExtendCriteria UserExtendCriteria)
		{
			this.UserExtendStore.Remove(UserExtendCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<UserExtend> All()
		{
			return this.UserExtendStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="UserExtendCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<UserExtend> GetUserExtendPaging(UserExtendCriteria UserExtendCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(UserExtendCriteria);

			ReturnPaging<UserExtend> returnPaging = new ReturnPaging<UserExtend>();
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
		/// <param name="UserExtendCriteria"></param>
		/// <returns></returns>
		public int Count(UserExtendCriteria UserExtendCriteria)
		{
			return GetQueryable(UserExtendCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="UserExtendCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<UserExtend> GetList(UserExtendCriteria UserExtendCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(UserExtendCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(UserExtendCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.UserExtendStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(UserExtendCriteria UserExtendCriteria)
		{
			return Count(UserExtendCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<UserExtend> GetQueryable(UserExtendCriteria UserExtendCriteria)
        {
            return this.UserExtendStore.Find(UserExtendCriteria.Query);
        }

		#endregion

        /// <summary>
        /// 获取UserExtend 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public UserExtend Single(string UserName)
        {
            IList<UserExtend> UserExtends = GetList(new UserExtendCriteria() { UserName = UserName });
            if (UserExtends.Count > 0)
                return UserExtends.First();
            else
                return null;
        }


        /// <summary>
        /// 锁定
        /// </summary>		
        public void Lock(string UserName)
        {
            Locks(new string[] { UserName });
        }

        /// <summary>
        /// 锁定
        /// </summary>		
        public void Locks(string[] UserNames)
        {
            if (UserNames.Length > 0)
            {
                GetQueryable(new UserExtendCriteria() { UserNames = UserNames }).Update(t => new UserExtend() { IsLock = true });

                this.SaveChanage();
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>		
        public void UnLock(string UserName)
        {
            UnLocks(new string[] { UserName });
        }

        /// <summary>
        /// 解锁
        /// </summary>		
        public void UnLocks(string[] UserNames)
        {
            if (UserNames.Length > 0)
            {
                GetQueryable(new UserExtendCriteria() { UserNames = UserNames }).Update(t => new UserExtend() { IsLock = false });

                this.SaveChanage();
            }
        }

        /// <summary>
        /// 检查是否有管理权限
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool HasManagePermission(string UserName, string[] CheckPermissionUserNames)
        {
            UserExtend UserExtend = this.UserExtendStore.Find(t => t.UserName == UserName).SingleOrDefault();
            if (UserExtend != null)
            {
                string[] UserRoles = new UserBAL("EmptyProject", UserExtend.UserName).GetUserRoles();
                if (UserRoles.Contains("管理员") || UserRoles.Contains("总部")) return true;

                return true;
                //return this.UserExtendStore.Find(t => CheckPermissionUserNames.Contains(t.UserName) && t.Organ_Id == UserExtend.Organ_Id)
                //    .Select(t => t.Organ_Id).Distinct().Count() == 1;
            }
            else
                return false;
        }

        /// <summary>
        /// 是否锁定
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool IsLock(string UserName)
        {
            return IsExist(new UserExtendCriteria() { UserName = UserName, IsLock = true });
        }
	}
}
