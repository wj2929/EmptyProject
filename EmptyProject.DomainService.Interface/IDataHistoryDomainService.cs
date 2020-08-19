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
	public interface IDataHistoryDomainService
	{ 

		#region AutoCode
		/// <summary>
		/// 添加一条信息
		/// </summary>		
		DataHistory AddDataHistory(DataHistory Info);

		/// <summary>
		/// 添加多条信息
		/// </summary>		
		IList<DataHistory> AddDataHistorys(IList<DataHistory> Info);

		/// <summary>
		/// 编辑一条信息
		/// </summary>		
		void EditDataHistory(DataHistory Info);
		
		/// <summary>
		/// 读取一条数据，如数据不存在，返回null
		/// </summary>		
		DataHistory Single(Guid Id);

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
		void Removes(DataHistoryCriteria DataHistoryCriteria);

		/// <summary>
		/// 获得所有信息
		/// </summary>		
		IList<DataHistory> All();

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
		bool IsExist(DataHistoryCriteria DataHistoryCriteria);

		/// <summary>
		/// 获取分页数据
		/// </summary>
		/// <param name="DataHistoryCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		ReturnPaging<DataHistory> GetDataHistoryPaging(DataHistoryCriteria DataHistoryCriteria, int PageNum = 1, int PageSize = 20);
        
		/// <summary>
		/// 统计数量
		/// </summary>
		/// <param name="WasteCertificateCriteria"></param>
		/// <returns></returns>
		int Count(DataHistoryCriteria DataHistoryCriteria);        

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="DataHistoryCriteria"></param>
		/// <param name="PageNum"></param>
		/// <param name="PageSize"></param>
		/// <returns></returns>
		IList<DataHistory> GetList(DataHistoryCriteria DataHistoryCriteria, int PageNum = 1, int PageSize = int.MaxValue);
		#endregion

	}
}
