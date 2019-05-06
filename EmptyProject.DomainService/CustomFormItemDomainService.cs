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
	internal class CustomFormItemDomainService : BaseDomainService, ICustomFormItemDomainService
	{ 
		public CustomFormItemDomainService(ICustomFormItemStore CustomFormItemStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.CustomFormItemStore = CustomFormItemStore;
		}
		private readonly ICustomFormItemStore CustomFormItemStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public CustomFormItem AddCustomFormItem(CustomFormItem AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.CustomFormItemStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<CustomFormItem> AddCustomFormItems(IList<CustomFormItem> Infos)
		{
            Infos.ForEach(t => 
            {
                this.CustomFormItemStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditCustomFormItem(CustomFormItem Info)
		{
			this.CustomFormItemStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public CustomFormItem Single(Guid Id)
		{
			return this.CustomFormItemStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.CustomFormItemStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.CustomFormItemStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(CustomFormItemCriteria CustomFormItemCriteria)
		{
			this.CustomFormItemStore.Remove(CustomFormItemCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<CustomFormItem> All()
		{
			return this.CustomFormItemStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="CustomFormItemCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<CustomFormItem> GetCustomFormItemPaging(CustomFormItemCriteria CustomFormItemCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(CustomFormItemCriteria);

			ReturnPaging<CustomFormItem> returnPaging = new ReturnPaging<CustomFormItem>();
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
		/// <param name="CustomFormItemCriteria"></param>
		/// <returns></returns>
		public int Count(CustomFormItemCriteria CustomFormItemCriteria)
		{
			return GetQueryable(CustomFormItemCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="CustomFormItemCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<CustomFormItem> GetList(CustomFormItemCriteria CustomFormItemCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(CustomFormItemCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(CustomFormItemCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.CustomFormItemStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(CustomFormItemCriteria CustomFormItemCriteria)
		{
			return Count(CustomFormItemCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<CustomFormItem> GetQueryable(CustomFormItemCriteria CustomFormItemCriteria)
        {
            return this.CustomFormItemStore.Find(CustomFormItemCriteria.Query);
        }

		#endregion

        /// <summary>
        /// 获取类目下的自定义表单列表
        /// </summary>
        /// <param name="CustomFormId"></param>
        /// <returns></returns>
        public IList<CustomFormItem> GetCustomFormItems(Guid CustomFormId)
        {
            return this.CustomFormItemStore.Find(g => g.CustomForm_Id == CustomFormId).OrderBy(g => g.CreateDate).OrderBy(g => g.Order).ToList();
        }

        /// <summary>
        /// 保存排序
        /// </summary>
        /// <param name="SortId"></param>
        /// <param name="PreviousId"></param>
        /// <returns></returns>
        public void SaveOrder(Guid CustomFormId, Guid SortId, Guid PreviousId)
        {
            bool SortStart = true;
            int PreviousOrder = 0;

            IList<CustomFormItem> Infos = this.CustomFormItemStore.Find(t => t.CustomForm_Id == CustomFormId).OrderBy(t => t.Order).ToList().ToList();

            CustomFormItem PreviousItem = Infos.SingleOrDefault(t => t.Id == PreviousId);

            CustomFormItem SortItem = Infos.SingleOrDefault(t => t.Id == SortId);

            if (PreviousItem != null)
            {
                SortStart = false;
                PreviousOrder = PreviousItem.Order;
                SortItem.Order = PreviousItem.Order + 1;
            }
            else
            {
                SortStart = true;
                PreviousOrder = 0;
                SortItem.Order = 0;
            }
            this.CustomFormItemStore.Edit(SortItem);

            foreach (var Item in Infos)
            {
                if (Item.Id == SortId)
                    continue;

                if (SortStart)
                {
                    if (Item.Order != PreviousOrder + 1)
                    {
                        Item.Order = PreviousOrder + 1;
                        this.CustomFormItemStore.Edit(Item);
                    }
                }
                PreviousOrder = Item.Id == PreviousId ? Item.Order + 1 : Item.Order;

                SortStart = SortStart ? true : Item.Id == PreviousId ? true : false;
            }

            this.SaveChanage();
        }

        /// <summary>
        /// 获取排序值
        /// </summary>
        /// <param name="Type"></param>
        public int GetOrder(Guid CustomFormId)
        {
            var q = this.CustomFormItemStore.Find(t => t.CustomForm_Id == CustomFormId)
                .OrderByDescending(t => t.Order)
                .Select(t => t.Order);
            return q.Count() == 0 ? 0 : q.First() + 1;
        }
	}
}
