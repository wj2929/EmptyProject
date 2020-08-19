
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
    internal class UserThirdLoginExtendDomainService : BaseDomainService, IUserThirdLoginExtendDomainService
    {
        public UserThirdLoginExtendDomainService(IUserThirdLoginExtendStore UserThirdLoginExtendStore,
            IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        {
            this.UserThirdLoginExtendStore = UserThirdLoginExtendStore;
        }
        private readonly IUserThirdLoginExtendStore UserThirdLoginExtendStore;

        #region AutoCode
        /// <summary>
        /// 添加一条信息
        /// </summary>		
        public UserThirdLoginExtend AddUserThirdLoginExtend(UserThirdLoginExtend AddInfo)
        {
            if (AddInfo == null)
                throw new ArgumentNullException("AddInfo");

            AddInfo = this.UserThirdLoginExtendStore.Add(AddInfo);
            this.SaveChanage();
            return AddInfo;
        }

        /// <summary>
        /// 添加多条信息
        /// </summary>		
        public IList<UserThirdLoginExtend> AddUserThirdLoginExtends(IList<UserThirdLoginExtend> Infos)
        {
            Infos.ForEach(t =>
            {
                this.UserThirdLoginExtendStore.Add(t);
            });
            this.SaveChanage();
            return Infos;
        }

        /// <summary>
        /// 编辑一条信息
        /// </summary>		
        public void EditUserThirdLoginExtend(UserThirdLoginExtend Info)
        {
            this.UserThirdLoginExtendStore.Edit(Info);
            this.SaveChanage();
        }

        /// <summary>
        /// 读取一条数据，如数据不存在，返回null
        /// </summary>		
        public UserThirdLoginExtend Single(Guid Id)
        {
            return this.UserThirdLoginExtendStore.Single(Id);
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>		
        public void Remove(Guid Id)
        {
            this.UserThirdLoginExtendStore.Remove(Id);
            this.SaveChanage();
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>		
        public void Removes(Guid[] Ids)
        {
            if (Ids.Length > 0)
            {
                this.UserThirdLoginExtendStore.Remove(Ids);
                this.SaveChanage();
            }
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>		
        public void Removes(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria)
        {
            this.UserThirdLoginExtendStore.Remove(UserThirdLoginExtendCriteria.Query);
            this.SaveChanage();
        }


        /// <summary>
        /// 获得所有信息
        /// </summary>		
        public IList<UserThirdLoginExtend> All()
        {
            return this.UserThirdLoginExtendStore.All().ToList();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="UserThirdLoginExtendCriteria"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public ReturnPaging<UserThirdLoginExtend> GetUserThirdLoginExtendPaging(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria, int PageNum = 1, int PageSize = 20)
        {
            var q = GetQueryable(UserThirdLoginExtendCriteria);

            ReturnPaging<UserThirdLoginExtend> returnPaging = new ReturnPaging<UserThirdLoginExtend>();
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
        /// <param name="UserThirdLoginExtendCriteria"></param>
        /// <returns></returns>
        public int Count(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria)
        {
            return GetQueryable(UserThirdLoginExtendCriteria).Count();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="UserThirdLoginExtendCriteria"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public IList<UserThirdLoginExtend> GetList(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria, int PageNum = 1, int PageSize = int.MaxValue)
        {
            PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(UserThirdLoginExtendCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
                return GetQueryable(UserThirdLoginExtendCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
        }
        /// <summary>
        /// 检查Id是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(Guid Id)
        {
            return this.UserThirdLoginExtendStore.IsExist(t => t.Id == Id);
        }

        /// <summary>
        /// 检查查询表达式是否存在记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria)
        {
            return Count(UserThirdLoginExtendCriteria) > 0;
        }

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<UserThirdLoginExtend> GetQueryable(UserThirdLoginExtendCriteria UserThirdLoginExtendCriteria)
        {
            return this.UserThirdLoginExtendStore.Find(UserThirdLoginExtendCriteria.Query);
        }

        #endregion

        /// <summary>
        /// 获取绑定用户名
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <returns></returns>
        public string GetBindUserName(string ThirdLoginOpenId, string ThirdLoginKey)
        {
            IList<UserThirdLoginExtend> UserThirdLoginExtends = GetList(new UserThirdLoginExtendCriteria() { OpenId = ThirdLoginOpenId, Key = ThirdLoginKey });
            if (UserThirdLoginExtends.Count > 0)
                return UserThirdLoginExtends.First().BindUserName;
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <returns></returns>
        public UserThirdLoginExtend Single(string ThirdLoginOpenId, string ThirdLoginKey)
        {
            IList<UserThirdLoginExtend> UserThirdLoginExtends = GetList(new UserThirdLoginExtendCriteria() { OpenId = ThirdLoginOpenId, Key = ThirdLoginKey });
            if (UserThirdLoginExtends.Count > 0)
                return UserThirdLoginExtends.First();
            else
                return null;
        }


        /// <summary>
        /// 绑定用户名
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <param name="BindUserName"></param>
        public void BindUserName(string ThirdLoginOpenId, string ThirdLoginKey, string BindUserName)
        {
            IList<UserThirdLoginExtend> UserThirdLoginExtends = GetList(new Domain.QueryObject.UserThirdLoginExtendCriteria()
            {
                OpenId = ThirdLoginOpenId,
                Key = ThirdLoginKey
            });

            UserThirdLoginExtend UserThirdLoginExtend = null;
            if (UserThirdLoginExtends.Count > 0)
            {
                UserThirdLoginExtend = UserThirdLoginExtends.First();
                if (UserThirdLoginExtend.BindUserName != BindUserName)
                {
                    UserThirdLoginExtend.BindUserName = BindUserName;
                    EditUserThirdLoginExtend(UserThirdLoginExtend);
                }
            }
            else
            {
                AddUserThirdLoginExtend(new UserThirdLoginExtend()
                {
                    BindUserName = BindUserName,
                    OpenId = ThirdLoginOpenId,
                    Key = ThirdLoginKey
                });
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="ThirdLoginOpenId"></param>
        /// <param name="ThirdLoginKey"></param>
        /// <param name="ExtendedAttributes"></param>
        public void Save(string ThirdLoginOpenId, string ThirdLoginKey, string ExtendedAttributes)
        {
            IList<UserThirdLoginExtend> UserThirdLoginExtends = GetList(new Domain.QueryObject.UserThirdLoginExtendCriteria()
            {
                OpenId = ThirdLoginOpenId,
                Key = ThirdLoginKey
            });

            UserThirdLoginExtend UserThirdLoginExtend = null;
            if (UserThirdLoginExtends.Count > 0)
            {
                UserThirdLoginExtend = UserThirdLoginExtends.First();
                if (UserThirdLoginExtend.ExtendedAttributes != ExtendedAttributes)
                {
                    UserThirdLoginExtend.ExtendedAttributes = ExtendedAttributes;
                    EditUserThirdLoginExtend(UserThirdLoginExtend);
                }
            }
            else
            {
                AddUserThirdLoginExtend(new UserThirdLoginExtend()
                {
                    ExtendedAttributes = ExtendedAttributes,
                    OpenId = ThirdLoginOpenId,
                    Key = ThirdLoginKey
                });
            }
        }
    }
}
