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
	internal class CategoryTypeDomainService : BaseDomainService, ICategoryTypeDomainService
	{ 
		public CategoryTypeDomainService(ICategoryTypeStore CategoryTypeStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.CategoryTypeStore = CategoryTypeStore;
		}
		private readonly ICategoryTypeStore CategoryTypeStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public CategoryType AddCategoryType(CategoryType AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.CategoryTypeStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<CategoryType> AddCategoryTypes(IList<CategoryType> Infos)
		{
            Infos.ForEach(t => 
            {
                this.CategoryTypeStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditCategoryType(CategoryType Info)
		{
			this.CategoryTypeStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public CategoryType Single(Guid Id)
		{
			return this.CategoryTypeStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.CategoryTypeStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.CategoryTypeStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(CategoryTypeCriteria CategoryTypeCriteria)
		{
			this.CategoryTypeStore.Remove(CategoryTypeCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<CategoryType> All()
		{
			return this.CategoryTypeStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="CategoryTypeCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<CategoryType> GetCategoryTypePaging(CategoryTypeCriteria CategoryTypeCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(CategoryTypeCriteria);

			ReturnPaging<CategoryType> returnPaging = new ReturnPaging<CategoryType>();
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
		/// <param name="CategoryTypeCriteria"></param>
		/// <returns></returns>
		public int Count(CategoryTypeCriteria CategoryTypeCriteria)
		{
			return GetQueryable(CategoryTypeCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="CategoryTypeCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<CategoryType> GetList(CategoryTypeCriteria CategoryTypeCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(CategoryTypeCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(CategoryTypeCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.CategoryTypeStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(CategoryTypeCriteria CategoryTypeCriteria)
		{
			return Count(CategoryTypeCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<CategoryType> GetQueryable(CategoryTypeCriteria CategoryTypeCriteria)
        {
            return this.CategoryTypeStore.Find(CategoryTypeCriteria.Query);
        }

		#endregion

        /// <summary>
        /// 核查Keycode存在
        /// </summary>
        /// <param name="Keycode"></param>
        /// <returns></returns>
        public bool CheckKeyExist(string Keycode)
        {
            return this.CategoryTypeStore.IsExist(c => c.Keycode == Keycode);
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="Keycode"></param>
        /// <returns></returns>
        public CategoryType Single(string Keycode)
        {
            return this.CategoryTypeStore.Single(c => c.Keycode == Keycode);
        }

        /// <summary>
        /// 设置分类类型排序
        /// </summary>
        /// <param name="SortIds"></param>
        public void SaveOrder(Guid[] SortIds)
        {
            if (SortIds.Length > 0)
            {
                IList<CategoryType> CategoryTypes = All();
                if (CategoryTypes.Where(t => SortIds.Contains(t.Id)).Count() == SortIds.Length)
                {
                    IDictionary<Guid, CategoryType> CategoryTypeDic = CategoryTypes.ToDictionary(t => t.Id, t => t);
                    int i = 0;
                    SortIds.ForEach(t =>
                    {
                        CategoryTypeDic[t].Order = i++;
                        this.CategoryTypeStore.Edit(CategoryTypeDic[t]);
                    });
                    this.SaveChanage();
                }
            }
        }

    }
}
