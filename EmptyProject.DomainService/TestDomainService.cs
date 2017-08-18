
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
	internal class TestDomainService : BaseDomainService, ITestDomainService
	{ 
		public TestDomainService(ITestStore TestStore,
			IUnitOfWork UnitOfWork)
			: base(UnitOfWork)
		{
			this.TestStore = TestStore;
		}
		private readonly ITestStore TestStore;

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		public Test AddTest(Test AddInfo)
		{
			if (AddInfo == null)
				throw new ArgumentNullException("AddInfo");

			AddInfo = this.TestStore.Add(AddInfo);
			this.SaveChanage();
			return AddInfo;
		}

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		public IList<Test> AddTests(IList<Test> Infos)
		{
            Infos.ForEach(t => 
            {
                this.TestStore.Add(t);
            });
			this.SaveChanage();
			return Infos;
		}

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		public void EditTest(Test Info)
		{
			this.TestStore.Edit(Info);
			this.SaveChanage();
		}

		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		public Test Single(Guid Id)
		{
			return this.TestStore.Single(Id);
		}

		/// <summary>
		/// 删除一条信息
		/// </summary>		
		public void Remove(Guid Id)
		{
			this.TestStore.Remove(Id);
			this.SaveChanage();
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(Guid[] Ids)
		{
			if (Ids.Length > 0)
			{
				this.TestStore.Remove(Ids);
				this.SaveChanage();
			}
		}

		/// <summary>
		/// 删除多条信息
		/// </summary>		
		public void Removes(TestCriteria TestCriteria)
		{
			this.TestStore.Remove(TestCriteria.Query);
			this.SaveChanage();
		}


		/// <summary>
		/// 获得所有信息
		/// </summary>		
		public IList<Test> All()
		{
			return this.TestStore.All().ToList();
		}

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="TestCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public ReturnPaging<Test> GetTestPaging(TestCriteria TestCriteria, int PageNum = 1, int PageSize = 20)
		{
			var q = GetQueryable(TestCriteria);

			ReturnPaging<Test> returnPaging = new ReturnPaging<Test>();
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
		/// <param name="TestCriteria"></param>
		/// <returns></returns>
		public int Count(TestCriteria TestCriteria)
		{
			return GetQueryable(TestCriteria).Count();
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="TestCriteria"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageNum"></param>
		/// <returns></returns>
		public IList<Test> GetList(TestCriteria TestCriteria, int PageNum = 1, int PageSize = int.MaxValue)
		{
			PageNum = PageNum == 0 ? 1 : PageNum;
            if (PageNum == 1 && PageSize == int.MaxValue)
                return GetQueryable(TestCriteria).OrderByDescending(c => c.CreateDate).ToList();
            else
				return GetQueryable(TestCriteria).OrderByDescending(c => c.CreateDate).Skip((PageNum - 1) * PageSize).Take(PageSize).ToList();
		}
		/// <summary>
		/// 检查Id是否存在
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(Guid Id)
		{
			return this.TestStore.IsExist(t => t.Id == Id);
		}

		/// <summary>
		/// 检查查询表达式是否存在记录
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public bool IsExist(TestCriteria TestCriteria)
		{
			return Count(TestCriteria) > 0;
		}

        /// <summary>
        /// 数据库查询表达式
        /// </summary>
        /// <returns></returns>
        IQueryable<Test> GetQueryable(TestCriteria TestCriteria)
        {
            return this.TestStore.Find(TestCriteria.Query);
        }

		#endregion
	}
}
