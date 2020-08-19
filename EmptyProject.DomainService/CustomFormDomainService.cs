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
    internal class CustomFormDomainService : BaseDomainService, ICustomFormDomainService
    {
        public CustomFormDomainService(ICustomFormStore CustomFormStore,
            IUnitOfWork UnitOfWork)
            : base(UnitOfWork)
        {
            this.CustomFormStore = CustomFormStore;
        }
        private readonly ICustomFormStore CustomFormStore;

        #region AutoCode
        /// <summary>
        /// 添加一条信息
        /// </summary>		
        public CustomForm AddCustomForm(CustomForm AddInfo)
        {
            if (AddInfo == null)
                throw new ArgumentNullException("AddInfo");

            AddInfo.Order = GetOrder();
            AddInfo = this.CustomFormStore.Add(AddInfo);
            this.SaveChanage();
            return AddInfo;
        }

        /// <summary>
        /// 添加多条信息
        /// </summary>		
        public IList<CustomForm> AddCustomForms(IList<CustomForm> Infos)
        {
            Infos.ForEach(t =>
            {
                this.CustomFormStore.Add(t);
            });
            this.SaveChanage();
            return Infos;
        }

        /// <summary>
        /// 编辑一条信息
        /// </summary>		
        public void EditCustomForm(CustomForm Info)
        {
            this.CustomFormStore.Edit(Info);
            this.SaveChanage();
        }

        /// <summary>
        /// 读取一条数据，如数据不存在，返回null
        /// </summary>		
        public CustomForm Single(Guid Id)
        {
            return this.CustomFormStore.Single(Id);
        }

        /// <summary>
        /// 删除一条信息
        /// </summary>		
        public void Remove(Guid Id)
        {
            this.CustomFormStore.Remove(Id);
            this.SaveChanage();
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>		
        public void Removes(Guid[] Ids)
        {
            if (Ids.Length > 0)
            {
                this.CustomFormStore.Remove(Ids);
                this.SaveChanage();
            }
        }

        /// <summary>
        /// 删除多条信息
        /// </summary>		
        public void Removes(CustomFormCriteria CustomFormCriteria)
        {
            this.CustomFormStore.Remove(CustomFormCriteria.Query);
            this.SaveChanage();
        }


        /// <summary>
        /// 获得所有信息
        /// </summary>		
        public IList<CustomForm> All()
        {
            return this.CustomFormStore.All().ToList();
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="CustomFormCriteria"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public ReturnPaging<CustomForm> GetCustomFormPaging(CustomFormCriteria CustomFormCriteria, int PageNum = 1, int PageSize = 20)
        {
            var q = GetQueryable(CustomFormCriteria);

            ReturnPaging<CustomForm> returnPaging = new ReturnPaging<CustomForm>();
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
        /// <param name="CustomFormCriteria"></param>
        /// <returns></returns>
        public int Count(CustomFormCriteria CustomFormCriteria)
        {
            return GetQueryable(CustomFormCriteria).Count();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="CustomFormCriteria"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        public IList<CustomForm> GetList(CustomFormCriteria CustomFormCriteria, int PageNum = 1, int PageSize = int.MaxValue)
        {
            PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(CustomFormCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
                return GetQueryable(CustomFormCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
        }
        /// <summary>
        /// 检查Id是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(Guid Id)
        {
            return this.CustomFormStore.IsExist(t => t.Id == Id);
        }

        /// <summary>
        /// 检查查询表达式是否存在记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool IsExist(CustomFormCriteria CustomFormCriteria)
        {
            return Count(CustomFormCriteria) > 0;
        }

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<CustomForm> GetQueryable(CustomFormCriteria CustomFormCriteria)
        {
            return this.CustomFormStore.Find(CustomFormCriteria.Query);
        }

        #endregion

        /// <summary>
        /// 根据Keycode获取商品类目
        /// </summary>
        /// <param name="Keycode"></param>
        /// <returns></returns>
        public CustomForm SingleByKeycode(string Keycode)
        {
            return this.CustomFormStore.Single(g => g.Key == Keycode);
        }


        /// <summary>
        /// 设置表单排序
        /// </summary>
        /// <param name="SortIds"></param>
        public void SaveOrder(Guid[] SortIds)
        {
            if (SortIds.Length > 0)
            {
                IList<CustomForm> CustomForms = All();
                if (CustomForms.Where(t => SortIds.Contains(t.Id)).Count() == SortIds.Length)
                {
                    IDictionary<Guid, CustomForm> CustomFormDic = CustomForms.ToDictionary(t => t.Id, t => t);
                    int i = 0;
                    SortIds.ForEach(t =>
                    {
                        CustomFormDic[t].Order = i++;
                        this.CustomFormStore.Edit(CustomFormDic[t]);
                    });
                    this.SaveChanage();
                }
            }
        }

        /// <summary>
        /// 获取排序值
        /// </summary>
        /// <param name="Type"></param>
        public int GetOrder()
        {
            var q = this.CustomFormStore.Find(t => 1 == 1)
                .OrderByDescending(t => t.Order)
                .Select(t => t.Order);
            return q.Count() == 0 ? 0 : q.First() + 1;
        }
    }
}
