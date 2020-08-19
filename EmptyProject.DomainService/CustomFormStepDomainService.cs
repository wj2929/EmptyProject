
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
	internal class CustomFormStepDomainService : BaseDomainService, ICustomFormStepDomainService
	{ 
		public CustomFormStepDomainService(ICustomFormStepStore CustomFormStepStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.CustomFormStepStore = CustomFormStepStore;
		}
		private readonly ICustomFormStepStore CustomFormStepStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public CustomFormStep AddCustomFormStep(CustomFormStep AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.CustomFormStepStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<CustomFormStep> AddCustomFormSteps(IList<CustomFormStep> Infos)
		{
            Infos.ForEach(t => 
            {
                this.CustomFormStepStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditCustomFormStep(CustomFormStep Info)
		{
			this.CustomFormStepStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public CustomFormStep Single(Guid Id)
		{
			return this.CustomFormStepStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.CustomFormStepStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.CustomFormStepStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(CustomFormStepCriteria CustomFormStepCriteria)
		{
			this.CustomFormStepStore.Remove(CustomFormStepCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<CustomFormStep> All()
		{
			return this.CustomFormStepStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="CustomFormStepCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<CustomFormStep> GetCustomFormStepPaging(CustomFormStepCriteria CustomFormStepCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(CustomFormStepCriteria);

			ReturnPaging<CustomFormStep> returnPaging = new ReturnPaging<CustomFormStep>();
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
		/// <param name="CustomFormStepCriteria"></param>
		/// <returns></returns>
		public int Count(CustomFormStepCriteria CustomFormStepCriteria)
		{
			return GetQueryable(CustomFormStepCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="CustomFormStepCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<CustomFormStep> GetList(CustomFormStepCriteria CustomFormStepCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(CustomFormStepCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(CustomFormStepCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.CustomFormStepStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(CustomFormStepCriteria CustomFormStepCriteria)
		{
			return Count(CustomFormStepCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<CustomFormStep> GetQueryable(CustomFormStepCriteria CustomFormStepCriteria)
        {
            return this.CustomFormStepStore.Find(CustomFormStepCriteria.Query);
        }

		#endregion
	}
}
